using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosSesionFachada
    {
        public ListaSesiones ObtenerTodosFiltro(Sesion sesion, FiltroSesion filtro)
        {
            SesionPersistencia servicio = new SesionPersistencia();
            return servicio.ObtenerTodosFiltro(sesion, filtro);
        }
    }
}
