using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public enum Metodos
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        TransaccionInsertar,
        [EnumMember]
        TransaccionModificar,
        [EnumMember]
        TransaccionEliminar,
        [EnumMember]
        TransaccionObtener,
        [EnumMember]
        TransaccionObtenerTodosFiltro,
        [EnumMember]
        Transmitiendo,

        [EnumMember]
        CambioPrecioInsertar,
        [EnumMember]
        CambioPrecioModificar,
        [EnumMember]
        CambioPrecioEliminar,
        [EnumMember]
        CambioPrecioObtener,
        [EnumMember]
        CambioPrecioObtenerTodosFiltro,

        [EnumMember]
        PreciosGasolinasConsecutivo,
        [EnumMember]
        PreciosGasolinasInsertar,
        [EnumMember]
        PreciosGasolinasModificar,
        [EnumMember]
        PreciosGasolinasEliminar,
        [EnumMember]
        PreciosGasolinasObtener,
        [EnumMember]
        PreciosGasolinasObtenerTodosFiltro,

        [EnumMember]
        AdministrarClientesConsecutivo,
        [EnumMember]
        AdministrarClientesInsertar,
        [EnumMember]
        AdministrarClientesModificar,
        [EnumMember]
        AdministrarClientesEliminar,
        [EnumMember]
        AdministrarClientesObtener,
        [EnumMember]
        AdministrarClientesObtenerTodosFiltro,

        [EnumMember]
        ModificarUltimaConexion,
        [EnumMember]
        ModificarFechaHoraCliente,
        [EnumMember]
        CambiarEstatus,

        [EnumMember]
        AdministrarUsuariosConsecutivo,
        [EnumMember]
        AdministrarUsuariosInsertar,
        [EnumMember]
        AdministrarUsuariosModificar,
        [EnumMember]
        AdministrarUsuariosEliminar,
        [EnumMember]
        AdministrarUsuariosObtener,
        [EnumMember]
        AdministrarUsuariosObtenerTodosFiltro,

        [EnumMember]
        AdministrarUsuariosClienteConsecutivo,
        [EnumMember]
        AdministrarUsuariosClienteInsertar,
        [EnumMember]
        AdministrarUsuariosClienteModificar,
        [EnumMember]
        AdministrarUsuariosClienteEliminar,
        [EnumMember]
        AdministrarUsuariosClienteObtener,
        [EnumMember]
        AdministrarUsuariosClienteObtenerTodosFiltro,
        [EnumMember]
        AdministrarUsuariosClienteInsertarModificar,
        [EnumMember]
        AdministrarUsuariosClienteNuevaContrasenia,
        [EnumMember]
        AdministrarUsuariosClienteCambiarContrasenia,
        [EnumMember]
        AdministrarUsuariosClienteRestablecerContrasenia,
        [EnumMember]
        AdministrarUsuariosClienteSolicitarContrasenia,
        [EnumMember]
        AdministrarUsuariosClienteModificarPrivilegios,

        [EnumMember]
        AdministrarDistribuidoresConsecutivo,
        [EnumMember]
        AdministrarDistribuidoresInsertar,
        [EnumMember]
        AdministrarDistribuidoresModificar,
        [EnumMember]
        AdministrarDistribuidoresEliminar,
        [EnumMember]
        AdministrarDistribuidoresObtener,
        [EnumMember]
        AdministrarDistribuidoresObtenerTodosFiltro,

        [EnumMember]
        SesionObtenerTodosFiltro,

        [EnumMember]
        ObtenerFechaHoraServidor,
        [EnumMember]
        ObtenerFechaHoraCentralServidor,

        [EnumMember]
        ProcesarNotificaciones,

        [EnumMember]
        ServicioPings,

        [EnumMember]
        MonitorConexionesInsertar,
        [EnumMember]
        MonitorConexionesModificar,
        [EnumMember]
        MonitorConexionesEliminar,
        [EnumMember]
        MonitorConexionesObtener,
        [EnumMember]
        MonitorConexionesObtenerTodosFiltro,

        [EnumMember]
        MonitorAplicacionesInsertar,
        [EnumMember]
        MonitorAplicacionesModificar,
        [EnumMember]
        MonitorAplicacionesEliminar,
        [EnumMember]
        MonitorAplicacionesObtener,
        [EnumMember]
        MonitorAplicacionesObtenerTodosFiltro,

        [EnumMember]
        Actualizar,

        //[EnumMember]
        //VolumetricoWebObtenerEstacionesTodosFiltro,
        //[EnumMember]
        //VolumetricoWebValidarLogin,
        //[EnumMember]
        //VolumetricoWebObtenerTanquesTodosFiltro,
        //[EnumMember]
        //VolumetricoWebObtenerDispensariosTodosFiltro,

        [EnumMember]
        AdicionaWeblLogin,

        [EnumMember]
        AdicionalWebCambiarFlujo,

        [EnumMember]
        AdicionalWebEstadoFlujo,

        [EnumMember]
        AdicionalWebObtenerMangueras,

        [EnumMember]
        AdicionalWebEstablecerDispensario,

        [EnumMember]
        AdicionalWebEstablecerDispensarioGlobal,

        [EnumMember]
        AdicionalWebVerificarEstaciones,

        [EnumMember]
        Ping,

        [EnumMember]
        PingSesion,

        [EnumMember]
        GetConfig,

        [EnumMember]
        ObtenerReporteVentasCombustible
    }
}
