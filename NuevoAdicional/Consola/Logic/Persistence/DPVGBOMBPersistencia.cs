using System;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;
using Consola.Logic.Entities;
using System.Collections.Generic;

namespace Consola.Logic.Persistence
{
    public class DPVGBOMBPersistencia
    {
        private const string CONSULTA = "SELECT " +
                                            "MANGUERA, " +
                                            "POSCARGA, " +
                                            "COMBUSTIBLE, " +
                                            "ISLA, " +
                                            "CON_PRECIO, " +
                                            "CON_POSICION, " +
                                            "CON_DIGITOAJUSTE, " +
                                            "IMPRESORA, " +
                                            "ACTIVO, " +
                                            "IMPRIMEAUTOM, " +
                                            "DIGITOAJUSTEPRECIO, " +
                                            "MODOOPERACION, " +
                                            "TANQUE, " +
                                            "DIGITOSGILBARCO, " +
                                            "DECIMALESGILBARCO, " +
                                            "DIGITOAJUSTEVOL, " +
                                            "DIGITOAJUSTEPRESET " +
                                       "FROM " +
                                            "DPVGBOMB " +
                                       "WHERE " +
                                            "(MANGUERA = @MANGUERA OR @MANGUERA = 0)";

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

        private DPVGBOMB Read(FbDataReader reader)
        {
            DPVGBOMB result = new DPVGBOMB();
            result.Manguera = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            result.Poscarga = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
            result.Combustible = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            result.Isla = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
            result.Con_Precio = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            result.Con_Posicion = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
            result.Con_DigitoAjuste = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
            result.Impresora = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
            result.Activo = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
            result.ImprimeAutom = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
            result.DigitoAjustePrecio = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            result.ModoOperacion = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
            result.Tanque = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            result.DigitosGilbarco = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            result.DecimalesGilbarco = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            result.DigitoAjusteVol = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            result.DigitoAjustePreset = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
            return result;
        }

        private FbParameter[] GetParameters(DPVGBOMB e)
        {
            FbParameter[] paramtr = new FbParameter[]
                {
                    new FbParameter("@MANGUERA", e.Manguera),
                    new FbParameter("@POSCARGA", e.Poscarga),
                    new FbParameter("@COMBUSTIBLE", e.Combustible),
                    new FbParameter("@ISLA", e.Isla),
                    new FbParameter("@CON_PRECIO", e.Con_Precio),
                    new FbParameter("@CON_POSICION", e.Con_Posicion),
                    new FbParameter("@CON_DIGITOAJUSTE", e.Con_DigitoAjuste),
                    new FbParameter("@IMPRESORA", e.Impresora),
                    new FbParameter("@ACTIVO", e.Activo),
                    new FbParameter("@IMPRIMEAUTOM", e.ImprimeAutom),
                    new FbParameter("@DIGITOAJUSTEPRECIO", e.DigitoAjustePrecio),
                    new FbParameter("@MODOOPERACION", e.ModoOperacion),
                    new FbParameter("@TANQUE", e.Tanque),
                    new FbParameter("@DIGITOSGILBARCO", e.DigitosGilbarco),
                    new FbParameter("@DECIMALESGILBARCO", e.DecimalesGilbarco),
                    new FbParameter("@DIGITOAJUSTEVOL", e.DigitoAjusteVol),
                    new FbParameter("@DIGITOAJUSTEPRESET", e.DigitoAjustePreset),
                };

            return paramtr;
        }

        public DPVGBOMB ObtenerDPVGBOMB(FiltroDPVGBOMB f)
        {
            DPVGBOMB result = null;

            this.DbConn((comm) =>
            {
                comm.CommandText = CONSULTA;
                comm.Parameters.Add("@MANGUERA", f.Manguera);

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

        public ListaDPVGBOMB ObtenerTodosDPVGBOMB(FiltroDPVGBOMB f)
        {
            ListaDPVGBOMB result = new ListaDPVGBOMB();

            this.DbConn((comm) =>
            {
                comm.CommandText = CONSULTA;
                comm.Parameters.Add("@MANGUERA", f.Manguera);

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

        public DPVGBOMB InsertarDPVGBOMB(DPVGBOMB e)
        {
            DPVGBOMB result = null;

            this.DbConn((comm) =>
            {
                comm.CommandText = "INSERT INTO DPVGBOMB " +
                                        "(MANGUERA, POSCARGA, COMBUSTIBLE, ISLA, CON_PRECIO, CON_POSICION, CON_DIGITOAJUSTE, IMPRESORA, ACTIVO, IMPRIMEAUTOM, DIGITOAJUSTEPRECIO, MODOOPERACION, TANQUE, DIGITOSGILBARCO, DECIMALESGILBARCO, DIGITOAJUSTEVOL, DIGITOAJUSTEPRESET) " +
                                   "VALUES " +
                                        "(@MANGUERA, @POSCARGA, @COMBUSTIBLE, @ISLA, @CON_PRECIO, @CON_POSICION, @CON_DIGITOAJUSTE, @IMPRESORA, @ACTIVO, @IMPRIMEAUTOM, @DIGITOAJUSTEPRECIO, @MODOOPERACION, @TANQUE, @DIGITOSGILBARCO, @DECIMALESGILBARCO, @DIGITOAJUSTEVOL, @DIGITOAJUSTEPRESET) ";

                comm.Parameters.Clear();
                comm.Parameters.AddRange(this.GetParameters(e));

                result = (comm.ExecuteNonQuery() >= 1 ? e : null);
            });

            return result;
        }

        public DPVGBOMB ActualizarDPVGBOMB(DPVGBOMB e)
        {
            DPVGBOMB result = null;

            this.DbConn((comm) =>
            {
                comm.CommandText = "UPDATE DPVGBOMB SET " +
                                        "POSCARGA = @POSCARGA, " +
                                        "COMBUSTIBLE = @COMBUSTIBLE, " +
                                        "ISLA = @ISLA, " +
                                        "CON_PRECIO = @CON_PRECIO, " +
                                        "CON_POSICION = @CON_POSICION, " +
                                        "CON_DIGITOAJUSTE = @CON_DIGITOAJUSTE, " +
                                        "IMPRESORA = @IMPRESORA, " +
                                        "ACTIVO = @ACTIVO, " +
                                        "IMPRIMEAUTOM = @IMPRIMEAUTOM, " +
                                        "DIGITOAJUSTEPRECIO = @DIGITOAJUSTEPRECIO, " +
                                        "MODOOPERACION = @MODOOPERACION, " +
                                        "TANQUE = @TANQUE, " +
                                        "DIGITOSGILBARCO = @DIGITOSGILBARCO, " +
                                        "DECIMALESGILBARCO = @DECIMALESGILBARCO, " +
                                        "DIGITOAJUSTEVOL = @DIGITOAJUSTEVOL, " +
                                        "DIGITOAJUSTEPRESET = @DIGITOAJUSTEPRESET " +
                                   "WHERE " +
                                        "(MANGUERA = @MANGUERA)";

                comm.Parameters.Clear();
                comm.Parameters.AddRange(this.GetParameters(e));

                result = (comm.ExecuteNonQuery() >= 1 ? e : null);
            });

            return result;
        }
    }
}
