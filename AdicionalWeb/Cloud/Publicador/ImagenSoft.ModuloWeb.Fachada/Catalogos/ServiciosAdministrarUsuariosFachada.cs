using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosAdministrarUsuariosFachada
    {
        public int Consecutivo(Sesion sesion)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.Consecutivo(sesion);
        }

        public AdministrarUsuarios Insertar(Sesion sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.Insertar(sesion, entidad);
        }

        public AdministrarUsuarios Modificar(Sesion sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.Modificar(sesion, entidad);
        }

        public bool Eliminar(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.Eliminar(sesion, filtro);
        }

        public AdministrarUsuarios Obtener(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.Obtener(sesion, filtro);
        }

        public ListaAdministrarUsuarios ObtenerTodosFiltro(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuariosPersistencia servicio = new AdministrarUsuariosPersistencia();
            return servicio.ObtenerTodosFiltro(sesion, filtro);
        }
    }
}
