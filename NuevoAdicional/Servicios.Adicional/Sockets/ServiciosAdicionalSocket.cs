using Adicional.Entidades;
using Adicional.Entidades.Web;
using Persistencia;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.Adicional.Sockets
{
    public class ServiciosAdicionalSocket
    {
        internal byte[] ProcesarPeticion(SolicitudAdicional peticion)
        {
            switch (peticion.Metodo)
            {
                case MetodosAdicional.Ping: return Serializador.Serializar(true, ProtocoloSerializacion.Socket);

                case MetodosAdicional.EstadoFlujo: return Serializador.Serializar(this.ObtenerEstatus(), ProtocoloSerializacion.Socket);

                case MetodosAdicional.ObtenerTipoDispensario: return Serializador.Serializar(this.TipoDispensario(), ProtocoloSerializacion.Socket);

                case MetodosAdicional.CambiarFlujo:

                    FiltroCambiarFlujo cfFiltro = Serializador.Deserializar<FiltroCambiarFlujo>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.CambiarFlujo(cfFiltro), ProtocoloSerializacion.Socket);

                case MetodosAdicional.ObtenerPorcentajes:

                    FiltroMangueras opFiltro = Serializador.Deserializar<FiltroMangueras>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.ObtenerPorcentajes(opFiltro), ProtocoloSerializacion.Socket);

                case MetodosAdicional.ObtenerPorcentajesPosicion:

                    FiltroMangueras oppFiltro = Serializador.Deserializar<FiltroMangueras>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.ObtenerPorcentajes(oppFiltro.Estacion.Id), ProtocoloSerializacion.Socket);

                case MetodosAdicional.EstablecerPorcentaje:

                    FiltroMangueras epFiltro = Serializador.Deserializar<FiltroMangueras>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.EstablecerPorcentaje(epFiltro, false), ProtocoloSerializacion.Socket);

                case MetodosAdicional.EstablecerPorcentajeGlobal:

                    FiltroMangueras epgFiltro = Serializador.Deserializar<FiltroMangueras>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.EstablecerPorcentaje(epgFiltro, true), ProtocoloSerializacion.Socket);

                case MetodosAdicional.ObtenerReporteVentasCombustible:

                    FiltroReporteVentasCombustible rptFiltro = Serializador.Deserializar<FiltroReporteVentasCombustible>(peticion.Parametro as byte[], ProtocoloSerializacion.Socket);
                    return Serializador.Serializar(this.ObtenerReporteVentasCombustible(rptFiltro), ProtocoloSerializacion.Socket);

                    break;

                case MetodosAdicional.None:
                default:
                    break;
            }

            return null;
        }

        public MarcaDispensario TipoDispensario()
        {
            ServiciosAdicional servicio = new ServiciosAdicional();
            return servicio.TipoDispensarioCloud();
        }

        private string ObtenerEstatus()
        {
            return new ServiciosCliente.ServiciosCliente().ObtenerEstatus();
        }

        private string CambiarFlujo(FiltroCambiarFlujo filtro)
        {
            return new ServiciosAdicional().SubirBajarFlujoCloud(filtro.Usuario, filtro.Estandar);
        }

        private List<int> ObtenerPorcentajes(int claveEstacion)
        {
            return new HistorialPersistencia().HistorialObtenerPosiciones(claveEstacion);
        }

        private ListaHistorial ObtenerPorcentajes(FiltroMangueras filtro)
        {
            ServiciosAdicional servicio = new ServiciosAdicional();
            return servicio.HistorialObtenerTodos(1, filtro.Posicion);
            //ListaHistorial result = new ListaHistorial();
            //ListaHistorial aux = null;
            //List<int> posiciones = servicio.HistorialObtenerPosiciones(1);
            //posiciones.Where(p => p == filtro.Posicion || filtro.Posicion == 0).ToList().ForEach(p =>
            //    {
            //        aux = servicio.HistorialObtenerPorPosicion(1, p);
            //        if (aux != null && aux.Count > 0)
            //        {
            //            result.AddRange(aux);
            //        }
            //    });

            //return result;
        }

        private bool EstablecerPorcentaje(FiltroMangueras filtro, bool esGlobal)
        {
            ServiciosAdicional servicio = new ServiciosAdicional();
            return servicio.EstablecerPorcentajeCloud(filtro.Usuario, filtro.Historial, esGlobal);
        }

        private List<ReporteVentasCombustible> ObtenerReporteVentasCombustible(FiltroReporteVentasCombustible filtro)
        {
            ServiciosAdicional servicio = new ServiciosAdicional();
            return servicio.ObtenerReporteVentasCombustible(filtro);
        }
    }
}