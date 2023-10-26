using System;
using System.Configuration;
using System.Data;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;
using Persistencia;
using System.Data.SqlClient;

namespace Persistencia
{
    public class TicketsPersistencia
    {
        public string Usuario;
        private string _getStringConnection;
        private string GetStringConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_getStringConnection))
                {
                    _getStringConnection = ConfigurationManager.ConnectionStrings["GasConsola"].ConnectionString;
                }

                return _getStringConnection;
            }
        }

        private Ticket ReaderToEntidad(FbDataReader reader)
        {
            Ticket pResult = new Ticket();

            pResult.Folio = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            pResult.Precio = reader.IsDBNull(1) ? 0D : reader.GetDouble(1);
            pResult.Volumen = reader.IsDBNull(2) ? 0D : reader.GetDouble(2);
            pResult.Importe = reader.IsDBNull(3) ? 0D : reader.GetDouble(3);
            pResult.Facturado = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);

            return pResult;
        }

        public Ticket TicketObtener(FiltroTicket filtro)
        {
            Ticket pResult = null;
            string sentencia = @"SELECT FOLIO,
	                                    PRECIO,
	                                    VOLUMEN,
	                                    IMPORTE,
                                        FACTURADO
                                   FROM DPVGMOVI
                                  WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = filtro.Folio;

                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    try
                                    {
                                        if (reader.Read())
                                        {
                                            pResult = ReaderToEntidad(reader);
                                        }
                                    }
                                    finally
                                    {
                                        if (!reader.IsClosed) { reader.Close(); }
                                    }
                                }
                            }
                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public int TicketConsecutivo()
        {
            int pResult = 0;
            string sentencia = @"SELECT MAX(FOLIO)
                                   FROM DPVGMOVI";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    using (FbCommand comando = conexion.CreateCommand())
                    {
                        comando.CommandText = sentencia;
                        conexion.Open();

                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    pResult = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed) { reader.Close(); }
                            }
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult + 1;
        }

        public bool TicketActualizar(Ticket entidad)
        {
            bool pResult = false;
            string sentencia = @"UPDATE DPVGMOVI
	                                SET VOLUMEN = @VOLUMEN,
                                        PRECIO = @PRECIO,
		                                IMPORTE = @IMPORTE,
		                                TAG = @TAG
                                  WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Double).Value = entidad.Folio;
                                comando.Parameters.Add("@VOLUMEN", FbDbType.Double).Value = entidad.Volumen;
                                comando.Parameters.Add("@PRECIO", FbDbType.Double).Value = entidad.Precio;
                                comando.Parameters.Add("@IMPORTE", FbDbType.Double).Value = entidad.Importe;
                                comando.Parameters.Add("@TAG", FbDbType.Integer).Value = entidad.NoAplicar ? 5 : 0;

                                pResult = comando.ExecuteNonQuery() > 0;
                            }

                            BitacoraPersistencia servicio = new BitacoraPersistencia();
                            servicio.BitacoraInsertar(new Bitacora()
                                {
                                    Fecha = DateTime.Now.Date,
                                    Hora = DateTime.Now.TimeOfDay,
                                    Id_usuario = Usuario,
                                    Suceso = string.Format("Actualizado Ticket {0:D6}", entidad.Folio)
                                });

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public Ticket TicketRegistrar(Ticket entidad)
        {
            entidad = ObtenerComplementos(entidad);

            Ticket pResult = null;
            string sentencia = @"INSERT INTO DPVGMOVI
                                        (FOLIO,
                                         FECHA,
                                         HORA,
                                         POSCARGA,
                                         COMBUSTIBLE,
                                         VOLUMEN,
                                         IMPORTE,
                                         IMPRESO,
                                         APLICAR,
                                         HORASTR,
                                         FECHACORTE,
                                         CORTE,
                                         PRECIO,
                                         TOTAL01,
                                         TOTAL02,
                                         FACTURADO,
                                         TAG,
                                         TOTAL03,
                                         TOTAL04,
                                         TIPOPAGO,
                                         REFERENCIABITACORA,
                                         CUPONIMPRESO)
                                 VALUES (@FOLIO,
                                         @FECHA,
                                         @HORA,
                                         @POSCARGA,
                                         @COMBUSTIBLE,
                                         @VOLUMEN,
                                         @IMPORTE,
                                         @IMPRESO,
                                         @APLICAR,
                                         @HORASTR,
                                         @FECHACORTE,
                                         @CORTE,
                                         @PRECIO,
                                         @TOTAL01,
                                         @TOTAL02,
                                         @FACTURADO,
                                         @TAG,
                                         @TOTAL03,
                                         @TOTAL04,
                                         @TIPOPAGO,
                                         @REFERENCIABITACORA,
                                         @CUPONIMPRESO)";
            string senActualizar = @"UPDATE DPVGMOVI
	                                    SET IMPRESO = @IMPRESO,
                                            IDTRANSACCIONOG = @IDTRANSACCIONOG
                                      WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            bool flgOk = false;
                            int folio = this.TicketConsecutivo();

                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = folio;
                                if (entidad.Fecha > DateTime.MinValue)
                                {
                                    comando.Parameters.Add("@FECHA", FbDbType.Date).Value = entidad.Fecha.Date;
                                    comando.Parameters.Add("@HORA", FbDbType.Date).Value = entidad.Fecha;
                                }
                                else
                                {
                                    comando.Parameters.Add("@FECHA", FbDbType.Date).Value = DateTime.Now.Date;
                                    comando.Parameters.Add("@HORA", FbDbType.Date).Value = DateTime.Now;
                                }
                                comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = entidad.Posicion;
                                comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = entidad.Combustible;
                                comando.Parameters.Add("@VOLUMEN", FbDbType.Double).Value = entidad.Volumen;
                                comando.Parameters.Add("@IMPORTE", FbDbType.Double).Value = entidad.Importe;
                                comando.Parameters.Add("@IMPRESO", FbDbType.VarChar).Value = "No";
                                comando.Parameters.Add("@APLICAR", FbDbType.VarChar).Value = "No";
                                comando.Parameters.Add("@HORASTR", FbDbType.VarChar).Value = DateTime.Now.ToString("HH:mm:ss");
                                comando.Parameters.Add("@FECHACORTE", FbDbType.Date).Value = entidad.Fecha.Date;
                                comando.Parameters.Add("@CORTE", FbDbType.Integer).Value = 1;
                                comando.Parameters.Add("@PRECIO", FbDbType.Double).Value = entidad.Precio;
                                comando.Parameters.Add("@TOTAL01", FbDbType.Double).Value = entidad.Total01;
                                comando.Parameters.Add("@TOTAL02", FbDbType.Double).Value = entidad.Total02;
                                comando.Parameters.Add("@FACTURADO", FbDbType.VarChar).Value = "No";
                                comando.Parameters.Add("@TAG", FbDbType.Integer).Value = entidad.NoAplicar ? 5 : 0;
                                comando.Parameters.Add("@TOTAL03", FbDbType.Double).Value = 0D;
                                comando.Parameters.Add("@TOTAL04", FbDbType.Double).Value = 0D;
                                comando.Parameters.Add("@TIPOPAGO", FbDbType.Integer).Value = 0;
                                comando.Parameters.Add("@REFERENCIABITACORA", FbDbType.Integer).Value = 0;
                                comando.Parameters.Add("@CUPONIMPRESO", FbDbType.VarChar).Value = "No";

                                flgOk = comando.ExecuteNonQuery() > 0;
                            }

                            if (flgOk)
                            {
                                int nuevoIdTransaccion = 0;

                                if (ConfigurationManager.AppSettings["ConexionOG"] != string.Empty)
                                {
                                    using (var connection = new SqlConnection(ConfigurationManager.AppSettings["ConexionOG"]))
                                    {
                                        try
                                        {
                                            connection.Open();
                                            var command = new SqlCommand("SELECT IdManguera FROM Manguera Where IdDispensario=@IdDispensario and NoManguera=@NoManguera", connection);
                                            command.Parameters.AddWithValue("@IdDispensario", entidad.Posicion);
                                            command.Parameters.AddWithValue("@NoManguera", entidad.ConPosicion);
                                            var reader = command.ExecuteReader();

                                            short IdManguera = 0;
                                            if (reader.Read())
                                                IdManguera = reader.GetInt16(0);
                                            else
                                                IdManguera = (short)entidad.Manguera;
                                            reader.Close();
                                            connection.Close();

                                            command = new SqlCommand("OG_TransaccionInsertar", connection);
                                            command.CommandType = CommandType.StoredProcedure;

                                            command.Parameters.AddWithValue("@IdTransaccion", 0);
                                            if (entidad.Fecha > DateTime.MinValue)
                                                command.Parameters.AddWithValue("@FechaHora", entidad.Fecha);
                                            else
                                                command.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                                            command.Parameters.AddWithValue("@IdManguera", IdManguera);
                                            command.Parameters.AddWithValue("@Volumen", entidad.Volumen);
                                            command.Parameters.AddWithValue("@PrecioUnitario", entidad.Precio);
                                            command.Parameters.AddWithValue("@Importe", entidad.Importe);
                                            command.Parameters.AddWithValue("@TotalizadorAnterior", 0);
                                            command.Parameters.AddWithValue("@TotalizadorFinal", 0);
                                            command.Parameters.AddWithValue("@Tipo", 4);
                                            command.Parameters.AddWithValue("@Impreso", true);
                                            command.Parameters.AddWithValue("@IdMedioEntrega", string.Empty);
                                            command.Parameters.AddWithValue("@IdSistemaMedicion", string.Empty);
                                            command.Parameters.AddWithValue("@IdHidrocarburoPetrolifero", entidad.ConProductoPrecio);
                                            command.Parameters.Add("@NuevoIdTransaccion", SqlDbType.Int);
                                            command.Parameters["@NuevoIdTransaccion"].Direction = ParameterDirection.Output;

                                            connection.Open();
                                            command.ExecuteNonQuery();
                                            connection.Close();

                                            nuevoIdTransaccion = (int)command.Parameters["@NuevoIdTransaccion"].Value;
                                        }
                                        finally
                                        {
                                            if (connection.State == ConnectionState.Open)
                                                connection.Close();
                                        }
                                    }
                                }

                                using (FbCommand comando = new FbCommand(senActualizar, conexion, transaccion))
                                {
                                    comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = folio;
                                    comando.Parameters.Add("@IMPRESO", FbDbType.VarChar).Value = "Si";
                                    comando.Parameters.Add("@IDTRANSACCIONOG", FbDbType.Integer).Value = nuevoIdTransaccion;

                                    flgOk = comando.ExecuteNonQuery() > 0;
                                }
                            }

                            if (flgOk)
                            {
                                pResult = new Ticket()
                                {
                                    Folio = folio
                                };
                            }
                            else
                            {
                                pResult = null;
                            }

                            BitacoraPersistencia servicio = new BitacoraPersistencia();
                            servicio.BitacoraInsertar(new Bitacora()
                                {
                                    Fecha = DateTime.Now.Date,
                                    Hora = DateTime.Now.TimeOfDay,
                                    Id_usuario = Usuario,
                                    Suceso = string.Format("Nuevo Ticket {0:D6}", pResult.Folio)
                                });

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        private Ticket ObtenerComplementos(Ticket entidad)
        {
            string sentencia = @"SELECT (SELECT TURNO
                                           FROM DPVGTURN
                                          WHERE FECHA = (SELECT MAX(FECHA)
                                                           FROM DPVGTURN
                                                          WHERE ESTATUS = 'A')
                                            AND ESTATUS = 'A') AS TURNO,
                                        (SELECT FECHA
                                           FROM DPVGTURN
                                          WHERE FECHA = (SELECT MAX(FECHA)
                                                           FROM DPVGTURN
                                                          WHERE ESTATUS = 'A')
                                            AND ESTATUS = 'A') AS FECHATURNO,
                                        B.MANGUERA,
                                        B.CON_POSICION,
                                        C.CON_PRODUCTOPRECIO,
                                        (SELECT FIRST(1) TOTAL01
                                           FROM DPVGMOVI
                                          WHERE FECHA = (SELECT FIRST(1) MAX(FECHA)
                                                           FROM DPVGMOVI)) AS TOTAL01,
                                        (SELECT FIRST(1) TOTAL02
                                           FROM DPVGMOVI
                                          WHERE FECHA = (SELECT FIRST(1) MAX(FECHA)
                                                           FROM DPVGMOVI)) AS TOTAL02
                                   FROM RDB$DATABASE M
                                   LEFT OUTER JOIN DPVGBOMB B
                                     ON B.POSCARGA = @POSCARGA
                                    AND B.COMBUSTIBLE = @COMBUSTIBLE
                                   LEFT OUTER JOIN DPVGTCMB C
                                     ON B.COMBUSTIBLE=C.CLAVE";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = entidad.Folio;
                                comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = entidad.Posicion;
                                comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = entidad.Combustible;

                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    try
                                    {
                                        if (reader.Read())
                                        {
                                            if (entidad == null)
                                            {
                                                entidad = new Ticket();
                                            }
                                            entidad.Turno = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                            entidad.FechaHoraTurno = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
                                            entidad.Manguera = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                            entidad.ConPosicion = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                                            entidad.ConProductoPrecio = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                                            entidad.Total01 = reader.IsDBNull(5) ? 0D : reader.GetDouble(5);
                                            entidad.Total02 = reader.IsDBNull(6) ? 0D : reader.GetDouble(6);
                                        }
                                    }
                                    finally
                                    {
                                        if (!reader.IsClosed) { reader.Close(); }
                                    }
                                }
                            }
                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return entidad;
        }

        public ListaCombustible ObtenerCombustibles()
        {
            string sentencia = @"SELECT CLAVE,
                                        NOMBRE,
                                        PRECIOFISICO,
                                       (SELECT MAX(POSCARGA) FROM DPVGBOMB)
                                   FROM DPVGTCMB";
            ListaCombustible pResult = new ListaCombustible();
            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    try
                                    {
                                        Combustible combustible = null;
                                        while (reader.Read())
                                        {
                                            combustible = new Combustible();
                                            combustible.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                            combustible.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                            combustible.Precio = reader.IsDBNull(2) ? 0D : reader.GetDouble(2);
                                            combustible.MaxPosCarga = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);

                                            pResult.Add(combustible);
                                        }
                                    }
                                    finally
                                    {
                                        if (!reader.IsClosed) { reader.Close(); }
                                    }
                                }
                            }
                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }
    }
}
