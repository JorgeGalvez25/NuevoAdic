using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Adicional.Entidades;
using AdicionalWeb.Entidades;
using FirebirdSql.Data.FirebirdClient;
using Servicios.Adicional;

namespace AdicionalWeb.Persistencia
{
    public class EstacionesAdicionalPersistencia
    {
        private class Stored
        {
            public Stored()
            {

            }

            public Stored(DateTime expiring, object value)
            {
                Expired = expiring;
                Item = value;
            }

            public DateTime Expired { get; set; }

            public object Item { get; set; }
        }

        private class MyCache
        {
            private Dictionary<object, Stored> _items = new Dictionary<object, Stored>();

            public MyCache()
            {

            }

            public MyCache(double maxExpire)
            {
                MyCache.DefaultSegToExpire = maxExpire;
            }

            public static double DefaultSegToExpire = 30D;

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Add(object idx, object value, DateTime expire)
            {
                if (!_items.ContainsKey(idx))
                {
                    _items.Add(idx, new Stored(expire, value));
                }
                else
                {
                    _items[idx] = new Stored(expire, value);
                }
            }

            public object this[object idx]
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return (_items.ContainsKey(idx)) ? ((_items[idx].Expired >= DateTime.Now) ? _items[idx].Item : null) : null;
                }
                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (!_items.ContainsKey(idx))
                    {
                        _items.Add(idx, new Stored(DateTime.Now.AddSeconds(MyCache.DefaultSegToExpire), value));
                    }
                    else if (_items[idx].Expired <= DateTime.Now)
                    {
                        _items.Remove(idx);
                    }
                    else
                    {
                        _items[idx] = new Stored(DateTime.Now.AddSeconds(MyCache.DefaultSegToExpire), value);
                    }
                }
            }
        }
        // 1800 Seg. = 30 Min.
        private static MyCache caching = new MyCache(1800);

        public static Dictionary<int, Estacion> ListaEstaciones
        {
            get
            {
                Dictionary<int, Estacion> _estaciones = null;
                string idx = "Estacion";

                if (caching[idx] == null)
                {
                    _estaciones = ObtenerEstacionesDiccionario();
                    caching.Add(idx, _estaciones, DateTime.Now.AddSeconds(MyCache.DefaultSegToExpire));
                }
                else if (((Dictionary<int, Estacion>)caching[idx]).Count <= 0)
                {
                    _estaciones = ObtenerEstacionesDiccionario();
                    caching.Add(idx, _estaciones, DateTime.Now.AddSeconds(MyCache.DefaultSegToExpire));
                }

                return ((Dictionary<int, Estacion>)caching[idx]);
            }
        }

        private AdicionalWeb.Persistencia.Enlaces.Conexiones _enlace;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static bool ServiciosAdicional(string ipServicios, ref string msj)
        {
            ChannelFactory<IServiciosAdicional> cfAdicional = new ChannelFactory<IServiciosAdicional>("epAdicional");
            Servicios.Adicional.IServiciosAdicional srvAdi = cfAdicional.CreateChannel(new EndpointAddress(string.Format("net.tcp://{0}/ServiciosAdicional", ipServicios)));

            try
            {
                var licencia = srvAdi.LicenciaObtener(Adicional.Entidades.Licencia.ClaveWeb);
                if (!srvAdi.LicenciaValida(licencia, Adicional.Entidades.Licencia.Version))
                {
                    msj = "Submódulo Web no tiene licencia";
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (srvAdi != null)
                {
                    using (IClientChannel clientInstance = ((IClientChannel)srvAdi))
                    {
                        if (clientInstance.State == System.ServiceModel.CommunicationState.Faulted)
                        {
                            try { clientInstance.Abort(); }
                            catch { }
                            try { cfAdicional.Abort(); }
                            catch { }
                        }
                        else if (clientInstance.State != System.ServiceModel.CommunicationState.Closed)
                        {
                            try { clientInstance.Close(); }
                            catch { }
                            try { cfAdicional.Close(); }
                            catch { }
                        }
                    }
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static Dictionary<int, Estacion> ObtenerEstacionesDiccionario()
        {
            /* VALICACIÓN POR MEMBRESIA */
            EstacionesAdicionalPersistencia srvEstacion = new EstacionesAdicionalPersistencia();
            ListaEstacion _lst = srvEstacion.EstacionesAdicionalObtenerTodos();

            List<int> excluir = new List<int>();
            _lst.AsyncForEach(p =>
                {
                    AdicionalWeb.Entidades.Licencia licencia = null;
                    LicenciaPersistencia srvLicencia = new LicenciaPersistencia();
                    try { licencia = srvLicencia.LicenciaObtener(new FiltroLicencia() { Clave = p.Id }); }
                    catch { licencia = null; }

                    if (!(licencia == null ? false : srvLicencia.LicenciaValida(licencia)))
                    {
                        excluir.Add(p.Id);
                    }
                });

            //_lst.RemoveAll(p => excluir.Contains(p.Id));
            return _lst.OrderBy(p => p.Id).ToDictionary(x => x.Id, y => y);

            /* VALIDACION POR LICENCIAMIENTO */
            //EstacionesAdicionalPersistencia servicio = new EstacionesAdicionalPersistencia();
            //var lstEstaciones = servicio.EstacionesAdicionalObtenerTodos();
            //Dictionary<int, Estacion> _est = lstEstaciones.ToDictionary(x => x.Id, y => y);

            //string msj = string.Empty;
            //List<int> toRemove = new List<int>();
            //foreach (var item in _est.Keys)
            //{
            //    if (!ServiciosAdicional(_est[item].IpServicios, ref msj))
            //    {
            //        toRemove.Add(item);
            //    }
            //}

            //toRemove.ForEach(p => _est.Remove(p));

            //return _est;
        }

        public EstacionesAdicionalPersistencia()
        {
            this._enlace = new AdicionalWeb.Persistencia.Enlaces.Conexiones();
        }

        public Estacion EstacionesAdicionalObtener(int estacion)
        {
            Estacion resultado = null;

            this._enlace.AdicionalClienteConsulta((cmd) =>
            {
                cmd.CommandText = "SELECT ID, " +
                                        " NOMBRE, " +
                                        " IPSERVICIOS, " +
                                        " ESTADO, " +
                                        " ULTIMOMOVIMIENTO, " +
                                        " PROTECCIONES_ACTIVAS, " +
                                        " TIPODISPENSARIO " +
                                   " FROM ESTACIONES";
                using (FbDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        resultado = this.ReaderToEntidad(reader);
                    }
                }
            });

            return resultado;
        }

        public ListaEstacion EstacionesAdicionalObtenerTodos()
        {
            ListaEstacion resultado = new ListaEstacion();

            this._enlace.AdicionalClienteConsulta((cmd) =>
                {
                    cmd.CommandText = "SELECT ID, " +
                                            " NOMBRE, " +
                                            " IPSERVICIOS, " +
                                            " ESTADO, " +
                                            " ULTIMOMOVIMIENTO, " +
                                            " PROTECCIONES_ACTIVAS, " +
                                            " TIPODISPENSARIO " +
                                       " FROM ESTACIONES";
                    using (FbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(this.ReaderToEntidad(reader));
                        }
                    }
                });

            return resultado;
        }

        private Estacion ReaderToEntidad(FbDataReader reader)
        {
            Estacion entidad = new Estacion();
            {
                entidad.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                entidad.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                entidad.IpServicios = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                entidad.Estado = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                entidad.UltimoMovimiento = reader.IsDBNull(4) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : reader.GetDateTime(4);
                entidad.ProteccionesActivas = reader.IsDBNull(5) ? false : reader.GetString(5).Equals("S", StringComparison.CurrentCultureIgnoreCase);
                entidad.TipoDispensario = (MarcaDispensario)(reader.IsDBNull(6) ? 0 : reader.GetInt32(6));
            }

            return entidad;
        }
    }
}
