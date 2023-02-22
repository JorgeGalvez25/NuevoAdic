
namespace Adicional.Entidades
{
    public class Licencia
    {
        public static string ClabeAutor = "CVL5";
        public static string ClaveBoton = "CVL501";
        public static string ClaveMovil = "CVL502";
        public static string ClaveWeb = "CVL503";
        public static string Version = "3.1";

        public string Razon_social { get; set; }
        public string Sistema { get; set; }
        //public string Version { get; set; }
        public string TipoLicencia { get; set; }
        public string ClaveAutor { get; set; }
        public string Modulo { get; set; }
        public int Usuarios { get; set; }
        public string Estemporal { get; set; }
        public string Fecha_vence { get; set; }
        public bool Valida { get; set; }

        public Licencia()
        {
            this.Razon_social = string.Empty;
            this.TipoLicencia = string.Empty;
            this.ClaveAutor = string.Empty;
            this.Usuarios = 0;
            this.Modulo = string.Empty;
            this.Estemporal = string.Empty;
            this.Fecha_vence = string.Empty;
        }

        public Licencia Clone()
        {
            Licencia pResult = new Licencia();

            pResult.Razon_social = this.Razon_social;
            pResult.TipoLicencia = this.TipoLicencia;
            pResult.ClaveAutor = this.ClaveAutor;
            pResult.Modulo = this.Modulo;
            pResult.Usuarios = this.Usuarios;
            pResult.Estemporal = this.Estemporal;
            pResult.Fecha_vence = this.Fecha_vence;

            return pResult;
        }

        public int CompareTo(Licencia ALicencia)
        {

            if (this.Razon_social.CompareTo(ALicencia.Razon_social) != 0) return 1;
            if (this.TipoLicencia.CompareTo(ALicencia.TipoLicencia) != 0) return 1;
            if (this.ClaveAutor.CompareTo(ALicencia.ClaveAutor) != 0) return 1;
            if (this.Usuarios.CompareTo(ALicencia.Usuarios) != 0) return 1;
            if (this.Modulo.CompareTo(ALicencia.Modulo) != 0) return 1;
            if (this.Estemporal.CompareTo(ALicencia.Estemporal) != 0) return 1;
            if (this.Fecha_vence.CompareTo(ALicencia.Fecha_vence) != 0) return 1;

            return 0;
        }

        public string ToString(string modulo)
        {
            return string.Format("\"{0}|{1}|3.1|Abierta|{2}|{3}|{4}|{5}\"",
                                 this.Razon_social,
                                 modulo,
                                 this.ClaveAutor,
                                 Usuarios,
                                 (this.Estemporal.Equals("Si", System.StringComparison.CurrentCultureIgnoreCase) ? "True" : "False"),
                                 string.IsNullOrEmpty(this.Fecha_vence) ? "01/01/1800" : this.Fecha_vence);
        }
    }
}
