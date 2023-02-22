using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Persistencia;

namespace ServiciosCliente
{
    public class ConsolaPersistencia
    {
        public string ObtenerNoCentinel()
        {
            string result = null;

            string sentencia = "SELECT FIRST 1 SERIEKEY FROM DGENEMPR";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

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

        public string ObtenerRazonSocial()
        {
            string result = null;

            string sentencia = "SELECT FIRST 1 RAZONSOCIAL FROM DGENEMPR";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

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
