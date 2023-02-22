using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Servicios;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.Adicional;
using ImagenSoft.ModuloWeb.Persistencia.Servicios;
using Adicional.Entidades.Sockets.Extenciones;
using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Adicional.Entidades.SocketBidireccional;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosFachada
    {
        private int _maxTimeout = -1;
        private int _maxBuffSize = -1;

        private static Regex _noEstacion = new Regex("^E[0-9]{5}$", RegexOptions.Compiled);

        private int MaxTimeout
        {
            get
            {
                if (_maxTimeout <= 0)
                {
                    _maxTimeout = ConfigurationManager.AppSettings["maxTimeout"].ToInteger(300000);
                }

                return _maxTimeout;
            }
        }
        private int MaxBuffSize
        {
            get
            {
                if (_maxBuffSize <= 0)
                {
                    _maxBuffSize = ConfigurationManager.AppSettings["maxBuffSize"].ToInteger(1048576);
                }

                return _maxBuffSize;
            }
        }

        public RespuestaHostWeb EnviarPeticion(SolicitudHostWeb peticion)
        {
            string id = peticion.Sesion.NoCliente;

            try
            {
                //if (peticion.Sesion.NoCliente.StartsWith("E", StringComparison.CurrentCultureIgnoreCase))
                if (_noEstacion.IsMatch(peticion.Sesion.NoCliente))
                {
                    try { AdministrarClientesModificarUltimaConexion(peticion.Sesion, new FiltroAdministrarClientes() { NoEstacion = peticion.Sesion.NoCliente }); }
                    catch { }
                }

                switch (peticion.Metodo)
                {
                    //case Metodos.TransaccionInsertar: return TransaccionInsertar(peticion.Sesion, (MonitorTransaccion)peticion.Parametro);
                    //case Metodos.TransaccionModificar: return TransaccionModificar(peticion.Sesion, (MonitorTransaccion)peticion.Parametro);
                    //case Metodos.TransaccionEliminar: return TransaccionEliminar(peticion.Sesion, (FiltroMonitorTransaccion)peticion.Parametro);
                    //case Metodos.TransaccionObtener: return TransaccionObtener(peticion.Sesion, (FiltroMonitorTransaccion)peticion.Parametro);
                    //case Metodos.TransaccionObtenerTodosFiltro: return TransaccionObtenerTodosFiltro(peticion.Sesion, (FiltroMonitorTransaccion)peticion.Parametro);
                    //case Metodos.Transmitiendo: return Transmitiendo(peticion.Sesion, (MonitorTransaccion)peticion.Parametro);

                    //case Metodos.CambioPrecioInsertar: return CambioPrecioInsertar(peticion.Sesion, (MonitorCambioPrecio)peticion.Parametro);
                    //case Metodos.CambioPrecioModificar: return CambioPrecioModificar(peticion.Sesion, (MonitorCambioPrecio)peticion.Parametro);
                    //case Metodos.CambioPrecioEliminar: return CambioPrecioEliminar(peticion.Sesion, (FiltroMonitorCambioPrecio)peticion.Parametro);
                    //case Metodos.CambioPrecioObtener: return CambioPrecioObtener(peticion.Sesion, (FiltroMonitorCambioPrecio)peticion.Parametro);
                    //case Metodos.CambioPrecioObtenerTodosFiltro: return CambioPrecioObtenerTodosFiltro(peticion.Sesion, (FiltroMonitorCambioPrecio)peticion.Parametro);
                    //case Metodos.CambiarEstatus: return CambioPrecioCambiarEstatus(peticion.Sesion, (FiltroMonitorCambioPrecio)peticion.Parametro);

                    //case Metodos.PreciosGasolinasConsecutivo: return PreciosGasolinasConsecutivo(peticion.Sesion);
                    //case Metodos.PreciosGasolinasInsertar: return PreciosGasolinasInsertar(peticion.Sesion, (PreciosGasolinas)peticion.Parametro);
                    //case Metodos.PreciosGasolinasModificar: return PreciosGasolinasModificar(peticion.Sesion, (PreciosGasolinas)peticion.Parametro);
                    //case Metodos.PreciosGasolinasEliminar: return PreciosGasolinasEliminar(peticion.Sesion, (FiltroPreciosGasolinas)peticion.Parametro);
                    //case Metodos.PreciosGasolinasObtener: return PreciosGasolinasObtener(peticion.Sesion, (FiltroPreciosGasolinas)peticion.Parametro);
                    //case Metodos.PreciosGasolinasObtenerTodosFiltro: return PreciosGasolinasObtenerTodosFiltro(peticion.Sesion, (FiltroPreciosGasolinas)peticion.Parametro);

                    case Metodos.AdministrarClientesConsecutivo: return AdministrarClientesConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarClientesInsertar: return AdministrarClientesInsertar(peticion.Sesion, (AdministrarClientes)peticion.Parametro);
                    case Metodos.AdministrarClientesModificar: return AdministrarClientesModificar(peticion.Sesion, (AdministrarClientes)peticion.Parametro);
                    case Metodos.AdministrarClientesEliminar: return AdministrarClientesEliminar(peticion.Sesion, (FiltroAdministrarClientes)peticion.Parametro);
                    case Metodos.AdministrarClientesObtener: return AdministrarClientesObtener(peticion.Sesion, (FiltroAdministrarClientes)peticion.Parametro);
                    case Metodos.AdministrarClientesObtenerTodosFiltro: return AdministrarClientesObtenerTodosFiltro(peticion.Sesion, (FiltroAdministrarClientes)peticion.Parametro);

                    //case Metodos.ModificarUltimaConexion: return AdministrarClientesModificarUltimaConexion(peticion.Sesion, (FiltroAdministrarClientes)peticion.Parametro);
                    //case Metodos.ModificarFechaHoraCliente: return AdministrarClientesModificarFechaHoraCliente(peticion.Sesion, (FiltroAdministrarClientes)peticion.Parametro);

                    case Metodos.AdministrarUsuariosConsecutivo: return AdministrarUsuariosConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarUsuariosInsertar: return AdministrarUsuariosInsertar(peticion.Sesion, (AdministrarUsuarios)peticion.Parametro);
                    case Metodos.AdministrarUsuariosModificar: return AdministrarUsuariosModificar(peticion.Sesion, (AdministrarUsuarios)peticion.Parametro);
                    case Metodos.AdministrarUsuariosEliminar: return AdministrarUsuariosEliminar(peticion.Sesion, (FiltroAdministrarUsuarios)peticion.Parametro);
                    case Metodos.AdministrarUsuariosObtener: return AdministrarUsuariosObtener(peticion.Sesion, (FiltroAdministrarUsuarios)peticion.Parametro);
                    case Metodos.AdministrarUsuariosObtenerTodosFiltro: return AdministrarUsuariosObtenerTodosFiltro(peticion.Sesion, (FiltroAdministrarUsuarios)peticion.Parametro);

                    case Metodos.AdministrarUsuariosClienteConsecutivo: return AdministrarUsuariosClienteConsecutivo(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteInsertar: return AdministrarUsuariosClienteInsertar(peticion.Sesion, (AdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteModificar: return AdministrarUsuariosClienteModificar(peticion.Sesion, (AdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteEliminar: return AdministrarUsuariosClienteEliminar(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteObtener: return AdministrarUsuariosClienteObtener(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteObtenerTodosFiltro: return AdministrarUsuariosClienteObtenerTodosFiltro(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteInsertarModificar: return AdministrarUsuariosClienteInsertarModificar(peticion.Sesion, (ListaAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteNuevaContrasenia: return AdministrarUsuariosClienteNuevaContrasenia(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteRestablecerContrasenia: return AdministrarUsuariosClienteRestablecerContrasenia(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteSolicitarContrasenia: return AdministrarUsuariosClienteSolicitarContrasenia(peticion.Sesion, (FiltroAdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteCambiarContrasenia: return AdministrarUsuariosClienteCambiarContrasenia(peticion.Sesion, (AdministrarUsuariosClientes)peticion.Parametro);
                    case Metodos.AdministrarUsuariosClienteModificarPrivilegios: return AdministrarUsuariosClienteModificarPrivilegios(peticion.Sesion, (AdministrarUsuariosClientes)peticion.Parametro);

                    case Metodos.AdministrarDistribuidoresConsecutivo: return AdministrarDistribuidoresConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarDistribuidoresInsertar: return AdministrarDistribuidoresInsertar(peticion.Sesion, (AdministrarDistribuidores)peticion.Parametro);
                    case Metodos.AdministrarDistribuidoresModificar: return AdministrarDistribuidoresModificar(peticion.Sesion, (AdministrarDistribuidores)peticion.Parametro);
                    case Metodos.AdministrarDistribuidoresEliminar: return AdministrarDistribuidoresEliminar(peticion.Sesion, (FiltroAdministrarDistribuidores)peticion.Parametro);
                    case Metodos.AdministrarDistribuidoresObtener: return AdministrarDistribuidoresObtener(peticion.Sesion, (FiltroAdministrarDistribuidores)peticion.Parametro);
                    case Metodos.AdministrarDistribuidoresObtenerTodosFiltro: return AdministrarDistribuidoresObtenerTodosFiltro(peticion.Sesion, (FiltroAdministrarDistribuidores)peticion.Parametro);

                    case Metodos.SesionObtenerTodosFiltro: return SesionObtenerTodosFiltro(peticion.Sesion, (FiltroSesionModuloWeb)peticion.Parametro);

                    case Metodos.ObtenerFechaHoraServidor: return ObtenerFechaHoraServidor();
                    //case Metodos.Actualizar: return Actualizar((RequestUpdater)peticion.Parametro);

                    case Metodos.ProcesarNotificaciones: return this.ProcesarNotificaciones();

                    case Metodos.ServicioPings: return this.IniciarServicioPing();

                    //case Metodos.MonitorConexionesInsertar: return MonitorConexionesInsertar(peticion.Sesion, (MonitorConexiones)peticion.Parametro);
                    //case Metodos.MonitorConexionesModificar: return MonitorConexionesModificar(peticion.Sesion, (MonitorConexiones)peticion.Parametro); ;
                    //case Metodos.MonitorConexionesEliminar: return MonitorConexionesEliminar(peticion.Sesion, (FiltroMonitorConexiones)peticion.Parametro);
                    //case Metodos.MonitorConexionesObtener: return MonitorConexionesObtener(peticion.Sesion, (FiltroMonitorConexiones)peticion.Parametro);
                    //case Metodos.MonitorConexionesObtenerTodosFiltro: return MonitorConexionesObtenerTodosFiltro(peticion.Sesion, (FiltroMonitorConexiones)peticion.Parametro);

                    //case Metodos.MonitorAplicacionesInsertar: return MonitorAplicacionesInsertar(peticion.Sesion, (MonitorAplicaciones)peticion.Parametro);
                    //case Metodos.MonitorAplicacionesModificar: return MonitorAplicacionesModificar(peticion.Sesion, (MonitorAplicaciones)peticion.Parametro); ;
                    //case Metodos.MonitorAplicacionesEliminar: return MonitorAplicacionesEliminar(peticion.Sesion, (FiltroMonitorAplicaciones)peticion.Parametro);
                    //case Metodos.MonitorAplicacionesObtener: return MonitorAplicacionesObtener(peticion.Sesion, (FiltroMonitorAplicaciones)peticion.Parametro);
                    //case Metodos.MonitorAplicacionesObtenerTodosFiltro: return MonitorAplicacionesObtenerTodosFiltro(peticion.Sesion, (FiltroMonitorAplicaciones)peticion.Parametro);

                    case Metodos.AdicionaWeblLogin: return AdicionalWebValidarLogin(peticion.Sesion, (UsuarioWeb)peticion.Parametro);
                    case Metodos.AdicionalWebVerificarEstaciones: return AdicionalWebVerificarEstaciones(peticion.Sesion);
                    case Metodos.AdicionalWebCambiarFlujo: return AdicionalWebCambiarFlujo(peticion.Sesion, (Adicional.Entidades.Web.FiltroCambiarFlujo)peticion.Parametro);
                    case Metodos.AdicionalWebEstadoFlujo: return AdicionalWebEstadoFlujo(peticion.Sesion, (Adicional.Entidades.Web.FiltroCambiarFlujo)peticion.Parametro);
                    case Metodos.AdicionalWebObtenerMangueras: return AdicionalWebObtenerMangueras(peticion.Sesion, (Adicional.Entidades.Web.FiltroMangueras)peticion.Parametro);
                    case Metodos.AdicionalWebEstablecerDispensario: return AdicionalWebEstablecerDispensario(peticion.Sesion, (Adicional.Entidades.Web.FiltroMangueras)peticion.Parametro);
                    case Metodos.AdicionalWebEstablecerDispensarioGlobal: return AdicionalWebEstablecerDispensarioGlobal(peticion.Sesion, (Adicional.Entidades.Web.FiltroMangueras)peticion.Parametro);

                    case Metodos.Ping: return Ping();
                    case Metodos.PingSesion: return Ping(peticion.Sesion);

                    case Metodos.None:
                    default: return new RespuestaHostWeb() { EsValido = false, Mensaje = "No se puede acceder al método solicitado." };
                }
            }
            catch (BitacoraException bita)
            {
                StringBuilder sb = new StringBuilder();
                if (peticion != null)
                {
                    sb.AppendLine(string.Format("{0}: {1}", peticion.Metodo.ToString(), peticion.Sesion.NoCliente));
                }

                sb.AppendLine(string.Format("Date: {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now))
                  .AppendLine(MensajesRegistros.GetFullMessage(bita));

                MensajesRegistros.Error(id, "Host Servicios Web", string.Format(sb.ToString().Trim()));
                throw new Exception("Error Servicios Web", bita);
            }
            catch (Exception e)
            {
                this.LogException(e, id, "EnviarPeticion");
                throw;
            }
        }

        #region Administrar Clientes

        public RespuestaHostWeb AdministrarClientesConsecutivo(SesionModuloWeb sesion)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Consecutivo(sesion);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesConsecutivo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesInsertar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesModificar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesEliminar(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesObtener(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesModificarUltimaConexionBidi(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ModificarUltimaConexionBidi(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarUltimaConexionBidi");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesModificarUltimaConexion(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ModificarUltimaConexion(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarUltimaConexion");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarClientesModificarFechaHoraCliente(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ModificarFechaHoraCliente(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarFechaHoraCliente");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Administrar Usuarios

        public RespuestaHostWeb AdministrarUsuariosConsecutivo(SesionModuloWeb sesion)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Consecutivo(sesion);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosConsecutivo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosInsertar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosModificar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosObtener(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Administrar Usuarios Cliente

        public RespuestaHostWeb AdministrarUsuariosClienteConsecutivo(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Consecutivo(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteConsecutivo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteInsertar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteModificar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteObtener(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteInsertarModificar(SesionModuloWeb sesion, ListaAdministrarUsuariosClientes lista)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.InsertarModificar(sesion, lista);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteInsertarModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteNuevaContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.GenerarNuevaContrasenia(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteNuevaContrasenia");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteCambiarContrasenia(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.CambiarContrasenia(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteCambiarContrasenia");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteRestablecerContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.RestablecerContrasenia(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteRestablecerContrasenia");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteSolicitarContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.SolicitarRestablecerContrasenia(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteSolicitarRestablecerContrasenia");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdministrarUsuariosClienteModificarPrivilegios(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ModificarPrivilegios(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteModificarPrivilegios");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        #endregion

        #region Administrar Distribuidores

        public RespuestaHostWeb AdministrarDistribuidoresConsecutivo(SesionModuloWeb sesion)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Consecutivo(sesion);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresConsecutivo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarDistribuidoresInsertar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarDistribuidoresModificar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarDistribuidoresEliminar(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarDistribuidoresObtener(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb AdministrarDistribuidoresObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Monitor Cambio de Precios

        public RespuestaHostWeb CambioPrecioInsertar(SesionModuloWeb sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb CambioPrecioModificar(SesionModuloWeb sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb CambioPrecioEliminar(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb CambioPrecioObtener(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb CambioPrecioObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb CambioPrecioCambiarEstatus(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                servicio.CambiarEstatus(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "CambioPrecioCambiarEstatus");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Monitor Transacciones

        public RespuestaHostWeb TransaccionInsertar(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "TransaccionInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb TransaccionModificar(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "TransaccionModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb TransaccionEliminar(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "TransaccionEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb TransaccionObtener(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "TransaccionObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb TransaccionObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "TransaccionObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb Transmitiendo(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                servicio.Transmitiendo(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "Transmitiendo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Monitor Conexiones

        public RespuestaHostWeb MonitorConexionesInsertar(SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            MonitorConexionesPersistencia servicio = new MonitorConexionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorConexionesInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorConexionesModificar(SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            MonitorConexionesPersistencia servicio = new MonitorConexionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorConexionesModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorConexionesEliminar(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexionesPersistencia servicio = new MonitorConexionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorConexionesEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorConexionesObtener(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexionesPersistencia servicio = new MonitorConexionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorConexionesObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorConexionesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexionesPersistencia servicio = new MonitorConexionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorConexionesObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Monitor Aplicaciones

        public RespuestaHostWeb MonitorAplicacionesInsertar(SesionModuloWeb sesion, MonitorAplicaciones entidad)
        {
            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorAplicacionesInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorAplicacionesModificar(SesionModuloWeb sesion, MonitorAplicaciones entidad)
        {
            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorAplicacionesModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorAplicacionesEliminar(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorAplicacionesEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorAplicacionesObtener(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorAplicacionesObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb MonitorAplicacionesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "MonitorAplicacionesObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Precios Gasolinas

        public RespuestaHostWeb PreciosGasolinasConsecutivo(SesionModuloWeb sesion)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Consecutivo(sesion);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasConsecutivo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb PreciosGasolinasInsertar(SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Insertar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasInsertar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb PreciosGasolinasModificar(SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Modificar(sesion, entidad);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasModificar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb PreciosGasolinasEliminar(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Eliminar(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasEliminar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb PreciosGasolinasObtener(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.Obtener(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasObtener");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        public RespuestaHostWeb PreciosGasolinasObtenerTodosFiltro(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "PreciosGasolinasObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Sesion

        public RespuestaHostWeb SesionObtenerTodosFiltro(SesionModuloWeb sesion, FiltroSesionModuloWeb filtro)
        {
            SesionPersistencia servicio = new SesionPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerTodosFiltro(sesion, filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "SesionObtenerTodosFiltro");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Actualizador

        public RespuestaHostWeb Actualizar(RequestUpdater request)
        {
            ActualizadorPersistencia servicio = new ActualizadorPersistencia();
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                respuesta.Resultado = servicio.ObtenerActualizaciones(request);
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "Actualizar");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            //return SerializadorServiciosWeb.Serializar(respuesta);
            return respuesta;
        }

        #endregion

        #region Proceso Envio Correo

        RespuestaHostWeb ProcesarNotificaciones()
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            NotificacionesPersistencia servicio = new NotificacionesPersistencia();

            try
            {
                servicio.ProcesarNotificaciones();
                respuesta.Resultado = true;
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "ProcesarNotificaciones");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        #endregion

        private RespuestaHostWeb ObtenerFechaHoraServidor()
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();

            try
            {
                respuesta.Resultado = DateTime.Now;
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "ObtenerFechaHoraServidor");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        #region Adicional

        private Estacion ObtenerProveedorAdicional(SesionModuloWeb sesion, string noEstacion, out string message, out Adicional.Proveedor.Sockets.Proveedor proveedor)
        {
            message = string.Empty;
            EstacionesPersistencia srvEstacion = new EstacionesPersistencia();
            FiltroEstacion fEstacion = new FiltroEstacion()
                {
                    NoEstacion = noEstacion,
                    Matriz = sesion != null ? (sesion.EstacionActual != null ? sesion.EstacionActual.Matriz : string.Empty) : string.Empty
                };
            Estacion estacion = srvEstacion.Obtener(sesion, fEstacion);

            if (estacion == null)
            {
                message = "La estación no existe o esta inactiva.";
                proveedor = null;
                return null;
            }

            //proveedor =  new Adicional.Proveedor.Sockets.Proveedor(estacion.IP, estacion.Puerto);

            proveedor = this.crearProveedorAcicional(estacion);

            return estacion;
        }

        private Adicional.Proveedor.Sockets.Proveedor crearProveedorAcicional(Estacion estacion)
        {
            StateObjectBidireccional state = ClientManager.Get(estacion.NoEstacion);

            if (state == null)
            {
                return new Adicional.Proveedor.Sockets.Proveedor(estacion.IP, estacion.Puerto);//, this.MaxBuffSize, this.MaxTimeout);
            }
            else if (!state.IsConnected())
            {
                throw new Exception(string.Format("Estación {0} esta fuera de línea", estacion.NoEstacion));
            }

            return new Adicional.Proveedor.Sockets.Proveedor(state);
        }

        private ListaEstaciones VerificarEstaciones(EstacionesPersistencia srvEstacion, SesionModuloWeb sesion, FiltroEstacion filtro)
        {
            ListaEstaciones estaciones = new ListaEstaciones();
            ListaEstaciones aux = srvEstacion.ObtenerTodosFiltro(sesion, filtro);

            if (aux != null && aux.Count > 0)
            {
                Parallel.ForEach(aux, p =>
                {
                    try
                    {
                        if (p.Conexion)
                        {
                            Adicional.Proveedor.Sockets.Proveedor proveedor = proveedor = this.crearProveedorAcicional(p); //new Adicional.Proveedor.Sockets.Proveedor(p.IP, p.Puerto);
                            p.Conexion = proveedor.Ping();

                            if (p.Conexion)
                            {
                                p.Dispensario = proveedor.GetMarcaDispensario();
                            }
                        }
                    }
                    catch
                    { p.Conexion = false; }

                    estaciones.Add(p);
                });
            }
            return estaciones;
        }

        public RespuestaHostWeb AdicionalWebValidarLogin(SesionModuloWeb sesion, UsuarioWeb entidad)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                AdministrarUsuariosClientesPersistencia srvUsuarios = new AdministrarUsuariosClientesPersistencia();
                AdministrarUsuariosClientes usuario = srvUsuarios.Obtener(sesion, new FiltroAdministrarUsuariosClientes()
                    {
                        NoEstacion = entidad.NoEstacion,
                        Usuario = entidad.Usuario,
                        Activo = "Si"
                    });

                if (usuario == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = "El usuario no existe o esta inactivo.";
                    return respuesta;
                }

                ImagenSoft.ServiciosWeb.Entidades.Sesion sesionVol = new ImagenSoft.ServiciosWeb.Entidades.Sesion()
                {
                    Id = sesion.Clave,
                    NoCliente = entidad.NoEstacion,
                    Nombre = sesion.Nombre,
                    Clave = sesion.Clave,
                    Empresa = new Framework.Entidades.Empresa()
                    {
                        Id = sesion.Empresa.Id
                    }
                };

                ImagenSoft.ServiciosWeb.Proveedor.Publicador.ServiciosProveedorVolumetricoWeb srvVol = new ImagenSoft.ServiciosWeb.Proveedor.Publicador.ServiciosProveedorVolumetricoWeb(sesionVol, ImagenSoft.ServiciosWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);

                var rspVol = srvVol.ValidarLogin(sesionVol, entidad.NoEstacion);

                if (rspVol != null && !rspVol.EsValido)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = rspVol.Mensaje.Replace("-|", string.Empty).Trim();
                    return respuesta;
                }

                if (usuario.Usuario != entidad.Usuario || usuario.Password != entidad.Password)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = "El usuario o contraseña inválida.";
                    return respuesta;
                }

                EstacionesPersistencia srvEstacion = new EstacionesPersistencia();
                FiltroEstacion fEstacion = new FiltroEstacion() { NoEstacion = entidad.NoEstacion, Activo = (bool?)true };
                Estacion estacion = srvEstacion.Obtener(sesion, fEstacion);

                if (estacion == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = "La estación no existe o esta inactiva.";
                    return respuesta;
                }

                ListaEstaciones estaciones = new ListaEstaciones();

                if (usuario.Rol.Equals(ConstantesModuloWeb.Roles.MAESTRO, StringComparison.OrdinalIgnoreCase) || usuario.Privilegios.Permisos.VerTodasEstaciones)
                {
                    fEstacion.Matriz = estacion.Matriz;
                    fEstacion.NoEstacion = string.Empty;
                    estaciones.AddRange(VerificarEstaciones(srvEstacion, sesion, fEstacion));
                }
                else
                {
                    estaciones.Add(estacion);
                }

                if (estaciones != null && estaciones.Count > 0)
                {
                    Parallel.ForEach(estaciones, p =>
                        {
                            if (p.NoEstacion == estacion.NoEstacion)
                            {
                                estacion.Dispensario = p.Dispensario;
                            }
                        });
                }

                sesion.Estacion = estacion; // La estacion del usuario
                sesion.EstacionActual = estacion; // La estacion en la que se encuentra el usuario revisando información
                sesion.Estaciones = estaciones; // Listado del grupo de estaciones a las que pertenece el usuario

                sesion.Nombre = usuario.Nombre;
                sesion.Sistema = "Modulo Web";
                sesion.DireccionIP = string.Format("{0}:{1}", estacion.IP, estacion.Puerto);
                sesion.HoraInicial = DateTime.Now;
                sesion.Usuario.Nombre = usuario.Nombre;
                sesion.Usuario.CorreoElectronico = usuario.Correo;
                sesion.Usuario.Activo = "Si";
                sesion.Usuario.Password = usuario.Password;

                respuesta.Resultado = sesion;
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebValidarLogin");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebVerificarEstaciones(SesionModuloWeb sesion)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();

            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.Matriz, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                respuesta.Resultado = VerificarEstaciones(new EstacionesPersistencia(), sesion, new FiltroEstacion()
                    {
                        Matriz = sesion.EstacionActual.Matriz
                    });
                respuesta.EsValido = true;
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebCambiarFlujo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebCambiarFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();

            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, filtro.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                respuesta.Resultado =
                    respuesta.EsValido = servicio.CambiarFlujo(filtro);
                if (!respuesta.EsValido)
                {
                    respuesta.Mensaje = "No hay comunicación con la estación.";
                }
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebCambiarFlujo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebEstadoFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                respuesta.Resultado = servicio.EstadoDelFlujo();

            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstadoFlujo");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebObtenerMangueras(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                Adicional.Entidades.ListaHistorial historial = servicio.GetMangueras(filtro);

                ManguerasPersistencia srvMangueras = new ManguerasPersistencia();
                respuesta.Resultado = srvMangueras.ObtenerPosiciones(filtro.Estacion.TipoDispensario, historial);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebTipoDispensario");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebEstablecerDispensario(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                filtro.Usuario = new Adicional.Entidades.Web.UsuarioCloud()
                    {
                        NoEstacion = sesion.EstacionActual.NoEstacion,
                        Correo = sesion.Usuario.CorreoElectronico,
                        Password = sesion.Usuario.Password,
                        Usuario = sesion.Usuario.Nombre
                    };

                respuesta.Resultado = servicio.SetPorcentaje(filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstablecerDispensario");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            return respuesta;
        }

        public RespuestaHostWeb AdicionalWebEstablecerDispensarioGlobal(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                Estacion estacion = ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    respuesta.EsValido = false;
                    respuesta.Mensaje = msj;
                    return respuesta;
                }

                filtro.Usuario = new Adicional.Entidades.Web.UsuarioCloud()
                    {
                        NoEstacion = sesion.EstacionActual.NoEstacion,
                        Correo = sesion.Usuario.CorreoElectronico,
                        Password = sesion.Usuario.Password,
                        Usuario = sesion.Usuario.Nombre
                    };

                respuesta.Resultado = servicio.SetGlobal(filtro);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstablecerDispensarioGlobal");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }
            return respuesta;
        }

        #endregion

        private string LogException(Exception e, string noCliente, string id)
        {
            string fullMessage = MensajesRegistros.GetFullMessage(e);
            MensajesRegistros.Error(noCliente, string.Format("Host Modulo Web - ModuloWeb - {0}", id), fullMessage.Trim());
            return fullMessage.Trim();
        }

        public RespuestaHostWeb IniciarServicioPing()
        {
            RespuestaHostWeb respuesta = new RespuestaHostWeb();
            ServicioPingConexionPersistencia servicio = new ServicioPingConexionPersistencia();

            try
            {
                respuesta.Resultado = servicio.DoProcess();
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "Serivico PING");//"ProcesarNotificaciones");
                respuesta.EsValido = false;
                respuesta.Mensaje = e.Message;
            }

            return respuesta;
        }

        private RespuestaHostWeb Ping()
        {
            return new RespuestaHostWeb()
                {
                    Resultado = true
                };
        }

        private RespuestaHostWeb Ping(SesionModuloWeb sesion)
        {
            bool result = false;
            //if ("Si".Equals(ConfigurationManager.AppSettings["RegistrarPing"] ?? "No", StringComparison.OrdinalIgnoreCase))
            //{
            //    try
            //    {

            //        RemoteEndpointMessageProperty cliente = ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]);
            //        MensajesRegistros.Object("HostModuloWeb-Clientes", string.Format("IP - Log _ {0} - {1}:{2}", sesion.NoCliente, cliente.Address, cliente.Port), cliente);
            //    }
            //    catch { }
            //}

            try
            {
                ServiciosAdministrarClientesFachada servicio = new ServiciosAdministrarClientesFachada();
                servicio.ModificarFechaHoraCliente(sesion, new FiltroAdministrarClientes()
                {
                    FechaUltimaConexion = DateTime.Now,
                    NoEstacion = sesion.NoCliente,
                    FechaHoraCliente = sesion.FechaHoraCliente,
                    Version = "4.1"
                });

                result = true;
                //ServiciosPreciosGasolinerasFachada srvPrecios = new ServiciosPreciosGasolinerasFachada();
                //result = srvPrecios.ClienteValidoCambioPrecios(sesion);

                //ServiciosMonitorAplicaciones srvAplicaciones = new ServiciosMonitorAplicaciones();
                //srvAplicaciones.ModificarAplicaciones(sesion);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Ping: {0}", sesion.NoCliente))
                  .AppendLine(MensajesRegistros.GetFullMessage(e));

                MensajesRegistros.Error("Host Modulo Web", string.Format(sb.ToString().Trim()));
            }

            return new RespuestaHostWeb()
            {
                Resultado = result
            };
        }
    }
}
