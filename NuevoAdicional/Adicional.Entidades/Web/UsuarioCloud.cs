using System;
using System.Runtime.Serialization;

namespace Adicional.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class UsuarioCloud
    {
        public UsuarioCloud()
        {
            this.Correo = string.Empty;
            this.NoEstacion = string.Empty;
            this.Password = string.Empty;
            this.Privilegios = new object();
            this.Usuario = string.Empty;
        }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public object Privilegios { get; set; }
    }
}
