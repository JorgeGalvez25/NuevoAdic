using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ImagenSoft.ModuloWeb.Entidades.Base
{
    [Serializable]
    [DataContract]
    public class UsuarioModuloWeb
    {
        public UsuarioModuloWeb()
        {
            this.Activo =
                this.CorreoElectronico =
                this.Nombre =
                this.Password = string.Empty;
            this.Variables = new List<string>();
        }

        [DataMember]
        public string Activo { set; get; }

        [DataMember]
        public int Clave { set; get; }

        [DataMember]
        public string CorreoElectronico { set; get; }

        [DataMember]
        public string Nombre { set; get; }

        [DataMember]
        public string Password { set; get; }

        [DataMember]
        public System.Collections.Generic.List<string> Variables { set; get; }

        public UsuarioModuloWeb Clone()
        {
            return this.MemberwiseClone() as UsuarioModuloWeb;
        }
    }
}
