using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Adicional.Entidades;

namespace Persistencia
{
    public class EstacionPersistencia
    {
        public Estacion ReaderToEntidad(FbDataReader reader)
        {
            Estacion pResult = new Estacion();

            pResult.Id = (int)reader["ID"];
            pResult.Nombre = reader["NOMBRE"] is System.DBNull ? "" : (string)reader["NOMBRE"];
            pResult.IpServicios = reader["IPSERVICIOS"] is System.DBNull ? "" : (string)reader["IPSERVICIOS"];
            pResult.Estado = reader["ESTADO"].ToString();
            pResult.UltimoMovimiento = reader["ULTIMOMOVIMIENTO"] is System.DBNull ? DateTime.MinValue : (DateTime)reader["ULTIMOMOVIMIENTO"];
            string protecciones = reader["PROTECCIONES_ACTIVAS"] is System.DBNull ? "N" : (string)reader["PROTECCIONES_ACTIVAS"];
            pResult.TipoDispensario = (MarcaDispensario)(reader["TIPO_DISPENSARIO"] is DBNull ? 0 : Convert.ToInt32(reader["TIPO_DISPENSARIO"]));
            if (pResult.TipoDispensario == (MarcaDispensario)6)
                pResult.TipoDispensario = (MarcaDispensario)4;

            pResult.ProteccionesActivas = protecciones == "S";
            return pResult;
        }

        public Estacion EstacionObtener(int id)
        {

            Estacion pResult = null;

            string sentencia = "SELECT * FROM ESTACIONES WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = id;

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

        public ListaEstacion ObtenerLista()
        {
            ListaEstacion pResult = new ListaEstacion();

            string sentencia = "SELECT * FROM ESTACIONES";

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

        public Estacion EstacionInsertar(Estacion estacion)
        {
            Estacion pResult = null;

            string sentencia = "INSERT INTO ESTACIONES(ID, NOMBRE, IPSERVICIOS, ESTADO, ULTIMOMOVIMIENTO) VALUES(@ID, @NOMBRE, @IPSERVICIOS, @ESTADO, @ULTIMOMOVIMIENTO, @TIPO_DISPENSARIO)";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = estacion.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = estacion.Nombre;
            comando.Parameters.Add("@IPSERVICIOS", FbDbType.VarChar).Value = estacion.IpServicios;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estacion.Estado;
            comando.Parameters.Add("@ULTIMOMOVIMIENTO", FbDbType.Date).Value = estacion.UltimoMovimiento;
            comando.Parameters.Add("@TIPO_DISPENSARIO", FbDbType.Integer).Value = (int)estacion.TipoDispensario;

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

            pResult = EstacionObtener(estacion.Id);

            return pResult;
        }

        public bool EstacionActualizarEstado(string estado, DateTime ultimoMov)
        {
            bool result = false;
            string sentencia = "UPDATE ESTACIONES SET NOMBRE = @NOMBRE, IPSERVICIOS = @IPSERVICIOS, ESTADO = @ESTADO, ULTIMOMOVIMIENTO = @ULTIMOMOVIMIENTO, TIPO_DISPENSARIO = @TIPO_DISPENSARIO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estado;
            comando.Parameters.Add("@ULTIMOMOVIMIENTO", FbDbType.Date).Value = ultimoMov;

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

        public Estacion EstacionActualizar(Estacion estacion)
        {
            Estacion pResult = null;

            string sentencia = "UPDATE ESTACIONES SET NOMBRE = @NOMBRE, IPSERVICIOS = @IPSERVICIOS, ESTADO = @ESTADO, ULTIMOMOVIMIENTO = @ULTIMOMOVIMIENTO, TIPO_DISPENSARIO = @TIPO_DISPENSARIO WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = estacion.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = estacion.Nombre;
            comando.Parameters.Add("@IPSERVICIOS", FbDbType.VarChar).Value = estacion.IpServicios;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estacion.Estado;
            comando.Parameters.Add("@ULTIMOMOVIMIENTO", FbDbType.Date).Value = estacion.UltimoMovimiento;
            comando.Parameters.Add("@TIPO_DISPENSARIO", FbDbType.Integer).Value = (int)estacion.TipoDispensario;

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

            pResult = EstacionObtener(estacion.Id);

            return pResult;
        }

        public bool EstacionEliminar(int id)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM ESTACIONES WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = id;

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

        public bool EstacionActivarProtecciones(int idEstacion, bool enable)
        {
            int registros = 0;

            string sentencia = "UPDATE ESTACIONES SET PROTECCIONES_ACTIVAS = @PROTECCIONES_ACTIVAS WHERE ID = @ID";

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = idEstacion;
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

        public string ObtenerNombreEstacion()
        {
            string result = null;
            string sentencia = "SELECT NOMBRE || '|' || TIPODISPENSARIO FROM DPVGESTS";

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
