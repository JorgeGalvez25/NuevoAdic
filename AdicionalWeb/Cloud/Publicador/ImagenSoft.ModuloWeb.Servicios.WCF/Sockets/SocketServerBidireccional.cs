using Adicional.Entidades;
using Adicional.Entidades.SocketBidireccional;
using Adicional.Entidades.Sockets.Extenciones;
using Adicional.Proveedor.Sockets;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Utileria;
using ImagenSoft.ModuloWeb.Extensiones;
using ImagenSoft.ModuloWeb.Fachada;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace ImagenSoft.ModuloWeb.Servicios.WCF.Sockets
{
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public class SocketServerBidireccional : SocketServerBase
    {
        [ThreadStatic]
        private Thread m_localThread;

        private bool f_isOpen;

        private int v_bufferSize;

        private int v_maxTimeOut;

        private int v_port;

        private const string SERVICE_NAME = "SocketBidireccional";


        public int BUFFER_SIZE
        {
            get
            {
                if (this.v_bufferSize <= 0)
                {
                    this.v_bufferSize = ConstantesSocket.MAX_BUFFER_SIZE;
                }
                return this.v_bufferSize;
            }
        }

        public int MAX_TIME_OUT
        {
            get
            {
                if (this.v_maxTimeOut <= 0)
                {
                    this.v_maxTimeOut = ConstantesSocket.MAX_TIME_OUT;
                }

                return this.v_maxTimeOut;
            }
        }

        public int PORT
        {
            get
            {
                if (this.v_port <= 0)
                {
                    string value = ConfigurationManager.AppSettings["SocketBidireccionalPort"] ?? "808";
                    int output = 0;
                    int.TryParse(value, out output);
                    this.v_port = (output == 0 ? 808 : output);
                }

                return this.v_port;
            }
        }


        public SocketServerBidireccional()
            : this(0)
        {

        }

        public SocketServerBidireccional(int port)
        {
            this.m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.m_socket.ReceiveBufferSize =
                this.m_socket.SendBufferSize = this.BUFFER_SIZE;
            this.m_socket.ReceiveTimeout =
                this.m_socket.SendTimeout = this.MAX_TIME_OUT;

            if (!port.IsBetween(1, 65535))
            {
                port = this.PORT;
            }

            this.m_socket.Bind(new IPEndPoint(IPAddress.Any, port));
            this.m_socket.NoDelay = true;
            //this.m_socket.Blocking = false;
            this.m_socket.UseOnlyOverlappedIO = true;
        }

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public void Start()
        {
            using (MensajesRegistros.EnterExitMethod _sLog = new MensajesRegistros.EnterExitMethod(SERVICE_NAME, "public void Start()"))
            {
                this.m_socket.Listen(int.MaxValue);
                // Evento de inicio de escucha
                _sLog.LogMessage("Iniciando...");
                this.f_isOpen = true;
                this.m_localThread = new Thread(new ThreadStart(() =>
                    {
                        using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod(SERVICE_NAME, string.Format("SocketServerBidireccional_{0}", this.PORT)))
                        {
                            try
                            {
                                using (Pool pool = new Pool(15))
                                {
                                    Socket socket = null;
                                    _log.LogMessage("Iniciando escucha...");
                                    while (this.f_isOpen)
                                    {
                                        try
                                        {
                                            if ((socket = this.m_socket.Accept()) != null)
                                            {
                                                pool.Enqueue(new PoolItem(new Action<object>(p => (p as SocketClienteRequest).ProcesarAccept()), new SocketClienteRequest(this, socket)));
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            _log.LogException("SocketServerBidireccional_Pool", e);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _log.LogException("SocketServerBidireccional", ex);
                            }
                        }
                    }))
                {
                    IsBackground = true,
                    Name = string.Format("SocketServerBidireccional_{0}", this.PORT)
                };

                this.m_localThread.Start();
            }
        }

        [SecurityCritical]
        public void Stop()
        {
            this.f_isOpen = false;

            try
            {
                try
                {
                    if (this.m_socket != null)
                    {
                        ClientManager.RemoveAll();
                        this.internalCloseSocket(this.m_socket);
                        this.m_socket = null;
                    }
                }
                catch
                {
                    if (this.m_socket != null)
                    {
                        this.internalCloseSocket(this.m_socket);
                        this.m_socket = null;
                    }
                }
            }
            finally
            {
                // Evento de detencion
                MensajesRegistros.Informacion("Servicio Socket: Detenido");
            }
        }

        public bool IsConnected
        {
            get
            {
                return ((this.m_socket == null)
                        ? false
                        : this.m_socket.IsConnected());
            }
        }
    }

    internal class SocketClienteRequest : SocketServerBase, IDisposable
    {
        private byte[] m_buffer;

        private SocketServerBidireccional m_parent;

        public SocketClienteRequest(SocketServerBidireccional parent, Socket socket)
        {
            this.m_parent = parent;
            this.m_socket = socket;
            this.m_buffer = new byte[ConstantesSocket.MAX_BUFFER_SIZE];
        }

        public void ProcesarAccept()
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod())
            {
                try
                {
                    this.m_socket.NoDelay = true;
                    //this.m_socket.Blocking = false;
                    this.m_socket.UseOnlyOverlappedIO = true;
                    //this.m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    //this.m_socket.SetSocketKeepAliveValues(10000, 1000);

                    StateObjectBidireccional state = new StateObjectBidireccional(0);
                    state.WorkSocket = this.m_socket;

                    _log.LogMessage("Socket aceptado");

                    this.m_socket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, new AsyncCallback(this.receiveCallback), state).AsyncWaitHandle.WaitOne();
                }
                catch (SocketException se)
                {
                    _log.LogException("Aceptando", se);
                    if (this.m_socket != null)
                    {
                        this.internalCloseSocket(this.m_socket);
                    }
                }
                catch (Exception e)
                {
                    _log.LogException("Aceptando", e);
                    if (this.m_socket != null)
                    {
                        this.internalCloseSocket(this.m_socket);
                    }
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForFullGCComplete();
                    GC.Collect();
                }
            }
        }

        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        private void receiveCallback(IAsyncResult argument)
        {
            StateObjectBidireccional state = (argument.AsyncState as StateObjectBidireccional);
            Lock.TryMutex(state.Id, () =>
                {
                    byte[] buffer = new byte[this.m_buffer.Length];
                    lock (this.m_buffer)
                    {
                        Buffer.BlockCopy(this.m_buffer, 0, buffer, 0, this.m_buffer.Length);
                        Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
                    }

                    using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod())
                    {
                        try
                        {
                            Socket handler = state.WorkSocket;
                            SocketError error = SocketError.Success;
                            int received = 0;

                            try
                            {
                                received = handler.EndReceive(argument, out error);
                                Array.Resize(ref buffer, received);
                            }
                            catch (Exception e)
                            {
                                _log.LogException("ReceiveCallback-b", e);
                                ClientManager.Remove(state);
                                return;
                            }

                            try
                            {
                                if (received > 0)
                                {
                                    bool isKeepAlive = Array.IndexOf(buffer, ConstantesSocket.BEL_BYTE) >= 0;

                                    if (!isKeepAlive)
                                    {
                                        bool isEof = Array.IndexOf(buffer, ConstantesSocket.ETX_BYTE) >= 0;
                                        bool isSyn = Array.IndexOf(buffer, ConstantesSocket.SYN_BYTE) >= 0;
                                        bool isAck = Array.IndexOf(buffer, ConstantesSocket.ACK_BYTE) >= 0;

                                        if (isAck)
                                        {
                                            state.ClearBuffer(0);
                                            state.Buffer = new byte[] { ConstantesSocket.SYN_BYTE, ConstantesSocket.ACK_BYTE };
                                            state.BytesReceived =
                                                state.OffSet = 0;
                                            handler.Send(state.Buffer, 0, state.Buffer.Length, SocketFlags.None);
                                        }
                                        else if (isSyn)
                                        {
                                            // NOTA: No hacer un dispose ni close de los Streams ya que puede cerrarse el socket XD
                                            using (MemoryStream readMem = new MemoryStream()) // Almacenamiento
                                            {
                                                using (BufferedStream readBuf = new BufferedStream(readMem, state.WorkSocket.ReceiveBufferSize))
                                                {
                                                    int readCount = received;
                                                    int offSet = 0;

                                                    do
                                                    {
                                                        if (error != SocketError.Success)
                                                        {
                                                            _log.LogError("Error de comunicación {0}", error);
                                                            break;
                                                        }

                                                        readBuf.Write(buffer, offSet, readCount);
                                                        offSet += readCount;

                                                        if (Array.IndexOf(buffer, ConstantesSocket.ETX_BYTE) > 0) break;
                                                    }
                                                    while ((readCount = state.WorkSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None, out error)) > 0);

                                                    readBuf.Flush();
                                                    readMem.Flush();
                                                    readMem.Seek(0, SeekOrigin.Begin);
                                                    readMem.Position = 0;
                                                    buffer = readMem.ToArray();
                                                }
                                            }

                                            state.Buffer = buffer;
                                            byte[] response = this.requestSyn(state);

                                            ServiciosFachada servicio = new ServiciosFachada();
                                            _log.LogObject("Ultima Conexión SYN", servicio.AdministrarClientesModificarUltimaConexionBidi(new SesionModuloWeb() { NoCliente = state.Id }, new FiltroAdministrarClientes()
                                                {
                                                    NoEstacion = state.Id,
                                                    FechaUltimaConexion = DateTime.Now,
                                                    Activo = "Si",
                                                    Conexion = "Si"
                                                }));

                                            //if (response != null && response.Length > 0)
                                            //{
                                            //    /***** Procesamiento de solicitudes y envio de respuesta *****/
                                            //    Proveedor current = new Proveedor(state);
                                            //    _log.LogObject("Syn Result", current.Syn(response));
                                            //}

                                            // MonitorPuertosActualizarEstatus(new Sesion() { NoCliente = state.Id }, new FiltroMonitorPuertos() { NoEstacion = state.Id, Activo = "Si", MonitorearPuertos = "Si" });
                                        }
                                        else if (isEof)
                                        {
                                            state.Buffer = buffer;
                                            byte[] response = this.requestSyn(state);

                                            ServiciosFachada servicio = new ServiciosFachada();
                                            _log.LogObject("Ultima Conexión-SYN", servicio.AdministrarClientesModificarUltimaConexionBidi(new SesionModuloWeb() { NoCliente = state.Id }, new FiltroAdministrarClientes()
                                            {
                                                NoEstacion = state.Id,
                                                FechaUltimaConexion = DateTime.Now,
                                                Activo = "Si",
                                                Conexion = "Si"
                                            }));

                                            //if (response != null && response.Length > 0)
                                            //{
                                            //    /***** Procesamiento de solicitudes y envio de respuesta *****/
                                            //    Proveedor current = new Proveedor(state);
                                            //    _log.LogObject("SYN Result", current.Syn(response));
                                            //}

                                            state.ClearBuffer();
                                            state.BytesReceived =
                                                state.OffSet = 0;

                                            //servicio.MonitorPuertosActualizarEstatus(new Sesion() { NoCliente = state.Id }, new FiltroMonitorPuertos() { NoEstacion = state.Id, Activo = "Si", MonitorearPuertos = "Si" });
                                        }
                                        // Maybe this never used
                                        else
                                        {
                                            handler.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, new AsyncCallback(this.receiveCallback), state).AsyncWaitHandle.WaitOne();
                                        }
                                    }
                                }
                                else
                                {
                                    byte[] bufferSend = new byte[] { ConstantesSocket.SYN_BYTE, ConstantesSocket.ACK_BYTE };
                                    handler.Send(bufferSend, 0, bufferSend.Length, SocketFlags.None);
                                    Array.Clear(bufferSend, 0, bufferSend.Length);
                                    Array.Resize(ref bufferSend, 0);
                                    bufferSend = null;

                                    state.ClearBuffer();
                                    state.BytesReceived =
                                        state.OffSet = 0;
                                }
                            }
                            catch (Exception e)
                            {
                                _log.LogException("ReceiveCallback-r", e);
                                ClientManager.Remove(state);
                            }
                        }
                        finally
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                        }
                    }
                });
        }

        [Obsolete("Ya no se utiliza todos los mensajes se envian sincronica")]
        private void sendResponseCallback(IAsyncResult argument)
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod())
            {
                StateObjectBidireccional state = (argument.AsyncState as StateObjectBidireccional);
                Socket handler = state.WorkSocket;

                try
                {
                    state.OffSet += handler.EndSend(argument);

                    if (state.OffSet < state.Buffer.Length)
                    {
                        handler.BeginSend(state.Buffer,
                                          state.OffSet,
                                          Math.Min(state.Buffer.Length - state.OffSet, ConstantesSocket.MAX_BUFFER_SIZE),
                                          SocketFlags.None,
                                          new AsyncCallback(this.sendResponseCallback),
                                          state);
                    }
                    else
                    {
                        state.ClearBuffer(0);
                        state.BytesReceived =
                            state.OffSet = 0;
                        Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
                        Array.Resize(ref this.m_buffer, 0);
                    }
                }
                catch (SocketException se)
                {
                    _log.LogException("sendResponseCallback", se);
                    ClientManager.Remove(state);
                }
                catch (Exception e)
                {
                    _log.LogException("sendResponseCallback", e);
                    ClientManager.Remove(state);
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!this.f_disposed)
            {
                this.f_disposed = true;

                this.m_socket = null;

                if (this.m_buffer != null)
                {
                    Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
                    Array.Resize(ref this.m_buffer, 0);
                    this.m_buffer = null;
                }

                this.m_parent = null;
            }
        }

        #endregion
    }

    public class SocketServerBase
    {
        internal Socket m_socket;

        internal bool f_disposed = false;

        internal bool internalCloseSocket(Socket socket)
        {
            if (socket != null)
            {
                try { socket.Close(0); }
                catch { }

                socket = null;
            }

            return socket == null;
        }

        public bool Disconnect(string key)
        {
            StateObjectBidireccional state = ClientManager.Get(key);
            if (state == null) { return false; }
            if (state.WorkSocket == null) return false;

            this.internalCloseSocket(state.WorkSocket);
            ClientManager.Remove(key);
            return true;
        }

        internal static ListaAdministrarClientes cacheClientes = new ListaAdministrarClientes();
        internal static DateTime fechaCache;

        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal byte[] request(StateObjectBidireccional state)
        {
            byte[] cleanBuffer = state.GetCleanBuffer(state.Buffer);
            // Operaciones a persistencia y logica
            ServiciosModuloWebSocket servicio = new ServiciosModuloWebSocket();
            return state.SetStxEtxToBuffer(servicio.EnviarPeticion(cleanBuffer));
        }

        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal byte[] requestSyn(StateObjectBidireccional state)
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod(state.Id, "internal byte[] requestSyn(StateObjectBidireccional state)"))
            {
                byte[] cleanBuffer = state.GetCleanBuffer(state.Buffer);

                state.Id = string.Format("E{0}", ConstantesSocket.LOCAL_ENCODING.GetString(cleanBuffer.Take(5).ToArray()));
                string version = ConstantesSocket.LOCAL_ENCODING.GetString(cleanBuffer.Skip(5).Take(4).ToArray());
                _log.LogMessage("ID {0}", state.Id);
                _log.LogMessage("Versión {0}", version);
                byte[] result = new byte[] { ConstantesSocket.ESC_BYTE }; // Valor invalido (solo para responder)

                // Operaciones a persistencia y logica
                Lock.Try(Lock.CACHE_CLIENTES_SOCKET, () =>
                    {
                        if (fechaCache <= DateTime.Now)
                        {
                            if (cacheClientes != null)
                            {
                                cacheClientes.Clear();
                            }
                            else
                            {
                                cacheClientes = new ListaAdministrarClientes();
                            }
                            ServiciosFachada servicio = new ServiciosFachada();
                            cacheClientes.AddRange((servicio.AdministrarClientesObtenerTodosFiltro(null, new FiltroAdministrarClientes() { Activo = "Si" }).Resultado as ListaAdministrarClientes));
                            fechaCache = DateTime.Now.AddMinutes(5);// Cada 5 minutos se actualizara el cache
                        }
                    });

                if (cacheClientes != null)
                {
                    _log.LogMessage("Existe {0} : {1}", state.Id, cacheClientes.Exists(p => p.NoEstacion.Equals(state.Id)));
                    //if (cacheClientes.AsParallel().Any(p => ((p.TipoServicio == TipoServicioCliente.MembresiaEstandar || p.TipoServicio == TipoServicioCliente.MembresiaPlus) &&
                    //                                         (p.EstatusMembresia == EstatusMembresiaCliente.PorVencer || p.EstatusMembresia == EstatusMembresiaCliente.Vigente)) &&
                    //                                         p.NoEstacion.Equals(state.Id)))
                    //{
                    if (cacheClientes.AsParallel().Any(p => p.NoEstacion.Equals(state.Id)))
                    {
                        _log.LogMessage("El cliente {0} fue aceptado", state.Id);
                        ClientManager.Add(state);
                        result = new byte[] { ConstantesSocket.ACK_BYTE }; // Valor valido (solo para responder)
                    }
                    else
                    {
                        _log.LogMessage("El cliente {0} NO fue aceptado", state.Id);
                        try { state.WorkSocket.Close(); }
                        catch (Exception e) { _log.LogException("Error al cerrar el socket invalido", e); }
                        return null;
                    }
                }

                return state.SetStxEtxToBuffer(result);
            }
        }
    }
}
