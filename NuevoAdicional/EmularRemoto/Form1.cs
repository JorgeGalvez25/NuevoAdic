using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EmularRemoto
{
    public partial class Form1 : Form
    {
        private string ipAddres = "192.168.0.162";//"192.168.1.64";//"192.168.0.67";
        private int puerto = 9091;
        private IPEndPoint endPoint;
        private Socket socket;
        private Socket handler;
        private Thread hiloEscuchar;
        private byte[] trama2 = new byte[] { 0x7E, 0x00, 0x12, 0x92, 0x00, 0x13, 0xA2, 0x00, 0x00, 0x40, 0x91, 0xC0, 0x01, 0xFF, 0xFE, 0x02, 0x01, 0x00, 0x01, 0x00, 0x00, 0x24 };
        private byte[] trama3 = new byte[] { 0x7E, 0x00, 0x12, 0x92, 0x00, 0x13, 0xA2, 0x00, 0x00, 0x40, 0x91, 0xC0, 0x01, 0xFF, 0xFE, 0x02, 0x01, 0x00, 0x01, 0x00, 0x00, 0x24 };
        private byte[] trama4 = new byte[] { 0x7E, 0x00, 0x12, 0x92, 0x00, 0x13, 0xA2, 0x00, 0x00, 0x40, 0x91, 0xC0, 0x01, 0xFF, 0xFE, 0x02, 0x01, 0x00, 0x01, 0x00, 0x00, 0x24 };
        private bool saliendo = false;

        private delegate void escribirTexto(Control caja, string texto);

        private void escribirInfo(Control caja, string texto)
        {
            TextBox cajaTexto = (TextBox)caja;
            cajaTexto.Text += texto + "\r\n";
            cajaTexto.Select(cajaTexto.Text.Length, 1);
            cajaTexto.ScrollToCaret();
        }

        public Form1()
        {
            InitializeComponent();

            endPoint = new IPEndPoint(IPAddress.Parse(ipAddres), puerto);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            lblEstadoSocket.Text = "Socket Desconectado";
            gbRemotos.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Thread hiloConectar = new Thread(new ThreadStart(conectar));
            hiloConectar.Start();
            lblEstadoSocket.Text = "Conectando Socket...";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            saliendo = true;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
            }
            catch {  }

            base.OnClosing(e);
        }

        private void conectar()
        {
            while (!socket.Connected && !saliendo)
            {
                try
                {
                    socket.Bind(endPoint);
                    socket.Listen(5);
                    lblEstadoSocket.Text = "Socket conectado a: " + ipAddres;
                    gbRemotos.Enabled = true;

                    hiloEscuchar = new Thread(new ThreadStart(leerSocket));
                    hiloEscuchar.Start();
                }
                catch (SocketException se)
                {
                    //MessageBox.Show(se.ErrorCode.ToString() + " " + se.Message);
                }
                catch (Exception) { }
            }
        }

        private void leerSocket()
        {
            List<byte> receive = new List<byte>();
            int checkSum = 0x00;
            int lastByte = 0x00;
            Int16 longitud = 0x00;
            byte numByte = 0x00;

            while (socket.Connected)
            {
                try
                {
                    if (!socket.Poll(100, SelectMode.SelectRead))
                    {
                        continue;
                    }

                    byte[] byteRecibido = new byte[1];
                    //socket.ReceiveTimeout = 2000;
                    socket.Receive(byteRecibido, 1, SocketFlags.Partial);

                    if (lastByte == 0x7E)
                    {
                        longitud = (Int16)(byteRecibido[0] << 8);
                        numByte = 1;
                        receive = new List<byte>();
                    }
                    else if (numByte == 1)
                    {
                        longitud += byteRecibido[0];
                        numByte++;
                    }
                    else if ((numByte - 2) < longitud)
                    {
                        checkSum += byteRecibido[0];
                        numByte++;

                        receive.Add(byteRecibido[0]);
                    }
                    else if ((numByte - 2) == longitud)
                    {
                        //Disparar Evento
                        //InterpretarComando(receive.ToArray());
                        numByte = 0;
                    }

                    lastByte = byteRecibido[0];
                }
                catch (SocketException se)
                {
                    MessageBox.Show(se.ErrorCode.ToString());
                }
                catch (Exception)
                {
                    if (!saliendo)
                    {
                        lblEstadoSocket.Text = "Error en Socket... Reconectando...";
                        socket.Close();
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        while (!socket.Connected && !saliendo)
                        {
                            try
                            {
                                socket.Connect(endPoint);
                            }
                            catch { }
                        }

                        lblEstadoSocket.Text = "Socket conectado a: " + ipAddres;
                    }
                }
            }
        }

        private void chkRemoto_CheckedChanged(object sender, EventArgs e)
        {
            int tag = (int)((CheckBox)sender).Tag;

            switch (tag)
            {
                case 1:
                    {
                        if (chkRemoto1.Checked)
                        {
                            byte[] trama = new byte[] { 0x7E, 0x00, 0x12, 0x92, 0x00, 0x13, 0xA2, 0x00, 0x00, 0x40, 0x91, 0xC0, 0x01, 0xFF, 0xFE, 0x02, 0x01, 0x00, 0x01, 0x00, 0x00, 0x24 };

                            // Iniciar el hilo de envío
                            bwRemoto1.RunWorkerAsync(trama);
                        }
                        else
                        {
                            // Finalizar el hilo de envío
                            bwRemoto1.CancelAsync();
                        }
                    } break;
                default:
                    break;
            }
        }

        private void bwRemoto1_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] trama = (byte[])e.Argument;
            string texto = string.Empty;

            while (!bwRemoto1.CancellationPending || !saliendo)
            {
                if (socket.Poll(100, SelectMode.SelectWrite))
                {
                    socket.Send(trama);
                    texto = "7E 00 12 92 00 13 A2 00 40 91 C0 01 FF FE 02 01 00 01 00 00 01 24";
                    this.Invoke(new escribirTexto(escribirInfo), txtComandosEnviados, texto);

                    socket.Send(trama);
                    texto = "7E 00 12 92 00 13 A2 00 40 91 C0 01 FF FE 02 01 00 01 00 00 01 24";
                    this.Invoke(new escribirTexto(escribirInfo), txtComandosEnviados, texto);

                    Thread.Sleep(2000);
                }
            }
        }

        private void bwRemoto2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bwRemoto3_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bwRemoto4_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
