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
            string estatus = new ConfiguracionPersistencia().ConfiguracionObtener(1).Estado;
            string pMensajeRespuesta = string.Empty;
            variables = Utilerias.ObtenerListaVar();
            if (!ValidaLicencia("CVL5"))
                throw new System.ArgumentException("Licencia CVL5 Inválida.");
            if (ConfigurationManager.AppSettings["CambiaConsola"] == "Si")
            {
                if (estatus == "Estandar" || !std)
                {
                    new ProcesosFlujo().AplicarFlujo(AListaHistorial);
                    new ProcesosComando().AplicaComando(std, paro, out pMensajeRespuesta);
                }
                else if (estatus == "Mínimo" && std)
                {
                    new ProcesosFlujo().AplicarFlujo(AListaHistorial);
                    //new ProcesosComando().AplicaComando(std, paro, out pMensajeRespuesta);
                    pMensajeRespuesta = "Ok";
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

            return pMensajeRespuesta;
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
            try
            {
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
                        return rsp;
                }

                CambiaServiciosDisp(estatus, std);
                pMensajeRespuesta = "Ok";

                if (estatus != "Estandar" && std)
                {
                    System.Threading.Thread.Sleep(2000);
                    pMensajeRespuesta = SeguimientoRspCmnd(ComandoSocket("DISPENSERSX|FLUSTD|" + comando), false);
                }
                return pMensajeRespuesta;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error AplicarFlujoBennettSocket: " + ex.Message + " Comando: " + comando);
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
                if (new[] { "2", "5", "6", "7" }.Contains(tipoClb))
                {
                    int xpos = AListaHistorial[0].Posicion;
                    comando = AListaHistorial[0].Posicion + ":";
                    for (int i = 0; i < AListaHistorial.Count; i++)
                    {
                        if (xpos != AListaHistorial[i].Posicion)
                            comando = comando.Remove(comando.Length - 1) + ";" + AListaHistorial[i].Posicion + ":";
                        xpos = AListaHistorial[i].Posicion;
                        comando += AListaHistorial[i].Porcentaje.ToString() + ",";
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

                if (!new[] {"1", "2", "5", "6", "7" }.Contains(tipoClb) || std)
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
            Dictionary<string, string> variables = Utilerias.ObtenerListaVar();

            string ComandosPorServicio;
            if (!variables.TryGetValue("ComandosPorServicio", out ComandosPorServicio))
                ComandosPorServicio = "No";

            if (ConfigurationManager.AppSettings["ModoGateway"] == "Si")
                SeguimientoRspCmnd(ComandoSocket("DISPENSERSX|EJECCMND|PROT " + comandostr), false);
            else if (ComandosPorServicio == "Si")
            {
                string servConsola;
                if (!variables.TryGetValue("PuertoServicio", out servConsola))
                    servConsola = "http://127.0.0.1:9199/bin/";

                new ServicioDisp(servConsola).EjecutaComando("PROT " + comandostr);
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

        public string ComandoSocket(string cmd)
        {
            int BufferSize = 1024 * 1024;
            string[] hostSocket = ConfigurationManager.AppSettings["HostPDispensarios"].Split(':');

            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.ReceiveBufferSize = BufferSize;
                    socket.Connect(new IPEndPoint(IPAddress.Parse(hostSocket[0]), Convert.ToInt32(hostSocket[1])));

                    byte[] commandBytes = Encoding.ASCII.GetBytes(cmd);
                    socket.Send(commandBytes);

                    StringBuilder response = new StringBuilder();
                    byte[] buffer = new byte[BufferSize];
                    int bytesRead;

                    do
                    {
                        bytesRead = socket.Receive(buffer);
                        response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    }
                    while (bytesRead == BufferSize);

                    return response.ToString();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("SendCommand: " + e.Message + " Host: " + hostSocket[0] + ":" + hostSocket[1]);
            }
        }

        public bool CambiaServiciosDisp(string estatus, bool std)
        {
            if ((estatus == "Estandar" && !std) || (estatus != "Estandar" && std))
            {
                //Detener servicio
                ServiceController sc = new ServiceController(estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioX"] : ConfigurationManager.AppSettings["ServicioOpengas"]);

                try
                {
                    if (sc != null && sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                    }
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    sc.Close();
                }
                catch (Exception ex)
                {
                    GuardarMensaje(string.Format("ERROR_CambiaDisp({0}).txt", DateTime.Now.ToString("yyMMddHHmmss")), ex.Message + ex.TargetSite + ex.StackTrace);
                    return false;
                }

                try
                {
                    EditarXMLNotify(estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioOpengas"] : ConfigurationManager.AppSettings["ServicioX"]);
                }
                catch
                {
                }

                //Iniciar servicio
                sc = new ServiceController(estatus == "Estandar" ? ConfigurationManager.AppSettings["ServicioOpengas"] : ConfigurationManager.AppSettings["ServicioX"]);

                try
                {
                    if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    sc.Close();
                }
                catch (Exception ex)
                {
                    GuardarMensaje(string.Format("ERROR_CambiaDisp({0}).txt", DateTime.Now.ToString("yyMMddHHmmss")), ex.Message + ex.TargetSite + ex.StackTrace);
                    return false;
                }

                return true;
            }
            else
                return true;
        }

        public string SeguimientoRspCmnd(string rsp, bool single)
        {
            try
            {
                string folio = rsp.Split('|')[3];
                string resp, resp2;
                for (int i = 1; i <= 20; i++)
                {
                    System.Threading.Thread.Sleep(250);
                    resp = ComandoSocket("DISPENSERSX|RESPCMND|" + folio);
                    resp2 = resp.Split('|')[3];
                    resp = resp.Split('|')[2].ToUpper();
                    if (resp == "TRUE")
                        return "Ok";
                    else if (resp2.Length > 1)
                        return resp;
                }
                return "Sin respuesta";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error SeguimientoRspCmnd: " + ex.Message + " rsp: " + rsp);
            }
        }

        public void EditarXMLNotify(string valor)
        {
            try
            {
                string filePathExe = Path.Combine(ConfigurationManager.AppSettings["RutaXMLNotify"], "OG.Notify.exe");
                string filePathConf = Path.Combine(ConfigurationManager.AppSettings["RutaXMLNotify"], "OG.Notify.exe.config");

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
