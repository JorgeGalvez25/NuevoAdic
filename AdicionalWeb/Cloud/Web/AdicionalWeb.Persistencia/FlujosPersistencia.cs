using System;
using System.Data;
using Adicional.Entidades;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Enlaces;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;

namespace AdicionalWeb.Persistencia
{
    public class FlujosPersistencia
    {
        private AdicionalWeb.Persistencia.Enlaces.Conexiones _enalces;

        public FlujosPersistencia()
        {
            this._enalces = new AdicionalWeb.Persistencia.Enlaces.Conexiones();
        }

        public int FlujoInsertar(AdicionalWeb.Entidades.Flujo entidad)
        {
            this._enalces.ConsolaTransaccion((cmd) =>
                {
                    cmd.CommandText = "INSERT INTO DPVGCMND (MODULO, FECHAHORA, COMANDO, APLICADO, RESULTADO) " +
                                                   " VALUES (@MODULO, @FECHAHORA, @COMANDO, @APLICADO, @RESULTADO) " +
                                                " RETURNING FOLIO ";
                    cmd.Parameters.AddRange(this.ParametrosConsulta(entidad));
                    FbParameter outParameter = new FbParameter(@"FOLIO", FbDbType.Integer);
                    outParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParameter);
                    cmd.ExecuteNonQuery();

                }, new FiltroEstaciones() { Clave = entidad.Estacion });

            return 0;
        }

