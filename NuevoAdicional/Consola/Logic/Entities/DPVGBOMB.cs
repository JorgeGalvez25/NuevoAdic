using System.Collections.Generic;

namespace Consola.Logic.Entities
{
    public class DPVGBOMB
    {
        public DPVGBOMB()
        {
            this.Activo =
                this.Impresora =
                this.ImprimeAutom =
                this.ModoOperacion = string.Empty;
            this.Combustible =
                this.Con_DigitoAjuste =
                this.Con_Posicion =
                this.Con_Precio =
                this.DecimalesGilbarco =
                this.DigitoAjustePrecio =
                this.DigitoAjustePreset =
                this.DigitoAjusteVol =
                this.DigitosGilbarco =
                this.Isla =
                this.Manguera =
                this.Poscarga =
                this.Tanque = 0;
        }

        public int Manguera { get; set; }
        public int Poscarga { get; set; }
        public int Combustible { get; set; }
        public int Isla { get; set; }
        public int Con_Precio { get; set; }
        public int Con_Posicion { get; set; }
        public int Con_DigitoAjuste { get; set; }
        public string Impresora { get; set; }
        public string Activo { get; set; }
        public string ImprimeAutom { get; set; }
        public int DigitoAjustePrecio { get; set; }
        public string ModoOperacion { get; set; }
        public int Tanque { get; set; }
        public int DigitosGilbarco { get; set; }
        public int DecimalesGilbarco { get; set; }
        public int DigitoAjusteVol { get; set; }
        public int DigitoAjustePreset { get; set; }
    }

    public class ListaDPVGBOMB : List<DPVGBOMB>
    {
        ~ListaDPVGBOMB()
        {
            if (this != null)
            {
                this.Clear();
            }
        }
    }

    public class FiltroDPVGBOMB
    {
        public FiltroDPVGBOMB()
        {
            this.Activo = string.Empty;
            this.Combustible =
                this.Isla =
                this.Manguera =
                this.Poscarga = 0;
        }

        public int Manguera { get; set; }
        public int Poscarga { get; set; }
        public int Combustible { get; set; }
        public int Isla { get; set; }
        public string Activo { get; set; }
    }
}
