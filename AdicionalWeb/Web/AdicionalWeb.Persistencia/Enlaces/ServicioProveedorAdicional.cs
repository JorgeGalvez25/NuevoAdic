using System;
using System.ServiceModel;
using Adicional.Entidades;
using Servicios.Adicional;
using ServiciosCliente;
using System.Collections.Generic;

namespace AdicionalWeb.Persistencia.Enlaces
{
    public class ServicioProveedorAdicional
    {
        private void OperacionCliente(int estacion, Action<IServiciosCliente> fn)
        {
            System.ServiceModel.Channels.IChannelFactory<IServiciosCliente> cfCliente = new ChannelFactory<IServiciosCliente>("epCliente");
            IServiciosCliente servicio = cfCliente.CreateChannel(new EndpointAddress(string.Format("net.tcp://{0}/ServiciosCliente", EstacionesAdicionalPersistencia.ListaEstaciones[estacion].IpServicios)));

            try
            {
                fn(servicio);
            }
            finally
            {
                if (servicio != null)
                {
                    try
                    {
                        using (IClientChannel clientInstance = ((IClientChannel)servicio))
                        {
                            switch (clientInstance.State)
                            {
                                case System.ServiceModel.CommunicationState.Faulted:
                                    clientInstance.Abort();
                                    cfCliente.Abort();
                                    break;
                                case CommunicationState.Closed:
                                    break;
                                default:
                                    clientInstance.Close();
                                    cfCliente.Close();
                                    break;
                            }
                        }
                    }
                    catch { }
                    finally
                    {
                        servicio = null;
                        cfCliente = null;
                    }
                }
            }
        }
        private void OperacionAdicional(int estacion, Action<IServiciosAdicional> fn)
        {
            System.ServiceModel.Channels.IChannelFactory<IServiciosAdicional> cfAdicional = new ChannelFactory<IServiciosAdicional>("epAdicional");
            IServiciosAdicional servicio = cfAdicional.CreateChannel(new EndpointAddress(string.Format("net.tcp://{0}/ServiciosAdicional", EstacionesAdicionalPersistencia.ListaEstaciones[estacion].IpServicios)));
            try
            {
                fn(servicio);
            }
            finally
            {
                if (servicio != null)
                {
                    try
                    {
                        using (IClientChannel clientInstance = ((IClientChannel)servicio))
                        {
                            switch (clientInstance.State)
                            {
                                case System.ServiceModel.CommunicationState.Faulted:
                                    clientInstance.Abort();
                                    cfAdicional.Abort();
                                    break;
                                case CommunicationState.Closed:
                                    break;
                                default:
                                    clientInstance.Close();
                                    cfAdicional.Close();
                                    break;
                            }
                        }
                    }
                    catch { }
                    finally
                    {
                        servicio = null;
                        cfAdicional = null;
                    }
                }
            }
        }

        public Bitacora BitacoraInsertar(Bitacora bitacora, int estacion)
        {
            Bitacora result = null;
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.BitacoraInsertar(bitacora);
                });
            return result;
        }

        public Configuracion ConfiguracionObtener(int id, int estacion)
        {
            Configuracion result = null;
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.ConfiguracionObtener(id);
                });
            return result;
        }

        public bool ConfiguracionCambiarEstado(string std, int estacion)
        {
            bool result = false;
            this.OperacionAdicional(estacion, (servicio) =>
            {
                result = servicio.ConfiguracionCambiarEstado(std);
            });
            return result;
        }

        public Historial HistorialInsertar(Historial entidad, int estacion)
        {
            Historial result = null;
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.HistorialInsertar(entidad);
                });
            return result;
        }

        public Licencia LicenciaObtener(string modulo, int estacion)
        {
            Licencia result = null;
            this.OperacionAdicional(estacion, (servicio) =>
            {
                result = servicio.LicenciaObtener(modulo);
            });
            return result;
        }

        public bool LicenciaValida(Licencia licencia, string version, int estacion)
        {
            bool result = false;
            this.OperacionAdicional(estacion, (servicio) =>
            {
                result = servicio.LicenciaValida(licencia, version);
            });
            return result;
        }

        public List<int> HistorialObtenerPosiciones(int estacion)
        {
            List<int> result = new List<int>();
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.HistorialObtenerPosiciones(estacion);
                });
            return result;
        }

        public ListaHistorial HistorialObtenerPorPosicion(int estacion, int posicion)
        {
            ListaHistorial result = new ListaHistorial();
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.HistorialObtenerPorPosicion(estacion, posicion);
                });
            return result;
        }

        public ListaHistorial HistorialObtenerRecientes(int estacion)
        {
            ListaHistorial result = new ListaHistorial();
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.HistorialObtenerRecientes(estacion);
                });
            return result;
        }

        public string AplicarFlujo(bool std, MarcaDispensario marca, System.Collections.Generic.List<Adicional.Entidades.Historial> historial, int estacion)
        {
            string result = string.Empty;
            this.OperacionCliente(estacion, (servicio) =>
                {
                    result = servicio.AplicarFlujo(std, marca, historial);
                });
            return result;
        }

        public string SubirBajarFlujo(bool std, int estacion)
        {
            string result = string.Empty;
            this.OperacionCliente(estacion, (servicio) =>
                {
                    result = servicio.SubirBajarFlujo(std);
                });
            return result;
        }

        public DateTime ConfiguracionActualizarUltimoMovimiento(DateTime fecha, int estacion)
        {
            DateTime result = DateTime.MinValue;
            this.OperacionAdicional(estacion, (servicio) =>
            {
                result = servicio.ConfiguracionActualizarUltimoMovimiento(fecha);
            });
            return result;
        }

        public Estacion EstacionActualizar(Estacion entidad, int estacion)
        {
            Estacion result = null;
            this.OperacionAdicional(estacion, (servicio) =>
                {
                    result = servicio.EstacionActualizar(entidad);
                });
            return result;
        }
    }
}
