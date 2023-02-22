using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.SesionMdl
{
    public partial class VContrasena : XtraForm, IVContrasena
    {
        #region Variables

        private SesionModuloWeb Sesion;
        public bool Realizado { get; set; }

        #endregion

        public VContrasena(VContrasenaPresenter presenter, ref SesionModuloWeb sesion)
        {
            InitializeComponent();
            Sesion = sesion;
            Presenter = presenter;
            iniciar();
        }

        protected override void OnLoad(EventArgs e)
        {
            _presenter.OnViewReady();
            base.OnLoad(e);
        }

        #region Funiones Privadas

        public void iniciar()
        {
            Realizado = false;

            txtContarseña.Properties.MaxLength = 10;
            txtConfirmacion.Properties.MaxLength = 10;
            txtContarseña.Properties.PasswordChar = 'l';
            txtConfirmacion.Properties.PasswordChar = 'l';

            txtConfirmacion.Properties.ReadOnly = true;
            btnAceptar.Enabled = false;

            crearEventos();
        }

        private void crearEventos()
        {
            txtContarseña.EditValueChanged += new EventHandler(txtContarseña_EditValueChanged);
            txtConfirmacion.Validating += new System.ComponentModel.CancelEventHandler(txtConfirmacion_Validating);
            btnAceptar.Click += new EventHandler(btnAceptar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);
            txtContarseña.PreviewKeyDown += new PreviewKeyDownEventHandler(txtContarseña_PreviewKeyDown);
            txtConfirmacion.PreviewKeyDown += new PreviewKeyDownEventHandler(txtConfirmacion_PreviewKeyDown);
            btnAceptar.PreviewKeyDown += new PreviewKeyDownEventHandler(btnAceptar_PreviewKeyDown);
            btnCancelar.PreviewKeyDown += new PreviewKeyDownEventHandler(btnCancelar_PreviewKeyDown);
        }

        private void eliminarEventos()
        {
            txtContarseña.EditValueChanged -= new EventHandler(txtContarseña_EditValueChanged);
            txtConfirmacion.Validating -= new System.ComponentModel.CancelEventHandler(txtConfirmacion_Validating);
            btnAceptar.Click -= new EventHandler(btnAceptar_Click);
            btnCancelar.Click -= new EventHandler(btnCancelar_Click);
            txtContarseña.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtContarseña_PreviewKeyDown);
            txtConfirmacion.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtConfirmacion_PreviewKeyDown);
            btnAceptar.PreviewKeyDown -= new PreviewKeyDownEventHandler(btnAceptar_PreviewKeyDown);
            btnCancelar.PreviewKeyDown -= new PreviewKeyDownEventHandler(btnCancelar_PreviewKeyDown);
        }

        #endregion

        #region Validaciones

        private bool contraseñaEstaVacia(out string mensaje)
        {
            if (txtContarseña.Text.Length == 0)
            {
                mensaje = string.Format(ListadoMensajes.Error_Vacio, "Contraseña");
                txtContarseña.Focus();
                txtContarseña.SelectAll();
                return false;
            }
            mensaje = string.Empty;
            return true;
        }

        private bool confirmacionEstaVacia(out string mensaje)
        {
            if (txtConfirmacion.Text.Length == 0)
            {
                mensaje = ListadoMensajes.Error_Debe_Confirmar_Contrasena;
                txtConfirmacion.Focus();
                txtConfirmacion.SelectAll();
                return false;
            }
            mensaje = string.Empty;
            return true;
        }

        private bool confirmacionEsValida(out string mensaje)
        {
            if (txtContarseña.Text != txtConfirmacion.Text)
            {
                mensaje = ListadoMensajes.Error_Confirmacion_Diferente;
                txtConfirmacion.Focus();
                txtConfirmacion.SelectAll();
                return false;
            }
            mensaje = string.Empty;
            return true;
        }

        #endregion

        #region Eventos

        void btnCancelar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                txtContarseña.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                btnAceptar.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                eliminarEventos();
            }
        }

        void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            eliminarEventos();
        }

        void btnAceptar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                btnCancelar.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                txtConfirmacion.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                eliminarEventos();
            }
        }

        void txtConfirmacion_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                btnAceptar.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                txtContarseña.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                eliminarEventos();
            }
        }

        void txtContarseña_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                txtConfirmacion.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                btnCancelar.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                eliminarEventos();
            }
        }

        void btnAceptar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            if (!contraseñaEstaVacia(out mensaje)) { Mensaje.MensajeError(mensaje); return; }
            if (!confirmacionEstaVacia(out mensaje)) { Mensaje.MensajeError(mensaje); return; }
            if (!confirmacionEsValida(out mensaje)) { Mensaje.MensajeError(mensaje); return; }

            if (_presenter.CambiarContrasena(Sesion.Usuario.Clave, Sesion.Usuario.Password, ImagenSoft.ModuloWeb.Entidades.Utilerias.GetMD5(txtContarseña.Text)))
            {
                Sesion.Usuario.Password = ImagenSoft.ModuloWeb.Entidades.Utilerias.GetMD5(txtContarseña.Text);
                Realizado = true;
                this.Close();
                eliminarEventos();
            }
        }

        void txtConfirmacion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtConfirmacion.Text.Length == 0) return;

            string mensaje = string.Empty;
            if (!confirmacionEsValida(out mensaje))
            {
                txtConfirmacion.Text = string.Empty;
                txtConfirmacion.Properties.ReadOnly = true;
                btnAceptar.Enabled = false;
                e.Cancel = true;
                throw new Exception(mensaje);
            }
        }

        void txtContarseña_EditValueChanged(object sender, EventArgs e)
        {
            txtConfirmacion.Text = string.Empty;
            txtConfirmacion.Properties.ReadOnly = string.IsNullOrEmpty(txtContarseña.Text);
            btnAceptar.Enabled = !string.IsNullOrEmpty(txtContarseña.Text);
        }

        #endregion
    }
}

