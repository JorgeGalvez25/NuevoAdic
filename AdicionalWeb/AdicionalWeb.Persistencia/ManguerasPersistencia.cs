using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;
using AdicionalWeb.Persistencia.Enlaces;

namespace AdicionalWeb.Persistencia
{
    public class ManguerasPersistencia
    {
        private static Regex MatchCombustible = new Regex(@"(^[diesel|premium|magna])\w+", RegexOptions.Compiled |
                                                                                           RegexOptions.Multiline |
                                                                                           RegexOptions.IgnoreCase);
        private AdicionalWeb.Entidades.Usuario Usuario;
        private AdicionalWeb.Persistencia.Enlaces.Conexiones _enlace;
        private static AdicionalWeb.Entidades.ListaCombustible _combustibles;
        private static AdicionalWeb.Entidades.ListaCombustible Combustibles
        {
            get
            {
                if (_combustibles == null)
                {
                    CombustiblesPersistencia servicio = new CombustiblesPersistencia();
                    _combustibles = servicio.CombustibleObtenerTodos(new AdicionalWeb.Entidades.FiltroCombustible());
                }
                return _combustibles;
            }
        }

        public ManguerasPersistencia(AdicionalWeb.Entidades.Usuario usr)
        {
            this._enlace = new AdicionalWeb.Persistencia.Enlaces.Conexiones();
            this.Usuario = usr;
        }

        public AdicionalWeb.Entidades.ListaDispensarios ObtenerPosiciones(Estacion est)
        {
            var lstPosiciones = this.ObtenerListaPosiciones(est);
            AdicionalWeb.Entidades.ListaDispensarios lstResult = null;

            switch (est.TipoDispensario)
            {
                case MarcaDispensario.Wayne:
                    lstResult = llenarListaWayne(est, lstPosiciones);
                    break;
                case MarcaDispensario.Bennett:
                    lstResult = llenarListaBennet(est, lstPosiciones);
                    break;
                case MarcaDispensario.Team:
                    lstResult = llenarListaTeam(est, lstPosiciones);
                    break;
                case MarcaDispensario.Gilbarco:
                    lstResult = llenarListaGilbarco(est, lstPosiciones);
                    break;
                case MarcaDispensario.HongYang:
                    lstResult = llenarListaHongYang(est, lstPosiciones);
                    break;
                case MarcaDispensario.Ninguno:
                default:
                    lstResult = new AdicionalWeb.Entidades.ListaDispensarios();
                    break;
            }

            return lstResult;
        }

        public AdicionalWeb.Entidades.Dispensarios MangueraInsertar(AdicionalWeb.Entidades.Dispensarios dispensario, int estacion)
        {
            #region Bitacora

            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            srvAdicional.BitacoraInsertar(new Bitacora()
                {
                    Id_usuario = Usuario == null ? "AdicionalWeb" : Usuario.Nombre,
                    Suceso = "Aplicar cambio de porcentaje",
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.TimeOfDay
                }, estacion);

            #endregion

            string sentencia = "INSERT INTO HISTORIAL(ID_ESTACION, " +
                                                     "FECHA, " +
                                                     "HORA, " +
                                                     "POSICION, " +
                                                     "MANGUERA, " +
                                                     "PORCENTAJE, " +
                                                     "COMBUSTIBLE) " +
                                             "VALUES (@ID_ESTACION, " +
                                                     "@FECHA, " +
                                                     "@HORA, " +
                                                     "@POSICION, " +
                                                     "@MANGUERA, " +
                                                     "@PORCENTAJE, " +
                                                     "@COMBUSTIBLE)";
            bool isOk = false;
            this._enlace.AdicionalHostConsultaTransaccion((cmd) =>
                {
                    cmd.CommandText = sentencia;
                    cmd.Parameters.AddRange(this.ObtenerParametros(dispensario, estacion));
                    isOk = cmd.ExecuteNonQuery() > 0;
                });

            return isOk ? dispensario : null;
        }

        private FbParameter[] ObtenerParametros(AdicionalWeb.Entidades.Dispensarios dispensario, int estacion)
        {
            FbParameter[] result = new FbParameter[]
                {
                    new FbParameter("@ID_ESTACION", FbDbType.Integer) { Value = estacion },
                    new FbParameter("@FECHA", FbDbType.Date) { Value = (dispensario.fecha== DateTime.MinValue ? DateTime.Now : dispensario.fecha).Date },
                    new FbParameter("@HORA", FbDbType.Time) { Value = (dispensario.fecha== DateTime.MinValue ? DateTime.Now : dispensario.fecha).TimeOfDay },
                    new FbParameter("@POSICION", FbDbType.Integer) { Value = dispensario.posicion },
                    new FbParameter("@MANGUERA", FbDbType.Integer) { Value = dispensario.id },
                    new FbParameter("@PORCENTAJE", FbDbType.Numeric) { Value = dispensario.valor },
                    new FbParameter("@COMBUSTIBLE", FbDbType.SmallInt) { Value = dispensario.combustible }
                };
            return result;
        }

