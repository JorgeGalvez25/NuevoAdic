using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Adicional.Entidades;

namespace ServiciosCliente
{
    public class ComandosPersistencia
    {

        public Comandos ReaderToEntidad(FbDataReader reader)
        {
            Comandos pResult = new Comandos();

            pResult.Folio = reader["FOLIO"] is System.DBNull ? 0 : (int)reader["FOLIO"];
            pResult.Modulo = reader["MODULO"] is System.DBNull ? "" : (string)reader["MODULO"];
            pResult.Fechahora = reader["FECHAHORA"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["FECHAHORA"];
            pResult.Comando = reader["COMANDO"] is System.DBNull ? "" : (string)reader["COMANDO"];
            pResult.Aplicado = reader["APLICADO"] is System.DBNull ? "" : (string)reader["APLICADO"];
            pResult.Resultado = reader["RESULTADO"] is System.DBNull ? "" : (string)reader["RESULTADO"];

            return pResult;
        }

        public Comandos ComandosObtener(int AFolio)
        {

            Comandos pResult = null;

            string sentencia = "SELECT * FROM DPVGCMND WHERE FOLIO = @FOLIO";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = AFolio;

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

        public int ComandoMax()
        {

            int pResult = 0;

            string sentencia = "SELECT MAX(FOLIO)+1 AS FOLIO FROM DPVGCMND";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    pResult = (int)reader["FOLIO"];
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public ListaComandos ObtenerLista()
        {
            ListaComandos pResult = new ListaComandos();

            string sentencia = "SELECT * FROM DPVGCMND";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
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

        public Comandos ComandosInsertar(Comandos AComandos)
        {

            Comandos pResult = null;

            string sentencia = "INSERT INTO DPVGCMND(FOLIO, MODULO, FECHAHORA, COMANDO, APLICADO, RESULTADO) VALUES(@FOLIO, @MODULO, @FECHAHORA, @COMANDO, @APLICADO, @RESULTADO)";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = ComandoMax();
            comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = AComandos.Modulo;
            comando.Parameters.Add("@FECHAHORA", FbDbType.Date).Value = System.DateTime.Now;
            comando.Parameters.Add("@COMANDO", FbDbType.VarChar).Value = AComandos.Comando;
            comando.Parameters.Add("@APLICADO", FbDbType.VarChar).Value = AComandos.Aplicado;
            comando.Parameters.Add("@RESULTADO", FbDbType.VarChar).Value = AComandos.Resultado;

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

            pResult = ComandosObtener(AComandos.Folio);

            return pResult;
        }

        public Comandos ComandosActualizar(Comandos AComandos)
        {

            Comandos pResult = null;

            string sentencia = "UPDATE DPVGCMND SET MODULO = @MODULO, FECHAHORA = @FECHAHORA, COMANDO = @COMANDO, APLICADO = @APLICADO, RESULTADO = @RESULTADO WHERE FOLIO = @FOLIO";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = AComandos.Folio;
            comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = AComandos.Modulo;
            comando.Parameters.Add("@FECHAHORA", FbDbType.Date).Value = AComandos.Fechahora;
            comando.Parameters.Add("@COMANDO", FbDbType.VarChar).Value = AComandos.Comando;
            comando.Parameters.Add("@APLICADO", FbDbType.VarChar).Value = AComandos.Aplicado;
            comando.Parameters.Add("@RESULTADO", FbDbType.VarChar).Value = AComandos.Resultado;

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

            pResult = ComandosObtener(AComandos.Folio);

            return pResult;
        }

        public bool ComandosEliminar(int AFolio)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM DPVGCMND WHERE FOLIO = @FOLIO";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = AFolio;

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

        public Comandos ComandoInsertarReturning(Comandos AComandos)
        {

            Comandos pResult = null;
            string sentencia = "INSERT INTO DPVGCMND(MODULO, FECHAHORA, COMANDO, APLICADO, RESULTADO) VALUES(@MODULO, @FECHAHORA, @COMANDO, @APLICADO, @RESULTADO) RETURNING FOLIO";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = AComandos.Modulo;
            comando.Parameters.Add("@FECHAHORA", FbDbType.Date).Value = AComandos.Fechahora;
            comando.Parameters.Add("@COMANDO", FbDbType.VarChar).Value = AComandos.Comando;
            comando.Parameters.Add("@APLICADO", FbDbType.VarChar).Value = AComandos.Aplicado;
            comando.Parameters.Add("@RESULTADO", FbDbType.VarChar).Value = AComandos.Resultado;
            FbParameter outParameter = new FbParameter(@"FOLIO", FbDbType.Integer);
            outParameter.Direction = ParameterDirection.Output;
            comando.Parameters.Add(outParameter);

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

            pResult = ComandosObtener((int)outParameter.Value);

            return pResult;
        }

        public void ActualizaComando1(string AComando)
        {

            string sentencia = "UPDATE DPVGCONF SET POSCLIENTE = @COMANDO1";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@COMANDO1", FbDbType.VarChar).Value = AComando;

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
    

    public class ListaComandos : List<Comandos>
    {
    }

}
