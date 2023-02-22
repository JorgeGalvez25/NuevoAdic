using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Base;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class MonitorTransaccion : MonitorBase,
                                      IComparable<MonitorTransaccion>
    {
        public MonitorTransaccion()
            : base()
        {
            this.EstatusTransaccion = EstatusTransaccion.None;
            this.FechaCorteTransmision = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaProximaTransmision = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaUltimaTransmision = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Observaciones = string.Empty;
        }

        [DataMember]
        public DateTime FechaCorteTransmision { get; set; }

        [DataMember]
        public EstatusTransaccion EstatusTransaccion { get; set; }

        [DataMember]
        public DateTime FechaProximaTransmision { get; set; }

        [DataMember]
        public DateTime FechaUltimaTransmision { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        #region IComparable<MonitorTransaccion> Members

        public int CompareTo(MonitorTransaccion other)
        {
            if (base.CompareTo(other) == 1) { return 1; }
            if (this.EstatusTransaccion != other.EstatusTransaccion) { return 1; }
            if (this.FechaCorteTransmision != other.FechaCorteTransmision) { return 1; }
            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMonitorTransaccion : List<MonitorTransaccion>
    {
        public ListaMonitorTransaccion OrdenarPor(OrdenarMonitor orden)
        {
            ListaMonitorTransaccion aux = new ListaMonitorTransaccion();

            switch (orden)
            {
                case OrdenarMonitor.DiasAtraso:
                    aux.AddRange(this.OrderByDescending(p => p.DiasAtraso));
                    break;
                case OrdenarMonitor.NoEstacion:
                    aux.AddRange(this.OrderBy(p => p.Estacion));
                    break;
                case OrdenarMonitor.NombreComercial:
                    aux.AddRange(this.OrderBy(p => p.NombreComercial));
                    break;
            }

            return aux;
        }

        public ListaMonitorTransaccion BuscarPorNombreComercial(string nomComercial)
        {
            ListaMonitorTransaccion aux = new ListaMonitorTransaccion();
            aux.AddRange(this.Where(p => p.NombreComercial.ToUpper().Contains(nomComercial.ToUpper())));
            return aux;
        }

        public ListaMonitorTransaccion ObtenerPorFiltroY(FiltroMonitorTransaccion filtro)
        {
            ListaMonitorTransaccion aux = new ListaMonitorTransaccion();
            aux.AddRange(this.Where(p => (p.EstatusConexion == filtro.EstatusConexion || filtro.EstatusConexion == EstatusConexion.None) &&
                                         (p.EstatusTransaccion == filtro.EstatusTransaccion || filtro.EstatusTransaccion == EstatusTransaccion.None)));

            return aux;
        }

        public ListaMonitorTransaccion ObtenerPorFiltroO(FiltroMonitorTransaccion filtro)
        {
            ListaMonitorTransaccion aux = new ListaMonitorTransaccion();
            aux.AddRange(this.Where(p => (p.EstatusConexion == filtro.EstatusConexion || filtro.EstatusConexion == EstatusConexion.None) ||
                                         (p.EstatusTransaccion == filtro.EstatusTransaccion || filtro.EstatusTransaccion == EstatusTransaccion.None)));

            return aux;
        }

        public ListaMonitorTransaccion ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaMonitorTransaccion result = new ListaMonitorTransaccion();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaMonitorTransaccion()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroMonitorTransaccion : FiltroMonitorBase
    {
        public FiltroMonitorTransaccion()
            : base()
        {
            this.EstatusTransaccion = EstatusTransaccion.None;
            this.FechaUltimaTransmision = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaCorteTransmision = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [DataMember]
        public EstatusTransaccion EstatusTransaccion { get; set; }

        [DataMember]
        public DateTime FechaCorteTransmision { get; set; }

        [DataMember]
        public DateTime FechaUltimaTransmision { get; set; }

        [DataMember]
        public bool ConMonitoreo { get; set; }
    }
}
