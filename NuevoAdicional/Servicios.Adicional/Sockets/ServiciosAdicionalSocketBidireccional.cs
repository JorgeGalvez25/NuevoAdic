using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Adicional.Entidades;
using Adicional.Entidades.SocketBidireccional;
using Adicional.Entidades.Sockets.Extenciones;

namespace Servicios.Adicional.Sockets
{
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public class ServiciosAdicionalSocketBidireccional
    {
        #region Propiedades

        private string ID;

        private Socket m_socket;

        private bool m_isConnecting;

        private AsyncOperation m_context;

        private IPEndPoint _remoteEndPoint;

        //private System.Timers.Timer m_timer;

        private byte[] m_buffer;

        private bool f_isTimerInitialized = false;

        #region Statics

        private static readonly object _lock = new object();
        private static readonly object _lockSocket = new object();
        private static readonly object _lockConnecting = new object();
        private static readonly object _lockSerializing = new object();

        private static readonly Dictionary<string, IPAddress> hostCache = new Dictionary<string, IPAddress>();

        #endregion

        #endregion

        #region Publicos

        public ServiciosAdicionalSocketBidireccional()
        {
            this.m_buffer = new byte[ConstantesSocket.MAX_BUFFER_SIZE];
            this.m_context = AsyncOperationManager.CreateOperation(null);
            //this.m_timer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            //this.m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        //void m_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    System.Timers.Timer _inner = (sender as System.Timers.Timer);
        //    try
        //    {
        //        lock (_lockSocket)
        //        {
        //            _inner.Stop();

        //        again:
        //            if (!this.m_socket.IsConnected())
        //            {
        //                if (this.m_isConnecting)
        //                {
        //                    Thread.Sleep(TimeSpan.FromSeconds(5));
        //                    goto again;
        //                }
        //                this.m_context.Post(_ => reconnect(), null);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        if (_inner.Interval <= TimeSpan.FromMinutes(5).TotalMilliseconds)
        //        {
        //            _inner.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
        //        }
        //        _inner.Start();
        //    }
        //}

        [SecurityCritical]
        public void Disconnect()
        {
            this.ID = default(string);

            if (this._remoteEndPoint != null)
            {
                this._remoteEndPoint = null;
            }

            if (this.m_context != null)
            {
                this.m_context = null;
            }

            this.closeSocket();
            this.m_isConnecting = false;
        }

        public IPEndPoint GetRemoteEndPoint()
        {
            lock (_lockSocket)
            {
                if (this.m_socket == null) return null;
                return (this.m_socket.IsConnected()) ? this.m_socket.RemoteEndPoint as IPEndPoint
                                                     : this._remoteEndPoint;
            }
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.Synchronized)]
        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        public void Connect(string host, int port, string id)
        {
            // Detiene la ejecución de otros hilos (locales) hasta que se realize la conexion
            lock (_lockConnecting)
            {
                if (this.m_isConnecting) return;
                this.m_isConnecting = true;
            }

            try
            {
                lock (_lockSocket)
                {
                    // Validar socket
                    if (this.m_socket != null)
                    {
                        if (this.isInternetAvailable() && this.m_socket.IsConnected(false)) return;
                    }

                    this.ID = id;

                    IPAddress innerHost = this.getHost(host);
                    this._remoteEndPoint = new IPEndPoint(innerHost, port);
                    TimeSpan awaitTime = TimeSpan.FromSeconds(10);

                again:
                    // Bandera para evitar el acceso a otros hilos (locales)
                    this.m_isConnecting = true;
                    this.closeSocket();

                    this.m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        this.m_socket.NoDelay = true;
                        //this.m_socket.Blocking = false;
                        this.m_socket.UseOnlyOverlappedIO = true;
                        this.m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        this.m_socket.SetSocketKeepAliveValues(1000, 3000);

                        // Asignacion de variables de buffer y timeouts
                        this.m_socket.ReceiveBufferSize =
                                this.m_socket.SendBufferSize = ConstantesSocket.MAX_BUFFER_SIZE;
                        this.m_socket.ReceiveTimeout =
                            this.m_socket.SendTimeout = ConstantesSocket.MAX_TIME_OUT;
                    }
                    catch //(ObjectDisposedException ob)
                    {
                        //_u.GuardarEnLog(ob);
                        this.closeSocket();
                        Thread.Sleep(awaitTime);
                        goto again;
                    }

                    try
                    {
                        // Despues de que se termine la operacion de conexion inicial
                        // esta validacion impedira un nuevo intento de conexion
                        // (ocurria que se enviaba 2 veces el intento de conexion a Servicios Web)
                        if (this.isInternetAvailable() && !this.m_socket.IsConnected())
                        {
                            this.m_socket.Connect(this._remoteEndPoint);

                            // Envia los datos de sincronización con Servicios Web
                            if (!this.sendSyn())
                            {
                                Thread.Sleep(500);// Medio segundo para reintentar
                                goto again;
                            }
                        }
                        else
                        {
                            // Si no es posible una comunicación adecuada se cierra el socket actual
                            this.internalCloseSocket(this.m_socket);
                            this.m_socket = null;
                            // Se detiene por 30 segundo para dar tiempo de rehabilitacion,
                            // ademas de evitar consumos de recursos innecesarios
                            Thread.Sleep(awaitTime);
                            // Repite la operacion de conexion como si fuera la primera vez de la comunicación
                            goto again;
                        }
                    }
                    catch //(Exception e)
                    {
                        //_u.GuardarEnLog(e);
                        this.closeSocket();
                        Thread.Sleep(awaitTime);
                        goto again;
                    }
                }
            }
            finally
            {
                //this.initializeTimer();
                this.m_isConnecting = false;
            }
        }