        public AdicionalWeb.Entidades.Flujo FlujoObtener(AdicionalWeb.Entidades.Flujo entidad)
        {
            AdicionalWeb.Entidades.Flujo result = null;
            this._enalces.ConsolaConsulta((cmd) =>
                {
                    cmd.CommandText = " SELECT MODULO, " +
                                             " FECHAHORA, " +
                                             " COMANDO, " +
                                             " APLICADO, " +
                                             " RESULTADO " +
                                        " FROM DPVGCMND " +
                                       " WHERE FOLIO = @FOLIO ";
                    cmd.Parameters.AddRange(this.ParametrosConsulta(entidad));

                    using (FbDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                result = new AdicionalWeb.Entidades.Flujo();
                                result.Modulo = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                result.FechaHora = reader.IsDBNull(1) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : reader.GetDateTime(1);
                                result.Comando = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                result.Aplicado = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                result.Resultado = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            }
                        }
                        finally
                        {
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                        }
                    }
                }, new FiltroEstaciones() { Clave = entidad.Estacion });

            return result;
        }

        public bool FlujoEliminar(AdicionalWeb.Entidades.Flujo entidad)
        {
            bool resultado = false;
            this._enalces.ConsolaTransaccion((cmd) =>
                {
                    cmd.CommandText = " DELETE FROM DPVGCMND " +
                                            " WHERE FOLIO = @FOLIO";
                    cmd.Parameters.AddRange(ParametrosConsulta(entidad));

                    resultado = (cmd.ExecuteNonQuery() > 0);
                }, new FiltroEstaciones() { Clave = entidad.Estacion });

            return resultado;
        }

        public string GetRespuestaComando(int estacion, int folio)
        {
            bool pSigue = true;
            string pResult = string.Empty;
            int pContador = 0;

            do
            {
                var pComando = this.FlujoObtener(new AdicionalWeb.Entidades.Flujo() { Folio = folio });

                if (string.IsNullOrEmpty(pComando.Resultado.Trim()))
                {
                    pResult = pComando.Resultado;
                    pSigue = false;
                }
                else
                {
                    pContador++;
                    if (pContador >= 20)
                    {
                        pSigue = false;
                    }
                    System.Threading.Thread.Sleep(500);
                }

            } while (pSigue == true);

            if (string.IsNullOrEmpty(pResult.Trim()))
            {
                this.FlujoEliminar(new AdicionalWeb.Entidades.Flujo() { Folio = folio });
            }

            return pResult;
        }

        private FbParameter[] ParametrosConsulta(AdicionalWeb.Entidades.Flujo filtro)
        {
            FbParameter[] parameters = new FbParameter[]
                {
                    new FbParameter("@MODULO", FbDbType.VarChar, 4) { Value = filtro.Modulo },
                    new FbParameter("@FECHAHORA", FbDbType.Date) { Value = filtro.FechaHora },
                    new FbParameter("@COMANDO", FbDbType.VarChar, 80) { Value = filtro.Comando },
                    new FbParameter("@APLICADO", FbDbType.VarChar, 2) { Value = filtro.Aplicado },
                    new FbParameter("@RESULTADO", FbDbType.VarChar, 80) { Value = filtro.Resultado }
                };

            return parameters;
        }

        public AdicionalResponse ObtenerEstadoFlujo(AdicionalWeb.Entidades.Usuario usuario, int estacion)
        {
            AdicionalResponse resultado = new AdicionalResponse();
            try
            {
                try
                {
                    Bitacora bitacora = new Bitacora()
                    {
                        Id_usuario = string.Format("{0}", usuario.Nombre),
                        Fecha = DateTime.Now.Date,
                        Hora = DateTime.Now.TimeOfDay,
                        Suceso = "Obtener Estado Flujo"
                    };
                    ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
                    Configuracion cfg = srvAdicional.ConfiguracionObtener(1, estacion);
                    resultado.Result = cfg.Estado;
                    srvAdicional.BitacoraInsertar(bitacora, estacion);
                }
                catch (Exception)
                {
                    resultado.IsFaulted = true;
                    resultado.Message = "Canal de comunicación no disponible";
                }
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        public AdicionalResponse SubirBajarFlujo(AdicionalWeb.Entidades.Usuario usuario, string std, int estacion)
        {
            AdicionalResponse resultado = new AdicionalResponse();
            try
            {
                ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();

                try
                {
                    Bitacora bitacora = new Bitacora()
                        {
                            Id_usuario = usuario == null ? "AdicionalWeb" : usuario.Nombre,
                            Fecha = DateTime.Now.Date,
                            Hora = DateTime.Now.TimeOfDay,
                            Suceso = std.Equals("Estandar", StringComparison.OrdinalIgnoreCase) ? "Subir Flujo" : "Bajar Flujo"
                        };

                    if (string.IsNullOrEmpty(std))
                    {
                        resultado.IsFaulted = true;
                        resultado.Message = "variable \"std\" es nula";
                        return resultado;
                    }

                    var est = EstacionesAdicionalPersistencia.ListaEstaciones[estacion];
                    List<Historial> pListaHistorial = new List<Historial>();
                    pListaHistorial.AddRange(srvAdicional.HistorialObtenerRecientes(estacion).ToArray());
                    resultado.Result = srvAdicional.AplicarFlujo(std.Equals("Estandar", StringComparison.OrdinalIgnoreCase), est.TipoDispensario, pListaHistorial, estacion);
                    //resultado.Result = srvAdicional.SubirBajarFlujo(std.Equals("Estandar", StringComparison.OrdinalIgnoreCase), estacion);

                    if (resultado.Result.ToString().Equals("Ok", StringComparison.OrdinalIgnoreCase))
                    {
                        resultado.Result = std;

                        srvAdicional.ConfiguracionCambiarEstado(std, estacion);
                        srvAdicional.BitacoraInsertar(bitacora, estacion);
                    }
                    else
                    {
                        resultado.IsFaulted = true;
                        resultado.Message = "No se aplicó el flujo";
                    }
                }
                catch (Exception)
                {
                    resultado.IsFaulted = true;
                    resultado.Message = "Canal de comunicación no disponible";
                }
            }
            catch (Exception e)
            {
                resultado.IsFaulted = true;
                resultado.Message = e.Message;
            }
            return resultado;
        }

        public Historial HistorialInsertar(Historial entidad, int estacion)
        {
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            return srvAdicional.HistorialInsertar(entidad, estacion);
        }

        public void BitacoraInsertar(Bitacora bitacora, int estacion)
        {
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            srvAdicional.BitacoraInsertar(bitacora, estacion);
        }

        public DateTime ConfiguracionActualizarUltimoMovimiento(DateTime fecha, int estacion)
        {
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            return srvAdicional.ConfiguracionActualizarUltimoMovimiento(fecha, estacion);
        }

        public Estacion EstacionActualizar(Estacion entidad, int estacion)
        {
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            return srvAdicional.EstacionActualizar(entidad, estacion);
        }

        public bool ConfiguracionCambiarEstado(string estado, int estacion)
        {
            ServicioProveedorAdicional srvAdicional = new ServicioProveedorAdicional();
            return srvAdicional.ConfiguracionCambiarEstado(estado, estacion);
        }

        public Adicional.Entidades.Licencia LicenciaObtener(string modulo, int estacion)
        {
            ServicioProveedorAdicional servicio = new ServicioProveedorAdicional();
            return servicio.LicenciaObtener(modulo, estacion);

        }

        public bool LicenciaValida(Adicional.Entidades.Licencia licencia, string version, int estacion)
        {
            ServicioProveedorAdicional servicio = new ServicioProveedorAdicional();
            return servicio.LicenciaValida(licencia, version, estacion);
        }
    }
}
