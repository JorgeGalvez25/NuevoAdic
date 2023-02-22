using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdicionalWeb.Code;

namespace AdicionalWeb
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errMsj = string.Empty;
            try
            {
                if (this.Request.QueryString["flj"] != null)
                {
                    Server.Transfer("~/pages/flujos/flujo.aspx", true);
                }
                else if (this.Request.QueryString["mng"] != null)
                {
                    Server.Transfer("~/pages/mangueras/manguera.aspx", true);
                }
            }
            catch (Exception ex)
            {
                Mensajeria.MostrarModalErr(string.Format("Ocurrió un fallo al intentar obtener los {0}.", errMsj.ToLower()));
                //EventExceptionLog.WriteToEventLog(ex, string.Format("Transferir_{0}", errMsj));
            }
        }
    }
}
