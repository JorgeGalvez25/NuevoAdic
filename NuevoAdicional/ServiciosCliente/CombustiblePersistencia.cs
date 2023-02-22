using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace ServiciosCliente
{
    public class CombustiblePersistencia
    {
        public bool CombustibleActualizar(int comb, string tag3)
        {

            bool result = false;

            string sentencia = "UPDATE DPVGTCMB SET TAG3 = @TAG3 WHERE CLAVE = @CLAVE";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@TAG3", FbDbType.VarChar).Value = tag3;
            comando.Parameters.Add("@CLAVE", FbDbType.VarChar).Value = comb;

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
    }
}
