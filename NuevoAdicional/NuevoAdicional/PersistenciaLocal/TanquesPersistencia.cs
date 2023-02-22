﻿using System;
using System.Configuration;
using System.Data;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;
using Persistencia;

namespace NuevoAdicional.PersistenciaLocal
{
    public class TanquesPersistencia
    {
        private string _getStringConnection;
        private string GetStringConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_getStringConnection))
                {
                    _getStringConnection = ConfigurationManager.ConnectionStrings["GasConsola"].ConnectionString;
                }

                return _getStringConnection;
            }
        }

        public bool TanquesRegistrar(Tanques entidad)
        {
            entidad = ObtenerComplemento(entidad);

            bool pResult = false;
            string sentencia = @"INSERT INTO DPVGETAN 
	                                   (FOLIO, 
	                                    FECHA,
	                                    FECHAHORA,
	                                    CORTE, 
	                                    TANQUE, 
	                                    COMBUSTIBLE, 
	                                    VOLUMENINICIAL,
	                                    VOLUMENFINAL, 
	                                    VOLUMENRECEPCION, 
	                                    TEMPERATURA, 
	                                    FECHAHORAINICIAL, 
	                                    FECHAHORAFINAL)
                                VALUES (@FOLIO, 
	                                    @FECHA, 
	                                    @FECHAHORA,
		                                @CORTE, 
		                                @TANQUE, 
		                                @COMBUSTIBLE, 
		                                @VOLUMENINICIAL,
		                                @VOLUMENFINAL, 
		                                @VOLUMENRECEPCION, 
		                                @TEMPERATURA, 
		                                @FECHAHORAINICIAL, 
		                                @FECHAHORAFINAL)";

            string actualizar = @"UPDATE DPVGETAN 
                                     SET FECHAHORADISP = @FECHAHORADISP, 
                                         GENERADO = @GENERADO 
                                   WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = entidad.Folio;
                                comando.Parameters.Add("@FECHA", FbDbType.Date).Value = entidad.FechaHora.Date;
                                comando.Parameters.Add("@FECHAHORA", FbDbType.Date).Value = entidad.FechaHora;
                                comando.Parameters.Add("@CORTE", FbDbType.Integer).Value = entidad.Corte;
                                comando.Parameters.Add("@TANQUE", FbDbType.Integer).Value = entidad.Tanque;
                                comando.Parameters.Add("@COMBUSTIBLE", FbDbType.Integer).Value = entidad.Combustible;
                                comando.Parameters.Add("@VOLUMENINICIAL", FbDbType.Double).Value = entidad.VolumenInicial;
                                comando.Parameters.Add("@VOLUMENFINAL", FbDbType.Double).Value = entidad.VolumenFinal;
                                comando.Parameters.Add("@VOLUMENRECEPCION", FbDbType.Double).Value = entidad.VolumenRecepcion;
                                comando.Parameters.Add("@TEMPERATURA", FbDbType.Double).Value = entidad.Temperatura;
                                comando.Parameters.Add("@FECHAHORAINICIAL", FbDbType.Date).Value = entidad.FechaHoraInicial;
                                comando.Parameters.Add("@FECHAHORAFINAL", FbDbType.Date).Value = entidad.FechaHoraFinal;

                                pResult = comando.ExecuteNonQuery() > 0;
                            }

                            using (FbCommand comando = new FbCommand(actualizar, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = entidad.Folio;
                                comando.Parameters.Add("@FECHA", FbDbType.Date).Value = entidad.FechaHora.Date;
                                comando.Parameters.Add("@FECHAHORADISP", FbDbType.Date).Value = entidad.FechaHoraFinal.ToString("yyMMddHHmm");
                                comando.Parameters.Add("@GENERADO", FbDbType.Integer).Value = "Si";

                                pResult = comando.ExecuteNonQuery() > 0;
                            }

                            BitacoraPersistencia servicio = new BitacoraPersistencia();
                            servicio.BitacoraInsertar(new Bitacora()
                                {
                                    Fecha = DateTime.Now.Date,
                                    Hora = DateTime.Now.TimeOfDay,
                                    Id_usuario = Configuraciones.NombreUsuario,
                                    Suceso = string.Format("Registrado Tanque {0:D6}", entidad.Folio)
                                });

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public bool TanquesModificar(Tanques entidad)
        {
            bool pResult = false;
            string sentencia = @"UPDATE DPVGETAN 
                                    SET VOLUMENRECEPCION = @VOLUMENRECEPCION, 
                                        GENERADO = @GENERADO 
                                  WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = entidad.Folio;
                                comando.Parameters.Add("@GENERADO", FbDbType.VarChar).Value = entidad.Generado;
                                comando.Parameters.Add("@VOLUMENRECEPCION", FbDbType.Double).Value = entidad.VolumenRecepcion;

                                pResult = comando.ExecuteNonQuery() > 0;
                            }

                            BitacoraPersistencia servicio = new BitacoraPersistencia();
                            servicio.BitacoraInsertar(new Bitacora()
                                {
                                    Fecha = DateTime.Now.Date,
                                    Hora = DateTime.Now.TimeOfDay,
                                    Id_usuario = Configuraciones.NombreUsuario,
                                    Suceso = string.Format("Actualizado Tanque {0:D6}", entidad.Folio)
                                });

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public bool TanquesEliminar(FiltroTanques filtro)
        {
            bool pResult = false;
            string sentencia = @"DELETE FROM DPVGETAN 
                                  WHERE FOLIO = @FOLIO";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = filtro.Folio;

                                pResult = comando.ExecuteNonQuery() > 0;
                            }

                            BitacoraPersistencia servicio = new BitacoraPersistencia();
                            servicio.BitacoraInsertar(new Bitacora()
                                {
                                    Fecha = DateTime.Now.Date,
                                    Hora = DateTime.Now.TimeOfDay,
                                    Id_usuario = Configuraciones.NombreUsuario,
                                    Suceso = string.Format("Eliminar Tanque {0:D6}", filtro.Folio)
                                });

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public Tanques TanquesObtener(FiltroTanques filtro)
        {
            Tanques pResult = null;
            string sentencia = @"SELECT FOLIO,
	                                    FECHA,
	                                    CORTE,
	                                    TANQUE,
	                                    COMBUSTIBLE,
	                                    VOLUMENINICIAL,
	                                    VOLUMENFINAL,
	                                    VOLUMENRECEPCION,
	                                    TEMPERATURA,
	                                    TERMINALDIST,
	                                    TIPODOC,
	                                    FECHADOC,
	                                    FOLIODOC,
	                                    VOLUMENDOC,
	                                    FECHAHORAINICIAL,
	                                    FECHAHORAFINAL,
	                                    DATOSADICIONALES,
	                                    FECHAHORADISP,
	                                    FECHAHORA,
	                                    FECHATURNO,
	                                    TURNO,
	                                    TRASPASO,
	                                    TANQUE_ORIGEN,
	                                    VENTAS,
	                                    GENERADO,
	                                    ENTRADASRELACIONADAS,
	                                    RELACIONMAESTRO
                                   FROM DPVGETAN
                                  WHERE FOLIO = @FOLIO 
                                    AND FECHA = @FECHA";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FOLIO", FbDbType.Integer).Value = filtro.Folio;
                                comando.Parameters.Add("@FECHA", FbDbType.Integer).Value = filtro.Fecha;

                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        pResult = readerToEntidad(reader);
                                    }
                                }
                            }

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public ListaTanques TanquesObtenerTodos(FiltroTanques filtro)
        {
            ListaTanques pResult = new ListaTanques();
            string sentencia = @"SELECT FOLIO,
	                                    FECHA,
	                                    CORTE,
	                                    TANQUE,
	                                    COMBUSTIBLE,
	                                    VOLUMENINICIAL,
	                                    VOLUMENFINAL,
	                                    VOLUMENRECEPCION,
	                                    TEMPERATURA,
	                                    TERMINALDIST,
	                                    TIPODOC,
	                                    FECHADOC,
	                                    FOLIODOC,
	                                    VOLUMENDOC,
	                                    FECHAHORAINICIAL,
	                                    FECHAHORAFINAL,
	                                    DATOSADICIONALES,
	                                    FECHAHORADISP,
	                                    FECHAHORA,
	                                    FECHATURNO,
	                                    TURNO,
	                                    TRASPASO,
	                                    TANQUE_ORIGEN,
	                                    VENTAS,
	                                    GENERADO,
	                                    ENTRADASRELACIONADAS,
	                                    RELACIONMAESTRO
                                   FROM DPVGETAN
                                  WHERE FECHA = @FECHA";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                comando.Parameters.Add("@FECHA", FbDbType.Date).Value = filtro.Fecha;

                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    Tanques entidad = null;
                                    while (reader.Read())
                                    {
                                        entidad = readerToEntidad(reader);
                                        pResult.Add(entidad);
                                    }
                                }
                            }

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        private Tanques readerToEntidad(FbDataReader reader)
        {
            Tanques entidad = new Tanques();

            entidad.Folio = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            entidad.Fecha = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
            entidad.Corte = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            entidad.Tanque = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
            entidad.Combustible = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            entidad.VolumenInicial = reader.IsDBNull(5) ? 0D : reader.GetDouble(5);
            entidad.VolumenFinal = reader.IsDBNull(6) ? 0D : reader.GetDouble(6);
            entidad.VolumenRecepcion = reader.IsDBNull(7) ? 0D : reader.GetDouble(7);
            entidad.Temperatura = reader.IsDBNull(8) ? 0D : reader.GetDouble(8);
            entidad.TerminalDist = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
            entidad.TipoDoc = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
            entidad.FechaDoc = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11);
            entidad.FolioDoc = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
            entidad.VolumenDoc = reader.IsDBNull(13) ? 0D : reader.GetDouble(13);
            entidad.FechaHoraInicial = reader.IsDBNull(14) ? DateTime.MinValue : reader.GetDateTime(14);
            entidad.FechaHoraFinal = reader.IsDBNull(15) ? DateTime.MinValue : reader.GetDateTime(15);
            entidad.DatosAdicionales = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
            entidad.FechaHoraDisp = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
            entidad.FechaHora = reader.IsDBNull(18) ? DateTime.MinValue : reader.GetDateTime(18);
            entidad.FechaTurno = reader.IsDBNull(19) ? DateTime.MinValue : reader.GetDateTime(19);
            entidad.Turno = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);
            entidad.Traspaso = reader.IsDBNull(21) ? string.Empty : reader.GetString(21);
            entidad.Tanque_Origen = reader.IsDBNull(22) ? 0 : reader.GetInt32(22);
            entidad.Ventas = reader.IsDBNull(23) ? 0 : reader.GetInt32(23);
            entidad.Generado = reader.IsDBNull(24) ? string.Empty : reader.GetString(24);
            entidad.EntradasRelacionadas = reader.IsDBNull(25) ? string.Empty : reader.GetString(25);
            entidad.RelacionMaestro = reader.IsDBNull(26) ? 0 : reader.GetInt32(26);

            return entidad;
        }

        public ListaDpvgTanq ObtenerTanques()
        {
            ListaDpvgTanq pResult = new ListaDpvgTanq();
            string sentencia = @"SELECT T.TANQUE,
                                        T.COMBUSTIBLE,
                                        C.NOMBRE,
                                        C.PRECIOFISICO
                                   FROM DPVGTANQ T
                                   LEFT OUTER JOIN DPVGTCMB C
                                     ON T.COMBUSTIBLE = C.CLAVE";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    DpvgTanq tanque = null;
                                    while (reader.Read())
                                    {
                                        tanque = new DpvgTanq();
                                        tanque.Tanque = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                        tanque.Combustible = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                        tanque.Nombre = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                        tanque.Precio = reader.IsDBNull(3) ? 0D : reader.GetDouble(3);
                                        pResult.Add(tanque);
                                    }
                                }
                            }

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return pResult;
        }

        public Tanques ObtenerComplemento(Tanques entidad)
        {
            string sentencia = @"SELECT FIRST(1) CORTE,
                                       (SELECT MAX(FOLIO) + 1 FROM DPVGETAN)
                                   FROM DPVGCVOL 
                                  WHERE ESTATUS='A'";

            using (FbConnection conexion = new FbConnection(GetStringConnection))
            {
                try
                {
                    conexion.Open();
                    using (FbTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (FbCommand comando = new FbCommand(sentencia, conexion, transaccion))
                            {
                                using (FbDataReader reader = comando.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        entidad.Corte = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                        entidad.Folio = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                    }
                                }
                            }

                            transaccion.Commit();
                        }
                        catch (Exception e)
                        {
                            transaccion.Rollback();
                            throw e;
                        }
                    }
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }

            return entidad;
        }
    }
}
