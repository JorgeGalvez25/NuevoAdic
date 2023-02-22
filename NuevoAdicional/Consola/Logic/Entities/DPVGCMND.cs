using System;
using System.Collections.Generic;

namespace Consola.Logic.Entities
{
    public class DPVGCMND
    {
        public DPVGCMND()
        {
            this.Aplicado =
                this.Comando =
                this.Modulo =
                this.Resultado = string.Empty;
            this.FechaHora = DateTime.MinValue;
            this.Folio = 0;
        }

        public int Folio { get; set; }
        public string Modulo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Comando { get; set; }
        public string Aplicado { get; set; }
        public string Resultado { get; set; }

        public DPVGCMND Clonar()
        {
            return (DPVGCMND)this.MemberwiseClone();
        }
    }

    public class ListaDPVGCMND : List<DPVGCMND>
    {
        ~ListaDPVGCMND()
        {
            if (this != null)
            {
                this.Clear();
            }
        }
    }

    public class FiltroDPVGCMND
    {
        public FiltroDPVGCMND()
        {
            this.Aplicado =
                this.Modulo =
                this.Resultado = string.Empty;
            this.FechaHora = DateTime.MinValue;
            this.Folio = 0;
        }

        public int Folio { get; set; }
        public string Modulo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Aplicado { get; set; }
        public string Resultado { get; set; }
    }
}
