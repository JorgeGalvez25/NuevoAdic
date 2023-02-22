using System;
using System.Configuration;
using System.Text;
using System.Threading;
using AdicionalWeb.Code;
using AdicionalWeb.Code.SMTP;
using AdicionalWeb.Entidades;
using AdicionalWeb.Persistencia;
using AdicionalWeb.Persistencia.Validaciones;

namespace AdicionalWeb.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["NoSesion"] != null &&
                    this.Request.QueryString["NoSesion"] == "1")
                {
                    Mensajeria.MostrarModalInfo("No tiene sesión activa.");
                }
            }
        }

        public void btnAcceder_ServerClick(object sender, EventArgs e)
        {
            LoginPersistencia servicio = new LoginPersistencia();

            if (string.IsNullOrEmpty(this.txtUsuario.Value.Trim()))
            {
                Mensajeria.ConfigurarToolTip(this.txtUsuario, "No puede ser vacía");
                Mensajeria.MostrarStaticToolTip();
                return;
            }

            if (string.IsNullOrEmpty(this.txtPassword.Value.Trim()))
            {
                Mensajeria.ConfigurarToolTip(this.txtPassword, "No puede ser vacía");
                Mensajeria.MostrarStaticToolTip();
                return;
            }

            var entidad = servicio.LoginObtener(new AdicionalWeb.Entidades.FiltroSesion() { Nombre = this.txtUsuario.Value });

            if (entidad == null)
            {
                Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                return;
            }

            if (!entidad.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
            {
                Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                return;
            }

            if (!entidad.Clave.Equals(this.txtPassword.Value.Trim()))
            {
                Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                return;
            }

            Usuario usuario = new Usuario();
            {
                usuario.Nombre = entidad.Nombre;
                usuario.Password = entidad.Clave;
                usuario.Correo = entidad.Correo;
            }

            var key = TimeAuthenticator.GenerateKey();

            string expira = ConfigurationManager.AppSettings["validaExpira"] ?? "5";
            int iExpira = 0;
            if (!int.TryParse(expira, out iExpira)) { iExpira = 5; }

            var auth = new TimeAuthenticator(null, null, 60 * iExpira);
            var code = string.Empty;

            do { code = auth.GetCode(key).Trim(); }
            while (!auth.CheckCode(key, code));

            SMTPManager smtp = new SMTPManager();
            SMTPParameter parameters = new SMTPParameter();
            {
                parameters.Destinatary.Add(usuario.Correo);
                parameters.Subject = "Validación en 2 pasos";
                var template = smtp.LoadTemplate(AdicionalUtils.CombinePaths(ConstantesAdicional.Login.PATH_TEMPLATE, "mail.html"));
                parameters.Message = string.IsNullOrEmpty(template) ? code.ToString() : template.ReplaceEx("[{codigo}]", code);
            }

            string msj = string.Empty;
            if (!smtp.Send(parameters, ref msj))
            {
                Mensajeria.MostrarModalInfo(msj);
                return;
            }

            DateTime _fecha = DateTime.Now.AddMinutes(iExpira);

            if (Cache.Get("GeneratedKey") != null) { Cache.Remove("GeneratedKey"); }
            Cache.Add("GeneratedKey", key, null, DateTime.MaxValue, _fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);

            if (Cache.Get("usuario") != null) { Cache.Remove("usuario"); }
            Cache.Add("usuario", usuario, null, DateTime.MaxValue, _fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);

            Thread.Sleep(1000 * 2);
            Server.Transfer("~/Login/LoginValidator.aspx", true);
        }

        private long GoogleAuthenticatorCode(string secret)
        {
            var key = Base32Encoding.ToBytes(secret);
            var message = DateTime.Now.Ticks / 30;
            var hmacSha1 = new System.Security.Cryptography.HMACSHA1(key);
            var hash = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(message.ToString()));
            long truncatedHash = 0L;
            StringBuilder sb = new StringBuilder();

            for (int i = hash.Length - 4; i < hash.Length; i++)
            {
                sb.Append(hash[i]);
            }

            truncatedHash = long.Parse(sb.ToString());
            long code = truncatedHash % 1000000L;

            //pad code with 0 until length of code is 6
            return code;
        }
    }
}
