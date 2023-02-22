using System;
using System.Runtime.Serialization;

namespace Adicional.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class Respuesta
    {
        public Respuesta()
        {
            this.IsFaulted = false;
            this.Message = string.Empty;
            this.Result = null;
        }

        [DataMember]
        public bool IsFaulted { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Object Result { get; set; }
    }
}
