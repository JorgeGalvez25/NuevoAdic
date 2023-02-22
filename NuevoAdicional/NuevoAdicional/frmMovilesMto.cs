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
    public partial class frmMovilesMto : Form
    {
        public Moviles Movil { get; private set; }

        public frmMovilesMto(Moviles movil, bool modificar)
        {
            InitializeComponent();

            this.Movil = movil;
            txtTelefono.ReadOnly = modificar;

            this.Text = string.Concat(modificar ? "Actualizar" : "Alta de", " dispositivo móvil");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (txtTelefono.ReadOnly)
                txtResponsable.Focus();
        }

        private void DesplegarEntidad()
        {
            txtTelefono.Text = Movil.Telefono;
            txtResponsable.Text = Movil.Responsable;
        }

        private void ObtenerEntidad()
        {
            Movil.Telefono = txtTelefono.Text;
            Movil.Responsable = txtResponsable.Text;
            if (!txtTelefono.ReadOnly)
                Movil.Activo = "S";
        }

        private bool DatosCorrectos(out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                mensajeError = "Es necesario especificar un número de teléfono";
                txtTelefono.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtResponsable.Text))
            {
                mensajeError = "Es necesario especificar un responsable";
                txtResponsable.Focus();
                return false;
            }

            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string mensajeError = string.Empty;

            if (DatosCorrectos(out mensajeError))
            {
                ObtenerEntidad();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
