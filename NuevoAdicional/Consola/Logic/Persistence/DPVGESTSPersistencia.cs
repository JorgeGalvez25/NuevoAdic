using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consola.Logic.Entities;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace Consola.Logic.Persistence
{
    public class DPVGESTSPersistencia
    {
        private const string CONSULTA = "Select CLAVE, " +
                                               "NOMBRE, " +
                                               "CONSOLA, " +
                                               "TIPODISPENSARIO, " +
                                               "TIPOTANQUES, " +
                                               "NUMEROESTACION, " +
                                               "TIPOINTERFACE " +
                                          "From DPVGESTS " +
                                         "Where (CLAVE = @CLAVE OR @CLAVE = 0)";

        private void DbConn(Action<FbCommand> action)
        {
            using (FbConnection conn = new FbConnection(ConfigurationManager.ConnectionStrings["Consola"].ConnectionString))
            {
                conn.Open();
                try
                {
                    using (FbCommand comm = conn.CreateCommand())
                    {
                        action(comm);
                    }
                }
                finally
                {
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private DPVGESTS Read(FbDataReader reader)
        {
            DPVGESTS result = new DPVGESTS();
            result.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            result.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            result.Consola = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            result.TipoDispensario = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
            result.TipoTanques = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            result.NumeroEstacion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
            result.TipoInterface = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
            return result;
        }

        public DPVGESTS ObtenerDPVGESTS(FiltroDPVGESTS f)
        {
            DPVGESTS result = null;

            this.DbConn((comm) =>
            {
                comm.CommandText = CONSULTA;
                comm.Parameters.Add("@CLAVE", f.Clave);

                using (FbDataReader reader = comm.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            result = Read(reader);
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

            });

            return result;
        }

        public ListaDPVGESTS ObtenerTodosDPVGESTS(FiltroDPVGESTS f)
        {
            ListaDPVGESTS result = new ListaDPVGESTS();

            this.DbConn((comm) =>
            {
                comm.CommandText = CONSULTA;
                comm.Parameters.Add("@CLAVE", f.Clave);

                using (FbDataReader reader = comm.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            result.Add(Read(reader));
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

            });

            return result;
        }
    }
}
