using System;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarClientesMdl
{
    public interface IVMAdministrarClientes
    {
        DateTime ObtenerFechaHoraServidor();

        bool Insertar(AdministrarClientes entidad);

        bool Modificar(AdministrarClientes entidad);

        bool Eliminar(FiltroAdministrarClientes filtro);

        AdministrarClientes Obtener(FiltroAdministrarClientes filtro);

        ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro);
    }
}