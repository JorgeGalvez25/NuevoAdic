using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace NuevoAdicional
{
    public class UsuarioPersistencia
    {
        public Usuario ReaderToEntidad(FbDataReader reader)
        {
            Usuario pResult = new Usuario();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Nombre = reader["NOMBRE"] is System.DBNull ? string.Empty : (string)reader["NOMBRE"];
            pResult.Clave = reader["CLAVE"] is System.DBNull ? string.Empty : (string)reader["CLAVE"];
            pResult.Activo = reader["ACTIVO"] is System.DBNull ? string.Empty : (string)reader["ACTIVO"];
            pResult.Variables = reader["VARIABLES"] is System.DBNull ? string.Empty : (string)reader["VARIABLES"];

            return pResult;
        }

        public Usuario UsuarioObtener(int AId)
        {
            Usuario pResult = null;
            string sentencia = "SELECT * FROM USUARIO WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = null;
            FbTransaction transaccion = null;

            try
            {
                EstacionPersistencia estPers = new EstacionPersistencia();

                conexion.Open();
                transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
                comando = new FbCommand(sentencia, conexion, transaccion);
                comando.Parameters.Add("@ID", FbDbType.Integer).Value = AId;

                FbDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    pResult = ReaderToEntidad(reader);
                    pResult.Estaciones = estPers.EstacionObtenerPorUsuario(pResult.Id, conexion, transaccion);
                }
                transaccion.Commit();
            }
            catch (Exception e)
            {
                if (transaccion != null) { transaccion.Rollback(); }
                throw e;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public ListaUsuario ObtenerLista()
        {
            ListaUsuario pResult = new ListaUsuario();
            string sentencia = "SELECT * FROM USUARIO WHERE NOMBRE <> 'Administrador'";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = null;
            FbTransaction transaccion = null;

            try
            {
                EstacionPersistencia estPers = new EstacionPersistencia();

                conexion.Open();
                transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
                comando = new FbCommand(sentencia, conexion, transaccion);
                FbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Usuario user = ReaderToEntidad(reader);

                    user.Estaciones = estPers.EstacionObtenerPorUsuario(user.Id, conexion, transaccion);
                    pResult.Add(user);
                }
                reader.Close();
                transaccion.Commit();
            }
            catch (Exception ex)
            {
                if (transaccion != null) { transaccion.Rollback(); }
                throw ex;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public ListaUsuario ObtenerListaActivos()
        {
            ListaUsuario pResult = new ListaUsuario();
            string sentencia = "SELECT * FROM USUARIO WHERE ACTIVO = 'Si'";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pResult.Add(ReaderToEntidad(reader));
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public Usuario UsuarioInsertar(Usuario AUsuario)
        {
            Usuario pResult = null;
            string sentencia = "INSERT INTO USUARIO(ID, NOMBRE, CLAVE, ACTIVO) VALUES(@ID, @NOMBRE, @CLAVE, @ACTIVO) RETURNING ID";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AUsuario.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = AUsuario.Nombre;
            comando.Parameters.Add("@CLAVE", FbDbType.VarChar).Value = AUsuario.Clave;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = "Si";
            comando.Parameters.Add("@IDRET", FbDbType.Integer);
            comando.Parameters["@IDRET"].Direction = ParameterDirection.ReturnValue;

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

            AUsuario.Id = Convert.ToInt32(comando.Parameters["@IDRET"].Value);
            pResult = UsuarioObtener(AUsuario.Id);

            return pResult;
        }

        public Usuario UsuarioActualizar(Usuario usuario)
        {
            Usuario pResult = null;
            string sentencia = "UPDATE USUARIO SET NOMBRE = @NOMBRE, CLAVE = @CLAVE, ACTIVO = @ACTIVO, VARIABLES = @VARIABLES WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = usuario.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = usuario.Nombre;
            comando.Parameters.Add("@CLAVE", FbDbType.VarChar).Value = usuario.Clave;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = usuario.Activo;
            comando.Parameters.Add("@VARIABLES", FbDbType.VarChar).Value = usuario.Variables;

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

            pResult = UsuarioObtener(usuario.Id);

            return pResult;
        }

        public bool UsuarioEliminar(int AId)
        {
            bool pResult = true;

            string sentencia = "DELETE FROM USUARIO WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

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

            return pResult;
        }

        private bool UsuarioEliminarEstaciones(int idUsuario, FbConnection conexion, FbTransaction transaccion)
        {
            bool pResult = true;
            string sentencia = "DELETE FROM ESTACIONUSUARIO WHERE IDUSUARIO = @IDUSUARIO";
            FbCommand comando = new FbCommand(sentencia, conexion, transaccion);

            comando.Parameters.Add("@IDUSUARIO", FbDbType.Integer).Value = idUsuario;

            pResult = comando.ExecuteNonQuery() > 0;

            return pResult;
        }

        public bool UsuarioActualizarEstaciones(Usuario usuario)
        {
            bool pResult = true;

            string sentencia = "INSERT INTO ESTACIONUSUARIO (IDESTACION, IDUSUARIO) VALUES (@IDESTACION, @IDUSUARIO)";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = null;
            FbTransaction transaccion = null;

            try
            {
                conexion.Open();
                transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
                UsuarioEliminarEstaciones(usuario.Id, conexion, transaccion);

                comando = new FbCommand(sentencia, conexion, transaccion);
                comando.Parameters.Add("@IDESTACION", FbDbType.Integer);
                comando.Parameters.Add("@IDUSUARIO", FbDbType.Integer);

                foreach (Estacion item in usuario.Estaciones)
                {
                    comando.Parameters["@IDESTACION"].Value = item.Id;
                    comando.Parameters["@IDUSUARIO"].Value = usuario.Id;
                    comando.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();
                pResult = false;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public bool RevisarAgregarColumnaVariables()
        {
            bool result = false;
            string sentencia = "select count(*) from RDB$RELATION_FIELDS where RDB$SYSTEM_FLAG = 0 and upper(RDB$RELATION_NAME) = 'USUARIO' and upper(RDB$FIELD_NAME) = 'VARIABLES'";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = null;

            try
            {
                EstacionPersistencia estPers = new EstacionPersistencia();

                conexion.Open();
                comando = new FbCommand(sentencia, conexion);

                Int16 tmp = Convert.ToInt16(comando.ExecuteScalar());

                if (tmp == 0)
                {
                    //comando = new FbCommand("ALTER TABLE USUARIO ADD VARIABLES VARCHAR(1000)", conexion);
                    comando.CommandText = "ALTER TABLE USUARIO ADD VARIABLES VARCHAR(1000)";

                    comando.ExecuteNonQuery();

                    comando.CommandText = sentencia;
                    tmp = Convert.ToInt16(comando.ExecuteScalar());

                    if (tmp > 0)
                    {
                        try
                        {
                            comando.CommandText = "UPDATE USUARIO SET VARIABLES = 'Reporte 1=Si;Reporte 2=Si;Restringido a 1 día=No' WHERE ID = 1";
                            comando.ExecuteNonQuery();
                        }
                        catch (Exception) { }
                    }
                }

                result = tmp > 0;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return result;
        }

    }

}
