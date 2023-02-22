using System.Collections.Generic;
using System;

namespace Consola.Logic.Entities
{
    public class DPVGESTS
    {
        public DPVGESTS()
        {
            this.Clave =
                this.TipoDispensario =
                this.TipoInterface =
                this.TipoTanques = 0;

            this.Consola =
                this.Nombre =
                this.NumeroEstacion = string.Empty;
        }

        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string Consola { get; set; }
        public int TipoDispensario { get; set; }
        public int TipoTanques { get; set; }
        public string NumeroEstacion { get; set; }
        public int TipoInterface { get; set; }

        public DPVGESTS Clonar()
        {
            return (DPVGESTS)this.MemberwiseClone();
        }

        public Dictionary<string, string> GetConsola()
        {
            if (string.IsNullOrEmpty(this.Consola)) { return null; }

            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] splt = this.Consola.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] aux = null;
            Array.ForEach(splt, p =>
                {
                    aux = p.Split(new char[] { '=' });
                    result.Add(aux[0].Trim(), aux[1].Trim());
                });

            return result;
        }
    }

    public class ListaDPVGESTS : List<DPVGESTS>
    {
        ~ListaDPVGESTS()
        {
            if (this != null)
            {
                this.Clear();
            }
        }
    }

    public class FiltroDPVGESTS
    {
        public FiltroDPVGESTS()
        {
            this.Clave =
                this.TipoDispensario =
                this.TipoInterface =
                this.TipoTanque = 0;
        }

        public int Clave { get; set; }
        public int TipoDispensario { get; set; }
        public int TipoTanque { get; set; }
        public int TipoInterface { get; set; }
    }
}
