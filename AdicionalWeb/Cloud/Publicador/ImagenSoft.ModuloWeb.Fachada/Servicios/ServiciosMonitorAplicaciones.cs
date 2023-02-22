using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia;

namespace ImagenSoft.ModuloWeb.Fachada
{
    public class ServiciosMonitorAplicaciones
    {
        public bool ModificarAplicaciones(Sesion sesion)
        {
            if (sesion.Aplicaciones == null) { return false; }
            if (sesion.Aplicaciones.Count <= 0) { return false; }

            MonitorAplicacionesPersistencia servicio = new MonitorAplicacionesPersistencia();
            MonitorAplicaciones entidad = new MonitorAplicaciones();
            {
                entidad.IdCliente = sesion.Clave;
                entidad.Estacion = sesion.NoCliente;
                entidad.IdDistribuidor = sesion.IdDistribuidor;

                entidad.Version = sesion.Aplicaciones.Version;
                entidad.MemoriaTotal = sesion.Aplicaciones.MemoriaEquipoTotal;
                entidad.SistemaOperativo = sesion.Aplicaciones.SistemaOperativo;
                entidad.MemoriaDisponible = sesion.Aplicaciones.MemoriaEquipoDisponible;

                sesion.Aplicaciones.ForEach(p =>
                    {
                        entidad.Detalle.Add(new MonitorAplicacionesDetalle()
                            {
                                Servicio = p.Nombre,
                                IdCliente = sesion.Clave,
                                Estacion = sesion.NoCliente,
                                MemoriaUsada = p.MemoriaUsada,
                                Observaciones = p.Observaciones
                            });
                    });
            }
            return servicio.ModificarInsertar(sesion, entidad);
        }
    }
}
