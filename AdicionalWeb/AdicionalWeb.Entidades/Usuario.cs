using System;
using System.Collections.Generic;

namespace AdicionalWeb.Entidades
{
    [Serializable]
    public class Usuario
    {
        public Usuario()
        {
            this.Correo = string.Empty;
            this.Nombre = string.Empty;
            this.Password = string.Empty;
        }

        public string Nombre { get; set; }

        public string Password { get; set; }

        public string Correo { get; set; }
    }

    [Serializable]
    public class ListaUsuario : List<Usuario>
    {
        ~ListaUsuario()
        {
            this.Clear();
        }
    }

    [Serializable]
    public class FiltroUsuario
    {
        public FiltroUsuario()
        {
            this.Correo = string.Empty;
            this.Nombre = string.Empty;
        }

        public string Nombre { get; set; }

        public string Correo { get; set; }
    }
}
