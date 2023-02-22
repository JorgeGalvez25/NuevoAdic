using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador;
using ImagenSoft.ModuloWeb.Entidades.Web;
using System;
using System.ServiceModel;

namespace ImagenSoft.ModuloWeb.Interfaces.Publicador
{
    [ServiceContract(Namespace = "http://moduloweb.adicional.com")]
    public interface IModuloWeb
    {
        [OperationContract]
        byte[] EnviarPeticion(byte[] solicitud);
    }

    //[ServiceContract(Namespace = "http://moduloweb.adicional.com")]
    //public interface IModuloWebProveedor
    //{
    //    #region Servicios

    //    #region Monitor Transaccion

    //    [OperationContract]
    //    MonitorTransaccion TrasaccionInsertar(Sesion sesion, MonitorTransaccion entidad);

    //    [OperationContract]
    //    MonitorTransaccion TransaccionModificar(Sesion sesion, MonitorTransaccion entidad);

    //    [OperationContract]
    //    bool TransaccionEliminar(Sesion sesion, FiltroMonitorTransaccion filtro);

    //    [OperationContract]
    //    MonitorTransaccion TransaccionObtener(Sesion sesion, FiltroMonitorTransaccion filtro);

    //    [OperationContract]
    //    ListaMonitorTransaccion TransaccionObtenerTodosFiltro(Sesion sesion, FiltroMonitorTransaccion filtro);

    //    [OperationContract]
    //    void Transmitiendo(Sesion sesion, MonitorTransaccion entidad);

    //    #endregion

    //    #region Monitor Cambio de Precios

    //    [OperationContract]
    //    MonitorCambioPrecio CambioPrecioInsertar(Sesion sesion, MonitorCambioPrecio entidad);

    //    [OperationContract]
    //    MonitorCambioPrecio CambioPrecioModificar(Sesion sesion, MonitorCambioPrecio entidad);

    //    [OperationContract]
    //    bool CambioPrecioEliminar(Sesion sesion, FiltroMonitorCambioPrecio filtro);

    //    [OperationContract]
    //    MonitorCambioPrecio CambioPrecioObtener(Sesion sesion, FiltroMonitorCambioPrecio filtro);

    //    [OperationContract]
    //    ListaMonitorCambioPrecio CambioPrecioObtenerTodosFiltro(Sesion sesion, FiltroMonitorCambioPrecio filtro);

    //    #endregion

    //    #region Monitor Conexiones

    //    [OperationContract]
    //    MonitorConexiones MonitorConexionesInsertar(Sesion sesion, MonitorConexiones entidad);

    //    [OperationContract]
    //    MonitorConexiones MonitorConexionesModificar(Sesion sesion, MonitorConexiones entidad);

    //    [OperationContract]
    //    bool MonitorConexionesEliminar(Sesion sesion, FiltroMonitorConexiones filtro);

    //    [OperationContract]
    //    MonitorConexiones MonitorConexionesObtener(Sesion sesion, FiltroMonitorConexiones filtro);

    //    [OperationContract]
    //    ListaMonitorConexiones MonitorConexionesObtenerTodosFiltro(Sesion sesion, FiltroMonitorConexiones filtro);

    //    #endregion

    //    #region Monitor Aplicaciones

    //    [OperationContract]
    //    MonitorAplicaciones MonitorAplicacionesInsertar(Sesion sesion, MonitorAplicaciones entidad);

    //    [OperationContract]
    //    MonitorAplicaciones MonitorAplicacionesModificar(Sesion sesion, MonitorAplicaciones entidad);

    //    [OperationContract]
    //    bool MonitorAplicacionesEliminar(Sesion sesion, FiltroMonitorAplicaciones filtro);

    //    [OperationContract]
    //    MonitorAplicaciones MonitorAplicacionesObtener(Sesion sesion, FiltroMonitorAplicaciones filtro);

    //    [OperationContract]
    //    ListaMonitorAplicaciones MonitorAplicacionesObtenerTodosFiltro(Sesion sesion, FiltroMonitorAplicaciones filtro);

    //    #endregion

    //    #region Precio de Gasolinas

    //    [OperationContract]
    //    int PreciosGasolinasConsecutivo(Sesion sesion);

    //    [OperationContract]
    //    PreciosGasolinas PreciosGasolinasInsertar(Sesion sesion, PreciosGasolinas entidad);

    //    [OperationContract]
    //    PreciosGasolinas PreciosGasolinasModificar(Sesion sesion, PreciosGasolinas entidad);

    //    [OperationContract]
    //    bool PreciosGasolinasEliminar(Sesion sesion, FiltroPreciosGasolinas filtro);

    //    [OperationContract]
    //    PreciosGasolinas PreciosGasolinasObtener(Sesion sesion, FiltroPreciosGasolinas filtro);

    //    [OperationContract]
    //    ListaPreciosGasolinas PreciosGasolinasObtenerTodosFiltro(Sesion sesion, FiltroPreciosGasolinas filtro);

    //    #endregion

    //    [OperationContract]
    //    DateTime ObtenerFechaHoraServidor();

    //    [OperationContract]
    //    DateTime ObtenerFechaHoraCentralServidor();

    //    #endregion

    //    #region Catalogos

    //    #region Administrar Clientes

