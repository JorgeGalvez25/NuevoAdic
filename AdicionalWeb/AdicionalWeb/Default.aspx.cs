using System;

namespace AdicionalWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Server.Transfer("~/Login/Login.aspx", true);
        }
    }
}
