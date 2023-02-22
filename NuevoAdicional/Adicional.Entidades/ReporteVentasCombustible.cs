using System;
using System.Xml.Serialization;

namespace Adicional.Entidades
{
    [Serializable]
    public class ReporteVentasCombustible
    {
        public ReporteVentasCombustible()
        {
            this.Descripcion = string.Empty;
            this.HexHash = string.Empty;
        }

        [XmlIgnore]
        public int Combustible { get; set; }

        public string Descripcion { get; set; }

        [XmlIgnore]
        public string HexHash { get; set; }

        [XmlIgnore]
        public decimal Precio { get; set; }

        public decimal Volumen { get; set; }

        public decimal Importe { get; set; }
    }

    [Serializable]
    public enum TipoVentasCombustible
    {
        None,
        VentasReales,
        VentasAjustadas,
        AjustePorCombustible
    }

    [Serializable]
    public class FiltroReporteVentasCombustible
    {
        public FiltroReporteVentasCombustible()
        {
            this.NoEstacion = string.Empty;
            this.Tipo = TipoVentasCombustible.None;
        }

        public string NoEstacion { get; set; }

        public DateTime Fecha { get; set; }

        public TipoVentasCombustible Tipo { get; set; }
    }
}
