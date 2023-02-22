using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdicionalWeb.Persistencia.Enlaces;
using AdicionalWeb.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace AdicionalWeb.Persistencia
{
    public class CombustiblesPersistencia
    {
        private Conexiones _enlace;

        public CombustiblesPersistencia()
        {
            this._enlace = new Conexiones();
        }

        public ListaCombustible CombustibleObtenerTodos(FiltroCombustible filtro)
        {
            ListaCombustible resultado = new ListaCombustible();

            this._enlace.GasolineraConsulta((cmd) =>
                {
                    cmd.CommandText = "SELECT CLAVE, " +
                                            " NOMBRE " +
                                       " FROM DGASTCMB";
                    using (FbDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            Combustibles comb = null;
                            while (reader.Read())
                            {
                                comb = this.ReaderToEntidad(reader);
                                resultado.Add(comb);
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

            return resultado;
        }

        private Combustibles ReaderToEntidad(FbDataReader reader)
        {
            Combustibles comb = new Combustibles();
            {
                comb.Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                comb.Combustible = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            }
            return comb;
        }
    }
}
