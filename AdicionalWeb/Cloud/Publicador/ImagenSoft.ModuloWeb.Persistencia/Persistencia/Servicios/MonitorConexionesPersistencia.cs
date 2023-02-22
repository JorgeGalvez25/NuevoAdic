using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class MonitorConexionesPersistencia
    {
        private static readonly Object _lock = new Object();

        private string NombreEntidad = typeof(MonitorConexiones).Name;

        public MonitorConexiones Insertar(SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            MonitorConexiones resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Insertar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = entidad.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorConexiones Modificar(SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            MonitorConexiones resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Modificar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = entidad.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            bool resultado = false;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Eliminar(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = filtro.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorConexiones Obtener(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexiones resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Obtener(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Obtener, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))// comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //    {
            //        try
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                resultado = await this.readerToEntidad(reader);

            //                //if (resultado.FechaInicioSesion.Date <= DateTime.Now.AddDays(-1).Date)
            //                //{
            //                //    resultado.EstatusConexion = EstatusConexion.FueraDeLinea;
            //                //    //this.Modificar(sesion, resultado);
            //                //}
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado;
        }

        public ListaMonitorConexiones ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            ListaMonitorConexiones resultado = new ListaMonitorConexiones();

            using (Conexiones conexion = new Conexiones())
            {
                ListaMonitorConexiones aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                if (aux != null && aux.Count > 0)
                {
                    resultado.AddRange(aux);
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())// comm.EndExecuteReader(comm.BeginExecuteReader()))
            //    {
            //        try
            //        {
            //            MonitorConexiones auxiliar = null;
            //            while (reader.Read())
            //            {
            //                auxiliar = await this.readerToEntidad(reader);
            //                //this.Modificar(sesion, auxiliar);
            //                resultado.Add(auxiliar);
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado;
        }

        #region Transacciones
        internal MonitorConexiones Insertar(Conexiones conexion, SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            bool resultado = false;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros, (conn) =>
                                                            {
                                                                BitacoraPersistencia servicio = new BitacoraPersistencia();
                                                                {
                                                                    StringBuilder sb = new StringBuilder();
                                                                    sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                      .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                      .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
                                                                      .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MC"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = entidad.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal MonitorConexiones Modificar(Conexiones conexion, SesionModuloWeb sesion, MonitorConexiones entidad)
        {
            bool resultado = false;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros, (conn) =>
                                                            {
                                                                BitacoraPersistencia servicio = new BitacoraPersistencia();
                                                                {
                                                                    StringBuilder sb = new StringBuilder();
                                                                    sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                      .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                      .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
                                                                      .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MC"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = entidad.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            bool resultado = false;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Eliminar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros, (conn) =>
                                                            {
                                                                BitacoraPersistencia servicio = new BitacoraPersistencia();
                                                                {
                                                                    StringBuilder sb = new StringBuilder();
                                                                    sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                      .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                      .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
                                                                      .AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MC"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //        {
            //            Sesion = sesion,
            //            Bitacora = new Bitacora()
            //            {
            //                EstatusConexion = filtro.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MC"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        internal MonitorConexiones Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            MonitorConexiones resultado = null;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         if (await reader.ReadAsync())
                                                         {
                                                             resultado = await this.readerToEntidad(reader);
                                                         }
                                                         return resultado;
                                                     }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Obtener, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))// comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //    {
            //        try
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                resultado = await this.readerToEntidad(reader);

            //                //if (resultado.FechaInicioSesion.Date <= DateTime.Now.AddDays(-1).Date)
            //                //{
            //                //    resultado.EstatusConexion = EstatusConexion.FueraDeLinea;
            //                //    //this.Modificar(sesion, resultado);
            //                //}
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado;
        }

        internal ListaMonitorConexiones ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorConexiones filtro)
        {
            ListaMonitorConexiones resultado = new ListaMonitorConexiones();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         MonitorConexiones auxiliar = null;
                                                         while (reader.Read())
                                                         {
                                                             auxiliar = await this.readerToEntidad(reader);
                                                             resultado.Add(auxiliar);
                                                         }
                                                         return resultado;
                                                     }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())// comm.EndExecuteReader(comm.BeginExecuteReader()))
            //    {
            //        try
            //        {
            //            MonitorConexiones auxiliar = null;
            //            while (reader.Read())
            //            {
            //                auxiliar = await this.readerToEntidad(reader);
            //                //this.Modificar(sesion, auxiliar);
            //                resultado.Add(auxiliar);
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado;
        }
        #endregion

        private async Task<MonitorConexiones> readerToEntidad(SqlDataReader reader)
        {
            MonitorConexiones entidad = new MonitorConexiones();
            {
                entidad.Estacion = await reader.IsDBNullAsync(0) ? string.Empty : reader.GetString(0);
                entidad.NombreComercial = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                string conexion = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
                entidad.FechaInicioSesion = await reader.IsDBNullAsync(3) ? SqlDateTime.MinValue.Value : reader.GetDateTime(3);
                entidad.IdDistribuidor = await reader.IsDBNullAsync(4) ? 0 : reader.GetInt32(4);
                entidad.Distribuidor = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
                entidad.Version = await reader.IsDBNullAsync(6) ? string.Empty : reader.GetString(6);

                entidad.EstatusConexion = (entidad.FechaInicioSesion.Date > DateTime.Now.AddDays(-1).Date)
                                                ? ((entidad.FechaInicioSesion.AddMinutes(15) > DateTime.Now)
                                                    ? EstatusConexion.EnLinea
                                                    : EstatusConexion.FueraDeLinea)
                                                : EstatusConexion.FueraDeLinea;

                entidad.DiasAtraso = (int)(DateTime.Now.Date - entidad.FechaInicioSesion.Date).TotalDays;
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(MonitorConexiones entidad)
        {
            string conexion = string.Empty;

            switch (entidad.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", entidad.Estacion),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@ESTATUSCONEXION", conexion),
                    new SqlParameter("@DISTRIBUIDOR", entidad.Distribuidor),
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroMonitorConexiones filtro)
        {
            string conexion = string.Empty;

            switch (filtro.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", filtro.NoEstacion),
                    new SqlParameter("@ESTATUSCONEXION", conexion),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@DISTRIBUIDOR", filtro.Distribuidor),
                };
        }
    }
}
