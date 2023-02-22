using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class MonitorAplicacionesDetalle : IComparable<MonitorAplicacionesDetalle>
    {
        public MonitorAplicacionesDetalle()
        {
            this.Estacion = string.Empty;
            this.Observaciones = string.Empty;
            this.Servicio = string.Empty;
        }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public int Indice { get; set; }

        [DataMember]
        public string Servicio { get; set; }

        [DataMember]
        public decimal MemoriaUsada { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        #region IComparable<MonitorAplicacionesDetalle> Members

        public int CompareTo(MonitorAplicacionesDetalle other)
        {
            if (this.Estacion != other.Estacion) { return 1; }
            if (this.IdCliente != other.IdCliente) { return 1; }
            if (this.Indice != other.Indice) { return 1; }
            if (this.MemoriaUsada != other.MemoriaUsada) { return 1; }
            if (this.Observaciones != other.Observaciones) { return 1; }
            if (this.Servicio != other.Servicio) { return 1; }
            return 0;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaMonitorAplicacionesDetalle : List<MonitorAplicacionesDetalle>
    {
        public ListaMonitorAplicacionesDetalle ObtenerOrdenadoMayorConsumo()
        {
            if (this.Count <= 0) { return this; }
            ListaMonitorAplicacionesDetalle aux = new ListaMonitorAplicacionesDetalle();
            aux.AddRange(this.OrderByDescending(p => p.MemoriaUsada));
            return aux;
        }

        public MonitorAplicacionesDetalle ObtenerMayorConsumo()
        {
            return this.ObtenerOrdenadoMayorConsumo().FirstOrDefault();
        }

        ~ListaMonitorAplicacionesDetalle()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroMonitorAplicacionesDetalle
    {
        public FiltroMonitorAplicacionesDetalle()
        {
            this.Estacion = string.Empty;
        }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public int Indice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Aplicaciones
    {
        [DataMember]
        public string Nombre { get; set; }
        /// <summary>
        /// Utilizar con la propiedad de la clase Process.PrivateMemorySize64
        /// </summary>
        [DataMember]
        public decimal MemoriaUsada { get; set; }

        [DataMember]
        public string Observaciones { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaAplicaciones : List<Aplicaciones>
    {
        public ListaAplicaciones()
        {
            this.SistemaOperativo = string.Empty;
            this.Plataforma = string.Empty;
            this.Version = string.Empty;
        }

        /// <summary>
        /// Utilizar con la propiedad de la clase Computer.Info.TotalPhysicalMemory
        /// </summary>
        [DataMember]
        public decimal MemoriaEquipoTotal { get; set; }

        /// <summary>
        /// Utilizar con la propiedad de la clase Computer.Info.AvailablePhysicalMemory
        /// </summary>
        [DataMember]
        public decimal MemoriaEquipoDisponible { get; set; }

        [DataMember]
        public decimal MemoriaEquipoUtilizada { get { return this.MemoriaEquipoTotal - this.MemoriaEquipoUtilizada; } }

        /// <summary>
        /// Utilizar con la propiedad de la clase Computer.Info.OSFullName
        /// </summary>
        [DataMember]
        public string SistemaOperativo { get; set; }

        /// <summary>
        /// Utilizar con la propiedad de la clase Compu.Info.OSPlatform
        /// </summary>
        [DataMember]
        public string Plataforma { get; set; }

        /// <summary>
        /// Utilizar con la propiedad de la clase Compu.Info.OSVersion
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        ~ListaAplicaciones()
        {
            this.Clear();
        }
    }
}
