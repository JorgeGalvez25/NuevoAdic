using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public interface IVLAdministrarDistribuidores
    {
        bool Eliminar(FiltroAdministrarDistribuidores filtro);

        ListaAdministrarDistribuidores ObtenerTodosFiltro(FiltroAdministrarDistribuidores filtro);
    }
}

