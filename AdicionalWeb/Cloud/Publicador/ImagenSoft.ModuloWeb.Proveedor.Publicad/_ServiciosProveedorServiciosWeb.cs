using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Entidades.Enumeradores;
using ImagenSoft.ServiciosWeb.Entidades.Servicios;
using ImagenSoft.ServiciosWeb.Entidades.Servicios.Actualizador;
using ImagenSoft.ServiciosWeb.Interfaces.Publicador;
using ImagenSoft.ServiciosWeb.Proveedor.Conexion;
using System.ComponentModel;
using System.ServiceModel.Channels;

namespace ImagenSoft.ServiciosWeb.Proveedor.Publicador
{
    public class ServiciosProveedorServiciosWeb : IServiciosWeb,
                                                  IServiciosWebPerform,
                                                  IServiciosWebProveedor
    {
        private class ProxyServiciosWeb : ClientBase<IServiciosWeb>
        {
            public ProxyServiciosWeb(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }
            public IServiciosWeb GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        private class ProxyServiciosWebPerform : ClientBase<IServiciosWebPerform>
        {
            public ProxyServiciosWebPerform(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }
            public IServiciosWebPerform GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        #region Operaciones

        public Sesion Sesion;
        public ConfigCliente Configuracion { get; private set; }

        private const string TituloMensaje = "Proveedor - {0}";
        private TipoConexionUsuario _tipo;
        private ServiciosConexion serviciosConexion;

        public ServiciosProveedorServiciosWeb(Sesion sesion, TipoConexionUsuario tipo)
        {
            this._tipo = tipo;
            this.Sesion = sesion;
            if (serviciosConexion == null) { serviciosConexion = new ServiciosConexion(tipo); }
        }

        private void inicializarCanal(TipoConexionUsuario tipo)
        {
            if (serviciosConexion == null)
            {
                serviciosConexion = new ServiciosConexion(tipo);
            }
        }

        private void OperacionMonitor(SolicitudServiciosWeb solicitud, Action<RespuestaServiciosWeb> fn)
        {
            //var channel = serviciosConexion.GetChannelMonitor();
            //var servicio = channel.CreateChannel();
            var channel = new ProxyServiciosWeb(serviciosConexion.NetTcpBinding(), new EndpointAddress(serviciosConexion.HostMonitor.ToString()));
            var servicio = channel.GetChannel;

            try
            {
                var response = SerializadorServiciosWeb.Deserializar<RespuestaServiciosWeb>(servicio.EnviarPeticionTransmisor(SerializadorServiciosWeb.Serializar(solicitud)));
                if (response != null && !response.EsValido)
                {
                    MensajesRegistros.Error("Cliente Servicios Web", string.Format("{0}:\r\n{1}", solicitud.Metodo.ToString(), response.Mensaje));
                }
                fn(response);
            }
            catch (Exception e)
            {
                this.inicializarCanal(_tipo);
                MensajesRegistros.Error(string.Format(TituloMensaje, fn.Method.Name), e.Message);
                throw e;
            }
            finally
            {
                IClientChannel clientInstance = ((IClientChannel)servicio);
                if (clientInstance.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    try { clientInstance.Abort(); }
                    catch { }
                    try { channel.Abort(); }
                    catch { }
                }
                else if (clientInstance.State != System.ServiceModel.CommunicationState.Closed)
                {
                    try { clientInstance.Close(); }
                    catch { }
                    try { channel.Close(); }
                    catch { }
                }
            }
        }
        private void OperacionPerform(Action<IServiciosWebPerform> fn)
        {
            var channel = serviciosConexion.GetChannelPerform();
            var servicio = channel.CreateChannel();
            try
            {
                fn(servicio);
            }
            catch (Exception e)
            {
                this.inicializarCanal(_tipo);
                MensajesRegistros.Error(string.Format(TituloMensaje, fn.Method.Name), e.Message);
                throw e;
            }
            finally
            {
                IClientChannel clientInstance = ((IClientChannel)servicio);
                if (clientInstance.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    try { clientInstance.Abort(); }
                    catch { }
                    try { channel.Abort(); }
                    catch { }
                }
                else if (clientInstance.State != System.ServiceModel.CommunicationState.Closed)
                {
                    try { clientInstance.Close(); }
                    catch { }
                    try { channel.Close(); }
                    catch { }
                }
            }
        }

        #endregion

        #region Servicios

        #region Transacciones

        public MonitorTransaccion TrasaccionInsertar(Sesion sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccion result = null;

            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.TransaccionInsertar;
            }

            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (MonitorTransaccion)response.Resultado;
                });

            return result;
        }

