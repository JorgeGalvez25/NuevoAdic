using System;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;
using Consola.Logic.Entities;

namespace Consola.Logic.Persistence
{
    public class DPVGTCMBPersistence
    {
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

        private DPVGTCMB Read(FbDataReader reader)
        {
            DPVGTCMB result = new DPVGTCMB();
            result.Clave = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            result.Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            result.ClavePEMEX = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            result.Con_ProductoPrecio = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            result.PrecioFisico = reader.IsDBNull(4) ? 0D : reader.GetDouble(4);
            result.AgruparCon = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
            result.DigitoAjustePrecio = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
            result.Tag = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
            result.Tag2 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
            result.Tag3 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
            return result;
        }

        public DPVGTCMB ObtenerDPVGTCMB(FiltroDPVGTCMB f)
        {
            DPVGTCMB result = null;

            this.DbConn((comm) =>
                {
                    comm.CommandText = "Select " +
                                            "CLAVE, " +
                                            "NOMBRE, " +
                                            "CLAVEPEMEX, " +
                                            "CON_PRODUCTOPRECIO, " +
                                            "PRECIOFISICO, " +
                                            "AGRUPAR_CON, " +
                                            "DIGITOAJUSTEPRECIO, " +
                                            "TAG, " +
                                            "TAG2, " +
                                            "TAG3 " +
                                       "From " +
                                            "DPVGTCMB " +
                                       "Where " +
                                            "(CLAVE = @CLAVE OR @CLAVE = 0)";
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

        public ListaDPVGTCMB ObtenerTodosDPVGTCMB(FiltroDPVGTCMB f)
        {
            ListaDPVGTCMB result = new ListaDPVGTCMB();

            this.DbConn((comm) =>
            {
                comm.CommandText = "Select " +
                                        "CLAVE, " +
                                        "NOMBRE, " +
                                        "CLAVEPEMEX, " +
                                        "CON_PRODUCTOPRECIO, " +
                                        "PRECIOFISICO, " +
                                        "AGRUPAR_CON, " +
                                        "DIGITOAJUSTEPRECIO, " +
                                        "TAG, " +
                                        "TAG2, " +
                                        "TAG3 " +
                                   "From " +
                                        "DPVGTCMB " +
                                   "Where " +
                                        "(CLAVE = @CLAVE OR @CLAVE = 0)";
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

        public DPVGTCMB InsertarDPVGTCMB(DPVGTCMB e)
        {
            DPVGTCMB result = null;

            this.DbConn((comm) =>
                {
                    comm.CommandText = @"INSERT INTO DPVGTCMB " +
                                            "(CLAVE, NOMBRE, CLAVEPEMEX, CON_PRODUCTOPRECIO, PRECIOFISICO, AGRUPAR_CON, DIGITOAJUSTEPRECIO, TAG, TAG2, TAG3) " +
                                        "VALUES " +
                                            "(@CLAVE, @NOMBRE, @CLAVEPEMEX, @CON_PRODUCTOPRECIO, @PRECIOFISICO, @AGRUPAR_CON, @DIGITOAJUSTEPRECIO, @TAG, @TAG2, @TAG3)";

                    comm.Parameters.Clear();
                    comm.Parameters.Add("@CLAVE", e.Clave);
                    comm.Parameters.Add("@NOMBRE", e.Nombre);
                    comm.Parameters.Add("@CLAVEPEMEX", e.ClavePEMEX);
                    comm.Parameters.Add("@CON_PRODUCTOPRECIO", e.Con_ProductoPrecio);
                    comm.Parameters.Add("@PRECIOFISICO", e.PrecioFisico);
                    comm.Parameters.Add("@AGRUPAR_CON", e.AgruparCon);
                    comm.Parameters.Add("@DIGITOAJUSTEPRECIO", e.DigitoAjustePrecio);
                    comm.Parameters.Add("@TAG", e.Tag);
                    comm.Parameters.Add("@TAG2", e.Tag2);
                    comm.Parameters.Add("@TAG3", e.Tag3);

                    result = (comm.ExecuteNonQuery() >= 1 ? e : null);
                });

            return result;
        }

        public DPVGTCMB ActualizarDPVGTCMB(DPVGTCMB e)
        {
            DPVGTCMB result = null;

            this.DbConn((comm) =>
                {
                    comm.CommandText = @"UPDATE DPVGTCMB SET " +
                                            "NOMBRE = @NOMBRE, " +
                                            "CLAVEPEMEX = @CLAVEPEMEX, " +
                                            "CON_PRODUCTOPRECIO = @CON_PRODUCTOPRECIO, " +
                                            "PRECIOFISICO = @PRECIOFISICO, " +
                                            "AGRUPAR_CON = @AGRUPAR_CON, " +
                                            "DIGITOAJUSTEPRECIO = @DIGITOAJUSTEPRECIO, " +
                                            "TAG = @TAG, " +
                                            "TAG2 = @TAG2, " +
                                            "TAG3 = @TAG3 " +
                                        "WHERE " +
                                            "(CLAVE = @CLAVE)";

                    comm.Parameters.Clear();
                    comm.Parameters.Add("@CLAVE", e.Clave);
                    comm.Parameters.Add("@NOMBRE", e.Nombre);
                    comm.Parameters.Add("@CLAVEPEMEX", e.ClavePEMEX);
                    comm.Parameters.Add("@CON_PRODUCTOPRECIO", e.Con_ProductoPrecio);
                    comm.Parameters.Add("@PRECIOFISICO", e.PrecioFisico);
                    comm.Parameters.Add("@AGRUPAR_CON", e.AgruparCon);
                    comm.Parameters.Add("@DIGITOAJUSTEPRECIO", e.DigitoAjustePrecio);
                    comm.Parameters.Add("@TAG", e.Tag);
                    comm.Parameters.Add("@TAG2", e.Tag2);
                    comm.Parameters.Add("@TAG3", e.Tag3);

                    result = (comm.ExecuteNonQuery() >= 1 ? e : null);
                });

            return result;
        }
    }
}