using ImagenSoft.ModuloWeb.Entidades;
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
    public class AdministrarUsuariosPersistencia
    {
        private string NombreEntidad = typeof(AdministrarUsuarios).Name;

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

        public AdministrarUsuarios Insertar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            TaskCompletionSource<AdministrarUsuarios> _task = new TaskCompletionSource<AdministrarUsuarios>(entidad);
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

        public AdministrarUsuarios Modificar(SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            TaskCompletionSource<AdministrarUsuarios> _task = new TaskCompletionSource<AdministrarUsuarios>(entidad);
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

        public bool Eliminar(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
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

        public AdministrarUsuarios Obtener(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            TaskCompletionSource<AdministrarUsuarios> _task = new TaskCompletionSource<AdministrarUsuarios>(filtro);
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

        public ListaAdministrarUsuarios ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            TaskCompletionSource<ListaAdministrarUsuarios> _task = new TaskCompletionSource<ListaAdministrarUsuarios>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();
                        ListaAdministrarUsuarios aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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

        private async Task<AdministrarUsuarios> readerToEntidad(SqlDataReader reader)
        {
            AdministrarUsuarios entidad = new AdministrarUsuarios();
            Task<string> tskEnlaces = reader.GetFieldValueAsync<string>(10);
            tskEnlaces.ConfigureAwait(false);
            entidad.Clave = await reader.IsDBNullAsync(0) ? 0 : await reader.GetFieldValueAsync<int>(0);
            entidad.Nombre = await reader.IsDBNullAsync(1) ? string.Empty : await reader.GetFieldValueAsync<string>(1);
            entidad.Contrasena = await reader.IsDBNullAsync(2) ? string.Empty : await reader.GetFieldValueAsync<string>(2);
            entidad.Puesto = await reader.IsDBNullAsync(3) ? string.Empty : await reader.GetFieldValueAsync<string>(3);
            entidad.Email = await reader.IsDBNullAsync(4) ? string.Empty : await reader.GetFieldValueAsync<string>(4);
            entidad.Fecha = await reader.IsDBNullAsync(5) ? SqlDateTime.MinValue.Value : await reader.GetFieldValueAsync<DateTime>(5);
            entidad.UltimoCambio = await reader.IsDBNullAsync(6) ? SqlDateTime.MinValue.Value : await reader.GetFieldValueAsync<DateTime>(6);
            entidad.Activo = await reader.IsDBNullAsync(7) ? string.Empty : await reader.GetFieldValueAsync<string>(7);
            entidad.IdDistribuidor = await reader.IsDBNullAsync(8) ? 0 : await reader.GetFieldValueAsync<int>(8);
            entidad.Distribuidor = await reader.IsDBNullAsync(9) ? string.Empty : await reader.GetFieldValueAsync<string>(9);
            string enlaces = await tskEnlaces;

            if (!string.IsNullOrEmpty(enlaces))
            {
                entidad.Permisos.FromXML(enlaces);
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(AdministrarUsuarios entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@CLAVE", entidad.Clave),
                    new SqlParameter("@NOMBRE", entidad.Nombre),
                    new SqlParameter("@CONTRASENA", entidad.Contrasena),
                    new SqlParameter("@EMAIL", entidad.Email),
                    new SqlParameter("@PUESTO", entidad.Puesto),
                    new SqlParameter("@FECHA", entidad.Fecha <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.Fecha),
                    new SqlParameter("@ULTIMOCAMBIO", entidad.UltimoCambio <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.UltimoCambio),
                    new SqlParameter("@ACTIVO", entidad.Activo),
                    new SqlParameter("@DISTRIBUIDOR", entidad.IdDistribuidor),
                    new SqlParameter("@PERMISOS", entidad.Permisos.ToXML().ToString())
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroAdministrarUsuarios filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@CLAVE", filtro.Clave),
                    new SqlParameter("@NOMBRE", filtro.Nombre),
                    new SqlParameter("@ACTIVO", filtro.Activo),
                    new SqlParameter("@FECHA", filtro.Fecha <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.Fecha),
                    new SqlParameter("@DISTRIBUIDOR", filtro.IdDistribuidor),
                    new SqlParameter("@ULTIMOCAMBIO", filtro.UltimoCambio <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.UltimoCambio)
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
                        resultado = await reader.IsDBNullAsync(0) ? 0 : await reader.GetFieldValueAsync<int>(0);
                    }
                    return resultado;
                }).Wait();

            return resultado + 1;
        }

        internal AdministrarUsuarios Insertar(Conexiones conexion, SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            bool resultado = false;

            entidad.Clave = this.Consecutivo(conexion, sesion);

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        resultado = ((int)conn.CurrentResult) > 0;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Usuario: {0}", entidad.Nombre).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AU"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Wait();

            return resultado ? entidad : null;
        }

        internal AdministrarUsuarios Modificar(Conexiones conexion, SesionModuloWeb sesion, AdministrarUsuarios entidad)
        {
            bool resultado = false;
            DateTime f = DateTime.Now;
            entidad.UltimoCambio = new DateTime(f.Year, f.Month, f.Day, f.Hour, f.Minute, f.Second);

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        resultado = ((int)conn.CurrentResult) > 0;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Usuario: {0}", entidad.Nombre).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Modificado: {0}", resultado ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AU"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Wait();
            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
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
                          .AppendFormat("Usuario: {0}", filtro.Nombre).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Eliminado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                        Bitacora bita = new Bitacora()
                        {
                            Estacion = sesion.NoCliente,
                            Error = sb.ToString().Trim(),
                            Fecha = DateTime.Now,
                            Tipo = "AU"
                        };
                        try { servicio.Insertar(conn, sesion, bita); }
                        catch { }
                    }
                }).Result > 0;
        }

        internal AdministrarUsuarios Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuarios resultado = null;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

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

        internal ListaAdministrarUsuarios ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarUsuarios filtro)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

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
