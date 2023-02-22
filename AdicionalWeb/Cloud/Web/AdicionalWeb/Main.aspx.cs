using System;
using System.Linq;
using AdicionalWeb.Code;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace AdicionalWeb
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errMsj = string.Empty;
            try
            {
                if (this.Request.QueryString["mng"] != null)
                {
                    errMsj = "los dispensarios";
                    Server.Transfer("~/pages/mangueras/manguera.aspx", true);
                }
                else if (this.Request.QueryString["flj"] != null)
                {
                    Server.Transfer("~/pages/flujos/flujo.aspx", true);
                }
                else if (this.Request.QueryString["rpt"] != null)
                {
                    Server.Transfer("~/pages/reports/RptVtasCombustible.aspx", true);
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(errMsj))
                {
                    Mensajeria.MostrarModalErr(string.Format("Ocurrió un fallo al intentar obtener {0}.", errMsj.ToLower()));
                }
            }
        }
    }
}
