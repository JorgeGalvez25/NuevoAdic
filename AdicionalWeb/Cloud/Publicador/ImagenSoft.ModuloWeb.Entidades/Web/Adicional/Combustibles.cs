using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagenSoft.ServiciosWeb.Entidades.Web.Adicional
{
    [Serializable]
    public class Combustibles
    {
        public Combustibles()
        {
            this.Combustible = string.Empty;
        }

        public int Id { get; set; }

        public string Combustible { get; set; }
    }

    [Serializable]
    public class ListaCombustible : List<Combustibles>
    {
        ~ListaCombustible()
        {
            this.Clear();
        }
    }

    [Serializable]
    public class FiltroCombustible
    {
        public int Id { get; set; }
    }
}
