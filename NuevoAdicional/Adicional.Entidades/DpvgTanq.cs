using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class DpvgTanq
    {
        public DpvgTanq()
        {
            this.Combustible = 0;
            this.Nombre = string.Empty;
            this.Precio = 0D;
            this.Tanque = 0;
        }

        [DataMember]
        public int Tanque { get; set; }

        [DataMember]
        public int Combustible { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public double Precio { get; set; }
    }

    public class ListaDpvgTanq : List<DpvgTanq>
    {
        ~ListaDpvgTanq()
        {
            this.Clear();
        }
    }
}
