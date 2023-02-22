using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Entidades
{
    [Serializable]
    [DataContract]
    [XmlInclude(typeof(SesionModuloWeb))]
    [XmlInclude(typeof(FiltroSesionModuloWeb))]
    public class SolicitudHostWeb
    {
        public SolicitudHostWeb()
        {
            this.Sesion = new ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb();
            this.Metodo = Metodos.None;
            this.Parametro = new object();
        }

        [DataMember]
        public ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb Sesion { get; set; }

        [DataMember]
        public Metodos Metodo { get; set; }

        [DataMember]
        public object Parametro { get; set; }
    }
}
