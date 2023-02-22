using System;

namespace AdicionalWeb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_PreRequestHandlerExecute(object src, EventArgs e)
        {
            if (AdicionalWeb.Persistencia.EstacionesAdicionalPersistencia.ListaEstaciones.Count <= 0)
            {
                DateTime fecha = DateTime.Now.AddSeconds(30);

                if (this.Context.Cache.Get("MensajeTtl") == null)
                {
                    this.Context.Cache.Add("MensajeTtl", "Aplicación", null, DateTime.MaxValue, fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);
                }

                if (this.Context.Cache.Get("Mensaje") == null)
                {
                    this.Context.Cache.Add("Mensaje", "Aplicación expiró.", null, DateTime.MaxValue, fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);
                }

                if (this.Context.Handler is System.Web.UI.Page)
                {
                    this.Context.Server.Transfer("~/pages/mensajes/mensaje.aspx", false);
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}