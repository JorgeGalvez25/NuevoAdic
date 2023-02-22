using Adicional.Entidades;
using Adicional.Entidades.SocketBidireccional;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Servicios;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using ImagenSoft.ModuloWeb.Proveedor.Conexion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace ImagenSoft.ModuloWeb.Proveedor.Publicador
{
    //public class ServiciosProveedorAdicionalWeb : IServiciosAdicionalProveedor
    public class ServiciosModuloWebProveedor : IModuloWebProveedor,
                                               IModuloWebPerform
    {
        #region Propiedades

        public SesionModuloWeb Sesion;
        public ConfigCliente Configuracion { get; private set; }

        private const string TituloMensaje = "Proveedor ModuloWeb - {0}";

        private TipoConexionUsuario _tipo;
        private ServiciosConexion serviciosConexion;

        #endregion

        #region Proxy

        private class ProxyServiciosWeb : ClientBase<IModuloWebAdicional>
        {
            public ProxyServiciosWeb(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }

            public IModuloWebAdicional GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        private class ProxyModuloWeb : ClientBase<IModuloWeb>
        {
            public ProxyModuloWeb(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }

            public IModuloWeb GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        private class ProxyServiciosWebPerform : ClientBase<IModuloWebPerform>
        {
            public ProxyServiciosWebPerform(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
            {
            }
            public IModuloWebPerform GetChannel
            {
                get
                {
                    return base.Channel;
                }
            }
        }

        public class ProxySocketProveedor
        {
            string host = string.Empty;
            int puerto = 0;
            int maxBufferSize = 0;
            int maxTimeout = 0;
            private StateObjectBidireccional m_state;
            private static byte[] SocketEnd = new byte[] { 60, 33, 45, 45, 69, 78, 68, 45, 45, 62 };

            public ProxySocketProveedor()
                : this("127.0.0.1", 808)
            {

            }

            public ProxySocketProveedor(string ip, int port)
                : this(ip, port, 1048576, 300000)
            {

            }

            public ProxySocketProveedor(string host, int puerto, int maxBufferSize, int maxTimeout)
            {
                this.host = host;
                this.puerto = puerto;
                this.maxBufferSize = maxBufferSize;
                this.maxTimeout = maxTimeout;
            }

            public ProxySocketProveedor(StateObjectBidireccional state)
            {
                this.m_state = state;
            }

            #region Public

            [MethodImpl(MethodImplOptions.Synchronized)]
            internal RespuestaHostWeb Send(byte[] buffer)
            {
                //byte[] buffer = Serializador.Serializar(peticion, ProtocoloSerializacion.Socket);

                //Agregar bandera <!--END--> al mensaje.
                Array.Resize(ref buffer, buffer.Length + SocketEnd.Length);
                Array.Copy(SocketEnd, 0, buffer, buffer.Length - SocketEnd.Length, SocketEnd.Length);

                byte[] recept = null;

                if (this.m_state == null)
                {
                    SocketCliente canal = new SocketCliente();
                    canal.Open(host, puerto, maxBufferSize, maxTimeout);

                    recept = canal.Send(buffer);
                    canal.Close();
                }
                else
                {
                    SocketClienteBidireccional canal = new SocketClienteBidireccional();
                    this.m_state.Buffer = this.m_state.SetStxEtxToBuffer(buffer);// Agrega el inicio y fin de mensaje
                    recept = canal.Send(this.m_state);
                }

                RespuestaHostWeb respuesta = null;

                try
                {
                    respuesta = SerializadorModuloWeb.DeserializarXML<RespuestaHostWeb>(recept);//, Adicional.Entidades.ProtocoloSerializacion.Socket);

                    if (!respuesta.EsValido)
                        throw new Exception(respuesta.Mensaje);
                }
                finally
                {
                    if (recept != null)
                    {
                        Array.Clear(recept, 0, recept.Length);
                        Array.Resize(ref recept, 0);
                        recept = null;
                    }

                    if (this.m_state != null)
                    {
                        this.m_state.ClearBuffer();
                        this.m_state.BytesReceived =
                            this.m_state.OffSet = 0;
                    }
                }

                return respuesta;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static Uri GetHost()
            {
                UriBuilder _uriSb = new UriBuilder("tcp://localhost:808");

                string _uriStr = ConfigurationManager.AppSettings["ModuloWebSocket"];

                if (!string.IsNullOrEmpty(_uriStr))
                {
                    if (_uriStr.Contains("://"))
                    {
                        int idx = _uriStr.IndexOf("://") + 3;
                        _uriStr = _uriStr.Substring(idx).Trim();
                    }

                    string[] split = _uriStr.Split(new char[] { ':', '/' }, StringSplitOptions.RemoveEmptyEntries);


                    _uriSb.Host = split[0].Trim();
                    if (split.Length > 1)
                    {
                        _uriSb.Port = Convert.ToInt32(split[1].Trim());
                    }

                    _uriSb.Scheme = "tcp";
                }

                return _uriSb.Uri;
            }

            #endregion

            #region Clases

            private sealed class SocketCliente
            {
                Socket client = null;
                int maxBufferSize = 0;
                Exception excepcion = null;
                ManualResetEvent allDone = new ManualResetEvent(false);

                public void Open(string host, int puerto, int maxBufferSize, int maxTimeout)
                {
                    //maxBufferSize = 2 * 128;
                    allDone.Reset();
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    client.SendBufferSize = maxBufferSize;
                    client.SendTimeout = maxTimeout;
                    client.ReceiveBufferSize = maxBufferSize;
                    client.ReceiveTimeout = maxTimeout;

                    client.BeginConnect(new IPEndPoint(Array.Find(Dns.GetHostAddresses(host), ip => ip.AddressFamily == AddressFamily.InterNetwork), puerto), new AsyncCallback(connectCallback), client);
                    this.maxBufferSize = maxBufferSize;
                    allDone.WaitOne();

                    done();
                }

                public byte[] Send(byte[] buffer)
                {
                    StateObject state = new StateObject();
                    state.WorkSocket = client;
                    state.Buffer = buffer;

                    try
                    {
                        allDone.Reset();
                        client.BeginSend(state.Buffer, state.OffSet, state.Buffer.Length, SocketFlags.None, new AsyncCallback(sendCallback), state);
                        allDone.WaitOne();

                        done();

                        state.Buffer = new byte[maxBufferSize];
                        state.OffSet = 0;

                        allDone.Reset();
                        client.BeginReceive(state.Buffer, state.OffSet, state.Buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), state);
                        allDone.WaitOne();

                        done();
                    }
                    catch (Exception e)
                    {
                        this.excepcion = e;
                    }
                    byte[] result = new byte[state.OffSet];
                    Array.Copy(state.Buffer, result, state.OffSet);

                    return result;
                }

                public void Close()
                {
                    if (client.Connected)
                    {
                        client.Disconnect(false);
                        client.Shutdown(SocketShutdown.Both);
                    }

                    client.Close();
                    client = null;
                }

                void done()
                {
                    if (this.excepcion != null)
                    {
                        try
                        {
                            throw this.excepcion;
                        }
                        finally
                        {
                            this.excepcion = null;
                        }
                    }
                }

                void connectCallback(IAsyncResult argument)
                {
                    try
                    {
                        Socket client = (argument.AsyncState as Socket);
                        client.EndConnect(argument);
                        allDone.Set();
                    }
                    catch (Exception e)
                    {
                        this.excepcion = e;
                        allDone.Set();
                    }
                }

                void sendCallback(IAsyncResult argument)
                {
                    try
                    {
                        StateObject state = (argument.AsyncState as StateObject);
                        Socket handler = state.WorkSocket;

                        state.OffSet += handler.EndSend(argument);

                        if (state.OffSet < state.Buffer.Length)
                        {
                            handler.BeginSend(state.Buffer, state.OffSet, state.Buffer.Length - state.OffSet, SocketFlags.None, new AsyncCallback(sendCallback), state);
                        }
                        else
                        {
                            allDone.Set();
                        }
                    }
                    catch (Exception e)
                    {
                        this.excepcion = e;
                        allDone.Set();
                    }
                }

                void receiveCallback(IAsyncResult argument)
                {
                    try
                    {
                        StateObject state = (argument.AsyncState as StateObject);
                        Socket handler = state.WorkSocket;

                        state.BytesReceived = handler.EndReceive(argument);

                        if (state.BytesReceived > 0)
                        {
                            state.OffSet += state.BytesReceived;

                            //Validar bandera <!--END--> en el mensaje.
                            byte[] flag = new byte[10];
                            Array.Copy(state.Buffer, state.OffSet < 10 ? 0 : state.OffSet - 10, flag, 0, 10);

                            if (Enumerable.SequenceEqual(SocketEnd, flag))
                            {
                                allDone.Set();
                            }
                            else
                            {
                                handler.BeginReceive(state.Buffer, state.OffSet, state.Buffer.Length - state.OffSet, SocketFlags.None, new AsyncCallback(receiveCallback), state);
                            }
                        }
                        else
                        {
                            allDone.Set();
                        }
                    }
                    catch (Exception e)
                    {
                        this.excepcion = e;
                        allDone.Set();
                    }
                }
            }

            class SocketClienteBidireccional
            {
                ManualResetEvent m_waitSend = new ManualResetEvent(false);
                ManualResetEvent m_waitReceive = new ManualResetEvent(false);

                public byte[] Send(StateObjectBidireccional state)
                {
                    SocketError error = SocketError.Success;
                    if (state.WorkSocket.Blocking)
                    {
                        state.WorkSocket.Send(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, out error);
                    }
                    else
                    {
                        IAsyncResult asyncSend = state.WorkSocket.BeginSend(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, out error, null, null);

                        if (error == SocketError.Success && asyncSend != null)
                        {
                            if (!asyncSend.IsCompleted)
                            {
                                asyncSend.AsyncWaitHandle.WaitOne(state.WorkSocket.SendTimeout);
                            }

                            state.WorkSocket.EndSend(asyncSend, out error);
                        }
                    }

                    state.ClearBuffer();
                    state.OffSet = 0;

                    byte[] result = new byte[0];

                    if (error == SocketError.Success)
                    {
                        using (MemoryStream readMem = new MemoryStream())
                        {
                            using (BufferedStream readBuf = new BufferedStream(readMem, state.WorkSocket.ReceiveBufferSize))
                            {
                                int offSet = 0;
                                int readCount = 0;
                                byte[] _inner = new byte[state.WorkSocket.ReceiveBufferSize];

                                try
                                {
                                    while ((readCount = state.WorkSocket.EndReceive(state.WorkSocket.BeginReceive(_inner, 0, _inner.Length, SocketFlags.None, null, null), out error)) > 0)
                                    {
                                        if (error != SocketError.Success)
                                        {
                                            //Gurock.SmartInspect.SiAuto.Main.LogError("Error de comunicación {0}", error);
                                            break;
                                        }

                                        readBuf.Write(_inner, offSet, readCount);
                                        offSet += readCount;

                                        if (Array.IndexOf(_inner, ConstantesSocket.ETX_BYTE) > 0) { break; }
                                    }

                                    readBuf.Flush();
                                    readMem.Flush();
                                    readMem.Seek(0, SeekOrigin.Begin);
                                    readMem.Position = 0;

                                    result = state.GetCleanBuffer(readMem.ToArray());
                                }
                                finally
                                {
                                    if (_inner != null)
                                    {
                                        Array.Clear(_inner, 0, _inner.Length);
                                        Array.Resize(ref _inner, 0);
                                        _inner = null;
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }

            #endregion
        }

        #endregion

        public ServiciosModuloWebProveedor(SesionModuloWeb sesion, TipoConexionUsuario tipo)
        {
            this._tipo = tipo;
            this.inicializarCanal(this._tipo);
            this.Sesion = sesion;
        }

        private T GetConexionModuloWeb<T>(SesionModuloWeb sesion, SolicitudHostWeb solicitud, ref string message)
        {
            Uri _localUri = ProxySocketProveedor.GetHost(); // new Uri(ConfigurationManager.AppSettings["ModuloWebSocket"] ?? "http://localhost:808");
            ProxySocketProveedor servicio = new ProxySocketProveedor(_localUri.Host, _localUri.Port);
            RespuestaHostWeb result = servicio.Send(SerializadorModuloWeb.SerializarXML(solicitud)); //, Adicional.Entidades.ProtocoloSerializacion.Socket));

            if (!result.EsValido)
            {
                message = result.Mensaje;
                MensajesRegistros.Error(string.Format(TituloMensaje, solicitud.Metodo.ToString()), result.Mensaje);
                throw new Exception(result.Mensaje);
            }

            return SerializadorModuloWeb.DeserializarXML<T>(result.Resultado as byte[]);// Serializador.Deserializar<T>(result.Resultado as byte[], Adicional.Entidades.ProtocoloSerializacion.Socket);

            #region Comentado
            //try
            //{
            //    ProxyModuloWeb _servicioMonitor = new ProxyModuloWeb(serviciosConexion.GetMonitorBinding(), new EndpointAddress(serviciosConexion.HostMonitor.ToString()));
            //    IModuloWeb servicio = null;
            //    try
            //    {
            //        servicio = _servicioMonitor.GetChannel;

            //        if (!this.Sesion.Compare(sesion))
            //        {
            //            this.Sesion = sesion;
            //        }

            //        if (solicitud.Sesion == null)
            //        {
            //            solicitud.Sesion = this.Sesion;
            //        }

            //        var resultado = SerializadorServiciosWeb.Deserializar<RespuestaHostWeb>(servicio.EnviarPeticion(SerializadorServiciosWeb.Serializar(solicitud)));
            //        if (!resultado.EsValido)
            //        {
            //            message = resultado.Mensaje;
            //            return default(T);
            //        }
            //        return (T)resultado.Resultado;
            //    }
            //    finally
            //    {
            //        if (servicio != null)
            //        {
            //            var clientInstace = (servicio as IClientChannel);
            //            try
            //            {
            //                if (clientInstace.State == CommunicationState.Faulted)
            //                {
            //                    try { clientInstace.Abort(); }
            //                    catch { }
            //                    try { _servicioMonitor.Abort(); }
            //                    catch { }
            //                }
            //                else
            //                {
            //                    clientInstace.BeginClose((r) =>
            //                        {
            //                            object[] items = (object[])r.AsyncState;
            //                            ProxyServiciosWeb proxy = (items[1] as ProxyServiciosWeb);

            //                            try
            //                            {
            //                                using (IClientChannel cInstance = (items[0] as IClientChannel))
            //                                {
            //                                    cInstance.EndClose(r);
            //                                    switch (cInstance.State)
            //                                    {
            //                                        case CommunicationState.Closed:
            //                                        case CommunicationState.Closing:
            //                                            try { proxy.Close(); }
            //                                            catch { proxy.Abort(); }
            //                                            break;
            //                                        case CommunicationState.Created:
            //                                        case CommunicationState.Opened:
            //                                        case CommunicationState.Opening:
            //                                            try
            //                                            {
            //                                                cInstance.Close();
            //                                                proxy.Close();
            //                                            }
            //                                            catch
            //                                            {
            //                                                cInstance.Abort();
            //                                                proxy.Abort();
            //                                            }
            //                                            break;
            //                                        case CommunicationState.Faulted:
            //                                        default:
            //                                            try
            //                                            {
            //                                                cInstance.Abort();
            //                                                proxy.Abort();
            //                                            }
            //                                            catch { }
            //                                            break;
            //                                    }
            //                                }
            //                            }
            //                            catch (Exception)
            //                            {
            //                                try { proxy.Abort(); }
            //                                catch { }
            //                            }
            //                        }, new object[] { clientInstace, _servicioMonitor });
            //                }
            //            }
            //            catch
            //            {
            //                try { clientInstace.Abort(); }
            //                catch { }
            //                try { _servicioMonitor.Abort(); }
            //                catch { }
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    MensajesRegistros.Error(string.Format(TituloMensaje, solicitud.Metodo.ToString()), MensajesRegistros.GetFullMessage(e));
            //    throw;
            //} 
            #endregion
        }

        private T GetFunctionConexion<T>(SesionModuloWeb sesion, SolicitudHostWeb solicitud, ref string message)
        {
            try
            {
                ProxyServiciosWeb _servicioMonitor = new ProxyServiciosWeb(serviciosConexion.GetMonitorBinding(), new EndpointAddress(serviciosConexion.HostMonitor.ToString()));
                IModuloWebAdicional servicio = null;
                try
                {
                    servicio = _servicioMonitor.GetChannel;

                    if (!this.Sesion.Compare(sesion))
                    {
                        this.Sesion = sesion;
                    }

                    if (solicitud.Sesion == null)
                    {
                        solicitud.Sesion = this.Sesion;
                    }

                    var resultado = SerializadorModuloWeb.Deserializar<RespuestaHostWeb>(servicio.EnviarPeticion(SerializadorModuloWeb.Serializar(solicitud)));
                    if (!resultado.EsValido)
                    {
                        message = resultado.Mensaje;
                        return default(T);
                    }
                    return (T)resultado.Resultado;
                }
                finally
                {
                    if (servicio != null)
                    {
                        var clientInstace = (servicio as IClientChannel);
                        try
                        {
                            if (clientInstace.State == CommunicationState.Faulted)
                            {
                                try { clientInstace.Abort(); }
                                catch { }
                                try { _servicioMonitor.Abort(); }
                                catch { }
                            }
                            else
                            {
                                clientInstace.BeginClose((r) =>
                                {
                                    object[] items = (object[])r.AsyncState;
                                    ProxyServiciosWeb proxy = (items[1] as ProxyServiciosWeb);

                                    try
                                    {
                                        using (IClientChannel cInstance = (items[0] as IClientChannel))
                                        {
                                            cInstance.EndClose(r);
                                            switch (cInstance.State)
                                            {
                                                case CommunicationState.Closed:
                                                case CommunicationState.Closing:
                                                    try { proxy.Close(); }
                                                    catch { proxy.Abort(); }
                                                    break;
                                                case CommunicationState.Created:
                                                case CommunicationState.Opened:
                                                case CommunicationState.Opening:
                                                    try
                                                    {
                                                        cInstance.Close();
                                                        proxy.Close();
                                                    }
                                                    catch
                                                    {
                                                        cInstance.Abort();
                                                        proxy.Abort();
                                                    }
                                                    break;
                                                case CommunicationState.Faulted:
                                                default:
                                                    try
                                                    {
                                                        cInstance.Abort();
                                                        proxy.Abort();
                                                    }
                                                    catch { }
                                                    break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        try { proxy.Abort(); }
                                        catch { }
                                    }
                                }, new object[] { clientInstace, _servicioMonitor });
                            }
                        }
                        catch
                        {
                            try { clientInstace.Abort(); }
                            catch { }
                            try { _servicioMonitor.Abort(); }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MensajesRegistros.Error(string.Format(TituloMensaje, solicitud.Metodo.ToString()), MensajesRegistros.GetFullMessage(e));
                throw;
            }
        }

        private void GetFunctionPerform(Action<IModuloWebPerform> fn)
        {
            try
            {
                IModuloWebPerform servicio = null;
                ProxyServiciosWebPerform ServicioPerform = new ProxyServiciosWebPerform(serviciosConexion.GetPerformBinding(), new EndpointAddress(serviciosConexion.HostPerform.ToString()));
                try
                {
                    servicio = ServicioPerform.GetChannel;
                    fn(servicio);
                }
                finally
                {
                    if (servicio != null)
                    {
                        var clientInstace = (IClientChannel)servicio;
                        if (clientInstace.State == CommunicationState.Faulted)
                        {
                            try { clientInstace.Abort(); }
                            catch { }
                        }
                        else
                        {
                            clientInstace.BeginClose((r) =>
                            {
                                try
                                {
                                    object[] items = (object[])r.AsyncState;
                                    ProxyServiciosWebPerform proxy = ((ProxyServiciosWebPerform)items[1]);
                                    using (IClientChannel cInstance = ((IClientChannel)items[0]))
                                    {
                                        cInstance.EndClose(r);
                                        switch (cInstance.State)
                                        {
                                            case CommunicationState.Closed:
                                            case CommunicationState.Closing:
                                                try { proxy.Close(); }
                                                catch { proxy.Abort(); }
                                                break;
                                            case CommunicationState.Created:
                                            case CommunicationState.Opened:
                                            case CommunicationState.Opening:
                                                try
                                                {
                                                    cInstance.Close();
                                                    proxy.Close();
                                                }
                                                catch
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                break;
                                            case CommunicationState.Faulted:
                                            default:
                                                try
                                                {
                                                    cInstance.Abort();
                                                    proxy.Abort();
                                                }
                                                catch { }
                                                break;
                                        }
                                    }
                                }
                                catch (Exception) { }
                            }, new object[] { clientInstace, ServicioPerform });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //this.inicializarCanal(_tipo);
                MensajesRegistros.Error(string.Format(TituloMensaje, fn.Method.Name), e.Message);
                throw e;
            }
        }

        private void inicializarCanal(TipoConexionUsuario tipo)
        {
            if (serviciosConexion == null)
            {
                serviciosConexion = new ServiciosConexion(tipo);
            }
        }

        private SesionModuloWeb ObtenerSesionDefault()
        {
            return new SesionModuloWeb()
                {
                    Empresa = new Entidades.Base.DatosEmpresa()//new ImagenSoft.Framework.Entidades.Empresa()
                        {
                            Id = 1
                        }
                };
        }

        #region Administrar Usuarios Clientes Members

        public ListaAdministrarUsuariosClientes AdministrarUsuariosClienteObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteObtenerTodosFiltro,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            ListaAdministrarUsuariosClientes resultado = GetConexionModuloWeb<ListaAdministrarUsuariosClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuariosClientes AdministrarUsuariosClienteObtener(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteObtener,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            AdministrarUsuariosClientes resultado = GetConexionModuloWeb<AdministrarUsuariosClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuariosClientes AdministrarUsuariosClienteInsertar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteInsertar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarUsuariosClientes resultado = GetConexionModuloWeb<AdministrarUsuariosClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuariosClientes AdministrarUsuariosClienteModificar(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteModificar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarUsuariosClientes resultado = GetConexionModuloWeb<AdministrarUsuariosClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteEliminar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteNuevaContrasenia(SesionModuloWeb sesion, FiltroAdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteNuevaContrasenia,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteSolicitarContrasenia(SesionModuloWeb sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteSolicitarContrasenia,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuariosClientes AdministrarUsuariosClienteModificarPrivilegios(SesionModuloWeb sesion, AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteModificarPrivilegios,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarUsuariosClientes resultado = GetConexionModuloWeb<AdministrarUsuariosClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteCambiarContrasenia(SesionModuloWeb sesion, Entidades.Web.AdministrarUsuariosClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteCambiarContrasenia,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteRestablecerContrasenia(SesionModuloWeb sesion, Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteRestablecerContrasenia,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosClienteInsertarModificar(SesionModuloWeb sesion, Entidades.Web.ListaAdministrarUsuariosClientes listado)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosClienteInsertarModificar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(listado)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        //public ListaEstaciones AdicionalWebObtenerEstacionesTodosFiltro(Sesion sesion, FiltroEstacion filtro)
        //{
        //    SolicitudHostWeb solicitud = new SolicitudHostWeb();
        //    {
        //        solicitud.Metodo = Metodos.AdicionalWebObtenerEstacionesTodosFiltro;
        //        solicitud.Sesion = sesion;
        //        solicitud.Parametro = filtro;
        //    }
        //    string message = string.Empty;
        //    ListaEstaciones tmp = GetFunctionConexion<ListaEstaciones>(sesion, solicitud, ref message);
        //    if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
        //    {
        //        throw new Exception(message);
        //    }

        //    return tmp;
        //}

        //public IGas.Servicios.Entidades.ListaTanqueServicios AdicionalWebObtenerTanquesTodosFiltro(Sesion sesion, AdicionalWebFiltroBase filtro)
        //{
        //    SolicitudHostWeb solicitud = new SolicitudHostWeb();
        //    {
        //        solicitud.Metodo = Metodos.AdicionalWebObtenerTanquesTodosFiltro;
        //        solicitud.Sesion = sesion;
        //        solicitud.Parametro = filtro;
        //    }

        //    string message = string.Empty;
        //    IGas.Servicios.Entidades.ListaTanqueServicios tmp = GetFunctionConexion<IGas.Servicios.Entidades.ListaTanqueServicios>(sesion, solicitud, ref message);

        //    if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
        //    {
        //        throw new Exception(message);
        //    }

        //    return tmp;
        //}

        //public IGas.Servicios.Entidades.ListaDispensarioServicios AdicionalWebObtenerDispensariosTodosFiltro(Sesion sesion, AdicionalWebFiltroBase filtro)
        //{
        //    SolicitudHostWeb solicitud = new SolicitudHostWeb();
        //    {
        //        solicitud.Metodo = Metodos.AdicionalWebObtenerDispensariosTodosFiltro;
        //        solicitud.Sesion = sesion;
        //        solicitud.Parametro = filtro;
        //    }

        //    string message = string.Empty;
        //    IGas.Servicios.Entidades.ListaDispensarioServicios tmp = GetFunctionConexion<IGas.Servicios.Entidades.ListaDispensarioServicios>(sesion, solicitud, ref message);

        //    if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
        //    {
        //        throw new Exception(message);
        //    }

        //    return tmp;
        //}

        #endregion

        #region Administrar Clientes

        public int AdministrarClientesConsecutivo(SesionModuloWeb sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesConsecutivo,
                Sesion = sesion
            };

            string message = string.Empty;
            int resultado = GetConexionModuloWeb<int>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarClientes AdministrarClientesInsertar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesInsertar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarClientes resultado = GetConexionModuloWeb<AdministrarClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarClientes AdministrarClientesModificar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesModificar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarClientes resultado = GetConexionModuloWeb<AdministrarClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarClientesEliminar(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesEliminar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarClientes AdministrarClientesObtener(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesObtener,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            AdministrarClientes resultado = GetConexionModuloWeb<AdministrarClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public ListaAdministrarClientes AdministrarClientesObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarClientesObtenerTodosFiltro,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            ListaAdministrarClientes resultado = GetConexionModuloWeb<ListaAdministrarClientes>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        #endregion

        #region Distribuidores

        public int AdministrarDistribuidoresConsecutivo(SesionModuloWeb sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresConsecutivo,
                Sesion = sesion
            };

            string message = string.Empty;
            int resultado = GetConexionModuloWeb<int>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresInsertar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresInsertar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarDistribuidores resultado = GetConexionModuloWeb<AdministrarDistribuidores>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresModificar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresModificar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarDistribuidores resultado = GetConexionModuloWeb<AdministrarDistribuidores>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarDistribuidoresEliminar(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresEliminar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarDistribuidores AdministrarDistribuidoresObtener(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresObtener,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            AdministrarDistribuidores resultado = GetConexionModuloWeb<AdministrarDistribuidores>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public ListaAdministrarDistribuidores AdministrarDistribuidoresObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarDistribuidoresObtenerTodosFiltro,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            ListaAdministrarDistribuidores resultado = GetConexionModuloWeb<ListaAdministrarDistribuidores>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        #endregion

        #region Administrar Usuarios

        public int AdministrarUsuariosConsecutivo(SesionModuloWeb sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosConsecutivo,
                Sesion = sesion
            };

            string message = string.Empty;
            int resultado = GetConexionModuloWeb<int>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuarios AdministrarUsuariosInsertar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosInsertar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarUsuarios resultado = GetConexionModuloWeb<AdministrarUsuarios>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuarios AdministrarUsuariosModificar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosModificar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            AdministrarUsuarios resultado = GetConexionModuloWeb<AdministrarUsuarios>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public bool AdministrarUsuariosEliminar(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosEliminar,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool resultado = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public AdministrarUsuarios AdministrarUsuariosObtener(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosObtener,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            AdministrarUsuarios resultado = GetConexionModuloWeb<AdministrarUsuarios>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        public ListaAdministrarUsuarios AdministrarUsuariosObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdministrarUsuariosObtenerTodosFiltro,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            ListaAdministrarUsuarios resultado = GetConexionModuloWeb<ListaAdministrarUsuarios>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        #endregion


        #region Host Adicional Proveedor Members

        public ListaEstaciones AdicionalWebVerificarEstaciones(SesionModuloWeb sesion)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebVerificarEstaciones,
                Sesion = sesion
            };

            string message = string.Empty;
            ListaEstaciones tmp = GetConexionModuloWeb<ListaEstaciones>(sesion, solicitud, ref message);// GetFunctionConexion<ListaEstaciones>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public bool AdicionalWebCambiarFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebCambiarFlujo,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool tmp = GetConexionModuloWeb<bool>(sesion, solicitud, ref message); //GetFunctionConexion<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public SesionModuloWeb AdicionalWebValidarLogin(SesionModuloWeb sesion, UsuarioWeb entidad)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionaWeblLogin,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(entidad)
            };

            string message = string.Empty;
            SesionModuloWeb tmp = GetConexionModuloWeb<SesionModuloWeb>(sesion, solicitud, ref message);// GetFunctionConexion<SesionModuloWeb>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public string AdicionalWebEstadoFlujo(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroCambiarFlujo filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebEstadoFlujo,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            string tmp = GetConexionModuloWeb<string>(sesion, solicitud, ref message);// GetFunctionConexion<string>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public Entidades.Web.Adicional.ListaDispensarios AdicionalWebObtenerMangueras(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebObtenerMangueras,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            Entidades.Web.Adicional.ListaDispensarios tmp = GetConexionModuloWeb<Entidades.Web.Adicional.ListaDispensarios>(sesion, solicitud, ref message);// GetFunctionConexion<Entidades.Web.Adicional.ListaDispensarios>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public bool AdicionalWebEstablecerDispensario(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebEstablecerDispensario,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool tmp = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);// GetFunctionConexion<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public bool AdicionalWebEstablecerDispensarioGlobal(SesionModuloWeb sesion, Adicional.Entidades.Web.FiltroMangueras filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.AdicionalWebEstablecerDispensarioGlobal,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            bool tmp = GetConexionModuloWeb<bool>(sesion, solicitud, ref message);// GetFunctionConexion<bool>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        public bool ServicioPings()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.ServicioPings,
                Sesion = this.ObtenerSesionDefault()
            };

            string message = string.Empty;
            bool result = GetConexionModuloWeb<bool>(this.Sesion, solicitud, ref message);// GetFunctionConexion<bool>(this.Sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return result;
        }

        public List<ReporteVentasCombustible> ObtenerReporteVentasCombustible(SesionModuloWeb sesion, FiltroReporteVentasCombustible filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.ObtenerReporteVentasCombustible,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            List<ReporteVentasCombustible> tmp = GetConexionModuloWeb<List<ReporteVentasCombustible>>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return tmp;
        }

        #endregion

        #region Sesiones

        public ListaSesiones SesionObtenerTodosFiltro(SesionModuloWeb sesion, FiltroSesionModuloWeb filtro)
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.SesionObtenerTodosFiltro,
                Sesion = sesion,
                Parametro = SerializadorModuloWeb.SerializarXML(filtro)
            };

            string message = string.Empty;
            ListaSesiones resultado = GetConexionModuloWeb<ListaSesiones>(sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        #endregion

        #region Servicios Ping

        public bool Ping()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.Ping,
                Sesion = ObtenerSesionDefault(),
                Parametro = null,
            };

            string message = string.Empty;
            bool result = GetConexionModuloWeb<bool>(solicitud.Sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return result;

            //bool result = false;
            //ManualResetEvent await = new ManualResetEvent(false);
            //Exception exception = null;
            //ThreadPool.QueueUserWorkItem(_ =>
            //{
            //    try
            //    {
            //        this.GetFunctionPerform((servicio) =>
            //                {
            //                    result = servicio.Ping();
            //                    await.Set();
            //                });
            //    }
            //    catch (Exception e)
            //    {
            //        exception = e;
            //        await.Set();
            //    }
            //});

            //await.WaitOne();
            //if (exception != null) throw exception;
            //return result;
        }

        public bool Ping(SesionModuloWeb sesion)
        {
            if (!this.Sesion.Compare(sesion))
            {
                this.Sesion = sesion;
            }

            this.Sesion.FechaHoraCliente = DateTime.Now;

            SolicitudHostWeb solicitud = new SolicitudHostWeb()
            {
                Metodo = Metodos.PingSesion,
                Sesion = this.Sesion,
                Parametro = null,
            };

            string message = string.Empty;
            bool result = GetConexionModuloWeb<bool>(this.Sesion, solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return result;

            //if (!this.Sesion.Compare(sesion))
            //{
            //    this.Sesion = sesion;
            //}

            //this.Sesion.FechaHoraCliente = DateTime.Now;

            //bool result = false;
            //ManualResetEvent await = new ManualResetEvent(false);
            //Exception exception = null;
            //ThreadPool.QueueUserWorkItem(_ =>
            //{
            //    try
            //    {
            //        this.GetFunctionPerform((servicio) =>
            //        {
            //            result = servicio.Ping(sesion);
            //            await.Set();
            //        });
            //    }
            //    catch (Exception e)
            //    {
            //        exception = e;
            //        await.Set();
            //    }
            //});

            //await.WaitOne();
            //if (exception != null) throw exception;
            //return result;
        }

        public byte[] GetConfig(byte[] request)
        {
            byte[] result = null;
            ManualResetEvent await = new ManualResetEvent(false);
            Exception exception = null;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    this.GetFunctionPerform((servicio) =>
                    {
                        result = servicio.GetConfig(request);
                        await.Set();
                    });
                }
                catch (Exception e)
                {
                    exception = e;
                    await.Set();
                }
            });

            await.WaitOne();
            if (exception != null) throw exception;
            return result;
        }

        #endregion

        public DateTime ObtenerFechaHoraServidor()
        {
            SolicitudHostWeb solicitud = new SolicitudHostWeb();
            {
                solicitud.Metodo = Metodos.ObtenerFechaHoraServidor;
            }

            string message = string.Empty;
            DateTime resultado = GetConexionModuloWeb<DateTime>(ObtenerSesionDefault(), solicitud, ref message);

            if (!string.IsNullOrEmpty((message ?? string.Empty).Trim()))
            {
                throw new Exception(message);
            }

            return resultado;
        }

        //internal class UtileriasProveedor
        //{
        //    #region Serializacion GZip

        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    public static byte[] SerializarGZip<T>(T obj)
        //    {
        //        ManualResetEvent _task = new ManualResetEvent(false);
        //        byte[] result = new byte[0];
        //        Exception exception = null;
        //        ThreadPool.QueueUserWorkItem((w) =>
        //        {
        //            try
        //            {
        //                using (MemoryStream msCompressed = new MemoryStream())
        //                {
        //                    using (GZipStream gZipStream = new GZipStream(msCompressed, CompressionMode.Compress))
        //                    {
        //                        using (MemoryStream msDecompressed = new MemoryStream())
        //                        {
        //                            new BinaryFormatter().Serialize(msDecompressed, obj);
        //                            byte[] byteArray = msDecompressed.ToArray();

        //                            gZipStream.Write(byteArray, 0, byteArray.Length);
        //                            gZipStream.Close();
        //                            result = msCompressed.ToArray();
        //                            _task.Set();
        //                        }
        //                    }
        //                }

        //            }
        //            catch (Exception e)
        //            {
        //                exception = e;
        //                _task.Set();
        //            }
        //        });

        //        _task.WaitOne();
        //        if (exception != null)
        //        {
        //            throw exception;
        //        }
        //        return result;
        //    }

        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    public static T DeserializarGZip<T>(byte[] arrBytes)
        //    {
        //        ManualResetEvent _task = new ManualResetEvent(false);
        //        T result = default(T);
        //        Exception exception = null;
        //        ThreadPool.QueueUserWorkItem((w) =>
        //        {
        //            try
        //            {
        //                using (MemoryStream mem = new MemoryStream(arrBytes))
        //                {
        //                    using (GZipStream gZipStream = new GZipStream(mem, CompressionMode.Decompress, true))
        //                    {
        //                        mem.Position = 0L;
        //                        BinaryFormatter bin2 = new BinaryFormatter();
        //                        result = (T)bin2.Deserialize(gZipStream);
        //                        _task.Set();
        //                    }
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                exception = e;
        //                _task.Set();
        //            }
        //        });

        //        _task.WaitOne();
        //        if (exception != null)
        //        {
        //            throw exception;
        //        }
        //        return result;
        //    }

        //    #endregion

        //    #region Serializacion XML

        //    public static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings()
        //    {
        //        Encoding = new UTF8Encoding(false),
        //        Indent = false,
        //        NewLineHandling = NewLineHandling.None
        //    };

        //    public static byte[] SerializarXML<T>(T obj)
        //    {
        //        try
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                using (BufferedStream buff = new BufferedStream(m))
        //                {
        //                    XmlSerializer xml = new XmlSerializer(typeof(T));

        //                    using (XmlWriter writer = XmlWriter.Create(buff, XmlWriterSettings))
        //                    {
        //                        xml.Serialize(writer, obj);

        //                        writer.Flush();
        //                        buff.Flush();
        //                        m.Position = 0L;
        //                        return m.ToArray();
        //                    }
        //                }
        //            }
        //        }
        //        catch (System.Exception e)
        //        {
        //            return new byte[0];
        //        }
        //    }

        //    public static T DeserializarXML<T>(byte[] arrBytes)
        //    {
        //        using (MemoryStream m = new MemoryStream(arrBytes))
        //        {
        //            using (BufferedStream buff = new BufferedStream(m))
        //            {
        //                XmlSerializer xml = new XmlSerializer(typeof(T));
        //                return (T)xml.Deserialize(buff);
        //            }
        //        }
        //    }

        //    #endregion
        //}
    }
}
