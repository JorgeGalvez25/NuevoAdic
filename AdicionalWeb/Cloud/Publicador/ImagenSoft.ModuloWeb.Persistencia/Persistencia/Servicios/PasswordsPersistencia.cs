using ImagenSoft.ModuloWeb.Entidades;
using System;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia.Servicios
{
    public class PasswordsPersistencia
    {
        public string ObtenerNuevaContraseniaHash()
        {
            return Utilerias.GetMD5(ObtenerNuevaContrasenia());
        }

        public string ObtenerNuevaContrasenia()
        {
            string auxi = Guid.NewGuid()
                              .ToString()
                              .Replace("-", string.Empty);
            return auxi.Substring(auxi.Length - 6, 6);
        }
    }
}
