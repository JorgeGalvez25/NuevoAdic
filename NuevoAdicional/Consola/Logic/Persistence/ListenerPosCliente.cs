using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Configuration;
using Consola.Logic.Entities;
using System.IO.Ports;
using Consola.Connect;

namespace Consola.Logic.Persistence
{
    public class ListenerPosCliente
    {
        private static FbConnection _connection = null;
        private static FbConnection _connection2 = null;
        private static FbRemoteEvent _revent = null;
        private static FbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    FbConnectionStringBuilder sb = new FbConnectionStringBuilder(ConfigurationManager.ConnectionStrings["GasConsola"].ConnectionString);
                    {
                        sb.Pooling = false;
                    }

                    _connection = new FbConnection(sb.ToString());
                }
                return _connection;
            }
            set
            {
                if (value == null && _connection != null)
                {
                    if (_connection.State == ConnectionState.Open) { _connection.Close(); }
                    _connection.Dispose();
                }

                _connection = value;
            }
        }

        private static FbConnection Connection2
        {
            get
            {
                if (_connection2 == null)
                {
                    FbConnectionStringBuilder sb = new FbConnectionStringBuilder(ConfigurationManager.ConnectionStrings["GasConsola"].ConnectionString);
                    {
                        sb.Pooling = false;
                    }

                    _connection2 = new FbConnection(sb.ToString());
                }
                return _connection2;
            }
            set
            {
                if (value == null && _connection2 != null)
                {
                    if (_connection2.State == ConnectionState.Open) { _connection2.Close(); }
                    _connection2.Dispose();
                }

                _connection2 = value;
            }
        }
        private static FbRemoteEvent Revent
        {
            get
            {
                if (_revent == null)
                {
                    _revent = new FbRemoteEvent(Connection);
                    _revent.AddEvents(new string[] { "post_event", "Cliente0", "comandonuevo"/*, "Cliente0"*/ });
                }
                return _revent;
            }
            set
            {
                if (value == null && _revent != null)
                {
                    _revent.CancelEvents();
                }

                _revent = value;
            }
        }

        public void UnListen()
        {
            if (_revent != null)
            {
                Revent.RemoteEventCounts -= Revent_RemoteEventCounts;
                Revent.CancelEvents();
                Revent = null;
            }
            if (_connection != null)
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
        Action<string> fn;

        public void Listener(Action<string> fn)
        {
            try
            {
                if (Connection.State != ConnectionState.Open) { Connection.Open(); }
                Revent.RemoteEventCounts += (Revent_RemoteEventCounts);
                this.fn = fn;

                // Queue events
                Revent.QueueEvents();
            }
            catch //(Exception e)
            {
                throw;
            }
        }

        void Revent_RemoteEventCounts(object sender, FbRemoteEventEventArgs e)
        {
            try
            {
                if (e.Counts > 0)
                {
                    string fmt = string.Empty;
                    switch (e.Name.ToLower())
                    {
                        case "cliente1":
                            //fmt = string.Format("Nombre: {0}, Contador: {1:N0}, Cancelado: {2}", e.Name, e.Counts, e.Cancel);
                            break;
                        case "cliente0":
                        case "comandonuevo":
                            fn(e.Name);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex;
            }
        }

        private const string ACTUALIZAR_MIN = "UPDATE DPVGCONF SET " +
                                                    "POSCLIENTE = @POSCLIENTE " +
                                              "WHERE " +
                                                    "(RAZONSOCIAL = @RAZONSOCIAL OR @RAZONSOCIAL = CAST('' AS VARCHAR(80)))";

        private void DbConn(Action<FbCommand> callback)
        {
            FbConnection conn = Connection;
            {
                if (conn.State != ConnectionState.Open) { conn.Open(); }
                //using (FbTransaction transa = conn.BeginTransaction())
                {
                    try
                    {
                        using (FbCommand comm = conn.CreateCommand())
                        {
                            //comm.Transaction = transa;
                            comm.Parameters.Clear();
                            callback(comm);
                            //transa.CommitRetaining();
                        }
                    }
                    catch (Exception e)
                    {
                        var x = e;
                        //transa.RollbackRetaining();
                    }
                }
            }
        }
        public bool ActualizarPosCliente(FiltroDPVGCONF f)
        {
            bool result = false;

            //this.DbConn((comm) =>
            //    {
            //        comm.CommandText = ACTUALIZAR_MIN;
            //        comm.Parameters.Clear();
            //        comm.Parameters.Add("@POSCLIENTE", f.PosCliente);
            //        comm.Parameters.Add("@RAZONSOCIAL", f.RazonSocial.Trim());

            //        result = (comm.ExecuteNonQuery() > 0);
            //    });

            FbConnection conexion = Connection2;
            FbCommand comando = new FbCommand(ACTUALIZAR_MIN, conexion);

            comando.Parameters.Add("@POSCLIENTE", f.PosCliente);
            comando.Parameters.Add("@RAZONSOCIAL", f.RazonSocial.Trim());
            conexion.Open();
            try
            {
                result = (comando.ExecuteNonQuery() > 0);
            }
            catch //(Exception e)
            {
                result = false;
            }
            Connection2 = null;
            return result;
        }

        private const string CONSULTA_COMANDO = "SELECT FOLIO, " +
                                                       "MODULO, " +
                                                       "FECHAHORA, " +
                                                       "COMANDO," +
                                                       "APLICADO, " +
                                                       "RESULTADO " +
                                                  "FROM DPVGCMND " +
                                                 "WHERE (FOLIO =@FOLIO OR @FOLIO = 0) AND " +
                                                       "(MODULO = @MODULO OR @MODULO = CAST('' AS VARCHAR(4))) AND " +
                                                       "(APLICADO = @APLICADO OR @APLICADO = CAST('' AS VARCHAR(2))) AND " +
                                                       "(RESULTADO = @RESULTADO OR @RESULTADO = CAST('' AS VARCHAR(80)))";

        public ListaDPVGCMND ObtenerTodosComandos(FiltroDPVGCMND f)
        {
            ListaDPVGCMND resultado = new ListaDPVGCMND();

            this.DbConn((comm) =>
                {
                    comm.CommandText = CONSULTA_COMANDO;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@APLICADO", f.Aplicado);
                    comm.Parameters.Add("@FOLIO", f.Folio);
                    comm.Parameters.Add("@MODULO", f.Modulo);
                    comm.Parameters.Add("@RESULTADO", f.Resultado);

                    using (FbDataReader reader = comm.ExecuteReader())
                    {
                        DPVGCMND aux = null;
                        while (reader.Read())
                        {
                            aux = new DPVGCMND();
                            {
                                aux.Folio = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                aux.Modulo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                aux.FechaHora = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                                aux.Comando = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                aux.Aplicado = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                                aux.Resultado = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            }
                            resultado.Add(aux);
                        }
                    }

                });

            return resultado;
        }

        public SerialConnectionConfig GetSerialConfig()
        {
            SerialConnectionConfig result = new SerialConnectionConfig();
            this.DbConn((comm) =>
            {
                comm.CommandText = @"SELECT NUMEROPUERTO, VELOCIDAD, PARIDAD, BITSDATOS, BITSPARO FROM DPVGPUER WHERE CLAVE=""DISP""";

                using (FbDataReader reader = comm.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            result.PortName = reader.IsDBNull(0) ? "COM1" : "COM" + reader.GetString(0);
                            result.BaudRate = reader.IsDBNull(1) ? (BaudRate)4200 : (BaudRate)reader.GetInt32(1);
                            switch (reader.GetString(2))
                            {
                                case "Par":
                                    result.Parity = Parity.Even;
                                    break;
                                case "Impar":
                                    result.Parity = Parity.Odd;
                                    break;
                                case "Ninguna":
                                    result.Parity = Parity.None;
                                    break;
                                default:
                                    result.Parity = Parity.Even;
                                    break;
                            }
                            result.DataBits = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            result.StopBits = reader.GetInt32(4) == 1 ? result.StopBits = StopBits.One : StopBits.Two;
                        }
                    }
                    finally
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                    }
                }

            });

            return result;
        }
    }
}
