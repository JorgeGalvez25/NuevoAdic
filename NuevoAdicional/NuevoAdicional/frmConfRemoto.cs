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
    public partial class frmConfRemoto : Form
    {
        public frmConfRemoto()
        {
            InitializeComponent();
        }

        private void txtSegmentos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void txtSegmentos_Validating(object sender, CancelEventArgs e)
        {
            byte valor = 0;

            if (string.IsNullOrEmpty((sender as TextBox).Text))
            {
                (sender as TextBox).Text = "0";
            }

            if (!byte.TryParse((sender as TextBox).Text, out valor))
            {
                errorSegmentos.SetError((sender as Control), "El valor del segmento debe estar entre 0 y 255");
                e.Cancel = true;
            }
            else
            {
                errorSegmentos.SetError((sender as Control), string.Empty);
            }
        }
    }
}
