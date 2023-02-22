using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace Persistencia
{
    public class ProteccionPersistencia
    {
        private Proteccion readerToEntidad(FbDataReader reader)
        {
            Proteccion result = new Proteccion();

            //result.Estacion = Convert.ToInt32(reader["ID_ESTACION"]);
            result.Litros = Convert.ToInt32(reader["LITROS"]);
            result.Activa = Convert.ToString(reader["ACTIVA"]);
            
            return result;
        }

        public ListaProteccion ObtenerLista(int idEstacion)
        {
            string sentencia = "SELECT * FROM PROTECCIONES";// WHERE ID_ESTACION = @ID_ESTACION";
            ListaProteccion pResult = new ListaProteccion();

            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbCommand comando = new FbCommand(sentencia, conexion);

            //comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = idEstacion;

            try
            {
                conexion.Open();
                FbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Proteccion tmp = new Proteccion();
                    tmp = readerToEntidad(reader);

                    pResult.Add(tmp);
                }
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return pResult;
        }

        private Proteccion ProteccionInsertar(Proteccion entidad, FbConnection conexion, FbTransaction transaccion)
        {
            Proteccion pResult = null;
            int cant;
            string sentencia = "INSERT INTO PROTECCIONES (LITROS,ACTIVA) VALUES (@LITROS,@ACTIVA)";
            FbCommand comando = new FbCommand(sentencia, conexion, transaccion);

            comando.Parameters.Add("@LITROS", FbDbType.Integer).Value = entidad.Litros;
            comando.Parameters.Add("@ACTIVA", FbDbType.VarChar).Value = entidad.Activa;

            cant = comando.ExecuteNonQuery();
            pResult = cant > 0 ? entidad : null;

            return pResult;
        }

        private bool ProteccionEliminar(int estacion, FbConnection conexion, FbTransaction transaccion)
        {
            int result;
            string sentencia = "DELETE FROM PROTECCIONES";// WHERE ID_ESTACION = @ID_ESTACION";
            FbCommand comando = new FbCommand(sentencia, conexion, transaccion);

            //comando.Parameters.Add("@ID_ESTACION", FbDbType.Integer).Value = estacion;
            result = comando.ExecuteNonQuery();
            
            return result > 0;
        }

        public ListaProteccion ProteccionInsertarActualizar(ListaProteccion protecciones)
        {
            ListaProteccion proteccionesTmp = new ListaProteccion();
            FbConnection conexion = new Conexiones().ConexionObtener("Adicional");
            FbTransaction transaccion = null;

            try
            {
                conexion.Open();
                transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);

                ProteccionEliminar(protecciones[0].Estacion, conexion, transaccion);
                
                    foreach (Proteccion item in protecciones)
                    {
                        if (item.Litros > 0)
                        {
                            Proteccion tmp = ProteccionInsertar(item, conexion, transaccion);

                            if (tmp != null)
                            {
                                proteccionesTmp.Add(tmp);
                            }
                            else
                            {
                                throw new Exception("No ha sido posible insertar la protección");
                            }
                        }
                    }
                

                transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();
                return null;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return proteccionesTmp;
        }
    }
}
