using System;
using System.Collections.Generic;
using Adicional.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace Persistencia
{
    public class LicenciaPersistencia
    {
        public List<Licencia> LicenciasObtener()
        {
            List<Licencia> lics = new List<Licencia>();
            string mod = "CVL5";
            string rSocial = string.Empty;
            string tLic = "1 Usuarios";
            int usrs = 1;
            string esTmp = "No";
            DateTime fechaVence = DateTime.Now.Date.AddDays(30);

            // Obtener datos Principales
            string sentenciaConf = "SELECT FIRST 1 * FROM DPVGCONF";
            using (FbCommand comando = new FbCommand(sentenciaConf))
            {
                using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.GASCONSOLA))
                {
                    try
                    {
                        comando.Connection.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    Licencia lic = new Licencia();

                                    lic.Modulo = mod;
                                    lic.Razon_social = rSocial = reader["RazonSocial"].ToString();
                                    lic.TipoLicencia = tLic;
                                    lic.Usuarios = usrs;
                                    lic.Estemporal = esTmp;
                                    lic.Fecha_vence = fechaVence.ToString("dd/MM/yyyy", ContantesAdicional.CulturaLocal);
                                    lic.ClaveAutor = reader["Licencia2"].ToString();

                                    lics.Add(lic);
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (comando.Connection.State == System.Data.ConnectionState.Open)
                            comando.Connection.Close();
                    }
                }
            }

            // Obtener subModulos
            string sentencia = "SELECT * FROM LICENCIAS";

            using (FbCommand comando = new FbCommand(sentencia))
            {
                using (FbConnection conexion = new Conexiones().ConexionObtener(Conexiones.ADICIONAL))
                {
                    try
                    {
                        Licencia lic = null;

                        conexion.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {

                            try
                            {
                                while (reader.Read())
                                {
                                    lic = new Licencia();

                                    lic.Modulo = reader["Modulo"].ToString();
                                    lic.Razon_social = rSocial;
                                    lic.TipoLicencia = tLic;
                                    lic.Usuarios = usrs;
                                    lic.Estemporal = esTmp;
                                    lic.Fecha_vence = fechaVence.ToString("dd/MM/yyyy", ContantesAdicional.CulturaLocal);
                                    lic.ClaveAutor = reader["Licencia"].ToString();

                                    lics.Add(lic);
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (conexion.State == System.Data.ConnectionState.Open)
                            conexion.Close();
                    }
                }
            }

            return lics;
        }

        public Licencia LicenciaObtener(string modulo)
        {
            Licencia lic = new Licencia();
            string mod = "CVL5";
            string rSocial = string.Empty;
            string tLic = "1 Usuarios";
            int usrs = 1;

            // Obtener datos Principales
            string sentenciaConf = "SELECT FIRST 1 " +
                                          "RAZONSOCIAL, " +
                                          "LICENCIA2, " +
                                          "ESTEMPORAL2, " +
                                          "FECHAVENCE2 " +
                                     "FROM DPVGCONF";
            using (FbCommand comando = new FbCommand(sentenciaConf))
            {
                using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.GASCONSOLA))
                {
                    try
                    {
                        comando.Connection.Open();
                        using (FbDataReader reader = comando.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    lic.Modulo = mod;
                                    lic.TipoLicencia = tLic;
                                    lic.Usuarios = usrs;
                                    lic.Razon_social = rSocial = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                    lic.ClaveAutor = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                    lic.Estemporal = reader.IsDBNull(2) ? "No" : reader.GetString(2);
                                    lic.Fecha_vence = (reader.IsDBNull(3) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : reader.GetDateTime(3)).ToString("dd/MM/yyyy", ContantesAdicional.CulturaLocal);
                                }
                            }
                            finally
                            {
                                if (!reader.IsClosed)
                                {
                                    reader.Close();
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (comando.Connection.State == System.Data.ConnectionState.Open)
                            comando.Connection.Close();
                    }
                }
            }

            if (!modulo.Equals(mod))
            {
                // Obtener subModulos
                string sentencia = "SELECT MODULO, " +
                                          "LICENCIA, " +
                                          "TEMPORAL, " +
                                          "FECHAVENCE " +
                                     "FROM LICENCIAS " +
                                    "WHERE MODULO = @MODULO";

                using (FbCommand comando = new FbCommand(sentencia))
                {
                    using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.ADICIONAL))
                    {
                        try
                        {
                            comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = modulo;

                            comando.Connection.Open();
                            using (FbDataReader reader = comando.ExecuteReader())
                            {
                                try
                                {
                                    while (reader.Read())
                                    {
                                        lic.Modulo = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                        lic.ClaveAutor = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                        lic.Estemporal = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                        lic.Fecha_vence = (reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3)).ToString("dd/MM/yyyy", ContantesAdicional.CulturaLocal);
                                    }
                                }
                                finally
                                {
                                    if (!reader.IsClosed)
                                    {
                                        reader.Close();
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if (comando.Connection.State == System.Data.ConnectionState.Open)
                                comando.Connection.Close();
                        }
                    }
                }
            }

            return lic;
        }

        public Licencia LicenciaInsertar(string modulo, string lic)
        {
            string sentencia = "INSERT INTO LICENCIAS(MODULO, LICENCIA) VALUES(@MODULO, @LICENCIA)";

            using (FbCommand comando = new FbCommand(sentencia))
            {
                using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.ADICIONAL))
                {
                    try
                    {
                        comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = modulo;
                        comando.Parameters.Add("@LICENCIA", FbDbType.VarChar).Value = lic;

                        comando.Connection.Open();
                        using (FbTransaction transa = comando.Connection.BeginTransaction())
                        {
                            try
                            {
                                comando.ExecuteNonQuery();
                                transa.Commit();
                            }
                            catch (Exception)
                            {
                                transa.Rollback();
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        if (comando.Connection.State == System.Data.ConnectionState.Open)
                            comando.Connection.Close();
                    }
                }
            }

            return LicenciaObtener(modulo);
        }

        public Licencia LicenciaActualizar(Licencia licencia)
        {
            string sentencia = "UPDATE LICENCIAS SET LICENCIA = @LICENCIA WHERE MODULO = @MODULO";
            using (FbCommand comando = new FbCommand(sentencia))
            {
                using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.ADICIONAL))
                {
                    try
                    {
                        comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = licencia.Modulo;
                        comando.Parameters.Add("@LICENCIA", FbDbType.VarChar).Value = licencia.ClaveAutor;

                        comando.Connection.Open();
                        using (FbTransaction transa = comando.Connection.BeginTransaction())
                        {
                            try
                            {
                                comando.ExecuteNonQuery();
                                transa.Commit();
                            }
                            catch (Exception)
                            {
                                transa.Rollback();
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        if (comando.Connection.State == System.Data.ConnectionState.Open)
                            comando.Connection.Close();
                    }
                }
            }

            return LicenciaObtener(licencia.Modulo);
        }

        public bool LicenciaEliminar(string modulo)
        {
            bool result = false;
            string sentencia = "DELETE FROM LICENCIAS WHERE MODULO = @MODULO";

            using (FbCommand comando = new FbCommand(sentencia))
            {
                using (comando.Connection = new Conexiones().ConexionObtener(Conexiones.ADICIONAL))
                {
                    comando.Parameters.Add("@MODULO", FbDbType.VarChar).Value = modulo;

                    try
                    {
                        comando.Connection.Open();
                        using (FbTransaction transa = comando.Connection.BeginTransaction())
                        {
                            try
                            {
                                result = comando.ExecuteNonQuery() > 0;
                                transa.Commit();
                            }
                            catch (Exception)
                            {
                                transa.Rollback();
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        if (comando.Connection.State == System.Data.ConnectionState.Open)
                            comando.Connection.Close();
                    }
                }
            }

            return result;
        }
    }
}
