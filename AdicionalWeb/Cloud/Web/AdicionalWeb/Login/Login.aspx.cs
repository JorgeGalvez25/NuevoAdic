using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using AdicionalWeb.Code;
using AdicionalWeb.Code.SMTP;
using AdicionalWeb.Extesiones;
using AdicionalWeb.Persistencia.Validaciones;
using ImagenSoft.ModuloWeb.Entidades.Web;
using AdicionalCloud = ImagenSoft.ModuloWeb;

namespace AdicionalWeb.Login
{
    public partial class Login : System.Web.UI.Page
    {
        private class ConstantesLogin
        {
            public const string INFO_FLAG = "-i|";

            public const string WARN_FLAG = "-w|";

            public const string ERR_FLAG = "-e|";

            public const string INVALID_USER_PASS = "Usuario o contraseña inválida";
        }

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


        private T getAndValidateData<T>(Dictionary<string, object> data, string toFind)
        {
            KeyValuePair<string, object> obj = data.FirstOrDefault(p => p.Key
                                                                         .ToLower()
                                                                         .Contains(toFind.ToLower()));

            if (obj.Key == null) { return default(T); }
            return (T)obj.Value;
        }

        private Parametros GetParameters(string data)
        {
            List<int> items = Encoding.UTF8.GetString(Convert.FromBase64String(data)).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                     .Select(p => int.Parse(p))
                                                                                     .ToList();
            string decompressed = AdicionalUtils.Decompress(items);
            Dictionary<string, object> parametros = AdicionalUtils.JSSerializer.Deserialize<Dictionary<string, object>>(decompressed);

            Parametros result = new Parametros();
            result.Usuario = (getAndValidateData<string>(parametros, "txtUsuario") ?? string.Empty).Trim();
            result.Estacion = (getAndValidateData<string>(parametros, "txtNoEstacion") ?? string.Empty).Trim();
            result.Password = (getAndValidateData<string>(parametros, "txtPassword") ?? string.Empty).Trim();

            return result;
        }


