using System;
using System.Configuration;
using System.Web.Security;
using AdicionalWeb.Code;
using AdicionalWeb.Extesiones;
using AdicionalWeb.Persistencia.Validaciones;
using ImagenSoft.ModuloWeb.Entidades.Web;

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
                string expira = ConfigurationManager.AppSettings["validaExpira"] ?? "10";
                int iExpira = 0;
                if (!int.TryParse(expira, out iExpira)) { iExpira = 10; }

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

            UsuarioWeb usuario = ((UsuarioWeb)Cache["usuario"]).Clone();
            var key = (Cache["GeneratedKey"] ?? string.Empty).Clone().ToString();
            ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb sesionCloud = ((ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)Cache["sesionCloud"].Clone());

            if (timeAuth.CheckCode(key, this.txtValidacion.Value.Trim(), null))
            {
                if (Cache["usuario"] != null) { Cache.Remove("usuario"); }
                if (Cache["GeneratedKey"] != null) { Cache.Remove("GeneratedKey"); }
                if (Cache["sesionCloud"] != null) { Cache.Remove("sesionCloud"); }
                Session.Clear();
                Session.RemoveAll();
                AdminSession.CrearSession(sesionCloud, usuario);
                FormsAuthentication.SetAuthCookie(usuario.Usuario, true);
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
