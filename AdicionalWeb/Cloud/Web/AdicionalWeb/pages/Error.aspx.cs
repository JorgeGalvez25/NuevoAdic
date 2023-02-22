using System;
using System.Text;

namespace AdicionalWeb.pages
{
    public partial class Error : System.Web.UI.Page
    {
        public string ErrorMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Exception caughtException = (Exception)Application["TheException"];

            StringBuilder sb = new StringBuilder().AppendFormat("Mensaje: {0}", caughtException.Message).AppendLine();

            Exception aux = caughtException.InnerException;
            while (aux != null)
            {
                sb.AppendLine(aux.Message);
                aux = aux.InnerException;
            }

            ErrorMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString().Trim()));
        }
    }
}
