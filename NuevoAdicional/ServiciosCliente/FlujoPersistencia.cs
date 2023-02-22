using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Adicional.Entidades;
using ServiciosCliente;

namespace Persistencia
{
    public class FlujoPersistencia
    {

        public Flujo ReaderToEntidad(FbDataReader reader)
        {

            Flujo pResult = new Flujo();

            pResult.Poscarga = reader["POSCARGA"] is System.DBNull ? 0 : (int)reader["POSCARGA"];
            pResult.Slowflow = reader["SLOWFLOW"] is System.DBNull ? 0 : (double)reader["SLOWFLOW"];
            pResult.Slowflow2 = reader["SLOWFLOW2"] is System.DBNull ? 0 : (double)reader["SLOWFLOW2"];
            pResult.Slowflow3 = reader["SLOWFLOW3"] is System.DBNull ? 0 : (double)reader["SLOWFLOW3"];

            return pResult;
        }

        public Flujo FlujoObtener(int APoscarga)
        {

            Flujo pResult = null;

            string sentencia = "SELECT * FROM DPVGPCAR WHERE POSCARGA = @POSCARGA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = APoscarga;

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

        public ListaFlujo ObtenerLista()
        {
            ListaFlujo pResult = new ListaFlujo();

            string sentencia = "SELECT * FROM DPVGPCAR";

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

        public Flujo FlujoInsertar(Flujo AFlujo){
	
		Flujo pResult = null;
	
		string sentencia = "INSERT INTO DPVGPCAR(POSCARGA, SLOWFLOW, SLOWFLOW2, SLOWFLOW3) VALUES(@POSCARGA, @SLOWFLOW, @SLOWFLOW2, @SLOWFLOW3)";
	
		FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
		FbCommand comando = new FbCommand(sentencia, conexion);
	
		comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = AFlujo.Poscarga;
		comando.Parameters.Add("@SLOWFLOW", FbDbType.Double).Value = AFlujo.Slowflow;
		comando.Parameters.Add("@SLOWFLOW2", FbDbType.Double).Value = AFlujo.Slowflow2;
		comando.Parameters.Add("@SLOWFLOW3", FbDbType.Double).Value = AFlujo.Slowflow3;
	
		try{
			conexion.Open();
	
			comando.ExecuteNonQuery();
		}
		finally{
			if (conexion.State == ConnectionState.Open)
				conexion.Close();
		}
	
		pResult = FlujoObtener(AFlujo.Poscarga);
	
		return pResult;
	}

        public Flujo FlujoActualizar(Flujo AFlujo){
	
		Flujo pResult = null;
	
		string sentencia = "UPDATE DPVGPCAR SET SLOWFLOW = @SLOWFLOW, SLOWFLOW2 = @SLOWFLOW2, SLOWFLOW3 = @SLOWFLOW3 WHERE POSCARGA = @POSCARGA";
	
		FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
		FbCommand comando = new FbCommand(sentencia, conexion);
	
		comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = AFlujo.Poscarga;
		comando.Parameters.Add("@SLOWFLOW", FbDbType.Double).Value = AFlujo.Slowflow;
		comando.Parameters.Add("@SLOWFLOW2", FbDbType.Double).Value = AFlujo.Slowflow2;
		comando.Parameters.Add("@SLOWFLOW3", FbDbType.Double).Value = AFlujo.Slowflow3;
	
		try{
			conexion.Open();
	
			comando.ExecuteNonQuery();
	
		}
		finally{
			if (conexion.State == ConnectionState.Open)
				conexion.Close();
		}
	
	 	pResult = FlujoObtener(AFlujo.Poscarga);
	
		return pResult;
	}

        public bool FlujoEliminar()
        {

            bool pResult = true;

            string sentencia = "DELETE FROM DPVGPCAR";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

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

    public class ListaFlujo : List<Flujo>
    {
    }

}
