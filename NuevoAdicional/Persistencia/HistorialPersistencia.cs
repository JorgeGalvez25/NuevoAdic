using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Adicional.Entidades;

namespace Persistencia
{
    public class HistorialPersistencia
    {

        public Historial ReaderToEntidad(FbDataReader reader)
        {

            Historial pResult = new Historial();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Id_Estacion = reader["ID_ESTACION"] is System.DBNull ? 0 : (int)reader["ID_ESTACION"];
            pResult.Fecha = reader["FECHA"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["FECHA"];
            pResult.Hora = reader["HORA"] is System.DBNull ? TimeSpan.MinValue : (TimeSpan)reader["HORA"];
            pResult.Posicion = reader["POSICION"] is System.DBNull ? 0 : (int)reader["POSICION"];
            pResult.Manguera = reader["MANGUERA"] is System.DBNull ? 0 : (int)reader["MANGUERA"];
            pResult.Porcentaje = reader["PORCENTAJE"] is System.DBNull ? 0 : (decimal)reader["PORCENTAJE"];
            pResult.Estado = reader["ESTADO"] is System.DBNull ? "" : (string)reader["ESTADO"];
            pResult.Calibracion = reader["CALIBRACION"] is System.DBNull ? 0 : (int)reader["CALIBRACION"];
            pResult.Conf = reader["CONF"] is System.DBNull ? 0 : (decimal)reader["CONF"];
            pResult.Abajo = reader["ABAJO"] is System.DBNull ? "No" : (string)reader["ABAJO"];

            try
            {
                pResult.Combustible = reader.GetInt16(8);
            }
            catch
            {
                pResult.Combustible = 0;
            }

            return pResult;
        }

        public Historial HistorialObtener(int AId)
        {

            Historial pResult = null;

            string sentencia = "SELECT * FROM HISTORIAL WHERE ID = @ID";

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

        public ListaHistorial ObtenerLista()
        {
            ListaHistorial pResult = new ListaHistorial();

            string sentencia = "SELECT * FROM HISTORIAL";

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

        public ListaHistorial ObtenerRecientes(int AEstacion)
        {
            ListaHistorial pResult = new ListaHistorial();

            string sentencia = "SELECT * FROM HISTORIAL WHERE FECHA = (SELECT MAX(FECHA) FROM HISTORIAL) AND HORA = (SELECT MAX(HORA) FROM HISTORIAL WHERE FECHA = (SELECT MAX(FECHA) FROM HISTORIAL)) AND ID_ESTACION = ID_ESTACION ORDER BY POSICION";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);
            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AEstacion;

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

        public Historial HistorialInsertar(Historial AHistorial)
        {

            Historial pResult = null;

            string sentencia = "INSERT INTO HISTORIAL(ID, ID_ESTACION, FECHA, HORA, POSICION, MANGUERA, PORCENTAJE, ESTADO, COMBUSTIBLE, CONF, CALIBRACION, ABAJO) " +
                "VALUES(@ID, @ID_ESTACION, @FECHA, @HORA, @POSICION, @MANGUERA, @PORCENTAJE, @ESTADO, @COMBUSTIBLE, @CONF, @CALIBRACION, @ABAJO)";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AHistorial.Id;
            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AHistorial.Id_Estacion;
            comando.Parameters.Add("@FECHA", FbDbType.Date).Value = AHistorial.Fecha;
            comando.Parameters.Add("@HORA", FbDbType.Time).Value = AHistorial.Hora;
            comando.Parameters.Add("@POSICION", FbDbType.Integer).Value = AHistorial.Posicion;
            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = AHistorial.Manguera;
            comando.Parameters.Add("@PORCENTAJE", FbDbType.Numeric).Value = AHistorial.Porcentaje;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = AHistorial.Estado;
            comando.Parameters.Add("@COMBUSTIBLE", FbDbType.SmallInt).Value = AHistorial.Combustible;
            comando.Parameters.Add("@CONF", FbDbType.Numeric).Value = AHistorial.Conf;
            comando.Parameters.Add("@CALIBRACION", FbDbType.Integer).Value = AHistorial.Calibracion;
            comando.Parameters.Add("@ABAJO", FbDbType.VarChar).Value = AHistorial.Abajo;

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

            if (AHistorial.Calibracion != 0)
            {
                pResult = null;
                sentencia = "UPDATE DPVGBOMB SET HJ_ADDR=@CALIBRACION WHERE POSCARGA=@POSCARGA AND CON_POSICION=@CON_POSICION";

                conexion = new Conexiones().ConexionObtener("GasConsola");
                comando = new FbCommand(sentencia, conexion);

                comando.Parameters.Add("@CALIBRACION", FbDbType.Decimal).Value = AHistorial.Calibracion == -1 ? 0 : AHistorial.Calibracion;
                comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = AHistorial.Posicion;
                comando.Parameters.Add("@CON_POSICION", FbDbType.Integer).Value = AHistorial.Manguera;


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

            pResult = HistorialObtener(AHistorial.Id);

            return pResult;
        }

        public Historial HistorialActualizar(Historial AHistorial)
        {

            Historial pResult = null;

            string sentencia = "UPDATE HISTORIAL SET ID_ESTACION = @ID_ESTACION, FECHA = @FECHA, HORA = @HORA, POSICION = @POSICION, MANGUERA = @MANGUERA, PORCENTAJE = @PORCENTAJE, ESTADO = @ESTADO, COMBUSTIBLE = @COMBUSTIBLE, CONF = @CONF, CALIBRACION = @CALIBRACION, ABAJO = @ABAJO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AHistorial.Id;
            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AHistorial.Id_Estacion;
            comando.Parameters.Add("@FECHA", FbDbType.Date).Value = AHistorial.Fecha;
            comando.Parameters.Add("@HORA", FbDbType.Time).Value = AHistorial.Hora;
            comando.Parameters.Add("@POSICION", FbDbType.Integer).Value = AHistorial.Posicion;
            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = AHistorial.Manguera;
            comando.Parameters.Add("@PORCENTAJE", FbDbType.Numeric).Value = AHistorial.Porcentaje;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = AHistorial.Estado;
            comando.Parameters.Add("@COMBUSTIBLE", FbDbType.SmallInt).Value = AHistorial.Combustible;
            comando.Parameters.Add("@CONF", FbDbType.Numeric).Value = AHistorial.Conf;
            comando.Parameters.Add("@CALIBRACION", FbDbType.Integer).Value = AHistorial.Calibracion;
            comando.Parameters.Add("@ABAJO", FbDbType.VarChar).Value = AHistorial.Abajo;

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

            if (AHistorial.Calibracion != 0)
            {
                pResult = null;
                sentencia = "UPDATE DPVGBOMB SET HJ_ADDR=@CALIBRACION WHERE POSCARGA=@POSCARGA AND CON_POSICION=@CON_POSICION";

                conexion = new Conexiones().ConexionObtener("GasConsola");
                comando = new FbCommand(sentencia, conexion);

                comando.Parameters.Add("@CALIBRACION", FbDbType.Decimal).Value = AHistorial.Calibracion == -1 ? 0 : AHistorial.Calibracion;
                comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = AHistorial.Posicion;
                comando.Parameters.Add("@CON_POSICION", FbDbType.Integer).Value = AHistorial.Manguera;

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

            pResult = HistorialObtener(AHistorial.Id);

            return pResult;
        }

        public bool HistorialEliminar(int AId)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM HISTORIAL WHERE ID = @ID";

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

        public ListaHistorial HistorialObtenerPorEstacion(int AId_Estacion)
        {
            ListaHistorial pResult = new ListaHistorial();
            string sentencia = "SELECT * FROM HISTORIAL WHERE ID_ESTACION = @ID_ESTACION ORDER BY POSICION, MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AId_Estacion;

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

        public List<int> HistorialObtenerPosiciones(int AId_Estacion)
        {
            List<int> pResult = new List<int>();
            string sentencia = "SELECT DISTINCT(POSICION) FROM HISTORIAL WHERE ID_ESTACION = ID_ESTACION";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AId_Estacion;

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pResult.Add(reader.GetInt32(0));
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public ListaHistorial HistorialObtenerPorPosicion(int AId_Estacion, int APosicion)
        {
            ListaHistorial pResult = new ListaHistorial();
            string sentencia = @"SELECT * FROM HISTORIAL WHERE ID_ESTACION = ID_ESTACION AND POSICION = @POSICION AND FECHA = (SELECT MAX(FECHA) FROM HISTORIAL WHERE ID_ESTACION = ID_ESTACION AND POSICION = @POSICION) 
                                AND HORA = (SELECT MAX(HORA) FROM HISTORIAL WHERE ID_ESTACION = ID_ESTACION AND POSICION = @POSICION AND FECHA = (SELECT MAX(FECHA) FROM HISTORIAL WHERE ID_ESTACION = ID_ESTACION AND POSICION = @POSICION)) ORDER BY MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AId_Estacion;
            comando.Parameters.Add("@POSICION", FbDbType.Integer).Value = APosicion;

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


        public ListaHistorial HistorialObtenerTodos(int AId_Estacion, int APosicion)
        {
            ListaHistorial pResult = new ListaHistorial();
            string sentencia = @"SELECT H.* " +
                                  "FROM HISTORIAL H " +
                                  "LEFT OUTER JOIN (SELECT DISTINCT(POSICION) AS POSICION " +
                                                     "FROM HISTORIAL " +
                                                    "WHERE ID_ESTACION = ID_ESTACION) P " +
                                    "ON P.POSICION = H.POSICION " +
                                 "WHERE H.ID_ESTACION = H.ID_ESTACION " +
                                   "AND (H.POSICION = @POSICION OR @POSICION = 0) " +
                                   "AND H.FECHA = (SELECT MAX(H1.FECHA) " +
                                                    "FROM HISTORIAL H1 " +
                                                   "WHERE H1.ID_ESTACION = H.ID_ESTACION " +
                                                     "AND H1.POSICION = P.POSICION) " +
                                   "AND H.HORA = (SELECT MAX(H2.HORA) " +
                                                   "FROM HISTORIAL H2 " +
                                                  "WHERE H2.ID_ESTACION = H.ID_ESTACION " +
                                                    "AND H2.POSICION = P.POSICION " +
                                                    "AND H2.FECHA = (SELECT MAX(H3.FECHA) " +
                                                                      "FROM HISTORIAL H3 " +
                                                                     "WHERE H3.ID_ESTACION = H.ID_ESTACION " +
                                                                       "AND H3.POSICION = P.POSICION)) " +
                                 "ORDER BY H.MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = AId_Estacion;
            comando.Parameters.Add("@POSICION", FbDbType.Integer).Value = APosicion;

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
    }
}
