using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace ServiciosCliente
{
    public class BombaPersistencia
    {

        public Bomba ReaderToEntidad(FbDataReader reader)
        {

            Bomba pResult = new Bomba();

            //if ((reader["TIPODISPENSARIO"] is System.DBNull ? 0 : (int)reader["TIPODISPENSARIO"]) == 5)
            //{
            //    pResult.poscarga = reader["HJ_ADDR"] is System.DBNull ? 0 : (int)reader["HJ_ADDR"];
            //    pResult.manguera = reader["HJ_LADO"] is System.DBNull ? 0 : (int)reader["HJ_LADO"];
            //}
            //else
            //{
                pResult.manguera = reader["CON_POSICION"] is System.DBNull ? 0 : (int)reader["CON_POSICION"];
                pResult.poscarga = reader["POSCARGA"] is System.DBNull ? 0 : (int)reader["POSCARGA"];
            //}
            pResult.combustible = reader["COMBUSTIBLE"] is System.DBNull ? 0 : (int)reader["COMBUSTIBLE"];
            pResult.isla = reader["ISLA"] is System.DBNull ? 0 : (int)reader["ISLA"];
            pResult.con_precio = reader["CON_PRECIO"] is System.DBNull ? 0 : (int)reader["CON_PRECIO"];
            pResult.con_posicion = reader["CON_POSICION"] is System.DBNull ? 0 : (int)reader["CON_POSICION"];
            pResult.con_digitoajuste = reader["CON_DIGITOAJUSTE"] is System.DBNull ? 0 : (int)reader["CON_DIGITOAJUSTE"];
            pResult.impresora = reader["IMPRESORA"] is System.DBNull ? 0 : (int)reader["IMPRESORA"];
            pResult.Activo = reader["ACTIVO"] is System.DBNull ? "" : (string)reader["ACTIVO"];
            pResult.imprimeautom = reader["IMPRIMEAUTOM"] is System.DBNull ? "" : (string)reader["IMPRIMEAUTOM"];
            pResult.digitoajusteprecio = reader["DIGITOAJUSTEPRECIO"] is System.DBNull ? 0 : (int)reader["DIGITOAJUSTEPRECIO"];
            pResult.campolectura = reader["CAMPOLECTURA"] is System.DBNull ? "" : (string)reader["CAMPOLECTURA"];
            pResult.modooperacion = reader["MODOOPERACION"] is System.DBNull ? "" : (string)reader["MODOOPERACION"];
            pResult.Tanque = reader["TANQUE"] is System.DBNull ? 0 : (int)reader["TANQUE"];
            pResult.impretarjetas = reader["IMPRETARJETAS"] is System.DBNull ? "" : (string)reader["IMPRETARJETAS"];
            pResult.digitosgilbarco = reader["DIGITOSGILBARCO"] is System.DBNull ? 0 : (int)reader["DIGITOSGILBARCO"];
            pResult.decimalesgilbarco = reader["HJ_ADDR"] is System.DBNull ? 0 : (int)reader["HJ_ADDR"];
            pResult.digitoajustevol = reader["TEAM_NODISP"] is System.DBNull ? 0 : (int)reader["TEAM_NODISP"];
            pResult.rfid = reader["RFID"] is System.DBNull ? "" : (string)reader["RFID"];
            pResult.cliente = reader["CLIENTE"] is System.DBNull ? "" : (string)reader["CLIENTE"];
            pResult.vehiculo = reader["VEHICULO"] is System.DBNull ? "" : (string)reader["VEHICULO"];
            pResult.control_aros = reader["CONTROL_AROS"] is System.DBNull ? "" : (string)reader["CONTROL_AROS"];
            pResult.error = reader["ERROR"] is System.DBNull ? "" : (string)reader["ERROR"];
            pResult.mensaje_error = reader["MENSAJE_ERROR"] is System.DBNull ? "" : (string)reader["MENSAJE_ERROR"];

            return pResult;
        }

        public Bomba BombaObtener(int Amanguera)
        {

            Bomba pResult = null;

            string sentencia = "SELECT * FROM DPVGBOMB WHERE MANGUERA = @MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = Amanguera;

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

        public ListaBomba ObtenerLista()
        {
            ListaBomba pResult = new ListaBomba();

            string sentencia = "SELECT B.*,E.TIPODISPENSARIO FROM DPVGBOMB B, DPVGESTS E ORDER BY POSCARGA ASC, CON_POSICION ASC";

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

        public Bomba BombaInsertar(Bomba ABomba)
        {

            Bomba pResult = null;

            string sentencia = "INSERT INTO DPVGBOMB(MANGUERA, POSCARGA, COMBUSTIBLE, ISLA, CON_PRECIO, CON_POSICION, CON_DIGITOAJUSTE, IMPRESORA, ACTIVO, IMPRIMEAUTOM, DIGITOAJUSTEPRECIO, CAMPOLECTURA, MODOOPERACION, TANQUE, IMPRETARJETAS, DIGITOSGILBARCO, DECIMALESGILBARCO, DIGITOAJUSTEVOL, RFID, CLIENTE, VEHICULO, CONTROL_AROS, ERROR, MENSAJE_ERROR) VALUES(@MANGUERA, @POSCARGA, @COMBUSTIBLE, @ISLA, @CON_PRECIO, @CON_POSICION, @CON_DIGITOAJUSTE, @IMPRESORA, @ACTIVO, @IMPRIMEAUTOM, @DIGITOAJUSTEPRECIO, @CAMPOLECTURA, @MODOOPERACION, @TANQUE, @IMPRETARJETAS, @DIGITOSGILBARCO, @DECIMALESGILBARCO, @DIGITOAJUSTEVOL, @RFID, @CLIENTE, @VEHICULO, @CONTROL_AROS, @ERROR, @MENSAJE_ERROR)";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = ABomba.manguera;
            comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = ABomba.poscarga;
            comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ABomba.combustible;
            comando.Parameters.Add("@ISLA", FbDbType.Integer).Value = ABomba.isla;
            comando.Parameters.Add("@CON_PRECIO", FbDbType.Integer).Value = ABomba.con_precio;
            comando.Parameters.Add("@CON_POSICION", FbDbType.Integer).Value = ABomba.con_posicion;
            comando.Parameters.Add("@CON_DIGITOAJUSTE", FbDbType.Integer).Value = ABomba.con_digitoajuste;
            comando.Parameters.Add("@IMPRESORA", FbDbType.Integer).Value = ABomba.impresora;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = ABomba.Activo;
            comando.Parameters.Add("@IMPRIMEAUTOM", FbDbType.VarChar).Value = ABomba.imprimeautom;
            comando.Parameters.Add("@DIGITOAJUSTEPRECIO", FbDbType.Integer).Value = ABomba.digitoajusteprecio;
            comando.Parameters.Add("@CAMPOLECTURA", FbDbType.VarChar).Value = ABomba.campolectura;
            comando.Parameters.Add("@MODOOPERACION", FbDbType.VarChar).Value = ABomba.modooperacion;
            comando.Parameters.Add("@TANQUE", FbDbType.Integer).Value = ABomba.Tanque;
            comando.Parameters.Add("@IMPRETARJETAS", FbDbType.VarChar).Value = ABomba.impretarjetas;
            comando.Parameters.Add("@DIGITOSGILBARCO", FbDbType.Integer).Value = ABomba.digitosgilbarco;
            comando.Parameters.Add("@DECIMALESGILBARCO", FbDbType.Integer).Value = ABomba.decimalesgilbarco;
            comando.Parameters.Add("@DIGITOAJUSTEVOL", FbDbType.Integer).Value = ABomba.digitoajustevol;
            comando.Parameters.Add("@RFID", FbDbType.VarChar).Value = ABomba.rfid;
            comando.Parameters.Add("@CLIENTE", FbDbType.VarChar).Value = ABomba.cliente;
            comando.Parameters.Add("@VEHICULO", FbDbType.VarChar).Value = ABomba.vehiculo;
            comando.Parameters.Add("@CONTROL_AROS", FbDbType.VarChar).Value = ABomba.control_aros;
            comando.Parameters.Add("@ERROR", FbDbType.VarChar).Value = ABomba.error;
            comando.Parameters.Add("@MENSAJE_ERROR", FbDbType.VarChar).Value = ABomba.mensaje_error;

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

            pResult = BombaObtener(ABomba.manguera);

            return pResult;
        }

        public Bomba BombaActualizar(Bomba ABomba)
        {

            Bomba pResult = null;

            string sentencia = "UPDATE DPVGBOMB SET POSCARGA = @POSCARGA, COMBUSTIBLE = @COMBUSTIBLE, ISLA = @ISLA, CON_PRECIO = @CON_PRECIO, CON_POSICION = @CON_POSICION, CON_DIGITOAJUSTE = @CON_DIGITOAJUSTE, IMPRESORA = @IMPRESORA, ACTIVO = @ACTIVO, IMPRIMEAUTOM = @IMPRIMEAUTOM, DIGITOAJUSTEPRECIO = @DIGITOAJUSTEPRECIO, CAMPOLECTURA = @CAMPOLECTURA, MODOOPERACION = @MODOOPERACION, TANQUE = @TANQUE, IMPRETARJETAS = @IMPRETARJETAS, DIGITOSGILBARCO = @DIGITOSGILBARCO, DECIMALESGILBARCO = @DECIMALESGILBARCO, DIGITOAJUSTEVOL = @DIGITOAJUSTEVOL, RFID = @RFID, CLIENTE = @CLIENTE, VEHICULO = @VEHICULO, CONTROL_AROS = @CONTROL_AROS, ERROR = @ERROR, MENSAJE_ERROR = @MENSAJE_ERROR WHERE MANGUERA = @MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = ABomba.manguera;
            comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = ABomba.poscarga;
            comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = ABomba.combustible;
            comando.Parameters.Add("@ISLA", FbDbType.Integer).Value = ABomba.isla;
            comando.Parameters.Add("@CON_PRECIO", FbDbType.Integer).Value = ABomba.con_precio;
            comando.Parameters.Add("@CON_POSICION", FbDbType.Integer).Value = ABomba.con_posicion;
            comando.Parameters.Add("@CON_DIGITOAJUSTE", FbDbType.Integer).Value = ABomba.con_digitoajuste;
            comando.Parameters.Add("@IMPRESORA", FbDbType.Integer).Value = ABomba.impresora;
            comando.Parameters.Add("@ACTIVO", FbDbType.VarChar).Value = ABomba.Activo;
            comando.Parameters.Add("@IMPRIMEAUTOM", FbDbType.VarChar).Value = ABomba.imprimeautom;
            comando.Parameters.Add("@DIGITOAJUSTEPRECIO", FbDbType.Integer).Value = ABomba.digitoajusteprecio;
            comando.Parameters.Add("@CAMPOLECTURA", FbDbType.VarChar).Value = ABomba.campolectura;
            comando.Parameters.Add("@MODOOPERACION", FbDbType.VarChar).Value = ABomba.modooperacion;
            comando.Parameters.Add("@TANQUE", FbDbType.Integer).Value = ABomba.Tanque;
            comando.Parameters.Add("@IMPRETARJETAS", FbDbType.VarChar).Value = ABomba.impretarjetas;
            comando.Parameters.Add("@DIGITOSGILBARCO", FbDbType.Integer).Value = ABomba.digitosgilbarco;
            comando.Parameters.Add("@DECIMALESGILBARCO", FbDbType.Integer).Value = ABomba.decimalesgilbarco;
            comando.Parameters.Add("@DIGITOAJUSTEVOL", FbDbType.Integer).Value = ABomba.digitoajustevol;
            comando.Parameters.Add("@RFID", FbDbType.VarChar).Value = ABomba.rfid;
            comando.Parameters.Add("@CLIENTE", FbDbType.VarChar).Value = ABomba.cliente;
            comando.Parameters.Add("@VEHICULO", FbDbType.VarChar).Value = ABomba.vehiculo;
            comando.Parameters.Add("@CONTROL_AROS", FbDbType.VarChar).Value = ABomba.control_aros;
            comando.Parameters.Add("@ERROR", FbDbType.VarChar).Value = ABomba.error;
            comando.Parameters.Add("@MENSAJE_ERROR", FbDbType.VarChar).Value = ABomba.mensaje_error;

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

            pResult = BombaObtener(ABomba.manguera);

            return pResult;
        }

        public bool BombaEliminar(int Amanguera)
        {

            bool pResult = true;

            string sentencia = "DELETE FROM DPVGBOMB WHERE MANGUERA = @MANGUERA";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@MANGUERA", FbDbType.Integer).Value = Amanguera;

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

        public List<Bomba> ObtenerBombasPosicion(int posicion)
        {
            List<Bomba> pResult = new List<Bomba>();
            string sentencia = "SELECT B.*,E.TIPODISPENSARIO FROM DPVGBOMB B, DPVGESTS E WHERE POSCARGA=@POSCARGA ORDER BY CON_POSICION";

            FbConnection conexion = new Conexiones().ConexionObtener("GasConsola");
            FbCommand comando = new FbCommand(sentencia, conexion);

            comando.Parameters.Add("@POSCARGA", FbDbType.Integer).Value = posicion;

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

    public class ListaBomba : List<Bomba>
    {
    }

}
