using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Consola.Logic.Entities
{
    public class DPVGTCMB
    {
        public DPVGTCMB()
        {
            this.AgruparCon =
                this.Clave =
                this.DigitoAjustePrecio = 0;
            this.ClavePEMEX =
                this.Con_ProductoPrecio =
                this.Nombre =
                this.Tag =
                this.Tag2 =
                this.Tag3 = string.Empty;
            this.PrecioFisico = 0D;
        }

        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string ClavePEMEX { get; set; }
        public string Con_ProductoPrecio { get; set; }
        public double PrecioFisico { get; set; }
        public int AgruparCon { get; set; }
        public int DigitoAjustePrecio { get; set; }
        public string Tag { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
    }

    public class ListaDPVGTCMB : List<DPVGTCMB>
    {
        ~ListaDPVGTCMB()
        {
            if (this != null)
            {
                this.Clear();
            }
        }
    }

    public class FiltroDPVGTCMB
    {
        public int Clave { get; set; }
    }
}
