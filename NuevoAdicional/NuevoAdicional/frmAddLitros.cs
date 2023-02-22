using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NuevoAdicional
{
    public partial class frmAddLitros : Form
    {
        public int Litros
        {
            get
            {
                return Convert.ToInt32(txtLitros.Text);
            }
        }

        public frmAddLitros()
        {
            InitializeComponent();
        }

        private void txtLitros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int litros = 0;

            if (!int.TryParse(this.txtLitros.Text, out litros))
            {
                MessageBox.Show("No es una cantidad entera válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtLitros.Focus();
                this.txtLitros.SelectAll();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
