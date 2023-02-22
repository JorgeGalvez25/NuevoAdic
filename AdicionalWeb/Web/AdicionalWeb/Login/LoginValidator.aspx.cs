using System;
using System.Configuration;
using System.Web.Security;
using AdicionalWeb.Code;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia.Validaciones;

namespace AdicionalWeb.Login
{
    public partial class LoginValidator : System.Web.UI.Page
    {
        private static readonly UsedCodesManager usedCodesManager = new UsedCodesManager();
        private static TimeAuthenticator timeAuth;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (timeAuth == null)
            {
                string expira = ConfigurationManager.AppSettings["validaExpira"] ?? "5";
                int iExpira = 0;
                if (!int.TryParse(expira, out iExpira)) { iExpira = 5; }

                timeAuth = new TimeAuthenticator(usedCodesManager, null, 60 * iExpira);
            }
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtValidacion.Value.Trim()))
            {
                Mensajeria.MostrarModalErr("El campo de validación no debe ser vacío.");
                return;
            }
            Usuario usuario = ((Usuario)Cache["usuario"]).Clone();
            if (Cache["usuario"] != null) { Cache.Remove("usuario"); }
            var key = (Cache["GeneratedKey"] ?? "").Clone().ToString();
            if (Cache["GeneratedKey"] != null) { Cache.Remove("GeneratedKey"); }

            if (timeAuth.CheckCode(key, this.txtValidacion.Value.Trim(), null))
            {
                Session.Clear();
                Session.RemoveAll();
                AdminSession.CrearSession(usuario);
                FormsAuthentication.SetAuthCookie(usuario.Nombre, true);
                this.Response.Redirect("~/Main.aspx", true);
            }
            else
            {
                Mensajeria.MostrarModalErr("El código es inválido o ya expiró, intente nuevamente.");
                return;
            }
        }
    }
}
