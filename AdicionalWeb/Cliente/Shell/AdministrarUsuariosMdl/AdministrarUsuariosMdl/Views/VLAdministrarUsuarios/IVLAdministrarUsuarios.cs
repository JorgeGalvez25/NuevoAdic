using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public interface IVLAdministrarUsuarios
    {
        int Consecutivo();

        bool Eliminar(FiltroAdministrarUsuarios filtro);

        ListaAdministrarUsuarios ObtenerTodos(FiltroAdministrarUsuarios filtro);

        ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro);
    }
}

