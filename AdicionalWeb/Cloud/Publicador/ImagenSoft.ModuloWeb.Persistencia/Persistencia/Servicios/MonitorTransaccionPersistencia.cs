using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class MonitorTransaccionPersistencia
    {
        private static readonly Object _lock = new Object();

        private string NombreEntidad = typeof(MonitorTransaccion).Name;

        public MonitorTransaccion Insertar(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccion resultado = null;
            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Insertar(conexion, sesion, entidad);
            }

            //{
            //    // Existe el cliente y esta activo?
            //    AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
            //    var cliente = srvCliente.Obtener(sesion, new FiltroAdministrarClientes() { NoEstacion = entidad.Estacion });

            //    if (cliente == null) { return null; }
            //    if (!cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase)) { return entidad; }

            //    // Existe registrado en BD?
            //    MonitorTransaccion item = this.Obtener(sesion, new FiltroMonitorTransaccion() { NoEstacion = entidad.Estacion });
            //    if (item != null) { return entidad; }
            //}

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);

            //            comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //            comm.Parameters.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            //            resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //            BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //            {
            //                StringBuilder sb = new StringBuilder();
            //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                  .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //                  .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //                  .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
            //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                  .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //                Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                };
            //                try { servicio.Insertar(sesion, bita); }
            //                catch { }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            throw new BitacoraException(e.Message, e)
            //            {
            //                Sesion = sesion,
            //                Bitacora = new Bitacora()
            //                {
            //                    EstatusTransaccion = entidad.EstatusTransaccion,
            //                    EstatusConexion = entidad.EstatusConexion,
            //                    Estacion = sesion.NoCliente,
            //                    Error = e.Message,
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                }
            //            };
            //        }
            //    }).Wait();

            return resultado;
        }

        public MonitorTransaccion Modificar(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            MonitorTransaccion resultado = null;
            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Modificar(conexion, sesion, entidad);
            }
            return resultado;
            //bool resultado = false;

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //            comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //            comm.Parameters.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            //            resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //            BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //            {
            //                StringBuilder sb = new StringBuilder();
            //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                  .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //                  .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //                  .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
            //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                  .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //                Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                };
            //                try { servicio.Insertar(sesion, bita); }
            //                catch { }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            throw new BitacoraException(e.Message, e)
            //                {
            //                    Sesion = sesion,
            //                    Bitacora = new Bitacora()
            //                    {
            //                        EstatusConexion = entidad.EstatusConexion,
            //                        EstatusTransaccion = entidad.EstatusTransaccion,
            //                        Estacion = sesion.NoCliente,
            //                        Error = e.Message,
            //                        Fecha = DateTime.Now,
            //                        Tipo = "MT"
            //                    }
            //                };
            //        }
            //    }).Wait();

            //return resultado ? entidad : null;
        }

        public void ModificarFechaProxima(SesionModuloWeb sesion)
        {
            using (Conexiones conexion = new Conexiones())
            {
                this.ModificarFechaProxima(conexion, sesion);
            }

            //try
            //{
            //    new Conexiones().ExecuteDataReader(async (comm) =>
            //            {
            //                try
            //                {
            //                    DateTime proxima = this.ObtenerFechaProxima(sesion);

            //                    comm.CommandText = xml.GetOperation(TipoOperacion.Especial_2, this.NombreEntidad);
            //                    comm.Parameters.AddRange(new SqlParameter[]
            //                        {
            //                            new SqlParameter("@ESTACION", sesion.NoCliente),
            //                            new SqlParameter("@FECHAPROXIMATRANSACCION", proxima),
            //                        });

            //                    await comm.ExecuteNonQueryAsync();//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery());
            //                }
            //                catch (Exception e)
            //                {
            //                    throw new BitacoraException(e.Message, e)
            //                        {
            //                            Sesion = sesion,
            //                            Bitacora = new Bitacora()
            //                            {
            //                                EstatusConexion = EstatusConexion.EnLinea,
            //                                Estacion = sesion.NoCliente,
            //                                Error = e.Message,
            //                                Fecha = DateTime.Now,
            //                                Tipo = "MT"
            //                            }
            //                        };
            //                }
            //            }).Wait();
            //}
            //catch
            //{ }
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            bool resultado = false;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Eliminar(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
            //            comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //            resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //            BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //            {
            //                StringBuilder sb = new StringBuilder();
            //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                  .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
            //                  .AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
            //                  .AppendFormat("Transmición: {0}", filtro.EstatusTransaccion).AppendLine()
            //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                  .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //                Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                };
            //                try { servicio.Insertar(sesion, bita); }
            //                catch { }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            throw new BitacoraException(e.Message, e)
            //            {
            //                Sesion = sesion,
            //                Bitacora = new Bitacora()
            //                {
            //                    EstatusTransaccion = filtro.EstatusTransaccion,
            //                    EstatusConexion = filtro.EstatusConexion,
            //                    Estacion = sesion.NoCliente,
            //                    Error = e.Message,
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                }
            //            };
            //        }
            //    }).Wait();

            return resultado;
        }

        public MonitorTransaccion Obtener(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccion resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Obtener(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Obtener, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //        using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))// comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //        {
            //            try
            //            {
            //                if (await reader.ReadAsync())
            //                {
            //                    resultado = await this.readerToEntidad(reader);
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed) { reader.Close(); }
            //            }
            //        }
            //    }).Wait();

            return resultado;
        }

        public ListaMonitorTransaccion ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            ListaMonitorTransaccion resultado = new ListaMonitorTransaccion();
            using (Conexiones conexion = new Conexiones())
            {
                ListaMonitorTransaccion aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                if (aux != null && aux.Count > 0)
                {
                    resultado.AddRange(aux);
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //        using (SqlDataReader reader = await comm.ExecuteReaderAsync())// comm.EndExecuteReader(comm.BeginExecuteReader()))
            //        {
            //            try
            //            {
            //                MonitorTransaccion auxiliar = null;
            //                while (await reader.ReadAsync())
            //                {
            //                    auxiliar = await this.readerToEntidad(reader);
            //                    resultado.Add(auxiliar);
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed) { reader.Close(); }
            //            }
            //        }
            //    }).Wait();

            return resultado;
        }

        #region Trasacciones
        internal MonitorTransaccion Insertar(Conexiones conexion, SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            bool resultado = false;

            AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
            AdministrarClientes cliente = srvCliente.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = entidad.Estacion });

            if (cliente == null) { return null; }
            if (!cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase)) { return entidad; }

            // Existe registrado en BD?
            MonitorTransaccion item = this.Obtener(conexion, sesion, new FiltroMonitorTransaccion() { NoEstacion = entidad.Estacion });
            if (item != null) { return entidad; }

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
                                                                      .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MT"
                                                                    };
                                                                    try { servicio.Insertar(conn, sesion, bita); }
                                                                    catch { }
                                                                }

                                                            }))
            {
                aux.Wait();
                resultado = aux.Result > 0;
            }
            //{
            //    // Existe el cliente y esta activo?
            //    AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
            //    var cliente = srvCliente.Obtener(sesion, new FiltroAdministrarClientes() { NoEstacion = entidad.Estacion });

            //    if (cliente == null) { return null; }
            //    if (!cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase)) { return entidad; }

            //    // Existe registrado en BD?
            //    MonitorTransaccion item = this.Obtener(sesion, new FiltroMonitorTransaccion() { NoEstacion = entidad.Estacion });
            //    if (item != null) { return entidad; }
            //}

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);

            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //        comm.Parameters.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
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
            //                EstatusTransaccion = entidad.EstatusTransaccion,
            //                EstatusConexion = entidad.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal MonitorTransaccion Modificar(Conexiones conexion, SesionModuloWeb sesion, MonitorTransaccion entidad)
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
                                                                      .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MT"
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
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //        comm.Parameters.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Transmición: {0}", entidad.EstatusTransaccion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
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
            //                EstatusTransaccion = entidad.EstatusTransaccion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal void ModificarFechaProxima(Conexiones conexion, SesionModuloWeb sesion)
        {
            DateTime proxima = this.ObtenerFechaProxima(sesion);

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Especial_2,
                tabla = this.NombreEntidad,
                parameters = new SqlParameter[]
                    {
                        new SqlParameter("@ESTACION", sesion.NoCliente),
                        new SqlParameter("@FECHAPROXIMATRANSACCION", proxima),
                    }
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros))
            {
                aux.Wait();
            }

            //try
            //{
            //    new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            DateTime proxima = this.ObtenerFechaProxima(sesion);

            //            comm.CommandText = xml.GetOperation(TipoOperacion.Especial_2, this.NombreEntidad);
            //            comm.Parameters.AddRange(new SqlParameter[]
            //                        {
            //                            new SqlParameter("@ESTACION", sesion.NoCliente),
            //                            new SqlParameter("@FECHAPROXIMATRANSACCION", proxima),
            //                        });

            //            await comm.ExecuteNonQueryAsync();//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery());
            //        }
            //        catch (Exception e)
            //        {
            //            throw new BitacoraException(e.Message, e)
            //            {
            //                Sesion = sesion,
            //                Bitacora = new Bitacora()
            //                {
            //                    EstatusConexion = EstatusConexion.EnLinea,
            //                    Estacion = sesion.NoCliente,
            //                    Error = e.Message,
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MT"
            //                }
            //            };
            //        }
            //    }).Wait();
            //}
            //catch
            //{ }
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
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
                                                                      .AppendFormat("Transmición: {0}", filtro.EstatusTransaccion).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MT"
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
            //              .AppendFormat("Transmición: {0}", filtro.EstatusTransaccion).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
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
            //                EstatusTransaccion = filtro.EstatusTransaccion,
            //                EstatusConexion = filtro.EstatusConexion,
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MT"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        internal MonitorTransaccion Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            MonitorTransaccion resultado = null;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         if (await reader.ReadAsync().ConfigureAwait(false))
                                                         {
                                                             resultado = await this.readerToEntidad(reader).ConfigureAwait(false);
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

        internal ListaMonitorTransaccion ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorTransaccion filtro)
        {
            ListaMonitorTransaccion resultado = new ListaMonitorTransaccion();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         MonitorTransaccion auxiliar = null;
                                                         while (await reader.ReadAsync().ConfigureAwait(false))
                                                         {
                                                             auxiliar = await this.readerToEntidad(reader).ConfigureAwait(false);
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
            //            MonitorTransaccion auxiliar = null;
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
            //}).Wait();

            return resultado;
        }
        #endregion

        #region transacciones Old
        //public async Task<bool> Eliminar(SqlCommand comm, Sesion sesion, FiltroMonitorTransaccion filtro)
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
        //              .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
        //              .AppendFormat("Conexión: {0}", filtro.EstatusConexion).AppendLine()
        //              .AppendFormat("Transmición: {0}", filtro.EstatusTransaccion).AppendLine()
        //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
        //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

        //            Bitacora bita = new Bitacora()
        //            {
        //                Estacion = sesion.NoCliente,
        //                Error = sb.ToString().Trim(),
        //                Fecha = DateTime.Now,
        //                Tipo = "MT"
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
        //                EstatusTransaccion = filtro.EstatusTransaccion,
        //                EstatusConexion = filtro.EstatusConexion,
        //                Estacion = sesion.NoCliente,
        //                Error = e.Message,
        //                Fecha = DateTime.Now,
        //                Tipo = "MT"
        //            }
        //        };
        //    }

        //    return resultado;
        //}
        #endregion

        #region Varios
        public void Transmitiendo(SesionModuloWeb sesion, MonitorTransaccion entidad)
        {
            int dias = int.Parse(ConfigurationManager.AppSettings["DiasConsulta"] ?? "7");
            if (dias <= 0) { dias = 7; }
            if (entidad.FechaCorteTransmision.Date < DateTime.Now.Date.AddDays(-1 * dias)) { return; }

            AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
            AdministrarClientes cliente = srvCliente.Obtener(sesion, new FiltroAdministrarClientes() { NoEstacion = entidad.Estacion });

            if (cliente == null) { return; }
            if (!cliente.MonitorearTransmisiones.Equals("Si", StringComparison.OrdinalIgnoreCase)) { return; }

            FiltroMonitorTransaccion filtro = new FiltroMonitorTransaccion()
            {
                NoEstacion = entidad.Estacion,
                EstatusConexion = EstatusConexion.EnLinea,
                EstatusTransaccion = entidad.EstatusTransaccion,
                FechaCorteTransmision = entidad.FechaCorteTransmision,
                FechaUltimaTransmision = entidad.FechaUltimaTransmision,
                ConMonitoreo = true
            };
            MonitorTransaccion transa = Obtener(sesion, filtro);

            string query = string.Empty;
            List<SqlParameter> parametros = new List<SqlParameter>();
            XMLLoader xml = new XMLLoader();

            if (transa != null)
            {
                transa.FechaCorteTransmision = entidad.FechaCorteTransmision;
                transa.FechaUltimaTransmision = entidad.FechaUltimaTransmision;
                transa.EstatusTransaccion = filtro.EstatusTransaccion;
                transa.Observaciones = entidad.Observaciones;

                query = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
                parametros.AddRange(this.ConfiguraParametros(transa));
            }
            else
            {
                query = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
                parametros.AddRange(this.ConfiguraParametros(entidad));
            }

            parametros.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            using (Conexiones conexion = new Conexiones())
            {
                ParametrosConexion _parametros = new ParametrosConexion()
                {
                    operacion = TipoOperacion.None,
                    tabla = this.NombreEntidad,
                    parameters = parametros,
                    query = query
                };

                using (Task<int> aux = conexion.ExecuteNonQuery(_parametros))
                {
                    aux.Wait();
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            var filtro = new FiltroMonitorTransaccion()
            //                {
            //                    NoEstacion = entidad.Estacion,
            //                    EstatusConexion = EstatusConexion.EnLinea,
            //                    EstatusTransaccion = entidad.EstatusTransaccion,
            //                    FechaCorteTransmision = entidad.FechaCorteTransmision,
            //                    FechaUltimaTransmision = entidad.FechaUltimaTransmision,
            //                    ConMonitoreo = true
            //                };
            //            var transa = Obtener(sesion, filtro);
            //            if (transa != null)
            //            {
            //                transa.FechaCorteTransmision = entidad.FechaCorteTransmision;
            //                transa.FechaUltimaTransmision = entidad.FechaUltimaTransmision;
            //                transa.EstatusTransaccion = filtro.EstatusTransaccion;
            //                transa.Observaciones = entidad.Observaciones;

            //                comm.CommandText = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
            //                comm.Parameters.AddRange(this.ConfiguraParametros(transa));
            //            }
            //            else
            //            {
            //                comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //                comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //            }
            //            comm.Parameters.Add(new SqlParameter("@FECHAPROXIMATRANSACCION", this.ObtenerFechaProxima(sesion)));

            //            await comm.ExecuteNonQueryAsync();// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery());
            //        }
            //        catch (Exception e)
            //        {
            //            throw new BitacoraException(e.Message, e)
            //                {
            //                    Sesion = sesion,
            //                    Bitacora = new Bitacora()
            //                        {
            //                            Estacion = sesion.NoCliente,
            //                            Error = e.Message,
            //                            Fecha = DateTime.Now,
            //                            Tipo = "MT"
            //                        }
            //                };
            //        }
            //    }).Wait();
        }

        private async Task<MonitorTransaccion> readerToEntidad(SqlDataReader reader)
        {
            MonitorTransaccion entidad = new MonitorTransaccion();
            {
                entidad.Estacion = await reader.IsDBNullAsync(0).ConfigureAwait(false) ? string.Empty : reader.GetString(0);
                entidad.NombreComercial = await reader.IsDBNullAsync(1).ConfigureAwait(false) ? string.Empty : reader.GetString(1);
                string conexion = await reader.IsDBNullAsync(2).ConfigureAwait(false) ? string.Empty : reader.GetString(2);
                string transaccion = await reader.IsDBNullAsync(3).ConfigureAwait(false) ? string.Empty : reader.GetString(3);
                entidad.FechaInicioSesion = await reader.IsDBNullAsync(4).ConfigureAwait(false) ? SqlDateTime.MinValue.Value : reader.GetDateTime(4);
                entidad.FechaUltimaTransmision = await reader.IsDBNullAsync(5).ConfigureAwait(false) ? SqlDateTime.MinValue.Value : reader.GetDateTime(5);
                int horas = await reader.IsDBNullAsync(6).ConfigureAwait(false) ? 4 : reader.GetInt32(6);
                entidad.IdDistribuidor = await reader.IsDBNullAsync(7).ConfigureAwait(false) ? 0 : reader.GetInt32(7);
                entidad.Distribuidor = await reader.IsDBNullAsync(8).ConfigureAwait(false) ? string.Empty : reader.GetString(8);
                entidad.FechaProximaTransmision = await reader.IsDBNullAsync(9).ConfigureAwait(false) ? SqlDateTime.MinValue.Value : reader.GetDateTime(9);
                entidad.Version = await reader.IsDBNullAsync(10).ConfigureAwait(false) ? string.Empty : reader.GetString(10);
                entidad.FechaCorteTransmision = await reader.IsDBNullAsync(11).ConfigureAwait(false) ? SqlDateTime.MinValue.Value : reader.GetDateTime(11);
                entidad.Observaciones = await reader.IsDBNullAsync(12).ConfigureAwait(false) ? string.Empty : reader.GetString(12);

                switch (transaccion)
                {
                    case "O": entidad.EstatusTransaccion = EstatusTransaccion.Ok; break;
                    case "E": entidad.EstatusTransaccion = EstatusTransaccion.Error; break;
                    case "P":
                    default: entidad.EstatusTransaccion = EstatusTransaccion.Procesando; break;
                }

                entidad.DiasAtraso = (int)(DateTime.Now.Date - entidad.FechaCorteTransmision.Date).TotalDays;

                DateTime add2Horas = entidad.FechaProximaTransmision.AddHours(2);
                if (Utilerias.FechaMexico.Between(entidad.FechaProximaTransmision, add2Horas) &&
                    entidad.EstatusTransaccion != EstatusTransaccion.Ok)
                {
                    entidad.EstatusTransaccion = EstatusTransaccion.Procesando;
                }
                else
                {
                    if (entidad.Version.Equals("4.1"))
                    {
                        horas = 24;
                    }

                    //if (entidad.FechaUltimaTransmision.Date <= DateTime.Now.Date.AddHours(-1 * horas))
                    if (entidad.FechaUltimaTransmision.Date < entidad.FechaProximaTransmision.Date.AddHours(-1 * horas))// && entidad.FechaProximaTransmision.Date >= DateTime.Now.Date.AddHours(horas))
                    {
                        entidad.EstatusTransaccion = EstatusTransaccion.Error;
                    }
                    else
                    {
                        if (entidad.FechaCorteTransmision.Date < entidad.FechaUltimaTransmision.Date.AddDays(-1))
                        {
                            entidad.EstatusTransaccion = EstatusTransaccion.Error;
                        }
                    }

                    if (entidad.EstatusTransaccion == EstatusTransaccion.Procesando)
                    {
                        entidad.EstatusTransaccion = EstatusTransaccion.Error;
                    }
                }

                entidad.EstatusConexion = (entidad.FechaInicioSesion.Date > DateTime.Now.AddDays(-1).Date)
                                                ? ((entidad.FechaInicioSesion.AddMinutes(15) > DateTime.Now)
                                                    ? EstatusConexion.EnLinea
                                                    : EstatusConexion.FueraDeLinea)
                                                : EstatusConexion.FueraDeLinea;
            }
            return entidad;
        }

        private string GetEstatusTransaccion(EstatusTransaccion estatusTransaccion)
        {
            switch (estatusTransaccion)
            {
                case EstatusTransaccion.Ok:
                    return "O";
                case EstatusTransaccion.Error:
                    return "E";
                case EstatusTransaccion.Procesando:
                    return "P";
                default:
                    break;
            }
            return string.Empty;
        }

        private string GetEstatusConexion(EstatusConexion estatusConexion)
        {
            switch (estatusConexion)
            {
                case EstatusConexion.EnLinea:
                    return "L";
                case EstatusConexion.FueraDeLinea:
                    return "F";
            }

            return string.Empty;
        }

        private SqlParameter[] ConfiguraParametros(MonitorTransaccion entidad)
        {
            string conexion = this.GetEstatusConexion(entidad.EstatusConexion);
            string transaccion = this.GetEstatusTransaccion(entidad.EstatusTransaccion);

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", entidad.Estacion),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@ESTATUSCONEXION", conexion),
                    new SqlParameter("@ESTATUSTRANSACCION", transaccion),
                    new SqlParameter("@FECHACORTE", entidad.FechaCorteTransmision <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaCorteTransmision),
                    new SqlParameter("@FECHAULTIMATRANSACCION", entidad.FechaUltimaTransmision <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaUltimaTransmision),
                    new SqlParameter("@DISTRIBUIDOR", entidad.Distribuidor),
                    new SqlParameter("@OBSERVACIONES", entidad.Observaciones)
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroMonitorTransaccion filtro)
        {
            string conexion = this.GetEstatusConexion(filtro.EstatusConexion);
            string transaccion = this.GetEstatusTransaccion(filtro.EstatusTransaccion);

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", filtro.NoEstacion),
                    new SqlParameter("@ESTATUSCONEXION", conexion),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@ESTATUSTRANSACCION", transaccion),
                    new SqlParameter("@FECHACORTE", filtro.FechaCorteTransmision <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaCorteTransmision),
                    new SqlParameter("@FECHAULTIMATRANSACCION", filtro.FechaUltimaTransmision <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaUltimaTransmision),
                    new SqlParameter("@DISTRIBUIDOR", filtro.Distribuidor),
                    new SqlParameter("@TRANSMISIONES", filtro.ConMonitoreo ? "Si" : "No")
                };
        }

        private DateTime ObtenerFechaProxima(SesionModuloWeb sesion)
        {
            return (sesion.HoraFinal.TimeOfDay < Utilerias.FechaMexico.TimeOfDay)
                        ? DateTime.Now.Date.AddDays(1).AddTicks(sesion.HoraInicial.TimeOfDay.Ticks)
                        : DateTime.Now.Date.AddTicks(sesion.HoraInicial.TimeOfDay.Ticks);
        }

        #endregion
    }
}
