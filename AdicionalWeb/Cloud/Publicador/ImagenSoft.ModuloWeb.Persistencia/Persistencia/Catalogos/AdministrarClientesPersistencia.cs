using ImagenSoft.Extensiones;
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
    public class AdministrarClientesPersistencia
    {
        private string NombreEntidad = typeof(AdministrarClientes).Name;

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

        public AdministrarClientes Insertar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            TaskCompletionSource<AdministrarClientes> _task = new TaskCompletionSource<AdministrarClientes>(entidad);
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

        public AdministrarClientes Modificar(SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            TaskCompletionSource<AdministrarClientes> _task = new TaskCompletionSource<AdministrarClientes>(entidad);
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

        public bool Eliminar(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
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

        public AdministrarClientes Obtener(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            TaskCompletionSource<AdministrarClientes> _task = new TaskCompletionSource<AdministrarClientes>(filtro);
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

        public ListaAdministrarClientes ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            TaskCompletionSource<ListaAdministrarClientes> _task = new TaskCompletionSource<ListaAdministrarClientes>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
                            ListaAdministrarClientes aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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

        public bool ModificarUltimaConexion(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            bool resultado = false;

            if (string.IsNullOrEmpty(filtro.NoEstacion)) { return resultado; }

            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            AdministrarClientes item = this.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = filtro.NoEstacion });

                            if (item != null)
                            {
                                ParametrosConexion parametros = new ParametrosConexion()
                                {
                                    operacion = TipoOperacion.Especial_2,
                                    tabla = this.NombreEntidad,
                                    parameters = this.ConfiguraParametros(filtro)
                                };

                                resultado = conexion.ExecuteNonQuery(parametros).Result > 0;
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

        public bool ModificarUltimaConexionBidi(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            bool resultado = false;

            if (string.IsNullOrEmpty(filtro.NoEstacion)) { return resultado; }

            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            AdministrarClientes item = this.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = filtro.NoEstacion });

                            if (item != null)
                            {
                                var prms = this.ConfiguraParametros(filtro);

                                ParametrosConexion parametros = new ParametrosConexion()
                                {
                                    operacion = TipoOperacion.Especial_4,
                                    tabla = this.NombreEntidad,
                                    parameters = prms
                                };

                                conexion.ExecuteNonQuery(parametros).Wait();

                                parametros = new ParametrosConexion()
                                {
                                    operacion = TipoOperacion.Especial_3,
                                    tabla = this.NombreEntidad,
                                    parameters = prms
                                };

                                resultado = conexion.ExecuteNonQuery(parametros).Result > 0;
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

        public bool ModificarFechaHoraCliente(SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            bool resultado = false;
            if (string.IsNullOrEmpty(filtro.NoEstacion)) { return resultado; }

            TaskCompletionSource<bool> _task = new TaskCompletionSource<bool>(filtro);
            Task.Run(() =>
                {
                    using (Conexiones conexion = new Conexiones())
                    {
                        try
                        {
                            AdministrarClientes item = this.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = filtro.NoEstacion });
                            if (item != null)
                            {
                                filtro.FechaUltimaConexion = DateTime.Now;
                                filtro.FechaHoraCliente = sesion.FechaHoraCliente;
                                filtro.Version = "4.1";

                                ParametrosConexion parametros = new ParametrosConexion()
                                {
                                    operacion = TipoOperacion.Especial_1,
                                    tabla = this.NombreEntidad,
                                    parameters = this.ConfiguraParametros(filtro)
                                };

                                resultado = conexion.ExecuteNonQuery(parametros, (conn) =>
                                {
                                    MonitorTransaccionPersistencia servicio = new MonitorTransaccionPersistencia();
                                    servicio.ModificarFechaProxima(sesion);
                                }).Result > 0;
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

        private async Task<AdministrarClientes> readerToEntidad(SqlDataReader reader)
        {
            Task<SqlXml> tskEnlaces = reader.GetFieldValueAsync<SqlXml>(16);
            tskEnlaces.ConfigureAwait(false);

            AdministrarClientes entidad = new AdministrarClientes();
            entidad.Clave = await reader.IsDBNullAsync(0) ? 0 : await reader.GetFieldValueAsync<int>(0);
            entidad.NoEstacion = await reader.IsDBNullAsync(1) ? string.Empty : await reader.GetFieldValueAsync<string>(1);
            entidad.NombreComercial = await reader.IsDBNullAsync(2) ? string.Empty : await reader.GetFieldValueAsync<string>(2);
            entidad.EMail = await reader.IsDBNullAsync(3) ? string.Empty : await reader.GetFieldValueAsync<string>(3);
            entidad.Telefono = await reader.IsDBNullAsync(4) ? string.Empty : await reader.GetFieldValueAsync<string>(4);
            entidad.Contacto = await reader.IsDBNullAsync(5) ? string.Empty : await reader.GetFieldValueAsync<string>(5);
            entidad.FechaAlta = await reader.IsDBNullAsync(6) ? SqlDateTime.MinValue.Value : await reader.GetFieldValueAsync<DateTime>(6);
            entidad.FechaUltimaConexion = await reader.IsDBNullAsync(7) ? SqlDateTime.MinValue.Value : await reader.GetFieldValueAsync<DateTime>(7);
            entidad.Activo = await reader.IsDBNullAsync(8) ? string.Empty : await reader.GetFieldValueAsync<string>(8);
            entidad.HorasCorte = await reader.IsDBNullAsync(9) ? 0 : await reader.GetFieldValueAsync<int>(9);
            string zona = await reader.IsDBNullAsync(10) ? string.Empty : await reader.GetFieldValueAsync<string>(10);
            entidad.Desface = await reader.IsDBNullAsync(11) ? 0 : await reader.GetFieldValueAsync<int>(11);
            entidad.IdDistribuidor = await reader.IsDBNullAsync(12) ? 0 : await reader.GetFieldValueAsync<int>(12);
            entidad.Distribuidor = await reader.IsDBNullAsync(13) ? string.Empty : await reader.GetFieldValueAsync<string>(13);
            entidad.MonitorearCambioPrecio = await reader.IsDBNullAsync(14) ? string.Empty : await reader.GetFieldValueAsync<string>(14);
            entidad.MonitorearTransmisiones = await reader.IsDBNullAsync(15) ? string.Empty : await reader.GetFieldValueAsync<string>(15);
            entidad.DiasAtraso = (int)(DateTime.Now - entidad.FechaUltimaConexion).TotalDays;
            entidad.Zona = zona.Equals("F", StringComparison.CurrentCultureIgnoreCase) ? ZonasCambioPrecio.ZonaFronteriza : ZonasCambioPrecio.ZonaNormal;
            entidad.Version = await reader.IsDBNullAsync(17) ? string.Empty : await reader.GetFieldValueAsync<string>(17);
            entidad.Membrecia = await reader.IsDBNullAsync(18) ? (bool?)null : ((await reader.GetFieldValueAsync<string>(18)).IEquals("Si"));
            entidad.FechaMembrecia = await reader.IsDBNullAsync(19) ? SqlDateTime.MinValue.Value : await reader.GetFieldValueAsync<DateTime>(19);
            entidad.Matriz = await reader.IsDBNullAsync(20) ? string.Empty : await reader.GetFieldValueAsync<string>(20);
            entidad.Host = await reader.IsDBNullAsync(21) ? string.Empty : await reader.GetFieldValueAsync<string>(21);
            entidad.Puerto = await reader.IsDBNullAsync(22) ? 0 : await reader.GetFieldValueAsync<int>(22);
            entidad.Conexion = await reader.IsDBNullAsync(23) ? "No" : await reader.GetFieldValueAsync<string>(23);
            if ("Si".Equals(entidad.Conexion, StringComparison.OrdinalIgnoreCase))
            {
                entidad.Conexion = (entidad.FechaUltimaConexion.AddMinutes(15) >= DateTime.Now) ? "Si" : "No";
            }

            SqlXml enlaces = tskEnlaces.Result ?? SqlXml.Null;
            if (!enlaces.IsNull)
            {
                entidad.Enlaces.FromXML(enlaces.Value);
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(AdministrarClientes entidad)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@CLAVE", entidad.Clave),
                    new SqlParameter("@NOESTACION", entidad.NoEstacion),
                    new SqlParameter("@NOMBRECOMERCIAL", entidad.NombreComercial),
                    new SqlParameter("@EMAIL", entidad.EMail),
                    new SqlParameter("@TELEFONO", entidad.Telefono),
                    new SqlParameter("@CONTACTO", entidad.Contacto),
                    new SqlParameter("@FECHAALTA", entidad.FechaAlta <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : entidad.FechaAlta),
                    new SqlParameter("@FECHAULTIMOACCESO", entidad.FechaUltimaConexion <= SqlDateTime.MinValue.Value ? SqlDateTime.Null : entidad.FechaUltimaConexion),
                    new SqlParameter("@ACTIVO", entidad.Activo),
                    new SqlParameter("@HORASCORTE", entidad.HorasCorte),
                    new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza? "F" : "N")),
                    new SqlParameter("@ENLACES", entidad.Enlaces.ToXML().ToString()),
                    new SqlParameter("@DISTRIBUIDOR", entidad.IdDistribuidor),
                    new SqlParameter("@DESFACE", entidad.Desface),
                    new SqlParameter("@CAMBIOPRECIOS", entidad.MonitorearCambioPrecio),
                    new SqlParameter("@TRANSMISIONES", entidad.MonitorearTransmisiones),
                    new SqlParameter("@MEMBRECIA", entidad.Membrecia == null ? string.Empty : (entidad.Membrecia.Value ? "Si" : "No")),
                    new SqlParameter("@FECHAMEMBRECIA", entidad.FechaMembrecia <= SqlDateTime.MinValue.Value ? SqlDateTime.Null : entidad.FechaMembrecia),
                    new SqlParameter("@VERSION", (string.IsNullOrEmpty(entidad.Version) ? "3.1" : entidad.Version))
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroAdministrarClientes filtro)
        {
            return new SqlParameter[]
                {
                    new SqlParameter("@CLAVE", filtro.Clave),
                    new SqlParameter("@NOESTACION", filtro.NoEstacion),
                    new SqlParameter("@NOMBRECOMERCIAL", filtro.NombreComercial),
                    new SqlParameter("@FECHAINICIOALTA", filtro.FechaInicioAlta <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaInicioAlta),
                    new SqlParameter("@FECHAHORACLIENTE", filtro.FechaHoraCliente <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaHoraCliente),
                    new SqlParameter("@FECHAFINALTA", filtro.FechaFinAlta <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaFinAlta),
                    new SqlParameter("@FECHAINICIOULTIMOACCESO", filtro.FechaInicioUltimaConexion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaInicioUltimaConexion),
                    new SqlParameter("@FECHAFINULTIMOACCESO", filtro.FechaFinUltimaConexion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaFinUltimaConexion),
                    new SqlParameter("@FECHAULTIMOACCESO", filtro.FechaUltimaConexion <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaUltimaConexion),
                    new SqlParameter("@ACTIVO", filtro.Activo),
                    new SqlParameter("@ZONA", (filtro.Zona == ZonasCambioPrecio.ZonaFronteriza)? "F" : "N"),
                    new SqlParameter("@DISTRIBUIDOR", filtro.Distribuidor),
                    new SqlParameter("@MATRIZ", filtro.Matriz),
                    new SqlParameter("@MEMBRECIA", filtro.Membrecia == null ? string.Empty : (filtro.Membrecia.Value ? "Si" : "No")),
                    new SqlParameter("@FECHAMEMBRECIA", filtro.FechaMembrecia <= SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : filtro.FechaMembrecia),
                    new SqlParameter("@VERSION", (string.IsNullOrEmpty(filtro.Version) ? "3.1" : filtro.Version)),
                    new SqlParameter("@CONEXION", filtro.Conexion)
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

            conexion.ExecuteReader(parametros, async (comm, reader) =>
                {
                    if (await reader.ReadAsync())
                    {
                        resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                    }
                    return resultado;
                }).Wait();

            return resultado + 1;
        }

        internal AdministrarClientes Insertar(Conexiones conexion, SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Insertar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    bool flgOk = (int)(conn.CurrentResult ?? 0) > 0;
                    if (flgOk)
                    {
                        ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.EstacionesPersistencia srvEstacionesWeb = new ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.EstacionesPersistencia();
                        if (srvEstacionesWeb.Obtener(conn, sesion, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroEstacion() { NoEstacion = entidad.NoEstacion }) == null)
                        {
                            srvEstacionesWeb.Insertar(conn, sesion, new ImagenSoft.ModuloWeb.Entidades.Web.Estacion()
                                {
                                    ConMembresia = entidad.Membrecia == null ? false : entidad.Membrecia.Value,
                                    Consola = string.Empty,
                                    IP = entidad.Host,
                                    NoEstacion = entidad.NoEstacion,
                                    Matriz = entidad.Matriz,
                                    Nombre = entidad.NombreGrupo,
                                    Puerto = entidad.Puerto
                                });
                        }
                    }

                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Estación: {0}", entidad.NoEstacion).AppendLine()
                          .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                          .AppendFormat("Registrado: {0}", flgOk ? "Si" : "No");

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

        internal AdministrarClientes Modificar(Conexiones conexion, SesionModuloWeb sesion, AdministrarClientes entidad)
        {
            AdministrarClientes tmpCliente = this.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = entidad.NoEstacion });

            if (tmpCliente != null)
            {
                entidad.FechaUltimaConexion = tmpCliente.FechaUltimaConexion;
                entidad.Version = tmpCliente.Version;
            }

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            return (conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    bool flgOk = ((int)conn.CurrentResult) > 0;
                    if (flgOk)
                    {
                        ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.EstacionesPersistencia srvEstacionesWeb = new ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.EstacionesPersistencia();
                        ImagenSoft.ModuloWeb.Entidades.Web.Estacion estacion = new ImagenSoft.ModuloWeb.Entidades.Web.Estacion()
                        {
                            ConMembresia = entidad.Membrecia == null ? false : entidad.Membrecia.Value,
                            Consola = string.Empty,
                            IP = entidad.Host,
                            NoEstacion = entidad.NoEstacion,
                            Matriz = entidad.Matriz,
                            Nombre = entidad.NombreGrupo,
                            Puerto = entidad.Puerto
                        };

                        if (srvEstacionesWeb.Obtener(conn, sesion, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroEstacion() { NoEstacion = entidad.NoEstacion }) == null)
                        {
                            srvEstacionesWeb.Insertar(conn, sesion, estacion);
                        }
                        else
                        {
                            srvEstacionesWeb.Modificar(conn, sesion, estacion);
                        }
                    }

                    MonitorTransaccionPersistencia srvTransaccion = new MonitorTransaccionPersistencia();
                    srvTransaccion.ModificarFechaProxima(conn, sesion);
                }).Result > 0) ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Eliminar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            return (conexion.ExecuteNonQuery(parametros, (conn) =>
                {
                    MonitorCambioPrecioPersistencia srvMonitor = new MonitorCambioPrecioPersistencia();
                    MonitorTransaccionPersistencia srvTransa = new MonitorTransaccionPersistencia();
                    srvMonitor.Eliminar(conn, sesion, new FiltroMonitorCambioPrecio() { NoEstacion = filtro.NoEstacion });
                    srvTransa.Eliminar(conn, sesion, new FiltroMonitorTransaccion() { NoEstacion = filtro.NoEstacion });

                    BitacoraPersistencia servicio = new BitacoraPersistencia();
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                          .AppendFormat("Estación: {0}", filtro.NoEstacion).AppendLine()
                          .AppendFormat("Zona: {0}", filtro.Zona).AppendLine()
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
                }).Result > 0);
        }

        internal AdministrarClientes Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Obtener,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            AdministrarClientes resultado = null;
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

        internal ListaAdministrarClientes ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroAdministrarClientes filtro)
        {
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
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
