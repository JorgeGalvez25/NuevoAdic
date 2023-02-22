using ImagenSoft.ModuloWeb.Entidades;
namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public interface IVMAdministrarDistribuidores
    {
        int Consecutivo();

        AdministrarDistribuidores Insertar(AdministrarDistribuidores entidad);

        AdministrarDistribuidores Modificar(AdministrarDistribuidores entidad);

        AdministrarDistribuidores Obtener(FiltroAdministrarDistribuidores filtro);
    }
}
