using System;
using System.Web;
using AdicionalWeb.Entidades;

public class AdminSession
{
    public static string ID = "usuario";

    public static string ESTACIONES = "estaciones";
    public static string ESTACION = "estacion";

    public static void CrearSession(Usuario usuario)
    {
        var context = HttpContext.Current;
        var expire = DateTime.Now.AddHours(2D);
        context.Session.Timeout = expire.TimeOfDay.Minutes;
        context.Session[ID] = usuario;
    }
}