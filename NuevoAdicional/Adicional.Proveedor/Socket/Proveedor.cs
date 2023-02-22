using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Adicional.Entidades;
using Adicional.Entidades.Bloqueos;
using Adicional.Entidades.SocketBidireccional;
using Adicional.Entidades.Web;

namespace Adicional.Proveedor.Sockets
{
    public class Proveedor
    {
        string host = string.Empty;
        int puerto = 0;
        int maxBufferSize = 0;
        int maxTimeout = 0;
        private StateObjectBidireccional m_state;
        private static byte[] SocketEnd = new byte[] { 60, 33, 45, 45, 69, 78, 68, 45, 45, 62 };

        public Proveedor()
            : this("127.0.0.1", 808)
        {

        }

        public Proveedor(string ip, int port)
            : this(ip, port, 1048576, 300000)
        {

        }

        public Proveedor(string host, int puerto, int maxBufferSize, int maxTimeout)
        {
            this.host = host;
            this.puerto = puerto;
            this.maxBufferSize = maxBufferSize;
            this.maxTimeout = maxTimeout;
        }

        public Proveedor(StateObjectBidireccional state)
        {
            this.m_state = state;
        }

        #region Privadas

        [MethodImpl(MethodImplOptions.Synchronized)]
        private RespuestaAdicional send(byte[] buffer)
        {
            //byte[] buffer = Serializador.Serializar(peticion, ProtocoloSerializacion.Socket);

            //Agregar bandera <!--END--> al mensaje.
            Array.Resize(ref buffer, buffer.Length + SocketEnd.Length);
            Array.Copy(SocketEnd, 0, buffer, buffer.Length - SocketEnd.Length, SocketEnd.Length);

        @send:
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

            RespuestaAdicional respuesta = null;

            try
            {
                respuesta = Serializador.Deserializar<RespuestaAdicional>(recept, ProtocoloSerializacion.Socket);

                if (respuesta.ReceiveFailure)
                    goto @send;

                if (respuesta.Error)
                    throw new Exception(respuesta.Excepcion);
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

        #endregion

        #region Cloud

        //public bool Ping()
        //{
        //    SolicitudAdicional solicitud = new SolicitudAdicional();
        //    solicitud.Metodo = MetodosAdicional.Ping;
        //    solicitud.Parametro = new byte[0];

        //    RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
        //    return Serializador.Deserializar<bool>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        //}

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string EstadoDelFlujo()
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.EstadoFlujo;
            solicitud.Parametro = new byte[0];

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<string>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public MarcaDispensario GetMarcaDispensario()
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.ObtenerTipoDispensario;
            solicitud.Parametro = new byte[0];

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<MarcaDispensario>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CambiarFlujo(FiltroCambiarFlujo filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.CambiarFlujo;
            solicitud.Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket);

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            string strResult = Serializador.Deserializar<string>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
            return strResult.Equals("Ok", StringComparison.OrdinalIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SetGlobal(FiltroMangueras filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.EstablecerPorcentajeGlobal;
            solicitud.Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket);

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<bool>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SetPorcentaje(FiltroMangueras filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.EstablecerPorcentaje;
            solicitud.Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket);

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<bool>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ListaHistorial GetMangueras(FiltroMangueras filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.ObtenerPorcentajes;
            solicitud.Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket);

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<ListaHistorial>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<int> GetManguerasPosicion(FiltroMangueras filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional();
            solicitud.Metodo = MetodosAdicional.ObtenerPorcentajesPosicion;
            solicitud.Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket);

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<List<int>>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Ping()
        {
            SolicitudAdicional peticion = new SolicitudAdicional();
            peticion.Metodo = MetodosAdicional.Ping;

            RespuestaAdicional respuesta = send(Serializador.Serializar(peticion, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<Boolean>(respuesta.Resultado as byte[], ProtocoloSerializacion.Socket);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Syn(byte[] buffer)
        {
            if (this.m_state != null)
            {
                SocketError error = SocketError.Success;
                this.m_state.WorkSocket.Send(buffer, 0, buffer.Length, SocketFlags.None, out error);
                return error == SocketError.Success;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<ReporteVentasCombustible> ObtenerReporteVentasCombustible(FiltroReporteVentasCombustible filtro)
        {
            SolicitudAdicional solicitud = new SolicitudAdicional()
            {
                Metodo = MetodosAdicional.ObtenerReporteVentasCombustible,
                Parametro = Serializador.Serializar(filtro, ProtocoloSerializacion.Socket)
            };

            RespuestaAdicional result = this.send(Serializador.Serializar(solicitud, ProtocoloSerializacion.Socket));
            return Serializador.Deserializar<List<ReporteVentasCombustible>>(result.Resultado as byte[], ProtocoloSerializacion.Socket);
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

            [MethodImpl(MethodImplOptions.Synchronized)]
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

            //void receiveCallback(IAsyncResult argument)
            //{
            //    try
            //    {
            //        StateObject state = (argument.AsyncState as StateObject);
            //        Socket handler = state.WorkSocket;

            //        state.BytesReceived = handler.EndReceive(argument);

            //        if (state.BytesReceived > 0)
            //        {
            //            byte[] buffer = new byte[handler.ReceiveBufferSize];

            //            using (MemoryStream resultStream = new MemoryStream())
            //            {
            //                int len = 0;
            //                int count = 0;

            //            continueReading:
            //                len = 0;

            //                do
            //                {
            //                    //Thread.Sleep(1);
            //                    len = handler.Available;
            //                    count++;
            //                    if (count >= 10000)
            //                    {
            //                        count = -1;
            //                        break;
            //                    }
            //                } while (len <= 0);

            //                while (handler.Available > 0)
            //                {
            //                    len = handler.Receive(buffer);
            //                    resultStream.Write(buffer, 0, len);
            //                }

            //                state.Buffer = resultStream.ToArray();

            //                if (!System.Text.Encoding.UTF8.GetString(state.Buffer).Trim().EndsWith("<!--END-->"))
            //                {
            //                    if (count >= 0)
            //                    {
            //                        count = 0;
            //                        goto continueReading;
            //                    }
            //                }
            //            }

            //            state.OffSet += state.Buffer.Length;

            //            ////Validar bandera <!--END--> en el mensaje.
            //            //byte[] flag = new byte[10];
            //            //Array.Copy(state.Buffer, state.OffSet < 10 ? 0 : state.OffSet - 10, flag, 0, 10);

            //            //if (Enumerable.SequenceEqual(SocketEnd, flag))
            //            //{
            //            //    allDone.Set();
            //            //}
            //            //else
            //            //{
            //            //    handler.BeginReceive(state.Buffer, state.OffSet, state.Buffer.Length - state.OffSet, SocketFlags.None, new AsyncCallback(receiveCallback), state);
            //            //}
            //        }
            //        //else
            //        //{
            //        //    allDone.Set();
            //        //}
            //        allDone.Set();
            //    }
            //    catch (Exception e)
            //    {
            //        this.excepcion = e;
            //        allDone.Set();
            //    }
            //}

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

            [MethodImpl(MethodImplOptions.Synchronized)]
            public byte[] Send(StateObjectBidireccional state)
            {
                SocketError error = SocketError.Success;

                IAsyncResult asyncSend = state.WorkSocket.BeginSend(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, out error, null, null);

                if (error == SocketError.Success && asyncSend != null)
                {
                    if (!asyncSend.IsCompleted)
                    {
                        asyncSend.AsyncWaitHandle.WaitOne(state.WorkSocket.SendTimeout);
                    }

                    state.WorkSocket.EndSend(asyncSend, out error);
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
                            int available = state.WorkSocket.Available;
                            try
                            {
                                while ((readCount = state.WorkSocket.EndReceive(state.WorkSocket.BeginReceive(_inner, 0, _inner.Length, SocketFlags.None, null, null), out error)) > 0)
                                {
                                    available = state.WorkSocket.Available;
                                    if (error != SocketError.Success) { break; }
                                    if (readCount == 0) { break; }

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
}


