using System;
using System.Runtime.CompilerServices;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Licencia
    {
        public Licencia()
        {
            this.ClaveActualizacion = string.Empty;
            this.RazonSocial = string.Empty;
            this.Revision = string.Empty;
        }

        public int Clave { get; set; }

        public string RazonSocial { get; set; }

        public string Revision { get; set; }

        public string ClaveActualizacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public static DateTime FechaLiberacion { [MethodImpl(MethodImplOptions.Synchronized)]get { return new DateTime(2015, 1, 1); } }

        public override string ToString()
        {
            return string.Format("licenciacongruente \"{0}|CVAC|3.1|Abierta|{1}|1|True|{2:dd/MM/yyyy}\"", this.RazonSocial, this.ClaveActualizacion, this.FechaActualizacion);
        }
    }

    [Serializable]
    public class FiltroLicencia
    {
        public FiltroLicencia()
        {
            this.RazonSocial = string.Empty;
        }

        public int Clave { get; set; }

        public string RazonSocial { get; set; }
    }
}
