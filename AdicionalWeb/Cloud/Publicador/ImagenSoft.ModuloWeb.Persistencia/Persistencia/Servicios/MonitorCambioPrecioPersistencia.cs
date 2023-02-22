using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class MonitorCambioPrecioPersistencia
    {
        private static readonly Object _lock = new Object();

        public static Dictionary<string, bool> ClientesValidos = new Dictionary<string, bool>();

        private string NombreEntidad = typeof(MonitorCambioPrecio).Name;

        public MonitorCambioPrecio Insertar(SesionModuloWeb sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecio resultado = null;

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

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
            //              .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
            //              .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
            //              .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MP"
            //                };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BitacoraException(e.Message, e)
            //            {
            //                Sesion = sesion,
            //                Bitacora = new Bitacora()
            //                    {
            //                        Estacion = sesion.NoCliente,
            //                        Error = e.Message,
            //                        Fecha = DateTime.Now,
            //                        Tipo = "MP"
            //                    }
            //            };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorCambioPrecio Modificar(SesionModuloWeb sesion, MonitorCambioPrecio entidad)
        {
            MonitorCambioPrecio resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Modificar(conexion, sesion, entidad);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        try
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //            comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //            resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //            BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //            {
            //                StringBuilder sb = new StringBuilder();
            //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                  .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //                  .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //                  .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
            //                  .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
            //                  .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
            //                  .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
            //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                  .AppendFormat("Modificado: {0}", resultado ? "Si" : "No");

            //                Bitacora bita = new Bitacora()
            //                    {
            //                        Estacion = sesion.NoCliente,
            //                        Error = sb.ToString().Trim(),
            //                        Fecha = DateTime.Now,
            //                        Tipo = "MP"
            //                    };
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
            //                        {
            //                            Estacion = sesion.NoCliente,
            //                            Error = e.Message,
            //                            Fecha = DateTime.Now,
            //                            Tipo = "MP"
            //                        }
            //                };
            //        }
            //    }).Wait();

            return resultado;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
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
            //              .AppendFormat("Precios: {0}", filtro.PrecioProgramado).AppendLine()
            //              .AppendFormat("Programado: {0}", filtro.Programado).AppendLine()
            //              .AppendFormat("Aplicado: {0}", filtro.Aplicado).AppendLine()
            //              .AppendFormat("Fecha Cliente: {0}", filtro.FechaHoraCliente).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Eliminado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "MP"
            //                };
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
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        public MonitorCambioPrecio Obtener(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecio resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Obtener(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Obtener, this.NombreEntidad);
            //        comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //        using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow)) //comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //        {
            //            try
            //            {
            //                if (await reader.ReadAsync())
            //                {
            //                    resultado = await this.readerToEntidad(reader);
            //                    //ModificarObtener(sesion, resultado);
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

        public ListaMonitorCambioPrecio ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            ListaMonitorCambioPrecio resultado = new ListaMonitorCambioPrecio();

            using (Conexiones conexion = new Conexiones())
            {
                ListaMonitorCambioPrecio aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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
            //            MonitorCambioPrecio auxiliar = null;
            //            while (await reader.ReadAsync())
            //            {
            //                auxiliar = await this.readerToEntidad(reader);
            //                //ModificarObtener(sesion, auxiliar);
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

        #region Trasacciones
        internal MonitorCambioPrecio Insertar(Conexiones conexion, SesionModuloWeb sesion, MonitorCambioPrecio entidad)
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
                                                                      .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
                                                                      .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
                                                                      .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
                                                                      .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MP"
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
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
            //              .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
            //              .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
            //              .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
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
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal MonitorCambioPrecio Modificar(Conexiones conexion, SesionModuloWeb sesion, MonitorCambioPrecio entidad)
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
                                                                      .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
                                                                      .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
                                                                      .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
                                                                      .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MP"
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

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Estación: {0}", entidad.Estacion).AppendLine()
            //              .AppendFormat("Conexión: {0}", entidad.EstatusConexion).AppendLine()
            //              .AppendFormat("Precios: {0}", entidad.PrecioProgramado).AppendLine()
            //              .AppendFormat("Programado: {0}", entidad.Programado).AppendLine()
            //              .AppendFormat("Aplicado: {0}", entidad.Aplicado).AppendLine()
            //              .AppendFormat("Fecha Cliente: {0}", entidad.FechaHoraCliente).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Modificado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
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
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
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
                                                                      .AppendFormat("Precios: {0}", filtro.PrecioProgramado).AppendLine()
                                                                      .AppendFormat("Programado: {0}", filtro.Programado).AppendLine()
                                                                      .AppendFormat("Aplicado: {0}", filtro.Aplicado).AppendLine()
                                                                      .AppendFormat("Fecha Cliente: {0}", filtro.FechaHoraCliente).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Eliminado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "MP"
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
            //              .AppendFormat("Precios: {0}", filtro.PrecioProgramado).AppendLine()
            //              .AppendFormat("Programado: {0}", filtro.Programado).AppendLine()
            //              .AppendFormat("Aplicado: {0}", filtro.Aplicado).AppendLine()
            //              .AppendFormat("Fecha Cliente: {0}", filtro.FechaHoraCliente).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Eliminado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
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
            //                Estacion = sesion.NoCliente,
            //                Error = e.Message,
            //                Fecha = DateTime.Now,
            //                Tipo = "MP"
            //            }
            //        };
            //    }
            //}).Wait();

            return resultado;
        }

        internal MonitorCambioPrecio Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            MonitorCambioPrecio resultado = null;

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

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow)) //comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //    {
            //        try
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                resultado = await this.readerToEntidad(reader);
            //                //ModificarObtener(sesion, resultado);
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

        internal ListaMonitorCambioPrecio ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            ListaMonitorCambioPrecio resultado = new ListaMonitorCambioPrecio();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         MonitorCambioPrecio auxiliar = null;
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
            //    comm.CommandText = xml.GetOperation(TipoOperacion.ObtenerTodos, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync())// comm.EndExecuteReader(comm.BeginExecuteReader()))
            //    {
            //        try
            //        {
            //            MonitorCambioPrecio auxiliar = null;
            //            while (await reader.ReadAsync())
            //            {
            //                auxiliar = await this.readerToEntidad(reader);
            //                //ModificarObtener(sesion, auxiliar);
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

        #region Transacciones Old

        //internal async Task<bool> Eliminar(SqlCommand comm, Sesion sesion, FiltroMonitorCambioPrecio filtro)
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
        //              .AppendFormat("Precios: {0}", filtro.PrecioProgramado).AppendLine()
        //              .AppendFormat("Programado: {0}", filtro.Programado).AppendLine()
        //              .AppendFormat("Aplicado: {0}", filtro.Aplicado).AppendLine()
        //              .AppendFormat("Fecha Cliente: {0}", filtro.FechaHoraCliente).AppendLine()
        //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
        //              .AppendFormat("Eliminado: {0}", resultado ? "Si" : "No");

        //            Bitacora bita = new Bitacora()
        //            {
        //                Estacion = sesion.NoCliente,
        //                Error = sb.ToString().Trim(),
        //                Fecha = DateTime.Now,
        //                Tipo = "MP"
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
        //                Estacion = sesion.NoCliente,
        //                Error = e.Message,
        //                Fecha = DateTime.Now,
        //                Tipo = "MP"
        //            }
        //        };
        //    }

        //    return resultado;
        //}

        #endregion

        public void CambiarEstatus(SesionModuloWeb sesion, FiltroMonitorCambioPrecio filtro)
        {
            using (Conexiones conexion = new Conexiones())
            {
                AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
                try
                {
                    AdministrarClientes cliente = srvCliente.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = filtro.NoEstacion });

                    if (cliente == null) { return; }
                    if (!cliente.Activo.Equals("Si", StringComparison.OrdinalIgnoreCase)) { return; }
                    if (!cliente.MonitorearCambioPrecio.Equals("Si", StringComparison.OrdinalIgnoreCase)) { return; }

                    MonitorCambioPrecio precio = this.Obtener(conexion, sesion, filtro);
                    MonitorCambioPrecio item = new MonitorCambioPrecio()
                        {
                            Aplicado = filtro.Aplicado,
                            Estacion = filtro.NoEstacion,
                            FechaHoraCliente = filtro.FechaHoraCliente,
                            EstatusConexion = filtro.EstatusConexion,
                            PrecioProgramado = filtro.PrecioProgramado,
                            FechaAplicacion = filtro.FechaAplicacion,
                            Zona = filtro.Zona,
                            Programado = filtro.Programado
                        };

                    if (precio == null)
                    {
                        item.FechaInicioSesion = DateTime.Now;
                        item.NombreComercial = sesion.Empresa.NombreComercial;
                        item.RazonSocial = sesion.Empresa.RazonSocial;

                        this.Insertar(conexion, sesion, item);
                    }
                    else
                    {
                        item.FechaHoraCliente = precio.FechaHoraCliente;
                        item.FechaInicioSesion = precio.FechaInicioSesion;
                        item.NombreComercial = precio.NombreComercial;
                        item.RazonSocial = precio.RazonSocial;

                        this.Modificar(conexion, sesion, item);
                    }
                }
                finally
                {
                    srvCliente.ModificarUltimaConexion(sesion, new FiltroAdministrarClientes()
                        {
                            FechaHoraCliente = filtro.FechaHoraCliente,
                            FechaUltimaConexion = DateTime.Now,
                            Zona = filtro.Zona,
                            NoEstacion = filtro.NoEstacion
                        });
                }
            }
        }

        private async Task<MonitorCambioPrecio> readerToEntidad(SqlDataReader reader)
        {
            MonitorCambioPrecio entidad = new MonitorCambioPrecio();
            {
                entidad.Estacion = await reader.IsDBNullAsync(0) ? string.Empty : reader.GetString(0);
                entidad.NombreComercial = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                string conexion = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
                entidad.FechaHoraCliente = await reader.IsDBNullAsync(3) ? SqlDateTime.MinValue.Value : reader.GetDateTime(3);
                entidad.FechaInicioSesion = await reader.IsDBNullAsync(4) ? SqlDateTime.MinValue.Value : reader.GetDateTime(4);
                entidad.PrecioProgramado = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
                string aplicado = await reader.IsDBNullAsync(6) ? string.Empty : reader.GetString(6);
                string zona = await reader.IsDBNullAsync(7) ? string.Empty : reader.GetString(7);
                entidad.FechaAplicacion = await reader.IsDBNullAsync(8) ? SqlDateTime.MinValue.Value : reader.GetDateTime(8);
                string programado = await reader.IsDBNullAsync(9) ? string.Empty : reader.GetString(9);
                entidad.IdDistribuidor = await reader.IsDBNullAsync(10) ? 0 : reader.GetInt32(10);
                entidad.Distribuidor = await reader.IsDBNullAsync(11) ? string.Empty : reader.GetString(11);
                entidad.Version = await reader.IsDBNullAsync(12) ? string.Empty : reader.GetString(12);

                switch (conexion.ToUpper())
                {
                    case "L":
                        entidad.EstatusConexion = EstatusConexion.EnLinea;
                        break;
                    case "F":
                        entidad.EstatusConexion = EstatusConexion.FueraDeLinea;
                        break;
                }

                switch (aplicado.ToUpper())
                {
                    case "SI":
                        entidad.Aplicado = Aplicado.Si;
                        break;
                    case "NO":
                        entidad.Aplicado = Aplicado.No;
                        break;
                    case "PD":
                        entidad.Aplicado = Aplicado.Pendiente;
                        break;
                }

                switch (zona.ToUpper())
                {
                    case "N":
                        entidad.Zona = ZonasCambioPrecio.ZonaNormal;
                        break;
                    case "F":
                        entidad.Zona = ZonasCambioPrecio.ZonaFronteriza;
                        break;
                }

                switch (programado.ToUpper())
                {
                    case "SI":
                        entidad.Programado = EstatusProgramado.Si;
                        break;
                    case "NO":
                        entidad.Programado = EstatusProgramado.No;
                        break;
                    default:
                        break;
                }

                entidad.EstatusConexion = (entidad.FechaInicioSesion.AddMinutes(15) > DateTime.Now) ? EstatusConexion.EnLinea
                                                                                                    : EstatusConexion.FueraDeLinea;

                int dias = (int)(entidad.FechaHoraCliente - entidad.FechaAplicacion).TotalDays;

                if (dias >= 0 && entidad.Aplicado != Aplicado.Si)
                {
                    entidad.DiasAtraso = dias;
                }

                if (entidad.DiasAtraso >= 1)
                {
                    entidad.Aplicado = Aplicado.No;
                }
                else if (entidad.DiasAtraso == 0 &&
                         entidad.Aplicado != Aplicado.Si &&
                         entidad.Programado == EstatusProgramado.Si)
                {
                    entidad.Aplicado = Aplicado.Pendiente;
                }
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(MonitorCambioPrecio entidad)
        {
            string zona = string.Empty;
            string aplicado = string.Empty;
            string conexion = string.Empty;

            switch (entidad.Aplicado)
            {
                case Aplicado.No:
                    aplicado = "No";
                    break;
                case Aplicado.Si:
                    aplicado = "Si";
                    break;
                case Aplicado.Pendiente:
                    aplicado = "PD";
                    break;
            }

            switch (entidad.Zona)
            {
                case ZonasCambioPrecio.ZonaFronteriza:
                    zona = "F";
                    break;
                case ZonasCambioPrecio.ZonaNormal:
                    zona = "N";
                    break;
            }

            switch (entidad.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
                default:
                    break;
            }

            string programado = "No";
            switch (entidad.Programado)
            {
                case EstatusProgramado.Si:
                    programado = "Si";
                    break;
                case EstatusProgramado.No:
                    programado = "No";
                    break;
                default:
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", entidad.Estacion),
                    new SqlParameter("@NOMBRECOMERCIAL", entidad.NombreComercial),
                    new SqlParameter("@ESTATUSCONEXION", conexion),
                    new SqlParameter("@FECHAHORACLIENTE", entidad.FechaHoraCliente <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaHoraCliente),
                    new SqlParameter("@FECHAINICIOSESION", entidad.FechaInicioSesion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaInicioSesion),
                    new SqlParameter("@PRECIOPROGRAMADO", entidad.PrecioProgramado),
                    new SqlParameter("@APLICADO", aplicado),
                    new SqlParameter("@FECHAAPLICACION", entidad.FechaAplicacion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaAplicacion),
                    new SqlParameter("@ZONA", zona),
                    new SqlParameter("@DISTRIBUIDOR", entidad.IdDistribuidor),
                    new SqlParameter("@PROGRAMADO", programado)
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroMonitorCambioPrecio filtro)
        {
            string zona = string.Empty;
            string conexion = string.Empty;
            string aplicado = string.Empty;

            switch (filtro.Aplicado)
            {
                case Aplicado.No:
                    aplicado = "No";
                    break;
                case Aplicado.Si:
                    aplicado = "Si";
                    break;
                case Aplicado.Pendiente:
                    aplicado = "PD";
                    break;
            }

            switch (filtro.EstatusConexion)
            {
                case EstatusConexion.EnLinea:
                    conexion = "L";
                    break;
                case EstatusConexion.FueraDeLinea:
                    conexion = "F";
                    break;
                default:
                    break;
            }

            switch (filtro.Zona)
            {
                case ZonasCambioPrecio.ZonaFronteriza:
                    zona = "F";
                    break;
                case ZonasCambioPrecio.ZonaNormal:
                    zona = "N";
                    break;
            }

            string programado = "No";
            switch (filtro.Programado)
            {
                case EstatusProgramado.Si:
                    programado = "Si";
                    break;
                case EstatusProgramado.No:
                    programado = "No";
                    break;
                default:
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ESTACION", filtro.NoEstacion),
                    new SqlParameter("@ESTATUSCONEXION", aplicado),
                    new SqlParameter("@CONEXION", conexion),
                    new SqlParameter("@FECHAAPLICACION", filtro.FechaAplicacion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaAplicacion),
                    new SqlParameter("@ZONA", zona),
                    new SqlParameter("@ACTIVO", "Si"),
                    new SqlParameter("@DISTRIBUIDOR", filtro.Distribuidor),
                    new SqlParameter("@PROGRAMADO", programado),
                    new SqlParameter("@CAMBIOPRECIOS", filtro.ConMonitoreo ?? string.Empty),
                };
        }
    }
}
