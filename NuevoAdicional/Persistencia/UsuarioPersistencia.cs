using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace Persistencia
{
    public class UsuarioPersistencia
    {

        public Usuario ReaderToEntidad(FbDataReader reader)
        {

            Usuario pResult = new Usuario();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Nombre = reader["NOMBRE"] is System.DBNull ? "" : (string)reader["NOMBRE"];
            pResult.Clave = reader["CLAVE"] is System.DBNull ? "" : (string)reader["CLAVE"];
            pResult.Activo = reader["ACTIVO"] is System.DBNull ? "" : (string)reader["ACTIVO"];

            return pResult;
        }

        public Usuario UsuarioObtener(int AId)
        {

            Usuario pResult = null;

            string sentencia = "SELECT * FROM USUARIO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AId;

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    pResult = ReaderToEntidad(reader);
                }
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

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
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

        public ListaUsuario ObtenerListaActivos()
        {
            ListaUsuario pResult = new ListaUsuario();

            string sentencia = "SELECT * FROM USUARIO WHERE ACTIVO = 'Si'";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
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

            string sentencia = "INSERT INTO USUARIO(ID, NOMBRE, CLAVE, ACTIVO) VALUES(@ID, @NOMBRE, @CLAVE, @ACTIVO)";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AUsuario.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = AUsuario.Nombre;
            comando.Parameters.Add("@CLAVE", FbDbType.VarChar).Value = AUsuario.Clave;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = AUsuario.Activo;

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

            pResult = UsuarioObtener(AUsuario.Id);

            return pResult;
        }

        public Usuario UsuarioActualizar(Usuario AUsuario)
        {

            Usuario pResult = null;

            string sentencia = "UPDATE USUARIO SET NOMBRE = @NOMBRE, CLAVE = @CLAVE, ACTIVO = @ACTIVO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AUsuario.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = AUsuario.Nombre;
            comando.Parameters.Add("@CLAVE", FbDbType.VarChar).Value = AUsuario.Clave;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = AUsuario.Activo;

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

            pResult = UsuarioObtener(AUsuario.Id);

            return pResult;
        }

        public bool UsuarioEliminar(int AId)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM USUARIO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
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

    }

}
