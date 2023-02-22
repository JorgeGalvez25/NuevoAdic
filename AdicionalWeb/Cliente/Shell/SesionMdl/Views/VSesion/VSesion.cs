using System;
using System.Threading;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.SesionMdl
{
    public partial class VSesion : UserControl, IVSesion
    {
        #region Propiedades

        public SesionModuloWeb Sesion { get; set; }

        #endregion

        public VSesion(VSesionPresenter presenter)
        {
            this.InitializeComponent();
            this.Presenter = presenter;
            this.Iniciar();
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);
            this.lblVersion.Text = string.Format("Versión: {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            this.BeginSafe(delegate
                {
                    //this.ParentForm.Opacity = 0;
                    //this.Refresh();
                    this.ParentForm.MaximumSize = this.ParentForm.Size;
                    this.ParentForm.MinimumSize = this.ParentForm.Size;
                    this.ParentForm.TopMost = true;
                    this.ParentForm.FormBorderStyle = FormBorderStyle.None;

                    this.controlesVisibles(true);
                    this.luUsuario.Focus();

                    //this.ParentForm.Opacity = 1;
                    this.Refresh();
                    Application.DoEvents();
                    EstandarCliente.Infrastructure.Shell.FormaSplash.Cerrar();
                    Application.DoEvents();
                });
        }

        #region Privadas

        private void Iniciar()
        {
            this.controlesVisibles(false);

            this.txtContraseña.BeginSafe(delegate
            {
                //this.txtContraseña.Properties.PasswordChar = 'l';
                //this.txtContraseña.Properties.MaxLength = 10;
                this.txtContraseña.PasswordChar = 'l';
                this.txtContraseña.MaxLength = 10;
            });

            this.luUsuario.BeginSafe(delegate
                {
                    this._presenter.luUsuario(luUsuario);

                    this.luUsuario.EnterMoveNextControl = true;

                    if (this.luUsuario.Properties.DataSource == null)
                    {
                        this.btnAceptar.Enabled = false;
                    }
                    else
                    {
                        this.btnAceptar.Click += btnAceptar_Click;
                    }
                });

            this.btnCancelar.Click += this.btnCancelar_Click;
            this.luUsuario.PreviewKeyDown += this.luUsuario_PreviewKeyDown;
            this.btnCancelar.PreviewKeyDown += this.btnCancelar_PreviewKeyDown;
            this.txtContraseña.KeyDown += this.txtContraseña_KeyDown;
            this.btnCambiarContrasena.Click += this.btnCambiarContrasena_Click;
            this.btnCambiarContrasena.GotFocus += (e, s) => { this.txtContraseña.SelectAll(); this.txtContraseña.Focus(); };
            this.luUsuario.EditValueChanged += (e, s) => { this.txtContraseña.Text = string.Empty; };
        }

        private void efectoCerrar()
        {
            this.DoubleBuffered = true;
            this.ParentForm.BeginInvoke(new MethodInvoker(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        this.ParentForm.Opacity = this.ParentForm.Opacity - 0.01;
                        this.Refresh();
                        Thread.Sleep(5);
                    }
                }));
        }

        private void controlesVisibles(bool visible)
        {
            this.labelControl1.BeginSafe(delegate { this.labelControl1.Visible = visible; });
            this.labelControl2.BeginSafe(delegate { this.labelControl2.Visible = visible; });
            this.luUsuario.BeginSafe(delegate { this.luUsuario.Visible = visible; });
            this.txtContraseña.BeginSafe(delegate { this.txtContraseña.Visible = visible; });
            this.btnCambiarContrasena.BeginSafe(delegate { this.btnCambiarContrasena.Visible = false; });//visible; 
            this.btnAceptar.BeginSafe(delegate { this.btnAceptar.Visible = visible; });
            this.btnCancelar.BeginSafe(delegate { this.btnCancelar.Visible = visible; });
        }

        #endregion

        #region Eventos

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            SesionModuloWeb sesion = (this.luUsuario.Properties.DataSource as ListaSesiones).Find(u => { return u.Usuario.Clave == (int)this.luUsuario.EditValue; });

            try
            {
                if (this.ValidarContrasena(txtContraseña.Text, sesion.Usuario.Password))
                {
                    if (this._presenter.CambiarContraseña(sesion))
                    {
                        this.txtContraseña.Text = string.Empty;
                        this.txtContraseña.Focus();
                    }
                }
                else
                {
                    Mensaje.MensajeError(ListadoMensajes.Error_Contrasena_Invalida);
                    this.txtContraseña.SelectAll();
                    this.txtContraseña.Focus();
                    this.Sesion = null;
                }
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
                this.txtContraseña.SelectAll();
                this.txtContraseña.Focus();
                this.Sesion = null;
            }
        }

        private void txtContraseña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnAceptar_Click(sender, new EventArgs());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Sesion = null;
            //this.efectoCerrar();
            this.ParentForm.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            SesionModuloWeb sesion = (this.luUsuario.Properties.DataSource as ListaSesiones).Find(u => { return u.Usuario.Clave == (int)this.luUsuario.EditValue; });

            try
            {
                if (this.ValidarContrasena(txtContraseña.Text, sesion.Usuario.Password))
                {
                    this.Sesion = sesion;
                    {
                        this.Sesion.NoCliente = "Monitor";
                    }

                    //this.efectoCerrar();
                    this._presenter.OnCloseView();
                }
                else
                {
                    Mensaje.MensajeError(ListadoMensajes.Error_Contrasena_Invalida);
                    this.txtContraseña.SelectAll();
                    this.txtContraseña.Focus();
                    this.Sesion = null;
                }
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
                this.txtContraseña.SelectAll();
                this.txtContraseña.Focus();
                this.Sesion = null;
            }
        }

        private void btnCancelar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                this.luUsuario.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                this.btnAceptar.Focus();
                e.IsInputKey = true;
            }
        }

        private void luUsuario_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                this.txtContraseña.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                this.btnCancelar.Focus();
                e.IsInputKey = true;
            }
        }

        #endregion

        #region IVSesion Members

        public bool Ping()
        {
            bool resultado = false;
            try
            {
                resultado = this._presenter.Ping();
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public bool ValidarContrasena(string pass1, string pass2)
        {
            bool resultado = false;
            try
            {
                resultado = this._presenter.ValidarContrasena(pass1, pass2);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public ListaSesiones ObtenerTodosFiltro(FiltroSesionModuloWeb f)
        {
            ListaSesiones resultado = new ListaSesiones();
            try
            {
                resultado.AddRange(this._presenter.ObtenerTodosFiltro(f));
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }
            return resultado;
        }

        #endregion
    }
}
