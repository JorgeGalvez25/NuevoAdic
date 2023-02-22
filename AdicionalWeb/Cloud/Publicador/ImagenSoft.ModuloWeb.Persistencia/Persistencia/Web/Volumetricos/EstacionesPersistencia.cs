using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web
{
    public class EstacionesPersistencia
    {
        private const string MASIVE_UPDATE_CONEXION_QUERY = "UPDATE DBO.GRUPOCLIENTES SET CONEXION = '{0}' WHERE NOESTACION = '{1}';";

        //1753-01-01 00:00:00.000
        private const string MASIVE_UPDATE_FECHA_QUERY = "UPDATE DBO.CLIENTES SET FECHAULTIMOACCESO = '{0:yyyy-MM-dd HH:mm:ss.fff}' WHERE NOESTACION = '{1}';";

        private static readonly Object _lock = new Object();

        private static ListaEstaciones _listaEstaciones;
        public static ListaEstaciones ListaEstaciones
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (_listaEstaciones == null)
                {
                    _listaEstaciones = EstacionesObtenerListado();
                }
                else if (_listaEstaciones.Count <= 0)
                {
                    _listaEstaciones = EstacionesObtenerListado();
                }

                return _listaEstaciones;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_listaEstaciones != null)
                {
                    _listaEstaciones.Clear();
                }

                _listaEstaciones = value;
            }
        }

        private static ListaEstaciones EstacionesObtenerListado()
        {
            lock (_lock)
            {
                EstacionesPersistencia srvEstacion = new EstacionesPersistencia();
                ListaEstaciones _lst = srvEstacion.ObtenerTodosFiltro(null, new FiltroEstacion());

                //_lst.ParallelForEach(p =>
                Parallel.ForEach(_lst, (p) =>
                    {
                        // Serivicio
                        // Cambiar la IP destino
                        // Obtener el metodo ping del servicio a la IP de la estacion
                        // Omitir la Estacion de login
                        p.ConMembresia = true; // Requiere del metodo ping que proporcionara Enrique
                    });

                _lst.RemoveAll(p => !p.ConMembresia);
                return _lst;
            }
        }

        private string NombreEntidad = typeof(Estacion).Name;

        private async Task<Estacion> readerToEntidad(SqlDataReader reader)
        {
            Estacion entidad = new Estacion();
            entidad.NoEstacion = await reader.IsDBNullAsync(0) ? string.Empty : reader.GetString(0);
            entidad.Matriz = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
            entidad.Nombre = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
            entidad.IP = await reader.IsDBNullAsync(3) ? string.Empty : reader.GetString(3);
            entidad.Puerto = await reader.IsDBNullAsync(4) ? 0 : reader.GetInt32(4);
            entidad.Conexion = await reader.IsDBNullAsync(6) ? false : "Si".Equals(reader.GetString(6), StringComparison.OrdinalIgnoreCase);
            //entidad.Consola = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);

            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(Estacion entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@NOMBRE", entidad.Nombre),
                    new SqlParameter("@NOESTACION", entidad.NoEstacion),
                    new SqlParameter("@MATRIZ", entidad.Matriz),
                    new SqlParameter("@IP", entidad.IP),
                    new SqlParameter("@PUERTO", entidad.Puerto),
                    new SqlParameter("@CONSOLA", entidad.Consola),
                    new SqlParameter("@CONEXION", entidad.Conexion ? "Si" :"No"),
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroEstacion filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@NOESTACION", filtro.NoEstacion),
                    new SqlParameter("@IP", filtro.IP),
                    new SqlParameter("@MATRIZ", filtro.Matriz),
                    new SqlParameter("@CONEXION", filtro.Conexion == null ? string.Empty : (filtro.Conexion.Value ? "Si" :"No")),
                    new SqlParameter("@ACTIVO", filtro.Activo == null ? string.Empty : (filtro.Activo.Value ? "Si" :"No")),
                };
        }

        public Estacion Insertar(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, Estacion entidad)
        {
            TaskCompletionSource<Estacion> _task = new TaskCompletionSource<Estacion>(entidad);
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

        public Estacion Modificar(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, Estacion entidad)
        {
            TaskCompletionSource<Estacion> _task = new TaskCompletionSource<Estacion>(entidad);
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

        public Estacion Obtener(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, FiltroEstacion filtro)
        {
            TaskCompletionSource<Estacion> _task = new TaskCompletionSource<Estacion>(filtro);
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

        public ListaEstaciones ObtenerTodosFiltro(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, FiltroEstacion filtro)
        {
            TaskCompletionSource<ListaEstaciones> _task = new TaskCompletionSource<ListaEstaciones>(filtro);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {
                        ListaEstaciones resultado = new ListaEstaciones();

                        ListaEstaciones aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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

        public bool ModificarConexion(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, ListaEstaciones entidad)
        {
            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(entidad);
            Task.Run(() =>
            {
                using (Conexiones conexion = new Conexiones())
                {
                    try
                    {

                        _task.TrySetResult(this.ModificarConexion(conexion, sesion, entidad));
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

        internal Estacion Insertar(Conexiones conexion, ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, Estacion entidad)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return conexion.ExecuteNonQuery(parametros).Result > 0 ? entidad : null;
        }

        internal Estacion Modificar(Conexiones conexion, ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, Estacion entidad)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return conexion.ExecuteNonQuery(parametros).Result > 0 ? entidad : null;
        }

        internal Estacion Obtener(Conexiones conexion, ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, FiltroEstacion filtro)
        {
            Estacion resultado = null;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    int count = 1;
                    if (await reader.ReadAsync())
                    {
                        resultado = await this.readerToEntidad(reader).ConfigureAwait(false);
                        resultado.Clave = count++;
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }

        internal ListaEstaciones ObtenerTodosFiltro(Conexiones conexion, ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, FiltroEstacion filtro)
        {
            ListaEstaciones resultado = new ListaEstaciones();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            conexion.ExecuteReader(parametros, async (conn, reader) =>
                {
                    Estacion entidad = null;
                    int count = 1;

                    while (await reader.ReadAsync())
                    {
                        entidad = await this.readerToEntidad(reader).ConfigureAwait(false);
                        entidad.Clave = count++;
                        resultado.Add(entidad);
                    }
                    return resultado;
                }).Wait();

            return resultado;
        }

        internal bool ModificarConexion(Conexiones conexion, ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, ListaEstaciones entidad)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in entidad.Where(p => p.Conexion))
            {
                sb.AppendFormat(MASIVE_UPDATE_FECHA_QUERY, DateTime.Now, item.NoEstacion)
                      .AppendLine();
            }

            entidad.ForEach(p =>
                {
                    sb.AppendFormat(MASIVE_UPDATE_CONEXION_QUERY, (p.Conexion ? "Si" : "No"), p.NoEstacion)
                      .AppendLine();
                });

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.None,
                tabla = this.NombreEntidad,
                query = sb.ToString().Trim()
            };

            MensajesRegistros.Informacion(sb.ToString().Trim());

            return conexion.ExecuteNonQuery(parametros).Result > 0;
        }

        #endregion
    }
}
