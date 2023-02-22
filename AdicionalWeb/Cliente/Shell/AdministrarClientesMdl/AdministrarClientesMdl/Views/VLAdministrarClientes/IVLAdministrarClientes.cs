using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarClientesMdl
{
    public interface IVLAdministrarClientes
    {
        int Consecutivo();

        bool Insertar(AdministrarClientes entidad);

        bool Eliminar(FiltroAdministrarClientes filtro);

        ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro);

        ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro);
    }
}

