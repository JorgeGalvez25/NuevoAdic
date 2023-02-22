using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using AdicionalHost = Adicional.Entidades;

namespace ImagenSoft.ModuloWeb.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class AdicionalWebFiltroBase
    {
        public AdicionalWebFiltroBase()
        {
            this.IP = string.Empty;
            this.Matriz = string.Empty;
            this.NoEstacion = string.Empty;
        }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Matriz { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public int Puerto { get; set; }

        [DataMember]
        public AdicionalHost.ListaHistorial Historial { get; set; }
    }
}