        #region Obtener Lista

        private AdicionalWeb.Entidades.ListaDispensarios llenarListaWayne(Estacion est, List<int> listaPosiciones)
        {
            string[] nombresCombustibles = new string[] { "", "Magna", "Premium", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

            AdicionalWeb.Entidades.ListaDispensarios lstResult = new AdicionalWeb.Entidades.ListaDispensarios();
            ListaHistorial mangueras = null;
            listaPosiciones.ForEach(p =>
                {
                    mangueras = this.ObtenerPorPosicion(est, p);
                    mangueras.ForEach(m =>
                        {
                            if (!combustibles.ContainsKey(m.Combustible))
                            {
                                combustibles.Add(m.Combustible, true);
                                lstResult.Add(new AdicionalWeb.Entidades.Dispensarios()
                                    {
                                        id = m.Manguera,
                                        estacion = m.Id_Estacion,
                                        dispensario = p,
                                        combustible = m.Combustible,
                                        nombre = nombresCombustibles[m.Combustible],
                                        valor = decimal.ToDouble(m.Porcentaje)
                                    });
                            }
                        });
                });

            return lstResult;
        }

        private AdicionalWeb.Entidades.ListaDispensarios llenarListaBennet(Estacion est, List<int> listaPosiciones)
        {
            AdicionalWeb.Entidades.ListaDispensarios lstResult = new AdicionalWeb.Entidades.ListaDispensarios();
            ListaHistorial mangueras = null;
            AdicionalWeb.Entidades.Combustibles combustibles = null;
            listaPosiciones.ForEach(p =>
                {
                    mangueras = this.ObtenerPorPosicion(est, p);
                    mangueras.ForEach(m =>
                        {
                            combustibles = Combustibles.Find(q => q.Id == m.Combustible);
                            lstResult.Add(new AdicionalWeb.Entidades.Dispensarios()
                                {
                                    id = m.Manguera,
                                    estacion = m.Id_Estacion,
                                    posicion = p,
                                    combustible = m.Combustible,
                                    nombre = MatchCombustible.Match(combustibles.Combustible).Groups[0].Value.Trim(),// Regex.Match(combustibles.Combustible, @"(^[diesel|premium|magna])\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value.Trim(),
                                    valor = decimal.ToDouble(m.Porcentaje)
                                });
                        });
                });

            return lstResult;
        }

        private AdicionalWeb.Entidades.ListaDispensarios llenarListaTeam(Estacion est, List<int> listaPosiciones)
        {
            AdicionalWeb.Entidades.ListaDispensarios lstResult = new AdicionalWeb.Entidades.ListaDispensarios();
            int dispensario = 1;
            ListaHistorial mangueras = null;
            AdicionalWeb.Entidades.Combustibles combustibles = null;
            listaPosiciones.ForEach(p =>
                {
                    if (p % 2 == 1)
                    {
                        mangueras = this.ObtenerPorPosicion(est, p);

                        mangueras.ForEach(m =>
                        {
                            combustibles = Combustibles.Find(q => q.Id == m.Combustible);
                            lstResult.Add(new AdicionalWeb.Entidades.Dispensarios()
                            {
                                id = m.Manguera,
                                estacion = m.Id_Estacion,
                                dispensario = dispensario++,
                                combustible = m.Combustible,
                                nombre = MatchCombustible.Match(combustibles.Combustible).Groups[0].Value.Trim(),// Regex.Match(combustibles.Combustible, @"(^[diesel|premium|magna])\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value.Trim(),
                                valor = decimal.ToDouble(m.Porcentaje)
                            });
                        });
                    }
                });
            return lstResult;
        }

        private AdicionalWeb.Entidades.ListaDispensarios llenarListaGilbarco(Estacion est, List<int> listaPosiciones)
        {
            AdicionalWeb.Entidades.ListaDispensarios lstResult = new AdicionalWeb.Entidades.ListaDispensarios();

            string[] nombresCombustibles = new string[] { "", "Gasolina", "Gasolina", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();
            ListaHistorial mangueras = null;

            listaPosiciones.ForEach(p =>
                {
                    mangueras = this.ObtenerPorPosicion(est, p);
                    mangueras.ForEach(m =>
                        {
                            if (!combustibles.ContainsKey(m.Combustible))
                            {
                                if (m.Combustible == 1 || m.Combustible == 2)
                                    m.Combustible = 1;
                                if (!combustibles.ContainsKey(m.Combustible))
                                {
                                    combustibles.Add(m.Combustible, true);
                                    lstResult.Add(new AdicionalWeb.Entidades.Dispensarios()
                                        {
                                            id = m.Manguera,
                                            estacion = m.Id_Estacion,
                                            dispensario = p,
                                            combustible = m.Combustible,
                                            nombre = nombresCombustibles[m.Combustible],
                                            valor = decimal.ToDouble(m.Porcentaje),
                                            showTitleId = false
                                        });
                                }
                            }
                        });
                });
            return lstResult;
        }

        private AdicionalWeb.Entidades.ListaDispensarios llenarListaHongYang(Estacion est, List<int> listaPosiciones)
        {
            AdicionalWeb.Entidades.ListaDispensarios lstResult = new AdicionalWeb.Entidades.ListaDispensarios();
            ListaHistorial mangueras = null;
            AdicionalWeb.Entidades.Combustibles combustibles = null;
            listaPosiciones.ForEach(p =>
                {
                    mangueras = this.ObtenerPorPosicion(est, p);
                    mangueras.ForEach(m =>
                        {
                            combustibles = Combustibles.Find(q => q.Id == m.Combustible);
                            lstResult.Add(new AdicionalWeb.Entidades.Dispensarios()
                                {
                                    id = m.Manguera,
                                    estacion = m.Id_Estacion,
                                    dispensario = p,
                                    combustible = m.Combustible,
                                    nombre = MatchCombustible.Match(combustibles.Combustible).Groups[0].Value.Trim(),// Regex.Match(combustibles.Combustible, @"(^[diesel|premium|magna])\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value.Trim(),
                                    valor = decimal.ToDouble(m.Porcentaje)
                                });
                        });
                });

            return lstResult;
        }

        private List<int> ObtenerListaPosiciones(Estacion est)
        {
            List<int> pResult = new List<int>();

            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            pResult = srvAdicional.HistorialObtenerPosiciones(est.Id);

            //this._enlace.AdicionalHostConsulta((cmd) =>
            //    {
            //        cmd.CommandText = "SELECT DISTINCT(POSICION) FROM HISTORIAL WHERE ID_ESTACION = @ID_ESTACION";
            //        cmd.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = est.Id;

            //        using (FbDataReader reader = cmd.ExecuteReader())
            //        {
            //            try
            //            {
            //                while (reader.Read())
            //                {
            //                    if (!reader.IsDBNull(0))
            //                    {
            //                        pResult.Add(reader.GetInt32(0));
            //                    }
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed)
            //                {
            //                    reader.Close();
            //                }
            //            }
            //        }

            //    });

            return pResult;
        }

        private ListaHistorial ObtenerPorPosicion(Estacion est, int posicion)
        {
            ListaHistorial pResult = new ListaHistorial();
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            pResult = srvAdicional.HistorialObtenerPorPosicion(est.Id, posicion);
            //string sentencia = "SELECT * " +
            //                    " FROM HISTORIAL " +
            //                   " WHERE ID_ESTACION = ID_ESTACION " +
            //                     " AND POSICION = @POSICION " +
            //                     " AND FECHA = (SELECT MAX(FECHA) " +
            //                                    " FROM HISTORIAL " +
            //                                   " WHERE ID_ESTACION = ID_ESTACION " +
            //                                     " AND POSICION = @POSICION) " +
            //                     " AND HORA = (SELECT MAX(HORA) " +
            //                                   " FROM HISTORIAL " +
            //                                  " WHERE ID_ESTACION = ID_ESTACION " +
            //                                    " AND POSICION = @POSICION " +
            //                                    " AND FECHA = (SELECT MAX(FECHA) " +
            //                                                   " FROM HISTORIAL " +
            //                                                  " WHERE ID_ESTACION = ID_ESTACION " +
            //                                                    " AND POSICION = @POSICION)) " +
            //                   " ORDER BY MANGUERA";

            ////string sentencia = " SELECT H.* " +
            ////                    " FROM HISTORIAL H " +
            ////                   " WHERE H.ID_ESTACION = @ID_ESTACION  " +
            ////                     " AND H.POSICION = @POSICION " +
            ////                     " AND H.FECHA = (SELECT MAX(FECHA) " +
            ////                                      " FROM HISTORIAL " +
            ////                                     " WHERE ID_ESTACION = H.ID_ESTACION " +
            ////                                       " AND POSICION = H.POSICION " +
            ////                                       " AND COMBUSTIBLE = H.COMBUSTIBLE " +
            ////                                       " AND MANGUERA = H.MANGUERA) " +
            ////                     " AND H.HORA = (SELECT MAX(HORA) " +
            ////                                     " FROM HISTORIAL " +
            ////                                    " WHERE ID_ESTACION = H.ID_ESTACION " +
            ////                                      " AND POSICION = H.POSICION " +
            ////                                      " AND COMBUSTIBLE = H.COMBUSTIBLE " +
            ////                                      " AND MANGUERA = H.MANGUERA " +
            ////                                      " AND FECHA = (SELECT MAX(FECHA) " +
            ////                                                     " FROM HISTORIAL " +
            ////                                                    " WHERE ID_ESTACION = H.ID_ESTACION " +
            ////                                                      " AND POSICION = H.POSICION " +
            ////                                                      " AND COMBUSTIBLE = H.COMBUSTIBLE " +
            ////                                                      " AND MANGUERA = H.MANGUERA)) " +
            ////                   " ORDER BY H.MANGUERA";






            ///* @"SELECT * " +
            //     " FROM HISTORIAL " +
            //    " WHERE ID_ESTACION = @ID_ESTACION " +
            //      " AND POSICION = @POSICION " +
            //      " AND FECHA = (SELECT MAX(FECHA) " +
            //                     " FROM HISTORIAL " +
            //                    " WHERE ID_ESTACION = @ID_ESTACION " +
            //                      " AND POSICION = @POSICION) " +
            //      " AND HORA = (SELECT MAX(HORA) " +
            //                    " FROM HISTORIAL " +
            //                   " WHERE ID_ESTACION = @ID_ESTACION " +
            //                     " AND POSICION = @POSICION " +
            //                     " AND FECHA = (SELECT MAX(FECHA) " +
            //                                    " FROM HISTORIAL " +
            //                                   " WHERE ID_ESTACION = @ID_ESTACION " +
            //                                     " AND POSICION = @POSICION)) " +
            //     " ORDER BY MANGUERA";/**/
            //this._enlace.AdicionalHostConsulta((cmd) =>
            //    {
            //        cmd.CommandText = sentencia;
            //        cmd.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = est.Id;
            //        cmd.Parameters.Add("@POSICION", FbDbType.Integer).Value = posicion;

            //        using (FbDataReader reader = cmd.ExecuteReader())
            //        {
            //            try
            //            {
            //                while (reader.Read())
            //                {
            //                    pResult.Add(this.HistorialReaderToEntidad(reader));
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed)
            //                {
            //                    reader.Close();
            //                }
            //            }
            //        }
            //    });

            return pResult;
        }

