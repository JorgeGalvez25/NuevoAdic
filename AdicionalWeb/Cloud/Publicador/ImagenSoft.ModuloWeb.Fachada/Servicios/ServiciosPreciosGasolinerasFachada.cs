using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosPreciosGasolinerasFachada
    {
        public int Consecutivo(Sesion sesion)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.Consecutivo(sesion);
        }

        public PreciosGasolinas Insertar(Sesion sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.Insertar(sesion, entidad);
        }

        public PreciosGasolinas Modificar(Sesion sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.Modificar(sesion, entidad);
        }

        public bool Eliminar(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.Eliminar(sesion, filtro);
        }

        public PreciosGasolinas Obtener(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.Obtener(sesion, filtro);
        }

        public ListaPreciosGasolinas ObtenerTodosFiltro(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinasPersistencia servicio = new PreciosGasolinasPersistencia();
            return servicio.ObtenerTodosFiltro(sesion, filtro);
        }

        public bool ClienteValidoCambioPrecios(Sesion sesion)
        {
            bool existe = false;
            if (MonitorCambioPrecioPersistencia.ClientesValidos.ContainsKey(sesion.NoCliente))
            {
                // True si es valido, False si no lo es.
                // Pero se cambia para que cambie el resultado y sea valida la respuesta de respuesta.
                existe = !MonitorCambioPrecioPersistencia.ClientesValidos[sesion.NoCliente];
            }

            return (PreciosGasolinasPersistencia.HayCambioDePrecio && !existe);
        }
    }
}
