using System;
using System.Runtime.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    public class RespuestaHostWeb
    {
        public RespuestaHostWeb()
        {
            this.EsValido = true;
            this.Mensaje = string.Empty;
            this.Resultado = new object();
        }

        [DataMember]
        public bool EsValido { get; set; }

        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public object Resultado { get; set; }
    }
}
