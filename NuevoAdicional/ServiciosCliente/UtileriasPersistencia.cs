using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Runtime.InteropServices;

namespace ServiciosCliente
{
    public class UtileriasPersistencia
    {
        [DllImport("LibsDelphi.dll", EntryPoint = "LicenciaValidaDLL")]
        private static extern int LicenciaValidaDLL(string RazonSocial, string Sistema, string Version, string TipoLicencia, string ClaveAutor, int Usuarios, bool LicenciaTemporal, string Fecha);

        public bool ValidarLicencia(string sistema)
        {
            bool result = false;
            string sentencia = "SELECT FIRST 1 * FROM DPVGCONF";

            using (FbConnection conexion = new Conexiones().ConexionObtener("GasConsola"))
            {
                FbCommand comando = new FbCommand(sentencia, conexion);

                string razonSocial = string.Empty;
                string licencia = string.Empty;

                try
                {
                    conexion.Open();
                    FbDataReader reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        razonSocial = reader["RazonSocial"] is DBNull ? string.Empty : reader["RazonSocial"].ToString();
                        licencia = reader["Licencia2"] is DBNull ? string.Empty : reader["Licencia2"].ToString();
                    }

                    reader.Close();
                }
                catch { }
                finally
                {
                    if (conexion.State == System.Data.ConnectionState.Open)
                        conexion.Close();
                }

                result = LicenciaValidaDLL(razonSocial, sistema, "3.1", "Abierta", licencia, 1, false, string.Empty) == 1;
            }

            return result;
        }
    }
}
