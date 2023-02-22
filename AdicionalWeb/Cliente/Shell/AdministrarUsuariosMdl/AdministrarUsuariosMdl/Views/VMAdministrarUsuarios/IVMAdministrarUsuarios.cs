using System;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public interface IVMAdministrarUsuarios
    {
        DateTime ObtenerFechaHoraServidor();

        int Consecutivo();

        bool Insertar(AdministrarUsuarios entidad);

        bool Modificar(AdministrarUsuarios entidad);

        AdministrarUsuarios Obtener(FiltroAdministrarUsuarios filtro);

        ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro);
    }
}

