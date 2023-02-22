using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using Adicional.Entidades;

namespace Persistencia
{
    public class DerechoPersistencia
    {

        public Derecho ReaderToEntidad(FbDataReader reader)
        {
            Derecho pResult = new Derecho();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Id_Usuario = reader["ID_USUARIO"] is System.DBNull ? 0 : (int)reader["ID_USUARIO"];
            pResult.Id_Derecho = reader["ID_DERECHO"] is System.DBNull ? 0 : (int)reader["ID_DERECHO"];
            pResult.Nombre = reader["NOMBRE"] is System.DBNull ? "" : (string)reader["NOMBRE"];

            return pResult;
        }

        public Derecho DerechoObtener(int AId)
        {
            Derecho pResult = null;

            string sentencia = "SELECT * FROM DERECHO WHERE ID = @ID";

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

        public ListaDerecho ObtenerLista()
        {
            ListaDerecho pResult = new ListaDerecho();

            string sentencia = "SELECT * FROM DERECHO";

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

        public Derecho DerechoInsertar(Derecho ADerecho)
        {

            Derecho pResult = null;

            string sentencia = "INSERT INTO DERECHO(ID, ID_USUARIO, ID_DERECHO, NOMBRE) VALUES(@ID, @ID_USUARIO, @ID_DERECHO, @NOMBRE)";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = ADerecho.Id;
            comando.Parameters.Add("@ID_USUARIO", FbDbType.Integer).Value = ADerecho.Id_Usuario;
            comando.Parameters.Add("@ID_DERECHO", FbDbType.Integer).Value = ADerecho.Id_Derecho;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = ADerecho.Nombre;

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

            pResult = DerechoObtener(ADerecho.Id);

            return pResult;
        }

        public Derecho DerechoActualizar(Derecho ADerecho)
        {

            Derecho pResult = null;

            string sentencia = "UPDATE DERECHO SET ID_USUARIO = @ID_USUARIO, ID_DERECHO = @ID_DERECHO, NOMBRE = @NOMBRE WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = ADerecho.Id;
            comando.Parameters.Add("@ID_USUARIO", FbDbType.Integer).Value = ADerecho.Id_Usuario;
            comando.Parameters.Add("@ID_DERECHO", FbDbType.Integer).Value = ADerecho.Id_Derecho;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = ADerecho.Nombre;

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

            pResult = DerechoObtener(ADerecho.Id);

            return pResult;
        }

        public bool DerechoEliminar(int AId)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM DERECHO WHERE ID = @ID";

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

        public ListaDerecho ObtenerListaPorUsuario(int AId_Usuario)
        {
            ListaDerecho pResult = new ListaDerecho();
            string sentencia = "SELECT * FROM DERECHO WHERE ID_USUARIO = @ID_USUARIO";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_USUARIO", FbDbType.Integer).Value = AId_Usuario;

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

        public bool EliminarDerechosPorUsuario(int AId_Usuario)
        {
            bool pResult = false;
            string sentencia = "DELETE FROM DERECHO WHERE ID_USUARIO = @ID_USUARIO";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_USUARIO", FbDbType.Integer).Value = AId_Usuario;

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
