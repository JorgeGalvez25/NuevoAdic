using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consola.Logic.Entities;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace Consola.Logic.Persistence
{
    public class DPVGCONFPersistecia
    {
        public const string CONSULTA = "SELECT " +
                                            "RAZONSOCIAL, " +
                                            "NOMBRECOMERCIAL, " +
                                            "ESTACIONSERVICIO, " +
                                            "LECTORSERIAL, " +
                                            "ULTIMOFOLIOCR, " +
                                            "ULTIMOFOLIOPP, " +
                                            "IMPRESORATICKETS, " +
                                            "POSCLIENTE, " +
                                            "LONGCLIENTE, " +
                                            "POSVEHICULO, " +
                                            "LONGVEHICULO, " +
                                            "CONFIGPUERTOTARJETA, " +
                                            "RUTAVOLUMETRICOS, " +
                                            "IMPO_BOMBAS, " +
                                            "IMPO_ESTACIONES, " +
                                            "IMPO_PRECIOS, " +
                                            "IMPO_TANQUES, " +
                                            "IMPO_COMBUSTIBLES, " +
                                            "IMPO_TARJETAS, " +
                                            "IMPO_CONFIG, " +
                                            "PERMITIR_CAMBIO_FECHAHORA, " +
                                            "INT_ACT_PRECIOS, " +
                                            "IMPRESORAVOLUMETRICOS, " +
                                            "COMANDO1, " +
                                            "COMANDO2, " +
                                            "LEVANTAR_CONSOLAS, " +
                                            "RUTA_RESPALDOS, " +
                                            "LICENCIA, " +
                                            "NUMEROSERIE, " +
                                            "ESTACION_IGAS, " +
                                            "IMPRESORAGRAFICA, " +
                                            "DISPENSARIOS, " +
                                            "CONFIG_TICKET, " +
                                            "MASCARA_FLOAT, " +
                                            "MASCARA_HORA, " +
                                            "LICENCIA2, " +
                                            "ESTEMPORAL, " +
                                            "FECHAVENCE, " +
                                            "DIRECCIONPEMEX, " +
                                            "USUARIOPEMEX, " +
                                            "CLAVEPEMEX, " +
                                            "COMANDO3, " +
                                            "COMANDO4, " +
                                            "LICENCIA3, " +
                                            "LICENCIA4, " +
                                            "MODOADI, " +
                                            "ULTIMOIMPRESO, " +
                                            "CONFIGCUPON, " +
                                            "USARCUPONCORTESIA, " +
                                            "LICENCIA5, " +
                                            "FECHAVENCE5, " +
                                            "ESTEMPORAL5, " +
                                            "BAN_NAME, " +
                                            "BAN_PASSWORD, " +
                                            "BAN_CLIENID, " +
                                            "MODOPAGOSBANCARIOS, " +
                                            "USATURNOSALTERNATIVOS, " +
                                            "TICKET_PROMO, " +
                                            "ALIASMASTER, " +
                                            "FECHAVENCE2, " +
                                            "ESTEMPORAL2 " +
                                       "FROM " +
                                            "DPVGCONF " +
                                       "WHERE " +
                                            "RAZONSOCIAL = @RAZONSOCIAL ";

        private void DbConn(Action<FbCommand> action)
        {
            using (FbConnection conn = new FbConnection(ConfigurationManager.ConnectionStrings["Consola"].ConnectionString))
            {
                conn.Open();
                try
                {
                    using (FbCommand comm = conn.CreateCommand())
                    {
                        action(comm);
                    }
                }
                finally
                {
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private DPVGCONF Read(FbDataReader reader)
        {
            DPVGCONF result = new DPVGCONF();
            {
                result.RazonSocial = (reader.IsDBNull(0) ? string.Empty : reader.GetString(0));
                result.NombreComercial = (reader.IsDBNull(1) ? string.Empty : reader.GetString(0));
                result.EstacionServicio = (reader.IsDBNull(2) ? string.Empty : reader.GetString(0));
                result.LectorSerial = (reader.IsDBNull(3) ? string.Empty : reader.GetString(0));
                result.UltimoFolioCR = (reader.IsDBNull(4) ? string.Empty : reader.GetString(0));
                result.UltimoFolioPP = (reader.IsDBNull(5) ? string.Empty : reader.GetString(0));
                result.ImpresoraTickets = (reader.IsDBNull(6) ? string.Empty : reader.GetString(0));
                result.PosCliente = (reader.IsDBNull(7) ? string.Empty : reader.GetString(0));
                result.LongCliente = (reader.IsDBNull(8) ? string.Empty : reader.GetString(0));
                result.PosVehiculo = (reader.IsDBNull(9) ? string.Empty : reader.GetString(0));
                result.LongVehiculo = (reader.IsDBNull(10) ? string.Empty : reader.GetString(0));
                result.ConfigPuertoTarjeta = (reader.IsDBNull(11) ? string.Empty : reader.GetString(0));
                result.RutaVolumetricos = (reader.IsDBNull(12) ? string.Empty : reader.GetString(0));
                result.Impo_Bombas = (reader.IsDBNull(13) ? string.Empty : reader.GetString(0));
                result.Impo_Estaciones = (reader.IsDBNull(14) ? string.Empty : reader.GetString(0));
                result.Impo_Precios = (reader.IsDBNull(15) ? string.Empty : reader.GetString(0));
                result.Impo_Tanques = (reader.IsDBNull(16) ? string.Empty : reader.GetString(0));
                result.Impo_Combustibles = (reader.IsDBNull(17) ? string.Empty : reader.GetString(0));
                result.Impo_Tarjetas = (reader.IsDBNull(18) ? string.Empty : reader.GetString(0));
                result.Impo_Config = (reader.IsDBNull(19) ? string.Empty : reader.GetString(0));
                result.Permitir_Cambio_FechaHora = (reader.IsDBNull(20) ? string.Empty : reader.GetString(0));
                result.Int_Act_Precios = (reader.IsDBNull(21) ? string.Empty : reader.GetString(0));
                result.ImpresoraVolumetricos = (reader.IsDBNull(22) ? string.Empty : reader.GetString(0));
                result.Comando1 = (reader.IsDBNull(23) ? string.Empty : reader.GetString(0));
                result.Comando2 = (reader.IsDBNull(24) ? string.Empty : reader.GetString(0));
                result.Levantar_Consolas = (reader.IsDBNull(25) ? string.Empty : reader.GetString(0));
                result.Ruta_Respaldos = (reader.IsDBNull(26) ? string.Empty : reader.GetString(0));
                result.Licencia = (reader.IsDBNull(27) ? string.Empty : reader.GetString(0));
                result.NumeroSerie = (reader.IsDBNull(28) ? string.Empty : reader.GetString(0));
                result.Estacion_IGas = (reader.IsDBNull(29) ? string.Empty : reader.GetString(0));
                result.ImpresoraGrafica = (reader.IsDBNull(30) ? string.Empty : reader.GetString(0));
                result.Dispensarios = (reader.IsDBNull(31) ? string.Empty : reader.GetString(0));
                result.Config_Ticket = (reader.IsDBNull(32) ? string.Empty : reader.GetString(0));
                result.Mascara_Float = (reader.IsDBNull(33) ? string.Empty : reader.GetString(0));
                result.Mascara_Hora = (reader.IsDBNull(34) ? string.Empty : reader.GetString(0));
                result.Licencia2 = (reader.IsDBNull(35) ? string.Empty : reader.GetString(0));
                result.EsTemporal = (reader.IsDBNull(36) ? string.Empty : reader.GetString(0));
                result.FechaVence = (reader.IsDBNull(37) ? string.Empty : reader.GetString(0));
                result.DireccionPEMEX = (reader.IsDBNull(38) ? string.Empty : reader.GetString(0));
                result.UsuarioPEMEX = (reader.IsDBNull(39) ? string.Empty : reader.GetString(0));
                result.ClavePEMEX = (reader.IsDBNull(40) ? string.Empty : reader.GetString(0));
                result.Comando3 = (reader.IsDBNull(41) ? string.Empty : reader.GetString(0));
                result.Comando4 = (reader.IsDBNull(42) ? string.Empty : reader.GetString(0));
                result.Licencia3 = (reader.IsDBNull(43) ? string.Empty : reader.GetString(0));
                result.Licencia4 = (reader.IsDBNull(44) ? string.Empty : reader.GetString(0));
                result.ModoAdi = (reader.IsDBNull(45) ? string.Empty : reader.GetString(0));
                result.UltimoImpreso = (reader.IsDBNull(46) ? string.Empty : reader.GetString(0));
                result.ConfigCupon = (reader.IsDBNull(47) ? string.Empty : reader.GetString(0));
                result.UsarCuponCortesia = (reader.IsDBNull(48) ? string.Empty : reader.GetString(0));
                result.Licencia5 = (reader.IsDBNull(49) ? string.Empty : reader.GetString(0));
                result.FechaVence5 = (reader.IsDBNull(50) ? string.Empty : reader.GetString(0));
                result.EsTemporal5 = (reader.IsDBNull(51) ? string.Empty : reader.GetString(0));
                result.Ban_Name = (reader.IsDBNull(52) ? string.Empty : reader.GetString(0));
                result.Ban_Password = (reader.IsDBNull(53) ? string.Empty : reader.GetString(0));
                result.Ban_ClienID = (reader.IsDBNull(54) ? string.Empty : reader.GetString(0));
                result.ModoPagosBancarios = (reader.IsDBNull(55) ? string.Empty : reader.GetString(0));
                result.UsaTurnosAlternativos = (reader.IsDBNull(56) ? string.Empty : reader.GetString(0));
                result.Ticket_Promo = (reader.IsDBNull(57) ? string.Empty : reader.GetString(0));
                result.AliasMaster = (reader.IsDBNull(58) ? string.Empty : reader.GetString(0));
                result.FechaVence2 = (reader.IsDBNull(59) ? string.Empty : reader.GetString(0));
                result.EsTemporal2 = (reader.IsDBNull(60) ? string.Empty : reader.GetString(0));

            }
            return result;
        }

        private FbParameter[] GetParameters(DPVGCONF e)
        {
            FbParameter[] paramtr = new FbParameter[]
                {
                    new FbParameter("@RAZONSOCIAL", e.RazonSocial),
                    new FbParameter("@NOMBRECOMERCIAL", e.NombreComercial),
                    new FbParameter("@ESTACIONSERVICIO", e.EstacionServicio),
                    new FbParameter("@LECTORSERIAL", e.LectorSerial),
                    new FbParameter("@ULTIMOFOLIOCR", e.UltimoFolioCR),
                    new FbParameter("@ULTIMOFOLIOPP", e.UltimoFolioPP),
                    new FbParameter("@IMPRESORATICKETS", e.ImpresoraTickets),
                    new FbParameter("@POSCLIENTE", e.PosCliente),
                    new FbParameter("@LONGCLIENTE", e.LongCliente),
                    new FbParameter("@POSVEHICULO", e.PosVehiculo),
                    new FbParameter("@LONGVEHICULO", e.LongVehiculo),
                    new FbParameter("@CONFIGPUERTOTARJETA", e.ConfigPuertoTarjeta),
                    new FbParameter("@RUTAVOLUMETRICOS", e.RutaVolumetricos),
                    new FbParameter("@IMPO_BOMBAS", e.Impo_Bombas),
                    new FbParameter("@IMPO_ESTACIONES", e.Impo_Estaciones),
                    new FbParameter("@IMPO_PRECIOS", e.Impo_Precios),
                    new FbParameter("@IMPO_TANQUES", e.Impo_Tanques),
                    new FbParameter("@IMPO_COMBUSTIBLES", e.Impo_Combustibles),
                    new FbParameter("@IMPO_TARJETAS", e.Impo_Tarjetas),
                    new FbParameter("@IMPO_CONFIG", e.Impo_Config),
                    new FbParameter("@PERMITIR_CAMBIO_FECHAHORA", e.Permitir_Cambio_FechaHora),
                    new FbParameter("@INT_ACT_PRECIOS", e.Int_Act_Precios),
                    new FbParameter("@IMPRESORAVOLUMETRICOS", e.ImpresoraVolumetricos),
                    new FbParameter("@COMANDO1", e.Comando1),
                    new FbParameter("@COMANDO2", e.Comando2),
                    new FbParameter("@LEVANTAR_CONSOLAS", e.Levantar_Consolas),
                    new FbParameter("@RUTA_RESPALDOS", e.Ruta_Respaldos),
                    new FbParameter("@LICENCIA", e.Licencia),
                    new FbParameter("@NUMEROSERIE", e.NumeroSerie),
                    new FbParameter("@ESTACION_IGAS", e.Estacion_IGas),
                    new FbParameter("@IMPRESORAGRAFICA", e.ImpresoraGrafica),
                    new FbParameter("@DISPENSARIOS", e.Dispensarios),
                    new FbParameter("@CONFIG_TICKET", e.Config_Ticket),
                    new FbParameter("@MASCARA_FLOAT", e.Mascara_Float),
                    new FbParameter("@MASCARA_HORA", e.Mascara_Hora),
                    new FbParameter("@LICENCIA2", e.Licencia2),
                    new FbParameter("@ESTEMPORAL", e.EsTemporal),
                    new FbParameter("@FECHAVENCE", e.FechaVence),
                    new FbParameter("@DIRECCIONPEMEX", e.DireccionPEMEX),
                    new FbParameter("@USUARIOPEMEX", e.UsuarioPEMEX),
                    new FbParameter("@CLAVEPEMEX", e.ClavePEMEX),
                    new FbParameter("@COMANDO3", e.Comando3),
                    new FbParameter("@COMANDO4", e.Comando4),
                    new FbParameter("@LICENCIA3", e.Licencia3),
                    new FbParameter("@LICENCIA4", e.Licencia4),
                    new FbParameter("@MODOADI", e.ModoAdi),
                    new FbParameter("@ULTIMOIMPRESO", e.UltimoImpreso),
                    new FbParameter("@CONFIGCUPON", e.ConfigCupon),
                    new FbParameter("@USARCUPONCORTESIA", e.UsarCuponCortesia),
                    new FbParameter("@LICENCIA5", e.Licencia5),
                    new FbParameter("@FECHAVENCE5", e.FechaVence5),
                    new FbParameter("@ESTEMPORAL5", e.EsTemporal5),
                    new FbParameter("@BAN_NAME", e.Ban_Name),
                    new FbParameter("@BAN_PASSWORD", e.Ban_Password),
                    new FbParameter("@BAN_CLIENID", e.Ban_ClienID),
                    new FbParameter("@MODOPAGOSBANCARIOS", e.ModoPagosBancarios),
                    new FbParameter("@USATURNOSALTERNATIVOS", e.UsaTurnosAlternativos),
                    new FbParameter("@TICKET_PROMO", e.Ticket_Promo),
                    new FbParameter("@ALIASMASTER", e.AliasMaster),
                    new FbParameter("@FECHAVENCE2", e.FechaVence2),
                    new FbParameter("@ESTEMPORAL2", e.EsTemporal2),
                };

            return paramtr;
        }
    }
}
