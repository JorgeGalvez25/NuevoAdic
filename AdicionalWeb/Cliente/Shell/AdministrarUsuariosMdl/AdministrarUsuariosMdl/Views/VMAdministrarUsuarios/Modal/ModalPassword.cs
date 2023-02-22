using System;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;

namespace EstandarCliente.AdministrarUsuariosMdl.Views.VMAdministrarUsuarios.Modal
{
    public partial class ModalPassword : Form
    {
        public string Resultado { get; private set; }

        public ModalPassword()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.BeginSafe(CrearEventos);
            this.txtContrasena.Focus();
        }

        private void CrearEventos()
        {
            this.btnAceptar.BeginSafe(delegate { this.btnAceptar.Click += this.btnAceptar_Click; });
        }

        private bool validar(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtContrasena.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "contraseña");
                this.txtContrasena.Focus();
                this.txtContrasena.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtConfirmacion.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "confirmacion");
                this.txtConfirmacion.Focus();
                this.txtConfirmacion.SelectAll();
                return false;
            }

            if (!this.txtContrasena.Text.Equals(this.txtConfirmacion.Text))
            {
                msj = ListadoMensajes.Error_Confirmacion_Diferente;
                this.txtConfirmacion.Focus();
                this.txtConfirmacion.SelectAll();
                return false;
            }

            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string msj = string.Empty;
            if (!validar(ref msj))
            {
                Mensaje.MensajeError(msj);
                return;
            }

            this.Resultado = ImagenSoft.ModuloWeb.Entidades.Utilerias.GetMD5(this.txtContrasena.Text);
            this.DialogResult = DialogResult.OK;
        }
    }
}