        private Historial HistorialReaderToEntidad(FbDataReader reader)
        {
            Historial pResult = new Historial();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Id_Estacion = reader["ID_ESTACION"] is System.DBNull ? 0 : (int)reader["ID_ESTACION"];
            pResult.Fecha = reader["FECHA"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["FECHA"];
            pResult.Hora = reader["HORA"] is System.DBNull ? TimeSpan.MinValue : (TimeSpan)reader["HORA"];
            pResult.Posicion = reader["POSICION"] is System.DBNull ? 0 : (int)reader["POSICION"];
            pResult.Manguera = reader["MANGUERA"] is System.DBNull ? 0 : (int)reader["MANGUERA"];
            pResult.Porcentaje = reader["PORCENTAJE"] is System.DBNull ? 0 : (decimal)reader["PORCENTAJE"];
            pResult.Estado = reader["ESTADO"] is System.DBNull ? "" : (string)reader["ESTADO"];
            pResult.Combustible = (short)(reader["COMBUSTIBLE"] is System.DBNull ? 0 : reader["COMBUSTIBLE"]);
            //pResult.Calibracion = reader["CALIBRACION"] is System.DBNull ? 0 : (int)reader["CALIBRACION"];

            return pResult;
        }

        #endregion

        #region Cambiar Porcentajes

        public AdicionalWeb.Entidades.Dispensarios ManguerasInsertar(AdicionalWeb.Entidades.ListaDispensarios dispensario, int estacion)
        {
            AdicionalWeb.Entidades.Dispensarios result = null;
            bool resp = false;
            foreach (var n in dispensario)
            {
                result = this.MangueraInsertar(n, estacion);
                if (result == null)
                {
                    resp = true;
                }
            }

            return resp ? null : result;
        }

        #endregion
    }
}
