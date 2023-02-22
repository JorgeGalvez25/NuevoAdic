using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace ServiciosCliente
{
    public class Conexiones
    {
        private FbConnection ObtenerConexionAdicional()
        {
            string cs = ConfigurationManager.ConnectionStrings["Adicional"].ConnectionString;
            return new FbConnection(cs);
        }

        private FbConnection ObtenerConexionConsola()
        {
            string cs = ConfigurationManager.ConnectionStrings["GasConsola"].ConnectionString;
            return new FbConnection(cs);
        }

        private FbConnection ObtenerConexionAjusta()
        {
            string cs = ConfigurationManager.ConnectionStrings["Ajusta"].ConnectionString;
            return new FbConnection(cs);
        }
        private FbConnection ObtenerConexionMaster()
        {
            string cs = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;
            return new FbConnection(cs);
        }

        public FbConnection ConexionObtener(string ANombreDB)
        {
            switch (ANombreDB)
            {
                case "Adicional": return ObtenerConexionAdicional();
                case "GasConsola": return ObtenerConexionConsola();
                case "Ajusta": return ObtenerConexionAjusta();
                case "Master": return ObtenerConexionMaster();
                default: return null;
            }
        }
    }
}
