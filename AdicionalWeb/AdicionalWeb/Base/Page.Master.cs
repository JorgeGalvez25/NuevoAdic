using System;
using System.Linq;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia;

namespace AdicionalWeb.Base
{
    public partial class Page : System.Web.UI.MasterPage
    {
        public static int NoEstacion;
        public static ListaEstaciones lstEstaciones = new ListaEstaciones();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[AdminSession.ID] == null)
            {
                Response.Redirect("~/?NoSesion=1");
                return;
            }

            if (lstEstaciones.Count <= 0)
            {
                EstacionesPersistencia servicio = new EstacionesPersistencia();
                lstEstaciones.Clear();
                lstEstaciones.AddRange(servicio.EstacionObtenerTodos(new FiltroEstaciones()));

                var first = lstEstaciones.OrderBy(p => p.Clave).FirstOrDefault();
                NoEstacion = first == null ? 1 : first.Clave;
            }

            lstEstaciones.RemoveAll(p => !EstacionesAdicionalPersistencia.ListaEstaciones.ContainsKey(p.Clave));

            if (this.IsPostBack)
            {
                var first = lstEstaciones.OrderBy(p => p.Clave).FirstOrDefault();
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
                this.Request.Cookies.Clear();
                this.Session.Clear();
                this.Session.RemoveAll();
                this.Session.Abandon();
            }
            finally
            {
                this.Response.Redirect("~/", true);
            }
        }
    }
}
