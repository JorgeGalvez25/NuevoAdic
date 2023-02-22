using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace Persistencia
{
    public class Conexiones
    {
        public const string GASCONSOLA = "GasConsola";

        public const string ADICIONAL = "Adicional";

        public const string AJUSTADOR = "Ajusta";

        private FbConnection ObtenerConexionAdicional()
        {
            var cs = ConfigurationManager.ConnectionStrings[ADICIONAL].ConnectionString;
            return new FbConnection(cs);
        }

        private FbConnection ObtenerConexionConsola()
        {
            var cs = ConfigurationManager.ConnectionStrings[GASCONSOLA].ConnectionString;
            return new FbConnection(cs);
        }

        private FbConnection ObtenerConexionAjustador()
        {
            var cs = ConfigurationManager.ConnectionStrings[AJUSTADOR].ConnectionString;
            return new FbConnection(cs);
        }

        public FbConnection ConexionObtener(string ANombreDB)
        {
            switch (ANombreDB)
            {
                case ADICIONAL: return ObtenerConexionAdicional();
                case GASCONSOLA: return ObtenerConexionConsola();
                case AJUSTADOR: return ObtenerConexionAjustador();
                default: return null;
            }
        }
    }
}
