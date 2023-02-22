using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ServiciosCliente
{
    public class DispensariosPersistencia
    {
        public List<int> ObtenerPosPorTipo(int tipo)
        {

            string sentencia = "SELECT DISTINCT POSCARGA FROM DPVGBOMB B WHERE COMBUSTIBLE=@TIPO" +
                  " AND (SELECT MIN(MANGUERA) FROM DPVGBOMB WHERE POSCARGA=B.POSCARGA)=MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@TIPO", FbDbType.Integer).Value = tipo;
            List<int> result = new List<int>();
            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                    result.Add((int)reader["POSCARGA"]);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return result;
        }

        public string ObtenerDispensarios()
        {
            string sentencia = "SELECT DISPENSARIOS FROM DPVGCONF";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);
            string result;
            try
            {
                conexion.Open();
                result = comando.ExecuteScalar().ToString();
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
