using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public partial class frmUsuarioMto : Form
    {
        public Usuario _Usuario { get; set; }

        private void DespliegaEntidad()
        {
            this.txtNombre.Text            = _Usuario.Nombre;
            this.txtContraseña.Text        = _Usuario.Clave;
            this.txtContraseñaConfirm.Text = _Usuario.Clave;
        }

        private void ObtenerEntidad()
        {
            _Usuario.Nombre = txtNombre.Text;
            _Usuario.Clave  = txtContraseña.Text;
        }

        private bool DatosCorrectos(out string AMensajeError)
        {
            AMensajeError = string.Empty;
            if (txtNombre.Text.Trim().Length == 0)
            {
                AMensajeError = "Se necesita un nombre de usuario.";
                txtNombre.Focus();
                return false;
            }
            else if (txtContraseña.Text != txtContraseñaConfirm.Text)
            {
                AMensajeError = "Las contraseñas no coinciden.";
                txtContraseña.Focus();
                return false;
            }

            return true;
        }

        public frmUsuarioMto(Usuario AUsuario, bool modificarPassword)
        {
            InitializeComponent();
            this._Usuario = AUsuario;
            DespliegaEntidad();

            txtNombre.ReadOnly = modificarPassword;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pMensajeError = string.Empty;
            if (DatosCorrectos(out pMensajeError))
            {
                ObtenerEntidad();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(pMensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmUsuarioMto_Shown(object sender, EventArgs e)
        {
            if (txtNombre.ReadOnly)
            {
                txtContraseña.Focus();
            }
            else
            {
                txtNombre.Focus();
            }
        }
    }
}
