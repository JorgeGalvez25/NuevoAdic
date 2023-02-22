using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AdicionalWeb.Entidades;
using FirebirdSql.Data.FirebirdClient;

namespace AdicionalWeb.Persistencia.Enlaces
{
    public class ConexionConsola
    {
        private Conexiones _enlaces;
        private const string _sqlQueryEsts = " SELECT CONSOLA " +
                                           " FROM DPVGESTS ";

        public ConexionConsola()
        {
            this._enlaces = new Conexiones();
        }

        public string ConsolaIP(FiltroDispensarios filtro)
        {
            string resultado = string.Empty;
            string tmpResult = string.Empty;
            this._enlaces.ConsolaConsulta((cmd) =>
                {
                    cmd.CommandText = _sqlQueryEsts;

                    using (FbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            tmpResult = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        }
                    }
                }, new FiltroEstaciones() { Clave = filtro.Estacion });

            if (!string.IsNullOrEmpty(tmpResult))
            {
                Func<string, string, string> fnGetSeparators = new Func<string, string, string>((toFind, from) =>
                    {
                        string resp = string.Empty;
                        char[] spltEquals = new char[] { '=' };
                        char[] spltNewLine = new char[] { '\r', '\n' };
                        int idx = from.IndexOf(toFind, StringComparison.CurrentCultureIgnoreCase);

                        if (idx >= 0)
                        {
                            string[] strAux = from.Substring(idx).Split(spltNewLine, StringSplitOptions.RemoveEmptyEntries);
                            strAux = strAux[0].Split(spltEquals, StringSplitOptions.RemoveEmptyEntries);

                            if (strAux.Length == 2)
                            {
                                resp = strAux[1].Trim();
                            }
                        }

                        return resp;
                    });

                if (fnGetSeparators("ManejaServicio", tmpResult).Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    resultado = fnGetSeparators("PuertoServicio", tmpResult);
                }
            }

            return resultado;
        }
    }
}
