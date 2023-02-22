using System;
using System.Collections.Generic;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Estaciones
    {
        public Estaciones()
        {
            this.Consola = string.Empty;
            this.Nombre = string.Empty;
        }

        public int Clave { get; set; }

        public string Nombre { get; set; }

        public string Consola { get; set; }

        public bool ConMembresia { get; set; }
    }

    [Serializable]
    public class ListaEstaciones : List<Estaciones>
    {
        ~ListaEstaciones()
        {
            this.Clear();
        }
    }

    [Serializable]
    public class FiltroEstaciones
    {
        public FiltroEstaciones()
        {
            this.ClaveConsola = string.Empty;
            this.Nombre = string.Empty;
        }

        public int Clave { get; set; }

        public string Nombre { get; set; }

        public string ClaveConsola { get; set; }
    }
}
