using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace ServiciosCliente
{
    public class ProteccionPersistencia
    {
        public bool ProteccionEliminar()
        {
            bool result = false;
            string sentencia = "DELETE FROM DKIOPROT";
            FbConnection conexion = new Conexiones().ConexionObtener("Master");
            FbCommand comando = new FbCommand(sentencia, conexion);

            try
            {
                conexion.Open();
                result = comando.ExecuteNonQuery() > 0;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return result;
        }

        private void ProteccionEliminar(FbConnection conexion, FbTransaction transaccion)
        {
            string sentencia = "DELETE FROM DKIOPROT";
            FbCommand comando = new FbCommand(sentencia, conexion, transaccion);

            comando.ExecuteNonQuery();
        }

        public int ProteccionInsertar(List<int> litros, out string mensaje)
        {
            int result = 0;
            string sentencia = "INSERT INTO DKIOPROT (LITROS) VALUES (@LITROS)";
            FbConnection conexion = new Conexiones().ConexionObtener("Master");
            FbCommand comando = null;
            FbParameter parametro = new FbParameter("@LITROS", FbDbType.Float);
            FbTransaction transaccion = null;

            mensaje = string.Empty;

            try
            {
                conexion.Open();
                transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);

                ProteccionEliminar(conexion, transaccion);
                comando = new FbCommand(sentencia, conexion, transaccion);
                comando.Parameters.Add(parametro);

                foreach (int item in litros)
                {
                    comando.Parameters[0].Value = item;

                    result += comando.ExecuteNonQuery(); 
                }

                transaccion.Commit();
            }
            catch (Exception ex)
            {
                transaccion.Rollback();

                mensaje = ex.Message;
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
