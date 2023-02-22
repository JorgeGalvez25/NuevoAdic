
public class ConstantesPersistencia
{
    public const string NOMBRE_SERVICIO = "Modulo Web - Host Adicional";

    public const string APP_SETTING_CONNECTION_STRING = "moduloweb";

    public const int ID_ADMINISTRADOR = 0;

    public const int ID_DISTRIBUIDOR_MATRIZ = 1;

    private static string _connectionString;

    public static string ConnectionString
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings[ConstantesPersistencia.APP_SETTING_CONNECTION_STRING].ConnectionString;

                System.Data.SqlClient.SqlConnectionStringBuilder sb = new System.Data.SqlClient.SqlConnectionStringBuilder(strConn);
                sb.AsynchronousProcessing = true;
                sb.ApplicationName = ConstantesPersistencia.NOMBRE_SERVICIO;
                sb.PersistSecurityInfo = true;
                sb.MultipleActiveResultSets = true;
                sb.Pooling = true;
                sb.ConnectTimeout = 60 * 2; // 2 Minutos
                sb.LoadBalanceTimeout = 60 * 5; // 5 Minutos
                sb.PacketSize = short.MaxValue;// 32,767 bytes -> 32.767 Kb

                _connectionString = sb.ToString();
            }

            return _connectionString;
        }
    }
}