using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    [DataContract]
    public class SolicitudAdicional
    {
        public SolicitudAdicional()
        {
            this.Metodo = MetodosAdicional.None;
            this.Parametro = null;
        }

        [DataMember]
        public MetodosAdicional Metodo { get; set; }

        [DataMember]
        public object Parametro { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RespuestaAdicional
    {
        public RespuestaAdicional()
        {
            this.Metodo = MetodosAdicional.None;
            this.ReceiveFailure = false;
            this.Excepcion = string.Empty;
            this.Resultado = new object();
        }

        [DataMember]
        public MetodosAdicional Metodo { get; set; }

        [DataMember]
        public bool ReceiveFailure { get; set; }

        [DataMember]
        public bool Error { get; set; }

        [DataMember]
        public string Excepcion { get; set; }

        [DataMember]
        public object Resultado { get; set; }
    }

    [Serializable]
    [DataContract]
    public enum MetodosAdicional
    {
        [EnumMember]
        None = -1,

        [EnumMember]
        Ping,

        [EnumMember]
        Login,

        [EnumMember]
        CambiarFlujo,

        [EnumMember]
        EstadoFlujo,

        [EnumMember]
        ObtenerTipoDispensario,

        [EnumMember]
        ObtenerPorcentajes,

        [EnumMember]
        ObtenerPorcentajesPosicion,

        [EnumMember]
        EstablecerPorcentaje,

        [EnumMember]
        EstablecerPorcentajeGlobal,

        [EnumMember]
        ObtenerReporteVentasCombustible
    }
}
