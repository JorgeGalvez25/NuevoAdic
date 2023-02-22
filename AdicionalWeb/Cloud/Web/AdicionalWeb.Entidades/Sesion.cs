using System;
using System.Collections.Generic;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Sesion
    {
        public Sesion()
        {
            this.Activo = string.Empty;
            this.Clave = string.Empty;
            this.Correo = string.Empty;
            this.Nombre = string.Empty;
            this.NoEstacion = string.Empty;
            this.Password = string.Empty;
            this.Variables = string.Empty;
        }

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string NoEstacion { get; set; }

        public string Password { get; set; }

        public string Clave { get; set; }

        public string Activo { get; set; }

        public string Variables { get; set; }

        public string Correo { get; set; }

        public Estaciones Estacion { get; set; }
    }

    [Serializable]
    public class ListaSesion : List<Sesion>
    {
        ~ListaSesion()
        {
            this.Clear();
        }
    }

    [Serializable]
    public class FiltroSesion
    {
        public FiltroSesion()
        {
            this.Activo = string.Empty;
            this.Nombre = string.Empty;
            this.NoEstacion = string.Empty;
            this.Password = string.Empty;
        }

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string NoEstacion { get; set; }

        public string Password { get; set; }

        public string Activo { get; set; }
    }
}
