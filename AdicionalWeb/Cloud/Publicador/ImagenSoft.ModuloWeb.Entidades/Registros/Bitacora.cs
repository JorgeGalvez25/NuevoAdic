using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades.Registros
{
    [Serializable]
    [DataContract]
    public class Bitacora
    {
        public Bitacora()
        {
            this.Cliente = 0;
            this.Error = string.Empty;
            this.Estacion = string.Empty;
            this.Fecha = SqlDateTime.MinValue.Value;
            this.Id = 0;
            this.Tipo = string.Empty;
            this.EstatusConexion = EstatusConexion.None;
            this.EstatusTransaccion = EstatusTransaccion.None;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Cliente { get; set; }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public EstatusConexion EstatusConexion { get; set; }

        [DataMember]
        public EstatusTransaccion EstatusTransaccion { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaBitacora : List<Bitacora>
    {
        ~ListaBitacora()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroBitacora
    {
        public FiltroBitacora()
        {
            this.Cliente = 0;
            this.Estacion = string.Empty;
            this.Fecha = SqlDateTime.MinValue.Value;
            this.Id = 0;
            this.Tipo = string.Empty;
            this.FechaInicio = SqlDateTime.MinValue.Value;
            this.FechaFin = SqlDateTime.MinValue.Value;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Cliente { get; set; }

        [DataMember]
        public string Estacion { get; set; }

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public DateTime FechaInicio
        {
            get { return this._fInicio.Date; }
            set { this._fInicio = value; }
        }

        [DataMember]
        public DateTime FechaFin
        {
            get { return this.GetMaxDate(this._fFin); }
            set { this._fFin = value; }
        }

        [DataMember]
        private DateTime _fInicio;

        [DataMember]
        private DateTime _fFin;

        private DateTime GetMaxDate(DateTime fin)
        {
            if (fin <= SqlDateTime.MinValue.Value)
            {
                return DateTime.Now;
            }

            return new DateTime(fin.Year, fin.Month, fin.Day, 23, 59, 59);
        }
    }
}
