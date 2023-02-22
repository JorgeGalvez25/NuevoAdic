using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Base;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class MonitorAplicaciones : MonitorBase,
                                       IComparable<MonitorAplicaciones>
    {
        public MonitorAplicaciones()
        {
            this.EstatusAlerta = EstatusAplicacionAlertas.Ok;
            this.Detalle = new ListaMonitorAplicacionesDetalle();
        }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string SistemaOperativo { get; set; }

        [DataMember]
        public EstatusAplicacionAlertas EstatusAlerta { get; set; }

        [DataMember]
        public decimal MemoriaTotal { get; set; }

        [DataMember]
        public decimal MemoriaDisponible { get; set; }

        [DataMember]
        public ListaMonitorAplicacionesDetalle Detalle { get; set; }

        #region IComparable<MonitorAplicaciones> Members

        public int CompareTo(MonitorAplicaciones other)
        {
            if (base.CompareTo(other) == 1) { return 1; }
            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMonitorAplicaciones : List<MonitorAplicaciones>
    {
        public ListaMonitorAplicaciones OrdenarPor(OrdenarMonitor orden)
        {
            ListaMonitorAplicaciones aux = new ListaMonitorAplicaciones();

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

        public ListaMonitorAplicaciones BuscarPorNombreComercial(string nomComercial)
        {
            ListaMonitorAplicaciones aux = new ListaMonitorAplicaciones();
            aux.AddRange(this.Where(p => p.NombreComercial.ToUpper().Contains(nomComercial.ToUpper())));
            return aux;
        }

        public ListaMonitorAplicaciones ObtenerPorFiltro(FiltroMonitorAplicaciones filtro)
        {
            ListaMonitorAplicaciones aux = new ListaMonitorAplicaciones();
            aux.AddRange(this.Where(p => p.EstatusConexion == filtro.EstatusConexion));

            return aux;
        }

        public ListaMonitorAplicaciones ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaMonitorAplicaciones result = new ListaMonitorAplicaciones();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaMonitorAplicaciones()
        {
            this.Clear();
        }
    }
    [Serializable]
    [DataContract]
    public class FiltroMonitorAplicaciones : FiltroMonitorBase
    {
        public FiltroMonitorAplicaciones()
            : base()
        {
            this.EstatusAlerta = EstatusAplicacionAlertas.Ok;
        }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public EstatusAplicacionAlertas EstatusAlerta { get; set; }
    }
}
