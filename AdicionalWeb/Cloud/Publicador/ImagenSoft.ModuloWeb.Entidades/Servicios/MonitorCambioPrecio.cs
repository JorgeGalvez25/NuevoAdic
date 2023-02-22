using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ImagenSoft.Atributos.ControlCatalogo;
using ImagenSoft.ModuloWeb.Entidades.Base;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class MonitorCambioPrecio : MonitorBase,
                                       IComparable<MonitorCambioPrecio>
    {
        public MonitorCambioPrecio()
            : base()
        {
            this.Aplicado = Aplicado.No;
            this.PrecioProgramado = string.Empty;
            this.Zona = ZonasCambioPrecio.None;
            this.FechaHoraCliente = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Programado = EstatusProgramado.Todos;
        }

        [DataMember]
        public DateTime FechaHoraCliente { get; set; }

        [DataMember]
        public string PrecioProgramado { get; set; }

        [DataMember]
        public Aplicado Aplicado { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }

        [DataMember]
        public DateTime FechaAplicacion { get; set; }

        [DataMember]
        public EstatusProgramado Programado { get; set; }

        #region IComparable<MonitorCambioPrecio> Members

        public int CompareTo(MonitorCambioPrecio other)
        {
            if (base.CompareTo(other) == 1) { return 1; }
            if (this.FechaHoraCliente != other.FechaHoraCliente) { return 1; }
            if (this.PrecioProgramado != other.PrecioProgramado) { return 1; }
            if (this.Aplicado != other.Aplicado) { return 1; }
            if (this.Zona != other.Zona) { return 1; }

            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMonitorCambioPrecio : List<MonitorCambioPrecio>
    {
        public ListaMonitorCambioPrecio OrdenarPor(OrdenarMonitor orden)
        {
            ListaMonitorCambioPrecio aux = new ListaMonitorCambioPrecio();

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

        public ListaMonitorCambioPrecio BuscarPorNombreComercial(string nomComercial)
        {
            ListaMonitorCambioPrecio aux = new ListaMonitorCambioPrecio();
            aux.AddRange(this.Where(p => p.NombreComercial.ToUpper().Contains(nomComercial.ToUpper())));
            return aux;
        }

        public ListaMonitorCambioPrecio ObtenerPorFiltro(FiltroMonitorCambioPrecio filtro)
        {
            ListaMonitorCambioPrecio aux = new ListaMonitorCambioPrecio();
            aux.AddRange(this.Where(p => (p.EstatusConexion == filtro.EstatusConexion || filtro.EstatusConexion == ImagenSoft.ModuloWeb.Entidades.Enumeradores.EstatusConexion.None) &&
                                         (p.PrecioProgramado == filtro.PrecioProgramado || string.IsNullOrEmpty(filtro.PrecioProgramado)) &&
                                         (p.Aplicado == filtro.Aplicado || filtro.Aplicado == Aplicado.Todos)));
            return aux;
        }

        public ListaMonitorCambioPrecio ObtenerPorFiltroO(FiltroMonitorCambioPrecio filtro)
        {
            ListaMonitorCambioPrecio aux = new ListaMonitorCambioPrecio();
            aux.AddRange(this.Where(p => (p.EstatusConexion == filtro.EstatusConexion || filtro.EstatusConexion == ImagenSoft.ModuloWeb.Entidades.Enumeradores.EstatusConexion.None) ||
                                         (p.PrecioProgramado == filtro.PrecioProgramado || string.IsNullOrEmpty(filtro.PrecioProgramado)) ||
                                         (p.Aplicado == filtro.Aplicado || filtro.Aplicado == Aplicado.Todos)));
            return aux;
        }

        public ListaMonitorCambioPrecio ObtenerZona(ZonasCambioPrecio zona)
        {
            ListaMonitorCambioPrecio aux = new ListaMonitorCambioPrecio();
            aux.AddRange(this.Where(p => p.Zona == zona || zona == ZonasCambioPrecio.None));
            return aux;
        }

        public ListaMonitorCambioPrecio ObtenerPorDistribuidor(int id)
        {
            if (id == 1) return this;

            ListaMonitorCambioPrecio result = new ListaMonitorCambioPrecio();
            result.AddRange(this.Where(p => p.IdDistribuidor == id));
            return result;
        }

        ~ListaMonitorCambioPrecio()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroMonitorCambioPrecio : FiltroMonitorBase
    {
        public FiltroMonitorCambioPrecio()
            : base()
        {
            this.Aplicado = Aplicado.Todos;
            this.PrecioProgramado = string.Empty;
            this.Zona = ZonasCambioPrecio.None;
            this.FechaAplicacion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.FechaHoraCliente = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Programado = EstatusProgramado.No;
            this.ConMonitoreo = string.Empty;
        }

        [DataMember]
        public string PrecioProgramado { get; set; }

        [DataMember]
        public Aplicado Aplicado { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }

        [DataMember]
        public DateTime FechaAplicacion { get; set; }

        [DataMember]
        public DateTime FechaHoraCliente { get; set; }

        [DataMember]
        public EstatusProgramado Programado { get; set; }

        [DataMember]
        public string ConMonitoreo { get; set; }
    }
}
