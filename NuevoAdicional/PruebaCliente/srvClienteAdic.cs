using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using Adicional.Entidades;
using Servicios.Adicional.Sockets;
using ServiciosCliente;

namespace PruebaCliente
{
    partial class srvClienteAdic : ServiceBase
    {
        private const string usuarioSincro = "Sincronización";

        private ServiceHost hostConsola;
        private ServiceHost hostAdicional;
        private ServiciosSocket servicioSocket;
        private ServiciosAdicionalSocketBidireccional servicioSocketBidireccional;
        private Timer tmrSincronizacion;
        private int noEstacion;

        // Para el remoto
        private string ipAddres = string.Empty;
        private int puerto = 0;
        private Servicios.Adicional.ServiciosAdicional srvAdicRem;

        public srvClienteAdic()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

            //System.Threading.Thread.Sleep(15 * 1000);

            try
            {
                servicioSocket = new ServiciosSocket();
                servicioSocket.Open();

                hostConsola = new ServiceHost(typeof(ServiciosCliente.ServiciosCliente));
                hostConsola.Open();

                hostAdicional = new ServiceHost(typeof(Servicios.Adicional.ServiciosAdicional));
                hostAdicional.Open();

                servicioSocketBidireccional = new ServiciosAdicionalSocketBidireccional();
                System.Threading.Thread _servicio = new System.Threading.Thread(() =>
                    {
                        try
                        {
                            EstacionConsPersistencia srvEstacion = new EstacionConsPersistencia();
                            string numEstacion = srvEstacion.ObtenerNumeroEstacion();
                            Uri host = new Uri(string.Format("tcp://{0}", ConfigurationManager.AppSettings["HostBidireccional"] ?? "localhost:808"));

                            servicioSocketBidireccional.Connect(host.DnsSafeHost, host.Port, numEstacion);
                        }
                        catch (Exception thex)
                        {
                            LogError(thex.Message);
                        }
                    }) { IsBackground = true, Name = "Servicios_Adicional_Socket_Bidireccional" };
                _servicio.Start();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                throw;
            }

            double horasReaplica = 0D;
            try
            {
                //string valorConf = appReader.GetValue("HorasReaplica", typeof(double)).ToString(); //debe estar en horas
                string valorConf = ConfigurationManager.AppSettings["HorasReaplica"] ?? "0"; //debe estar en horas

                double.TryParse(valorConf, out horasReaplica);
            }
            catch
            {
                horasReaplica = 0D;
            }

            noEstacion = 0;
            if (horasReaplica > 0)
            {
                tmrSincronizacion = new Timer(horasReaplica * 3600000);//1000 milisegundos * 60 segundos * 60 minutos
                tmrSincronizacion.Elapsed += new ElapsedEventHandler(tmrSincronizacion_Elapsed);
                tmrSincronizacion.Enabled = true;
                tmrSincronizacion.Start();
            }

            //Conectar al remoto
            try
            {
                //string valorConf = appReader.GetValue("ipRemoto", typeof(string)).ToString();
                string valorConf = ConfigurationManager.AppSettings["ipRemoto"] ?? "127.0.0.1:9091";
                string[] parteDir = valorConf.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                srvAdicRem = new Servicios.Adicional.ServiciosAdicional();

                ipAddres = parteDir[0];
                puerto = int.Parse(parteDir[1]);
                srvAdicRem.Iniciar(ipAddres, puerto);
            }
            catch (Exception)
            {
                ipAddres = string.Empty;
                puerto = 0;
            }
        }

        protected override void OnStop()
        {
            if (servicioSocket != null)
            {
                servicioSocket.Close();
                servicioSocket = null;
            }

            hostConsola.Close();
            hostConsola = null;

            hostAdicional.Close();
            hostAdicional = null;

            srvAdicRem.Terminar();

            if (tmrSincronizacion != null)
            {
                if (tmrSincronizacion.Enabled)
                {
                    tmrSincronizacion.Enabled = true;
                    tmrSincronizacion.Stop();
                }
                tmrSincronizacion.Elapsed -= new ElapsedEventHandler(tmrSincronizacion_Elapsed);
                tmrSincronizacion.Close();
                tmrSincronizacion.Dispose();
                tmrSincronizacion = null;
            }
        }

