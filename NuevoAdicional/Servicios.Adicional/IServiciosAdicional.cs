using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Adicional.Entidades;

namespace Servicios.Adicional
{
    [ServiceContract]
    public interface IServiciosAdicional
    {
        [OperationContract]
        List<Historial> ObtenerUltimosCambios(int AEstacion);

        #region Estaciones
        [OperationContract]
        string ObtenerNombreEstacion();

        [OperationContract]
        ListaEstacion ObtenerListaEstaciones();

        [OperationContract]
        Estacion EstacionInsertar(Estacion entidad);

        [OperationContract]
        Estacion EstacionActualizar(Estacion entidad);

        [OperationContract]
        bool EstacionEliminar(int id);

        [OperationContract]
        bool EstacionActivarProtecciones(int idEstacion, bool enable);
        #endregion

        #region Usuarios
        [OperationContract]
        ListaUsuario ObtenerUsuariosActivos();

        [OperationContract]
        Usuario UsuarioObtener(int idUsuario);

        [OperationContract]
        ListaUsuario UsuarioObtenerLista();

        [OperationContract]
        Usuario UsuarioInsertar(Usuario entidad);

        [OperationContract]
        Usuario UsuarioActualizar(Usuario entidad);

        [OperationContract]
        bool UsuarioEliminar(int idUsuario);
        #endregion

        #region Bitácora
        [OperationContract]
        Bitacora BitacoraInsertar(Bitacora entidad);

        [OperationContract]
        ListaBitacora BitacoraObtenerListaPorFecha(DateTime fechaInicial, DateTime fechaFinal);

        [OperationContract]
        bool UsuarioTieneMovimientosEnBitacora(string idUsuario);
        #endregion

        #region Historial
        [OperationContract]
        List<int> HistorialObtenerPosiciones(int claveEstacion);

        [OperationContract]
        ListaHistorial HistorialObtenerPorPosicion(int claveEstacion, int posicion);

        [OperationContract]
        ListaHistorial HistorialObtenerRecientes(int pIdEstacion);

        [OperationContract]
        Historial HistorialInsertar(Historial entidad);

        [OperationContract]
        Historial HistorialActualizar(Historial entidad);
        #endregion

        #region Derechos
        [OperationContract]
        ListaDerecho DerechoObtenerListaPorUsuario(int idUsuario);
        #endregion

        #region Configuracion
        [OperationContract]
        Configuracion ConfiguracionActualizar(Configuracion pConfig);

        [OperationContract]
        Configuracion ConfiguracionObtener(int id);

        [OperationContract]
        bool ConfiguracionActivarProtecciones(int idConfiguracion, bool enable);

        [OperationContract]
        bool ConfiguracionCambiarEstado(string estado);

        [OperationContract]
        DateTime ConfiguracionActualizarUltimoMovimiento(DateTime fecha);

        [OperationContract]
        DateTime ConfiguracionActualizarUltimaSincronizacion(DateTime fecha);
        #endregion

        #region Proteccion
        [OperationContract]
        ListaProteccion ProteccionObtenerLista(int idEstacion);

        [OperationContract]
        ListaProteccion ProteccionInsertarActualizar(ListaProteccion protecciones);
        #endregion

        #region remoto
        [OperationContract]
        EdoRemoto LeerEstadoRemoto();

        [OperationContract]
        void ApagarVisual();

        [OperationContract]
        void EncenderVisual();
        #endregion

        #region Moviles
        [OperationContract]
        Moviles MovilesObtener(FiltroMoviles moviles);

        [OperationContract]
        ListaMoviles MovilesObtenerTodos();

        [OperationContract]
        Moviles MovilesInsertar(Moviles entidad);

        [OperationContract]
        Moviles MovilesActualizar(Moviles entidad);

        [OperationContract]
        bool MovilesEliminar(string numero);
        #endregion

        #region Licencias
        [OperationContract]
        List<Licencia> LicenciasObtener();

        [OperationContract]
        Licencia LicenciaObtener(string modulo);

        [OperationContract]
        Licencia LicenciaInsertar(string modulo, string lic);

        [OperationContract]
        Licencia LicenciaActualizar(Licencia licencia);

        [OperationContract]
        bool LicenciaEliminar(string modulo);

        [OperationContract]
        bool LicenciaValida(Licencia lic, string version);
        #endregion

        #region Utilerias
        [OperationContract]
        bool IsAlive();
        #endregion
    }
}
