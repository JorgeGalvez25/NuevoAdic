using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Persistencia.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia
{
    public class Conexiones : IDisposable
    {
        public SqlCommand Comando;

        public object CurrentResult;

        public bool IsFaulted;

        public void Clear()
        {
            if (this.Comando != null)
            {
                if (this.Comando.Transaction != null)
                {
                    this.Comando.Transaction.Rollback();
                    this.Comando.Transaction.Dispose();
                }

                if (this.Comando.Connection != null)
                {
                    if (this.Comando.Connection.State != ConnectionState.Closed)
                    {
                        this.Comando.Connection.Close();
                    }

                    this.Comando.Connection.Dispose();
                }

                this.Comando.Dispose();
                this.Comando = null;
            }

            CurrentResult = null;
        }

        #region IDisposable Members

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.Clear();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Conexiones()
        {
            this.Dispose(false);
        }

        #endregion

        private void LogException(Exception e, string id, string aditional)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Date: {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now))
              .AppendLine(MensajesRegistros.GetFullMessage(e));
            MensajesRegistros.Error(string.Format("Host Servicios Web - {0}", id), string.Format("{0}\r\n{1}", aditional.Trim(), sb.ToString()));
        }

        private void SetParameters(IEnumerable<SqlParameter> parameters)
        {
            if (parameters != null && parameters.Count() > 0)
            {
                parameters.AsParallel()
                          .ToList()
                          .ForEach(p =>
                          {
                              if (this.Comando.Parameters.AsParallel()
                                                         .Cast<SqlParameter>()
                                                         .FirstOrDefault(q => q.ParameterName.Equals(p.ParameterName, StringComparison.OrdinalIgnoreCase)) != null)
                              {
                                  this.Comando.Parameters[p.ParameterName].Value = p.Value;
                              }
                              else
                              {
                                  this.Comando.Parameters.Add(p);
                              }
                          });
            }
        }

        private async Task<object> DoConnection(ParametrosConexion parametros, Func<Conexiones, Task<object>> fn)
        {
            object result = null;
            try
            {
                if (this.Comando == null)
                {
                    this.Comando = new SqlCommand();
                    this.Comando.Connection = new SqlConnection(ConstantesPersistencia.ConnectionString);
                }

                Task _open = null;

                if (this.Comando.Connection.State != ConnectionState.Open)
                {
                    _open = this.Comando.Connection.OpenAsync();
                    _open.ConfigureAwait(false);
                }

                if (parametros.operacion == TipoOperacion.None)
                {
                    this.Comando.CommandText = parametros.query;
                }
                else
                {
                    XMLLoader xml = new XMLLoader();
                    this.Comando.CommandText = xml.GetOperation(parametros.operacion, parametros.tabla);
                }

                this.SetParameters(parametros.parameters);

                if (_open != null && !_open.IsCompleted)
                {
                    await _open;
                }

                if (this.Comando.Transaction == null)
                {
                    this.Comando.Transaction = this.Comando.Connection.BeginTransaction();

                    try
                    {
                        result = await fn(this).ConfigureAwait(false);
                        CurrentResult = result;

                        if (this.Comando.Transaction != null)
                        {
                            this.Comando.Transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        if (this.Comando.Transaction != null)
                        {
                            this.Comando.Transaction.Rollback();
                        }
                        throw;
                    }
                }
                else
                {
                    result = await fn(this).ConfigureAwait(false);
                    CurrentResult = result;
                }
            }
            catch (AggregateException err)
            {
                err.Handle((ex) =>
                    {
                        this.IsFaulted = true;
                        this.LogException(ex, "Conexiones", parametros.tabla + " " + this.Comando.CommandText);
                        return true;
                    });
            }
            catch (BitacoraException bita)
            {
                this.IsFaulted = true;
                this.LogException(bita, "Conexiones", parametros.tabla + " " + this.Comando.CommandText);
            }
            catch (Exception e)
            {
                this.IsFaulted = true;
                this.LogException(e, "Conexiones", parametros.tabla + " " + this.Comando.CommandText);
            }
            return result;
        }

        public async Task<int> ExecuteNonQuery(ParametrosConexion parametros)
        {
            return (int)await this.ExecuteNonQuery(parametros, null).ConfigureAwait(false);
        }

        public async Task<int> ExecuteNonQuery(ParametrosConexion parametros, Action<Conexiones> fn)
        {
            return (int)await this.DoConnection(parametros, async (conexion) =>
                {
                    int result = await conexion.Comando.ExecuteNonQueryAsync().ConfigureAwait(false);
                    conexion.CurrentResult = result;
                    if (fn != null) { fn(conexion); }
                    return result;
                }).ConfigureAwait(false);
        }

        public async Task<object> ExecuteScalar(ParametrosConexion parametros)
        {
            return await this.DoConnection(parametros, null).ConfigureAwait(false);
        }

        public async Task<object> ExecuteScalar(ParametrosConexion parametros, Action<Conexiones> fn)
        {
            return await this.DoConnection(parametros, async (conexion) =>
                {
                    object result = await conexion.Comando.ExecuteScalarAsync().ConfigureAwait(false);
                    conexion.CurrentResult = result;
                    if (fn != null) { fn(conexion); }
                    return result;
                }).ConfigureAwait(false);
        }

        public async Task<object> ExecuteReader(ParametrosConexion parametros, Func<Conexiones, SqlDataReader, Task<object>> fn)
        {
            TaskCompletionSource<object> _task = new TaskCompletionSource<object>();
            await this.DoConnection(parametros, async (conexion) =>
                {
                    try
                    {
                        using (SqlDataReader reader = await conexion.Comando.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            try
                            {
                                conexion.CurrentResult = await fn(conexion, reader).ConfigureAwait(false);
                            }
                            finally
                            {
                                _task.TrySetResult(conexion.CurrentResult);
                                if ((reader != null) && (!reader.IsClosed))
                                {
                                    reader.Close();
                                }
                            }
                        }
                        return conexion.CurrentResult;
                    }
                    catch (Exception e)
                    {
                        _task.TrySetException(e);
                        throw;
                    }
                }).ConfigureAwait(false);
            return _task.Task;
        }
    }

    public class ParametrosConexion
    {
        public ParametrosConexion()
        {
            this.operacion = TipoOperacion.None;
            this.tabla = string.Empty;
            this.query = string.Empty;
        }

        public TipoOperacion operacion { get; set; }

        public string tabla { get; set; }

        public string query { get; set; }

        public IEnumerable<SqlParameter> parameters { get; set; }

        ~ParametrosConexion()
        {
            this.operacion = TipoOperacion.None;
            this.tabla = default(string);
            this.query = default(string);

            if (this.parameters != null)
            {
                parameters = Enumerable.Empty<SqlParameter>();
                parameters = null;
            }
        }
    }
}
