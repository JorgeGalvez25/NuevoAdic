using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using AdicionalWeb.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace AdicionalWeb.Persistencia.Enlaces
{
    internal class Conexiones
    {
        private ConnectionsStrings conexiones;

        public Conexiones()
        {
            this.conexiones = new ConnectionsStrings();
        }

        public void GasolineraConsulta(Action<FbCommand> fn)
        {
            this.GasolineraConexion(fn, false);
        }
        public void AdicionalHostConsulta(Action<FbCommand> fn)
        {
            this.AdicionalHostConexion(fn, false);
        }
        public void AdicionalClienteConsulta(Action<FbCommand> fn)
        {
            this.AdicionalClienteConexion(fn, false);
        }
        public void ConsolaConsulta(Action<FbCommand> fn, FiltroEstaciones filtro)
        {
            this.ConsolaConexion(fn, filtro, false);
        }

        public void GasolineraTransaccion(Action<FbCommand> fn)
        {
            this.GasolineraConexion(fn, true);
        }
        public void AdicionalHostConsultaTransaccion(Action<FbCommand> fn)
        {
            this.AdicionalHostConexion(fn, false);
        }
        public void AdicionalClienteConsultaTransaccion(Action<FbCommand> fn)
        {
            this.AdicionalClienteConexion(fn, true);
        }
        public void ConsolaTransaccion(Action<FbCommand> fn, FiltroEstaciones filtro)
        {
            this.ConsolaConexion(fn, filtro, true);
        }

        private void GasolineraConexion(Action<FbCommand> fn, bool withTransaccion)
        {
            using (FbCommand cmd = new FbCommand())
            {
                using (cmd.Connection = new FbConnection(this.conexiones.Gasolinera))
                {
                    try
                    {
                        cmd.Connection.Open();
                        if (withTransaccion)
                        {
                            using (cmd.Transaction = cmd.Connection.BeginTransaction())
                            {
                                try
                                {
                                    fn(cmd);
                                    cmd.Transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    cmd.Transaction.Rollback();
                                    throw e;
                                }
                            }
                        }
                        else
                        {
                            fn(cmd);
                        }
                    }
                    catch (Exception e)
                    {
                        string msj = this.WriteToEventLog(e, fn.Method.Name);
                        throw new Exception(exceptionMessage, new Exception(msj, e));
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
        private void AdicionalHostConexion(Action<FbCommand> fn, bool withTransaccion)
        {
            using (FbCommand cmd = new FbCommand())
            {
                using (cmd.Connection = new FbConnection(this.conexiones.AdicionalHost))
                {
                    try
                    {
                        cmd.Connection.Open();
                        if (withTransaccion)
                        {
                            using (cmd.Transaction = cmd.Connection.BeginTransaction())
                            {
                                try
                                {
                                    fn(cmd);
                                    cmd.Transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    cmd.Transaction.Rollback();
                                    throw e;
                                }
                            }
                        }
                        else
                        {
                            fn(cmd);
                        }
                    }
                    catch (Exception e)
                    {
                        string msj = WriteToEventLog(e, fn.Method.Name);
                        throw new Exception(exceptionMessage, new Exception(msj, e));
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
        private void AdicionalClienteConexion(Action<FbCommand> fn, bool withTransaccion)
        {
            using (FbCommand cmd = new FbCommand())
            {
                using (cmd.Connection = new FbConnection(this.conexiones.AdicionalCliente))
                {
                    try
                    {
                        cmd.Connection.Open();
                        if (withTransaccion)
                        {
                            using (cmd.Transaction = cmd.Connection.BeginTransaction())
                            {
                                try
                                {
                                    fn(cmd);
                                    cmd.Transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    cmd.Transaction.Rollback();
                                    throw e;
                                }
                            }
                        }
                        else
                        {
                            fn(cmd);
                        }
                    }
                    catch (Exception e)
                    {
                        string msj = this.WriteToEventLog(e, fn.Method.Name);
                        throw new Exception(exceptionMessage, new Exception(msj, e));
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
        private void ConsolaConexion(Action<FbCommand> fn, FiltroEstaciones filtro, bool withTransaccion)
        {
            using (FbCommand cmd = new FbCommand())
            {
                using (cmd.Connection = new FbConnection(this.ConsolaGetIP(filtro)))
                {
                    try
                    {
                        cmd.Connection.Open();
                        if (withTransaccion)
                        {
                            using (cmd.Transaction = cmd.Connection.BeginTransaction())
                            {
                                try
                                {
                                    fn(cmd);
                                    cmd.Transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    cmd.Transaction.Rollback();
                                    throw e;
                                }
                            }
                        }
                        else
                        {
                            fn(cmd);
                        }
                    }
                    catch (Exception e)
                    {
                        string msj = this.WriteToEventLog(e, fn.Method.Name);
                        throw new Exception(exceptionMessage, new Exception(msj, e));
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }

        private const string exceptionMessage = "Se ha producido una excepción. Por favor, compruebe el registro de eventos";

        private string WriteToEventLog(Exception e, string action)
        {
            StringBuilder message = new StringBuilder()
                .AppendLine("Se produjo una excepción comunicación con la fuente de datos.").AppendLine().AppendLine()
                .AppendLine("Acción: " + action).AppendLine().AppendLine()
                .AppendLine("Excepción: " + e.ToString()).AppendLine().AppendLine()
                .AppendLine("Pila: " + e.StackTrace);

            try
            {
                using (EventLog log = new EventLog())
                {
                    log.Source = "AdicionalWeb";
                    log.Log = "Application";
                    log.WriteEntry(message.ToString());
                }
            }
            catch
            { }
            return message.ToString();
        }

        private string ConsolaGetIP(FiltroEstaciones filtro)
        {
            string gasConsola = this.conexiones.GasConsola(filtro.Clave);
            //bdconsola
            FbConnectionStringBuilder sb = new FbConnectionStringBuilder();
            {
                sb.ConnectionString = gasConsola;
                if (string.IsNullOrEmpty(gasConsola))
                {
                    sb.UserID = "SYSDBA";
                    sb.Password = "masterkey";
                }
                EstacionesPersistencia servicio = new EstacionesPersistencia();
                var f = filtro.Clone();
                f.ClaveConsola = "bdconsola";
                var aux = servicio.EstacionObtenerConsola(f);

                if (!string.IsNullOrEmpty(aux))
                {
                    string[] splt = aux.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splt.Length == 2)
                    {
                        sb.DataSource = splt[0].Trim();
                        sb.Database = splt[1].Trim();
                    }
                }
            }

            return sb.ToString();
        }

        private class ConnectionsStrings
        {
            internal string _gasolinera;
            internal string _adicionalHost;
            internal string _adicionalCliente;

            public string GasConsola(int est)
            {
                return findConnectionString("GasConsola" + est.ToString("D2"));
            }

            public string Gasolinera
            {
                get
                {
                    if (string.IsNullOrEmpty(_gasolinera))
                    {
                        this._gasolinera = findConnectionString("Gasolinera");
                    }

                    return _gasolinera;
                }
            }
            public string AdicionalHost
            {
                get
                {
                    if (string.IsNullOrEmpty(this._adicionalHost))
                    {
                        this._adicionalHost = findConnectionString("AdicionalHost");
                    }

                    return _adicionalHost;
                }
            }
            public string AdicionalCliente
            {
                get
                {
                    if (string.IsNullOrEmpty(_adicionalCliente))
                    {
                        this._adicionalCliente = findConnectionString("AdicionalCliente");
                    }

                    return _adicionalCliente;
                }
            }

            internal string findConnectionString(string name)
            {
                var conf = ConfigurationManager.ConnectionStrings[name];
                return (conf == null ? string.Empty : conf.ConnectionString);
            }
        }
    }
}