        #endregion

        #region Privados

        private void initializeTimer()
        {
            if (!this.f_isTimerInitialized)
            {
                //this.m_timer.Start();
                //this.m_timer.Enabled = true;
                this.f_isTimerInitialized = true;
            }
        }

        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        private bool sendSyn()
        {
            SocketError error = SocketError.Success;
            try
            {
                StateObjectBidireccional state = new StateObjectBidireccional();
                state.WorkSocket = this.m_socket;
                state.Id = this.ID;

                state.Buffer = state.SetSynEtxToBuffer(ConstantesSocket.LOCAL_ENCODING.GetBytes(this.ID.Replace("E", string.Empty).Trim())
                                               .Concat(ConstantesSocket.LOCAL_ENCODING.GetBytes(Assembly.GetExecutingAssembly()
                                                                                                        .GetName()
                                                                                                        .Version
                                                                                                        .ToString()
                                                                                                        .Replace(".", string.Empty).Trim())).ToArray());
                IAsyncResult asyncSend = this.m_socket.BeginSend(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, out error, null, null);
                if (!this.m_socket.Blocking && asyncSend != null)
                {
                    if (!asyncSend.IsCompleted) { asyncSend.AsyncWaitHandle.WaitOne(this.m_socket.SendTimeout); }
                    this.m_socket.EndSend(asyncSend, out error);
                }
                state.ClearBuffer();
                if (error == SocketError.Success)
                {
                    this.m_socket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, out error, new AsyncCallback(this.receiveCallback), state);
                    return true;
                }

                return false;
            }
            catch //(Exception e)
            {
                //_u.GuardarEnLog(e);
                return false;
            }
        }

        bool f_isReconnecting = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void reconnect()
        {
            lock (_lock)
            {
                if (this.f_isReconnecting) { return; }
                this.f_isReconnecting = true;
            }

            try
            {
                IPEndPoint host = this.GetRemoteEndPoint();
                if (host == null) return;
                //Thread.Sleep(TimeSpan.FromSeconds(5));
                this.Connect(host.Address.ToString(), host.Port, this.ID);
            }
            finally
            {
                this.f_isReconnecting = false;
            }
        }

        private bool isInternetAvailable()
        {
            int description;
            bool internet = InternetGetConnectedState(out description, 0);
            //if (!internet)
            //{
            //    try
            //    {
            //        SiAuto.Main.LogSeparator();
            //        SiAuto.Main.EnterMethod("private bool isInternetAvailable()");
            //        SiAuto.Main.LogWarning("No hay conexión a Internet");
            //    }
            //    finally
            //    {
            //        SiAuto.Main.LeaveMethod("private bool isInternetAvailable()");
            //    }
            //}
            return internet;
        }

        private IPAddress getHost(string host)
        {
            lock (hostCache)
            {
                if (hostCache.ContainsKey(host)) return hostCache[host];

                IPAddress _inner = Dns.GetHostAddresses(host).FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);
                hostCache.Add(host, _inner);
                return _inner;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private byte[] request(StateObjectBidireccional state)
        {
            RespuestaAdicional respuesta = new RespuestaAdicional();
            SolicitudAdicional peticion = null;

            if (state.Buffer.Length <= 0)
            {
                lock (_lockSerializing)
                {
                    respuesta.Resultado = Serializador.Serializar<Boolean>(true, ProtocoloSerializacion.Socket);
                    goto done;
                }
            }

            try
            {
                lock (_lockSerializing)
                {
                    peticion = Serializador.Deserializar<SolicitudAdicional>(state.Buffer, ProtocoloSerializacion.Socket);
                    respuesta.Metodo = peticion.Metodo;
                }
            }
            catch //(Exception e)
            {
                //_u.GuardarEnLog(e);
                respuesta.ReceiveFailure = true;
                goto done;
            }

            try
            {
                ServiciosAdicionalSocket servicio = new ServiciosAdicionalSocket();
                respuesta.Resultado = servicio.ProcesarPeticion(peticion);

                //if (!ConfigServicios.Host.Log.SoloErrores)
                //    _u.GuardarEnLog(peticion, respuesta);
            }
            catch (Exception e)
            {
                //_u.GuardarEnLog(e, peticion);

                respuesta.Error = true;
                respuesta.Excepcion = e.Message;
            }

        done:
            lock (_lockSerializing)
            {
                byte[] buffer = Serializador.Serializar<RespuestaAdicional>(respuesta, ProtocoloSerializacion.Socket);
                state.Buffer = state.SetStxEtxToBuffer(buffer);
                return state.Buffer;
            }
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        private void closeSocket()
        {
            if (this.m_socket == null) return;
            try
            {
                if (this.internalCloseSocket(this.m_socket))
                {
                    this.m_socket = null;
                }
            }
            catch { }
        }

        [SecurityCritical]
        private bool internalCloseSocket(Socket socket)
        {
            if (socket != null)
            {
                try { socket.Disconnect(false); }
                catch { }
                try { socket.Shutdown(SocketShutdown.Both); }
                catch { }
                try { socket.Close(0); }
                catch { }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void validateSocketConnection(StateObjectBidireccional state, ref SocketError error)
        {
            if ((error == SocketError.Success || ((int)error) < -1) && (state.WorkSocket != null && state.IsConnected()))
            {
                state.WorkSocket.BeginReceive(this.m_buffer, 0, this.m_buffer.Length,
                                              SocketFlags.None, out error,
                                              new AsyncCallback(this.receiveCallback), state);

                if (error != SocketError.Success)
                {
                    this.m_context.Post(_ => { this.reconnect(); }, null);
                }
            }
            else
            {
                this.m_context.Post(_ => { this.reconnect(); }, null);
            }
        }

        #region Callbacks

        [MethodImpl(MethodImplOptions.Synchronized)]
        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        private void sendCallbackInitial(IAsyncResult argument)
        {
            SocketError error = SocketError.Success;
            StateObjectBidireccional state = (argument.AsyncState as StateObjectBidireccional);

            try
            {
                Socket handler = state.WorkSocket;
                handler.EndSend(argument, out error);

                if (error == SocketError.Success)
                {
                    state.ClearBuffer();
                    state.BytesReceived = 0;
                    //handler.BeginReceive(this.m_buffer, 0, this.m_buffer.Length, SocketFlags.None, out error, new AsyncCallback(this.receiveCallback), state);
                }
                else
                {
                    error = SocketError.TryAgain;
                    state.ClearAll();
                }
            }
            //catch (Exception e)
            //{
            //    _u.GuardarEnLog(e);
            //}
            finally
            {
                this.validateSocketConnection(state, ref error);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        [HostProtection(Action = SecurityAction.Demand, Synchronization = true, ExternalThreading = true)]
        private void receiveCallback(IAsyncResult argument)
        {
            byte[] readBuffer = new byte[this.m_buffer.Length];
            try
            {
                lock (this.m_buffer)
                {
                    Buffer.BlockCopy(this.m_buffer, 0, readBuffer, 0, this.m_buffer.Length);
                    Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
                }

                SocketError error = SocketError.Success;
                int received = 0;
                StateObjectBidireccional state = (argument.AsyncState as StateObjectBidireccional);
                try
                {
                    Socket handler = state.WorkSocket;

                    try
                    {
                        received = handler.EndReceive(argument, out error);
                        Array.Resize(ref readBuffer, received);
                    }
                    catch //(Exception e)
                    {
                        //_u.GuardarEnLog(e);
                        if (handler != null)
                        {
                            this.internalCloseSocket(handler);
                        }
                        error = SocketError.TryAgain;
                        //this.m_context.Post(_ => this.reconnect(), null);
                        return;
                    }

                    try
                    {
                        if (received > 0)
                        {
                            bool isKeepAlive = (Array.IndexOf(readBuffer, ConstantesSocket.BEL_BYTE) >= 0);// Valida byte NUL (PING o KeepAlive)

                            if (!isKeepAlive)
                            {
                                bool isStx = Array.IndexOf(readBuffer, ConstantesSocket.STX_BYTE) >= 0; //Fin de mensaje
                                bool isEof = Array.IndexOf(readBuffer, ConstantesSocket.ETX_BYTE) >= 0; //Fin de mensaje
                                bool isRejected = Array.IndexOf(readBuffer, ConstantesSocket.ESC_BYTE) >= 0; // Socket rechazado

                                if (isRejected) // Rejected message
                                {
                                    Thread.Sleep(TimeSpan.FromMinutes(1));
                                    state.ClearBuffer();
                                    received =
                                        state.OffSet =
                                        state.BytesReceived = 0;
                                    error = SocketError.TryAgain;
                                    //this.m_context.Post(_ => this.reconnect(), null);
                                    return;
                                }
                                else if (isEof) // End message
                                {
                                    try
                                    {
                                        state.Buffer = state.GetCleanBuffer(readBuffer); ;
                                        readBuffer = this.request(state);
                                        state.BytesReceived =
                                            state.OffSet = 0;

                                        if (readBuffer != null && readBuffer.Length > 0)
                                        {
                                            /***** Procesamiento de solicitudes y envio de respuesta *****/
                                            handler.BeginSend(readBuffer, 0, readBuffer.Length, SocketFlags.None, out error, null, null).AsyncWaitHandle.WaitOne(handler.SendTimeout);
                                            if (error != SocketError.Success)
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        state.ClearBuffer();
                                        state.BytesReceived =
                                            state.OffSet =
                                            received = 0;
                                    }
                                }
                                else if (isStx) // Start Message
                                {
                                    byte[] receivedBuffer = new byte[0];
                                    using (MemoryStream readMem = new MemoryStream())// Almacenamiento
                                    {
                                        using (BufferedStream readBuf = new BufferedStream(readMem))
                                        {
                                            int readCount = received;
                                            int offSet = 0;
                                            IAsyncResult asyncReceive = null;
                                            do
                                            {
                                                if (error != SocketError.Success)
                                                {
                                                    //Gurock.SmartInspect.SiAuto.Main.LogError("Error de comunicación {0}", error);
                                                    break;
                                                }

                                                readBuf.Write(readBuffer, offSet, readCount);
                                                offSet += readCount;

                                                if (Array.IndexOf(readBuffer, ConstantesSocket.ETX_BYTE) > 0) break;

                                                asyncReceive = state.WorkSocket.BeginReceive(readBuffer, 0, readBuffer.Length, SocketFlags.None, out error, null, null);
                                                if (!asyncReceive.IsCompleted)
                                                {
                                                    asyncReceive.AsyncWaitHandle.WaitOne();
                                                }

                                            } while ((readCount = state.WorkSocket.EndReceive(asyncReceive, out error)) > 0);

                                            readBuf.Flush();
                                            readMem.Flush();
                                            readMem.Seek(0, SeekOrigin.Begin);
                                            readMem.Position = 0;
                                            receivedBuffer = readMem.ToArray();
                                        }
                                    }

                                    try
                                    {
                                        state.Buffer = state.GetCleanBuffer(receivedBuffer); ;
                                        receivedBuffer = this.request(state);
                                        state.BytesReceived =
                                            state.OffSet = 0;

                                        if (receivedBuffer != null && receivedBuffer.Length > 0)
                                        {
                                            /***** Procesamiento de solicitudes y envio de respuesta *****/
                                            handler.BeginSend(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, out error, null, null).AsyncWaitHandle.WaitOne(handler.SendTimeout);
                                            if (error != SocketError.Success)
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        state.ClearBuffer();
                                        state.BytesReceived =
                                            state.OffSet =
                                            received = 0;

                                        if (receivedBuffer != null)
                                        {
                                            Array.Clear(receivedBuffer, 0, receivedBuffer.Length);
                                            Array.Resize(ref receivedBuffer, 0);
                                            receivedBuffer = null;
                                        }
                                    }
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
                                state.OffSet =
                                    received = 0;
                        }
                    }
                    catch //(Exception e)
                    {
                        //_u.GuardarEnLog(e);
                        if (error != SocketError.Success)
                        {
                            if (handler != null)
                            {
                                this.internalCloseSocket(handler);
                            }
                        }
                    }
                }
                finally
                {
                    this.validateSocketConnection(state, ref error);
                }
            }
            finally
            {
                if (readBuffer != null)
                {
                    Array.Clear(readBuffer, 0, readBuffer.Length);
                    Array.Resize(ref readBuffer, 0);
                    readBuffer = null;
                }
            }
        }

        #endregion

        #endregion
    }
}
