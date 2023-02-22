using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Entidades.Web.Adicional;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ImagenSoft.ModuloWeb.Interfaces.Publicador
{
    [ServiceContract(Namespace = "http://moduloweb.adicional.com")]
    public interface IModuloWebAdicional
    {
        [OperationContract]
        byte[] EnviarPeticion(byte[] solicitud);
    }

    [ServiceContract(Namespace = "http://moduloweb.adicional.com")]
    public interface IModuloWebProveedor//public interface IServiciosAdicionalProveedor
    {
        [OperationContract]
        bool AdicionalWebCambiarFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro);

        [OperationContract]
        SesionModuloWeb AdicionalWebValidarLogin(SesionModuloWeb sesion, UsuarioWeb entidad);

        [OperationContract]
        ListaEstaciones AdicionalWebVerificarEstaciones(SesionModuloWeb sesion);

        [OperationContract]
        string AdicionalWebEstadoFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro);

        [OperationContract]
        ListaDispensarios AdicionalWebObtenerMangueras(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro);

        [OperationContract]
        bool AdicionalWebEstablecerDispensario(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro);

        [OperationContract]
        bool AdicionalWebEstablecerDispensarioGlobal(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro);

        [OperationContract]
        List<Adicional.Entidades.ReporteVentasCombustible> ObtenerReporteVentasCombustible(SesionModuloWeb sesion, Adicional.Entidades.FiltroReporteVentasCombustible filtro);

        [OperationContract]
        bool ServicioPings();

        #region Administrar Clientes

        [OperationContract]
        int AdministrarClientesConsecutivo(SesionModuloWeb sesion);

        [OperationContract]
        AdministrarClientes AdministrarClientesInsertar(SesionModuloWeb sesion, AdministrarClientes entidad);

        [OperationContract]
        AdministrarClientes AdministrarClientesModificar(SesionModuloWeb sesion, AdministrarClientes entidad);

        [OperationContract]
        bool AdministrarClientesEliminar(SesionModuloWeb sesion, FiltroAdministrarClientes filtro);

        [OperationContract]
        AdministrarClientes AdministrarClientesObtener(SesionModuloWeb sesion, FiltroAdministrarClientes filtro);

        [OperationContract]
        ListaAdministrarClientes AdministrarClientesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarClientes filtro);

        //[OperationContract]
        //bool ModificarUltimaConexion(Sesion sesion, FiltroAdministrarClientes filtro);

        //[OperationContract]
        //bool ModificarFechaHoraCliente(Sesion sesion, FiltroAdministrarClientes filtro);

        //[OperationContract]
        //void CambiarEstatus(Sesion sesion, FiltroMonitorCambioPrecio filtro);

        #endregion

        #region Administrar Usarios Cliente

        [OperationContract]
        ListaAdministrarUsuariosClientes AdministrarUsuariosClienteObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro);

        [OperationContract]
        AdministrarUsuariosClientes AdministrarUsuariosClienteObtener(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro);

        [OperationContract]
        AdministrarUsuariosClientes AdministrarUsuariosClienteInsertar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad);

        [OperationContract]
        AdministrarUsuariosClientes AdministrarUsuariosClienteModificar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad);

        [OperationContract]
        bool AdministrarUsuariosClienteEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro);

        [OperationContract]
        bool AdministrarUsuariosClienteSolicitarContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro);

        [OperationContract]
        AdministrarUsuariosClientes AdministrarUsuariosClienteModificarPrivilegios(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad);

        [OperationContract]
        bool AdministrarUsuariosClienteNuevaContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes entidad);

        [OperationContract]
        bool AdministrarUsuariosClienteCambiarContrasenia(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad);

        [OperationContract]
        bool AdministrarUsuariosClienteRestablecerContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro);

        [OperationContract]
        bool AdministrarUsuariosClienteInsertarModificar(SesionModuloWeb sesion, ListaAdministrarUsuariosClientes listado);

        #endregion

        #region Distribuidores

        [OperationContract]
        int AdministrarDistribuidoresConsecutivo(SesionModuloWeb sesion);

        [OperationContract]
        AdministrarDistribuidores AdministrarDistribuidoresInsertar(SesionModuloWeb sesion, AdministrarDistribuidores entidad);

        [OperationContract]
        AdministrarDistribuidores AdministrarDistribuidoresModificar(SesionModuloWeb sesion, AdministrarDistribuidores entidad);

        [OperationContract]
        bool AdministrarDistribuidoresEliminar(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro);

        [OperationContract]
        AdministrarDistribuidores AdministrarDistribuidoresObtener(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro);

        [OperationContract]
        ListaAdministrarDistribuidores AdministrarDistribuidoresObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro);

        #endregion

        #region Administrar Usuarios

        [OperationContract]
        int AdministrarUsuariosConsecutivo(SesionModuloWeb sesion);

        [OperationContract]
        AdministrarUsuarios AdministrarUsuariosInsertar(SesionModuloWeb sesion, AdministrarUsuarios entidad);

        [OperationContract]
        AdministrarUsuarios AdministrarUsuariosModificar(SesionModuloWeb sesion, AdministrarUsuarios entidad);

        [OperationContract]
        bool AdministrarUsuariosEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro);

        [OperationContract]
        AdministrarUsuarios AdministrarUsuariosObtener(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro);

        [OperationContract]
        ListaAdministrarUsuarios AdministrarUsuariosObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro);

        #endregion

        [OperationContract]
        ListaSesiones SesionObtenerTodosFiltro(SesionModuloWeb sesion, FiltroSesionModuloWeb filtro);

        [OperationContract]
        DateTime ObtenerFechaHoraServidor();
    }
}
