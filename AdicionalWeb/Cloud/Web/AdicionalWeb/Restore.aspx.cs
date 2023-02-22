using System;
using System.Web;
using AdicionalWeb.Code;

namespace AdicionalWeb
{
    public partial class Restore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((Request.QueryString["id"] ?? string.Empty).Trim()))
            {
                Response.Redirect(string.Format("~/{0}", (Session[AdminSession.ID] == null)
                                                            ? string.Empty
                                                            : "Main.aspx").Trim(), true);
            }
            else
            {
                if (this.Validar())
                {
                    this.lblTexto.InnerHtml = "Para confirmar que se ha cambiado la contrase&ntilde;a inicie sesi&oacute;n con su nuevamente. " +
                                              string.Format("[<strong><a href=\"{0}\">Inicio</a></strong>]", ResolveUrl("~/"));
                }
                else
                {
                    Response.Redirect("~/", true);
                }
            }
        }

        private bool Validar()
        {
            var sesionCloud = (ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb)HttpContext.Current.Session[AdminSession.MODULO_WEB];

            if (sesionCloud == null)
            {
                sesionCloud = new ImagenSoft.ModuloWeb.Entidades.SesionModuloWeb()
                    {
                        Estacion = new ImagenSoft.ModuloWeb.Entidades.Web.Estacion()
                        {
                            NoEstacion = (Request.QueryString["est"] ?? string.Empty).Trim()
                        },
                        EstacionActual = new ImagenSoft.ModuloWeb.Entidades.Web.Estacion()
                        {
                            NoEstacion = (Request.QueryString["est"] ?? string.Empty).Trim()
                        }
                    };
            }

            long lId = 0L;
            if (!long.TryParse((Request.QueryString["id"] ?? "0").Trim(), out lId))
            {
                Mensajeria.MostrarModalErr("Ocurrio un problema al intentar cambiar la contraseña, vuelva a intentarlo más tarde por favor.");
                return false;
            }

            try
            {
                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.Monitor);
                if (!adicional.AdministrarUsuariosClienteRestablecerContrasenia(sesionCloud, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes()
                    {
                        NoEstacion = (Request.QueryString["est"] ?? string.Empty).Trim(),
                        Usuario = (Request.QueryString["usr"] ?? string.Empty).Trim(),
                        Id = lId
                    }))
                {
                    Mensajeria.MostrarModalErr("Ocurrio un problema al intentar cambiar la contraseña, vuelva a intentarlo más tarde por favor.");
                    return false;
                }
                else
                {
                    Response.Redirect(string.Format("~/{0}", (Session[AdminSession.ID] == null)
                                                                ? string.Empty
                                                                : "Main.aspx").Trim(), true);
                }
            }
            catch (Exception e)
            {
                Mensajeria.MostrarModalErr("Ocurrio un error inesperado, vuelva a intentarlo más tarde por favor.");
                return false;
            }

            return true;
        }
    }
}
