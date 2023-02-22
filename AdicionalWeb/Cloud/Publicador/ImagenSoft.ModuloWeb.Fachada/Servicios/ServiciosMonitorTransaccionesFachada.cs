using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosMonitorTransaccionesFachada
    {
        public MonitorTransaccion Insertar(Sesion sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            return servicio.Insertar(sesion, entidad);
        }

        public MonitorTransaccion Modificar(Sesion sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            return servicio.Modificar(sesion, entidad);
        }

        public bool Eliminar(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            return servicio.Eliminar(sesion, filtro);
        }

        public MonitorTransaccion Obtener(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            return servicio.Obtener(sesion, filtro);
        }

        public ListaMonitorTransaccion ObtenerTodosFiltro(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
            return servicio.ObtenerTodosFiltro(sesion, filtro);
        }
    }
}
