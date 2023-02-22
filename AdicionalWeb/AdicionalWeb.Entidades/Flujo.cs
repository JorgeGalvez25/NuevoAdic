using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Flujo
    {
        public Flujo()
        {
            this.Aplicado = string.Empty;
            this.Comando = string.Empty;
            this.Modulo = string.Empty;
            this.Resultado = string.Empty;
        }

        public int Estacion { get; set; }

        public int Folio { get; set; }

        public string Modulo { get; set; }

        public DateTime FechaHora { get; set; }

        public string Comando { get; set; }

        public string Aplicado { get; set; }

        public string Resultado { get; set; }
    }
}
