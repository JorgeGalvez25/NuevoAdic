using System.Web;
using ImagenSoft.ModuloWeb.Entidades.Web;

public class AdminSession
{
    public static string ID = "usuario";

    public static string USUARIOS = "usuarios";

    public static string PRIVILEGIOS = "Privilegios";

    public static string MODULO_WEB = "MODULO_WEB";

    public static string ESTACIONES = "estaciones";
    public static string ESTACION = "estacion";

    public static void CrearSession(ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesion, UsuarioWeb usuario)
    {
        var context = HttpContext.Current;

        //var expire = DateTime.Now.AddHours(2D);
        //context.Session.Timeout = (int)TimeSpan.FromTicks(expire.Ticks - DateTime.Now.Ticks).TotalMinutes;
        context.Session[ID] = usuario;
        context.Session[MODULO_WEB] = sesion;
    }
}