        void tmrSincronizacion_Elapsed(object sender, ElapsedEventArgs e)
        {
            string respuesta = string.Empty;
            byte status = 0;

            tmrSincronizacion.Stop();
            try
            {
                while (true)
                {
                    Servicios.Adicional.ServiciosAdicional srvAdic = new Servicios.Adicional.ServiciosAdicional();
                    List<Adicional.Entidades.Configuracion> cfgs = srvAdic.ConfiguracionesObtener();
                    string mensaje = string.Empty;
                    ServiciosCliente.ServiciosCliente srvClient = new ServiciosCliente.ServiciosCliente();
                    foreach (Adicional.Entidades.Configuracion cfg in cfgs)
                    {
                        srvAdic.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Sincronizar Estación" });
                        status = (byte)(cfg.Estado == "Mínimo" ? 0 : 1);
                        srvAdic.ConfiguracionActualizarUltimaSincronizacion(DateTime.Now);
                        bool resSicronizar = srvClient.Sincronizar(status, out mensaje);

                        if (!string.IsNullOrEmpty(mensaje))
                        {
                            if (resSicronizar)
                                srvAdic.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "El estado del dispensario es correcto." });
                            else
                            {
                                string edoAdicional = cfg.Estado;
                                string edoDisp = cfg.Estado.Equals("Mínimo") ? "Estandar" : "Mínimo";
                                bool restablecido = true;

                                srvAdic.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Diferencia de sincronización (Adicional: " + edoAdicional + " Dispensario: " + edoDisp + ")" });
                                restablecido = subirBajar(status, cfg.Estado, cfg.Id, srvAdic, srvClient);

                                if (!restablecido)
                                {
                                    srvAdic.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "No ha sido posible restablecer el estado." });
                                }
                            }
                        }
                        else
                        {
                            srvAdic.BitacoraInsertar(new Bitacora() { Id_usuario = usuarioSincro, Suceso = "Hubo un error al sincronizar." });
                        }

                        //if (status == 1)
                        //{
                        //    srvAdic.ConfiguracionActualizarUltimaSincronizacion(DateTime.Now);
                        //    srvAdic.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Reaplicación de flujo." });
                        //    subirBajar(status, cfg.Estado, noEstacion, srvAdic, srvClient);
                        //}
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Servicios.Adicional.ServiciosAdicional srvAdic = new Servicios.Adicional.ServiciosAdicional();
                srvAdic.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Error:" + ex.Message });
            }
            finally
            {
                tmrSincronizacion.Start();
            }
        }

        private bool subirBajar(byte status, string estadoActual, int estacion,
                                Servicios.Adicional.ServiciosAdicional servicioAdicional, ServiciosCliente.ServiciosCliente pServiciosCliente)
        {
            bool resultado = false;
            EstacionConsPersistencia estacionCons = new EstacionConsPersistencia();
            try
            {
                Adicional.Entidades.ListaHistorial pListaHistorial = servicioAdicional.HistorialObtenerRecientes(estacion);

                if (pListaHistorial != null && pListaHistorial.Count > 0)
                {
                    if (status == 1)
                    {
                        #region Subir
                        string pRespuesta = pServiciosCliente.AplicarFlujo(true, false, estacionCons.ObtenerMarcaDispensario(), (from h in pListaHistorial select h).ToList<Adicional.Entidades.Historial>());
                        srvAdicRem.ApagarVisual();

                        if (pRespuesta.Length > 0)
                        {
                            servicioAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Subir flujo" });

                            resultado = true;
                        }
                        else
                        {
                            servicioAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Error al subir flujo" });
                        }
                        #endregion
                    }
                    else
                    {
                        #region Bajar
                        string pRespuesta = pServiciosCliente.AplicarFlujo(false, false, estacionCons.ObtenerMarcaDispensario(), (from h in pListaHistorial select h).ToList<Adicional.Entidades.Historial>());
                        srvAdicRem.EncenderVisual();

                        if (pRespuesta.Length > 0)
                        {
                            servicioAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Bajar flujo" });

                            resultado = true;
                        }
                        else
                        {
                            servicioAdicional.BitacoraInsertar(new Adicional.Entidades.Bitacora() { Id_usuario = usuarioSincro, Suceso = "Error al bajar flujo" });
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        private void LogError(string error)
        {
            string evtLogName = "srvHostAdicional";
            if (!System.Diagnostics.EventLog.SourceExists("srvHostAdicional"))
            {
                System.Diagnostics.EventLog.CreateEventSource(evtLogName, "HostAdicional");
            }
            System.Diagnostics.EventLog.WriteEntry(evtLogName, error);
        }
    }
}
