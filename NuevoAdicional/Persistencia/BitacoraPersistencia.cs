using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using Adicional.Entidades;

namespace Persistencia
{
    public class BitacoraPersistencia
    {
        public Bitacora ReaderToEntidad(FbDataReader reader)
        {
            Bitacora pResult = new Bitacora();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Id_usuario = reader["ID_USUARIO"] is System.DBNull ? string.Empty : (string)reader["ID_USUARIO"];
            pResult.Fecha = reader["FECHA"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["FECHA"];
            pResult.Hora = reader["HORA"] is System.DBNull ? TimeSpan.MinValue : (TimeSpan)reader["HORA"];
            pResult.Suceso = reader["SUCESO"] is System.DBNull ? "" : (string)reader["SUCESO"];

            return pResult;
        }

        public Bitacora BitacoraObtener(int AId)
        {
            Bitacora pResult = null;
            string sentencia = "SELECT * FROM BITACORA WHERE ID = @ID";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@ID", FbDbType.Integer).Value = AId;

                    try
                    {
                        conexion.Open();
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
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }

            return pResult;
        }

        public ListaBitacora ObtenerLista()
        {
            ListaBitacora pResult = new ListaBitacora();

            string sentencia = "SELECT * FROM BITACORA";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    try
                    {
                        conexion.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    pResult.Add(ReaderToEntidad(reader));
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            return pResult;
        }

        public ListaBitacora ObtenerListaPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            ListaBitacora pResult = new ListaBitacora();
            string sentencia = "SELECT * FROM BITACORA WHERE FECHA BETWEEN @FECHAINI AND @FECHAFIN order by fecha desc, hora desc";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@FECHAINI", FbDbType.Date).Value = fechaInicial;
                    comando.Parameters.Add("@FECHAFIN", FbDbType.Date).Value = fechaFinal;

                    try
                    {
                        conexion.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    pResult.Add(ReaderToEntidad(reader));
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }

            return pResult;
        }

        public bool UsuarioTieneMovimientosEnBitacora(string AUsuario)
        {
            bool pResult = false;

            string sentencia = "SELECT * FROM BITACORA WHERE ID_USUARIO = @ID_USUARIO ROWS 1";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@ID_USUARIO", FbDbType.VarChar).Value = AUsuario;

                    try
                    {
                        conexion.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    pResult = true;
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }

            return pResult;
        }

        public Bitacora BitacoraInsertar(Bitacora ABitacora)
        {
            Bitacora pResult = null;

            string sentencia = "INSERT INTO BITACORA(ID, ID_USUARIO, FECHA, HORA, SUCESO) VALUES(@ID, @ID_USUARIO, @FECHA, @HORA, @SUCESO)";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@ID", FbDbType.Integer).Value = ABitacora.Id;
                    comando.Parameters.Add("@ID_USUARIO", FbDbType.VarChar).Value = ABitacora.Id_usuario;
                    comando.Parameters.Add("@FECHA", FbDbType.Date).Value = DateTime.Today;
                    comando.Parameters.Add("@HORA", FbDbType.Time).Value = DateTime.Today.TimeOfDay;
                    comando.Parameters.Add("@SUCESO", FbDbType.VarChar).Value = ABitacora.Suceso;

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
                pResult = BitacoraObtener(ABitacora.Id);
            }
            return pResult;
        }

        public Bitacora BitacoraActualizar(Bitacora ABitacora)
        {

            Bitacora pResult = null;

            string sentencia = "UPDATE BITACORA SET ID_USUARIO = @ID_USUARIO, FECHA = @FECHA, HORA = @HORA, SUCESO = @SUCESO WHERE ID = @ID";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@ID", FbDbType.Integer).Value = ABitacora.Id;
                    comando.Parameters.Add("@ID_USUARIO", FbDbType.VarChar).Value = ABitacora.Id_usuario;
                    comando.Parameters.Add("@FECHA", FbDbType.Date).Value = ABitacora.Fecha;
                    comando.Parameters.Add("@HORA", FbDbType.Time).Value = ABitacora.Hora;
                    comando.Parameters.Add("@SUCESO", FbDbType.VarChar).Value = ABitacora.Suceso;

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }

                    pResult = BitacoraObtener(ABitacora.Id);
                }
            }
            return pResult;
        }

        public bool BitacoraEliminar(int AId)
        {
            bool pResult = true;
            string sentencia = "DELETE FROM BITACORA WHERE ID = @ID";

            using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
            {
                using (FbCommand comando = new FbCommand(sentencia, conexion))
                {
                    comando.Parameters.Add("@ID", FbDbType.Integer).Value = AId;

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }

            return pResult;
        }
    }
}
