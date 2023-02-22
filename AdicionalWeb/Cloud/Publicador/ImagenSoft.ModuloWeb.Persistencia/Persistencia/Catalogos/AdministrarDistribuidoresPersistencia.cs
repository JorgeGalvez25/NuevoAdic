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
    public class AdministrarDistribuidoresPersistencia
    {
        private string NombreEntidad = typeof(AdministrarDistribuidores).Name;

        public int Consecutivo(SesionModuloWeb sesion)
        {
            TaskCompletionSource<int> _task = new TaskCompletionSource<int>(sesion);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            _task.TrySetResult(this.Consecutivo(conexion, sesion) + 1);
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarDistribuidores Insertar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            TaskCompletionSource<AdministrarDistribuidores> _task = new TaskCompletionSource<AdministrarDistribuidores>(entidad);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            _task.TrySetResult(this.Insertar(conexion, sesion, entidad));
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarDistribuidores Modificar(SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            TaskCompletionSource<AdministrarDistribuidores> _task = new TaskCompletionSource<AdministrarDistribuidores>(entidad);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            _task.TrySetResult(this.Modificar(conexion, sesion, entidad));
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            _task.TrySetResult(this.Eliminar(conexion, sesion, filtro));
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public AdministrarDistribuidores Obtener(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            TaskCompletionSource<AdministrarDistribuidores> _task = new TaskCompletionSource<AdministrarDistribuidores>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            _task.TrySetResult(this.Obtener(conexion, sesion, filtro));
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        public ListaAdministrarDistribuidores ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            TaskCompletionSource<ListaAdministrarDistribuidores> _task = new TaskCompletionSource<ListaAdministrarDistribuidores>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();
                            ListaAdministrarDistribuidores aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
                            if (aux != null && aux.Count > 0)
                            {
                                resultado.AddRange(aux);
                            }
                            _task.TrySetResult(resultado);
                        }
                        catch (Exception e)
                        {
                            _task.TrySetException(e);
                        }
                    }
                }).ConfigureAwait(false);

            return _task.Task.Result;
        }

        private async Task<AdministrarDistribuidores> readerToEntidad(SqlDataReader reader)
        {
            AdministrarDistribuidores entidad = new AdministrarDistribuidores();
            entidad.Clave = await reader.IsDBNullAsync(0) ? 0 : await reader.GetFieldValueAsync<int>(0);
            entidad.Descripcion = await reader.IsDBNullAsync(1) ? string.Empty : await reader.GetFieldValueAsync<string>(1);
            entidad.EMail = await reader.IsDBNullAsync(2) ? string.Empty : await reader.GetFieldValueAsync<string>(2);
            entidad.Activo = await reader.IsDBNullAsync(3) ? string.Empty : await reader.GetFieldValueAsync<string>(3);

            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(AdministrarDistribuidores entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@ID", entidad.Clave),
                    new SqlParameter("@DESCRIPCION", entidad.Descripcion),
                    new SqlParameter("@EMAIL", entidad.EMail),
                    new SqlParameter("@ACTIVO", entidad.Activo)
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroAdministrarDistribuidores filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@ID", filtro.Clave),
                    new SqlParameter("@DESCRIPCION", filtro.Descripcion),
                    new SqlParameter("@ACTIVO", filtro.Activo)
                };
        }

        #region Transacciones
        internal int Consecutivo(Conexiones conexion, SesionModuloWeb sesion)
        {
            int resultado = 0;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Consecutivo,
                tabla = this.NombreEntidad
            };

            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    if (await reader.ReadAsync())
                    {
                        resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                    }
                    return resultado;
                }).Wait();

            return resultado + 1;
        }

        internal AdministrarDistribuidores Insertar(Conexiones conexion, SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            entidad.Clave = this.Consecutivo(conexion, sesion);

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Registrado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AC"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Result > 0) ? entidad : null;
        }

        internal AdministrarDistribuidores Modificar(Conexiones conexion, SesionModuloWeb sesion, AdministrarDistribuidores entidad)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Descripción: {0}", entidad.Descripcion).AppendLine()
                          .AppendFormat("Activo: {0}", entidad.Activo).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Modificado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AC"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Result > 0) ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            AdministrarClientesPersistencia srvCliente = new AdministrarClientesPersistencia();
            AdministrarUsuariosPersistencia srvUsuario = new AdministrarUsuariosPersistencia();
            AdministrarClientes cliente = srvCliente.Obtener(conexion, sesion, new FiltroAdministrarClientes() { Distribuidor = filtro.Clave, Version = "4.1" });
            AdministrarUsuarios usuario = srvUsuario.Obtener(conexion, sesion, new FiltroAdministrarUsuarios() { IdDistribuidor = filtro.Clave });

            if (cliente != null || usuario != null)
            {
                throw new Exception("No se puede eliminar ya que tiene referencias.");
            }

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Eliminar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            return conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Descripción: {0}", filtro.Descripcion).AppendLine()
                          .AppendFormat("Activo: {0}", filtro.Activo).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Eliminar: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AC"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Result > 0;
        }

        internal AdministrarDistribuidores Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            AdministrarDistribuidores resultado = null;
            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    if (await reader.ReadAsync())
                    {
                        resultado = await this.readerToEntidad(reader);
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }

        internal ListaAdministrarDistribuidores ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarDistribuidores filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();
            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    while (await reader.ReadAsync())
                    {
                        resultado.Add(await this.readerToEntidad(reader));
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }
        #endregion
    }
}
