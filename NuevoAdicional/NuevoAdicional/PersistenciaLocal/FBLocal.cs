using FirebirdSql.Data.FirebirdClient;

namespace NuevoAdicional
{
    public class FBLocal
    {
        private static FbConnection Conexion;
        private static string cadenaConexion = @"User=SYSDBA;Password=masterkey;Database=[BaseDatos];DataSource=localhost; Port=3050;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true; MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";

        static FBLocal()
        {
            Conexion = null;
        }

        public static FbConnection ConexionObtener()
        {
            if (Conexion != null)
            {
                return Conexion;
            }

            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string ruta = string.Empty;
            FbConnectionStringBuilder sb = new FbConnectionStringBuilder();

            try
            {
                ruta = appReader.GetValue("BaseDatos", typeof(string)).ToString();
            }
            catch
            {
                ruta = @"c:\imagenco\dbi\NADICIONAL_CLIENT.FDB";
            }

            cadenaConexion = cadenaConexion.Replace("[BaseDatos]", ruta);

            Conexion = new FbConnection(cadenaConexion);

            return Conexion;
        }
    }
}
