using System;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades.Base
{
    [Serializable]
    [DataContract]
    public class MonitorBase : //ImagenSoft.Entidades.ClaseBase,
                               IComparable<MonitorBase>
    {
        public MonitorBase()
        {
            this.DiasAtraso = 0;
            this.Estacion = string.Empty;
            this.EstatusConexion = EstatusConexion.None;
            this.FechaInicioSesion = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.NombreComercial = string.Empty;
            this.RazonSocial = string.Empty;
            this.Distribuidor = string.Empty;
        }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public string NombreComercial { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public DateTime FechaInicioSesion { get; set; }

        [DataMember]
        public EstatusConexion EstatusConexion { get; set; }

        [DataMember]
        public int DiasAtraso { get; set; }

        [DataMember]
        public int IdDistribuidor { get; set; }

        [DataMember]
        public string Distribuidor { get; set; }

        [DataMember]
        public string Version { get; set; }

        #region IComparable<MonitorBase> Members

        public int CompareTo(MonitorBase other)
        {
            if (this.DiasAtraso != other.DiasAtraso) { return 1; }
            if (this.Estacion != other.Estacion) { return 1; }
            if (this.EstatusConexion != other.EstatusConexion) { return 1; }
            if (this.FechaInicioSesion != other.FechaInicioSesion) { return 1; }
            if (this.NombreComercial != other.NombreComercial) { return 1; }
            if (this.RazonSocial != other.RazonSocial) { return 1; }
            return 0;
        }

        #endregion
    }

    [Serializable]
    [DataContract]
    public class FiltroMonitorBase
    {
        public FiltroMonitorBase()
        {
            this.DiasAtraso = 0;
            this.NoEstacion = string.Empty;
            this.EstatusConexion = EstatusConexion.None;
        }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public int DiasAtraso { get; set; }

        [DataMember]
        public EstatusConexion EstatusConexion { get; set; }

        [DataMember]
        public int Distribuidor { get; set; }
    }
}
