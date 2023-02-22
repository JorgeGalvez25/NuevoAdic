using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using Adicional.Entidades;

namespace NuevoAdicional
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
            pResult.TipoDispensario = (MarcaDispensario)(reader["TIPODISPENSARIO"] is DBNull ? 0 : Convert.ToInt32(reader["TIPODISPENSARIO"]));
            if (pResult.TipoDispensario == (MarcaDispensario)6)
                pResult.TipoDispensario = (MarcaDispensario)4;
            pResult.ProteccionesActivas = protecciones == "S";
            return pResult;
        }

        public Estacion EstacionObtener(int AId)
        {
            Estacion pResult = null;
            string sentencia = "SELECT * FROM ESTACIONES WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
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

        public ListaEstacion ObtenerLista()
        {
            ListaEstacion pResult = new ListaEstacion();
            string sentencia = "SELECT * FROM ESTACIONES";

            FbConnection conexion = FBLocal.ConexionObtener();
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
            string sentencia = "INSERT INTO ESTACIONES(ID, NOMBRE, IPSERVICIOS, ESTADO, ULTIMOMOVIMIENTO, TIPODISPENSARIO) VALUES(@ID, @NOMBRE, @IPSERVICIOS, @ESTADO, @ULTIMOMOVIMIENTO, @TIPODISPENSARIO)";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = estacion.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = estacion.Nombre;
            comando.Parameters.Add("@IPSERVICIOS", FbDbType.VarChar).Value = estacion.IpServicios;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estacion.Estado;
            comando.Parameters.Add("@ULTIMOMOVIMIENTO", FbDbType.Date).Value = estacion.UltimoMovimiento;
            comando.Parameters.Add("@TIPODISPENSARIO", FbDbType.Integer).Value = (int)estacion.TipoDispensario;

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

        public Estacion EstacionActualizar(Estacion estacion)
        {
            Estacion pResult = null;
            string sentencia = "UPDATE ESTACIONES SET NOMBRE = @NOMBRE, IPSERVICIOS = @IPSERVICIOS, ESTADO = @ESTADO, ULTIMOMOVIMIENTO = @ULTIMOMOVIMIENTO, TIPODISPENSARIO = @TIPODISPENSARIO WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@ID", FbDbType.Integer).Value = estacion.Id;
            comando.Parameters.Add("@NOMBRE", FbDbType.VarChar).Value = estacion.Nombre;
            comando.Parameters.Add("@IPSERVICIOS", FbDbType.VarChar).Value = estacion.IpServicios;
            comando.Parameters.Add("@ESTADO", FbDbType.VarChar).Value = estacion.Estado;
            comando.Parameters.Add("@ULTIMOMOVIMIENTO", FbDbType.Date).Value = estacion.UltimoMovimiento;
            comando.Parameters.Add("@TIPODISPENSARIO", FbDbType.Integer).Value = (int)estacion.TipoDispensario;

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

        public bool EstacionEliminar(int AId)
        {
            bool pResult = true;
            string sentencia = "DELETE FROM ESTACIONES WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
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

        public bool EstacionActivarProtecciones(int idEstacion, bool enable)
        {
            int registros = 0;
            string sentencia = "UPDATE ESTACIONES SET PROTECCIONES_ACTIVAS = @PROTECCIONES_ACTIVAS WHERE ID = @ID";

            FbConnection conexion = FBLocal.ConexionObtener();
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

        public ListaEstacion EstacionObtenerPorUsuario(int idUsuario)
        {
            ListaEstacion pResult = new ListaEstacion();
            string sentencia = "Select e.* from ESTACIONUSUARIO eu join ESTACIONES e on eu.IDESTACION = e.ID where eu.IDUSUARIO = @IDUSUARIO";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            try
            {
                comando.Parameters.Add("@IDUSUARIO", FbDbType.Integer).Value = idUsuario;
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

        public ListaEstacion EstacionObtenerPorUsuario(int idUsuario, FbConnection conexion, FbTransaction transaccion)
        {
            ListaEstacion pResult = new ListaEstacion();
            string sentencia = "Select e.* from ESTACIONUSUARIO eu join ESTACIONES e on eu.IDESTACION = e.ID where eu.IDUSUARIO = @IDUSUARIO";
            FbCommand comando = new FbCommand(sentencia, conexion, transaccion);

            comando.Parameters.Add("@IDUSUARIO", FbDbType.Integer).Value = idUsuario;
            FbDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                pResult.Add(ReaderToEntidad(reader));
            }

            reader.Close();

            return pResult;
        }

        public bool EstacionPermiteEliminar(int idEstacion)
        {
            bool pResult = true;
            string sentencia = "SELECT COUNT(*) FROM ESTACIONUSUARIO WHERE IDESTACION = @IDESTACION";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@IDESTACION", FbDbType.Integer).Value = idEstacion;

            try
            {
                conexion.Open();

                pResult = Convert.ToInt32(comando.ExecuteScalar()) == 0;

            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        public List<string> ObtenerUsuariosAsignadosEstacion(int idEstacion)
        {
            List<string> result = new List<string>();
            string sentencia = "SELECT U.NOMBRE FROM ESTACIONUSUARIO EU JOIN USUARIO U ON EU.IDUSUARIO = U.ID WHERE EU.IDESTACION = @IDESTACION";

            FbConnection conexion = FBLocal.ConexionObtener();
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@IDESTACION", FbDbType.Integer).Value = idEstacion;

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader[0].ToString());
                }

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
