using Adicional.Entidades.SocketBidireccional;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia.Servicios
{
    public class ServicioPingConexionPersistencia
    {
        private static bool InProcess = false;
        private static readonly object _lock = new object();

        public bool DoProcess()
        {
            if (InProcess) { return false; }
            InProcess = true;

            Thread _inner = new Thread(_inner_Process) { IsBackground = true };
            _inner.Start();

            return true;
        }

        private void _inner_Process()
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("Proceso Ping", "private void _inner_Process()"))
            {
                try
                {
                    EstacionesPersistencia srvEstaciones = new EstacionesPersistencia();
                    SesionModuloWeb sesion = new SesionModuloWeb()
                        {
                            Nombre = "Host Modulo Web",
                            Sistema = "MW"
                        };
                    ListaEstaciones listado = srvEstaciones.ObtenerTodosFiltro(sesion, new FiltroEstacion() { Activo = true });
                    listado.ForEach(p => p.Conexion = false);

                    IEnumerable<Estacion> odrListado = listado.OrderBy(p => p.Matriz)
                                                              .ThenBy(p => p.NoEstacion);

                    _log.LogMessage("Se procesaran {1} Estaciones{0}{2}", Environment.NewLine,
                                                                          listado.Count,
                                                                          odrListado.Select(p => " - " + p.NoEstacion)
                                                                                    .Aggregate((x, y) => string.Format("{1}{0}{2}", Environment.NewLine, x, y)));

                    ICollection<string> ids = ClientManager.Keys;
                    _log.StartTimer("Tiempo Ping...");
                    try
                    {
                        StringBuilder sb = new StringBuilder();

                        (from i in odrListado.AsParallel()
                         group i by i.Matriz into gg
                         from j in gg
                         where j.Matriz == gg.Key
                         select j).ForAll(p =>
                                    {
                                        //Parallel.ForEach(lst, p =>
                                        //    {
                                        try
                                        {
                                            //_log.LogObject("Procesando...", p);
                                            Adicional.Proveedor.Sockets.Proveedor proveedor = null;

                                            if (ids.Contains(p.NoEstacion))
                                            {
                                                try
                                                {
                                                    proveedor = new Adicional.Proveedor.Sockets.Proveedor(ClientManager.Get(p.NoEstacion));
                                                    p.Conexion = proveedor.Ping();
                                                }
                                                catch (Exception exSKT)
                                                {
                                                    lock (_lock) { sb.AppendFormat("Fail Por Socket: {0} - {1}", p.NoEstacion, MensajesRegistros.GetFullMessage(exSKT)).AppendLine(); }
                                                    p.Conexion = false;
                                                    ClientManager.Remove(p.NoEstacion);
                                                }
                                            }

                                            if (!p.Conexion)
                                            {
                                                try
                                                {
                                                    proveedor = new Adicional.Proveedor.Sockets.Proveedor(p.IP, p.Puerto);
                                                    p.Conexion = proveedor.Ping();
                                                }
                                                catch (Exception exWCF)
                                                {
                                                    lock (_lock) { sb.AppendFormat("Fail Por WCF: {0} - {1}", p.NoEstacion, MensajesRegistros.GetFullMessage(exWCF)).AppendLine(); }
                                                    p.Conexion = false;
                                                }
                                            }
                                        }
                                        catch (Exception exG)
                                        {
                                            lock (_lock) { sb.AppendFormat("Fail: {0} - {1}", p.NoEstacion, MensajesRegistros.GetFullMessage(exG)).AppendLine(); }
                                            p.Conexion = false;
                                        }
                                    });

                        if (sb.Length > 0)
                        {
                            _log.LogMessage(sb.ToString().Trim());
                        }
                    }
                    finally
                    {
                        _log.StopTimer("Tiempo Ping");
                    }
                    ListaEstaciones lst = new ListaEstaciones();
                    lst.AddRange(odrListado);
                    srvEstaciones.ModificarConexion(sesion, lst);
                }
                catch (Exception e)
                {
                    _log.LogException("Servicio Pings", e);
                }
                finally
                {
                    InProcess = false;
                }
            }
        }
    }
}
