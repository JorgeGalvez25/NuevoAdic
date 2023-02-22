using System;
using AdicionalCloud = ImagenSoft.ModuloWeb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdicionalWeb.Code;
using System.Text.RegularExpressions;

namespace AdicionalWeb.Login
{
    public partial class LoginRecovery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnRecuperar_ServerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoEstacion.Value.Trim()))
            {
                Mensajeria.ConfigurarToolTip(this.txtNoEstacion, "No. Estación, no puede ser vacía");
                Mensajeria.MostrarStaticToolTip();
                return;
            }
            else if (!Regex.IsMatch(this.txtNoEstacion.Value.Trim(), "^(E[0-9]{5})", RegexOptions.Compiled))
            {
                Mensajeria.ConfigurarToolTip(this.txtNoEstacion, "No. Estación tiene un formato inválido.");
                Mensajeria.MostrarStaticToolTip();
                return;
            }

            if (string.IsNullOrEmpty(this.txtUsuario.Value.Trim()))
            {
                Mensajeria.ConfigurarToolTip(this.txtUsuario, "Usuario, no puede ser vacía");
                Mensajeria.MostrarStaticToolTip();
                return;
            }

            var emptySesion = new AdicionalCloud.Entidades.SesionModuloWeb();
            Uri uri = HttpContext.Current.Request.Url;
            string host = string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, (uri.Port == 80) ? string.Empty : ":" + uri.Port);

            try
            {
                AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor srvAdicional = new AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor(emptySesion, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.Monitor);
                if (srvAdicional.AdministrarUsuariosClienteSolicitarContrasenia(emptySesion, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes()
                {
                    NoEstacion = this.txtNoEstacion.Value.Trim(),
                    Usuario = this.txtUsuario.Value.Trim(),
                    Host = host
                }))
                {
                    Response.Redirect("~/Restore.aspx", true);
                }
                else
                {
                    Mensajeria.MostrarModalErr("No fue posible cambiar su contraseña, por favor intentelo más tarde.");
                }
            }
            catch (Exception)
            {
                Mensajeria.MostrarModalErr("Ocurrio un error inesperado, por favor intentelo más tarde. Si el problema persiste favor de reportarlo.");
            }
        }
    }
}
