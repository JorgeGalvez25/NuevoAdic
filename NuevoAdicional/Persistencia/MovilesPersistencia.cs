using System;
using System.Text;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace Persistencia
{
    public class MovilesPersistencia
    {
        private Moviles readerToEntidad(FbDataReader reader)
        {
            Moviles result = new Moviles();

            result.Telefono = reader["Telefono"] is DBNull ? string.Empty : reader["Telefono"].ToString();
            result.Responsable = reader["Responsable"] is DBNull ? string.Empty : reader["Responsable"].ToString();
            result.Password = reader["Password"] is DBNull ? string.Empty : reader["Password"].ToString();
            result.Activo = reader["Activo"] is DBNull ? string.Empty : reader["Activo"].ToString();
            string strXML = (reader["Permisos"] is DBNull) ? string.Empty : reader["Permisos"].ToString();

            if (!string.IsNullOrEmpty(strXML))
            {
                result.Permisos.FromXML(strXML);
            }
            else
            {
                result.Permisos = new Permisos(false);
            }

            return result;
        }

        public Moviles Obtener(FiltroMoviles filtro)
        {
            Moviles result = null;

            try
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
                {
                    try
                    {
                        string sentencia = "SELECT * " +
                                             "FROM MOVILES " +
                                            "WHERE ((TELEFONO = @TELEFONO) OR (@TELEFONO = CAST('' AS VARCHAR(10)))) " +
                                              "AND ((RESPONSABLE = @RESPONSABLE) OR (@RESPONSABLE = CAST('' AS VARCHAR(100)))) ";

                        using (FbCommand comando = new FbCommand(sentencia, conexion))
                        {
                            comando.Parameters.Add("@TELEFONO", FbDbType.VarChar).Value = filtro.Telefono;
                            comando.Parameters.Add("@RESPONSABLE", FbDbType.VarChar).Value = filtro.Responsable;

                            conexion.Open();
                            using (FbDataReader reader = comando.ExecuteReader())
                            {
                                try
                                {
                                    if (reader.Read())
                                    {
                                        result = readerToEntidad(reader);
                                    }
                                }
                                finally
                                {
                                    if (!reader.IsClosed)
                                        reader.Close();
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public ListaMoviles ObtenerTodos()
        {
            ListaMoviles result = new ListaMoviles();
            try
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
                {
                    try
                    {
                        string sentencia = "SELECT * FROM MOVILES";
                        using (FbCommand comando = new FbCommand(sentencia, conexion))
                        {
                            conexion.Open();
                            using (FbDataReader reader = comando.ExecuteReader())
                            {
                                try
                                {
                                    Moviles registro = null;
                                    while (reader.Read())
                                    {
                                        registro = readerToEntidad(reader);
                                        result.Add(registro);
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
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public Moviles Insertar(Moviles entidad)
        {
            Moviles result = null;

            try
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
                {
                    try
                    {
                        string sentencia = "INSERT INTO MOVILES (TELEFONO, RESPONSABLE, PASSWORD, ACTIVO, PERMISOS) VALUES (@TELEFONO, @RESPONSABLE, @PASSWORD, 'S', @PERMISOS)";

                        using (FbCommand comando = new FbCommand(sentencia, conexion))
                        {
                            comando.Parameters.Add("@TELEFONO", FbDbType.VarChar).Value = entidad.Telefono;
                            comando.Parameters.Add("@RESPONSABLE", FbDbType.VarChar).Value = entidad.Responsable;
                            comando.Parameters.Add("@PASSWORD", FbDbType.VarChar).Value = entidad.Password;
                            comando.Parameters.Add("@PERMISOS", FbDbType.Binary).Value = Encoding.UTF8.GetBytes(entidad.Permisos.ToXML());

                            conexion.Open();

                            if (comando.ExecuteNonQuery() > 0)
                                result = entidad;
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public Moviles Actualizar(Moviles entidad)
        {
            Moviles result = null;
            try
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
                {
                    try
                    {
                        string sentencia = "UPDATE MOVILES SET RESPONSABLE = @RESPONSABLE, PASSWORD = @PASSWORD, ACTIVO = @ACTIVO, PERMISOS = @PERMISOS WHERE TELEFONO = @TELEFONO";
                        using (FbCommand comando = new FbCommand(sentencia, conexion))
                        {
                            comando.Parameters.Add("@RESPONSABLE", FbDbType.VarChar).Value = entidad.Responsable;
                            comando.Parameters.Add("@PASSWORD", FbDbType.VarChar).Value = entidad.Password;
                            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = entidad.Activo;
                            comando.Parameters.Add("@TELEFONO", FbDbType.VarChar).Value = entidad.Telefono;
                            comando.Parameters.Add("@PERMISOS", FbDbType.Binary).Value = Encoding.UTF8.GetBytes(entidad.Permisos.ToXML());

                            conexion.Open();

                            if (comando.ExecuteNonQuery() > 0)
                                result = entidad;
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public bool Eliminar(string numero)
        {
            bool result = false;
            try
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener("Adicional"))
                {
                    try
                    {
                        string sentencia = "DELETE FROM MOVILES WHERE TELEFONO = @TELEFONO";
                        using (FbCommand comando = new FbCommand(sentencia, conexion))
                        {
                            comando.Parameters.Add("@TELEFONO", FbDbType.VarChar).Value = numero;
                            conexion.Open();
                            result = comando.ExecuteNonQuery() > 0;
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
