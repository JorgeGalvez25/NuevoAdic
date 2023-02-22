using HostPublicador.Servicios;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Proveedor.Publicador;
using ImagenSoft.ModuloWeb.Servicios.WCF.Sockets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceProcess;

namespace HostPublicador
{
    public partial class HostPublicador : ServiceBase
    {
        public HostPublicador()
        {
            this.InitializeComponent();
        }

        internal string TituloMensaje = "Host Modulo Web - {0}";

        protected override void OnStart(string[] args)
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("protected override void OnStart(string[] args)"))
            {
                try
                {
                    _log.LogMessage(TituloMensaje, "Iniciando servicio.");
                    if (!iniciliazarWCF())
                    {
                        this.Stop();
                        return;
                    }
                    WorkItem.Objetos<ServiciosModuloWebProveedor>.Add(new ServiciosModuloWebProveedor(new SesionModuloWeb() { Nombre = "Host Modulo Web", Sistema = "MW" }, TipoConexionUsuario.Monitor));

                    if (!inicializarSocket())
                    {
                        this.Stop();
                        return;
                    }

                    if (!inicializarSocketBidi())
                    {
                        this.Stop();
                        return;
                    }
                    if (!inicializarServicioPing())
                    {
                        this.Stop();
                        return;
                    }
                    _log.LogMessage(TituloMensaje, "Servicios iniciados.");
                }
                catch (Exception e)
                {
                    _log.LogException(e);
                    this.Stop();
                }
            }
        }

        private bool iniciliazarWCF()
        {
            bool result = false;
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("private void iniciliazarWCF()"))
            {
                try
                {
                    List<ServiceHost> ListHost = new List<ServiceHost>();

                    ListHost.AddRange(new ServiceHost[] 
                    { 
                        new ServiceHost(typeof(ImagenSoft.ModuloWeb.Servicios.WCF.ServicioModuloWeb)),
                        new ServiceHost(typeof(ImagenSoft.ModuloWeb.Servicios.WCF.ServiciosWebPerform)),
                        new ServiceHost(typeof(ImagenSoft.ModuloWeb.Servicios.WCF.ServiciosModuloWebAdicional))
                    });

                    //ServiceHost host =null ;
                    ListHost.ForEach(host =>
                    {
                        foreach (ServiceEndpoint ep in host.Description.Endpoints)
                        {
                            BindingElementCollection elements = ep.Binding.CreateBindingElements();

                            if (ep.ListenUri.ToString().EndsWith("Perform"))
                            {
                                TcpTransportBindingElement transport = elements.Find<TcpTransportBindingElement>();
                                if (transport != null)
                                {
                                    transport.ConnectionPoolSettings.IdleTimeout = new TimeSpan(0, 0, 30);
                                    transport.ConnectionPoolSettings.LeaseTimeout = new TimeSpan(0, 0, 30);
                                    transport.ConnectionPoolSettings.MaxOutboundConnectionsPerEndpoint = 2;
                                }
                            }

                            if (ep.Address.Uri.Scheme.StartsWith("http"))
                            {
                                CustomBinding customBinding = new CustomBinding(ep.Binding);
                                HttpTransportBindingElement transportElement = customBinding.Elements.Find<HttpTransportBindingElement>();
                                transportElement.KeepAliveEnabled = false;
                                ep.Binding = customBinding;
                            }

                            ReliableSessionBindingElement reliableSessionElement = elements.Find<ReliableSessionBindingElement>();

                            if (reliableSessionElement != null)
                            {
                                reliableSessionElement.MaxPendingChannels = 16384;
                                CustomBinding newBinding = new CustomBinding(elements);
                                // Need to copy properties from existing binding to the new binding
                                // all other properties (e.g. security,encoder,transport) should be preserved and do not need to be copied
                                newBinding.CloseTimeout = ep.Binding.CloseTimeout;
                                newBinding.OpenTimeout = ep.Binding.OpenTimeout;
                                newBinding.ReceiveTimeout = ep.Binding.ReceiveTimeout;
                                newBinding.SendTimeout = ep.Binding.SendTimeout;
                                newBinding.Name = ep.Binding.Name;
                                newBinding.Namespace = ep.Binding.Namespace;
                                ep.Binding = newBinding;
                            }
                        }
                    });

                    ListHost.ForEach(item => { item.Open(); });
                    WorkItem.Objetos<List<ServiceHost>>.Add(ListHost);
                    result = true;
                    _log.LogMessage("Servicios WCF: Iniciados");
                }
                catch (Exception e)
                {
                    result = false;
                    _log.LogException(e);
                }
            }
            return result;
        }

        private bool inicializarSocketBidi()
        {
            bool result = false;
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("private void inicializarSocketBidi()"))
            {
                try
                {
                    SocketServerBidireccional servicioSocketBidi = new SocketServerBidireccional();
                    servicioSocketBidi.Start();
                    WorkItem.Objetos<SocketServerBidireccional>.Add(servicioSocketBidi);
                    result = true;
                    _log.LogMessage("Servicio Socket Bidireccional: Iniciado");
                }
                catch (Exception e)
                {
                    result = false;
                    _log.LogException(e);
                }
            }
            return result;
        }

        private bool inicializarSocket()
        {
            bool result = false;
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("private bool inicializarSocket()"))
            {
                try
                {
                    SocketServer servicioSocket = new SocketServer();
                    servicioSocket.Open();
                    WorkItem.Objetos<SocketServer>.Add(servicioSocket);
                    result = true;
                    _log.LogMessage("Servicio Socket: Iniciado");
                }
                catch (Exception e)
                {
                    result = false;
                    _log.LogException(e);
                }
            }
            return result;
        }

        //private void inicializarNotificadorDistribuidores()
        //{
        //    if ((ConfigurationManager.AppSettings["NotificarDistribuidores"] ?? "No").Equals("Si", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        NotificacionDistribuidores servicioDistribuidores = new NotificacionDistribuidores();
        //        {
        //            servicioDistribuidores.Iniciar();
        //            WorkItem.Objetos<NotificacionDistribuidores>.Add(servicioDistribuidores);
        //        }
        //    }
        //}

        private bool inicializarServicioPing()
        {
            bool result = false;

            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod("private bool inicializarServicioPing()"))
            {
                try
                {
                    ServiciosPing servicioPing = new ServiciosPing();

                    string srvPing = (ConfigurationManager.AppSettings["ServicioPing"] ?? "10");
                    int pingTime = 0;

                    if (!int.TryParse(srvPing, out pingTime))
                    {
                        pingTime = 10;
                    }

                    servicioPing.Start(new TimeSpan(0, pingTime, 0));
                    WorkItem.Objetos<ServiciosPing>.Add(servicioPing);
                    result = true;
                    _log.LogMessage("Servicio de Ping: Iniciado");
                }
                catch (Exception e)
                {
                    result = false;
                    _log.LogException(e);
                }
            }
            return result;
        }

        protected override void OnStop()
        {
            try
            {
                if (WorkItem.Objetos<List<ServiceHost>>.Exist())
                {
                    List<ServiceHost> ListHost = WorkItem.Objetos<List<ServiceHost>>.Get();

                    if (ListHost != null)
                    {
                        ListHost.ForEach(item =>
                            {
                                try { item.Close(); }
                                catch
                                {
                                    try { item.Abort(); }
                                    catch { }
                                }
                            });

                        ListHost.Clear();
                        ListHost = null;
                    }
                }

                if (WorkItem.Objetos<SocketServer>.Exist())
                {
                    WorkItem.Objetos<SocketServer>.Get().Close();
                }

                if (WorkItem.Objetos<SocketServerBidireccional>.Exist())
                {
                    WorkItem.Objetos<SocketServerBidireccional>.Get().Stop();
                }

                //if (WorkItem.Objetos<NotificacionDistribuidores>.Exist())
                //{
                //    NotificacionDistribuidores servicioDistribuidores = WorkItem.Objetos<NotificacionDistribuidores>.Get();
                //    if (servicioDistribuidores != null)
                //    {
                //        servicioDistribuidores.Detener();
                //        servicioDistribuidores.Dispose();
                //        servicioDistribuidores = null;
                //    }
                //}

                WorkItem.Clear();
            }
            finally
            {
                MensajesRegistros.Informacion(TituloMensaje, "Servicio terminado.");
            }
        }
    }
}
