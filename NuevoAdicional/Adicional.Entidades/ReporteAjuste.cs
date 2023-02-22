using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Adicional.Entidades
{
    [DataContract]
    [Serializable]
    public class ReporteAjuste
    {
        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int Combustible { get; set; }

        [DataMember]
        public string NombreCombustible { get; set; }

        [DataMember]
        public double InvInicial { get; set; }

        [DataMember]
        public double Entradas { get; set; }

        [DataMember]
        public double EntradasMermas { get; set; }

        [DataMember]
        public double EntradasFisicas { get; set; }

        [DataMember]
        public double InvFinal { get; set; }

        [DataMember]
        public double SalidaDispensarios { get; set; }

        [DataMember]
        public double Ventas { get; set; }

        [DataMember]
        public double Jarreos { get; set; }

        [DataMember]
        public double Ajuste { get; set; }

        [DataMember]
        public double Diferencia { get; set; }

        [DataMember]
        public double Precio { get; set; }

        [DataMember]
        public double PorcMerma { get; set; }

        [DataMember]
        public double Pendiente { get; set; }

        //[DataMember]
        //public double PorcentajeMerma { get; set; }

        [DataMember]
        public int Corte { get; set; }

        public double VentasAntes { get { return this.SalidaDispensarios - this.Jarreos; } }

        public double VentasDespues
        {
            get
            {
                var op = this.VentasAntes - this.Ajuste;
                return op;// -(op * this.PorcentajeMerma / 100);
            }
        }//this.LitrosMerma; } }
        //public double VentasDespues { get { return this.VentasAntes - this.Ajuste; } }

        public double SalidaTanques { get { return this.InvInicial + this.EntradasMermas - this.InvFinal; } }

        public double ImporteAntes { get { return this.VentasAntes * this.Precio; } }

        public double ImporteDespues { get { return this.VentasDespues * this.Precio; } }

        public double ImporteAjuste { get { return this.Ajuste * this.Precio; } }

        public double PorcentajeAjuste { get { return this.VentasAntes != 0 ? this.Ajuste / this.VentasAntes * 100 : 0D; } }

        public double PorcentajeMerma
        {
            get
            {
                return this.SalidaTanques != 0 ? (this.VentasDespues - this.SalidaTanques) / this.SalidaTanques * 100 : 0D;
            }
        }

        public double LitrosMerma { get { return (this.VentasAntes - this.Ajuste) * this.PorcentajeMerma / 100; } }
        //{ get { return this.SalidaTanques * this.PorcentajeMerma / 100; } }

        public double ImporteMerma { get { return this.LitrosMerma * this.Precio; } }

        public double DiferenciaVentas { get { return this.VentasDespues - (this.InvInicial + this.EntradasMermas - this.InvFinal); } }
    }
}
