using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class PreciosGasolinas
    {
        public PreciosGasolinas()
        {
            this.Id = 0;
            this.Fecha = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.PrecioDiessel = 0M;
            this.PrecioMagna = 0M;
            this.PrecioPremium = 0M;
            this.Zona = ZonasCambioPrecio.None;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public decimal PrecioMagna { get; set; }

        [DataMember]
        public decimal PrecioPremium { get; set; }

        [DataMember]
        public decimal PrecioDiessel { get; set; }

        [DataMember]
        public int Desface { get; set; }

        [DataMember]
        public EstatusProgramado EstatusCambioPrecio { get; set; }
    }

    [Serializable]
    [CollectionDataContract]
    public class ListaPreciosGasolinas : List<PreciosGasolinas>
    {
        ~ListaPreciosGasolinas()
        {
            this.Clear();
        }
    }

    [Serializable]
    [DataContract]
    public class FiltroPreciosGasolinas
    {
        public FiltroPreciosGasolinas()
        {
            this.Id = 0;
            this.Fecha = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            this.Zona = ZonasCambioPrecio.None;
            this.EstatusCambioPrecio = EstatusProgramado.Todos;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public ZonasCambioPrecio Zona { get; set; }

        [DataMember]
        public EstatusProgramado EstatusCambioPrecio { get; set; }
    }

}
