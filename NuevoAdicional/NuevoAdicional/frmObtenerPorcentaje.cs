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
    public partial class frmObtenerPorcentaje : Form
    {
        public decimal Porcentaje;

        public frmObtenerPorcentaje(decimal porcentaje, decimal porcentajeMaximo, bool entero)
        {
            InitializeComponent();
            this.Porcentaje = porcentaje;
            this.numPorcentaje.Maximum = porcentajeMaximo;
            this.lblMaximo.Text = string.Format("Máximo {0}", porcentajeMaximo.ToString(entero ? "00" : "0.00"));
            if (entero)
            {
                this.numPorcentaje.Increment = 1;
                this.numPorcentaje.DecimalPlaces = 0;
            }
            DespliegaEntidad();
        }

        private void DespliegaEntidad()
        {
            this.numPorcentaje.Value = Porcentaje;
        }

        private void ObtenerEntidad()
        {
            this.Porcentaje = numPorcentaje.Value;
        }

        private bool DatosValidos(out string AMensajeError)
        {
            AMensajeError = string.Empty;
            double valor = -1;
            double.TryParse(numPorcentaje.Text, out valor);

            if (valor < 0)
            {
                AMensajeError = "Valor inválido.";
                numPorcentaje.Focus();
                return false;
            }

            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string pMensajeError = string.Empty;
            if (DatosValidos(out pMensajeError))
            {
                ObtenerEntidad();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(pMensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmObtenerPorcentaje_Shown(object sender, EventArgs e)
        {
            numPorcentaje.Focus();
        }
    }
}
