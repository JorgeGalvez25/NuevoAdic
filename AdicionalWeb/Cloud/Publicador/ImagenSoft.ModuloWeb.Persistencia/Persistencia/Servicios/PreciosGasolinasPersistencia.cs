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
    public class PreciosGasolinasPersistencia
    {
        public static bool HayCambioDePrecio { get; set; }

        private string NombreEntidad = typeof(PreciosGasolinas).Name;

        public int Consecutivo(SesionModuloWeb sesion)
        {
            int resultado = 0;
            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Consecutivo(conexion, sesion);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Consecutivo, this.NombreEntidad);

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))// comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //    {
            //        try
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado + 1;
        }

        public PreciosGasolinas Insertar(SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinas resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Insertar(conexion, sesion, entidad);
            }

            //bool flgExiste = false;

            //var item = Obtener(sesion, new FiltroPreciosGasolinas()
            //{
            //    Zona = entidad.Zona,
            //    Fecha = entidad.Fecha
            //});

            //if (item != null)
            //{
            //    if (item.PrecioDiessel == entidad.PrecioDiessel &&
            //        item.PrecioMagna == entidad.PrecioMagna &&
            //        item.PrecioPremium == entidad.PrecioPremium)
            //    {
            //        flgExiste = true;
            //        resultado = true;
            //    }
            //}

            //if (!flgExiste)
            //{
            //    new Conexiones().ExecuteDataReader(async (comm) =>
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
            //            entidad.Id = this.Consecutivo(sesion);
            //            comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
            //            resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //            if (resultado)
            //            {
            //                comm.CommandText = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
            //                comm.Parameters.Clear();
            //                comm.Parameters.Add(new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")));
            //                comm.Parameters.Add(new SqlParameter("@PROGRAMADO", "No"));
            //                comm.Parameters.Add(new SqlParameter("@APLICADO", "No"));
            //                comm.Parameters.Add(new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value));
            //                comm.Parameters.Add(new SqlParameter("@FECHAAPLICACION", DBNull.Value));

            //                bool result = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //                BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //                {
            //                    StringBuilder sb = new StringBuilder();
            //                    sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                      .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                      .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
            //                      .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
            //                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                      .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

            //                    Bitacora bita = new Bitacora()
            //                    {
            //                        Estacion = sesion.NoCliente,
            //                        Error = sb.ToString().Trim(),
            //                        Fecha = DateTime.Now,
            //                        Tipo = "PG"
            //                    };
            //                    try { servicio.Insertar(sesion, bita); }
            //                    catch { }
            //                }
            //            }
            //        }).Wait();
            //}

            return resultado;
        }

        public PreciosGasolinas Modificar(SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinas resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Modificar(conexion, sesion, entidad);
            }
            //new Conexiones().ExecuteDataReader(async (comm) =>
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //        entidad.EstatusCambioPrecio = EstatusProgramado.No;
            //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //        BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
            //              .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Modificado: {0}", resultado ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "PG"
            //                };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }

            //        if (resultado)
            //        {
            //            comm.CommandText = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
            //            comm.Parameters.Clear();
            //            comm.Parameters.Add(new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")));
            //            comm.Parameters.Add(new SqlParameter("@PROGRAMADO", "No"));
            //            comm.Parameters.Add(new SqlParameter("@APLICADO", "No"));
            //            comm.Parameters.Add(new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value));
            //            comm.Parameters.Add(new SqlParameter("@FECHAAPLICACION", DBNull.Value));

            //            bool result = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;
            //            {
            //                StringBuilder sb = new StringBuilder();
            //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //                  .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
            //                  .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
            //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //                  .AppendFormat("Modificado: {0}", result ? "Si" : "No");

            //                Bitacora bita = new Bitacora()
            //                {
            //                    Estacion = sesion.NoCliente,
            //                    Error = sb.ToString().Trim(),
            //                    Fecha = DateTime.Now,
            //                    Tipo = "PG"
            //                };
            //                try { servicio.Insertar(sesion, bita); }
            //                catch { }
            //            }
            //        }
            //    }).Wait();

            return resultado;
        }

        private PreciosGasolinas ModificarObtener(SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            PreciosGasolinas resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.ModificarObtener(conexion, sesion, entidad);
            }


            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //    resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;
            //}).Wait();

            return resultado;
        }

        public bool Eliminar(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            bool resultado = false;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Eliminar(conexion, sesion, filtro);
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //    BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //          .AppendFormat("Programado: {0}", filtro.EstatusCambioPrecio).AppendLine()
            //          .AppendFormat("Zona: {0}", filtro.Zona).AppendLine()
            //          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //          .AppendFormat("Eliminado: {0}", resultado ? "Si" : "No");

            //        Bitacora bita = new Bitacora()
            //        {
            //            Estacion = sesion.NoCliente,
            //            Error = sb.ToString().Trim(),
            //            Fecha = DateTime.Now,
            //            Tipo = "PG"
            //        };
            //        try { servicio.Insertar(sesion, bita); }
            //        catch { }
            //    }
            //}).Wait();

            return resultado;
        }

        public PreciosGasolinas Obtener(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinas resultado = null;

            using (Conexiones conexion = new Conexiones())
            {
                resultado = this.Obtener(conexion, sesion, filtro);
            }

            //AdministrarClientes cliente = null;
            //Action<string, bool> addCliente = new Action<string, bool>((c, v) =>
            //    {
            //        if (MonitorCambioPrecioPersistencia.ClientesValidos == null)
            //        {
            //            MonitorCambioPrecioPersistencia.ClientesValidos = new System.Collections.Generic.Dictionary<string, bool>();
            //        }

            //        if (MonitorCambioPrecioPersistencia.ClientesValidos.ContainsKey(c))
            //        {
            //            MonitorCambioPrecioPersistencia.ClientesValidos[c] = v;
            //        }
            //        else
            //        {
            //            MonitorCambioPrecioPersistencia.ClientesValidos.Add(c, v);
            //        }
            //    });

            //if (!sesion.NoCliente.Equals("Monitor", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
            //    cliente = servicio.Obtener(sesion, new FiltroAdministrarClientes() { NoEstacion = sesion.NoCliente });

            //    if (cliente == null)
            //    {
            //        addCliente(sesion.NoCliente, false);
            //        return resultado;
            //    }

            //    if (!cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        addCliente(sesion.NoCliente, false);
            //        return resultado;
            //    }

            //    if (!cliente.MonitorearCambioPrecio.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        addCliente(sesion.NoCliente, false);
            //        return resultado;
            //    }
            //}

            //addCliente(sesion.NoCliente, true);

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
            //                    if (cliente != null)
            //                    {
            //                        resultado.Desface = cliente.Desface;
            //                    }

            //                    if (resultado.Desface > 0)
            //                    {
            //                        resultado.Fecha = resultado.Fecha.AddMinutes(resultado.Desface);
            //                    }
            //                }
            //            }
            //            finally
            //            {
            //                if (!reader.IsClosed) { reader.Close(); }
            //            }
            //        }
            //    });

            return resultado;
        }

        public ListaPreciosGasolinas ObtenerTodosFiltro(SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            ListaPreciosGasolinas resultado = new ListaPreciosGasolinas();

            using (Conexiones conexion = new Conexiones())
            {
                ListaPreciosGasolinas aux = this.ObtenerTodosFiltro(conexion, sesion, filtro);
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
            //            PreciosGasolinas auxiliar = null;
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

        #region Transacciones
        internal int Consecutivo(Conexiones conexion, SesionModuloWeb sesion)
        {
            int resultado = 0;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Consecutivo,
                tabla = this.NombreEntidad
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         if (await reader.ReadAsync())
                                                         {
                                                             resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                                                         }
                                                         return resultado;
                                                     }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Consecutivo, this.NombreEntidad);

            //    using (SqlDataReader reader = await comm.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))// comm.EndExecuteReader(comm.BeginExecuteReader(System.Data.CommandBehavior.SingleRow)))
            //    {
            //        try
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                resultado = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //}).Wait();

            return resultado + 1;
        }

        internal PreciosGasolinas Insertar(Conexiones conexion, SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            bool resultado = false;
            bool flgExiste = false;

            PreciosGasolinas item = this.Obtener(conexion, sesion, new FiltroPreciosGasolinas()
            {
                Zona = entidad.Zona,
                Fecha = entidad.Fecha
            });

            if (item != null)
            {
                if (item.PrecioDiessel == entidad.PrecioDiessel &&
                    item.PrecioMagna == entidad.PrecioMagna &&
                    item.PrecioPremium == entidad.PrecioPremium)
                {
                    flgExiste = true;
                    resultado = true;
                }
            }

            if (!flgExiste)
            {
                entidad.Id = this.Consecutivo(conexion, sesion);
                ParametrosConexion parametros = new ParametrosConexion()
                {
                    operacion = TipoOperacion.Insertar,
                    tabla = this.NombreEntidad,
                    parameters = this.ConfiguraParametros(entidad)
                };

                using (Task<int> aux = conexion.ExecuteNonQuery(parametros, (conn) =>
                                                                {
                                                                    int value = (int)conn.CurrentResult;
                                                                    if (value > 0)
                                                                    {
                                                                        ParametrosConexion _parametros = new ParametrosConexion()
                                                                        {
                                                                            operacion = TipoOperacion.Especial_1,
                                                                            tabla = this.NombreEntidad,
                                                                            parameters = new SqlParameter[]
                                                                                {
                                                                                    new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")),
                                                                                    new SqlParameter("@PROGRAMADO", "No"),
                                                                                    new SqlParameter("@APLICADO", "No"),
                                                                                    new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value),
                                                                                    new SqlParameter("@FECHAAPLICACION", DBNull.Value)
                                                                                }
                                                                        };

                                                                        using (Task<int> inner = conn.ExecuteNonQuery(_parametros))
                                                                        {
                                                                            inner.Wait();
                                                                        }

                                                                        BitacoraPersistencia servicio = new BitacoraPersistencia();
                                                                        {
                                                                            StringBuilder sb = new StringBuilder();
                                                                            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                              .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
                                                                              .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                                                                              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                              .AppendFormat("Registrado: {0}", value > 0 ? "Si" : "No");

                                                                            Bitacora bita = new Bitacora()
                                                                            {
                                                                                Estacion = sesion.NoCliente,
                                                                                Error = sb.ToString().Trim(),
                                                                                Fecha = DateTime.Now,
                                                                                Tipo = "PG"
                                                                            };
                                                                            try { servicio.Insertar(conn, sesion, bita); }
                                                                            catch { }
                                                                        }
                                                                    }
                                                                }))
                {
                    aux.Wait();
                    resultado = aux.Result > 0;
                }


                //    new Conexiones().ExecuteDataReader(async (comm) =>
                //    {
                //        comm.CommandText = xml.GetOperation(TipoOperacion.Insertar, this.NombreEntidad);
                //        entidad.Id = this.Consecutivo(sesion);
                //        comm.Parameters.AddRange(this.ConfiguraParametros(entidad));
                //        resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

                //        if (resultado)
                //        {
                //            comm.CommandText = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
                //            comm.Parameters.Clear();
                //            comm.Parameters.Add(new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")));
                //            comm.Parameters.Add(new SqlParameter("@PROGRAMADO", "No"));
                //            comm.Parameters.Add(new SqlParameter("@APLICADO", "No"));
                //            comm.Parameters.Add(new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value));
                //            comm.Parameters.Add(new SqlParameter("@FECHAAPLICACION", DBNull.Value));

                //            bool result = await comm.ExecuteNonQueryAsync() > 0;//comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

                //            BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
                //            {
                //                StringBuilder sb = new StringBuilder();
                //                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                //                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                //                  .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
                //                  .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                //                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                //                  .AppendFormat("Registrado: {0}", resultado ? "Si" : "No");

                //                Bitacora bita = new Bitacora()
                //                {
                //                    Estacion = sesion.NoCliente,
                //                    Error = sb.ToString().Trim(),
                //                    Fecha = DateTime.Now,
                //                    Tipo = "PG"
                //                };
                //                try { servicio.Insertar(sesion, bita); }
                //                catch { }
                //            }
                //        }
                //    }).Wait();
            }

            return resultado ? entidad : null;
        }

        internal PreciosGasolinas Modificar(Conexiones conexion, SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            bool resultado = false;
            entidad.EstatusCambioPrecio = EstatusProgramado.No;
            BitacoraPersistencia servicio = new BitacoraPersistencia();

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros, (conn) =>
                                                            {
                                                                StringBuilder sb = new StringBuilder();
                                                                sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                  .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                  .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
                                                                  .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                                                                  .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                  .AppendFormat("Modificado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                Bitacora bita = new Bitacora()
                                                                {
                                                                    Estacion = sesion.NoCliente,
                                                                    Error = sb.ToString().Trim(),
                                                                    Fecha = DateTime.Now,
                                                                    Tipo = "PG"
                                                                };
                                                                try { servicio.Insertar(conn, sesion, bita); }
                                                                catch { }

                                                                if (((int)conn.CurrentResult) > 0)
                                                                {
                                                                    ParametrosConexion _parametros = new ParametrosConexion()
                                                                    {
                                                                        operacion = TipoOperacion.Especial_1,
                                                                        tabla = this.NombreEntidad,
                                                                        parameters = new SqlParameter[]
                                                                            {
                                                                                new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")),
                                                                                new SqlParameter("@PROGRAMADO", "No"),
                                                                                new SqlParameter("@APLICADO", "No"),
                                                                                new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value),
                                                                                new SqlParameter("@FECHAAPLICACION", DBNull.Value)
                                                                            }
                                                                    };

                                                                    using (Task<int> inner = conn.ExecuteNonQuery(_parametros, (iConn) =>
                                                                                                                  {
                                                                                                                      StringBuilder iSb = new StringBuilder();
                                                                                                                      iSb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
                                                                                                                         .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
                                                                                                                         .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
                                                                                                                         .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
                                                                                                                         .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                                                                         .AppendFormat("Modificado: {0}", ((int)iConn.CurrentResult) > 0 ? "Si" : "No");

                                                                                                                      Bitacora iBita = new Bitacora()
                                                                                                                      {
                                                                                                                          Estacion = sesion.NoCliente,
                                                                                                                          Error = iSb.ToString().Trim(),
                                                                                                                          Fecha = DateTime.Now,
                                                                                                                          Tipo = "PG"
                                                                                                                      };
                                                                                                                      try { servicio.Insertar(iConn, sesion, iBita); }
                                                                                                                      catch { }
                                                                                                                  }))
                                                                    {
                                                                        inner.Wait();
                                                                    }
                                                                }
                                                            }))
            {
                aux.Wait();
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //    entidad.EstatusCambioPrecio = EstatusProgramado.No;
            //    comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //    resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //    BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //          .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
            //          .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
            //          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //          .AppendFormat("Modificado: {0}", resultado ? "Si" : "No");

            //        Bitacora bita = new Bitacora()
            //        {
            //            Estacion = sesion.NoCliente,
            //            Error = sb.ToString().Trim(),
            //            Fecha = DateTime.Now,
            //            Tipo = "PG"
            //        };
            //        try { servicio.Insertar(sesion, bita); }
            //        catch { }
            //    }

            //    if (resultado)
            //    {
            //        comm.CommandText = xml.GetOperation(TipoOperacion.Especial_1, this.NombreEntidad);
            //        comm.Parameters.Clear();
            //        comm.Parameters.Add(new SqlParameter("@ZONA", (entidad.Zona == ZonasCambioPrecio.ZonaFronteriza ? "F" : "N")));
            //        comm.Parameters.Add(new SqlParameter("@PROGRAMADO", "No"));
            //        comm.Parameters.Add(new SqlParameter("@APLICADO", "No"));
            //        comm.Parameters.Add(new SqlParameter("@PRECIOPROGRAMADO", DBNull.Value));
            //        comm.Parameters.Add(new SqlParameter("@FECHAAPLICACION", DBNull.Value));

            //        bool result = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //              .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //              .AppendFormat("Programado: {0}", entidad.EstatusCambioPrecio).AppendLine()
            //              .AppendFormat("Zona: {0}", entidad.Zona).AppendLine()
            //              .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //              .AppendFormat("Modificado: {0}", result ? "Si" : "No");

            //            Bitacora bita = new Bitacora()
            //            {
            //                Estacion = sesion.NoCliente,
            //                Error = sb.ToString().Trim(),
            //                Fecha = DateTime.Now,
            //                Tipo = "PG"
            //            };
            //            try { servicio.Insertar(sesion, bita); }
            //            catch { }
            //        }
            //    }
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal PreciosGasolinas ModificarObtener(Conexiones conexion, SesionModuloWeb sesion, PreciosGasolinas entidad)
        {
            bool resultado = false;

            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.Modificar,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(entidad)
            };

            using (Task<int> aux = conexion.ExecuteNonQuery(parametros))
            {
                aux.Wait();
                resultado = aux.Result > 0;
            }

            //new Conexiones().ExecuteDataReader(async (comm) =>
            //{
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Modificar, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(entidad));

            //    resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;
            //}).Wait();

            return resultado ? entidad : null;
        }

        internal bool Eliminar(Conexiones conexion, SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
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
                                                                      .AppendFormat("Programado: {0}", filtro.EstatusCambioPrecio).AppendLine()
                                                                      .AppendFormat("Zona: {0}", filtro.Zona).AppendLine()
                                                                      .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
                                                                      .AppendFormat("Eliminado: {0}", ((int)conn.CurrentResult) > 0 ? "Si" : "No");

                                                                    Bitacora bita = new Bitacora()
                                                                    {
                                                                        Estacion = sesion.NoCliente,
                                                                        Error = sb.ToString().Trim(),
                                                                        Fecha = DateTime.Now,
                                                                        Tipo = "PG"
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
            //    comm.CommandText = xml.GetOperation(TipoOperacion.Eliminar, this.NombreEntidad);
            //    comm.Parameters.AddRange(this.ConfiguraParametros(filtro));

            //    resultado = await comm.ExecuteNonQueryAsync() > 0;// comm.EndExecuteNonQuery(comm.BeginExecuteNonQuery()) > 0;

            //    BitacoraPersistencia servicio = new BitacoraPersistencia(comm);
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        sb.AppendFormat("Nombre: {0}", sesion.Nombre).AppendLine()
            //          .AppendFormat("Clave: {0}", sesion.Clave).AppendLine()
            //          .AppendFormat("Programado: {0}", filtro.EstatusCambioPrecio).AppendLine()
            //          .AppendFormat("Zona: {0}", filtro.Zona).AppendLine()
            //          .AppendFormat("Fecha: {0:dd/MM/yyyy HH:mm}", DateTime.Now).AppendLine()
            //          .AppendFormat("Eliminado: {0}", resultado ? "Si" : "No");

            //        Bitacora bita = new Bitacora()
            //        {
            //            Estacion = sesion.NoCliente,
            //            Error = sb.ToString().Trim(),
            //            Fecha = DateTime.Now,
            //            Tipo = "PG"
            //        };
            //        try { servicio.Insertar(sesion, bita); }
            //        catch { }
            //    }
            //}).Wait();

            return resultado;
        }

        internal PreciosGasolinas Obtener(Conexiones conexion, SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            PreciosGasolinas resultado = null;
            AdministrarClientes cliente = null;
            Action<string, bool> addCliente = new Action<string, bool>((c, v) =>
            {
                if (MonitorCambioPrecioPersistencia.ClientesValidos == null)
                {
                    MonitorCambioPrecioPersistencia.ClientesValidos = new System.Collections.Generic.Dictionary<string, bool>();
                }

                if (MonitorCambioPrecioPersistencia.ClientesValidos.ContainsKey(c))
                {
                    MonitorCambioPrecioPersistencia.ClientesValidos[c] = v;
                }
                else
                {
                    MonitorCambioPrecioPersistencia.ClientesValidos.Add(c, v);
                }
            });

            if (!sesion.NoCliente.Equals("Monitor", StringComparison.CurrentCultureIgnoreCase))
            {
                AdministrarClientesPersistencia servicio = new AdministrarClientesPersistencia();
                cliente = servicio.Obtener(conexion, sesion, new FiltroAdministrarClientes() { NoEstacion = sesion.NoCliente });

                if (cliente == null)
                {
                    addCliente(sesion.NoCliente, false);
                    return resultado;
                }

                if (!cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    addCliente(sesion.NoCliente, false);
                    return resultado;
                }

                if (!cliente.MonitorearCambioPrecio.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    addCliente(sesion.NoCliente, false);
                    return resultado;
                }
            }

            addCliente(sesion.NoCliente, true);
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
                                                             if (cliente != null)
                                                             {
                                                                 resultado.Desface = cliente.Desface;
                                                             }

                                                             if (resultado.Desface > 0)
                                                             {
                                                                 resultado.Fecha = resultado.Fecha.AddMinutes(resultado.Desface);
                                                             }
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
            //                if (cliente != null)
            //                {
            //                    resultado.Desface = cliente.Desface;
            //                }

            //                if (resultado.Desface > 0)
            //                {
            //                    resultado.Fecha = resultado.Fecha.AddMinutes(resultado.Desface);
            //                }
            //            }
            //        }
            //        finally
            //        {
            //            if (!reader.IsClosed) { reader.Close(); }
            //        }
            //    }
            //});

            return resultado;
        }

        internal ListaPreciosGasolinas ObtenerTodosFiltro(Conexiones conexion, SesionModuloWeb sesion, FiltroPreciosGasolinas filtro)
        {
            ListaPreciosGasolinas resultado = new ListaPreciosGasolinas();
            ParametrosConexion parametros = new ParametrosConexion()
            {
                operacion = TipoOperacion.ObtenerTodos,
                tabla = this.NombreEntidad,
                parameters = this.ConfiguraParametros(filtro)
            };

            using (Task aux = conexion.ExecuteReader(parametros, async (conn, reader) =>
                                                     {
                                                         PreciosGasolinas auxiliar = null;
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
            //            PreciosGasolinas auxiliar = null;
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

        private async Task<PreciosGasolinas> readerToEntidad(SqlDataReader reader)
        {
            PreciosGasolinas entidad = new PreciosGasolinas();
            {
                entidad.Id = await reader.IsDBNullAsync(0) ? 0 : reader.GetInt32(0);
                entidad.Fecha = await reader.IsDBNullAsync(1) ? SqlDateTime.MinValue.Value : reader.GetDateTime(1).Date;
                entidad.PrecioMagna = await reader.IsDBNullAsync(2) ? 0M : reader.GetDecimal(2);
                entidad.PrecioPremium = await reader.IsDBNullAsync(3) ? 0M : reader.GetDecimal(3);
                entidad.PrecioDiessel = await reader.IsDBNullAsync(4) ? 0M : reader.GetDecimal(4);
                string zona = await reader.IsDBNullAsync(5) ? string.Empty : reader.GetString(5);
                string estatus = await reader.IsDBNullAsync(6) ? string.Empty : reader.GetString(6);

                if (entidad.Desface > 0)
                {
                    entidad.Fecha = entidad.Fecha.AddMinutes(entidad.Desface);
                }

                switch (zona.ToUpper())
                {
                    case "F":
                        entidad.Zona = ZonasCambioPrecio.ZonaFronteriza;
                        break;
                    case "N":
                        entidad.Zona = ZonasCambioPrecio.ZonaNormal;
                        break;
                }

                DateTime fecha = DateTime.Now;
                DateTime eFecha = entidad.Fecha.AddHours(2);

                if (((entidad.Fecha.Date <= fecha.Date) &&
                    (eFecha.TimeOfDay <= fecha.TimeOfDay)) &&
                    (!estatus.ToUpper().Equals("S")))
                {
                    entidad.EstatusCambioPrecio = EstatusProgramado.Si;
                    this.ModificarObtener(null, entidad);
                    PreciosGasolinasPersistencia.HayCambioDePrecio = false;

                    if (MonitorCambioPrecioPersistencia.ClientesValidos != null)
                    {
                        MonitorCambioPrecioPersistencia.ClientesValidos.Clear();
                    }
                }
                else
                {
                    entidad.EstatusCambioPrecio = estatus.ToUpper().Equals("S")
                                                    ? EstatusProgramado.Si
                                                    : EstatusProgramado.No;
                }
            }
            return entidad;
        }

        private SqlParameter[] ConfiguraParametros(PreciosGasolinas entidad)
        {
            string zona = string.Empty;

            switch (entidad.Zona)
            {
                case ZonasCambioPrecio.ZonaFronteriza:
                    zona = "F";
                    break;
                case ZonasCambioPrecio.ZonaNormal:
                    zona = "N";
                    break;
                default:
                    break;
            }

            string estatus = "N";
            switch (entidad.EstatusCambioPrecio)
            {
                case EstatusProgramado.Si:
                    estatus = "S";
                    break;
                case EstatusProgramado.No:
                    estatus = "N";
                    break;
                default:
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ID", entidad.Id),
                    new SqlParameter("@FECHA", entidad.Fecha.Date),
                    new SqlParameter("@PRECIOPREMIUM", entidad.PrecioPremium),
                    new SqlParameter("@PRECIOMAGNA", entidad.PrecioMagna),
                    new SqlParameter("@PRECIODIESSEL", entidad.PrecioDiessel),
                    new SqlParameter("@ZONA", zona),
                    new SqlParameter("@ESTATUS",estatus),
                };
        }

        private SqlParameter[] ConfiguraParametros(FiltroPreciosGasolinas filtro)
        {
            string zona = string.Empty;

            switch (filtro.Zona)
            {
                case ZonasCambioPrecio.ZonaFronteriza:
                    zona = "F";
                    break;
                case ZonasCambioPrecio.ZonaNormal:
                    zona = "N";
                    break;
                default:
                    break;
            }
            string estatus = "N";
            switch (filtro.EstatusCambioPrecio)
            {
                case EstatusProgramado.Si:
                    estatus = "S";
                    break;
                case EstatusProgramado.No:
                    estatus = "N";
                    break;
                default:
                    break;
            }

            return new SqlParameter[]
                {
                    new SqlParameter("@ID", filtro.Id),
                    new SqlParameter("@FECHA", filtro.Fecha),
                    new SqlParameter("@ZONA", zona),
                    new SqlParameter("@ESTATUS", estatus),
                };
        }
    }
}

