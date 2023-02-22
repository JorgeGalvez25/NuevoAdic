using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EstandarCliente.AdministrarClientesMdl.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using EstandarCliente.CargadorVistas.Constants;

namespace EstandarCliente.AdministrarClientesMdl.Views.VMAdministrarClientes.Modal
{
    public partial class MdlUsuarios : Form,
                                       IVMAdministrarGrupos
    {
        private bool flgIsEditing;
        private AdministrarClientes cliente;
        private VMAdministrarGruposPresenter _presenter;
        private ListaAdministrarUsuariosClientes lstUsuarios;

        public MdlUsuarios(AdministrarClientes cl, ListaAdministrarUsuariosClientes lst, VMAdministrarGruposPresenter presenter)
        {
            this._presenter = presenter;
            this.cliente = cl;
            this.lstUsuarios = lst;
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.flgIsEditing = false;
            base.OnLoad(e);
            this.inicilizarCatalogo();
            this.btnNuevo.Click += this.btnNuevo_Click;
            this.btnCerrar.Click += this.btnCerrar_Click;
            this.btnEliminar.Click += this.btnEliminar_Click;
            this.btnModificar.Click += this.btnModificar_Click;
            this.btnAgregarAListado.Click += this.btnAgregarAListado_Click;

            this.txtNombreUsuario.BeginSafe(delegate
                {
                    this.txtNombreUsuario.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    this.txtNombreUsuario.Enabled = false;
                });
            this.txtNombre.BeginSafe(delegate
                {
                    this.txtNombre.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    this.txtNombre.Enabled = false;
                });
            this.txtEmail.BeginSafe(delegate
                {
                    this.txtEmail.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    this.txtEmail.Enabled = false;
                });
            this.txtFechaContraseña.BeginSafe(delegate
                {
                    this.txtFechaContraseña.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    this.txtFechaContraseña.Enabled = false;
                });
            this.luRol.BeginSafe(delegate
                {
                    this.luRol.Properties.DataSource = ConstantesModuloWeb.Roles.RolesModulo.Select(p => new TypeElement<string>()
                        {
                            Display = p,
                            Value = p
                        }).ToArray();
                    this._presenter.InicializarLookUp(this.luRol);
                    this.luRol.Properties.AutoHeight = true;
                    this.luRol.Enabled = false;
                    this.luRol.Properties.AppearanceDisabled.ForeColor = Color.Black;
                });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Enabled = false; });
        }

        private void habilitarControles(bool habilitar, bool isNew)
        {
            this.txtNombreUsuario.BeginSafe(delegate { this.txtNombreUsuario.Enabled = habilitar; });
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Enabled = habilitar; });
            this.txtEmail.BeginSafe(delegate { this.txtEmail.Enabled = habilitar; });
            this.luRol.BeginSafe(delegate { this.luRol.Enabled = habilitar; });
            this.chkActivo.BeginSafe(delegate
            {
                this.chkActivo.Visible = !isNew;
                this.chkActivo.Enabled = habilitar;
            });
            this.btnAgregarAListado.BeginSafe(delegate { this.btnAgregarAListado.Visible = habilitar; });
        }
        private void limpiarControles()
        {
            this.txtNombreUsuario.BeginSafe(delegate { this.txtNombreUsuario.Text = string.Empty; });
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Text = string.Empty; });
            this.txtEmail.BeginSafe(delegate { this.txtEmail.Text = string.Empty; });
            this.txtFechaContraseña.BeginSafe(delegate { this.txtFechaContraseña.Text = DateTime.Now.ToString("dd/MM/yyyy"); });
            this.luRol.BeginSafe(delegate { this.luRol.EditValue = string.Empty; });
            this.chkActivo.BeginSafe(delegate
            {
                this.chkActivo.Visible = true;
                this.chkActivo.Enabled = false;
                this.chkActivo.Checked = false;
            });
        }
        private void inicilizarCatalogo()
        {
            this.grdUsuarios.DataSource = this.lstUsuarios;
            this.grdUsuarios.SuspendLayout();
            this.gridView1.BeginInit();

            this.grdUsuarios.UseEmbeddedNavigator = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowFooter = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowRowSizing = false;

            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].Visible = false;
            }

            this.gridView1.Columns["Usuario"].MinWidth = 200;
            this.gridView1.Columns["Password"].MinWidth = 200;
            this.gridView1.Columns["FechaCambio"].MinWidth = 10;
            this.gridView1.Columns["FechaCambio"].Width = 100;
            this.gridView1.Columns["Rol"].MinWidth = 10;
            this.gridView1.Columns["Rol"].Width = 67;
            this.gridView1.Columns["Correo"].MinWidth = 10;
            this.gridView1.Columns["Correo"].Width = 100;
            this.gridView1.Columns["Activo"].MinWidth = 10;
            this.gridView1.Columns["Activo"].Width = 67;

            this.gridView1.Columns["Usuario"].VisibleIndex = 0;
            this.gridView1.Columns["Password"].VisibleIndex = 1;
            this.gridView1.Columns["FechaCambio"].VisibleIndex = 2;
            this.gridView1.Columns["Rol"].VisibleIndex = 3;
            this.gridView1.Columns["Correo"].VisibleIndex = 4;
            this.gridView1.Columns["Activo"].VisibleIndex = 5;

            this.gridView1.Columns["Usuario"].Caption = "Usuario";
            this.gridView1.Columns["Password"].Caption = "Contraseña";
            this.gridView1.Columns["FechaCambio"].Caption = "Cambio Password";
            this.gridView1.Columns["Rol"].Caption = "Rol";
            this.gridView1.Columns["Correo"].Caption = "Correo";
            this.gridView1.Columns["Activo"].Caption = "Activo";

            this.gridView1.Columns["Activo"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView1.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.gridView1.Columns["Password"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();

            this.grdUsuarios.ResumeLayout();
            this.gridView1.EndInit();
            this.grdUsuarios.Refresh();
            this.gridView1.BestFitColumns();

            (this.gridView1.Columns["Password"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).CustomDisplayText += this.MdlUsuarios_CustomDisplayText;
            this.gridView1.DoubleClick += this.gridView1_DoubleClick;
            this.gridView1.FocusedRowChanged += this.gridView1_FocusedRowChanged;
            this.gridView1_FocusedRowChanged(null, null);
        }

        private void AsignarDatosCatalogo(string noEstacion, string matriz)
        {
            this.grdUsuarios.BeginSafe(delegate
                    {
                        this.grdUsuarios.DataSource = this.UsuariosClienteObtenerTodos(new FiltroAdministrarUsuariosClientes()
                        {
                            NoEstacion = noEstacion,
                            //Matriz = matriz
                        });

                        this.grdUsuarios.Update();
                        this.grdUsuarios.Refresh();
                        this.gridView1.RefreshData();
                    });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            this.limpiarControles();
            Application.DoEvents();

            if (((ListaAdministrarUsuariosClientes)this.grdUsuarios.DataSource)
                .Exists(p => p.Rol.Equals("Maestro", StringComparison.OrdinalIgnoreCase)))
            {
                this.luRol.EditValue = "Invitado";
            }
            else
            {
                this.luRol.EditValue = "Maestro";
            }

            this.habilitarControles(true, true);
            this.chkActivo.Checked = true;
            this.txtNombreUsuario.Focus();
            this.flgIsEditing = false;
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            AdministrarUsuariosClientes item = (AdministrarUsuariosClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

            if (item == null)
            {
                Mensaje.MensajeError("Seleccione un elemento de la lista.");
                return;
            }

            bool isDeleted = this.UsuariosClienteEliminar(new FiltroAdministrarUsuariosClientes()
            {
                NoEstacion = item.NoEstacion,
                Matriz = item.Matriz,
                Usuario = item.Usuario
            });

            if (isDeleted)
            {
                this.AsignarDatosCatalogo(item.NoEstacion, item.Matriz);
                Mensaje.MensajeInfo("Se eliminó el usuario {0} de la estación {1}", item.Usuario, item.NoEstacion);
            }
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            AdministrarUsuariosClientes item = (AdministrarUsuariosClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
            this.habilitarControles(true, false);
            this.chkActivo.Checked = item.Activo.Equals("Si", StringComparison.OrdinalIgnoreCase);
            //this.txtFechaContraseña.BeginSafe(delegate { this.txtFechaContraseña.Text = DateTime.Now.ToString("dd/MM/yyyy"); });
            this.flgIsEditing = true;

        }
        private void btnAgregarAListado_Click(object sender, EventArgs e)
        {
            AdministrarUsuariosClientes entidad = new AdministrarUsuariosClientes();
            entidad.Activo = this.chkActivo.Checked ? "Si" : "No";
            entidad.Correo = this.txtEmail.Text.Trim();
            entidad.FechaCambio = DateTime.Now;
            entidad.Matriz = cliente.Matriz;
            entidad.NoEstacion = cliente.NoEstacion;
            entidad.Nombre = this.txtNombre.Text.Trim();
            entidad.Usuario = this.txtNombreUsuario.Text.Trim();
            entidad.Rol = (string)this.luRol.EditValue;

            if (this.flgIsEditing)
            {
                AdministrarUsuariosClientes usuario = (AdministrarUsuariosClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                entidad.NuevoUsuario = entidad.Usuario;
                entidad.NuevoNoEstacion = entidad.NoEstacion;
                entidad.Usuario = usuario.Usuario;
                entidad.NoEstacion = usuario.NoEstacion;

                if (this.UsuariosClienteModificar(entidad))
                {
                    AsignarDatosCatalogo(entidad.NoEstacion, entidad.Matriz);
                }
            }
            else
            {
                if (this.UsuariosClienteInsertar(entidad))
                {
                    AsignarDatosCatalogo(entidad.NoEstacion, entidad.Matriz);
                }
            }

            this.gridView1_FocusedRowChanged(null, null);
            this.habilitarControles(false, false);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView1.FocusedColumn.FieldName.IEquals("Password"))
            {
                AdministrarUsuariosClientes item = (AdministrarUsuariosClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                Func<AdministrarUsuariosClientes, bool> fn = new Func<AdministrarUsuariosClientes, bool>((itm) =>
                {
                    try
                    {
                        this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                        return this._presenter.UsuariosClienteNuevaContrasenia(new FiltroAdministrarUsuariosClientes()
                            {
                                Correo = itm.Correo,
                                NoEstacion = itm.NoEstacion,
                                Matriz = itm.Matriz,
                                Usuario = itm.Usuario,
                                Password = itm.Password,
                                FechaCambio = DateTime.Now
                            });
                    }
                    catch (Exception) { return false; }
                    finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
                });

                string mensaje = string.Empty;
                if (!string.IsNullOrEmpty(item.Password))
                {
                    if (Mensaje.MensajeConf("¿Desea borrar la contraseña actual?"))
                    {
                        this.txtFechaContraseña.BeginSafe(delegate { this.txtFechaContraseña.Text = DateTime.Now.ToString("dd/MM/yyyy"); });
                        mensaje = fn(item) ? string.Format("Se ha cambiado la contraseña y se ha enviado un correo al correo \"{0}\" de usuario \"{1}\"", item.Correo, item.Nombre)
                                           : string.Format("Ocurrio un error y no fue posible cambiar la contraseña del usuario \"{0}\"", item.Nombre);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    mensaje = fn(item) ? string.Format("Se ha cambiado la contraseña y se ha enviado un correo al correo \"{0}\" de usuario \"{1}\"", item.Correo, item.Nombre)
                                       : string.Format("Ocurrio un error y no fue posible cambiar la contraseña del usuario \"{0}\"", item.Nombre);
                }

                ImagenSoft.Librerias.Mensaje.MensajeInfo(mensaje);
                this.AsignarDatosCatalogo(item.NoEstacion, item.Matriz);
            }
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.habilitarControles(false, false);
            AdministrarUsuariosClientes usuario = (AdministrarUsuariosClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
            this.txtNombreUsuario.BeginSafe(delegate { this.txtNombreUsuario.Text = usuario == null ? string.Empty : usuario.Usuario; });
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Text = usuario == null ? string.Empty : usuario.Nombre; });
            this.txtEmail.BeginSafe(delegate { this.txtEmail.Text = usuario == null ? string.Empty : usuario.Correo; });
            this.txtFechaContraseña.BeginSafe(delegate { this.txtFechaContraseña.Text = usuario == null ? string.Empty : usuario.FechaCambio.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.luRol.BeginSafe(delegate { this.luRol.EditValue = usuario == null ? string.Empty : usuario.Rol; });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Checked = usuario == null ? false : usuario.Activo.IEquals("Si"); });
        }

        private void MdlUsuarios_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (!string.IsNullOrEmpty(((string)e.Value ?? string.Empty).Trim()))
            {
                e.DisplayText = "(Contraseña establecida)";
                (sender as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).Appearance.ForeColor = Color.Gray;
            }
        }

        #region IVMAdministrarGrupos Members

        public ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.ObtenerTodosFiltro(filtro);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return new ListaAdministrarClientes();
            }
        }

        public bool UsuariosClienteInsertar(AdministrarUsuariosClientes entidad)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteInsertar(entidad);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public bool UsuariosClienteModificar(AdministrarUsuariosClientes entidad)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteModificar(entidad);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public bool UsuariosClienteEliminar(FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteEliminar(filtro);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception)
            {
                Mensaje.MensajeError("No fue posible eliminar el usuario {0} de la estación {1}", filtro.Usuario, filtro.NoEstacion);
                return false;
            }
        }

        public ListaAdministrarUsuariosClientes UsuariosClienteObtenerTodos(FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteObtenerTodos(filtro);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return new ListaAdministrarUsuariosClientes();
            }
        }

        public bool UsuariosClienteInsertarModificar(ListaAdministrarUsuariosClientes lista)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteInsertarModificar(lista);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public bool UsuariosClienteNuevaContrasenia(FiltroAdministrarUsuariosClientes filtro)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.UsuariosClienteNuevaContrasenia(filtro);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception)
            {
                Mensaje.MensajeError("No fue posible cambiar la contraseña al usuario {0} de la estación {1}", filtro.Usuario, filtro.NoEstacion);
                return false;
            }
        }

        public bool Insertar(AdministrarClientes entidad)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.Insertar(entidad);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception)
            {
                Mensaje.MensajeError("No fue posible registrar");
                return false;
            }
        }

        public bool Modificar(AdministrarClientes entidad)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.Modificar(entidad);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception)
            {
                Mensaje.MensajeError("No fue posible modificar");
                return false;
            }
        }

        public AdministrarClientes Obtener(FiltroAdministrarClientes filtro)
        {
            try
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                    return this._presenter.Obtener(filtro);
                }
                finally { this.BeginSafe(delegate { this.Cursor = Cursors.Default; }); }
            }
            catch (Exception)
            {
                Mensaje.MensajeError("No fue posible obtener");
                return null;
            }
        }

        #endregion

        private class TypeElement<T>
        {
            public string Display { get; set; }
            public T Value { get; set; }
        }
    }
}