        public void btnAcceder_ServerClick(object sender, EventArgs e)
        {
            //Parametros parametros = GetParameters(this.hdValues.Value);

            //if (string.IsNullOrEmpty(parametros.Estacion.Trim()))
            //{
            //    Mensajeria.ConfigurarToolTip(this.txtNoEstacion, "No. Estación, no puede ser vacía");
            //    Mensajeria.MostrarStaticToolTip();
            //    return;
            //}
            //else if (!Regex.IsMatch(parametros.Estacion.Trim(), "^(E[0-9]{5})", RegexOptions.Compiled))
            //{
            //    Mensajeria.ConfigurarToolTip(this.txtNoEstacion, "No. Estación tiene un formato inválido.");
            //    Mensajeria.MostrarStaticToolTip();
            //    return;
            //}

            //if (string.IsNullOrEmpty(parametros.Usuario.Trim()))
            //{
            //    Mensajeria.ConfigurarToolTip(this.txtUsuario, "Usuario, no puede ser vacía");
            //    Mensajeria.MostrarStaticToolTip();
            //    return;
            //}

            //if (string.IsNullOrEmpty(parametros.Password.Trim()))
            //{
            //    Mensajeria.ConfigurarToolTip(this.txtPassword, "Password, no puede ser vacía");
            //    Mensajeria.MostrarStaticToolTip();
            //    return;
            //}


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

            if (string.IsNullOrEmpty(this.txtPassword.Value.Trim()))
            {
                Mensajeria.ConfigurarToolTip(this.txtPassword, "Password, no puede ser vacía");
                Mensajeria.MostrarStaticToolTip();
                return;
            }

            var emptySesion = new AdicionalCloud.Entidades.SesionModuloWeb()
                {
                    Empresa = new ImagenSoft.ModuloWeb.Entidades.Base.DatosEmpresa() { Id = 1 }
                };

            AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor srvAdicional = new AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor(emptySesion, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.UsuarioWeb);
            string md5Password = this.txtPassword.Value.GetMD5().Trim();
            AdicionalCloud.Entidades.Web.UsuarioWeb entidad = new AdicionalCloud.Entidades.Web.UsuarioWeb()
                {
                    NoEstacion = txtNoEstacion.Value,//this.txtNoEstacion.Value,
                    Usuario = txtUsuario.Value,//this.txtUsuario.Value,
                    Password = md5Password//this.txtPassword.Value.GetMD5().Trim()
                };

            AdicionalCloud.Entidades.SesionModuloWeb sesionCloud = null;
            try
            {
                sesionCloud = srvAdicional.AdicionalWebValidarLogin(emptySesion, entidad);

                if (sesionCloud == null)
                {
                    Mensajeria.MostrarModalErr(ConstantesAdicional.NO_SESION);
                    return;
                }

                if (Cache.Get("sesionCloud") != null) { Cache.Remove("sesionCloud"); }
                Cache.Add("sesionCloud", sesionCloud, null, DateTime.MaxValue, DateTime.Now.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);

                if (sesionCloud.Usuario == null)
                {
                    Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                    return;
                }

                if (!sesionCloud.Usuario.Password.Equals(md5Password))
                {
                    Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                    return;
                }

                //if (!sesionCloud.Usuario.Password.Equals(this.txtPassword.Value.GetMD5().Trim()))
                //{
                //    Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_CONTRASENIA_INVALIDA);
                //    return;
                //}

                if (string.IsNullOrEmpty(sesionCloud.Usuario.CorreoElectronico))
                {
                    Mensajeria.MostrarModalErr(ConstantesAdicional.Login.MSJ_USUARIO_SIN_CORREO);
                    return;
                }

                entidad.Correo = sesionCloud.Usuario.CorreoElectronico;
                entidad.Estacion = sesionCloud.Estacion;
                entidad.Privilegios = new object();

                AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor srvProveedor = new AdicionalCloud.Proveedor.Publicador.ServiciosModuloWebProveedor(sesionCloud, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.Monitor);
                AdministrarUsuariosClientes usrCliente = srvProveedor.AdministrarUsuariosClienteObtener(sesionCloud, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes()
                {
                    NoEstacion = sesionCloud.EstacionActual.NoEstacion,
                    Usuario = entidad.Usuario,
                    Matriz = sesionCloud.EstacionActual.Matriz
                });

                entidad.Privilegios = usrCliente.Privilegios;
                entidad.Rol = usrCliente.Rol;

                if (usrCliente.Privilegios.Configuraciones.Validacion2Pasos)
                {
                    if (this.generateAndSendKey(entidad))
                    {
                        Thread.Sleep(1000 * 2);
                        Server.Transfer("~/Login/LoginValidator.aspx", true);
                    }
                }
                else
                {
                    Session.Clear();
                    Session.RemoveAll();
                    AdminSession.CrearSession(sesionCloud, entidad);
                    FormsAuthentication.SetAuthCookie(entidad.Usuario, true);
                    Session[AdminSession.PRIVILEGIOS] = usrCliente.Privilegios;
                    this.Response.Redirect("~/Main.aspx", true);
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message.Trim();
                if (ex.Message.StartsWith(ConstantesLogin.INFO_FLAG))
                {
                    msj = ex.Message.Replace(ConstantesLogin.INFO_FLAG, string.Empty);
                    Mensajeria.MostrarModalInfo(msj);
                }
                else if (ex.Message.StartsWith(ConstantesLogin.WARN_FLAG))
                {
                    msj = ex.Message.Replace(ConstantesLogin.WARN_FLAG, string.Empty);
                    Mensajeria.MostrarModalWarn(msj);
                }
                else if (ex.Message.StartsWith(ConstantesLogin.ERR_FLAG))
                {
                    msj = ex.Message.Replace(ConstantesLogin.ERR_FLAG, string.Empty);
                    Mensajeria.MostrarModalErr(msj.Trim());
                }
                else
                {
                    msj = string.Format(ConstantesLogin.INVALID_USER_PASS);
                    Mensajeria.MostrarModalErr(msj.Trim());
                }
            }
            finally
            {
                this.hdValues.Value = string.Empty;
            }
        }

        public void btnRestore_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Login/LoginRecovery.aspx", true);
        }

        private bool generateAndSendKey(ImagenSoft.ModuloWeb.Entidades.Web.UsuarioWeb entidad)
        {
            var key = TimeAuthenticator.GenerateKey();

            string expira = ConfigurationManager.AppSettings["validaExpira"] ?? "10";
            int iExpira = 0;
            if (!int.TryParse(expira, out iExpira)) { iExpira = 10; }

            var auth = new TimeAuthenticator(null, null, 60 * iExpira);
            var code = string.Empty;

            do { code = auth.GetCode(key).Trim(); }
            while (!auth.CheckCode(key, code));

            SMTPManager smtp = new SMTPManager();
            SMTPParameter parameters = new SMTPParameter();
            {
                parameters.Destinatary.Add(entidad.Correo);
                parameters.Subject = "Modulo Web - Validación en 2 pasos";
                var template = smtp.LoadTemplate(AdicionalUtils.CombinePaths(ConstantesAdicional.Login.PATH_TEMPLATE, "mail.html"));
                parameters.Message = string.IsNullOrEmpty(template) ? code.ToString() : template.ReplaceEx("[{codigo}]", code);
            }

            string msj = string.Empty;
            if (!smtp.Send(parameters, ref msj))
            {
                Mensajeria.MostrarModalErr(msj);
                return false;
            }

            DateTime _fecha = DateTime.Now.AddMinutes(iExpira);

            if (Cache.Get("GeneratedKey") != null) { Cache.Remove("GeneratedKey"); }
            Cache.Add("GeneratedKey", key, null, DateTime.MaxValue, _fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);

            if (Cache.Get("usuario") != null) { Cache.Remove("usuario"); }
            Cache.Add("usuario", entidad, null, DateTime.MaxValue, _fecha.TimeOfDay, System.Web.Caching.CacheItemPriority.Normal, null);

            return true;
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

        private class Parametros
        {
            public string Usuario { get; set; }

            public string Estacion { get; set; }

            public string Password { get; set; }
        }
    }
}
