using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using System.ServiceModel;
using ServiciosCliente;
using Consola.Connect;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Persistencia;
using Adicional.Entidades.Web;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Xml;

namespace ServiciosCliente
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServiciosCliente : IServiciosCliente
    {
        PSerial puerto;
        Dictionary<string, string> variables;
        public string AplicarFlujo(bool std, bool paro, MarcaDispensario marca, List<Adicional.Entidades.Historial> AListaHistorial)
        {
            try
            {
                string estatus = new ConfiguracionPersistencia().ConfiguracionObtener(1).Estado;
                string pMensajeRespuesta = string.Empty;
                variables = Utilerias.ObtenerListaVar();

                new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                {
                    Id_usuario = "DEBUG",
                    Suceso = string.Format("AplicarFlujo | Marca={0} |CambiaConsola={1} | ModoGateway={2} | estatus='{3}' | std={4}",
                        marca.ToString(),
                        ConfigurationManager.AppSettings["CambiaConsola"],
                        ConfigurationManager.AppSettings["ModoGateway"],
                        estatus,
                        std)
                });
                LogFlujo(string.Format("AplicarFlujo | Marca={0} | CambiaConsola={1} | ModoGateway={2} | estatus='{3}' | std={4} | paro={5}",
                    marca, ConfigurationManager.AppSettings["CambiaConsola"], ConfigurationManager.AppSettings["ModoGateway"], estatus, std, paro));

                if (!ValidaLicencia("CVL5"))
                    throw new System.ArgumentException("Licencia CVL5 Inválida.");

                if (ConfigurationManager.AppSettings["CambiaConsola"] == "Si")
                {
                    if (estatus.Equals("Estandar", StringComparison.OrdinalIgnoreCase) || !std)
                    {
                        new ProcesosFlujo().AplicarFlujo(AListaHistorial);
                        new ProcesosComando().AplicaComando(std, paro, out pMensajeRespuesta);
                    }
                    else if (estatus.Equals("Mínimo", StringComparison.OrdinalIgnoreCase) && std)
                    {
                        new ProcesosFlujo().AplicarFlujo(AListaHistorial);
                        pMensajeRespuesta = "Ok";
                    }
                    else
                    {
                        pMensajeRespuesta = string.Format("Estatus desconocido: '{0}'", estatus);
                    }
                }
                else if (ConfigurationManager.AppSettings["ModoGateway"] == "Si")
                {
                    switch (marca)
                    {
                        case MarcaDispensario.Ninguno:
                            break;
                        case MarcaDispensario.Wayne:
                            pMensajeRespuesta = AplicarFlujoWayneSocket(std, estatus, AListaHistorial);
                            break;
                        case MarcaDispensario.Bennett:
                            pMensajeRespuesta = AplicarFlujoBennettSocket(std, estatus, AListaHistorial);
                            break;
                        case MarcaDispensario.Team:
                            pMensajeRespuesta = AplicarFlujoTeamSocket(std, estatus, AListaHistorial);
                            break;
                        case MarcaDispensario.Gilbarco:
                            pMensajeRespuesta = AplicarFlujoGilbarcoSocket(std, estatus, AListaHistorial);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (ConfigurationManager.AppSettings["OpenGas"] == "Si" && (!ValidaLicencia("CVL7")))
                        throw new System.ArgumentException("Licencia CVL7 Inválida.");

                    switch (marca)
                    {
                        case MarcaDispensario.Ninguno:
                            break;
                        case MarcaDispensario.Wayne:
                            pMensajeRespuesta = AplicarFlujoWayne(std, AListaHistorial);
                            break;
                        case MarcaDispensario.Bennett:
                            pMensajeRespuesta = AplicarFlujoBennett(std, AListaHistorial);
                            break;
                        case MarcaDispensario.Team:
                            pMensajeRespuesta = AplicarFlujoTeam(std, AListaHistorial);
                            break;
                        case MarcaDispensario.Gilbarco:
                            pMensajeRespuesta = AplicarFlujoGilbarco(std, AListaHistorial);
                            break;
                        case MarcaDispensario.HongYang:
                            pMensajeRespuesta = AplicarFlujoHongYang(std, AListaHistorial);
                            break;
                        default:
                            break;
                    }
                }

                LogFlujo("AplicarFlujo regresa '" + pMensajeRespuesta + "'");
                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                LogFlujo("AplicarFlujo EXCEPCION: " + ex);
                return ex.Message;
            }
        }

        public List<Adicional.Entidades.Historial> ObtenerBombasEstacion()
        {
            ListaBomba pListaBombas = new BombaPersistencia().ObtenerLista();

            List<Historial> pResult = new List<Historial>();

            foreach (Bomba bomba in pListaBombas)
            {
                var pHistorial = new Historial();
                pHistorial.Posicion = bomba.poscarga;
                pHistorial.Manguera = bomba.manguera;
                pHistorial.Porcentaje = 0;
                pHistorial.Combustible = (short)bomba.combustible;
                pHistorial.Fecha = DateTime.Today;
                pHistorial.Hora = DateTime.Now.TimeOfDay;
                pHistorial.Conf = bomba.digitoajustevol;
                pHistorial.Calibracion = bomba.decimalesgilbarco;

                pResult.Add(pHistorial);
            }

            return pResult;
        }

        public List<ReporteAjuste> ObtenerReporteAjuste(DateTime fecha)
        {
            return new ReporteDeAjuste().ObtenerReporte(fecha);
        }

        public bool SetRegenerarArchivosVolumetricos(DateTime AFecha, int ACorte, out string AMensajeError)
        {
            AMensajeError = string.Empty;
            ServiciosArchivos pServiciosArchivos = new ServiciosArchivos();
            return pServiciosArchivos.SetRegenerarArchivosVolumetricos(AFecha, ACorte, out AMensajeError);
        }

        //public bool ProteccionEliminar()
        //{
        //    return new ProteccionPersistencia().ProteccionEliminar();
        //}

        //public int ProteccionInsertar(List<int> litros, out string mensaje)
        //{
        //    return new ProteccionPersistencia().ProteccionInsertar(litros, out mensaje);
        //}

        public bool Sincronizar(byte status, out string mensajeRespuesta)
        {
            mensajeRespuesta = string.Empty;
            bool comando = new ProcesosComando().AplicaComando("STAT " + status.ToString(), out mensajeRespuesta);

            return mensajeRespuesta.Equals("Ok", StringComparison.OrdinalIgnoreCase);
        }

        public List<ReporteAjuste> ObtenerReporte6a6(DateTime fecha)
        {
            return new ReporteDeAjuste().ObtenerReporte6a6(fecha);
        }

        public List<ReporteAjuste> ObtenerReporteDetallado(DateTime fecha)
        {
            return new ReporteDeAjuste().ObtenerReporteDetallado(fecha);
        }

        public ReporteAjuste ObtenerReporte2(DateTime fecha, int combustible, bool a24hrs)
        {
            return new ReporteDeAjuste().ObtenerReporte2(fecha, combustible, a24hrs);
        }

        #region Flujos Marcas

        public string AplicarFlujoGilbarco(bool std, List<Historial> AListaHistorial)
        {
            string pRespuesta;
            List<string> comandos = new List<string>();
            string tipoclb = variables.TryGetValue("TipoClb", out tipoclb) ? tipoclb : "0";
            string actProtec = ConfigurationManager.AppSettings["GilbarcoProtect"];
            decimal porGas = 0, porDie = 0;

            if (tipoclb == "3")
            {
                if (!ValidaLicencia("CVLG"))
                    throw new System.ArgumentException("Licencia Gilbarco Inválida.");
            }

            if (std) foreach (var h in AListaHistorial)
                {
                    if (h.Combustible == 3)
                        porDie = h.Porcentaje;
                    else
                        porGas = h.Porcentaje;

                }

            string cmd;

            if (tipoclb == "3")
            {
                foreach (var h in AListaHistorial)
                {
                    cmd = "P" + BuscaPosLibre(h.Combustible == 2 ? 1 : h.Combustible).ToString("00") + "0100" + "7957" +
                                (h.Combustible == 3 ? "4" + porDie.ToString("0") : "3" + porGas.ToString("0")) + "0";
                    if (cmd.Substring(0, 3) != "P00")
                        comandos.Add(cmd);
                }
            }
            else if (tipoclb == "4")
            {
                foreach (var h in AListaHistorial)
                {
                    cmd = "P" + BuscaPosLibre(h.Combustible == 2 ? 1 : h.Combustible).ToString("00") + "01000" + "957" +
                                (h.Combustible == 3 ? "4" + porDie.ToString("0") : "3" + porGas.ToString("0")) + "0";
                    if (cmd.Substring(0, 3) != "P00")
                        comandos.Add(cmd);
                }
            }
            else if (tipoclb == "5")
            {
                foreach (var h in AListaHistorial)
                {
                    cmd = "@020" + BuscaPosLibre(h.Combustible == 2 ? 1 : h.Combustible).ToString("00") + "010" + "957" +
                        (h.Combustible == 3 ? "4" + porDie.ToString("0") : "3" + porGas.ToString("0")) + (h.Combustible == 3 ? "10" : "11") + "0000";
                    if (cmd.Substring(0, 6) != "@02000")
                        comandos.Add(cmd);
                }
            }
            else
            {
                if (std)
                    comandos.Add("P" + BuscaPosLibre(1).ToString("00") + "01000" + "93715" + "0");
                else
                    comandos.Add("P" + BuscaPosLibre(1).ToString("00") + "01000" + "92476" + "0");
            }

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.Gilbarco);
            puerto = null;

            //new ProcesosComando().AplicaComando(std, out pRespuesta);

            return pRespuesta;
        }

        public string AplicarFlujoBennett(bool std, List<Historial> AListaHistorial)
        {
            string pRespuesta;
            List<string> comandos = new List<string>();

            string mangueras = "";
            string vAnterior = variables.TryGetValue("TipoClb", out vAnterior) ? vAnterior : "0";
            vAnterior = vAnterior != "2" ? "Si" : "No";

            if (vAnterior == "No")
            {
                if (!ValidaLicencia("CVLB"))
                    throw new System.ArgumentException("Licencia Bennett Inválida.");
            }

            for (int i = 0; i <= AListaHistorial.Count - 1; i++)
            {
                if (AListaHistorial[i].Estado == "Fuera" && (i == 0 || AListaHistorial[i - 1].Estado == "Fuera"))
                    continue;
                if (i != 0)
                    if (AListaHistorial[i].Posicion != AListaHistorial[i - 1].Posicion && AListaHistorial[i - 1].Estado != "Fuera")
                    {
                        if (vAnterior == "Si" && AListaHistorial[i].Conf != 300)
                            for (int j = mangueras.Length / 4; j < 4; j++)
                            {
                                mangueras += "+000";
                            }

                        comandos.Add((vAnterior == "Si" ? "W" : "Z") +
                                     AListaHistorial[i - 1].Posicion.ToString("00") +
                                     (vAnterior == "Si" || AListaHistorial[i].Conf == 300 ? "" : "+000+000") + mangueras);
                        mangueras = "";
                        if (AListaHistorial[i].Estado == "Fuera")
                            continue;
                    }

                mangueras += (std ? AListaHistorial[i].Porcentaje.ToString("+0.00").Replace(".", "") : "+000").Replace(",", "");

                if (i == AListaHistorial.Count - 1)
                {
                    if (vAnterior == "Si" && AListaHistorial[i].Conf != 300)
                        for (int j = mangueras.Length / 4; j < 4; j++)
                        {
                            mangueras += "+000";
                        }
                    comandos.Add((vAnterior == "Si" ? "W" : "Z") +
                                 AListaHistorial[i].Posicion.ToString("00") +
                                 (vAnterior == "Si" || AListaHistorial[i].Conf == 300 ? "" : "+000+000") + mangueras);
                }
            }

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.Bennett);
            puerto = null;

            return pRespuesta;
        }

        public string AplicarFlujoBennettSocket(bool std, string estatus, List<Historial> AListaHistorial)
        {
            string comando = string.Empty;
            Stopwatch swTotal = Stopwatch.StartNew();
            try
            {
                LogFlujo(string.Format("===== INICIO AplicarFlujoBennettSocket | std={0} | estatus='{1}' | HostPDispensarios={2} | ServicioX={3} | ServicioOpengas={4} | elementos={5}",
                    std, estatus,
                    ConfigurationManager.AppSettings["HostPDispensarios"],
                    ConfigurationManager.AppSettings["ServicioX"],
                    ConfigurationManager.AppSettings["ServicioOpengas"],
                    AListaHistorial == null ? "NULL" : AListaHistorial.Count.ToString()));
                if (AListaHistorial != null)
                    foreach (var h in AListaHistorial)
                        LogFlujo(string.Format("  Historial: Pos={0} Manguera={1} Comb={2} Porcentaje={3} Calibracion={4} Conf={5} Estado='{6}' Abajo='{7}'",
                            h.Posicion, h.Manguera, h.Combustible, h.Porcentaje, h.Calibracion, h.Conf, h.Estado, h.Abajo));

                string pMensajeRespuesta = string.Empty;
                int xpos = AListaHistorial[0].Posicion;
                comando = AListaHistorial[0].Posicion + ":";
                for (int i = 0; i < AListaHistorial.Count; i++)
                {
                    if (xpos != AListaHistorial[i].Posicion)
                        comando = comando.Remove(comando.Length - 1) + ";" + AListaHistorial[i].Posicion + ":";
                    xpos = AListaHistorial[i].Posicion;
                    decimal calibracionDecimal = (decimal)AListaHistorial[i].Calibracion / 100;
                    comando += AListaHistorial[i].Porcentaje.ToString() + (calibracionDecimal >= 0 ? "+" : "-") + Math.Abs(calibracionDecimal).ToString() + ",";
                }
                comando = comando.Remove(comando.Length - 1);
                LogFlujo("Comando armado: '" + comando + "'");

                if (estatus == "Estandar")
                {
                    LogFlujo("Camino: estatus Estandar -> se envia " + (std ? "FLUSTD" : "FLUMIN") + " al driver ANTES de cambiar servicios");
                    int folio;
                    string rsp = ComandoSocket("DISPENSERSX|" + (std ? "FLUSTD|" + comando : "FLUMIN"));

                    new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                    {
                        Id_usuario = "DEBUG",
                        Suceso = string.Format("ComandoSocket rsp='{0}' | partes={1}",
                            rsp,
                            rsp == null ? "NULL" : rsp.Split('|').Length.ToString())
                    });

                    string[] partes = rsp.Split('|');
                    LogFlujo(string.Format("Respuesta {0}: partes={1} | [2]='{2}' | [3]='{3}'", std ? "FLUSTD" : "FLUMIN",
                        partes.Length, partes.Length > 2 ? Visible(partes[2]) : "<no existe>", partes.Length > 3 ? Visible(partes[3]) : "<no existe>"));

                    if (Int32.TryParse(rsp.Split('|')[3], out folio))
                    {
                        LogFlujo("Folio de comando del driver: " + folio + " -> inicia seguimiento RESPCMND");
                        rsp = SeguimientoRspCmnd(rsp, false);
                        if (rsp != "Ok")
                        {
                            LogFlujo(string.Format("===== FIN AplicarFlujoBennettSocket ({0}ms) resultado='Servicio consola: {1}' (NO se cambian servicios)", swTotal.ElapsedMilliseconds, rsp));
                            return "Servicio consola: " + rsp;
                        }
                    }
                    else
                    {
                        LogFlujo(string.Format("===== FIN AplicarFlujoBennettSocket ({0}ms) la respuesta no trae folio numerico, se regresa respuesta cruda (NO se cambian servicios)", swTotal.ElapsedMilliseconds));
                        return rsp;
                    }
                }
                else
                    LogFlujo("Camino: estatus '" + estatus + "' -> solo se cambian servicios (el driver aplica el flujo estandar al iniciar)");

                // Igual que Gilbarco: al pasar de Minimo a Estandar el driver recien iniciado no ha sido
                // inicializado por la consola, asi que no se le envia FLUSTD ni se espera respuesta;
                // solo se valida que el cambio de servicio se haya realizado correctamente.
                bool cambioOk = CambiaServiciosDisp(estatus, std);
                LogFlujo("CambiaServiciosDisp regreso " + cambioOk);
                pMensajeRespuesta = cambioOk ? "Ok" : "Error al realizar cambio de servicio";

                LogFlujo(string.Format("===== FIN AplicarFlujoBennettSocket ({0}ms) resultado='{1}'", swTotal.ElapsedMilliseconds, pMensajeRespuesta));
                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                LogFlujo(string.Format("===== FIN AplicarFlujoBennettSocket CON EXCEPCION ({0}ms) comando='{1}': {2}", swTotal.ElapsedMilliseconds, comando, ex));
                new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                {
                    Id_usuario = "DEBUG Exception",
                    Suceso = ex.Message
                });
                return ex.Message;
            }
        }

        public string AplicarFlujoGilbarcoSocket(bool std, string estatus, List<Historial> AListaHistorial)
        {
            string tipoClb;
            string pMensajeRespuesta = string.Empty;
            if (!Utilerias.ObtenerListaVar().TryGetValue("TipoClb", out tipoClb))
                tipoClb = "0";
            string comando = string.Empty;

            try
            {
                if (new[] { "5", "6", "7", "8" }.Contains(tipoClb))
                {
                    int xpos = AListaHistorial[0].Posicion;
                    comando = AListaHistorial[0].Posicion + ":";
                    for (int i = 0; i < AListaHistorial.Count; i++)
                    {
                        if (xpos != AListaHistorial[i].Posicion)
                            comando = comando.Remove(comando.Length - 1) + ";" + AListaHistorial[i].Posicion + ":";
                        xpos = AListaHistorial[i].Posicion;
                        // TipoClb 8 aplica estos porcentajes, el driver los espera de un digito
                        comando += (tipoClb == "8" ? AListaHistorial[i].Porcentaje.ToString("0") : AListaHistorial[i].Porcentaje.ToString()) + ",";
                    }
                    comando = comando.Remove(comando.Length - 1);
                }
                else
                {
                    pMensajeRespuesta = string.Empty;
                    for (int i = 0; i < AListaHistorial.Count; i++)
                    {
                        comando += AListaHistorial[i].Porcentaje.ToString() + ";";
                    }
                    comando = comando.Remove(comando.Length - 1);
                }

                if (estatus == "Estandar")
                {
                    int folio;
                    string rsp = ComandoSocket("DISPENSERSX|" + (std ? "FLUSTD|" + comando : "FLUMIN"));
                    if (Int32.TryParse(rsp.Split('|')[3], out folio))
                    {
                        rsp = SeguimientoRspCmnd(rsp, false);
                        if (rsp != "Ok") return "Servicio consola: " + rsp;
                    }
                    else
                        return rsp.Split('|')[3];
                }

                if (!new[] {"1", "2", "5", "6", "7", "8" }.Contains(tipoClb) || std)
                    pMensajeRespuesta = CambiaServiciosDisp(estatus, std) ? "Ok" : "Error al realizar cambio de servicio";
                else
                    pMensajeRespuesta = "Ok";

                //if (pMensajeRespuesta == "Ok" && estatus != "Estandar" && std)
                //{
                //    System.Threading.Thread.Sleep(2000);
                //    int folio;
                //    string rsp = ComandoSocket("DISPENSERSX|FLUSTD|" + comando);
                //    pMensajeRespuesta = Int32.TryParse(rsp.Split('|')[3], out folio) ? "Ok" : rsp.Split('|')[3];
                //}

                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AplicarFlujoWayne(bool std, List<Historial> AListaHistorial)
        {
            string pRespuesta;
            List<string> comandos = new List<string>();

            foreach (var h in AListaHistorial)
            {
                comandos.Add("a" + h.Combustible.ToString("0") + "010024" + (std ? h.Porcentaje.ToString("0") : "0") +
                            (ConfigurationManager.AppSettings["WFusion"] == "Si" ? "0" : ""));
            }

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.Wayne);
            puerto = null;

            return pRespuesta;
        }

        public string AplicarFlujoWayneSocket(bool std, string estatus, List<Historial> AListaHistorial)
        {
            string pMensajeRespuesta = string.Empty;
            string comando = string.Empty;

            try
            {
                pMensajeRespuesta = string.Empty;
                for (int i = 0; i < AListaHistorial.Count; i++)
                {
                    comando += AListaHistorial[i].Porcentaje.ToString() + ";";
                }
                comando = comando.Remove(comando.Length - 1);

                if (estatus == "Estandar")
                {
                    int folio;
                    string rsp = ComandoSocket("DISPENSERSX|" + (std ? "FLUSTD|" + comando : "FLUMIN"));
                    if (Int32.TryParse(rsp.Split('|')[3], out folio))
                    {
                        rsp = SeguimientoRspCmnd(rsp, false);
                        if (rsp != "Ok") return "Servicio consola: " + rsp;
                    }
                    else
                        return rsp.Split('|')[3];
                }

                if (std)
                    pMensajeRespuesta = CambiaServiciosDisp(estatus, std) ? "Ok" : "Error al realizar cambio de servicio";
                else
                    pMensajeRespuesta = "Ok";

                if (pMensajeRespuesta == "Ok" && estatus != "Estandar" && std)
                {
                    System.Threading.Thread.Sleep(2000);
                    int folio;
                    string rsp = ComandoSocket("DISPENSERSX|FLUSTD|" + comando);
                    pMensajeRespuesta = Int32.TryParse(rsp.Split('|')[3], out folio) ? "Ok" : rsp.Split('|')[3];
                }

                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AplicarFlujoTeam(bool std, List<Historial> AListaHistorial)
        {
            string pRespuesta, checksum;
            List<string> comandos = new List<string>();
            string codTeam;
            if (!variables.TryGetValue("CodigoTeam", out codTeam))
                codTeam = "";

            if (codTeam.Length != 8)
            {
                pRespuesta = "El código TEAM no ha sido configurado o es incorrecto.";
                return pRespuesta;
            }

            int i = 0;
            foreach (var h in AListaHistorial)
            {
                if (h.Posicion % 2 == 1)
                {
                    checksum = (Convert.ToInt32(std ? h.Porcentaje : 0) + (codTeam.Length > 0 ? Convert.ToInt32(codTeam.Substring(0, 2)) + Convert.ToInt32(codTeam.Substring(2, 2)) +
                                Convert.ToInt32(codTeam.Substring(4, 2)) + Convert.ToInt32(codTeam.Substring(6, 2)) : 0)).ToString("00");
                    checksum = Convert.ToInt32(checksum) >= 100 ? checksum.Substring(checksum.Length - 2, 2) : checksum;

                    comandos.Add("E0" + (++i).ToString("00") + "00" + ((codTeam.Length / 2) + 2).ToString("00") +
                                (std ? h.Porcentaje.ToString("00") : "00") + codTeam + checksum);
                }
            }

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.Team);
            puerto = null;

            return pRespuesta;
        }

        public string AplicarFlujoTeamSocket(bool std, string estatus, List<Historial> AListaHistorial)
        {
            string pMensajeRespuesta = string.Empty;
            string comando = string.Empty;

            try
            {
                pMensajeRespuesta = string.Empty;
                for (int i = 0; i < AListaHistorial.Count; i++)
                {
                    comando += AListaHistorial[i].Posicion.ToString() + ":" + AListaHistorial[i].Porcentaje.ToString() + ";";
                }
                comando = comando.Remove(comando.Length - 1);

                if (estatus == "Estandar")
                {
                    int folio;
                    string rsp = ComandoSocket("DISPENSERSX|" + (std ? "FLUSTD|" + comando : "FLUMIN"));
                    if (Int32.TryParse(rsp.Split('|')[3], out folio))
                    {
                        rsp = SeguimientoRspCmnd(rsp, false);
                        if (rsp != "Ok") return "Servicio consola: " + rsp;
                    }
                    else
                        return rsp.Split('|')[3];
                }

                CambiaServiciosDisp(estatus, std);
                pMensajeRespuesta = "Ok";

                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AplicarFlujoHongYang(bool std, List<Historial> AListaHistorial)
        {
            string pRespuesta;
            List<string> comandos = new List<string>();

            foreach (var h in AListaHistorial)
            {
                comandos.Add(h.Posicion.ToString("X2") + "06" + h.Manguera.ToString("00") + "0F" +
                    (h.Porcentaje - (int)h.Porcentaje).ToString("0.00").Substring(2, 2) + ((int)h.Porcentaje).ToString("00"));
            }

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.HongYang);
            puerto = null;

            return pRespuesta;
        }

        public List<string> CalibrarBombas(List<string> comandos, int marca)
        {
            List<string> pRespuesta;
            puerto = new PSerial();
            pRespuesta = puerto.CalibrarBombas(comandos, marca);
            puerto = null;
            return pRespuesta;
        }

        public string SubirBajarFlujo(bool std)
        {
            string mensajeResp = string.Empty;
            bool comando = new ProcesosComando().AplicaComando(std, false, out mensajeResp);

            return mensajeResp;
        }

        public string AplicarProteccionGilbarco(bool std, string tipo)
        {
            string pRespuesta;
            string comando = "958";
            variables = Utilerias.ObtenerListaVar();
            List<string> comandos = new List<string>();

            if (std)
            {
                switch (tipo.Substring(0, 2).Trim())
                {
                    case "1":
                        tipo = "51";
                        break;
                    case "10":
                        tipo = "53";
                        break;
                    case "20":
                        tipo = "54";
                        break;
                    default:
                        tipo = "42";
                        break;
                }
            }
            else
                tipo = "50";

            comando += tipo;

            comandos.Add("P" + BuscaPosLibre(1).ToString("00") + "01000" + comando + "0");

            puerto = new PSerial();
            pRespuesta = puerto.EnviarComandos(comandos, (int)MarcaDispensario.Gilbarco);
            puerto = null;

            return pRespuesta;
        }

        #endregion


        #region Utilerias
        public bool IsAlive()
        {
            return true;
        }

        public int BuscaPosLibre(int tipo)
        {
            string servConsola, manejaServ, posiciones;
            variables.TryGetValue("ManejaServicios", out manejaServ);
            if (manejaServ != "Si")
            {
                posiciones = new DispensariosPersistencia().ObtenerDispensarios();
            }
            else
            {
                if (!variables.TryGetValue("PuertoServicio", out servConsola))
                    servConsola = "http://127.0.0.1:9199/bin/";
                ServicioDisp dispensarios = new ServicioDisp(servConsola);
                posiciones = dispensarios.GetEstadoPosiciones();
            }

            posiciones = posiciones.Substring(1, posiciones.Length - 1);
            DispensariosPersistencia dispensariosPer = new DispensariosPersistencia();
            List<int> posicionesTipo = dispensariosPer.ObtenerPosPorTipo(tipo);
            if (posicionesTipo.Count == 0 && tipo == 1)
                posicionesTipo = dispensariosPer.ObtenerPosPorTipo(2);
            if (posicionesTipo.Count == 0 && tipo == 1)
                posicionesTipo = dispensariosPer.ObtenerPosPorTipo(3);

            int i = 0;
            foreach (var p in posicionesTipo)
            {
                if (posiciones.Substring(p - 1, 1) == "1")
                    return p;
                i++;
                if (i == posicionesTipo.Count)
                    throw new System.ArgumentException("No hay posiciones libres de " + (tipo == 1 ? "gasolina." : "diesel."));
            }
            return 0;
        }

        public bool PosFinVta()
        {
            variables = Utilerias.ObtenerListaVar();
            string servConsola;

            if (!variables.TryGetValue("PuertoServicio", out servConsola))
                servConsola = "http://127.0.0.1:9199/bin/";
            ServicioDisp dispensarios = new ServicioDisp(servConsola);
            string posiciones = dispensarios.GetEstadoPosiciones();
            posiciones = posiciones.Substring(1, posiciones.Length - 1);
            int suma = 0;
            int i;
            for (i = 0; i < posiciones.Length; i++)
            {
                if (posiciones[i] == '2')
                    return true;
                else if (posiciones[i] == '#')
                    break;
                suma += Convert.ToInt32(posiciones.Substring(i, 1));
            }

            return false;
        }

        public bool CalibrarPosicion(int posicion)
        {
            List<Bomba> bombas = new List<Bomba>();
            bombas = new BombaPersistencia().ObtenerBombasPosicion(posicion);

            string valores = "";

            foreach (var bomba in bombas)
            {
                valores = valores + (bomba.decimalesgilbarco >= 0 ? bomba.decimalesgilbarco.ToString("+000") : bomba.decimalesgilbarco.ToString("000"));
            }
            List<string> comandos = new List<string>();
            comandos.Add("Z" + posicion.ToString("00") + ((bombas[0].digitoajustevol != 300 && bombas[0].digitoajustevol != 400) ? "+000+000" : "") + valores);
            if (bombas[0].digitoajustevol == 400)
            {
                comandos.Add("F" + posicion.ToString("00") + "9999");
                comandos.Add("F" + (posicion % 2 == 0 ? posicion - 1 : posicion + 1).ToString("00") + "9999");
            }

            string resp = new PSerial().EnviarComandos(comandos, (int)MarcaDispensario.Bennett);

            return resp == "ok";
        }

        [DllImport("LibsDelphi.dll", EntryPoint = "LicenciaValidaDLL")]
        private static extern int LicenciaValidaDLL(string RazonSocial, string Sistema, string Version, string TipoLicencia, string ClaveAutor, int Usuarios, bool LicenciaTemporal, string Fecha);

        public bool ValidaLicencia(string Sist)
        {
            try
            {
                string valor = string.Empty;
                string version = Licencia.Version;

                Licencia lic = new Licencia();
                lic.Razon_social = new ConsolaPersistencia().ObtenerRazonSocial();
                lic.Sistema = Sist;

                if (variables == null) { variables = Utilerias.ObtenerListaVar(); }

                //if (lic.Sistema == "CVL5")
                if (lic.Sistema == Licencia.ClabeAutor)
                {
                    //lic.Version = "3.1";
                    lic.TipoLicencia = "Abierta";
                    lic.ClaveAutor = variables.TryGetValue("Adicional41Lic", out valor) ? valor : string.Empty;
                    lic.Usuarios = 1;
                    lic.Estemporal = variables.TryGetValue("Adicional41FechaVence", out valor) ? "true" : "false";
                    lic.Fecha_vence = variables.TryGetValue("Adicional41FechaVence", out valor) ? valor : string.Empty;
                }
                else if (lic.Sistema == "CVLB")
                {
                    //lic.Version = "3.1";
                    lic.TipoLicencia = "Abierta";
                    lic.ClaveAutor = variables.TryGetValue("LicenciaBennett2", out valor) ? valor : string.Empty;
                    lic.Usuarios = 1;
                    lic.Estemporal = variables.TryGetValue("LicenciaBennett2FechaVence", out valor) ? "true" : "false";
                    lic.Fecha_vence = variables.TryGetValue("LicenciaBennett2FechaVence", out valor) ? valor : string.Empty;
                }
                else if (lic.Sistema == "CVLG")
                {
                    //lic.Version = "3.1";
                    lic.TipoLicencia = "Abierta";
                    lic.ClaveAutor = variables.TryGetValue("LicenciaGilbarco", out valor) ? valor : string.Empty;
                    lic.Usuarios = 1;
                    lic.Estemporal = variables.TryGetValue("LicenciaGilbarcoFechaVence", out valor) ? "true" : "false";
                    lic.Fecha_vence = variables.TryGetValue("LicenciaGilbarcoFechaVence", out valor) ? valor : string.Empty;
                }
                else if (lic.Sistema == "CVL7")
                {
                    lic.TipoLicencia = "Abierta";
                    lic.ClaveAutor = variables.TryGetValue("LicCVL7", out valor) ? valor : string.Empty;
                    lic.Usuarios = 1;
                    lic.Estemporal = variables.TryGetValue("LicCVL7FechaVence", out valor) ? "true" : "false";
                    lic.Fecha_vence = variables.TryGetValue("LicCVL7FechaVence", out valor) ? valor : string.Empty;
                }

                return LicenciaValidaDLL(lic.Razon_social,
                                         lic.Sistema,
                                         version,
                                         lic.TipoLicencia,
                                         lic.ClaveAutor,
                                         lic.Usuarios,
                                         lic.Estemporal == "true",
                                         lic.Fecha_vence) == 1;
            }
            catch
            {
                throw new System.ArgumentException("Error al validar licencia.");
            }
        }

        public string ObtenerEstatus()
        {
            return new ConfiguracionPersistencia().ConfiguracionObtener(1).Estado;
        }

        public void ComandoInsertar(Comandos comando)
        {
            new ComandosPersistencia().ComandosInsertar(comando);
        }

        public void AplicarProtecciones(string comandostr)
        {
            comandostr = NormalizarProtecciones(comandostr);
            Dictionary<string, string> variables = Utilerias.ObtenerListaVar();

            string ComandosPorServicio;
            if (!variables.TryGetValue("ComandosPorServicio", out ComandosPorServicio))
                ComandosPorServicio = "No";

            if (ConfigurationManager.AppSettings["ModoGateway"] == "Si")
            {
                string respuesta = SeguimientoRspCmnd(
                    ComandoSocket("DISPENSERSX|EJECCMND|PROT " + comandostr), false);
                if (!respuesta.Equals("Ok", StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("No fue posible aplicar las protecciones: " + respuesta);
            }
            else if (ComandosPorServicio == "Si")
            {
                string servConsola;
                if (!variables.TryGetValue("PuertoServicio", out servConsola))
                    servConsola = "http://127.0.0.1:9199/bin/";

                string respuesta = new ServicioDisp(servConsola).EjecutaComando("PROT " + comandostr);
                if (!respuesta.StartsWith("OK", StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("No fue posible aplicar las protecciones: " + respuesta);
            }
            else
            {
                Comandos comando = new Comandos();
                comando.Modulo = "DISP";
                comando.Comando = "PROT " + comandostr;
                ComandoInsertar(comando);
            }

            if (variables.ContainsKey("BennettProtec"))
                variables.Remove("BennettProtec");
            variables.Add("BennettProtec", comandostr);
            new EstacionConsPersistencia().ActualizaVariablesDispensario(variables);
        }

        private static string NormalizarProtecciones(string comandostr)
        {
            int[] litrosAceptados = new int[] { 1, 10, 20 };
            List<int> protecciones = new List<int>();

            foreach (string valor in (comandostr ?? string.Empty).Split(';'))
            {
                int litros;
                if (int.TryParse(valor.Trim(), out litros) &&
                    litrosAceptados.Contains(litros) &&
                    !protecciones.Contains(litros))
                {
                    protecciones.Add(litros);
                }
            }

            return string.Join(";", litrosAceptados
                .Where(litros => protecciones.Contains(litros))
                .Select(litros => litros.ToString())
                .ToArray());
        }

        public string ComandoSocket(string cmd)
        {
            int BufferSize = 1024 * 1024;
            string host = ConfigurationManager.AppSettings["HostPDispensarios"];
            LogFlujo("SOCKET -> [" + host + "] " + Visible(cmd));
            string[] hostSocket = host.Split(':');
            Stopwatch sw = Stopwatch.StartNew();
            string etapa = "Connect";

            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.ReceiveBufferSize = BufferSize;
                    socket.Connect(new IPEndPoint(IPAddress.Parse(hostSocket[0]), Convert.ToInt32(hostSocket[1])));
                    long msConexion = sw.ElapsedMilliseconds;

                    etapa = "Send";
                    byte[] commandBytes = Encoding.ASCII.GetBytes(cmd);
                    int bytesEnviados = socket.Send(commandBytes);

                    etapa = "Receive";
                    StringBuilder response = new StringBuilder();
                    byte[] buffer = new byte[BufferSize];
                    int bytesRead;
                    int lecturas = 0;

                    do
                    {
                        bytesRead = socket.Receive(buffer);
                        lecturas++;
                        response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    }
                    while (bytesRead == BufferSize);

                    string respuesta = response.ToString();
                    LogFlujo(string.Format("SOCKET <- [conexion={0}ms, total={1}ms, enviados={2} bytes, recibidos={3} bytes en {4} lectura(s)] {5}",
                        msConexion, sw.ElapsedMilliseconds, bytesEnviados, respuesta.Length, lecturas,
                        respuesta.Length == 0 ? "<VACIA: el servidor cerro la conexion sin responder>" : Visible(respuesta)));
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                LogFlujo(string.Format("SOCKET ERROR en {0} [{1}ms] {2}: {3}", etapa, sw.ElapsedMilliseconds, e.GetType().Name, e.Message));
                throw new ArgumentException("SendCommand: " + e.Message + " Host: " + hostSocket[0] + ":" + hostSocket[1]);
            }
        }

        public bool CambiaServiciosDisp(string estatus, bool std)
        {
            new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
            {
                Id_usuario = "DEBUG",
                Suceso = "Entró CambiaServiciosDisp"
            });
            string servicioDetener = estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioX"] : ConfigurationManager.AppSettings["ServicioOpengas"];
            string servicioIniciar = estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioOpengas"] : ConfigurationManager.AppSettings["ServicioX"];
            if ((estatus == "Estandar" && !std) || (estatus != "Estandar" && std))
            {
                LogFlujo(string.Format("CambiaServiciosDisp: estatus='{0}' std={1} -> detener '{2}' ({3}) e iniciar '{4}' ({5})",
                    estatus, std, servicioDetener, EstadoServicio(servicioDetener), servicioIniciar, EstadoServicio(servicioIniciar)));

                new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                {
                    Id_usuario = "DEBUG",
                    Suceso = "Detectó cambio servicio"
                });

                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    //Detener servicio
                    ServiceController sc = new ServiceController(servicioDetener);

                    if (sc != null && sc.Status == ServiceControllerStatus.Running)
                    {
                        LogFlujo("CambiaServiciosDisp: Stop('" + servicioDetener + "')");
                        sc.Stop();
                    }
                    else
                        LogFlujo("CambiaServiciosDisp: '" + servicioDetener + "' no estaba Running (" + sc.Status + "), no se envia Stop");
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeoutCambioServicio);
                    sc.Close();
                    LogFlujo(string.Format("CambiaServiciosDisp: '{0}' detenido en {1}ms", servicioDetener, sw.ElapsedMilliseconds));

                    new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                    {
                        Id_usuario = "DEBUG",
                        Suceso = "Detuvo servicio"
                    });
                }
                catch (Exception ex)
                {
                    LogFlujo(string.Format("CambiaServiciosDisp: ERROR al detener '{0}' [{1}ms]: {2}", servicioDetener, sw.ElapsedMilliseconds, ex));
                    GuardarMensaje(string.Format("ERROR_CambiaDisp({0}).txt", DateTime.Now.ToString("yyMMddHHmmss")), ex.Message + ex.TargetSite + ex.StackTrace);
                    return false;
                }

                try
                {
                    EditarXMLNotify(estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioOpengas"] : ConfigurationManager.AppSettings["ServicioX"]);

                    string valorCentinel = std ? ConfigurationManager.AppSettings["ServicioX"] : ConfigurationManager.AppSettings["ServicioOpengas"];
                    EditarJSONCentinel(valorCentinel);
                    LogFlujo("CambiaServiciosDisp: OG.Notify / Centinel actualizados");
                }
                catch (Exception ex)
                {
                    LogFlujo("CambiaServiciosDisp: error (ignorado) al actualizar OG.Notify / Centinel: " + ex.Message);
                }

                //Iniciar servicio


                try
                {
                    new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                    {
                        Id_usuario = "DEBUG",
                        Suceso = "Entró Iniciar servicio"
                    });

                    sw = Stopwatch.StartNew();
                    ServiceController sc = new ServiceController(servicioIniciar);

                    if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                    {
                        LogFlujo("CambiaServiciosDisp: Start('" + servicioIniciar + "')");
                        sc.Start();
                    }
                    else
                        LogFlujo("CambiaServiciosDisp: '" + servicioIniciar + "' no estaba Stopped (" + sc.Status + "), no se envia Start");
                    sc.WaitForStatus(ServiceControllerStatus.Running, TimeoutCambioServicio);
                    sc.Close();
                    LogFlujo(string.Format("CambiaServiciosDisp: '{0}' en Running en {1}ms", servicioIniciar, sw.ElapsedMilliseconds));

                    new BitacoraPersistencia().BitacoraInsertar(new Bitacora()
                    {
                        Id_usuario = "DEBUG",
                        Suceso = "Inició servicio"
                    });
                }
                catch (Exception ex)
                {
                    LogFlujo(string.Format("CambiaServiciosDisp: ERROR al iniciar '{0}' [{1}ms]: {2}", servicioIniciar, sw.ElapsedMilliseconds, ex));
                    GuardarMensaje(string.Format("ERROR_CambiaDisp({0}).txt", DateTime.Now.ToString("yyMMddHHmmss")), ex.Message + ex.TargetSite + ex.StackTrace);
                    return false;
                }

                return true;
            }
            else
            {
                LogFlujo(string.Format("CambiaServiciosDisp: estatus='{0}' std={1} -> no requiere cambio de servicio | '{2}'={3} | '{4}'={5}",
                    estatus, std, ConfigurationManager.AppSettings["ServicioX"], EstadoServicio(ConfigurationManager.AppSettings["ServicioX"]),
                    ConfigurationManager.AppSettings["ServicioOpengas"], EstadoServicio(ConfigurationManager.AppSettings["ServicioOpengas"])));
                return true;
            }
        }

        // Tiempo maximo para que un servicio de dispensarios llegue a Stopped / Running al cambiar de flujo.
        private static readonly TimeSpan TimeoutCambioServicio = TimeSpan.FromSeconds(60);

        private static readonly object lockLogFlujo = new object();

        /// <summary>
        /// Bitacora detallada del flujo por socket (lo enviado, lo recibido y los cambios de servicio).
        /// Solo se escribe si appSettings LogFlujoSocket="Si". Se guarda en RutaLogFlujo (appSettings)
        /// o en la carpeta Logs junto al ejecutable, un archivo por dia.
        /// </summary>
        private static void LogFlujo(string mensaje)
        {
            try
            {
                if (!string.Equals(ConfigurationManager.AppSettings["LogFlujoSocket"], "Si", StringComparison.OrdinalIgnoreCase))
                    return;

                string ruta = ConfigurationManager.AppSettings["RutaLogFlujo"];
                if (string.IsNullOrEmpty(ruta))
                    ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string archivo = Path.Combine(ruta, "FlujoSocket_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                string linea = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " [T" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + "] " + mensaje;

                lock (lockLogFlujo)
                {
                    File.AppendAllText(archivo, linea + Environment.NewLine);
                }
            }
            catch { }
        }

        /// <summary>
        /// Hace visibles los caracteres de control del protocolo del Bridge (SOH, STX, ETX, ETB...).
        /// </summary>
        private static string Visible(string texto)
        {
            if (texto == null)
                return "<null>";

            StringBuilder sb = new StringBuilder(texto.Length);
            foreach (char c in texto)
            {
                switch (c)
                {
                    case '\x01': sb.Append("<SOH>"); break;
                    case '\x02': sb.Append("<STX>"); break;
                    case '\x03': sb.Append("<ETX>"); break;
                    case '\x06': sb.Append("<ACK>"); break;
                    case '\x15': sb.Append("<NAK>"); break;
                    case '\x17': sb.Append("<ETB>"); break;
                    default:
                        if (c < ' ')
                            sb.Append("<" + ((int)c).ToString("X2") + ">");
                        else
                            sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        private static string EstadoServicio(string nombre)
        {
            try
            {
                using (ServiceController sc = new ServiceController(nombre))
                    return sc.Status.ToString();
            }
            catch (Exception ex)
            {
                return "ERROR(" + ex.Message + ")";
            }
        }

        public string SeguimientoRspCmnd(string rsp, bool single)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                string folio = rsp.Split('|')[3];
                LogFlujo("RESPCMND inicia seguimiento del folio '" + Visible(folio) + "' (20 intentos)");
                string resp, resp2;
                for (int i = 1; i <= 20; i++)
                {
                    System.Threading.Thread.Sleep(250);
                    resp = ComandoSocket("DISPENSERSX|RESPCMND|" + folio);
                    resp2 = resp.Split('|')[3];
                    resp = resp.Split('|')[2].ToUpper();
                    LogFlujo(string.Format("RESPCMND intento {0}/20 folio={1}: estado='{2}' detalle='{3}' [{4}ms acumulados]",
                        i, Visible(folio), Visible(resp), Visible(resp2), sw.ElapsedMilliseconds));
                    if (resp == "TRUE")
                    {
                        LogFlujo("RESPCMND folio " + Visible(folio) + " -> Ok en el intento " + i);
                        return "Ok";
                    }
                    else if (resp2.Length > 1)
                    {
                        LogFlujo("RESPCMND folio " + Visible(folio) + " -> error del driver: '" + Visible(resp2) + "' (se regresa '" + resp + "')");
                        return resp;
                    }
                }
                LogFlujo(string.Format("RESPCMND folio {0} -> Sin respuesta tras 20 intentos ({1}ms): el comando sigue pendiente en el driver", Visible(folio), sw.ElapsedMilliseconds));
                return "Sin respuesta";
            }
            catch (Exception ex)
            {
                LogFlujo(string.Format("RESPCMND EXCEPCION [{0}ms] rsp='{1}': {2}", sw.ElapsedMilliseconds, Visible(rsp), ex.Message));
                throw new ArgumentException("Error SeguimientoRspCmnd: " + ex.Message + " rsp: " + rsp);
            }
        }

        public void EditarXMLNotify(string valor)
        {
            try
            {
                string rutaNotify = ConfigurationManager.AppSettings["RutaXMLNotify"];

                // Validación de seguridad: Si no está configurado, salimos del método silenciosamente
                if (string.IsNullOrEmpty(rutaNotify))
                {
                    return;
                }

                string filePathExe = Path.Combine(rutaNotify, "OG.Notify.exe");
                string filePathConf = Path.Combine(rutaNotify, "OG.Notify.exe.config");

                //Detiene proceso
                Process[] processes = Process.GetProcessesByName("OG.Notify");
                if (processes.Length > 0)
                {
                    processes[0].Kill();
                    processes[0].WaitForExit();
                }

                //Edita archivo
                XmlDocument document = new XmlDocument();
                document.Load(filePathConf);

                XmlNodeList appSettingsNodes = document.SelectNodes("//configuration/appSettings/add");

                foreach (XmlNode appSettingsNode in appSettingsNodes)
                {
                    XmlAttribute NombreDispensarioAttribute = null;
                    if (appSettingsNode.Attributes["key"].Value == "NombreDispensario")
                        NombreDispensarioAttribute = appSettingsNode.Attributes["value"];
                    if (NombreDispensarioAttribute != null)
                    {
                        NombreDispensarioAttribute.Value = valor;
                        document.Save(filePathConf);
                        break;
                    }
                }

                //Inicia proceso
                Process p = new Process();
                p.StartInfo.FileName = filePathExe;
                p.Start();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al modificar archivo de configuración de OG.Notify: " + ex.Message);
            }
        }

        public void EditarJSONCentinel(string valor)
        {
            try
            {
                string directoryPath = ConfigurationManager.AppSettings["RutaJSONCentinel"];

                // Validación de seguridad: Si no está configurado, salimos del método silenciosamente
                if (string.IsNullOrEmpty(directoryPath))
                {
                    return;
                }

                string filePathConf = Path.Combine(directoryPath, "appsettings.json");

                if (File.Exists(filePathConf))
                {
                    string jsonContent = File.ReadAllText(filePathConf);

                    // Usamos Regex para buscar "NombreDispensario": "CUALQUIER_VALOR" y reemplazar el valor
                    string pattern = @"(""NombreDispensario""\s*:\s*"")[^""]*("")";
                    string replacement = "${1}" + valor + "${2}";

                    string newJsonContent = System.Text.RegularExpressions.Regex.Replace(jsonContent, pattern, replacement);

                    File.WriteAllText(filePathConf, newJsonContent);

                    // Reinicio de Centinel con los valores fijos solicitados
                    try
                    {
                        // Intentamos reiniciarlo como Servicio de Windows
                        ServiceController sc = new ServiceController("ogcvcentinela");
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                        sc.Close();
                    }
                    catch
                    {
                        // Si falla, lo reiniciamos como proceso utilizando la ruta fija proporcionada
                        Process[] processes = Process.GetProcessesByName("OpenGas.Centinela");
                        if (processes.Length == 0)
                            processes = Process.GetProcessesByName("ogcvcentinela");

                        if (processes.Length > 0)
                        {
                            processes[0].Kill();
                            processes[0].WaitForExit();
                        }

                        Process p = new Process();
                        p.StartInfo.FileName = @"C:\OpenGas\OG_Centinel\OpenGas.Centinela.exe";
                        p.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al modificar archivo de configuración JSON de Centinel: " + ex.Message);
            }
        }

        internal static void GuardarMensaje(string archivo, string mensaje)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(archivo, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                System.IO.StreamWriter m_streamWriter = new System.IO.StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                m_streamWriter.WriteLine(mensaje);
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch { }
        }


        #endregion


        #region Tanques

        public Tanques ObtenerComplemento(Tanques entidad)
        {
            return new TanquesPersistencia().ObtenerComplemento(entidad);
        }

        public ListaDpvgTanq ObtenerTanques()
        {
            return new TanquesPersistencia().ObtenerTanques();
        }

        public bool TanquesEliminar(FiltroTanques filtro, string usuario)
        {
            TanquesPersistencia tanque = new TanquesPersistencia();
            tanque.Usuario = usuario;
            return tanque.TanquesEliminar(filtro);
        }

        public bool TanquesModificar(Tanques entidad, string usuario)
        {
            TanquesPersistencia tanque = new TanquesPersistencia();
            tanque.Usuario = usuario;
            return tanque.TanquesModificar(entidad);
        }

        public Tanques TanquesObtener(FiltroTanques filtro)
        {
            return new TanquesPersistencia().TanquesObtener(filtro);
        }

        public ListaTanques TanquesObtenerTodos(FiltroTanques filtro)
        {
            return new TanquesPersistencia().TanquesObtenerTodos(filtro);
        }

        public bool TanquesRegistrar(Tanques entidad, string usuario)
        {
            TanquesPersistencia tanque = new TanquesPersistencia();
            tanque.Usuario = usuario;
            return tanque.TanquesRegistrar(entidad);
        }

        public bool RegistrarLectura(LecturaTanque entidad)
        {
            return new TanquesPersistencia().RegistrarLectura(entidad);
        }

        public string AplicarFlujoGilbarcoPorcentajes(string cmd)
        {
            string pMensajeRespuesta = string.Empty;
            if (ConfigurationManager.AppSettings["ModoGateway"] == "Si")
            {
                int folio;
                string rsp = ComandoSocket("DISPENSERSX|FLUACT|" + cmd);
                if (Int32.TryParse(rsp.Split('|')[3], out folio))
                    return SeguimientoRspCmnd(rsp, false);
                else
                    return rsp.Split('|')[3];
            }
            else
            {
                new ProcesosComando().AplicaComando("FLUACT", out pMensajeRespuesta);
                return pMensajeRespuesta;
            }
        }


        #endregion

        #region Tickets

        public ListaCombustible ObtenerCombustibles()
        {
            return new TicketsPersistencia().ObtenerCombustibles();
        }

        public bool TicketActualizar(Ticket entidad, string usuario)
        {
            TicketsPersistencia ticket = new TicketsPersistencia();
            ticket.Usuario = usuario;
            return ticket.TicketActualizar(entidad);
        }

        public int TicketConsecutivo()
        {
            return new TicketsPersistencia().TicketConsecutivo();
        }

        public Ticket TicketObtener(FiltroTicket filtro)
        {
            return new TicketsPersistencia().TicketObtener(filtro);
        }

        public Ticket TicketRegistrar(Ticket entidad, string usuario)
        {
            TicketsPersistencia ticket = new TicketsPersistencia();
            ticket.Usuario = usuario;
            return ticket.TicketRegistrar(entidad);
        }

        #endregion
    }
}
