using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace Persistencia
{
    public class ReporteVentasCombustiblePersistencia
    {
        private bool De6a6 { get { return "Si".Equals(ConfigurationManager.AppSettings["de6a6"] ?? "No", StringComparison.OrdinalIgnoreCase); } }

        public List<ReporteVentasCombustible> ReporteVentasReales(DateTime fecha)//, string noEstacion)
        {
            List<VentasCombustible> lstVentas = De6a6 ? ProcesarAjustes6a6(fecha) : ProcesarAjustes12a12(fecha);// GetVentas(fecha);
            return (from i in lstVentas
                    select new ReporteVentasCombustible()
                    {
                        Combustible = i.Combustible,
                        Descripcion = i.Descripcion,
                        Importe = i.ImporteVentaReal,
                        Volumen = i.VentaReal,
                        Precio = i.Precio
                    }).ToList();
            //return this.internalReporteVentasReales(fecha, noEstacion, lstVentas);
        }

        public List<ReporteVentasCombustible> ReporteVentasAjustadas(DateTime fecha)
        {
            List<VentasCombustible> lstVentas = De6a6 ? ProcesarAjustes6a6(fecha) : ProcesarAjustes12a12(fecha);// GetVentas(fecha);
            return (from i in lstVentas
                    select new ReporteVentasCombustible()
                    {
                        Combustible = i.Combustible,
                        Descripcion = i.Descripcion,
                        Importe = i.ImporteVentaAjustada,
                        Volumen = i.VentaAjustada,
                        Precio = i.Precio
                    }).ToList();
            //return this.internalReporteVentasAjustadas(fecha, lstVentas);
        }

        public List<ReporteVentasCombustible> ReporteAjusteCombustible(DateTime fecha)//, string noEstacion)
        {
            List<VentasCombustible> lstVentas = De6a6 ? ProcesarAjustes6a6(fecha) : ProcesarAjustes12a12(fecha);// GetVentas(fecha);
            return (from i in lstVentas
                    select new ReporteVentasCombustible()
                    {
                        Combustible = i.Combustible,
                        Descripcion = i.Descripcion,
                        Importe = i.ImporteDiferencia,
                        Volumen = i.Diferencia,
                        Precio = i.Precio
                    }).ToList();
            //return this.internalReporteAjusteCombustible(fecha, noEstacion, lstVentas);
        }

        //private string proceso(string argument)
        //{
        //    Process p = new Process();

        //    try
        //    {
        //        p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"LibsDelphi.exe");
        //        p.StartInfo.Arguments = argument;
        //        p.StartInfo.CreateNoWindow = true;
        //        p.StartInfo.UseShellExecute = false;
        //        p.StartInfo.RedirectStandardOutput = true;

        //        p.Start();
        //        p.WaitForExit();
        //        return p.StandardOutput.ReadToEnd();
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        p.Dispose();
        //        p = null;
        //    }
        //}

        //private List<ReporteVentasCombustible> agruparVentas(IEnumerable<ReporteVentasCombustible> linq)
        //{
        //    return (from i in linq.OrderBy(p => p.Combustible)
        //            group i by i.Combustible into g
        //            let items = g.Where(p => p.Combustible == g.Key)
        //            let uniq = items.FirstOrDefault() ?? new ReporteVentasCombustible()
        //            select new ReporteVentasCombustible()
        //            {
        //                Combustible = g.Key,
        //                Descripcion = uniq.Descripcion,
        //                Precio = uniq.Precio,
        //                Importe = items.Sum(p => p.Importe),
        //                Volumen = items.Sum(p => p.Volumen),
        //                HexHash = uniq.HexHash
        //            }).OrderBy(p => p.Combustible).ToList();
        //}

        //private List<ReporteVentasCombustible> internalReporteVentasAjustadas(DateTime fecha, IEnumerable<ReporteVentasCombustible> lstVentas)
        //{
        //    return agruparVentas(lstVentas);
        //}

        //private List<ReporteVentasCombustible> internalReporteVentasReales(DateTime fecha, string noEstacion, IEnumerable<ReporteVentasCombustible> lstVentas)
        //{
        //    string frmCommand = string.Concat(string.Format("gethexhash {0} ", noEstacion), "{0}");
        //    var dicHash = from i in lstVentas.Where(p => !string.IsNullOrEmpty(p.HexHash))
        //                                     .Select(p => p.HexHash)
        //                                     .Distinct()
        //                  let r_hash = Convert.ToDecimal(proceso(string.Format(frmCommand, i)), ContantesAdicional.CulturaLocal)
        //                  select new
        //                  {
        //                      Key = i,
        //                      Value = r_hash
        //                  };

        //    return agruparVentas((from i in lstVentas
        //                          join j in dicHash
        //                          on i.HexHash equals j.Key into g
        //                          from k in g.DefaultIfEmpty(new { Key = string.Empty, Value = i.Importe }).OrderBy(p => p.Key)
        //                          let r_hash = k.Value
        //                          let r_vol = i.Precio <= 0 ? r_hash : (r_hash / i.Precio)
        //                          select new ReporteVentasCombustible()
        //                          {
        //                              Combustible = i.Combustible,
        //                              Descripcion = i.Descripcion,
        //                              Precio = i.Precio,
        //                              Importe = r_hash,
        //                              Volumen = r_vol,
        //                              HexHash = i.HexHash
        //                          }).AsQueryable()).OrderBy(p => p.Combustible).ToList();


        //    //return agruparEjemplos(from i in lstVentas
        //    //                       let c_hash = dicHash.FirstOrDefault(p => p.Key.Equals(i.HexHash))
        //    //                       let r_hash = c_hash == null
        //    //                                        ? i.Importe
        //    //                                        : c_hash.value //ContainsKey(i.HexHash) ? dicHash[i.HexHash] : i.Importe
        //    //                       let r_vol = r_hash / i.Precio
        //    //                       select new ReporteVentasCombustible()
        //    //                       {
        //    //                           Combustible = i.Combustible,
        //    //                           Precio = i.Precio,
        //    //                           Importe = r_hash,
        //    //                           Volumen = r_vol,
        //    //                           HexHash = i.HexHash
        //    //                       }).OrderBy(p => p.Combustible);

        //    //return agruparEjemplos(from i in lstVentas.OrderBy(p => p.HexHash)
        //    //                       from j in lstVentas.Where(p => !string.IsNullOrEmpty(p.HexHash))
        //    //                                          .GroupBy(p => p.HexHash)
        //    //                       let _isHash = j.FirstOrDefault(p => p.HexHash == i.HexHash)
        //    //                       let r_hash = _isHash == null ? i.Importe : Convert.ToDecimal(proceso(string.Format("gethexhash {0} {1}", noEstacion, _isHash.HexHash)))
        //    //                       let r_vol = r_hash / i.Precio
        //    //                       select new ReporteVentasCombustible()
        //    //                       {
        //    //                           Combustible = i.Combustible,
        //    //                           Precio = i.Precio,
        //    //                           Importe = r_hash,
        //    //                           Volumen = r_vol,
        //    //                           HexHash = i.HexHash
        //    //                       }).OrderBy(p => p.Combustible);
        //}

        //private List<ReporteVentasCombustible> internalReporteAjusteCombustible(DateTime fecha, string noEstacion, IEnumerable<ReporteVentasCombustible> lstVentas)
        //{
        //    var ventasReales = internalReporteVentasReales(fecha, noEstacion, lstVentas);
        //    var ventasAjustadas = internalReporteVentasAjustadas(fecha, lstVentas);

        //    return agruparVentas(from i in ventasAjustadas
        //                         join j in ventasReales
        //                         on i.Combustible equals j.Combustible into cmb
        //                         from k in cmb.AsQueryable().DefaultIfEmpty(new ReporteVentasCombustible())
        //                         select new ReporteVentasCombustible()
        //                         {
        //                             Combustible = i.Combustible,
        //                             Descripcion = i.Descripcion,
        //                             HexHash = i.HexHash,
        //                             Importe = i.Importe - k.Importe,
        //                             Precio = i.Precio,
        //                             Volumen = i.Volumen - k.Volumen
        //                         }).OrderBy(p => p.Combustible).ToList();
        //}

        //private List<VentasCombustible> GetVentas(DateTime fecha)
        //{
        //    Dictionary<int, string> combustibles = getCombustibles();

        //    using (FbCommand comm = new FbCommand())
        //    {
        //        using (comm.Connection = new Conexiones().ConexionObtener(Conexiones.AJUSTADOR))
        //        {
        //            comm.Connection.Open();

        //            comm.CommandText = "Select m.Combustible, " +
        //                                      "m.Precio, " +

        //                                      "Sum(m.VentaReal) As VentaReal, " +
        //                                      "Sum(m.ImporteVentaReal) As ImporteVentaReal, " +

        //                                      "Sum(m.VentaAjustada) As VentaAjustada, " +
        //                                      "Sum(m.ImporteVentaAjustada) As ImporteVentaAjustada, " +

        //                                      "Sum(m.Diferencia) As Diferencia, " +
        //                                      "Sum(m.ImporteVentaReal - m.ImporteVentaAjustada) As ImporteDirefencia " +
        //                                 "From (Select Combustible, " +
        //                                              "Precio, " +
        //                                              "Volumen1 As VentaReal, " +
        //                                              "Volumen2 As VentaAjustada, " +
        //                                              "(Volumen1 - Volumen2) As Diferencia, " +
        //                                              "(Precio * Volumen1) As ImporteVentaReal, " +
        //                                              "(Precio * Volumen2) As ImporteVentaAjustada " +
        //                                         "From Claves " +
        //                                        "Where FechaAdmin = @FECHA) m " +
        //                                "Group By m.Combustible,  " +
        //                                      "m.Precio";
        //            comm.Parameters.Add(new FbParameter("@FECHA", fecha.Date));

        //            try
        //            {
        //                using (FbDataReader reader = comm.ExecuteReader())
        //                {
        //                    try
        //                    {
        //                        List<VentasCombustible> lst = new List<VentasCombustible>();

        //                        int _c = 0;
        //                        string _cDesc = string.Empty;

        //                        while (reader.Read())
        //                        {
        //                            _c = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
        //                            _cDesc = combustibles.ContainsKey(_c) ? combustibles[_c] : string.Empty;

        //                            lst.Add(new VentasCombustible()
        //                            {
        //                                Combustible = _c,
        //                                Descripcion = _cDesc,
        //                                Precio = reader.IsDBNull(1) ? 0M : reader.GetDecimal(0),

        //                                VentaReal = reader.IsDBNull(2) ? 0M : reader.GetDecimal(2),
        //                                ImporteVentaReal = reader.IsDBNull(3) ? 0M : reader.GetDecimal(3),

        //                                VentaAjustada = reader.IsDBNull(4) ? 0M : reader.GetDecimal(4),
        //                                ImporteVentaAjustada = reader.IsDBNull(5) ? 0M : reader.GetDecimal(5),

        //                                Diferencia = reader.IsDBNull(6) ? 0M : reader.GetDecimal(6),
        //                                ImporteDiferencia = reader.IsDBNull(7) ? 0M : reader.GetDecimal(7),
        //                            });
        //                        }
        //                        return lst;
        //                    }
        //                    finally
        //                    {
        //                        if (!reader.IsClosed)
        //                        {
        //                            reader.Close();
        //                        }
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                if (comm.Connection.State != ConnectionState.Closed)
        //                {
        //                    comm.Connection.Close();
        //                }
        //            }
        //        }
        //    }
        //}

        //private static Dictionary<int, string> getCombustibles()
        //{
        //    using (FbCommand comm = new FbCommand())
        //    {
        //        using (comm.Connection = new Conexiones().ConexionObtener(Conexiones.GASCONSOLA))
        //        {
        //            comm.Connection.Open();

        //            comm.CommandText = "Select Clave, " +
        //                                      "Nombre " +
        //                                 "From DpvgTCmb ";

        //            try
        //            {
        //                using (FbDataReader reader = comm.ExecuteReader())
        //                {
        //                    try
        //                    {
        //                        Dictionary<int, string> lst = new Dictionary<int, string>();

        //                        while (reader.Read())
        //                        {
        //                            lst.Add(reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
        //                                    reader.IsDBNull(1) ? string.Empty : reader.GetString(1));
        //                        }
        //                        return lst;
        //                    }
        //                    finally
        //                    {
        //                        if (!reader.IsClosed)
        //                        {
        //                            reader.Close();
        //                        }
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                if (comm.Connection.State != ConnectionState.Closed)
        //                {
        //                    comm.Connection.Close();
        //                }
        //            }
        //        }
        //    }
        //}

        #region De 6 a 6

        internal List<VentasCombustible> ProcesarAjustes6a6(DateTime fecha)
        {
            Conexiones conn = new Conexiones();

            using (FbConnection connConsola = conn.ConexionObtener(Conexiones.GASCONSOLA))
            {
                using (FbConnection connAjustador = conn.ConexionObtener(Conexiones.AJUSTADOR))
                {
                    connAjustador.Open();
                    connConsola.Open();
                    try
                    {
                        using (FbCommand commConsola = connConsola.CreateCommand())
                        {
                            using (FbCommand commAjusta = connAjustador.CreateCommand())
                            {
                                var salidas = this.salidasDisponibles(commConsola, fecha);
                                var jarreos = this.jarreos(commConsola, fecha);
                                this.salidasAjustes(commAjusta, fecha, salidas);
                                var precios = this.precios(commAjusta, fecha);
                                var combustibles = this.obtenerCombustibles(commConsola);

                                List<VentasCombustible> lst = new List<VentasCombustible>();
                                VentasCombustible resultado = null;

                                foreach (var key in salidas.Keys)
                                {
                                    resultado = new VentasCombustible();
                                    resultado.Combustible = key;

                                    if (combustibles.ContainsKey(key))
                                    {
                                        resultado.Descripcion = combustibles[key];
                                    }

                                    if (precios.ContainsKey(key))
                                    {
                                        resultado.Precio = precios[key];
                                    }

                                    if (jarreos.ContainsKey(key))
                                    {
                                        resultado.VentaReal = salidas[key].SalidaDisponible - jarreos[key];
                                    }
                                    else
                                    {
                                        resultado.VentaReal = salidas[key].SalidaDisponible;
                                    }

                                    resultado.VentaAjustada = resultado.VentaReal - salidas[key].Ajuste;
                                    resultado.Diferencia = resultado.VentaReal - resultado.VentaAjustada;

                                    resultado.ImporteVentaReal = resultado.VentaReal * resultado.Precio;
                                    resultado.ImporteVentaAjustada = resultado.VentaAjustada * resultado.Precio;
                                    resultado.ImporteDiferencia = resultado.ImporteVentaReal - resultado.ImporteVentaAjustada;

                                    lst.Add(resultado);
                                }

                                return lst;
                            }
                        }
                    }
                    finally
                    {
                        if (connConsola.State == ConnectionState.Open)
                            connConsola.Close();
                        if (connAjustador.State == ConnectionState.Open)
                            connAjustador.Close();
                    }
                }
            }
            /* Salida Disponible
             * Select Combustible,
             *        Coalesce(Sum(Volumen),0) As Volumen
             *   From DpvgMovi 
             *  Where Tag <> 1 
             *    And Hora :fIni And :fFin -- de las 06:00 a +i Dia 05:59:59
             *  Group By Combustible
             */

            /* Jarreos
             * Select Combustible,
             *        Coalesce(Sum(Volumen),0) As Volumen
             *   From DpvgMovi 
             *  Where Tag<>1 
             *    And Hora :fIni And :fFin -- de las 06:00 a +i Dia 05:59:59
             *    And Jarreo = 'Si'
             *  Group By Combustible
             */

            /* Salida Disponible y Ajuste
             * Select Combustible,
             *        Coalesce(Sum(Volumen1), 0) As Volumen, 
             *        Coalesce(Sum(Volumen2), 0) As Volumen2 
             *   From Claves
             *  Where FechaAdmin = :Fecha
             *  
             * Salida Disponible += Volumen
             * Ajuste = Volumen - Volumen2
             */

            /* Precio
             * 
             */

            /* Ventas Litros Antes = Salida Disponible - Jarreos
             * Ventas Litros Despues = Ventas Litros Antes - Ajuste
             * Diferencia = Ventas Listros Antes - Venta Listros Despues
             */
        }

        private Dictionary<int, SaldosAjustes> salidasDisponibles(FbCommand commConsola, DateTime fecha)
        {
            commConsola.CommandText = "Select Coalesce(Combustible, 0) As Combustible, " +
                                             "Coalesce(Sum(Volumen), 0) As Volumen " +
                                        "From DpvgMovi " +
                                       "Where Tag <> 1 " +
                                         "And Hora Between @FINI And @FFIN " +
                                       "Group By Combustible";

            commConsola.Parameters.Clear();
            commConsola.Parameters.AddWithValue("@FINI", fecha.Date.AddHours(6));
            commConsola.Parameters.AddWithValue("@FFIN", fecha.Date.AddDays(1).AddHours(6).AddTicks(-1));

            using (FbDataReader reader = commConsola.ExecuteReader())
            {
                try
                {
                    Dictionary<int, SaldosAjustes> salidas = new Dictionary<int, SaldosAjustes>();
                    int combustible = 0;
                    decimal volumen = 0M;

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        volumen = reader.IsDBNull(1) ? 0M : reader.GetDecimal(1);

                        if (salidas.ContainsKey(combustible))
                        {
                            salidas[combustible].SalidaDisponible += volumen;
                        }
                        else
                        {
                            salidas.Add(combustible, new SaldosAjustes()
                            {
                                Combustible = combustible,
                                SalidaDisponible = volumen
                            });
                        }
                    }

                    return salidas;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private Dictionary<int, decimal> jarreos(FbCommand commConsola, DateTime fecha)
        {
            commConsola.CommandText = "Select Coalesce(Combustible, 0) As Combustible, " +
                                             "Coalesce(Sum(Volumen), 0) As Volumen " +
                                        "From DpvgMovi " +
                                       "Where Tag <> 1 " +
                                         "And Hora Between @FINI And @FFIN " +
                                         "And Jarreo = 'Si' " +
                                       "Group By Combustible";

            commConsola.Parameters.Clear();
            commConsola.Parameters.AddWithValue("@FINI", fecha.Date.AddHours(6));
            commConsola.Parameters.AddWithValue("@FFIN", fecha.Date.AddDays(1).AddHours(6).AddTicks(-1));

            using (FbDataReader reader = commConsola.ExecuteReader())
            {
                try
                {
                    Dictionary<int, decimal> salidas = new Dictionary<int, decimal>();
                    int combustible = 0;
                    decimal volumen = 0M;

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        volumen = reader.IsDBNull(1) ? 0M : reader.GetDecimal(1);

                        if (salidas.ContainsKey(combustible))
                        {
                            salidas[combustible] += volumen;
                        }
                        else
                        {
                            salidas.Add(combustible, volumen);
                        }
                    }

                    return salidas;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private Dictionary<int, SaldosAjustes> salidasAjustes(FbCommand commAjustador, DateTime fecha, Dictionary<int, SaldosAjustes> salidasDisponibles)
        {
            commAjustador.CommandText = "Select Coalesce(Combustible, 0) As Combustible, " +
                                               "Coalesce(Sum(Volumen1), 0) As Volumen, " +
                                               "Coalesce(Sum(Volumen2), 0) As Volumen2 " +
                                          "From Claves " +
                                         "Where FechaAdmin = @FECHA " +
                                         "Group by Combustible";

            commAjustador.Parameters.Clear();
            commAjustador.Parameters.AddWithValue("@FECHA", fecha.Date);

            using (FbDataReader reader = commAjustador.ExecuteReader())
            {
                try
                {
                    int combustible = 0;
                    decimal salidaDisponible = 0M;
                    decimal ajuste = 0M;

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        salidaDisponible = reader.IsDBNull(1) ? 0M : reader.GetDecimal(1);
                        ajuste = reader.IsDBNull(2) ? 0M : reader.GetDecimal(2);

                        if (salidasDisponibles.ContainsKey(combustible))
                        {
                            salidasDisponibles[combustible].SalidaDisponible += salidaDisponible;
                            salidasDisponibles[combustible].Ajuste = salidaDisponible - ajuste;
                        }
                        else
                        {
                            salidasDisponibles.Add(combustible, new SaldosAjustes()
                            {
                                Combustible = combustible,
                                SalidaDisponible = salidaDisponible,
                                Ajuste = salidaDisponible - ajuste
                            });
                        }
                    }

                    return salidasDisponibles;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }


        #endregion

        #region De 12 a 12

        internal List<VentasCombustible> ProcesarAjustes12a12(DateTime fecha)
        {
            Conexiones conn = new Conexiones();

            using (FbConnection connConsola = conn.ConexionObtener(Conexiones.GASCONSOLA))
            {
                using (FbConnection connAjustador = conn.ConexionObtener(Conexiones.AJUSTADOR))
                {
                    connAjustador.Open();
                    connConsola.Open();
                    try
                    {
                        using (FbCommand commConsola = connConsola.CreateCommand())
                        {
                            using (FbCommand commAjusta = connAjustador.CreateCommand())
                            {
                                var salidas = this.saldos12a12(commAjusta, fecha);
                                var precios = this.precios(commAjusta, fecha);
                                var combustible = this.obtenerCombustibles(commConsola);

                                List<VentasCombustible> lst = new List<VentasCombustible>();
                                VentasCombustible resultado = null;

                                foreach (var key in salidas.Keys)
                                {
                                    resultado = new VentasCombustible();
                                    resultado.VentaReal = salidas[key].SalidaDisponible;
                                    resultado.VentaAjustada = salidas[key].Ajuste;
                                    resultado.Diferencia = resultado.VentaReal - resultado.VentaAjustada;

                                    if (precios.ContainsKey(key))
                                    {
                                        resultado.ImporteVentaReal = resultado.VentaReal * precios[key];
                                        resultado.ImporteVentaAjustada = resultado.VentaAjustada * precios[key];
                                        resultado.ImporteDiferencia = resultado.ImporteVentaReal - resultado.ImporteVentaAjustada;

                                        if (combustible.ContainsKey(key))
                                        {
                                            resultado.Descripcion = combustible[key];
                                        }
                                    }
                                    lst.Add(resultado);
                                }

                                return lst;
                            }
                        }
                    }
                    finally
                    {
                        if (connAjustador.State == ConnectionState.Open)
                            connAjustador.Close();
                    }
                }
            }
        }

        private Dictionary<int, SaldosAjustes> saldos12a12(FbCommand commAjustador, DateTime fecha)
        {
            commAjustador.CommandText = "Select Coalesce(Combustible, 0), " +
                                               "Coalesce(Sum(Salidas - Jarreos), 0) As Salidas, " +
                                               "Coalesce(Sum(Diferencia - Jarreos), 0) As Ajustado " +
                                          "From Historia " +
                                         "Where Fecha = @FECHA " +
                                         "Group By Combustible";

            commAjustador.Parameters.Clear();
            commAjustador.Parameters.AddWithValue("@FECHA", fecha.Date);

            using (FbDataReader reader = commAjustador.ExecuteReader())
            {
                try
                {
                    int combustible = 0;
                    decimal ventas = 0M;
                    decimal ajustadas = 0M;
                    Dictionary<int, SaldosAjustes> salidas = new Dictionary<int, SaldosAjustes>();

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        ventas = reader.IsDBNull(1) ? 0M : reader.GetDecimal(1);
                        ajustadas = reader.IsDBNull(2) ? 0M : reader.GetDecimal(2);

                        if (salidas.ContainsKey(combustible))
                        {
                            salidas[combustible].SalidaDisponible += ventas;
                            salidas[combustible].Ajuste += ajustadas;
                        }
                        else
                        {
                            salidas.Add(combustible, new SaldosAjustes()
                            {
                                Combustible = combustible,
                                SalidaDisponible = ventas,
                                Ajuste = ajustadas
                            });
                        }
                    }

                    return salidas;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        private Dictionary<int, decimal> precios(FbCommand commAjustador, DateTime fecha)
        {
            commAjustador.CommandText = "Select Distinct " +
                                               "Combustible, " +
                                               "Precio " +
                                          "From Claves " +
                                         "Where Fecha = @FECHA";

            commAjustador.Parameters.Clear();
            commAjustador.Parameters.AddWithValue("@FECHA", fecha.Date);

            using (FbDataReader reader = commAjustador.ExecuteReader())
            {
                try
                {
                    Dictionary<int, decimal> salidas = new Dictionary<int, decimal>();
                    int combustible = 0;
                    decimal precio = 0M;

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        precio = reader.IsDBNull(1) ? 0M : reader.GetDecimal(1);

                        if (salidas.ContainsKey(combustible))
                        {
                            //salidas[combustible] += precio;
                            salidas[combustible] = precio;
                        }
                        else
                        {
                            salidas.Add(combustible, precio);
                        }
                    }

                    return salidas;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private Dictionary<int, string> obtenerCombustibles(FbCommand commConsola)
        {
            commConsola.CommandText = "Select Clave, " +
                                             "Nombre " +
                                        "From DpvgTCmb";

            using (FbDataReader reader = commConsola.ExecuteReader())
            {
                try
                {
                    Dictionary<int, string> combustibles = new Dictionary<int, string>();
                    int combustible = 0;

                    while (reader.Read())
                    {
                        combustible = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);

                        if (combustibles.ContainsKey(combustible))
                        {
                            combustibles[combustible] = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        }
                        else
                        {
                            combustibles.Add(combustible, reader.IsDBNull(1) ? string.Empty : reader.GetString(1));
                        }
                    }

                    return combustibles;
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }


        private class SaldosAjustes
        {
            public int Combustible { get; set; }

            public decimal SalidaDisponible { get; set; }

            public decimal Ajuste { get; set; }
        }
    }

    internal class VentasCombustible
    {
        public VentasCombustible()
        {
            this.Descripcion = string.Empty;
        }

        public int Combustible { get; set; }

        public decimal Precio { get; set; }

        public string Descripcion { get; set; }


        public decimal VentaReal { get; set; }

        public decimal ImporteVentaReal { get; set; }


        public decimal VentaAjustada { get; set; }

        public decimal ImporteVentaAjustada { get; set; }


        public decimal Diferencia { get; set; }

        public decimal ImporteDiferencia { get; set; }
    }
}
