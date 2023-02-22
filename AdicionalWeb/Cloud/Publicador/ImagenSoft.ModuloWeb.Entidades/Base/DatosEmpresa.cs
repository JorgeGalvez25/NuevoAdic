using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ImagenSoft.ModuloWeb.Entidades.Base
{
    [Serializable]
    [DataContract]
    public class DatosEmpresa
    {
        public DatosEmpresa()
        {
            this.NombreComercial =
                this.RazonSocial = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NombreComercial { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }
    }
}
