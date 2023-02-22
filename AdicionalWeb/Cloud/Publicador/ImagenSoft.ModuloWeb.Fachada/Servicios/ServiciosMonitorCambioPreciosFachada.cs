using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosMonitorCambioPreciosFachada
    {
        public MonitorCambioPrecio Insertar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            return servicio.Insertar(sesion, entidad);
        }

        public MonitorCambioPrecio Modificar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            return servicio.Modificar(sesion, entidad);
        }

        public bool Eliminar(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            return servicio.Eliminar(sesion, filtro);
        }

        public MonitorCambioPrecio Obtener(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            return servicio.Obtener(sesion, filtro);
        }

        public ListaMonitorCambioPrecio ObtenerTodosFiltro(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            return servicio.ObtenerTodosFiltro(sesion, filtro);
        }

        public void CambiarEstatus(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecioPersistencia servicio = new MonitorCambioPrecioPersistencia();
            servicio.CambiarEstatus(sesion, filtro);
        }
    }
}