        public MonitorTransaccion TransaccionModificar(Sesion sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccion result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.TransaccionModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (MonitorTransaccion)response.Resultado;
                });

            return result;
        }

        public bool TransaccionEliminar(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.TransaccionEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public MonitorTransaccion TransaccionObtener(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccion result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.TransaccionObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (MonitorTransaccion)response.Resultado;
                });

            return result;
        }

        public ListaMonitorTransaccion TransaccionObtenerTodosFiltro(Sesion sesion, FiltroMonitorTransaccion filtro)
        {
            ListaMonitorTransaccion result = new ListaMonitorTransaccion();

            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.TransaccionObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (ListaMonitorTransaccion)response.Resultado;
                });

            return result;
        }

        public void Transmitiendo(Sesion sesion, MonitorTransaccion entidad)
        {
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.Transmitiendo;
            }
            this.OperacionMonitor(solicitud, (response) => { });
        }

        #endregion

        #region Cambio de Precios

        public MonitorCambioPrecio CambioPrecioInsertar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecio result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.CambioPrecioInsertar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (MonitorCambioPrecio)response.Resultado;
            });
            return result;
        }

        public MonitorCambioPrecio CambioPrecioModificar(Sesion sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecio result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.CambioPrecioModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (MonitorCambioPrecio)response.Resultado;
                });
            return result;
        }

        public bool CambioPrecioEliminar(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.CambioPrecioEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });
            return result;
        }

        public MonitorCambioPrecio CambioPrecioObtener(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecio result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.CambioPrecioObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (MonitorCambioPrecio)response.Resultado;
            });
            return result;
        }

        public ListaMonitorCambioPrecio CambioPrecioObtenerTodosFiltro(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            ListaMonitorCambioPrecio result = new ListaMonitorCambioPrecio();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.CambioPrecioObtenerTodosFiltro;
            }

            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (ListaMonitorCambioPrecio)response.Resultado;
                });

            return result;
        }

        public void CambiarEstatus(Sesion sesion, FiltroMonitorCambioPrecio filtro)
        {
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.CambiarEstatus;
            }
            this.OperacionMonitor(solicitud, (servicio) => { });
        }

        #endregion

        #region Precios Gasolinas

        public int PreciosGasolinasConsecutivo(Sesion sesion)
        {
            int result = 0;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Metodo = Metodos.PreciosGasolinasConsecutivo;
            }

            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (int)response.Resultado;
            });

            return result;
        }

        public PreciosGasolinas PreciosGasolinasInsertar(Sesion sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinas result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.PreciosGasolinasInsertar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (PreciosGasolinas)response.Resultado;
            });

            return result;
        }

        public PreciosGasolinas PreciosGasolinasModificar(Sesion sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinas result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.PreciosGasolinasModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (PreciosGasolinas)response.Resultado;
            });

            return result;
        }

        public bool PreciosGasolinasEliminar(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.PreciosGasolinasEliminar;
            }

            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public PreciosGasolinas PreciosGasolinasObtener(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinas result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.PreciosGasolinasObtener;
            }

            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (PreciosGasolinas)response.Resultado;
            });

            return result;
        }

        public ListaPreciosGasolinas PreciosGasolinasObtenerTodosFiltro(Sesion sesion, FiltroPreciosGasolinas filtro)
        {
            ListaPreciosGasolinas result = new ListaPreciosGasolinas();

            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.PreciosGasolinasObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ListaPreciosGasolinas)response.Resultado;
            });

            return result;
        }

        #endregion

        #region Monitor Conexiones

        public MonitorConexiones MonitorConexionesInsertar(Sesion sesion, MonitorConexiones entidad)
        {
            MonitorConexiones result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.MonitorConexionesInsertar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (MonitorConexiones)response.Resultado;
            });
            return result;
        }

        public MonitorConexiones MonitorConexionesModificar(Sesion sesion, MonitorConexiones entidad)
        {
            MonitorConexiones result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.MonitorConexionesModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (MonitorConexiones)response.Resultado;
            });
            return result;
        }

        public bool MonitorConexionesEliminar(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.MonitorConexionesEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });
            return result;
        }

        public MonitorConexiones MonitorConexionesObtener(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexiones result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.MonitorConexionesObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (MonitorConexiones)response.Resultado;
            });
            return result;
        }

        public ListaMonitorConexiones MonitorConexionesObtenerTodosFiltro(Sesion sesion, FiltroMonitorConexiones filtro)
        {
            ListaMonitorConexiones result = new ListaMonitorConexiones();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.MonitorConexionesObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ListaMonitorConexiones)response.Resultado;
            });

            return result;
        }

        #endregion

        #region Sesion

        public ListaSesiones SesionObtenerTodosFiltro(Sesion sesion, FiltroSesion filtro)
        {
            ListaSesiones result = new ListaSesiones();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.SesionObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (ListaSesiones)response.Resultado;
                });

            return result;
        }

        #endregion

        public bool Ping()
        {
            bool result = false;
            this.OperacionPerform((servicio) =>
            {
                result = servicio.Ping();
            });
            return result;
        }

        public bool Ping(Sesion sesion)
        {
            if (!this.Sesion.Compare(sesion))
            {
                this.Sesion = sesion;
            }

            this.Sesion.FechaHoraCliente = DateTime.Now;

            bool result = false;
            this.OperacionPerform((servicio) =>
                {
                    result = servicio.Ping(sesion);
                });
            return result;
        }

        public byte[] GetConfig(byte[] request)
        {
            byte[] result = null;
            this.OperacionPerform((servicio) =>
            {
                result = servicio.GetConfig(request);
            });
            return result;
        }

        public ConfigCliente GetConfig(string NoEstacion)
        {
            return SerializadorServiciosWeb.Deserializar<ConfigCliente>(GetConfig(SerializadorServiciosWeb.Serializar(NoEstacion)));
        }

        public DateTime ObtenerFechaHoraServidor()
        {
            DateTime result = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Metodo = Metodos.ObtenerFechaHoraServidor;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (DateTime)response.Resultado;
                });
            return result;
        }

        public DateTime ObtenerFechaHoraCentralServidor()
        {
            DateTime result = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Metodo = Metodos.ObtenerFechaHoraCentralServidor;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (DateTime)response.Resultado;
                });
            return result;
        }

        #endregion

        #region Catalogos

        #region Administrar Clientes

        public int AdministrarClientesConsecutivo(Sesion sesion)
        {
            int result = 0;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Metodo = Metodos.AdministrarClientesConsecutivo;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (int)response.Resultado;
            });

            return result;
        }

        public AdministrarClientes AdministrarClientesInsertar(Sesion sesion, AdministrarClientes entidad)
        {
            AdministrarClientes result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarClientesInsertar;
            }

            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarClientes)response.Resultado;
            });

            return result;
        }

        public AdministrarClientes AdministrarClientesModificar(Sesion sesion, AdministrarClientes entidad)
        {
            AdministrarClientes result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarClientesModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarClientes)response.Resultado;
            });

            return result;
        }

        public bool AdministrarClientesEliminar(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarClientesEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public AdministrarClientes AdministrarClientesObtener(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            AdministrarClientes result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarClientesObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarClientes)response.Resultado;
            });

            return result;
        }

        public ListaAdministrarClientes AdministrarClientesObtenerTodosFiltro(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            ListaAdministrarClientes result = new ListaAdministrarClientes();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarClientesObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ListaAdministrarClientes)response.Resultado;
            });

            return result;
        }

        public bool ModificarUltimaConexion(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.ModificarUltimaConexion;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public bool ModificarFechaHoraCliente(Sesion sesion, FiltroAdministrarClientes filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.ModificarFechaHoraCliente;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        #endregion

        #region Administrar Usuarios

        public int AdministrarUsuariosConsecutivo(Sesion sesion)
        {
            int result = 0;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Metodo = Metodos.AdministrarUsuariosConsecutivo;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (int)response.Resultado;
            });

            return result;
        }

        public AdministrarUsuarios AdministrarUsuariosInsertar(Sesion sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuarios result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarUsuariosInsertar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarUsuarios)response.Resultado;
            });

            return result;
        }

        public AdministrarUsuarios AdministrarUsuariosModificar(Sesion sesion, AdministrarUsuarios entidad)
        {
            AdministrarUsuarios result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarUsuariosModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarUsuarios)response.Resultado;
            });

            return result;
        }

        public bool AdministrarUsuariosEliminar(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarUsuariosEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public AdministrarUsuarios AdministrarUsuariosObtener(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuarios result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarUsuariosObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarUsuarios)response.Resultado;
            });

            return result;
        }

        public ListaAdministrarUsuarios AdministrarUsuariosObtenerTodosFiltro(Sesion sesion, FiltroAdministrarUsuarios filtro)
        {
            ListaAdministrarUsuarios result = new ListaAdministrarUsuarios();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarUsuariosObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ListaAdministrarUsuarios)response.Resultado;
            });

            return result;
        }

        #endregion

        #region Administrar Distribuidores

        public int AdministrarDistribuidoresConsecutivo(Sesion sesion)
        {
            int result = 0;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresConsecutivo;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (int)response.Resultado;
                });

            return result;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresInsertar(Sesion sesion, AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresInsertar;
            }
            this.OperacionMonitor(solicitud, (response) =>
                {
                    result = (AdministrarDistribuidores)response.Resultado;
                });

            return result;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresModificar(Sesion sesion, AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = entidad;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresModificar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarDistribuidores)response.Resultado;
            });

            return result;
        }

        public bool AdministrarDistribuidoresEliminar(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            bool result = false;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresEliminar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (bool)response.Resultado;
            });

            return result;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresObtener(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidores result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresObtener;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (AdministrarDistribuidores)response.Resultado;
            });

            return result;
        }

        public ListaAdministrarDistribuidores AdministrarDistribuidoresObtenerTodosFiltro(Sesion sesion, FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores result = new ListaAdministrarDistribuidores();
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = sesion;
                solicitud.Parametro = filtro;
                solicitud.Metodo = Metodos.AdministrarDistribuidoresObtenerTodosFiltro;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ListaAdministrarDistribuidores)response.Resultado;
            });

            return result;
        }

        #endregion

        #endregion

        #region IServiciosWeb Members

        public byte[] EnviarPeticionTransmisor(byte[] solicitud)
        {
            return null;
        }

        #endregion

        #region Actualizador

        public ResponseUpdater Actualizar(RequestUpdater request)
        {
            ResponseUpdater result = null;
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = new Sesion();
                solicitud.Parametro = request;
                solicitud.Metodo = Metodos.Actualizar;
            }
            this.OperacionMonitor(solicitud, (response) =>
            {
                result = (ResponseUpdater)response.Resultado;
            });
            return result;
        }

        #endregion

        #region Proceso Envio Correo

        public void ProcesarNotificaciones()
        {
            SolicitudServiciosWeb solicitud = new SolicitudServiciosWeb();
            {
                solicitud.Sesion = new Sesion();
                solicitud.Metodo = Metodos.ProcesarNotificaciones;
            }
            this.OperacionMonitor(solicitud, (response) => { });
        }

        #endregion
    }
}
