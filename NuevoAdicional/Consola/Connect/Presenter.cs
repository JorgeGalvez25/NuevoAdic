using System;
using System.IO.Ports;
using System.Text;
using Consola.Logic;
using Consola.Logic.Entities;
using System.Threading;
using System.IO;
using System.Windows;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;

namespace Consola.Connect
{
    public class Presenter
    {
        private SerialPort puerto;
        public int marca;

        public const byte SOH = 1;
        public const byte STX = 2;
        public const byte ETX = 3;
        public const byte EOT = 5;
        public const byte ACK = 6;
        public const byte NAK = 21;
        public const byte CAN = 24;
        public const byte ba = 97;
        public const byte b1 = 49;
        public const byte bi = 105;
        public const byte be = 101;

        [DllImport("DllPSerial.dll", EntryPoint = "EnviaComando", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern string EnviaComando(string comando, int nopuerto, int baudios, char paridad, int bitsdatos, int bitparo);

        public Presenter()
        {
            this.puerto = new SerialPort("COM1");
            //this.puerto.DataReceived += this.puerto_DataReceived;
            //this.puerto.ErrorReceived += this.puerto_ErrorReceived;
        }

        //private void puerto_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        //{
        //    System.Windows.Forms.MessageBox.Show(e.EventType.ToString());
        //}

        //public void puerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    SerialPort s = (SerialPort)sender;
        //    //string data = s.ReadExisting();

        //    //int buffLength = this.puerto.BytesToRead;
        //    //byte[] buffer = new byte[buffLength];
        //    //this.puerto.Read(buffer, 0, buffer.Length);


        //    //System.Windows.Forms.MessageBox.Show(e.EventType.ToString() + ": " + data + "\r\n" + Encoding.ASCII.GetString(buffer));
        //}

        public void CreateConnection(SerialConnectionConfig cfg)
        {
            if (cfg == null) { return; }

            if (this.puerto == null) { this.puerto = new SerialPort(); }
            if (!this.puerto.IsOpen)
            {
                this.puerto.BaudRate = (int)cfg.BaudRate;
                this.puerto.DataBits = cfg.DataBits;
                this.puerto.Parity = cfg.Parity;
                this.puerto.PortName = cfg.PortName;
                this.puerto.StopBits = cfg.StopBits;

                // Valores default
                this.puerto.ReadTimeout = 1000 * 2;
                this.puerto.WriteTimeout = 1000 * 2;
            }
        }

        /// <summary>
        /// Permite el envio de la información al puerto
        /// </summary>
        /// <exception cref="Exception">Arroja esta exceción en caso de que no exista una configuración previa</exception>
        /// <param name="message">Mensaje a enviar</param>
        /// <returns>Retorna el mensaje de respuesta del dispositivo conectado</returns>
        public byte[] Send(string message, bool cerrar, SerialConnectionConfig cfg)
        {
            if (this.puerto == null) { throw new Exception("-1: No se ha configurado el puerto."); }
            byte[] buffer = null;
            int cont = 3, cont2 = 4;
            try
            {
                while (true)
                {
                    try
                    {
                        if (!this.puerto.IsOpen) { this.puerto.Open(); }
                        string comando = "";
                        comando = ProcessMessage(message);
                        if (ConfigurationManager.AppSettings["Bitacora"] == "Si")
                        {
                            RegistraBitacora("ENVIO   ", comando);
                            RegistraBitacora("ENVHEX  ", ProcesaMensajeHex(Encoding.Default.GetBytes(comando)));
                        }
                        char paridad;
                        switch (cfg.Parity)
                        {
                            case Parity.Even:
                                paridad = 'E';
                                break;
                            case Parity.Odd:
                                paridad = 'O';
                                break;
                            case Parity.None:
                                paridad = 'N';
                                break;
                        }
                        this.puerto.Write(Encoding.Default.GetBytes(comando), 0, Encoding.Default.GetByteCount(comando));
                        System.Threading.Thread.Sleep(400);
                        int bufflength;
                        while (true)
                        {
                            bufflength = this.puerto.BytesToRead;
                            if (bufflength > 0 || --cont2 == 0)
                                break;
                            if (cont2 % 2 != 0)
                                Thread.Sleep(150);
                            else
                            {
                                RegistraBitacora("ENVIOREINT   ", comando);
                                RegistraBitacora("ENVHEXREINT   ", ProcesaMensajeHex(Encoding.Default.GetBytes(comando)));
                                this.puerto.Write(comando);
                                Thread.Sleep(500);
                            }
                        }

                        buffer = new byte[bufflength];
                        this.puerto.Read(buffer, 0, buffer.Length);

                        if (ConfigurationManager.AppSettings["Bitacora"] == "Si")
                        {
                            RegistraBitacora("RESP    ", Encoding.ASCII.GetString(buffer).Trim());
                            RegistraBitacora("RESPHEX ", ProcesaMensajeHex(buffer).Trim());
                        }

                        if (buffer.Length > 0 && --cont2 > 0)
                        {
                            if (buffer.Length >= 3 && marca == 1)
                            {
                                if (!(buffer[1] == ba && buffer[2] == b1))
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }
                            }
                            else if (marca == 3)
                            {
                                if (buffer[buffer.Length - 2] != 7 && buffer[buffer.Length - 2] != 1)
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }
                            }
                            else if (marca == 5)
                            {
                                if (buffer.Length != 3)
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }
                            }
                            else
                            {
                                if (buffer.Length < 4 && buffer[0] != ACK)
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }
                                else if (buffer.Length > 4 && buffer[1] != 64)
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }
                            }
                        }
                        if (buffer.Length == 0 && --cont2 > 0)
                        {
                            Thread.Sleep(250);
                            continue;
                        }
                        break;
                    }
                    catch (Exception e)
                    {
                        RegistraBitacora("Puerto: ", e.Message + e.TargetSite + e.StackTrace);
                        if ((--cont) <= 0)
                        {
                            this.puerto.DiscardInBuffer(); this.puerto.Close();
                            throw new Exception(string.Format("-2: {0}", e.Message));
                        }
                        else
                        {
                            if (e.Message.Substring(0, 9) == "Attempted" || e.Message.Substring(0, 7) == "Intento")
                                Thread.Sleep(250);
                            else
                                Thread.Sleep(200);
                        }
                    }
                }
            }
            finally
            {
                if (this.puerto.IsOpen && cerrar)
                {
                    this.puerto.DiscardInBuffer(); this.puerto.Close();
                }
            }
            return buffer;
        }

        public string Send(List<string> message, SerialConnectionConfig cfg)
        {
            int cont = 3;
            string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ruta = Path.GetDirectoryName(ruta);
            if (System.IO.File.Exists(ruta + (message[0].Substring(0, 1) == "&" ? @"\comandosc.txt" : @"\comandos.txt")))
            {
                try
                {
                    System.IO.File.Delete(ruta + (message[0].Substring(0, 1) == "&" ? @"\comandosc.txt" : @"\comandos.txt"));
                }
                catch (System.IO.IOException e)
                {
                    RegistraBitacora("ERROR: ", e.Message);
                }
            }
            foreach (string cmnd in message)
            {
                GuardaCmnTxt(cmnd);
            }
            char paridad = 'N';
            switch (cfg.Parity)
            {
                case Parity.Even:
                    paridad = 'E';
                    break;
                case Parity.Odd:
                    paridad = 'O';
                    break;
                case Parity.None:
                    paridad = 'N';
                    break;
            }
            try
            {
                while (true)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = ruta + @"\PSerial.exe";
                        p.StartInfo.Arguments = marca.ToString() + " " + cfg.PortName.Substring(3, 1) + " " + cfg.BaudRate.ToString().TrimStart('B') + " " +
                                                paridad + " " + cfg.DataBits.ToString() + " " + Convert.ToInt32(cfg.StopBits).ToString("0");
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();
                        string respuesta = p.ExitCode == 1 ? "ok" : "error";
                        p.Close();
                        p.Dispose();
                        p = null;

                        RegistraBitacora("RESP    ", respuesta);
                        if (respuesta == "ok")
                            return "ok";
                        else if (--cont == 0)
                            return "error";
                        else Thread.Sleep(300);
                    }
                    catch (Exception e)
                    {
                        cont--;
                        RegistraBitacora("ERROR: ", e.Message);
                        if (--cont == 0)
                            return "error";
                    }
                }
            }
            finally
            {
                if (System.IO.File.Exists(ruta + @"\comandosc.txt"))
                {
                    try
                    {
                        System.IO.File.Delete(ruta + @"\comandosc.txt");
                    }
                    catch (System.IO.IOException e)
                    {
                        RegistraBitacora("ERROR: ", e.Message);
                    }
                }
            }
        }

        private string ProcessMessage(string message)
        {
            string result = "";
            switch (marca)
            {
                case 1:
                    result = Gilbarco(message);
                    break;
                case 2:
                    result = Bennett(message);
                    break;
                case 3:
                    result = Team(message);
                    break;
                case 4:
                    result = Gilbarco(message);
                    break;
                case 5:
                    result = HongYang(message);
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        private string Gilbarco(string message)
        {
            // Start of Text - STX
            char stx = (char)STX;
            // End of Text - ETX
            char etx = (char)ETX;
            string data = string.Format("{0}{1}{2}", stx, message, etx);

            char xc = data[0];
            char cc = char.MinValue;
            for (int i = 0; i < data.Length; i++)
            {
                cc = data[i];
                xc ^= cc;
            }

            data = string.Format("{0}{1}{2}{3}", stx, message, etx, xc);

            return data;
        }

        private string Bennett(string message)
        {
            // Start of Text - STX
            char stx = (char)STX;
            // End of Text - ETX
            char etx = (char)ETX;
            string data = string.Format("{0}{1}{2}", stx, message, etx);
            string confirm = message + etx;
            char xc;
            int n = 0;
            for (int i = 0; i < confirm.Length; i++)
            {
                n += (int)confirm[i];
            }
            int m = n % 128;
            xc = (char)(128 - m);

            data = string.Format("{0}{1}{2}{3}", stx, message, etx, xc);

            return data;
        }

        private string Team(string message)
        {
            string sb = "";
            for (int i = 0; i <= message.Length - 2; i += 2)
            {
                sb += Convert.ToChar(Int32.Parse(message.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }
            return sb;
        }

        private string HongYang(string message)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= message.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(message.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }

            char xc;
            int n = 0;
            for (int i = 0; i < (sb.ToString()).Length; i++)
            {
                n += (int)(sb.ToString())[i];
            }
            int m = n % 256;
            xc = (char)(256 - m);

            return sb.ToString() + xc;
        }

        public void RegistraBitacora(string tipo, string mensaje)
        {
            string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ruta = Path.GetDirectoryName(ruta);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyy hh:mm:ss.fff ") + string.Format("{0,-8}", tipo) + mensaje);

            using (StreamWriter outfile = new StreamWriter(ruta + @"\bitacora.txt", true))
            {
                outfile.Write(sb.ToString());
            }

            sb = null;
        }

        public void GuardaCmnTxt(string comando)
        {
            string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ruta = Path.GetDirectoryName(ruta);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(comando);

            using (StreamWriter outfile = new StreamWriter(ruta + (comando.Substring(0, 1) == "&" ? @"\comandosc.txt" : @"\comandos.txt"), true))
            {
                outfile.Write(sb.ToString());
            }

            sb = null;
        }

        private string ProcesaMensajeHex(byte[] buffer)
        {
            StringBuilder hex = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposable)
        {
            if (disposable)
            {
                if (this.puerto != null)
                {
                    if (this.puerto.IsOpen)
                    {
                        this.puerto.Close();
                        //this.puerto.DataReceived -= this.puerto_DataReceived;
                        //this.puerto.ErrorReceived -= this.puerto_ErrorReceived;
                    }
                    this.puerto.Dispose();
                    this.puerto = null;
                }

                if (_logica != null)
                {
                    _logica.UnListenPosClie();
                    _logica = null;
                }
            }
        }

        ~Presenter()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region SerivicioLogica

        private static SerivicioLogica _logica = new SerivicioLogica();

        public void CreaListener(Action<string> fn)
        {
            _logica.ListenPosClie(fn);
        }

        public void CerrarListener()
        {
            _logica.UnListenPosClie();
        }

        public bool ActualizarPocision(FiltroDPVGCONF f)
        {
            return _logica.ActualizarPosClie(f);
        }

        public ListaDPVGTCMB ObtenerTodosCombustible(FiltroDPVGTCMB f)
        {
            return _logica.ObtenerTodosCombustible(f);
        }

        public DPVGTCMB ActualizarCombustible(DPVGTCMB e)
        {
            return _logica.ActualizarCombustible(e);
        }

        public ListaDPVGCMND ObtenerTodosComandos(FiltroDPVGCMND f)
        {
            return _logica.ObtenerTodosComandos(f);
        }

        public ListaDPVGESTS ObtenerTodosDPVGESTS(FiltroDPVGESTS f)
        {
            return _logica.ObtenerTodosDPVGESTS(f);
        }

        public SerialConnectionConfig GetSerialConfig()
        {
            return _logica.GetSerialConfig();
        }

        #endregion
    }
}
