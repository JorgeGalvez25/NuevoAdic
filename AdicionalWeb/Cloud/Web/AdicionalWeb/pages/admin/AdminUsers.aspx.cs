using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using AdicionalWeb.Code;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using System.Text;

namespace AdicionalWeb.pages.admin
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        public ListaEstaciones estaciones
        {
            get
            {
                return ((SesionModuloWeb)Session[AdminSession.MODULO_WEB]).Estaciones ?? new ListaEstaciones();
            }
        }
        public ListaAdministrarUsuariosClientes usuarios
        {
            get
            {
                return ((ListaAdministrarUsuariosClientes)Session[AdminSession.USUARIOS]) ?? new ListaAdministrarUsuariosClientes();
            }
        }

        public bool EsAdministrador;
        private Regex rgNoEstacion = new Regex(@"^E(\d){5}$", RegexOptions.Compiled);
        private Regex rgEmail = new Regex(@"([a-zA-Z0-9_\-\.]{1,40})@([a-zA-Z0-9_\-\.]{1,34})\.([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.Compiled);

        public AdministrarUsuariosClientes usuarioActual
        {
            get
            {
                UsuarioWeb usrActual = (Session[AdminSession.ID] as UsuarioWeb);
                return usuarios.Find(p => p.Usuario.Equals(usrActual.Usuario));
            }
        }

        public ImagenSoft.ModuloWeb.Entidades.Web.Privilegios PermisosActuales
        {
            get
            {
                AdministrarUsuariosClientes usr = usuarioActual;
                if (usr == null) return new ImagenSoft.ModuloWeb.Entidades.Web.Privilegios();

                if (ConstantesModuloWeb.Roles.MAESTRO.Equals(usr.Rol, StringComparison.OrdinalIgnoreCase))
                {
                    return new Privilegios()
                    {
                        Configuraciones = new Configuraciones()
                        {
                            Validacion2Pasos = true
                        },
                        Permisos = new ImagenSoft.ModuloWeb.Entidades.Web.Permisos()
                        {
                            CambiarPassword = true,
                            VerTodasEstaciones = true
                        }
                    };
                }

                return usr.Privilegios;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();

            this.Inicializar();
        }


        private void Inicializar()
        {
            string errMsj = string.Empty;
            try
            {
                SesionModuloWeb current = (Session[AdminSession.MODULO_WEB] as SesionModuloWeb);
                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(current, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.Monitor);
                ListaAdministrarUsuariosClientes aux = adicional.AdministrarUsuariosClienteObtenerTodosFiltro(current, new ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes()
                {
                    Matriz = current.EstacionActual.Matriz
                });

                if (aux != null && aux.Count > 0)
                {
                    UsuarioWeb usrActual = (Session[AdminSession.ID] as UsuarioWeb);
                    AdministrarUsuariosClientes clienteActual = aux.Find(p => p.Usuario.Equals(usrActual.Usuario));


                    EsAdministrador = ConstantesModuloWeb.Roles.MAESTRO.Equals(clienteActual.Rol, StringComparison.OrdinalIgnoreCase);
                    usuarios.Clear();

                    ListaAdministrarUsuariosClientes _usuarios = new ListaAdministrarUsuariosClientes();

                    if (EsAdministrador)
                    {
                        this.hdActualRol.Value = 1.ToString();
                        _usuarios.AddRange(aux.OrderBy(p => p.NoEstacion));
                    }
                    else
                    {
                        this.hdActualRol.Value = 0.ToString();
                        _usuarios.Add(clienteActual);
                    }

                    _usuarios.ForEach(p => p.Password = string.Empty);

                    if (Session[AdminSession.USUARIOS] != null)
                    {
                        (Session[AdminSession.USUARIOS] as ListaAdministrarUsuariosClientes).Clear();
                        (Session[AdminSession.USUARIOS] as ListaAdministrarUsuariosClientes).AddRange(_usuarios);
                    }
                    else
                    {
                        Session[AdminSession.USUARIOS] = _usuarios;
                    }

                    if (!this.IsPostBack)
                    {
                        this.cmbEstacionUsuario.Items.Clear();
                        this.cmbEstacionUsuario.Items.AddRange(_usuarios.Select(p => p.NoEstacion)
                                                                        .Distinct()
                                                                        .Select(p => new ListItem(p, p))
                                                                        .ToArray());
                        this.cmbEstacionUsuario.Items.Insert(0, new ListItem("Seleccione una...", "Ninguno"));
                        this.cmbEstacionUsuario.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(errMsj))
                {
                    Mensajeria.MostrarModalErr(string.Format("Ocurrió un fallo desconocido.", errMsj.ToLower()));
                }
                //EventExceptionLog.WriteToEventLog(ex, string.Format("Transferir_{0}", errMsj));
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
            List<int> items = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(p => int.Parse(p))
                                  .ToList();
            string decompressed = AdicionalUtils.Decompress(items);
            Dictionary<string, object> parametros = AdicionalUtils.JSSerializer.Deserialize<Dictionary<string, object>>(decompressed);

            Parametros result = new Parametros();
            result.Usuario = (getAndValidateData<string>(parametros, "hdUsuario") ?? string.Empty).Trim();
            result.Estacion = (getAndValidateData<string>(parametros, "hdEstacion") ?? string.Empty).Trim();
            result.Modo = (getAndValidateData<string>(parametros, "hdModo") ?? string.Empty).Trim();

            result.NewUsuario = (getAndValidateData<string>(parametros, "txtUsuario") ?? string.Empty).Trim();
            result.NewEstacion = (getAndValidateData<string>(parametros, "cmbEstacionUsuario") ?? string.Empty).Trim();
            result.NewMail = (getAndValidateData<string>(parametros, "txtMail") ?? string.Empty).Trim();
            result.NewNombre = (getAndValidateData<string>(parametros, "txtNombre") ?? string.Empty).Trim();
            result.NewActivo = "On".Equals(getAndValidateData<string>(parametros, "chkActivo"), StringComparison.OrdinalIgnoreCase);

            result.CambiarPassword = "On".Equals(getAndValidateData<string>(parametros, "chkChangePassword"));
            result.Password = (getAndValidateData<string>(parametros, "hdPassword") ?? string.Empty).Trim();
            result.NewPassword = (getAndValidateData<string>(parametros, "hdNewPassword") ?? string.Empty).Trim();
            result.ConfirmPassword = (getAndValidateData<string>(parametros, "hdConfirmPassword") ?? string.Empty).Trim();

            result.CambiarPrivilegios = "On".Equals(getAndValidateData<string>(parametros, "chkPrivilegios"), StringComparison.OrdinalIgnoreCase);
            result.Validacion2Pasos = "On".Equals(getAndValidateData<string>(parametros, "chkValidacion2Pasos"), StringComparison.OrdinalIgnoreCase);
            result.CambiarSuPassword = "On".Equals(getAndValidateData<string>(parametros, "chkCambiarPass"), StringComparison.OrdinalIgnoreCase);
            result.VerEstaciones = "On".Equals(getAndValidateData<string>(parametros, "chkVerEstaciones"), StringComparison.OrdinalIgnoreCase);

            return result;
        }


        public void btnAceptar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //Parametros data = this.GetParameters(this.hdValues.Value);

                //if (string.IsNullOrEmpty(data.NewUsuario))
                //{
                //    Mensajeria.MostrarModalErr("No fue posible modificar el usuario seleccionado, intentelo más tarde por favor");
                //    return;
                //}

                //if (string.IsNullOrEmpty(data.NewEstacion))
                //{
                //    Mensajeria.MostrarModalErr("Debe seleccionar una estación");
                //    this.cmbEstacionUsuario.Focus();
                //    return;
                //}

                //if (string.IsNullOrEmpty(data.NewNombre))
                //{
                //    Mensajeria.MostrarModalErr("El valor del campo 'Nombre' no puede ser vacío.");
                //    this.txtNombre.Focus();
                //    return;
                //}

                //if (string.IsNullOrEmpty(data.NewMail))
                //{
                //    Mensajeria.MostrarModalErr("El valor del campo 'Correo' no puede ser vacío.");
                //    this.txtMail.Focus();
                //    return;
                //}

                //if (!this.rgEmail.IsMatch(data.NewMail))
                //{
                //    Mensajeria.MostrarModalErr("El formato del campo 'Correo' es incorrecto.");
                //    this.txtMail.Focus();
                //    return;
                //}

                //if (data.CambiarPassword)
                //{
                //    if (string.IsNullOrEmpty(data.Password))
                //    {
                //        Mensajeria.MostrarModalErr("El valor del campo 'Contraseña' no puede ser vacío.");
                //        this.txtPassword.Focus();
                //        return;
                //    }

                //    if (string.IsNullOrEmpty(data.NewPassword))
                //    {
                //        Mensajeria.MostrarModalErr("El valor del campo 'Nueva contraseña' no puede ser vacío.");
                //        this.txtNewPassword.Focus();
                //        return;
                //    }

                //    if (string.IsNullOrEmpty(data.ConfirmPassword))
                //    {
                //        Mensajeria.MostrarModalErr("El valor del campo 'Confirmar contraseña' no puede ser vacío.");
                //        this.txtConfirm.Focus();
                //        return;
                //    }

                //    if (!data.NewPassword.Equals(data.ConfirmPassword))
                //    {
                //        Mensajeria.MostrarModalErr("El valor del campo 'Nueva contraseña' y 'Confirmar contraseña' son diferentes.");
                //        this.txtNewPassword.Focus();
                //        return;
                //    }
                //}


                if (string.IsNullOrEmpty(this.txtUsuario.Text.Trim()))
                {
                    Mensajeria.MostrarModalErr("No fue posible modificar el usuario seleccionado, intentelo más tarde por favor");
                    return;
                }

                if (string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
                {
                    Mensajeria.MostrarModalErr("El valor del campo \"Nombre\" no puede ser vacío.");
                    this.txtNombre.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.txtMail.Text.Trim()))
                {
                    Mensajeria.MostrarModalErr("El valor del campo \"Correo\" no puede ser vacío.");
                    this.txtMail.Focus();
                    return;
                }

                if (!this.rgEmail.IsMatch(this.txtMail.Text))
                {
                    Mensajeria.MostrarModalErr("El formato del campo \"Correo\" es incorrecto.");
                    this.txtMail.Focus();
                    return;
                }

                if (!this.rgNoEstacion.IsMatch(this.cmbEstacionUsuario.Value))
                {
                    Mensajeria.MostrarModalErr("Seleccione una estación.");
                    this.cmbEstacionUsuario.Focus();
                    return;
                }

                string modo = this.hdModo.Value.Trim();

                if (this.chkChangePassword.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtPassword.Text))
                    {
                        Mensajeria.MostrarModalErr("El valor del campo \"Contraseña\" no puede ser vacío.");
                        this.txtPassword.Focus();
                        return;
                    }

                    if (modo.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(this.txtNewPassword.Text))
                        {
                            Mensajeria.MostrarModalErr("El valor del campo \"Nueva contraseña\" no puede ser vacío.");
                            this.txtNewPassword.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.txtConfirm.Text))
                        {
                            Mensajeria.MostrarModalErr("El valor del campo \"Confirmar contraseña\" no puede ser vacío.");
                            this.txtConfirm.Focus();
                            return;
                        }

                        if (!this.txtNewPassword.Text.Equals(this.txtConfirm.Text))
                        {
                            Mensajeria.MostrarModalErr("El valor del campo \"Nueva contraseña\" y \"Confirmar contraseña\" son diferentes.");
                            this.txtNewPassword.Focus();
                            return;
                        }
                    }
                    else if (modo.Equals("R", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(this.txtConfirm.Text))
                        {
                            Mensajeria.MostrarModalErr("El valor del campo \"Confirmar contraseña\" no puede ser vacío.");
                            this.txtConfirm.Focus();
                            return;
                        }

                        if (!this.txtPassword.Text.Equals(this.txtConfirm.Text))
                        {
                            Mensajeria.MostrarModalErr("El valor del campo \"Nueva contraseña\" y \"Confirmar contraseña\" son diferentes.");
                            this.txtNewPassword.Focus();
                            return;
                        }
                    }
                }

                SesionModuloWeb current = (Session[AdminSession.MODULO_WEB] as SesionModuloWeb);
                ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional = new ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor(current, ImagenSoft.ModuloWeb.Entidades.Enumeradores.TipoConexionUsuario.Monitor);

                //string modo = (getAndValidateData<string>(data, "hdModo") ?? string.Empty).Trim();
                //string modo = this.hdModo.Value.Trim();

                //switch (data.Modo.ToUpper())
                switch (modo.ToUpper())
                {
                    case "M":
                        //if (this.Modificar(data, current, adicional))
                        if (this.Modificar(null, current, adicional))
                        {
                            Mensajeria.MostrarModalInfo("Se han realizado los cambios correctamente.");
                        }
                        break;
                    case "R":
                        //if (this.Registrar(data, current, adicional))
                        if (this.Registrar(null, current, adicional))
                        {
                            Mensajeria.MostrarModalInfo("Se han realizado los cambios correctamente.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Mensajeria.MostrarModalErr("Ocurrio un error inesperado favor de intentar nuevamente.");
                this.hdValues.Value = ex.Message;
            }
            finally
            {
                //this.hdValues.Value = string.Empty;
                this.Inicializar();
            }
        }

        private bool Modificar(Parametros data, SesionModuloWeb current, ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional)
        {
            //string strEstacion = (getAndValidateData<string>(data, "hdEstacion") ?? string.Empty).Trim();
            //string strUsuario = (getAndValidateData<string>(data, "hdUsuario") ?? string.Empty).Trim();

            AdministrarUsuariosClientes usuario = adicional.AdministrarUsuariosClienteObtener(current, new FiltroAdministrarUsuariosClientes()
            {
                NoEstacion = hdEstacion.Value.Trim(),
                Matriz = current.EstacionActual.Matriz,
                Usuario = hdUsuario.Value.Trim()
            });

            if (usuario == null)
            {
                Mensajeria.MostrarModalErr(string.Format("El usuario {0} no existe actualice la paágina y vuelva a intentarlo por favor.", this.hdUsuario.Value.Trim()));
                return false;
            }

            usuario.Activo = this.chkActivo.Checked ? "Si" : "No";
            usuario.Correo = this.txtMail.Text.Trim();
            usuario.Matriz = current.EstacionActual.Matriz;
            usuario.NoEstacion = this.hdEstacion.Value.Trim();
            usuario.NuevoNoEstacion = this.cmbEstacionUsuario.Value.Trim();
            usuario.Nombre = this.txtNombre.Text.Trim();
            usuario.NuevoUsuario = this.txtUsuario.Text.Trim();
            usuario.Usuario = this.hdUsuario.Value.Trim();
            bool result = true;

            if (adicional.AdministrarUsuariosClienteModificar(current, usuario) != null)
            {
                if (this.chkChangePassword.Checked)
                {
                    //if (usuario.Rol.Equals("Maestro", StringComparison.OrdinalIgnoreCase) || usuario.Privilegios.Permisos.CambiarPassword)
                    //if (usuario.Privilegios.Permisos.CambiarPassword)
                    //{
                    usuario.Password = this.txtConfirm.Text.Trim();
                    if (!adicional.AdministrarUsuariosClienteCambiarContrasenia(current, usuario))
                    {
                        Mensajeria.MostrarModalErr("No fue posible modificar la contraseña, intentelo más tarde por favor");
                        result = false;
                    }
                    //}
                }

                if (this.chkPrivilegios.Checked)
                {
                    if (ConstantesModuloWeb.Roles.MAESTRO.Equals(usuarioActual.Rol, StringComparison.OrdinalIgnoreCase))
                    {
                        if (usuario.Privilegios == null)
                        {
                            usuario.Privilegios = new Privilegios();
                        }

                        if (usuario.Privilegios.Configuraciones == null)
                        {
                            usuario.Privilegios.Configuraciones = new Configuraciones();
                        }

                        if (usuario.Privilegios.Permisos == null)
                        {
                            usuario.Privilegios.Permisos = new ImagenSoft.ModuloWeb.Entidades.Web.Permisos();
                        }

                        usuario.Privilegios.Configuraciones.Validacion2Pasos = this.chkValidacion2Pasos.Checked;
                        usuario.Privilegios.Permisos.CambiarPassword = this.chkCambiarPass.Checked;
                        usuario.Privilegios.Permisos.VerTodasEstaciones = this.chkVerEstaciones.Checked;

                        if (adicional.AdministrarUsuariosClienteModificarPrivilegios(current, usuario) == null)
                        {
                            Mensajeria.MostrarModalErr("No fue posible modificar los privilegios, intentelo más tarde por favor");
                            result = false;
                        }
                    }
                    else
                    {
                        Mensajeria.MostrarModalErr("Solo el administrador puede asignar permisos.");
                        result = false;
                    }
                }
            }
            else
            {
                Mensajeria.MostrarModalErr(string.Format("No fue posible modificar al usuario {0}, intentelo más tarde por favor", usuario.Usuario));
                result = false;
            }

            return result;
        }

        private bool Registrar(Parametros data, SesionModuloWeb current, ImagenSoft.ModuloWeb.Proveedor.Publicador.ServiciosModuloWebProveedor adicional)
        {
            AdministrarUsuariosClientes usuario = adicional.AdministrarUsuariosClienteObtener(current, new FiltroAdministrarUsuariosClientes()
            {
                NoEstacion = this.cmbEstacionUsuario.Value.Trim(),
                Matriz = current.EstacionActual.Matriz,
                Usuario = this.txtUsuario.Text.Trim()
            });

            if (usuario != null)
            {
                Mensajeria.MostrarModalErr(string.Format("El nombre de usuario \"{0}\" ya existe, utilice otro y vuelva a intentarlo por favor.", this.txtUsuario.Text.Trim()));
                this.txtUsuario.Focus();
                return false;
            }

            usuario = new AdministrarUsuariosClientes();
            usuario.Activo = "Si";
            usuario.Correo = this.txtMail.Text.Trim();
            usuario.Matriz = current.EstacionActual.Matriz;
            usuario.NoEstacion = this.cmbEstacionUsuario.Value.Trim();
            usuario.NuevoNoEstacion = this.cmbEstacionUsuario.Value.Trim();
            usuario.Nombre = this.txtNombre.Text.Trim();
            usuario.NuevoUsuario = this.txtUsuario.Text.Trim();
            usuario.Usuario = this.txtUsuario.Text.Trim();
            usuario.Password = Utilerias.GetMD5(this.txtPassword.Text.Trim());
            usuario.FechaCambio = DateTime.Now;
            usuario.Rol = this.cmbRoles.Value;

            bool result = true;

            if (usuario.Privilegios == null)
            {
                usuario.Privilegios = new Privilegios();
            }

            if (usuario.Privilegios.Configuraciones == null)
            {
                usuario.Privilegios.Configuraciones = new Configuraciones();
            }

            if (usuario.Privilegios.Permisos == null)
            {
                usuario.Privilegios.Permisos = new ImagenSoft.ModuloWeb.Entidades.Web.Permisos();
            }

            usuario.Privilegios.Configuraciones.Validacion2Pasos = this.chkValidacion2Pasos.Checked;
            usuario.Privilegios.Permisos.CambiarPassword = this.chkCambiarPass.Checked;
            usuario.Privilegios.Permisos.VerTodasEstaciones = this.chkVerEstaciones.Checked;

            if (adicional.AdministrarUsuariosClienteInsertar(current, usuario) == null)
            {
                Mensajeria.MostrarModalErr(string.Format("No fue posible registrar al usuario {0}, intentelo más tarde por favor", usuario.Usuario));
                result = false;
            }

            return result;
        }

        public string getRoles()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var rol in ImagenSoft.ModuloWeb.Entidades.ConstantesModuloWeb.Roles.RolesModulo)
            {
                sb.AppendFormat("<option value=\"{0}\"></option>", rol);
            }
            return sb.ToString();
        }

        private class Parametros
        {
            public string Usuario { get; set; }

            public string Estacion { get; set; }

            public string Modo { get; set; }


            public string NewUsuario { get; set; }

            public string NewNombre { get; set; }

            public string NewMail { get; set; }

            public string NewEstacion { get; set; }

            public bool NewActivo { get; set; }


            public bool CambiarPassword { get; set; }

            public string Password { get; set; }

            public string NewPassword { get; set; }

            public string ConfirmPassword { get; set; }


            public bool CambiarPrivilegios { get; set; }

            public bool Validacion2Pasos { get; set; }

            public bool CambiarSuPassword { get; set; }

            public bool VerEstaciones { get; set; }
        }
    }
}
