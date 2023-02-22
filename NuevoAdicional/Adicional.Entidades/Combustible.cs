using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class Combustible
    {
        public Combustible()
        {
            this.Clave = 0;
            this.Nombre = string.Empty;
            this.Precio = 0D;
            this.MaxPosCarga = 0;
        }

        [DataMember]
        public int Clave { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public double Precio { get; set; }

        [DataMember]
        public int MaxPosCarga { get; set; }
    }

    [CollectionDataContract]
    [Serializable]    
    public class ListaCombustible : List<Combustible>
    {
        ~ListaCombustible()
        {
            this.Clear();
        }
    }
}
