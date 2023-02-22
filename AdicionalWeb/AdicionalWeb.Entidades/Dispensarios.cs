using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Dispensarios
    {
        public Dispensarios()
        {
            this.nombre = string.Empty;
            this.fecha = DateTime.MinValue;
            this.showTitleId = true;
        }

        public int id { get; set; }

        public int dispensario { get; set; }

        public int posicion { get; set; }

        public string nombre { get; set; }

        public double valor { get; set; }

        public int combustible { get; set; }

        public int estacion { get; set; }

        public DateTime fecha { get; set; }

        public bool showTitleId { get; set; }
    }

    [Serializable]
    public class ListaDispensarios : List<Dispensarios>
    {
        ~ListaDispensarios()
        {
            this.Clear();
        }
    }

    [Serializable]
    public class FiltroDispensarios
    {
        public int id { get; set; }

        public int dispensario { get; set; }

        public int Estacion { get; set; }
    }
}
