using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class MonitorAplicacionesDetallePersistencia
    {
        private static readonly Object _lock = new Object();

        private string NombreEntidad = typeof(MonitorAplicacionesDetalle).Name;

        public MonitorAplicacionesDetalle Insertar(SesionModuloWeb sesion, MonitorAplicacionesDetalle entidad)
        {
            MonitorAplicacionesDetalle resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Insertar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        resultado = await this.InsertarMonitorAplicacionDetalle(sesion, comm, entidad);
            //    }).Wait();

            return resultado;
        }

        public MonitorAplicacionesDetalle Modificar(SesionModuloWeb sesion, MonitorAplicacionesDetalle entidad)
        {
            MonitorAplicacionesDetalle resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Modificar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        resultado = await this.ModificarMonitorAplicacionDetalle(sesion, comm, entidad);
            //    }).Wait();

            return resultado;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
        {
            bool resultado = false;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Eliminar(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        resultado = await this.EliminarMonitorAplicacionDetalle(sesion, comm, filtro);
            //    }).Wait();

            return resultado;
        }

        public MonitorAplicacionesDetalle Obtener(SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
        {
            MonitorAplicacionesDetalle resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Obtener(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    resultado = await this.ObtenerMonitorAplicacionDetalle(sesion, comm, filtro);
            //}).Wait();

            return resultado;
        }

        public ListaMonitorAplicacionesDetalle ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
        {
            ListaMonitorAplicacionesDetalle resultado = new ListaMonitorAplicacionesDetalle();

            using (Conexiones conexion = new Conexiones())
            {
                ListaMonitorAplicacionesDetalle aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                if (aux != null && aux.Count > 0)
                {
                    resultado.AddRange(aux);
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        resultado = await this.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, filtro);
            //    }).Wait();

            return resultado;
        }

        #region Transacciones

        internal MonitorAplicacionesDetalle Insertar(Conexiones conexion, SesionModuloWeb sesion, MonitorAplicacionesDetalle entidad)
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
                                                                        //.AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MD"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
                resultado = aux.Result > 0;
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    resultado = await this.InsertarMonitorAplicacionDetalle(sesion, comm, entidad);
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal MonitorAplicacionesDetalle Modificar(Conexiones conexion, SesionModuloWeb sesion, MonitorAplicacionesDetalle entidad)
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
                                                                        //.AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MD"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
                resultado = aux.Result > 0;
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    resultado = await this.ModificarMonitorAplicacionDetalle(sesion, comm, entidad);
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
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
                                                                      .AppendFormat("Estación: {0}", filtro.Estacion).AppendLine()
                                                                        //.AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MD"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }
                                                            }))
            {
                aux.Wait();
                resultado = aux.Result > 0;
            }
            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    resultado = await this.EliminarMonitorAplicacionDetalle(sesion, comm, filtro);
            //}).Wait();

            return resultado;
        }

        internal MonitorAplicacionesDetalle Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
        {
            MonitorAplicacionesDetalle resultado = null;

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
            //    resultado = await this.ObtenerMonitorAplicacionDetalle(sesion, comm, filtro);
            //}).Wait();

            return resultado;
        }

        internal ListaMonitorAplicacionesDetalle ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicacionesDetalle filtro)
        {
            ListaMonitorAplicacionesDetalle resultado = new ListaMonitorAplicacionesDetalle();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         MonitorAplicacionesDetalle auxiliar = null;
                                                         while (await reader.ReadAsync())
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
            //    resultado = await this.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, filtro);
            //}).Wait();

            return resultado;
        }
        #endregion

        private async Task<MonitorAplicacionesDetalle> readerToEntidad(SqlDataReader reader)
        {
            MonitorAplicacionesDetalle entidad = new MonitorAplicacionesDetalle();
            {
                entidad.IdCliente = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                entidad.Estacion = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                entidad.Indice = await reader.IsDBNullAsync(2) ? 0 : reader.GetInt32(2);
                entidad.Servicio = await reader.IsDBNullAsync(3) ? string.Empty : reader.GetString(3);
                entidad.MemoriaUsada = await reader.IsDBNullAsync(4) ? 0M : reader.GetDecimal(4);
                entidad.Observaciones = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(MonitorAplicacionesDetalle entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@IDCLIENTE", entidad.IdCliente),
                    new SqlParameter("@NOESTACION", entidad.Estacion),
                    new SqlParameter("@NOMBRESERVICIO", entidad.Servicio),
                    new SqlParameter("@MEMORIAUSADA", entidad.MemoriaUsada),
                    new SqlParameter("@OBSERVACIONES", entidad.Observaciones),
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroMonitorAplicacionesDetalle filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@IDCLIENTE", filtro.IdCliente),
                    new SqlParameter("@NOESTACION", filtro.Estacion),
                    new SqlParameter("@INDICE", filtro.Indice),
                };
        }

        //public async Task<bool> InsertarMonitorAplicacionDetalle(Sesion sesion, SqlCommand comm, MonitorAplicacionesDetalle entidad)
        //{
        //    bool resultado = false;
        //    try
        //    {
        //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
        //        comm.Parameters.Clear();
        //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

        //        resultado = await comm.ExecuteNonQueryAsync() > 0;//EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

        //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
        //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
        //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
        //                //.AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
        //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
        //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

        //            Bitacora bita = new Bitacora()
        //            {
        //                Estacion = sesion.NoCliente,
        //                Error = sb.ToString().Trim(),
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            };
        //            try { servicio.Insertar(sesion, bita); }
        //            catch { }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = false;
        //        throw new BitacoraException(e.Message, e)
        //        {
        //            Sesion = sesion,
        //            Bitacora = new Bitacora()
        //            {
        //                //EstatusConexion = entidad.EstatusConexion,
        //                Estacion = sesion.NoCliente,
        //                Error = e.Message,
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            }
        //        };
        //    }

        //    return resultado;
        //}

        //public async Task<bool> ModificarMonitorAplicacionDetalle(Sesion sesion, SqlCommand comm, MonitorAplicacionesDetalle entidad)
        //{
        //    bool resultado = false;
        //    try
        //    {
        //        comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
        //        comm.Parameters.Clear();
        //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

        //        resultado = await comm.ExecuteNonQueryAsync() > 0; //comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

        //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
        //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
        //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
        //                //.AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
        //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
        //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

        //            Bitacora bita = new Bitacora()
        //            {
        //                Estacion = sesion.NoCliente,
        //                Error = sb.ToString().Trim(),
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            };
        //            try { servicio.Insertar(sesion, bita); }
        //            catch { }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = false;
        //        throw new BitacoraException(e.Message, e)
        //        {
        //            Sesion = sesion,
        //            Bitacora = new Bitacora()
        //            {
        //                //EstatusConexion = entidad.EstatusConexion,
        //                Estacion = sesion.NoCliente,
        //                Error = e.Message,
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            }
        //        };
        //    }

        //    return resultado;
        //}

        //public async Task<bool> EliminarMonitorAplicacionDetalle(Sesion sesion, SqlCommand comm, FiltroMonitorAplicacionesDetalle filtro)
        //{
        //    bool resultado = false;
        //    try
        //    {
        //        comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
        //        comm.Parameters.Clear();
        //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

        //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

        //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
        //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
        //              .AppendFormat("Estación: {0}", filtro.Estacion).AppendLine()
        //                //.AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
        //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
        //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

        //            Bitacora bita = new Bitacora()
        //            {
        //                Estacion = sesion.NoCliente,
        //                Error = sb.ToString().Trim(),
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            };
        //            try { servicio.Insertar(sesion, bita); }
        //            catch { }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = false;
        //        throw new BitacoraException(e.Message, e)
        //        {
        //            Sesion = sesion,
        //            Bitacora = new Bitacora()
        //            {
        //                //EstatusConexion = filtro.EstatusConexion,
        //                Estacion = sesion.NoCliente,
        //                Error = e.Message,
        //                Fecha = DateTime.Now,
        //                Tipo = "MD"
        //            }
        //        };
        //    }

        //    return resultado;
        //}

        //public async Task<MonitorAplicacionesDetalle> ObtenerMonitorAplicacionDetalle(Sesion sesion, SqlCommand comm, FiltroMonitorAplicacionesDetalle filtro)
        //{
        //    MonitorAplicacionesDetalle resultado = null;
        //    comm.CommandText = xml.GetOperation(TipoOperacion.Obtener, this.NombreEntidad);
        //    comm.Parameters.Clear();
        //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

        //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))//EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
        //    {
        //        try
        //        {
        //            if (await reader.ReadAsync())
        //            {
        //                resultado = await this.readerToEntidad(reader);
        //            }
        //        }
        //        finally
        //        {
        //            if (!reader.IsClosed) { reader.Close(); }
        //        }
        //    }

        //    return resultado;
        //}

        //public async Task<ListaMonitorAplicacionesDetalle> ObtenerTodosMonitorAplicacionDetalle(Sesion sesion, SqlCommand comm, FiltroMonitorAplicacionesDetalle filtro)
        //{
        //    ListaMonitorAplicacionesDetalle resultado = new ListaMonitorAplicacionesDetalle();
        //    comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
        //    comm.Parameters.Clear();
        //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

        //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())//EndExecuteReader(comm.BeginExecuteReader()))
        //    {
        //        try
        //        {
        //            MonitorAplicacionesDetalle auxiliar = null;
        //            while (await reader.ReadAsync())
        //            {
        //                auxiliar = await this.readerToEntidad(reader);
        //                resultado.Add(auxiliar);
        //            }
        //        }
        //        finally
        //        {
        //            if (!reader.IsClosed) { reader.Close(); }
        //        }
        //    }

        //    return resultado;
        //}
    }
}
