using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class LecturaTanque
    {
        public LecturaTanque()
        {
            this.Tanque = 0;
            this.Fecha = DateTime.MinValue;
            this.Turno = 0;
            this.Lectura = 0;
        }

        [DataMember]
        public int Tanque { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int Turno { get; set; }

        [DataMember]
        public double Lectura { get; set; }
    }
}
