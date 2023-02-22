using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace Persistencia
{
    public class ConfiguracionPersistencia
    {

        public Configuracion ReaderToEntidad(FbDataReader reader)
        {

            Configuracion pResult = new Configuracion();

            pResult.Id = reader["ID"] is System.DBNull ? 0 : (int)reader["ID"];
            pResult.Cantidad_minima = reader["CANTIDAD_MINIMA"] is System.DBNull ? 0 : (decimal)reader["CANTIDAD_MINIMA"];
            string protecciones = reader["PROTECCIONES_ACTIVAS"] is System.DBNull ? "N" : (string)reader["PROTECCIONES_ACTIVAS"];
            pResult.Estado = reader["ESTADO"] is System.DBNull ? "Mínimo" : (string)reader["ESTADO"];
            pResult.UltimoMovimiento = reader["ULTIMOMOVIMIENTO"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["ULTIMOMOVIMIENTO"];
            pResult.UltimaSincro = reader["ULTIMASINCRO"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["ULTIMASINCRO"];
            pResult.HoraSincro = reader["HORASINCRO"] is System.DBNull ? TimeSpan.MinValue : (TimeSpan)reader["HORASINCRO"];

            pResult.ProteccionesActivas = protecciones == "S";

            return pResult;
        }

        public Configuracion ConfiguracionObtener(int AId)
        {

            Configuracion pResult = null;

            string sentencia = "SELECT * FROM CONFIGURACIONES WHERE ID = @ID";

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

        public List<Configuracion> ConfiguracionesObtener()
        {

            List<Configuracion> pResult = new List<Configuracion>();

            string sentencia = "SELECT * FROM CONFIGURACIONES";

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

        public ListaConfiguracion ObtenerLista()
        {
            ListaConfiguracion pResult = new ListaConfiguracion();

            string sentencia = "SELECT * FROM CONFIGURACIONES";

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

        public Configuracion ConfiguracionInsertar(Configuracion AConfiguracion)
        {

            Configuracion pResult = null;

            string sentencia = "INSERT INTO CONFIGURACIONES(ID, CANTIDAD_MINIMA) VALUES(@ID, @CANTIDAD_MINIMA)";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AConfiguracion.Id;
            comando.Parameters.Add("@CANTIDAD_MINIMA", FbDbType.Numeric).Value = AConfiguracion.Cantidad_minima;

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

            pResult = ConfiguracionObtener(AConfiguracion.Id);

            return pResult;
        }

        public Configuracion ConfiguracionActualizar(Configuracion AConfiguracion)
        {

            Configuracion pResult = null;

            string sentencia = "UPDATE CONFIGURACIONES SET CANTIDAD_MINIMA = @CANTIDAD_MINIMA WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = AConfiguracion.Id;
            comando.Parameters.Add("@CANTIDAD_MINIMA", FbDbType.Numeric).Value = AConfiguracion.Cantidad_minima;

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

            pResult = ConfiguracionObtener(AConfiguracion.Id);

            return pResult;
        }

        public bool ConfiguracionEliminar(int AId)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM CONFIGURACIONES WHERE ID = @ID";

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

        public bool ConfiguracionActivarProtecciones(int idConfiguracion, bool enable)
        {
            int registros = 0;

            string sentencia = "UPDATE CONFIGURACIONES SET PROTECCIONES_ACTIVAS = @PROTECCIONES_ACTIVAS WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = idConfiguracion;
            comando.Parameters.Add("@PROTECCIONES_ACTIVAS", FbDbType.VarChar).Value = enable ? "S" : "N";

            try
            {
                conexion.Open();

                registros = comando.ExecuteNonQuery();
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return registros > 0;
        }

        public bool ConfiguracionCambiarEstado(string estado)
        {
            int registros = 0;

            string sentencia = "UPDATE CONFIGURACIONES SET ESTADO = @ESTADO";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estado;

            try
            {
                conexion.Open();

                registros = comando.ExecuteNonQuery();
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return registros > 0;
        }

        public DateTime ConfiguracionActualizarUltimoMovimiento(DateTime fecha)
        {
            int registros = 0;

            string sentencia = "UPDATE CONFIGURACIONES SET ULTIMOMOVIMIENTO = @FECHA";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FECHA", FbDbType.Date).Value = DateTime.Now.Date;

            try
            {
                conexion.Open();

                registros = comando.ExecuteNonQuery();
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return registros > 0 ? fecha.Date : DateTime.MinValue;
        }

        public DateTime ConfiguracionActualizarUltimaSincronizacion(DateTime fecha)
        {
            int registros = 0;

            string sentencia = "UPDATE CONFIGURACIONES SET ULTIMASINCRO = @FECHA, HORASINCRO = @HORA";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@FECHA", FbDbType.Date).Value = DateTime.Now;
            comando.Parameters.Add("@HORA", FbDbType.Time).Value = DateTime.Now;

            try
            {
                conexion.Open();

                registros = comando.ExecuteNonQuery();
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return registros > 0 ? fecha.Date : DateTime.MinValue;
        }

    }

    public class ListaConfiguracion : List<Configuracion>
    {
    }

}
