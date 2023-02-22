using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Servicios;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using ImagenSoft.ModuloWeb.Proveedor.Conexion;

namespace ImagenSoft.ModuloWeb.Proveedor.Publicador
{
    public class ServiciosGeneralesServiciosWeb : IModuloWebProveedor, IModuloWebPerform, IDisposable
    {
        #region Propiedades

        public Sesion Sesion;
        public ConfigCliente Configuracion { get; private set; }

        private const string TituloMensaje = "Proveedor - {0}";

        private TipoConexionUsuario _tipo;
        private ServiciosConexion serviciosConexion;

        #endregion

        #region Proxy

        private class ProxyServiciosWeb : ClientBase<IModuloWeb>
        {
            public ProxyServiciosWeb(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }

            public IModuloWeb GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        private class ProxyServiciosWebPerform : ClientBase<IModuloWebPerform>
        {
            public ProxyServiciosWebPerform(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }
            public IModuloWebPerform GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        #endregion

        public ServiciosGeneralesServiciosWeb(Sesion sesion, TipoConexionUsuario tipo)
        {
            this._tipo = tipo;
            this.inicializarCanal(this._tipo);
            this.Sesion = sesion;
        }

        private T GetFunctionMonitor<T>(Sesion sesion, SolicitudHostWeb solicitud)
        {
            IModuloWeb servicio = null;
            try
            {
                ProxyServiciosWeb _servicioMonitor = new ProxyServiciosWeb(serviciosConexion.GetMonitorBinding(), new EndpointAddress(serviciosConexion.HostMonitor.ToString()));
                try
                {
                    servicio = _servicioMonitor.GetChannel;

                    if (!this.Sesion.Compare(sesion))
                    {
                        this.Sesion = sesion;
                    }

                    if (solicitud.Sesion == null)
                    {
                        solicitud.Sesion = this.Sesion;
                    }

                    var resultado = SerializadorServiciosWeb.Deserializar<RespuestaHostWeb>(servicio.EnviarPeticionTransmisor(SerializadorServiciosWeb.Serializar(solicitud)));
                    if (!resultado.EsValido)
                    {
                        throw new Exception(resultado.Mensaje);
                    }
                    return (T)resultado.Resultado;
                }
                finally
                {
                    if (servicio != null)
                    {
                        var clientInstace = (IClientChannel)servicio;
                        if (clientInstace.State == CommunicationState.Faulted)
                        {
                            try { clientInstace.Abort(); }
                            catch { }
                        }
                        else
                        {
                            clientInstace.BeginClose((r) =>
                            {
                                try
                                {
                                    object[] items = (object[])r.AsyncState;
                                    ProxyServiciosWeb proxy = ((ProxyServiciosWeb)items[1]);
                                    using (IClientChannel cInstance = ((IClientChannel)items[0]))
                                    {
                                        cInstance.EndClose(r);
                                        switch (cInstance.State)
                                        {
                                            case CommunicationState.Closed:
                                            case CommunicationState.Closing:
                                                try { proxy.Close(); }
                                                catch { proxy.Abort(); }
                                                break;
                                            case CommunicationState.Created:
                                            case CommunicationState.Opened:
                                            case CommunicationState.Opening:
                                                try
                                                {
                                                    cInstance.Close();
                                                    proxy.Close();
                                                }
                                                catch
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                break;
                                            case CommunicationState.Faulted:
                                            default:
                                                try
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                catch { }
                                                break;
                                        }
                                    }
                                }
                                catch (Exception) { }
                            }, new object[] { clientInstace, _servicioMonitor });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MensajesRegistros.Error(string.Format(TituloMensaje, solicitud.Metodo.ToString()), e.Message);
                throw;
            }

        }
        private void GetFunctionPerform(Action<IModuloWebPerform> fn)
        {
            IModuloWebPerform servicio = null;
            try
            {
                ProxyServiciosWebPerform _servicioPerform = new ProxyServiciosWebPerform(serviciosConexion.GetPerformBinding(), new EndpointAddress(serviciosConexion.HostPerform.ToString()));
                try
                {
                    servicio = _servicioPerform.GetChannel;

                    fn(servicio);
                }
                finally
                {
                    if (servicio != null)
                    {
                        var clientInstace = (IClientChannel)servicio;
                        if (clientInstace.State == CommunicationState.Faulted)
                        {
                            try { clientInstace.Abort(); }
                            catch { }
                        }
                        else
                        {
                            clientInstace.BeginClose((r) =>
                            {
                                try
                                {
                                    object[] items = (object[])r.AsyncState;
                                    ProxyServiciosWebPerform proxy = ((ProxyServiciosWebPerform)items[1]);
                                    using (IClientChannel cInstance = ((IClientChannel)items[0]))
                                    {
                                        cInstance.EndClose(r);
                                        switch (cInstance.State)
                                        {
                                            case CommunicationState.Closed:
                                            case CommunicationState.Closing:
                                                try { proxy.Close(); }
                                                catch { proxy.Abort(); }
                                                break;
                                            case CommunicationState.Created:
                                            case CommunicationState.Opened:
                                            case CommunicationState.Opening:
                                                try
                                                {
                                                    cInstance.Close();
                                                    proxy.Close();
                                                }
                                                catch
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                break;
                                            case CommunicationState.Faulted:
                                            default:
                                                try
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                catch { }
                                                break;
                                        }
                                    }
                                }
                                catch (Exception) { }
                            }, new object[] { clientInstace, _servicioPerform });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.inicializarCanal(_tipo);
                MensajesRegistros.Error(string.Format(TituloMensaje, fn.Method.Name), e.Message);
                throw;
            }
        }

        #region IServiciosWebProveedor Members

        public MonitorTransaccion TrasaccionInsertar(Sesion sesion, MonitorTransaccion entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.TransaccionInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorTransaccion>(sesion, solicitud);
        }

        public MonitorTransaccion TransaccionModificar(Sesion sesion, MonitorTransaccion entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.TransaccionModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorTransaccion>(sesion, solicitud);
        }

        public bool TransaccionEliminar(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.TransaccionEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public MonitorTransaccion TransaccionObtener(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.TransaccionObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<MonitorTransaccion>(sesion, solicitud);
        }

        public ListaMonitorTransaccion TransaccionObtenerTodosFiltro(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.TransaccionObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaMonitorTransaccion>(sesion, solicitud);
        }

        public void Transmitiendo(Sesion sesion, MonitorTransaccion entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.Transmitiendo;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            GetFunctionMonitor<object>(sesion, solicitud);
        }

        public MonitorCambioPrecio CambioPrecioInsertar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambioPrecioInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorCambioPrecio>(sesion, solicitud);
        }

        public MonitorCambioPrecio CambioPrecioModificar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambioPrecioModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorCambioPrecio>(sesion, solicitud);
        }

        public bool CambioPrecioEliminar(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambioPrecioEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public MonitorCambioPrecio CambioPrecioObtener(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambioPrecioObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<MonitorCambioPrecio>(sesion, solicitud);
        }

        public ListaMonitorCambioPrecio CambioPrecioObtenerTodosFiltro(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambioPrecioObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaMonitorCambioPrecio>(sesion, solicitud);
        }

        public MonitorConexiones MonitorConexionesInsertar(Sesion sesion, MonitorConexiones entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorConexionesInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorConexiones>(sesion, solicitud);
        }

        public MonitorConexiones MonitorConexionesModificar(Sesion sesion, MonitorConexiones entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorConexionesModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorConexiones>(sesion, solicitud);
        }

        public bool MonitorConexionesEliminar(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorConexionesEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public MonitorConexiones MonitorConexionesObtener(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorConexionesObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<MonitorConexiones>(sesion, solicitud);
        }

        public ListaMonitorConexiones MonitorConexionesObtenerTodosFiltro(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorConexionesObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaMonitorConexiones>(sesion, solicitud);
        }

        public int PreciosGasolinasConsecutivo(Sesion sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasConsecutivo;
                solicitud.Sesion = sesion;
            }

            return GetFunctionMonitor<int>(sesion, solicitud);
        }

        public PreciosGasolinas PreciosGasolinasInsertar(Sesion sesion, PreciosGasolinas entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<PreciosGasolinas>(sesion, solicitud);
        }

        public PreciosGasolinas PreciosGasolinasModificar(Sesion sesion, PreciosGasolinas entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<PreciosGasolinas>(sesion, solicitud);
        }

        public bool PreciosGasolinasEliminar(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public PreciosGasolinas PreciosGasolinasObtener(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<PreciosGasolinas>(sesion, solicitud);
        }

        public ListaPreciosGasolinas PreciosGasolinasObtenerTodosFiltro(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.PreciosGasolinasObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaPreciosGasolinas>(sesion, solicitud);
        }

        public DateTime ObtenerFechaHoraServidor()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.ObtenerFechaHoraServidor;
            }

            return GetFunctionMonitor<DateTime>(this.Sesion, solicitud);
        }

        public DateTime ObtenerFechaHoraCentralServidor()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.ObtenerFechaHoraCentralServidor;
            }

            return GetFunctionMonitor<DateTime>(this.Sesion, solicitud);
        }

        public int AdministrarClientesConsecutivo(Sesion sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesConsecutivo;
                solicitud.Sesion = sesion;
            }

            return GetFunctionMonitor<int>(sesion, solicitud);
        }

        public AdministrarClientes AdministrarClientesInsertar(Sesion sesion, AdministrarClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarClientes>(sesion, solicitud);
        }

        public AdministrarClientes AdministrarClientesModificar(Sesion sesion, AdministrarClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarClientes>(sesion, solicitud);
        }

        public bool AdministrarClientesEliminar(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public AdministrarClientes AdministrarClientesObtener(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<AdministrarClientes>(sesion, solicitud);
        }

        public ListaAdministrarClientes AdministrarClientesObtenerTodosFiltro(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarClientesObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaAdministrarClientes>(sesion, solicitud);
        }

        public bool ModificarUltimaConexion(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.ModificarUltimaConexion;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public bool ModificarFechaHoraCliente(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.ModificarFechaHoraCliente;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public void CambiarEstatus(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.CambiarEstatus;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            GetFunctionMonitor<object>(sesion, solicitud);
        }

        public int AdministrarUsuariosConsecutivo(Sesion sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosConsecutivo;
                solicitud.Sesion = sesion;
            }

            return GetFunctionMonitor<int>(sesion, solicitud);
        }

        public AdministrarUsuarios AdministrarUsuariosInsertar(Sesion sesion, AdministrarUsuarios entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarUsuarios>(sesion, solicitud);
        }

        public AdministrarUsuarios AdministrarUsuariosModificar(Sesion sesion, AdministrarUsuarios entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarUsuarios>(sesion, solicitud);
        }

        public bool AdministrarUsuariosEliminar(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public AdministrarUsuarios AdministrarUsuariosObtener(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<AdministrarUsuarios>(sesion, solicitud);
        }

        public ListaAdministrarUsuarios AdministrarUsuariosObtenerTodosFiltro(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaAdministrarUsuarios>(sesion, solicitud);
        }

        public int AdministrarDistribuidoresConsecutivo(Sesion sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresConsecutivo;
                solicitud.Sesion = sesion;
            }

            return GetFunctionMonitor<int>(sesion, solicitud);
        }

        public AdministrarDistribuidores AdministrarDistribuidoresInsertar(Sesion sesion, AdministrarDistribuidores entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarDistribuidores>(sesion, solicitud);
        }

        public AdministrarDistribuidores AdministrarDistribuidoresModificar(Sesion sesion, AdministrarDistribuidores entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<AdministrarDistribuidores>(sesion, solicitud);
        }

        public bool AdministrarDistribuidoresEliminar(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public AdministrarDistribuidores AdministrarDistribuidoresObtener(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<AdministrarDistribuidores>(sesion, solicitud);
        }

        public ListaAdministrarDistribuidores AdministrarDistribuidoresObtenerTodosFiltro(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarDistribuidoresObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaAdministrarDistribuidores>(sesion, solicitud);
        }

        public ListaSesiones SesionObtenerTodosFiltro(Sesion sesion, FiltroSesion filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.SesionObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaSesiones>(sesion, solicitud);
        }

        public ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador.ResponseUpdater Actualizar(ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador.RequestUpdater request)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.Actualizar;
                solicitud.Sesion = this.Sesion;
                solicitud.Parametro = request;
            }

            return GetFunctionMonitor<ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador.ResponseUpdater>(this.Sesion, solicitud);
        }

        public MonitorAplicaciones MonitorAplicacionesInsertar(Sesion sesion, MonitorAplicaciones entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorAplicacionesInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorAplicaciones>(sesion, solicitud);
        }

        public MonitorAplicaciones MonitorAplicacionesModificar(Sesion sesion, MonitorAplicaciones entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorAplicacionesModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<MonitorAplicaciones>(sesion, solicitud);
        }

        public bool MonitorAplicacionesEliminar(Sesion sesion, FiltroMonitorAplicaciones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorAplicacionesEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public MonitorAplicaciones MonitorAplicacionesObtener(Sesion sesion, FiltroMonitorAplicaciones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorAplicacionesObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<MonitorAplicaciones>(sesion, solicitud);
        }

        public ListaMonitorAplicaciones MonitorAplicacionesObtenerTodosFiltro(Sesion sesion, FiltroMonitorAplicaciones filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.MonitorAplicacionesObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<ListaMonitorAplicaciones>(sesion, solicitud);
        }

        #endregion

        #region IServiciosWebProveedor Members

        public int AdministrarUsuariosClienteConsecutivo(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteConsecutivo;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<int>(sesion, solicitud);
        }

        public Entidades.Web.AdministrarUsuariosClientes AdministrarUsuariosClienteInsertar(Sesion sesion, Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteInsertar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<Entidades.Web.AdministrarUsuariosClientes>(sesion, solicitud);
        }

        public Entidades.Web.AdministrarUsuariosClientes AdministrarUsuariosClienteModificar(Sesion sesion, Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<Entidades.Web.AdministrarUsuariosClientes>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteEliminar(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteEliminar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public Entidades.Web.AdministrarUsuariosClientes AdministrarUsuariosClienteObtener(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteObtener;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<Entidades.Web.AdministrarUsuariosClientes>(sesion, solicitud);
        }

        public Entidades.Web.ListaAdministrarUsuariosClientes AdministrarUsuariosClienteObtenerTodosFiltro(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteObtenerTodosFiltro;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<Entidades.Web.ListaAdministrarUsuariosClientes>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteInsertarModificar(Sesion sesion, Entidades.Web.ListaAdministrarUsuariosClientes listado)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteInsertarModificar;
                solicitud.Sesion = sesion;
                solicitud.Parametro = listado;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteNuevaContrasenia(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteNuevaContrasenia;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteCambiarContrasenia(Sesion sesion, Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteCambiarContrasenia;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteRestablecerContrasenia(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteRestablecerContrasenia;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public bool AdministrarUsuariosClienteSolicitarContrasenia(Sesion sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteSolicitarContrasenia;
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
            }

            return GetFunctionMonitor<bool>(sesion, solicitud);
        }

        public Entidades.Web.AdministrarUsuariosClientes AdministrarUsuariosClienteModificarPrivilegios(Sesion sesion, Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.AdministrarUsuariosClienteModificarPrivilegios;
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
            }

            return GetFunctionMonitor<Entidades.Web.AdministrarUsuariosClientes>(sesion, solicitud);
        }

        #endregion

        #region IServiciosWebPerform Members

        public bool Ping()
        {
            bool result = false;
            this.GetFunctionPerform((servicio) =>
                {
                    result = servicio.Ping();
                });
            return result;
        }

        public bool Ping(Sesion sesion)
        {
            if (!this.Sesion.Compare(sesion))
            {
                this.Sesion = sesion;
            }

            this.Sesion.FechaHoraCliente = DateTime.Now;

            bool result = false;
            this.GetFunctionPerform((servicio) =>
                {
                    result = servicio.Ping(sesion);
                });
            return result;
        }

        public byte[] GetConfig(byte[] request)
        {
            byte[] result = null;
            this.GetFunctionPerform((servicio) =>
                {
                    result = servicio.GetConfig(request);
                });
            return result;
        }

        public ConfigCliente GetConfig(string noEstacion)
        {
            this.Configuracion = SerializadorServiciosWeb.Deserializar<ConfigCliente>(GetConfig(SerializadorServiciosWeb.Serializar(noEstacion)));
            return this.Configuracion;
        }

        #endregion

        #region Proceso Envio Correo

        public void ProcesarNotificaciones()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Sesion = new Sesion();
                solicitud.Metodo = Metodos.ProcesarNotificaciones;
            }

            this.GetFunctionMonitor<object>(this.Sesion, solicitud);
        }

        #endregion

        private void inicializarCanal(TipoConexionUsuario tipo)
        {
            if (serviciosConexion == null)
            {
                serviciosConexion = new ServiciosConexion(tipo);
            }
        }

        #region IDisposable Members

        ~ServiciosGeneralesServiciosWeb()
        {
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (this.Configuracion != null)
            {
                this.Configuracion = null;
            }

            if (this.Sesion != null)
            {
                this.Sesion = null;
            }

            if (this.serviciosConexion != null)
            {
                this.serviciosConexion = null;
            }
        }

        #endregion
    }
}
