using Adicional.Entidades.SocketBidireccional;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Servicios;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.Adicional;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServicioFachadaSocket
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

        public byte[] EnviarPeticion(SolicitudHostWeb peticion)
        {
            string id = peticion.Sesion.NoCliente;

            try
            {
                if (_noEstacion.IsMatch(peticion.Sesion.NoCliente))
                {
                    try { AdministrarClientesModificarUltimaConexion(peticion.Sesion, new FiltroAdministrarClientes() { NoEstacion = peticion.Sesion.NoCliente }); }
                    catch { }
                }

                switch (peticion.Metodo)
                {
                    case Metodos.AdministrarClientesConsecutivo: return AdministrarClientesConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarClientesInsertar: return AdministrarClientesInsertar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarClientesModificar: return AdministrarClientesModificar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarClientesEliminar: return AdministrarClientesEliminar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarClientesObtener: return AdministrarClientesObtener(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarClientesObtenerTodosFiltro: return AdministrarClientesObtenerTodosFiltro(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarClientes>(peticion.Parametro as byte[]));

                    case Metodos.AdministrarUsuariosConsecutivo: return AdministrarUsuariosConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarUsuariosInsertar: return AdministrarUsuariosInsertar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuarios>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosModificar: return AdministrarUsuariosModificar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuarios>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosEliminar: return AdministrarUsuariosEliminar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuarios>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosObtener: return AdministrarUsuariosObtener(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuarios>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosObtenerTodosFiltro: return AdministrarUsuariosObtenerTodosFiltro(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuarios>(peticion.Parametro as byte[]));

                    case Metodos.AdministrarUsuariosClienteConsecutivo: return AdministrarUsuariosClienteConsecutivo(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteInsertar: return AdministrarUsuariosClienteInsertar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteModificar: return AdministrarUsuariosClienteModificar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteEliminar: return AdministrarUsuariosClienteEliminar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteObtener: return AdministrarUsuariosClienteObtener(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteObtenerTodosFiltro: return AdministrarUsuariosClienteObtenerTodosFiltro(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteInsertarModificar: return AdministrarUsuariosClienteInsertarModificar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<ListaAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteNuevaContrasenia: return AdministrarUsuariosClienteNuevaContrasenia(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteRestablecerContrasenia: return AdministrarUsuariosClienteRestablecerContrasenia(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteSolicitarContrasenia: return AdministrarUsuariosClienteSolicitarContrasenia(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteCambiarContrasenia: return AdministrarUsuariosClienteCambiarContrasenia(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuariosClientes>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarUsuariosClienteModificarPrivilegios: return AdministrarUsuariosClienteModificarPrivilegios(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarUsuariosClientes>(peticion.Parametro as byte[]));

                    case Metodos.AdministrarDistribuidoresConsecutivo: return AdministrarDistribuidoresConsecutivo(peticion.Sesion);
                    case Metodos.AdministrarDistribuidoresInsertar: return AdministrarDistribuidoresInsertar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarDistribuidores>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarDistribuidoresModificar: return AdministrarDistribuidoresModificar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<AdministrarDistribuidores>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarDistribuidoresEliminar: return AdministrarDistribuidoresEliminar(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarDistribuidores>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarDistribuidoresObtener: return AdministrarDistribuidoresObtener(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarDistribuidores>(peticion.Parametro as byte[]));
                    case Metodos.AdministrarDistribuidoresObtenerTodosFiltro: return AdministrarDistribuidoresObtenerTodosFiltro(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroAdministrarDistribuidores>(peticion.Parametro as byte[]));

                    case Metodos.AdicionaWeblLogin: return AdicionalWebValidarLogin(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<UsuarioWeb>(peticion.Parametro as byte[]));
                    case Metodos.AdicionalWebVerificarEstaciones: return AdicionalWebVerificarEstaciones(peticion.Sesion);
                    case Metodos.AdicionalWebCambiarFlujo: return AdicionalWebCambiarFlujo(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.Web.FiltroCambiarFlujo>(peticion.Parametro as byte[]));
                    case Metodos.AdicionalWebEstadoFlujo: return AdicionalWebEstadoFlujo(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.Web.FiltroCambiarFlujo>(peticion.Parametro as byte[]));
                    case Metodos.AdicionalWebObtenerMangueras: return AdicionalWebObtenerMangueras(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.Web.FiltroMangueras>(peticion.Parametro as byte[]));
                    case Metodos.AdicionalWebEstablecerDispensario: return AdicionalWebEstablecerDispensario(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.Web.FiltroMangueras>(peticion.Parametro as byte[]));
                    case Metodos.AdicionalWebEstablecerDispensarioGlobal: return AdicionalWebEstablecerDispensarioGlobal(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.Web.FiltroMangueras>(peticion.Parametro as byte[]));
                    case Metodos.ObtenerReporteVentasCombustible: return AdicionalWebObtenerReporteVentasCombustible(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<Adicional.Entidades.FiltroReporteVentasCombustible>(peticion.Parametro as byte[]));

                    case Metodos.Ping: return Ping();
                    case Metodos.PingSesion: return Ping(peticion.Sesion);

                    case Metodos.SesionObtenerTodosFiltro: return SesionObtenerTodosFiltro(peticion.Sesion, SerializadorModuloWeb.DeserializarXML<FiltroSesionModuloWeb>(peticion.Parametro as byte[]));

                    case Metodos.ObtenerFechaHoraServidor: return ObtenerFechaHoraServidor();

                    case Metodos.ServicioPings: return this.IniciarServicioPing();

                    case Metodos.None:
                    default: return SerializadorModuloWeb.SerializarXML(new RespuestaHostWeb() { EsValido = false, Mensaje = "No se puede acceder al método solicitado." });
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

        public byte[] AdministrarClientesConsecutivo(SesionModuloWeb sesion)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Consecutivo(sesion));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesConsecutivo");
                throw;
            }
        }

        public byte[] AdministrarClientesInsertar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Insertar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesInsertar");
                throw;
            }
        }

        public byte[] AdministrarClientesModificar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Modificar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificar");
                throw;
            }
        }

        public byte[] AdministrarClientesEliminar(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Eliminar(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesEliminar");
                throw;
            }
        }

        public byte[] AdministrarClientesObtener(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Obtener(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesObtener");
                throw;
            }
        }

        public byte[] AdministrarClientesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerTodosFiltro(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesObtenerTodosFiltro");
                throw;
            }
        }

        public byte[] AdministrarClientesModificarUltimaConexionBidi(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ModificarUltimaConexionBidi(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarUltimaConexionBidi");
                throw;
            }
        }

        public byte[] AdministrarClientesModificarUltimaConexion(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ModificarUltimaConexion(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarUltimaConexion");
                throw;
            }
        }

        public byte[] AdministrarClientesModificarFechaHoraCliente(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            try
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ModificarFechaHoraCliente(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarClientesModificarFechaHoraCliente");
                throw;
            }
        }

        #endregion

        #region Administrar Usuarios

        public byte[] AdministrarUsuariosConsecutivo(SesionModuloWeb sesion)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Consecutivo(sesion));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosConsecutivo");
                throw;
            }
        }

        public byte[] AdministrarUsuariosInsertar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Insertar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosInsertar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosModificar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Modificar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosModificar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Eliminar(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosEliminar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosObtener(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Obtener(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosObtener");
                throw;
            }
        }

        public byte[] AdministrarUsuariosObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            try
            {
                AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerTodosFiltro(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosObtenerTodosFiltro");
                throw;
            }
        }

        #endregion

        #region Administrar Usuarios Cliente

        public byte[] AdministrarUsuariosClienteConsecutivo(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Consecutivo(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteConsecutivo");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteInsertar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Insertar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteInsertar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteModificar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Modificar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteModificar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Eliminar(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteEliminar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteObtener(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Obtener(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteObtener");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerTodosFiltro(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteObtenerTodosFiltro");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteInsertarModificar(SesionModuloWeb sesion, ListaAdministrarUsuariosClientes lista)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.InsertarModificar(sesion, lista));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteInsertarModificar");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteNuevaContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.GenerarNuevaContrasenia(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteNuevaContrasenia");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteCambiarContrasenia(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.CambiarContrasenia(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteCambiarContrasenia");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteRestablecerContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.RestablecerContrasenia(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteRestablecerContrasenia");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteSolicitarContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.SolicitarRestablecerContrasenia(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteSolicitarRestablecerContrasenia");
                throw;
            }
        }

        public byte[] AdministrarUsuariosClienteModificarPrivilegios(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            try
            {
                AdministrarUsuariosClientesPersistencia servicio = new AdministrarUsuariosClientesPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ModificarPrivilegios(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarUsuariosClienteModificarPrivilegios");
                throw;
            }
        }

        #endregion

        #region Administrar Distribuidores

        public byte[] AdministrarDistribuidoresConsecutivo(SesionModuloWeb sesion)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Consecutivo(sesion));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresConsecutivo");
                throw;
            }
        }

        public byte[] AdministrarDistribuidoresInsertar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Insertar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresInsertar");
                throw;
            }
        }

        public byte[] AdministrarDistribuidoresModificar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Modificar(sesion, entidad));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresModificar");
                throw;
            }
        }

        public byte[] AdministrarDistribuidoresEliminar(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Eliminar(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresEliminar");
                throw;
            }
        }

        public byte[] AdministrarDistribuidoresObtener(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.Obtener(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresObtener");
                throw;
            }
        }

        public byte[] AdministrarDistribuidoresObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            try
            {
                AdministrarDistribuidoresPersistencia servicio = new AdministrarDistribuidoresPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerTodosFiltro(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdministrarDistribuidoresObtenerTodosFiltro");
                throw;
            }
        }

        #endregion

        #region Sesion

        public byte[] SesionObtenerTodosFiltro(SesionModuloWeb sesion, FiltroSesionModuloWeb filtro)
        {
            try
            {
                SesionPersistencia servicio = new SesionPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerTodosFiltro(sesion, filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "SesionObtenerTodosFiltro");
                throw;
            }
        }

        #endregion

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

            proveedor = this.crearProveedorAcicional(estacion);

            return estacion;
        }

        private Adicional.Proveedor.Sockets.Proveedor crearProveedorAcicional(Estacion estacion)
        {
            try
            {
                ICollection<string> ids = ClientManager.Keys;

                if (ids.Contains(estacion.NoEstacion))
                {
                    try
                    {
                        var state = ClientManager.Get(estacion.NoEstacion);
                        if (state != null)// && !state.IsConnected())
                        {
                            var resp = new Adicional.Proveedor.Sockets.Proveedor(state);
                            if (resp.Ping()) return resp;
                            else
                            {
                                MensajesRegistros.Informacion("No se encuentra conectado (Eliminando...)");
                                ClientManager.Remove(estacion.NoEstacion);
                            }
                        }
                        else
                        {
                            MensajesRegistros.Informacion("No se encuentra conectado (Eliminando...).");
                            ClientManager.Remove(estacion.NoEstacion);
                        }
                    }
                    catch (Exception exSKT)
                    {
                        MensajesRegistros.Advertencia("Fallo por socket", null);
                        ClientManager.Remove(estacion.NoEstacion);
                        MensajesRegistros.Excepcion("Fallo_Socket", exSKT);
                    }
                }

                try
                {
                    return new Adicional.Proveedor.Sockets.Proveedor(estacion.IP, estacion.Puerto);
                }
                catch (Exception exWCF)
                {
                    MensajesRegistros.Advertencia("Fallo por WCF", null);
                    MensajesRegistros.Excepcion("Fallo_WCF", exWCF);
                    throw;
                }
            }
            catch (Exception exG)
            {
                MensajesRegistros.Advertencia("Fallo por general", null);
                MensajesRegistros.Excepcion("Fallo_WCF", exG);
                throw;
            }

            //StateObjectBidireccional state = ClientManager.Get(estacion.NoEstacion);

            //if (state == null)
            //{
            //    return new Adicional.Proveedor.Sockets.Proveedor(estacion.IP, estacion.Puerto, this.MaxBuffSize, 30000);
            //}
            //else if (!state.IsConnected())
            //{
            //    throw new Exception(string.Format("Estación {0} esta fuera de línea", estacion.NoEstacion));
            //}

            //return new Adicional.Proveedor.Sockets.Proveedor(state);
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
                            Adicional.Proveedor.Sockets.Proveedor proveedor = proveedor = this.crearProveedorAcicional(p); //new Adicional.Proveedor.Sockets.Proveedor(p.IP, p.Puerto);
                            p.Conexion = proveedor.Ping();

                            if (p.Conexion)
                            {
                                p.Dispensario = proveedor.GetMarcaDispensario();
                            }
                        }
                        catch
                        { p.Conexion = false; }

                        estaciones.Add(p);
                    });
            }
            return estaciones;
        }

        public byte[] AdicionalWebValidarLogin(SesionModuloWeb sesion, UsuarioWeb entidad)
        {
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
                    throw new Exception("El usuario no existe o esta inactivo.");
                }

                if (sesion.Empresa == null) sesion.Empresa = new Entidades.Base.DatosEmpresa() { Id = 1 };

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
                    throw new Exception(rspVol.Mensaje.Replace("-|", string.Empty).Trim());
                }

                if (usuario.Usuario != entidad.Usuario || usuario.Password != entidad.Password)
                {
                    throw new Exception("El usuario o contraseña inválida.");
                }

                EstacionesPersistencia srvEstacion = new EstacionesPersistencia();
                FiltroEstacion fEstacion = new FiltroEstacion() { NoEstacion = entidad.NoEstacion, Activo = (bool?)true };
                Estacion estacion = srvEstacion.Obtener(sesion, fEstacion);

                if (estacion == null)
                {
                    throw new Exception("La estación no existe o esta inactiva.");
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
                    Adicional.Proveedor.Sockets.Proveedor proveedor = proveedor = this.crearProveedorAcicional(estacion);
                    estacion.Dispensario = proveedor.GetMarcaDispensario();
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

                return SerializadorModuloWeb.SerializarXML(sesion);
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebValidarLogin");
                throw;
            }
        }

        public byte[] AdicionalWebVerificarEstaciones(SesionModuloWeb sesion)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.Matriz, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                return SerializadorModuloWeb.SerializarXML(VerificarEstaciones(new EstacionesPersistencia(), sesion, new FiltroEstacion()
                {
                    Matriz = sesion.EstacionActual.Matriz
                }));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebCambiarFlujo");
                throw;
            }
        }

        public byte[] AdicionalWebCambiarFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, filtro.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                return SerializadorModuloWeb.SerializarXML(servicio.CambiarFlujo(filtro));
                //if (!respuesta.EsValido)
                //{
                //    respuesta.Mensaje = "No hay comunicación con la estación.";
                //}
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebCambiarFlujo");
                throw;
            }
        }

        public byte[] AdicionalWebEstadoFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                return SerializadorModuloWeb.SerializarXML(servicio.EstadoDelFlujo());
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstadoFlujo");
                throw;
            }
        }

        public byte[] AdicionalWebObtenerMangueras(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                Adicional.Entidades.ListaHistorial historial = servicio.GetMangueras(filtro);

                ManguerasPersistencia srvMangueras = new ManguerasPersistencia();
                return SerializadorModuloWeb.SerializarXML(srvMangueras.ObtenerPosiciones(filtro.Estacion.TipoDispensario, historial));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebTipoDispensario");
                throw;
            }
        }

        public byte[] AdicionalWebEstablecerDispensario(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                filtro.Usuario = new Adicional.Entidades.Web.UsuarioCloud()
                {
                    NoEstacion = sesion.EstacionActual.NoEstacion,
                    Correo = sesion.Usuario.CorreoElectronico,
                    Password = sesion.Usuario.Password,
                    Usuario = sesion.Usuario.Nombre
                };

                return SerializadorModuloWeb.SerializarXML(servicio.SetPorcentaje(filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstablecerDispensario");
                throw;
            }
        }

        public byte[] AdicionalWebEstablecerDispensarioGlobal(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                Estacion estacion = ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                filtro.Usuario = new Adicional.Entidades.Web.UsuarioCloud()
                {
                    NoEstacion = sesion.EstacionActual.NoEstacion,
                    Correo = sesion.Usuario.CorreoElectronico,
                    Password = sesion.Usuario.Password,
                    Usuario = sesion.Usuario.Nombre
                };

                return SerializadorModuloWeb.SerializarXML(servicio.SetGlobal(filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstablecerDispensarioGlobal");
                throw;
            }
        }

        public byte[] AdicionalWebObtenerReporteVentasCombustible(SesionModuloWeb sesion, Adicional.Entidades.FiltroReporteVentasCombustible filtro)
        {
            try
            {
                string msj = string.Empty;
                Adicional.Proveedor.Sockets.Proveedor servicio = null;
                ObtenerProveedorAdicional(sesion, sesion.EstacionActual.NoEstacion, out msj, out servicio);

                if (servicio == null)
                {
                    throw new Exception(msj);
                }

                return SerializadorModuloWeb.SerializarXML(servicio.ObtenerReporteVentasCombustible(filtro));
            }
            catch (Exception e)
            {
                this.LogException(e, sesion.NoCliente, "AdicionalWebEstablecerDispensarioGlobal");
                throw;
            }
        }

        #endregion

        public byte[] IniciarServicioPing()
        {
            try
            {
                ServicioPingConexionPersistencia servicio = new ServicioPingConexionPersistencia();
                return SerializadorModuloWeb.SerializarXML(servicio.DoProcess());
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "Serivico PING");//"ProcesarNotificaciones");
                throw;
            }
        }

        private byte[] Ping()
        {
            return SerializadorModuloWeb.SerializarXML(true);
        }

        private byte[] Ping(SesionModuloWeb sesion)
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

            return SerializadorModuloWeb.SerializarXML(result);
        }

        private byte[] ObtenerFechaHoraServidor()
        {
            try
            {
                return SerializadorModuloWeb.SerializarXML(DateTime.Now);
            }
            catch (Exception e)
            {
                this.LogException(e, string.Empty, "ObtenerFechaHoraServidor");
                throw;
            }
        }

        private string LogException(Exception e, string noCliente, string id)
        {
            string fullMessage = MensajesRegistros.GetFullMessage(e);
            MensajesRegistros.Error(noCliente, string.Format("Host Modulo Web - ModuloWeb - {0}", id), fullMessage.Trim());
            return fullMessage.Trim();
        }
    }
}