    //    [OperationContract]
    //    int AdministrarClientesConsecutivo(Sesion sesion);

    //    [OperationContract]
    //    AdministrarClientes AdministrarClientesInsertar(Sesion sesion, AdministrarClientes entidad);

    //    [OperationContract]
    //    AdministrarClientes AdministrarClientesModificar(Sesion sesion, AdministrarClientes entidad);

    //    [OperationContract]
    //    bool AdministrarClientesEliminar(Sesion sesion, FiltroAdministrarClientes filtro);

    //    [OperationContract]
    //    AdministrarClientes AdministrarClientesObtener(Sesion sesion, FiltroAdministrarClientes filtro);

    //    [OperationContract]
    //    ListaAdministrarClientes AdministrarClientesObtenerTodosFiltro(Sesion sesion, FiltroAdministrarClientes filtro);

    //    [OperationContract]
    //    bool ModificarUltimaConexion(Sesion sesion, FiltroAdministrarClientes filtro);

    //    [OperationContract]
    //    bool ModificarFechaHoraCliente(Sesion sesion, FiltroAdministrarClientes filtro);

    //    [OperationContract]
    //    void CambiarEstatus(Sesion sesion, FiltroMonitorCambioPrecio filtro);

    //    #endregion

    //    #region Administrar Usuarios

    //    [OperationContract]
    //    int AdministrarUsuariosConsecutivo(Sesion sesion);

    //    [OperationContract]
    //    AdministrarUsuarios AdministrarUsuariosInsertar(Sesion sesion, AdministrarUsuarios entidad);

    //    [OperationContract]
    //    AdministrarUsuarios AdministrarUsuariosModificar(Sesion sesion, AdministrarUsuarios entidad);

    //    [OperationContract]
    //    bool AdministrarUsuariosEliminar(Sesion sesion, FiltroAdministrarUsuarios filtro);

    //    [OperationContract]
    //    AdministrarUsuarios AdministrarUsuariosObtener(Sesion sesion, FiltroAdministrarUsuarios filtro);

    //    [OperationContract]
    //    ListaAdministrarUsuarios AdministrarUsuariosObtenerTodosFiltro(Sesion sesion, FiltroAdministrarUsuarios filtro);

    //    #endregion

    //    #region Administrar Usuarios Cliente

    //    [OperationContract]
    //    int AdministrarUsuariosClienteConsecutivo(Sesion sesion, FiltroAdministrarUsuariosClientes filtro);

    //    [OperationContract]
    //    AdministrarUsuariosClientes AdministrarUsuariosClienteInsertar(Sesion sesion, AdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    AdministrarUsuariosClientes AdministrarUsuariosClienteModificar(Sesion sesion, AdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteEliminar(Sesion sesion, FiltroAdministrarUsuariosClientes filtro);

    //    [OperationContract]
    //    AdministrarUsuariosClientes AdministrarUsuariosClienteObtener(Sesion sesion, FiltroAdministrarUsuariosClientes filtro);

    //    [OperationContract]
    //    ListaAdministrarUsuariosClientes AdministrarUsuariosClienteObtenerTodosFiltro(Sesion sesion, FiltroAdministrarUsuariosClientes filtro);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteInsertarModificar(Sesion sesion, ListaAdministrarUsuariosClientes listado);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteNuevaContrasenia(Sesion sesion, FiltroAdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteCambiarContrasenia(Sesion sesion, AdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteRestablecerContrasenia(Sesion sesion, FiltroAdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    bool AdministrarUsuariosClienteSolicitarContrasenia(Sesion sesion, FiltroAdministrarUsuariosClientes entidad);

    //    [OperationContract]
    //    Entidades.Web.AdministrarUsuariosClientes AdministrarUsuariosClienteModificarPrivilegios(Sesion sesion, AdministrarUsuariosClientes entidad);

    //    #endregion

    //    #region Administrar Distribuidores

    //    [OperationContract]
    //    int AdministrarDistribuidoresConsecutivo(Sesion sesion);

    //    [OperationContract]
    //    AdministrarDistribuidores AdministrarDistribuidoresInsertar(Sesion sesion, AdministrarDistribuidores entidad);

    //    [OperationContract]
    //    AdministrarDistribuidores AdministrarDistribuidoresModificar(Sesion sesion, AdministrarDistribuidores entidad);

    //    [OperationContract]
    //    bool AdministrarDistribuidoresEliminar(Sesion sesion, FiltroAdministrarDistribuidores filtro);

    //    [OperationContract]
    //    AdministrarDistribuidores AdministrarDistribuidoresObtener(Sesion sesion, FiltroAdministrarDistribuidores filtro);

    //    [OperationContract]
    //    ListaAdministrarDistribuidores AdministrarDistribuidoresObtenerTodosFiltro(Sesion sesion, FiltroAdministrarDistribuidores filtro);

    //    #endregion

    //    #endregion

    //    #region Sesion

    //    //[OperationContract]
    //    //Sesion SesionObtener(Sesion sesion, FiltroSesion filtro);

    //    [OperationContract]
    //    ListaSesiones SesionObtenerTodosFiltro(Sesion sesion, FiltroSesion filtro);

    //    #endregion

    //    #region Actualizador

    //    ResponseUpdater Actualizar(RequestUpdater request);

    //    #endregion
    //}
}
