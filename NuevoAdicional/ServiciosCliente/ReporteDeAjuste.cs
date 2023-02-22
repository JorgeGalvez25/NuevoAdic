using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace ServiciosCliente
{
    public class ReporteDeAjuste
    {
        private double porcMerma;

        public ReporteDeAjuste()
        {
            try
            {
                System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                porcMerma = Convert.ToDouble(appReader.GetValue("Porcentaje Merma", typeof(double)));
            }
            catch (Exception)
            {
                porcMerma = 0.0074;
            }
        }

        private bool HayCombustible(int ACombustible)
        {
            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            string pSentencia = "SELECT COUNT(*) FROM DPVGBOMB WHERE COMBUSTIBLE = @COMBUSTIBLE";

            FbCommand pComando = new FbCommand(pSentencia, conexion);
            pComando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ACombustible;

            conexion.Open();

            int pValor = (int)pComando.ExecuteScalar();

            conexion.Close();

            return pValor > 0;
        }

        private double getPrecioCombustible(FbConnection conexionAjusta, DateTime fecha, int combustible)
        {
            double result = 0;
            string sentencia = "SELECT FIRST 1 PRECIO FROM CLAVES WHERE FECHA = @FECHA AND COMBUSTIBLE = @COMBUSTIBLE";

            FbCommand comando = new FbCommand(sentencia, conexionAjusta);
            comando.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
            comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = combustible;

            result = Convert.ToDouble(comando.ExecuteScalar());

            return result;
        }

        private double DameInventarioInicialCombustible(DateTime AFecha, int ACorte, int ACombustible)
        {
            double pResult = 0;

            FbConnection conexion = new Conexiones().ConexionObtener("Ajusta");
            string pSentencia = "SELECT INVINICIAL_TANQUE  FROM HISTORIA WHERE FECHA = @FECHA AND CORTE = @CORTE AND COMBUSTIBLE = @COMBUSTIBLE";

            FbCommand pComando = new FbCommand(pSentencia, conexion);
            pComando.Parameters.Add("@FECHA", FbDbType.Date).Value = AFecha;
            pComando.Parameters.Add("@CORTE", FbDbType.Integer).Value = ACorte;
            pComando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ACombustible;

            conexion.Open();

            FbDataReader pReader = pComando.ExecuteReader();

            if (pReader.Read())
            {
                pResult = pReader.GetFloat(0);
            }

            conexion.Close();

            return pResult;
        }

        private int DameMaximoTurno(DateTime AFecha, int ACombustible)
        {
            int pResult = 0;

            FbConnection conexion = new Conexiones().ConexionObtener("Ajusta");
            string pSentencia = "SELECT MAX(CORTE) FROM HISTORIA WHERE FECHA = @FECHA AND COMBUSTIBLE = @COMBUSTIBLE";

            FbCommand pComando = new FbCommand(pSentencia, conexion);
            pComando.Parameters.Add("@FECHA", FbDbType.Date).Value = AFecha;
            pComando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ACombustible;

            conexion.Open();

            try
            {
                pResult = (int)pComando.ExecuteScalar();
            }
            catch
            {
                pResult = 6;
            }

            conexion.Close();

            return pResult;
        }

        private double DameInventarioFinalCombustible(DateTime AFecha, int ACorte, int ACombustible)
        {
            double pResult = 0;

            FbConnection conexion = new Conexiones().ConexionObtener("Ajusta");
            string pSentencia = "SELECT INVFINAL_TANQUE FROM HISTORIA WHERE FECHA = @FECHA AND CORTE = @CORTE AND COMBUSTIBLE = @COMBUSTIBLE";

            FbCommand pComando = new FbCommand(pSentencia, conexion);
            pComando.Parameters.Add("@FECHA", FbDbType.Date).Value = AFecha;
            pComando.Parameters.Add("@CORTE", FbDbType.Integer).Value = ACorte;
            pComando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ACombustible;

            conexion.Open();

            FbDataReader pReader = pComando.ExecuteReader();

            if (pReader.Read())
            {
                pResult = pReader.GetFloat(0);
            }

            conexion.Close();

            return pResult;
        }

        private DateTime getFechaHoraTurnoAdministrativo(DateTime fechaTurno, int turno, bool inicial)
        {
            DateTime result = fechaTurno;

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            string pSentencia = "SELECT FECHAHORAINICIAL, FECHAHORAFINAL FROM DPVGTURN WHERE FECHA = @FECHA AND TURNO = @TURNO";

            FbCommand pComando = new FbCommand(pSentencia, conexion);
            pComando.Parameters.Add("@FECHA", FbDbType.Date).Value = fechaTurno.Date;
            pComando.Parameters.Add("@TURNO", FbDbType.Integer).Value = turno;

            conexion.Open();

            FbDataReader reader = pComando.ExecuteReader();

            if (reader.Read())
            {
                result = reader.GetDateTime(inicial ? 0 : 1);
            }

            conexion.Close();

            return result;
        }

        public List<ReporteAjuste> ObtenerReporte(DateTime fecha)
        {
            List<ReporteAjuste> result = new List<ReporteAjuste>();
            //FbConnection conexionAjusta = null;
            //FbConnection conexionConsola = null;

            try
            {
                string sentencia = @"SELECT COMBUSTIBLE, SUM(SALIDAS) AS SALIDAS, SUM(AJUSTE) AS AJUSTE,SUM(DIFERENCIA) AS DIFERENCIA,
                                     SUM(ENTRADAS_TANQUE) AS ENTRADAS, SUM(JARREOS) AS JARREOS, SUM(INVFISICO) AS INVFISICO,
                                     COALESCE(SUM(PENDIENTE),0) AS PENDIENTE FROM HISTORIA WHERE FECHA = @FECHA GROUP BY COMBUSTIBLE";
                string sentenciaNomComb = @"select c.nombre from DPVGTCMB c where c.clave = @COMBUSTIBLE";
                string sentenciaInvIni = @"SELECT INVINICIAL_TANQUE FROM HISTORIA WHERE FECHA = @FECHA AND CORTE = @CORTE AND COMBUSTIBLE = @COMBUSTIBLE";
                string sentenciaInvFin = @"SELECT FIRST 1 INVFINAL_TANQUE FROM HISTORIA WHERE FECHA = @FECHA AND COMBUSTIBLE = @COMBUSTIBLE ORDER BY CORTE DESC";
                string sentenciaEntFac = @"Select coalesce(Sum(d.VolPemex), 0) as EntFct from DPVGDOCU d where
                                           exists(select t.folio from DPVGETAN t where t.fecha = @FECHA and
                                           d.folioentrada = t.folio and t.combustible = @COMBUSTIBLE)";
                //FbCommand comandoComb;
                //FbCommand comandoNombreComb;
                //FbCommand comandoInvIni;
                //FbCommand comandoInvFin;
                //FbCommand comandoEntFac;
                //FbDataReader readerComb;

                using (FbConnection conexionAjusta = new Conexiones().ConexionObtener("Ajusta"))
                {
                    try
                    {
                        conexionAjusta.Open();

                        using (FbCommand comandoComb = new FbCommand(sentencia, conexionAjusta))
                        {
                            comandoComb.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;

                            using (FbDataReader readerComb = comandoComb.ExecuteReader())
                            {
                                using (FbConnection conexionConsola = new Conexiones().ConexionObtener("GasConsola"))
                                {
                                    try
                                    {
                                        conexionConsola.Open();

                                        using (FbCommand comandoNombreComb = new FbCommand(sentenciaNomComb, conexionConsola))
                                        {
                                            comandoNombreComb.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);

                                            using (FbCommand comandoInvIni = new FbCommand(sentenciaInvIni, conexionAjusta))
                                            {
                                                comandoInvIni.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                comandoInvIni.Parameters.Add("@CORTE", FbDbType.Integer).Value = 1;
                                                comandoInvIni.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);

                                                using (FbCommand comandoInvFin = new FbCommand(sentenciaInvFin, conexionAjusta))
                                                {
                                                    comandoInvFin.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                    comandoInvFin.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);

                                                    using (FbCommand comandoEntFac = new FbCommand(sentenciaEntFac, conexionConsola))
                                                    {
                                                        comandoEntFac.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                        comandoEntFac.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);

                                                        ReporteAjuste renglon = null;
                                                        Dictionary<int, double> precios = new Dictionary<int, double>();
                                                        try
                                                        {
                                                            while (readerComb.Read())
                                                            {
                                                                renglon = new ReporteAjuste();

                                                                if (Convert.ToDouble(readerComb["SALIDAS"]) + Convert.ToDouble(readerComb["INVFISICO"]) != 0)
                                                                {
                                                                    renglon.Combustible = Convert.ToInt32(readerComb["COMBUSTIBLE"]);
                                                                    renglon.Jarreos = Convert.ToDouble(readerComb["JARREOS"]);
                                                                    renglon.SalidaDispensarios = Convert.ToDouble(readerComb["SALIDAS"]);
                                                                    renglon.Ajuste = Convert.ToDouble(readerComb["AJUSTE"]);
                                                                    renglon.Diferencia = Convert.ToDouble(readerComb["DIFERENCIA"]);
                                                                    renglon.EntradasFisicas = Convert.ToDouble(readerComb["INVFISICO"]);
                                                                    renglon.Pendiente = Convert.ToDouble(readerComb["PENDIENTE"]);

                                                                    renglon.PorcMerma = porcMerma;
                                                                    renglon.Fecha = fecha;

                                                                    comandoNombreComb.Parameters[0].Value = renglon.Combustible;
                                                                    renglon.NombreCombustible = comandoNombreComb.ExecuteScalar().ToString();

                                                                    if (!precios.ContainsKey(renglon.Combustible))
                                                                    {
                                                                        precios.Add(renglon.Combustible, getPrecioCombustible(conexionAjusta, fecha, renglon.Combustible));//Convert.ToDouble(comandoPrecio.ExecuteScalar()));
                                                                    }

                                                                    renglon.Precio = precios[renglon.Combustible];

                                                                    comandoInvIni.Parameters[2].Value = renglon.Combustible;
                                                                    renglon.InvInicial = Convert.ToDouble(comandoInvIni.ExecuteScalar());

                                                                    comandoInvFin.Parameters[1].Value = renglon.Combustible;
                                                                    renglon.InvFinal = Convert.ToDouble(comandoInvFin.ExecuteScalar());

                                                                    comandoEntFac.Parameters[1].Value = renglon.Combustible;
                                                                    renglon.Entradas = Convert.ToDouble(comandoEntFac.ExecuteScalar());
                                                                    renglon.EntradasMermas = renglon.Entradas * (1 - porcMerma);

                                                                    //// Obtener El Porcentaje de Merma
                                                                    //sentencia = @"SELECT coalesce(porcmermactble, 0) as merma " +
                                                                    //               "FROM HISTORIA " +
                                                                    //              "WHERE FECHA=@FECHA " +
                                                                    //               "and COMBUSTIBLE = @COMB " +
                                                                    //               "and corte = 1";
                                                                    //FbCommand comandoMerma = new FbCommand(sentencia, conexionAjusta);

                                                                    //comandoMerma.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha.Date;
                                                                    //comandoMerma.Parameters.Add("@COMB", FbDbType.Integer).Value = renglon.Combustible;

                                                                    result.Add(renglon);
                                                                }
                                                            }
                                                        }
                                                        finally
                                                        {
                                                            precios.Clear();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        if (!readerComb.IsClosed) readerComb.Close();
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (conexionAjusta.State == System.Data.ConnectionState.Open)
                        {
                            conexionAjusta.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public List<ReporteAjuste> ObtenerReporteDetallado(DateTime fecha)
        {
            List<ReporteAjuste> result = new List<ReporteAjuste>();
            //FbConnection conexionAjusta = null;
            //FbConnection conexionConsola = null;
            Dictionary<int, double> precios = new Dictionary<int, double>();

            try
            {
                string sentencia = @"SELECT * FROM HISTORIA WHERE FECHA = @FECHA ORDER BY CORTE, COMBUSTIBLE";
                string sentenciaNomComb = @"select c.nombre from DPVGTCMB c where c.clave = @COMBUSTIBLE";
                string sentenciaPrecio = @"SELECT FIRST 1 PRECIO FROM CLAVES WHERE FECHA = @FECHA AND COMBUSTIBLE = @COMBUSTIBLE";
                //FbCommand comandoComb;
                //FbCommand comandoNombreComb;
                //FbCommand comandoPrecio;
                //FbDataReader readerComb;

                using (FbConnection conexionAjusta = new Conexiones().ConexionObtener("Ajusta"))
                {
                    try
                    {
                        conexionAjusta.Open();

                        using (FbCommand comandoComb = new FbCommand(sentencia, conexionAjusta))
                        {
                            comandoComb.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                            using (FbDataReader readerComb = comandoComb.ExecuteReader())
                            {
                                try
                                {
                                    using (FbConnection conexionConsola = new Conexiones().ConexionObtener("GasConsola"))
                                    {
                                        try
                                        {
                                            conexionConsola.Open();
                                            using (FbCommand comandoNombreComb = new FbCommand(sentenciaNomComb, conexionConsola))
                                            {
                                                comandoNombreComb.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);
                                                using (FbCommand comandoPrecio = new FbCommand(sentenciaPrecio, conexionAjusta))
                                                {
                                                    comandoPrecio.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                    comandoPrecio.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer);

                                                    ReporteAjuste renglon = null;
                                                    while (readerComb.Read())
                                                    {
                                                        renglon = new ReporteAjuste();

                                                        renglon.Combustible = Convert.ToInt32(readerComb["COMBUSTIBLE"]);
                                                        renglon.SalidaDispensarios = Convert.ToDouble(readerComb["SALIDAS"]);
                                                        renglon.Ajuste = Convert.ToDouble(readerComb["AJUSTE"]);
                                                        renglon.Diferencia = Convert.ToDouble(readerComb["DIFERENCIA"]);
                                                        renglon.Corte = Convert.ToInt32(readerComb["CORTE"]);
                                                        renglon.Fecha = fecha;
                                                        renglon.PorcMerma = porcMerma;

                                                        comandoNombreComb.Parameters[0].Value = renglon.Combustible;
                                                        renglon.NombreCombustible = comandoNombreComb.ExecuteScalar().ToString();

                                                        if (!precios.ContainsKey(renglon.Combustible))
                                                        {
                                                            comandoPrecio.Parameters[1].Value = renglon.Combustible;
                                                            precios.Add(renglon.Combustible, Convert.ToDouble(comandoPrecio.ExecuteScalar()));
                                                        }

                                                        renglon.Precio = precios[renglon.Combustible];

                                                        result.Add(renglon);
                                                    }
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            if (conexionConsola.State == System.Data.ConnectionState.Open)
                                            {
                                                conexionConsola.Close();
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    if (!readerComb.IsClosed) readerComb.Close();
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (conexionAjusta.State == System.Data.ConnectionState.Open)
                        {
                            conexionAjusta.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {


            }


            return result;
        }

        public List<ReporteAjuste> ObtenerReporte6a6(DateTime fecha)
        {
            List<ReporteAjuste> result = new List<ReporteAjuste>();
            //FbConnection conexionConsola = null;
            //FbConnection conexionAjusta = null;

            try
            {
                // obtener los tipos de combustibles
                string sentencia = @"Select t.Tanque as Tanque, " +
                                           "t.combustible as comb, " +
                                           "(select c.nombre " +
                                              "from DPVGTCMB c " +
                                             "where c.clave = t.combustible) as Combustible " +
                                      "from DPVGTANQ t " +
                                     "where t.activo = 'Si' " +
                                     "Order by t.Tanque";
                //FbCommand comandoTanques;
                //FbDataReader readerTanques;

                using (FbConnection conexionConsola = new Conexiones().ConexionObtener("GasConsola"))
                {
                    conexionConsola.Open();

                    try
                    {
                        using (FbConnection conexionAjusta = new Conexiones().ConexionObtener("Ajusta"))
                        {
                            try
                            {
                                conexionAjusta.Open();

                                using (FbCommand comandoTanques = new FbCommand(sentencia, conexionConsola))
                                {
                                    using (FbDataReader readerTanques = comandoTanques.ExecuteReader())
                                    {
                                        try
                                        {
                                            int tanque = 0;
                                            ReporteAjuste tmp = null;
                                            while (readerTanques.Read())
                                            {
                                                tanque = Convert.ToInt32(readerTanques["Tanque"]);
                                                tmp = new ReporteAjuste()
                                                        {
                                                            Combustible = Convert.ToInt32(readerTanques["comb"]),
                                                            NombreCombustible = readerTanques["Combustible"].ToString(),
                                                            Fecha = fecha,
                                                        };

                                                // Obtener el volumen inicial del tanque
                                                sentencia = @"Select coalesce(Volumen, 0) as InvInicial " +
                                                               "from DPVGTURNT " +
                                                              "where Fecha = @FECHA " +
                                                                "and Tanque = @TANQUE " +
                                                                "and Turno = 3";
                                                using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                {
                                                    comandoVolumenes.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha.AddDays(-1);
                                                    comandoVolumenes.Parameters.Add("@TANQUE", FbDbType.Integer).Value = tanque;

                                                    tmp.InvInicial = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                }

                                                // Obtener el volumen final del tanque
                                                sentencia = @"Select coalesce(Volumen, 0) as InvFinal " +
                                                               "from DPVGTURNT " +
                                                              "where Fecha = @FECHA " +
                                                                "and Tanque = @TANQUE " +
                                                                "and Turno = 3";
                                                using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                {
                                                    comandoVolumenes.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                    comandoVolumenes.Parameters.Add("@TANQUE", FbDbType.Integer).Value = tanque;

                                                    tmp.InvFinal = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                }

                                                // Obtener las entradas facturadas
                                                sentencia = @"Select coalesce(Sum(d.VolPemex), 0) as EntFct " +
                                                               "from DPVGDOCU d " +
                                                               "where exists(select t.folio " +
                                                                              "from DPVGETAN t " +
                                                                             "where fecha = @FECHA " +
                                                                 "and d.folioentrada = t.folio " +
                                                                 "and t.tanque = @TANQUE)";
                                                using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                {
                                                    comandoVolumenes.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                    comandoVolumenes.Parameters.Add("@TANQUE", FbDbType.Integer).Value = tanque;

                                                    tmp.Entradas = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                    tmp.EntradasMermas = tmp.Entradas * (1 - porcMerma);
                                                    tmp.PorcMerma = porcMerma;
                                                }

                                                // Obtener las entradas físicas
                                                sentencia = @"select coalesce(Sum(t.volumenrecepcion),0) as Volumen " +
                                                               "from DPVGETAN t " +
                                                              "where fecha = @FECHA " +
                                                                "and Tanque = @TANQUE";
                                                using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                {
                                                    comandoVolumenes.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha;
                                                    comandoVolumenes.Parameters.Add("@TANQUE", FbDbType.Integer).Value = tanque;

                                                    tmp.EntradasFisicas = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                }

                                                // Agregar la linea al listado
                                                int indice = result.FindIndex(p => { return p.Combustible == tmp.Combustible; });
                                                if (indice >= 0)
                                                {
                                                    result[indice].InvInicial += tmp.InvInicial;
                                                    result[indice].InvFinal += tmp.InvInicial;
                                                    result[indice].Entradas += tmp.Entradas;
                                                    result[indice].EntradasFisicas += tmp.EntradasFisicas;
                                                    result[indice].EntradasMermas += tmp.EntradasMermas;
                                                }
                                                else
                                                {
                                                    // Obtener Salidas de Dispensario
                                                    // Hora inicial y Final del turno
                                                    DateTime fechaIni = fecha;
                                                    DateTime fechaFin = fecha;
                                                    try
                                                    {
                                                        fechaIni = getFechaHoraTurnoAdministrativo(fecha, 1, true);
                                                    }
                                                    catch (Exception) { fecha.Date.Add(new TimeSpan(6, 0, 0)); }
                                                    try
                                                    {
                                                        fechaFin = getFechaHoraTurnoAdministrativo(fecha, 3, true);
                                                    }
                                                    catch (Exception) { fechaFin = fecha.Date.Add(new TimeSpan(1, 5, 59, 59)); }


                                                    // Movimientos no ajustados
                                                    sentencia = @"Select coalesce(sum(Volumen),0) as volumen " +
                                                                   "from dpvgmovi " +
                                                                  "where Tag <> 1 " +
                                                                    "and (hora >= @FECHAINI and hora <= @FECHAFIN) " +
                                                                    "and combustible = @COMB " +
                                                                    "and jarreo = 'No'";
                                                    using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                    {
                                                        comandoVolumenes.Parameters.Add("@FECHAINI", FbDbType.Date).Value = fechaIni;
                                                        comandoVolumenes.Parameters.Add("@FECHAFIN", FbDbType.Date).Value = fechaFin;
                                                        comandoVolumenes.Parameters.Add("@COMB", FbDbType.Integer).Value = tmp.Combustible;

                                                        tmp.SalidaDispensarios = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                    }

                                                    // Jarreos
                                                    sentencia = @"Select coalesce(sum(Volumen),0) as volumen " +
                                                                   "from dpvgmovi " +
                                                                  "where (hora >= @FECHAINI and hora <= @FECHAFIN) " +
                                                                    "and combustible = @COMB " +
                                                                    "and jarreo='Si'";
                                                    using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionConsola))
                                                    {
                                                        comandoVolumenes.Parameters.Add("@FECHAINI", FbDbType.Date).Value = fechaIni;
                                                        comandoVolumenes.Parameters.Add("@FECHAFIN", FbDbType.Date).Value = fechaFin;
                                                        comandoVolumenes.Parameters.Add("@COMB", FbDbType.Integer).Value = tmp.Combustible;

                                                        tmp.Jarreos = Convert.ToDouble(comandoVolumenes.ExecuteScalar());
                                                    }

                                                    // Movimientos Ajustados
                                                    sentencia = @"select coalesce(Sum(Volumen1), 0) as Volumen, " +
                                                                        "coalesce(Sum(Volumen2), 0) as Volumen2 " +
                                                                   "from Claves " +
                                                                  "where FechaAdmin = @FECHA " +
                                                                    "and combustible = @COMB";
                                                    using (FbCommand comandoVolumenes = new FbCommand(sentencia, conexionAjusta))
                                                    {
                                                        comandoVolumenes.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha.Date;
                                                        comandoVolumenes.Parameters.Add("@COMB", FbDbType.Integer).Value = tmp.Combustible;

                                                        using (FbDataReader readerVolumenes = comandoVolumenes.ExecuteReader())
                                                        {
                                                            try
                                                            {
                                                                if (readerVolumenes.Read())
                                                                {
                                                                    tmp.SalidaDispensarios += Convert.ToDouble(readerVolumenes["Volumen"]);
                                                                    tmp.Ajuste = Convert.ToDouble(readerVolumenes["Volumen"]) - Convert.ToDouble(readerVolumenes["Volumen2"]);
                                                                }
                                                            }
                                                            finally
                                                            {

                                                                if (!readerVolumenes.IsClosed) readerVolumenes.Close();
                                                            }
                                                        }
                                                    }

                                                    // Obtener Precio de Combuistible
                                                    sentencia = @"SELECT FIRST 1 PRECIO " +
                                                                   "FROM CLAVES " +
                                                                  "WHERE FECHA = @FECHA " +
                                                                    "AND COMBUSTIBLE = @COMBUSTIBLE";
                                                    using (FbCommand comandoPrecio = new FbCommand(sentencia, conexionAjusta))
                                                    {
                                                        comandoPrecio.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha.Date;
                                                        comandoPrecio.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = tmp.Combustible;

                                                        tmp.Precio = Convert.ToDouble(comandoPrecio.ExecuteScalar());
                                                    }

                                                    //// Obtener El Porcentaje de Merma
                                                    //sentencia = @"SELECT coalesce(porcmermactble, 0) as merma " +
                                                    //               "FROM HISTORIA " +
                                                    //              "WHERE FECHA=@FECHA " +
                                                    //                "and COMBUSTIBLE = @COMB " +
                                                    //                "and corte = 1";
                                                    //using (FbCommand comandoMerma = new FbCommand(sentencia, conexionAjusta))
                                                    //{
                                                    //    comandoMerma.Parameters.Add("@FECHA", FbDbType.Date).Value = fecha.Date;
                                                    //    comandoMerma.Parameters.Add("@COMB", FbDbType.Integer).Value = tmp.Combustible;
                                                    //}

                                                    result.Add(tmp);
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            if (!readerTanques.IsClosed) readerTanques.Close();
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                if (conexionAjusta.State == System.Data.ConnectionState.Open)
                                {
                                    conexionAjusta.Close();
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (conexionConsola.State == System.Data.ConnectionState.Open)
                        {
                            conexionConsola.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public ReporteAjuste ObtenerReporte2(DateTime fecha, int combustible, bool a24hrs)
        {
            ReporteAjuste result = new ReporteAjuste();
            //FbConnection conexionAjusta = null;
            //FbConnection conexionConsola = null;
            Dictionary<int, double> precios = new Dictionary<int, double>();

            try
            {
                string sentencia = @"SELECT coalesce(SUM(VOLUMEN1 - VOLUMEN2),0) AS AJUSTE " +
                                      "FROM CLAVES " +
                                     "WHERE FECHAADMIN = @FECHAADMIN " +
                                       "AND COMBUSTIBLE = @COMBUSTIBLE";
                string sentenciaNomComb = @"select c.nombre " +
                                             "from DPVGTCMB c " +
                                            "where c.clave = @COMBUSTIBLE";

                if (a24hrs)
                {
                    sentencia = @"SELECT coalesce(SUM(VOLUMEN1 - VOLUMEN2),0) AS AJUSTE " +
                                   "FROM CLAVES " +
                                  "WHERE FECHA = @FECHAADMIN " +
                                    "AND COMBUSTIBLE = @COMBUSTIBLE";
                }

                //FbCommand comando;
                //FbCommand comandoNombreComb;

                using (FbConnection conexionConsola = new Conexiones().ConexionObtener("GasConsola"))
                {
                    try
                    {
                        conexionConsola.Open();

                        using (FbConnection conexionAjusta = new Conexiones().ConexionObtener("Ajusta"))
                        {
                            try
                            {
                                conexionAjusta.Open();

                                result.Combustible = combustible;
                                result.Precio = getPrecioCombustible(conexionAjusta, fecha, combustible);

                                using (FbCommand comando = new FbCommand(sentencia, conexionAjusta))
                                {
                                    comando.Parameters.Add("@FECHAADMIN", FbDbType.Date).Value = fecha;
                                    comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = combustible;
                                    result.Ajuste = Convert.ToDouble(comando.ExecuteScalar());
                                }

                                using (FbCommand comandoNombreComb = new FbCommand(sentenciaNomComb, conexionConsola))
                                {
                                    comandoNombreComb.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = combustible;
                                    result.NombreCombustible = comandoNombreComb.ExecuteScalar().ToString();
                                }
                            }
                            finally
                            {
                                if (conexionAjusta.State == System.Data.ConnectionState.Open)
                                {
                                    conexionAjusta.Close();
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (conexionConsola.State == System.Data.ConnectionState.Open)
                        {
                            conexionConsola.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
