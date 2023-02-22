using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.SesionMdl
{
    public interface IVSesion
    {
        bool Ping();

        bool ValidarContrasena(string pass1, string pass2);

        ListaSesiones ObtenerTodosFiltro(FiltroSesionModuloWeb f);
    }
}

