using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Consola.Logic.Entities;
using System.Configuration;
using System.IO.Ports;
using System.IO;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace Consola.Connect
{
    public class PSerial
    {
        private Presenter Presenter { get; set; }
        string resultado;
        bool fin;
        List<string> comandos, valoresCal;

        public PSerial()
        {
            Presenter = new Presenter();
        }

        private void DoAction(string cmd, bool cerrar)
        {
            var result = this.DoOperation(cmd, cerrar);
            if (result.Length == 0 && Presenter.marca != 3)
            {
                resultado = "No hubo respuesta de la consola";
                return;
            }
            if (Presenter.marca == 1)
            {
                if (result[1] == Presenter.ba && result[2] == Presenter.b1)
                    resultado = "Ok";
            }
            else if (Presenter.marca == 3)
            {
                resultado = "Ok";
                //switch (result[result.Length - 2])
                //{
                //    case 1:
                //        resultado = "Ok";
                //        break;
                //    case 7:
                //        resultado = "Ok";
                //        break;
                //    default:
                //        if (resultado != "Ok") resultado = "Error desconocido";
                //        break;
                //}
            }
            else if (Presenter.marca == 5)
            {
                if (result.Length == 3)
                    resultado = "Ok";
                else
                    resultado = "Respuesta desconocida";
            }
            else
            {
                if (cmd.Substring(1, 1) == "@" && !cerrar)
                    resultado = "Ok";
                if (result.Length == 1)
                {
                    switch (result[0])
                    {
                        case Presenter.ACK:
                            if (Presenter.marca == 4)
                            {
                                System.Threading.Thread.Sleep(500);
                                var result2 = this.DoOperation("E" + cmd.Substring(1, 2), cerrar);
                                if (result2.Length == 0)
                                {
                                    resultado = "No hubo respuesta de la consola";
                                    return;
                                }
                                if (result2[0] == Presenter.ACK)
                                    resultado = "Ok";
                                else
                                    resultado = "Error en cancelación";
                            }
                            else
                                resultado = "Ok";
                            break;
                        case Presenter.NAK:
                            resultado = "Comando Incorrecto";
                            break;
                        default:
                            resultado = "Error desconocido";
                            break;
                    }
                }
                else if (result[1] == 64)
                {
                    System.Threading.Thread.Sleep(500);
                    var result2 = this.DoOperation("E" + cmd.Substring(4, 2), cerrar);
                    if (result2.Length == 0)
                    {
                        resultado = "No hubo respuesta de la consola";
                        return;
                    }
                    if (result2[0] == Presenter.ACK)
                        resultado = "Ok";
                    else
                        resultado = "Error en cancelación";
                }
                else
                    resultado = "Error desconocido";
            }
        }

        private void DoAction(List<string> cmd)
        {
            resultado = DoOperation(cmd);
        }

        private void Calibrar(string cmd, bool cerrar)
        {
            var result = this.DoOperation(cmd, cerrar);
            if (result.Length == 0)
            {
                resultado = "No hubo respuesta de la consola";
                return;
            }
            valoresCal.Add(Encoding.ASCII.GetString(result).Trim());
        }

        private byte[] DoOperation(string send, bool cerrar)
        {
            SerialConnectionConfig cfg = this.GetConfig();
            this.Presenter.CreateConnection(cfg);
            return this.Presenter.Send(send, cerrar, cfg);
        }

        private string DoOperation(List<string> send)
        {
            SerialConnectionConfig cfg = this.GetConfig();
            return this.Presenter.Send(send, cfg);
        }


        private SerialConnectionConfig GetConfig()
        {
            SerialConnectionConfig cfg = Presenter.GetSerialConfig();
            return cfg;
        }

        public string EnviarComandos(List<string> comandos, int marca)
        {
            try
            {
                this.comandos = comandos;
                resultado = "";
                if (ConfigurationManager.AppSettings["OpenGas"] == "Si")
                {
                    try
                    {
                        LiberarPSerialSocket(0);
                        System.Threading.Thread.Sleep(200);
                        if (marca == 1 || marca == 2 || ConfigurationManager.AppSettings["ModoDelphi"] == "Si")
                        {
                            this.DoAction(comandos);
                        }
                        else
                            for (int i = 0; i <= comandos.Count - 1; i++)
                            {
                                System.Threading.Thread.Sleep(100);
                                this.DoAction(comandos[i], i == comandos.Count - 1);
                            }
                    }
                    catch (Exception e)
                    {
                        Presenter.RegistraBitacora("Evento: ", e.Message + e.TargetSite + e.StackTrace);
                        resultado = e.Message;
                    }
                    finally
                    {
                        LiberarPSerialSocket(1);
                    }
                }
                else
                {
                    Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 1 });
                    this.Presenter.CreaListener((name) =>
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(1000);
                            if (marca == 1 || marca == 2 || ConfigurationManager.AppSettings["ModoDelphi"] == "Si")
                            {
                                this.DoAction(comandos);
                            }
                            else
                                for (int i = 0; i <= comandos.Count - 1; i++)
                                {
                                    System.Threading.Thread.Sleep(100);
                                    this.DoAction(comandos[i], i == comandos.Count - 1);
                                }
                        }
                        catch (Exception e)
                        {
                            Presenter.RegistraBitacora("Evento: ", e.Message + e.TargetSite + e.StackTrace);
                            resultado = e.Message;
                        }
                        finally
                        {
                            fin = true;
                            this.Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 1 });
                            this.Presenter.CerrarListener();
                        }
                    });
                    Presenter.marca = marca;
                    Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 0 });
                    int espera;
                    if (comandos.Count >= 3)
                        espera = 5 * comandos.Count;
                    else
                        espera = 10 * comandos.Count;
                    while (true)
                    {
                        System.Threading.Thread.Sleep(400);
                        if (fin || --espera == 0)
                            break;
                    }
                    
                }
            }
            catch (Exception e)
            {
                Presenter.RegistraBitacora("Pserial: ", e.Message + e.TargetSite + e.StackTrace);
                Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 1 });
                return e.Message;
            }
            return resultado;
        }

        public List<string> CalibrarBombas(List<string> comandos, int marca)
        {
            try
            {
                valoresCal = new List<string>();
                this.comandos = comandos;
                resultado = "";
                Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 1 });
                this.Presenter.CreaListener((name) =>
                {
                    try
                    {
                        System.Threading.Thread.Sleep(500);
                        DoAction(comandos);
                    }
                    catch (Exception e)
                    {
                        Presenter.RegistraBitacora("Evento: ", e.Message + e.TargetSite + e.StackTrace);
                        resultado = e.Message;
                    }
                    finally
                    {
                        fin = true;
                        this.Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 1 });
                        Presenter.CerrarListener();
                    }
                });
                Presenter.marca = marca;
                Presenter.ActualizarPocision(new FiltroDPVGCONF() { PosCliente = 0 });
                int espera = 5 * comandos.Count;
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (fin || --espera == 0)
                        break;
                }

                string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
                ruta = Path.GetDirectoryName(ruta);
                StreamReader objReader = new StreamReader(ruta + @"\rspcal.txt");
                string sLine = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        valoresCal.Add(sLine);
                }
                objReader.Close();

                return valoresCal;
            }
            catch (Exception e)
            {
                Presenter.RegistraBitacora("Pserial: ", e.Message + e.TargetSite + e.StackTrace);
                return null;
            }
        }

        public bool LiberarPSerialSocket(int tipo)
        {
            byte[] bytes = new byte[1024];

            try
            {
                string[] hostSocket = ConfigurationManager.AppSettings["HostPDispensarios"].Split(':');
                IPHostEntry host = Dns.GetHostEntry(hostSocket[0]);
                IPAddress ipAddress = host.AddressList[1];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Convert.ToInt32(hostSocket[1]));

                Socket sender = new Socket(ipAddress.AddressFamily,
                                SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEP);

                byte[] msg = Encoding.ASCII.GetBytes(Convert.ToString(tipo));

                int bytesSent = sender.Send(msg);

                int bytesRec = sender.Receive(bytes);               

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

                return Encoding.ASCII.GetString(bytes, 0, bytesRec) == "1";
            }
            catch (Exception e)
            {
                Presenter.RegistraBitacora("LiberarPSerialSocket: ", e.Message + e.TargetSite + e.StackTrace);
                return false;
            }
        }
    }
}
