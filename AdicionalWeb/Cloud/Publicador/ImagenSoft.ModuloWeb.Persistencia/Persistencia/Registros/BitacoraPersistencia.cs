using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Registros;
using ImagenSoft.ModuloWeb.Persistencia.Persistencia;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia
{
    public class BitacoraPersistencia
    {
        private string NombreEntidad = typeof(Bitacora).Name;

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

        public Bitacora Insertar(SesionModuloWeb sesion, Bitacora entidad)
        {
            TaskCompletionSource<Bitacora> _task = new TaskCompletionSource<Bitacora>(entidad);
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

        public Bitacora Modificar(SesionModuloWeb sesion, Bitacora entidad)
        {
            TaskCompletionSource<Bitacora> _task = new TaskCompletionSource<Bitacora>(entidad);
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

        public bool Eliminar(SesionModuloWeb sesion, FiltroBitacora filtro)
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

        public Bitacora Obtener(SesionModuloWeb sesion, FiltroBitacora filtro)
        {
            TaskCompletionSource<Bitacora> _task = new TaskCompletionSource<Bitacora>(filtro);
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

        public ListaBitacora ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroBitacora filtro)
        {
            TaskCompletionSource<ListaBitacora> _task = new TaskCompletionSource<ListaBitacora>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        ListaBitacora resultado = new ListaBitacora();
                        ListaBitacora aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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

        #region Transacciones
        public int Consecutivo(Conexiones conexion, SesionModuloWeb sesion)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Consecutivo,
                tabla = this.NombreEntidad
            };

            int resultado = 0;
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

        public Bitacora Insertar(Conexiones conexion, SesionModuloWeb sesion, Bitacora entidad)
        {
            entidad.Fecha = DateTime.Now;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros).Result > 0 ? entidad : null);
        }

        public Bitacora Modificar(Conexiones conexion, SesionModuloWeb sesion, Bitacora entidad)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros).Result > 0 ? entidad : null);
        }

        public bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroBitacora filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Eliminar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            return conexion.ExecuteNonQuery(parametros).Result > 0;
        }

        public Bitacora Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroBitacora filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            Bitacora resultado = null;
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

        public ListaBitacora ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroBitacora filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            ListaBitacora resultado = new ListaBitacora();
            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    Bitacora auxiliar = null;
                    while (await reader.ReadAsync())
                    {
                        auxiliar = await this.readerToEntidad(reader);
                        resultado.Add(auxiliar);
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }
        #endregion

        private async Task<Bitacora> readerToEntidad(SqlDataReader reader)
        {
            Bitacora entidad = new Bitacora();
            entidad.Id = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
            entidad.Cliente = await reader.IsDBNullAsync(1) ? 0 : reader.GetInt32(1);
            entidad.Estacion = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
            entidad.Tipo = await reader.IsDBNullAsync(3) ? string.Empty : reader.GetString(3);
            entidad.Fecha = await reader.IsDBNullAsync(4) ? SqlDateTime.MinValue.Value : reader.GetDateTime(4);
            entidad.Error = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(Bitacora entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@ID", entidad.Id),
                    new SqlParameter("@CLIENTE", entidad.Cliente),
                    new SqlParameter("@FECHA", entidad.Fecha <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.Fecha),
                    new SqlParameter("@TIPO", entidad.Tipo),
                    new SqlParameter("@ESTACION", entidad.Estacion),
                    new SqlParameter("@ERROR", "Ver.:[4.1]\r\n" + entidad.Error)
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroBitacora filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@ID", filtro.Id),
                    new SqlParameter("@CLIENTE", filtro.Cliente),
                    new SqlParameter("@FECHA", filtro.Fecha <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.Fecha),
                    new SqlParameter("@TIPO", filtro.Tipo),
                    new SqlParameter("@ESTACION", filtro.Estacion),
                    new SqlParameter("@FECHAINICIO", filtro.FechaInicio <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaInicio),
                    new SqlParameter("@FECHAFIN", filtro.FechaFin <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaFin)
                };
        }
    }
}

public class BitacoraException : Exception
{
    public BitacoraException()
        : base()
    {
        this.Sesion = new SesionModuloWeb();
        this.Bitacora = new Bitacora();
    }

    public BitacoraException(string message)
        : base(message)
    {
        this.Sesion = new SesionModuloWeb();
        this.Bitacora = new Bitacora();
    }

    public BitacoraException(string message, Exception innerException)
        : base(message, innerException)
    {
        this.Sesion = new SesionModuloWeb();
        this.Bitacora = new Bitacora();
    }

    public BitacoraException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        this.Sesion = new SesionModuloWeb();
        this.Bitacora = new Bitacora();
    }

    public SesionModuloWeb Sesion { get; set; }

    public Bitacora Bitacora { get; set; }
}