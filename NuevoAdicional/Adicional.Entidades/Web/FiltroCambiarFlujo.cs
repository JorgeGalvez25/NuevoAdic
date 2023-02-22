using System;
using System.Runtime.Serialization;

namespace Adicional.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class FiltroCambiarFlujo
    {
        public FiltroCambiarFlujo()
        {
            this.NoEstacion = string.Empty;
        }

        [DataMember]
        public UsuarioCloud Usuario { get; set; }

        [DataMember]
        public bool Estandar { get; set; }

        [DataMember]
        public string NoEstacion { get; set; }

        ~FiltroCambiarFlujo()
        {
            this.Usuario = null;
            this.Estandar = default(bool);
        }
    }
}
