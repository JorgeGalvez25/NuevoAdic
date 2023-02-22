using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using Adicional.Entidades;

namespace ServiciosCliente
{
    public class EstacionConsPersistencia
    {
        public MarcaDispensario ObtenerMarcaDispensario()
        {
            string sentencia = "SELECT TIPODISPENSARIO FROM DPVGESTS";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            MarcaDispensario result = 0;
            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    if ((Int32)reader["TIPODISPENSARIO"] == 6)
                        result = ((MarcaDispensario)4);
                    else
                        result = ((MarcaDispensario)reader["TIPODISPENSARIO"]);
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
            return result;
        }

        public string ObtenerVariablesDispensario()
        {
            string sentencia = "SELECT CONSOLA FROM DPVGESTS";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            string result = "";
            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    result = ((string)reader["CONSOLA"]);
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
            return result;
        }

        public bool ActualizaVariablesDispensario(Dictionary<string, string> variables)
        {
            bool result = false;

            string memo = variables.Select(v => v.Key + "=" + v.Value).Aggregate((v1, v2) => v1 + Environment.NewLine + v2);

            string sentencia = "UPDATE DPVGESTS SET CONSOLA = @CONSOLA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@CONSOLA", FbDbType.VarChar).Value = memo;

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

        public string ObtenerNumeroEstacion()
        {
            string sentencia = "SELECT NUMEROESTACION FROM DPVGESTS";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            string result = string.Empty;
            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                }
                if (!reader.IsClosed)
                    reader.Close();
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
