using Adicional.Entidades;
using ImagenSoft.ModuloWeb.Entidades;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ImagenSoft.ModuloWeb.Servicios.WCF.Sockets
{
    public class SocketServer
    {
        #region Variables

        private static DateTime FechaLiberacionVersion = new DateTime(2014, 12, 16);
        private static DateTime FirebirdDateTimeMinValue = new DateTime(1753, 1, 1);
        private static byte[] SocketEnd = new byte[] { 60, 33, 45, 45, 69, 78, 68, 45, 45, 62 };

        private int _maxTimeout;
        private int MaxTimeout
        {
            get
            {
                if (_maxTimeout <= 0)
                {
                    int maxTimeout = 0;
                    if (int.TryParse((ConfigurationManager.AppSettings["MaxTimeout"] ?? "0"), out maxTimeout))
                    {
                        maxTimeout = 300000;
                    }

                    _maxTimeout = (maxTimeout <= 0) ? 300000 : maxTimeout;
                }

                return _maxTimeout;
            }
        }

        private int _maxBufferSize;
        private int MaxBufferSize
        {
            get
            {
                if (_maxBufferSize <= 0)
                {
                    int maxBufferSize = 0;
                    if (int.TryParse((ConfigurationManager.AppSettings["MaxBufferSize"] ?? "0"), out maxBufferSize))
                    {
                        maxBufferSize = 1048576;
                    }

                    _maxBufferSize = (maxBufferSize <= 0) ? 1048576 : maxBufferSize;
                }

                return _maxBufferSize;
            }
        }

        private int _port;
        private int Port
        {
            get
            {
                if (_port <= 0)
                {
                    int port = 0; ;
                    if (!int.TryParse((ConfigurationManager.AppSettings["Port"] ?? "0"), out port))
                    {
                        port = 808;
                    }

                    _port = (port <= 0) ? 808 : port;
                }

                return _port;
            }
        }

        private static bool isOpen = false;
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        Socket listener = null;
        BackgroundWorker hilo = null;

        #endregion

        #region Publicos

        public void Open()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.ReceiveBufferSize = this.MaxBufferSize;
            listener.ReceiveTimeout = this.MaxTimeout;
            listener.SendBufferSize = this.MaxBufferSize;
            listener.SendTimeout = this.MaxTimeout;

            listener.Bind(new IPEndPoint(IPAddress.Any, this.Port));
            listener.Listen(500);

            hilo = new BackgroundWorker() { WorkerSupportsCancellation = true };
            hilo.DoWork += hilo_DoWork;
            hilo.RunWorkerAsync(listener);
        }

        public void Close()
        {
            try
            {
                isOpen = false;

                hilo.CancelAsync();
                hilo.DoWork -= hilo_DoWork;
                hilo.Dispose();
                hilo = null;

                if (listener.Connected)
                {
                    listener.Disconnect(false);
                    listener.Shutdown(SocketShutdown.Both);
                }

                listener.Close();
                listener = null;
            }
            finally
            {
                MensajesRegistros.Informacion("Servicio Socket DETENIDO");
            }
        }

        #endregion

        #region Eventos

        void hilo_DoWork(object sender, DoWorkEventArgs e)
        {
            Socket listener = (e.Argument as Socket);

            isOpen = true;
            MensajesRegistros.Informacion("Escuchando: ", (listener.LocalEndPoint as IPEndPoint).Port);
            while (isOpen)
            {
                if (e.Cancel) break;
                allDone.Reset();
                listener.BeginAccept(new AsyncCallback(acceptCallback), listener);
                allDone.WaitOne();
            }
        }

        void acceptCallback(IAsyncResult argument)
        {
            if (isOpen)
            {
                allDone.Set();

                Socket listener = (argument.AsyncState as Socket);
                Socket handler = listener.EndAccept(argument);

                StateObject state = new StateObject();
                state.Buffer = new byte[0];
                state.WorkSocket = handler;

                try
                {
                    try
                    {
                        byte[] buffer = new byte[handler.ReceiveBufferSize];

                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            int len = 0;
                            int count = 0;

                        continueReading:
                            len = 0;

                            do
                            {
                                Thread.Sleep(1);
                                len = handler.Available;
                                count++;
                                if (count >= 10000)
                                {
                                    count = -1;
                                    break;
                                }
                            } while (len <= 0);

                            while (handler.Available > 0)
                            {
                                len = handler.Receive(buffer);
                                resultStream.Write(buffer, 0, len);
                            }

                            state.Buffer = resultStream.ToArray();

                            if (!System.Text.Encoding.UTF8.GetString(state.Buffer).Trim().EndsWith("<!--END-->"))
                            {
                                if (count >= 0)
                                {
                                    count = 0;
                                    goto continueReading;
                                }
                            }
                        }

                        state.Buffer = request(state.Buffer, state.OffSet);
                        state.OffSet = 0;

                        try
                        {
                            handler.BeginSend(state.Buffer, state.OffSet, state.Buffer.Length, SocketFlags.None, new AsyncCallback(sendCallback), state);
                        }
                        catch (Exception e)
                        {
                            MensajesRegistros.Excepcion("Server_Socket", e);
                            ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                            //_u.GuardarEnLog(_c.Mensaje.ProblemaEnvioSocket, e, handler, state);

                            handler.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        MensajesRegistros.Excepcion("Server_Socket", e);
                        ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                        //_u.GuardarEnLog(_c.Mensaje.ProblemaEnvioSocket, e, handler, state);

                        handler.Close();
                    }
                }
                catch (Exception e)
                {
                    MensajesRegistros.Excepcion("Server_Socket", e);
                    ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                    //_u.GuardarEnLog(_c.Mensaje.ProblemaRecepcionSocket, e, handler, state);

                    handler.Close();
                }
            }
        }

        void receiveCallback(IAsyncResult argument)
        {
            StateObject state = (argument.AsyncState as StateObject);
            Socket handler = state.WorkSocket;

            try
            {
                state.BytesReceived = handler.EndReceive(argument);
            }
            catch (Exception e)
            {
                MensajesRegistros.Excepcion("Server_Socket", e);
                ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                //_u.GuardarEnLog(_c.Mensaje.ProblemaRecepcionSocket, e, handler, state);

                handler.Close();

                return;
            }

            if (state.BytesReceived > 0)
            {
                state.OffSet += state.BytesReceived;

                //Validar bandera <!--END--> en el mensaje.
                byte[] flag = new byte[10];
                Array.Copy(state.Buffer, state.OffSet < 10 ? 0 : state.OffSet - 10, flag, 0, 10);

                if (Enumerable.SequenceEqual(SocketEnd, flag))
                {
                    state.Buffer = request(state.Buffer, state.OffSet);
                    state.OffSet = 0;

                    try
                    {
                        handler.BeginSend(state.Buffer, state.OffSet, state.Buffer.Length, SocketFlags.None, new AsyncCallback(sendCallback), state);
                    }
                    catch (Exception e)
                    {
                        MensajesRegistros.Excepcion("Server_Socket", e);
                        ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                        //_u.GuardarEnLog(_c.Mensaje.ProblemaEnvioSocket, e, handler, state);

                        handler.Close();
                    }
                }
                else
                {
                    try
                    {
                        handler.BeginReceive(state.Buffer, state.OffSet, state.Buffer.Length - state.OffSet, SocketFlags.None, new AsyncCallback(receiveCallback), state);
                    }
                    catch (Exception e)
                    {
                        MensajesRegistros.Excepcion("Server_Socket", e);
                        ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                        //_u.GuardarEnLog(_c.Mensaje.ProblemaRecepcionSocket, e, handler, state);

                        handler.Close();
                    }
                }
            }
            else
            {
                handler.Close();
            }
        }

        void sendCallback(IAsyncResult argument)
        {
            StateObject state = (argument.AsyncState as StateObject);
            Socket handler = state.WorkSocket;

            try
            {
                state.OffSet += handler.EndSend(argument);
            }
            catch (Exception e)
            {
                MensajesRegistros.Excepcion("Server_Socket", e);
                ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                //_u.GuardarEnLog(_c.Mensaje.ProblemaEnvioSocket, e, handler, state);

                handler.Close();

                return;
            }

            if (state.OffSet < state.Buffer.Length)
            {
                try
                {
                    handler.BeginSend(state.Buffer, state.OffSet, state.Buffer.Length - state.OffSet, SocketFlags.None, new AsyncCallback(sendCallback), state);
                }
                catch (Exception e)
                {
                    MensajesRegistros.Excepcion("Server_Socket", e);
                    ////Si se presenta un problema de comunicación, se registra en bitácora y se cierra el socket.
                    //_u.GuardarEnLog(_c.Mensaje.ProblemaEnvioSocket, e, handler, state);

                    handler.Close();
                }
            }
            else
            {
                handler.Close();
            }
        }

        #endregion

        #region Metodos

        private byte[] request(byte[] buffer, int offSet)
        {
            ServiciosModuloWebSocket servicio = new ServiciosModuloWebSocket();
            buffer = servicio.EnviarPeticion(buffer);

            //Agregar bandera <!--END--> al mensaje.
            Array.Resize(ref buffer, buffer.Length + SocketEnd.Length);
            Array.Copy(SocketEnd, 0, buffer, buffer.Length - SocketEnd.Length, SocketEnd.Length);

            return buffer;
        }

        #endregion
    }
}
