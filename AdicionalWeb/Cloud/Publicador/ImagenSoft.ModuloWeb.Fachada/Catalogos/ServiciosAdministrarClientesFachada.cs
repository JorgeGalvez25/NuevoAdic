using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosAdministrarClientesFachada
    {
        public bool ModificarFechaHoraCliente(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            return servicio.ModificarFechaHoraCliente(sesion, filtro);
        }
    }
}
