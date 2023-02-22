using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Adicional.Entidades;

namespace ServiciosCliente
{
    [ServiceContract]
    public interface IServiciosCliente
    {
        [OperationContract]
        string AplicarFlujo(bool std, bool paro,MarcaDispensario marca, List<Adicional.Entidades.Historial> AListaHistorial);

        [OperationContract]
        List<Historial> ObtenerBombasEstacion();

        [OperationContract]
        List<ReporteAjuste> ObtenerReporteAjuste(DateTime fecha);

        [OperationContract]
        bool SetRegenerarArchivosVolumetricos(DateTime AFecha, int ACorte, out string AMensajeError);

        //[OperationContract]
        //bool ProteccionEliminar();

        //[OperationContract]
        //int ProteccionInsertar(List<int> litros, out string mensaje);

        [OperationContract]
        bool Sincronizar(byte status, out string mensaje);

        [OperationContract]
        List<ReporteAjuste> ObtenerReporte6a6(DateTime fecha);

        [OperationContract]
        List<ReporteAjuste> ObtenerReporteDetallado(DateTime fecha);

        [OperationContract]
        ReporteAjuste ObtenerReporte2(DateTime fecha, int combustible, bool a24hrs);

        [OperationContract]
        string SubirBajarFlujo(bool std);

        [OperationContract]
        string AplicarProteccionGilbarco(bool std, string tipo);

        [OperationContract]
        string AplicarFlujoGilbarcoPorcentajes();

        #region Utilerias

        [OperationContract]
        bool IsAlive();
        [OperationContract]
        string ObtenerEstatus();
        [OperationContract]
        void ComandoInsertar(Comandos comando);
        [OperationContract]
        List<string> CalibrarBombas(List<string> comandos, int marca);
        [OperationContract]
        bool PosFinVta();
        [OperationContract]
        bool CalibrarPosicion(int posicion);
        [OperationContract]
        void AplicarProtecciones(string comandostr);
        #endregion

        #region Tanques

        [OperationContract]
        Tanques ObtenerComplemento(Tanques entidad);
        [OperationContract]
        ListaDpvgTanq ObtenerTanques();
        [OperationContract]
        bool TanquesEliminar(FiltroTanques filtro, string usuario);
        [OperationContract]
        bool TanquesModificar(Tanques entidad, string usuario);
        [OperationContract]
        Tanques TanquesObtener(FiltroTanques filtro);
        [OperationContract]
        ListaTanques TanquesObtenerTodos(FiltroTanques filtro);
        [OperationContract]
        bool TanquesRegistrar(Tanques entidad, string usuario);
        [OperationContract]
        bool RegistrarLectura(LecturaTanque entidad);

        #endregion

        #region Tickets
        [OperationContract]
        ListaCombustible ObtenerCombustibles();
        [OperationContract]
        bool TicketActualizar(Ticket entidad, string usuario);
        [OperationContract]
        int TicketConsecutivo();
        [OperationContract]
        Ticket TicketObtener(FiltroTicket filtro);
        [OperationContract]
        Ticket TicketRegistrar(Ticket entidad, string usuario);
        #endregion
    }
}
