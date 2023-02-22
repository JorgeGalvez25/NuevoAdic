using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades.Web
{
    [Serializable]
    [DataContract]
    public class Privilegios
    {
        public Privilegios()
        {
            this.Configuraciones = new Configuraciones();
            this.Permisos = new Permisos();
        }

        [DataMember]
        [XmlElement("Configuraciones")]
        public Configuraciones Configuraciones { get; set; }
        public bool ShouldSerializeConfiguraciones()
        {
            return Configuraciones != null;
        }

        [DataMember]
        [XmlElement("Permisos")]
        public Permisos Permisos { get; set; }
        public bool ShouldSerializePermisos()
        {
            return Permisos != null;
        }
    }

    [Serializable]
    [DataContract]
    public class Configuraciones
    {
        [DataMember]
        [XmlElement("Validacion2Pasos")]
        public bool Validacion2Pasos { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Permisos
    {
        [DataMember]
        [XmlElement("CambiarPassword")]
        public bool CambiarPassword { get; set; }

        [DataMember]
        [XmlElement("VerTodasEstaciones")]
        public bool VerTodasEstaciones { get; set; }
    }
}
