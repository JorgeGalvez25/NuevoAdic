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
    public class MonitorAplicacionesPersistencia
    {
        private static readonly Object _lock = new Object();

        private string NombreEntidad = typeof(MonitorAplicaciones).Name;

        public MonitorAplicaciones Insertar(SesionModuloWeb sesion, MonitorAplicaciones entidad)
        {
            MonitorAplicaciones resultado = null;
            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Insertar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    try
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //        comm.Parameters.Clear();
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        bool auxResultado = false;

            //        entidad.Detalle.ForEach((p) =>
            //            {
            //                using (Task<bool> item = srvDetalle.InsertarMonitorAplicacionDetalle(sesion, comm, p))
            //                {
            //                    item.Wait();
            //                    auxResultado = item.Result;
            //                }
            //            });

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorAplicaciones Modificar(SesionModuloWeb sesion, MonitorAplicaciones entidad)
        {
            MonitorAplicaciones resultado = null;
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

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        bool auxResultado = false;

            //        entidad.Detalle.ForEach(p =>
            //            {
            //                using (Task<bool> item = srvDetalle.ModificarMonitorAplicacionDetalle(sesion, comm, p))
            //                {
            //                    item.Wait();
            //                    auxResultado = item.Result;
            //                }
            //            });

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
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

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        FiltroMonitorAplicacionesDetalle fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //        {
            //            fltDetalle.Estacion = filtro.NoEstacion;
            //        }

            //        bool auxResultado = false;
            //        using (Task<bool> item = srvDetalle.EliminarMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //        {
            //            item.Wait();
            //            auxResultado = item.Result;
            //        }

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorAplicaciones Obtener(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            MonitorAplicaciones resultado = null;

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
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }

            //    MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //    {
            //        //MonitorAplicacionesDetalle auxMax = null;
            //        FiltroMonitorAplicacionesDetalle fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //        {
            //            fltDetalle.IdCliente = resultado.IdCliente;
            //            fltDetalle.Estacion = resultado.Estacion;
            //        }

            //        ListaMonitorAplicacionesDetalle aux = new ListaMonitorAplicacionesDetalle();
            //        using (Task<ListaMonitorAplicacionesDetalle> item = srvDetalle.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //        {
            //            item.Wait();
            //            aux.AddRange(item.Result);
            //        }

            //        resultado.Detalle = aux;
            //        MonitorAplicacionesDetalle auxMax = aux.ObtenerMayorConsumo();
            //        resultado.EstatusAlerta = this.CalcularEstatusAlertas(resultado.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
            //    }
            //}).Wait();

            return resultado;
        }

        public ListaMonitorAplicaciones ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            ListaMonitorAplicaciones resultado = new ListaMonitorAplicaciones();

            using (Conexiones conexion = new Conexiones())
            {
                ListaMonitorAplicaciones aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                if (aux != null && aux.Count > 0)
                {
                    resultado.AddRange(aux);
                }
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())//comm.EndExecuteReader(comm.BeginExecuteReader()))
            //    {
            //        try
            //        {
            //            MonitorAplicaciones auxiliar = null;
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

            //    MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //    {
            //        MonitorAplicacionesDetalle auxMax = null;
            //        ListaMonitorAplicacionesDetalle aux = null;
            //        FiltroMonitorAplicacionesDetalle fltDetalle = null;

            //        resultado.ForEach(p =>
            //            {

            //                fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //                {
            //                    fltDetalle.IdCliente = p.IdCliente;
            //                    fltDetalle.Estacion = p.Estacion;
            //                }
            //                aux = new ListaMonitorAplicacionesDetalle();
            //                using (Task<ListaMonitorAplicacionesDetalle> item = srvDetalle.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //                {
            //                    item.Wait();
            //                    aux.AddRange(item.Result);
            //                }
            //                p.Detalle = aux;
            //                auxMax = aux.ObtenerMayorConsumo();
            //                p.EstatusAlerta = this.CalcularEstatusAlertas(p.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
            //            });
            //    }
            //}).Wait();

            return resultado;
        }

        #region Transacciones
        internal MonitorAplicaciones Insertar(Conexiones conexion, SesionModuloWeb sesion, MonitorAplicaciones entidad)
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
                                                                MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
                                                                bool auxResultado = false;

                                                                entidad.Detalle.ForEach((p) =>
                                                                {
                                                                    auxResultado = srvDetalle.Insertar(conn, sesion, p) != null;
                                                                });

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
                                                                        Tipo = "MA"
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
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //        comm.Parameters.Clear();
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        bool auxResultado = false;

            //        entidad.Detalle.ForEach((p) =>
            //        {
            //            using (Task<bool> item = srvDetalle.InsertarMonitorAplicacionDetalle(sesion, comm, p))
            //            {
            //                item.Wait();
            //                auxResultado = item.Result;
            //            }
            //        });

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal MonitorAplicaciones Modificar(Conexiones conexion, SesionModuloWeb sesion, MonitorAplicaciones entidad)
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
                                                                MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
                                                                bool auxResultado = false;

                                                                entidad.Detalle.ForEach((p) =>
                                                                {
                                                                    auxResultado = srvDetalle.Modificar(conn, sesion, p) != null;
                                                                });

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
                                                                        Tipo = "MA"
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

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        bool auxResultado = false;

            //        entidad.Detalle.ForEach(p =>
            //        {
            //            using (Task<bool> item = srvDetalle.ModificarMonitorAplicacionDetalle(sesion, comm, p))
            //            {
            //                item.Wait();
            //                auxResultado = item.Result;
            //            }
            //        });

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
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
                                                                MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
                                                                bool auxResultado = false;

                                                                auxResultado = srvDetalle.Eliminar(conn, sesion, new FiltroMonitorAplicacionesDetalle()
                                                                    {
                                                                        Estacion = filtro.NoEstacion
                                                                    });

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
                                                                        Tipo = "MA"
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

            //        MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //        FiltroMonitorAplicacionesDetalle fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //        {
            //            fltDetalle.Estacion = filtro.NoEstacion;
            //        }

            //        bool auxResultado = false;
            //        using (Task<bool> item = srvDetalle.EliminarMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //        {
            //            item.Wait();
            //            auxResultado = item.Result;
            //        }

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
            //                Tipo = "MA"
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
            //                Tipo = "MA"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        internal MonitorAplicaciones Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            MonitorAplicaciones resultado = null;
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

                                                         MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
                                                         {
                                                             FiltroMonitorAplicacionesDetalle fltDetalle = new FiltroMonitorAplicacionesDetalle();
                                                             {
                                                                 fltDetalle.IdCliente = resultado.IdCliente;
                                                                 fltDetalle.Estacion = resultado.Estacion;
                                                             }

                                                             MonitorAplicacionesDetalle auxMax = null;
                                                             ListaMonitorAplicacionesDetalle lAux = srvDetalle.ObtenerTodosFiltro(conn, sesion, fltDetalle);

                                                             if (lAux != null && lAux.Count > 0)
                                                             {
                                                                 resultado.Detalle = lAux;
                                                                 auxMax = lAux.ObtenerMayorConsumo();
                                                             }

                                                             resultado.EstatusAlerta = this.CalcularEstatusAlertas(resultado.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
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

            //    MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //    {
            //        //MonitorAplicacionesDetalle auxMax = null;
            //        FiltroMonitorAplicacionesDetalle fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //        {
            //            fltDetalle.IdCliente = resultado.IdCliente;
            //            fltDetalle.Estacion = resultado.Estacion;
            //        }

            //        ListaMonitorAplicacionesDetalle aux = new ListaMonitorAplicacionesDetalle();
            //        using (Task<ListaMonitorAplicacionesDetalle> item = srvDetalle.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //        {
            //            item.Wait();
            //            aux.AddRange(item.Result);
            //        }

            //        resultado.Detalle = aux;
            //        MonitorAplicacionesDetalle auxMax = aux.ObtenerMayorConsumo();
            //        resultado.EstatusAlerta = this.CalcularEstatusAlertas(resultado.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
            //    }
            //}).Wait();

            return resultado;
        }

        internal ListaMonitorAplicaciones ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorAplicaciones filtro)
        {
            ListaMonitorAplicaciones resultado = new ListaMonitorAplicaciones();
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         MonitorAplicaciones auxiliar = null;
                                                         while (await reader.ReadAsync())
                                                         {
                                                             auxiliar = await this.readerToEntidad(reader);
                                                             resultado.Add(auxiliar);
                                                         }

                                                         MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
                                                         {
                                                             MonitorAplicacionesDetalle auxMax = null;
                                                             ListaMonitorAplicacionesDetalle laux = null;
                                                             FiltroMonitorAplicacionesDetalle fltDetalle = null;

                                                             resultado.ForEach(p =>
                                                             {
                                                                 fltDetalle = new FiltroMonitorAplicacionesDetalle();
                                                                 {
                                                                     fltDetalle.IdCliente = p.IdCliente;
                                                                     fltDetalle.Estacion = p.Estacion;
                                                                 }

                                                                 laux = srvDetalle.ObtenerTodosFiltro(conn, sesion, fltDetalle);

                                                                 if (laux != null && laux.Count > 0)
                                                                 {
                                                                     p.Detalle = laux;
                                                                     auxMax = laux.ObtenerMayorConsumo();
                                                                 }

                                                                 p.EstatusAlerta = this.CalcularEstatusAlertas(p.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
                                                             });
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

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())//comm.EndExecuteReader(comm.BeginExecuteReader()))
            //    {
            //        try
            //        {
            //            MonitorAplicaciones auxiliar = null;
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

            //    MonitorAplicacionesDetallePersistencia srvDetalle = new MonitorAplicacionesDetallePersistencia();
            //    {
            //        MonitorAplicacionesDetalle auxMax = null;
            //        ListaMonitorAplicacionesDetalle aux = null;
            //        FiltroMonitorAplicacionesDetalle fltDetalle = null;

            //        resultado.ForEach(p =>
            //        {

            //            fltDetalle = new FiltroMonitorAplicacionesDetalle();
            //            {
            //                fltDetalle.IdCliente = p.IdCliente;
            //                fltDetalle.Estacion = p.Estacion;
            //            }
            //            aux = new ListaMonitorAplicacionesDetalle();
            //            using (Task<ListaMonitorAplicacionesDetalle> item = srvDetalle.ObtenerTodosMonitorAplicacionDetalle(sesion, comm, fltDetalle))
            //            {
            //                item.Wait();
            //                aux.AddRange(item.Result);
            //            }
            //            p.Detalle = aux;
            //            auxMax = aux.ObtenerMayorConsumo();
            //            p.EstatusAlerta = this.CalcularEstatusAlertas(p.MemoriaTotal, auxMax == null ? 0 : auxMax.MemoriaUsada);
            //        });
            //    }
            //}).Wait();

            return resultado;
        }
        #endregion

        public bool ModificarInsertar(SesionModuloWeb sesion, MonitorAplicaciones entidad)
        {
            using (Conexiones conexion = new Conexiones())
            {
                MonitorAplicaciones resp = this.Obtener(conexion, sesion, new FiltroMonitorAplicaciones()
                    {
                        IdCliente = entidad.IdCliente,
                        NoEstacion = entidad.Estacion
                    });
                return ((resp == null) ? this.Insertar(conexion, sesion, entidad)
                                       : this.Modificar(conexion, sesion, entidad)) != null;
            }
        }

        private async Task<MonitorAplicaciones> readerToEntidad(SqlDataReader reader)
        {
            MonitorAplicaciones entidad = new MonitorAplicaciones();
            {
                entidad.IdCliente = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                entidad.Estacion = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                entidad.NombreComercial = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
                string conexion = await reader.IsDBNullAsync(3) ? string.Empty : reader.GetString(3);
                entidad.FechaInicioSesion = await reader.IsDBNullAsync(4) ? SqlDateTime.MinValue.Value : reader.GetDateTime(4);
                entidad.IdDistribuidor = await reader.IsDBNullAsync(5) ? 0 : reader.GetInt32(5);
                entidad.Distribuidor = await reader.IsDBNullAsync(6) ? string.Empty : reader.GetString(6);
                entidad.Version = await reader.IsDBNullAsync(7) ? string.Empty : reader.GetString(7);
                entidad.SistemaOperativo = await reader.IsDBNullAsync(8) ? string.Empty : reader.GetString(8);
                entidad.MemoriaTotal = await reader.IsDBNullAsync(9) ? 0M : reader.GetDecimal(9);
                entidad.MemoriaDisponible = await reader.IsDBNullAsync(10) ? 0M : reader.GetDecimal(10);

                entidad.EstatusConexion = (entidad.FechaInicioSesion.Date > DateTime.Now.AddDays(-1).Date)
                                                ? ((entidad.FechaInicioSesion.AddMinutes(15) > DateTime.Now)
                                                    ? EstatusConexion.EnLinea
                                                    : EstatusConexion.FueraDeLinea)
                                                : EstatusConexion.FueraDeLinea;
            }
            return entidad;
        }

        private EstatusAplicacionAlertas CalcularEstatusAlertas(decimal total, decimal variable)
        {
            if (variable == 0M) { return EstatusAplicacionAlertas.Ok; }
            decimal porcentaje = (total / variable) * 100;

            if (porcentaje >= 70)
            {
                return EstatusAplicacionAlertas.Error;
            }
            else if (porcentaje >= 25)
            {
                return EstatusAplicacionAlertas.Precaucion;
            }

            return EstatusAplicacionAlertas.Ok;
        }

        private SqlParameter[] ConfiguraParametros(MonitorAplicaciones entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@IDCLIENTE", entidad.IdCliente),
                    new SqlParameter("@NOESTACION", entidad.Estacion),
                    new SqlParameter("@SISTEMAOPERATIVO", entidad.SistemaOperativo),
                    new SqlParameter("@MEMORIAUTILIZADA", entidad.MemoriaTotal),
                    new SqlParameter("@MEMORIADISPONIBLE", entidad.MemoriaDisponible),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@DISTRIBUIDOR", entidad.IdDistribuidor),
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroMonitorAplicaciones filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@IDCLIENTE", filtro.IdCliente),
                    new SqlParameter("@NOESTACION", filtro.NoEstacion),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@DISTRIBUIDOR", filtro.Distribuidor),
                };
        }
    }
}
