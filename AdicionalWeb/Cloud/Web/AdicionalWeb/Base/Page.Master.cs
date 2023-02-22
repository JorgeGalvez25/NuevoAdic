using System;
using System.Linq;
using ImagenSoft.ModuloWeb.Entidades.Web;

namespace AdicionalWeb.Base
{
    public partial class Page : System.Web.UI.MasterPage
    {
        public int NoEstacion;
        public UsuarioWeb UsuarioActual
        {
            get
            {
                return (Session[AdminSession.ID] as UsuarioWeb);
            }
        }
        public ListaEstaciones lstEstaciones
        {
            get
            {
                return (Session[AdminSession.MODULO_WEB] as ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb).Estaciones;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[AdminSession.ID] == null)
            {
                Response.Redirect("~/?NoSesion=1");
                return;
            }

            var first = lstEstaciones.OrderBy(p => p.Clave).FirstOrDefault();
            NoEstacion = first == null ? 1 : first.Clave;

            //lstEstaciones.RemoveAll(p => !EstacionesAdicionalPersistencia.ListaEstaciones.ContainsKey(p.Clave));

            if (this.IsPostBack)
            {
                //var first = lstEstaciones.OrderBy(p => p.Clave).FirstOrDefault();
                int estacion = first == null ? 1 : first.Clave;

                if (this.Request.QueryString.HasKeys())
                {
                    if (!int.TryParse(this.Request.QueryString["est"], out estacion))
                    {
                        estacion = first == null ? 1 : first.Clave;
                    }
                }
                NoEstacion = estacion;
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                this.Session.Abandon();
                this.Session.Clear();
                this.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                this.Response.ExpiresAbsolute = DateTime.UtcNow.AddDays(-1d);
                this.Response.Expires = -1500;
                this.Response.CacheControl = "no-Cache";
                this.Response.Cookies.Clear();
                this.Request.Cookies.Clear();

                //this.Request.Cookies.Clear();
                //this.Session.Abandon();
                //this.Session.Clear();
                //this.Session.RemoveAll();
                //System.Web.HttpRuntime.Close();
                ////System.Web.HttpContext.Current.Cache
                //System.Web.Security.FormsAuthentication.SignOut();
            }
            finally
            {
                this.Response.Redirect("~/", true);
            }
        }
    }
}
