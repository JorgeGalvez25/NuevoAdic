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
    public partial class frmReporteAjusteParametros : Form
    {
        private int numReporte = 0;

        public DateTime FechaIni
        {
            get
            {
                switch (numReporte)
                {
                    case 1: return deFechaIni.Value.Date;
                    case 2: return deFechaIni02.Value.Date;
                    default: return DateTime.MinValue;
                }
            }
        }

        public DateTime FechaFin { get { return deFechaFin.Value.Date; } }

        public bool Detallado { get { return false; } }

        public bool NombreEstacion
        {
            get
            {
                switch (numReporte)
                {
                    case 1: return true;
                    case 2: return true;
                    default: return false;
                }
            }
        }

        public bool EntradasFisicas { get { return false; } }

        public bool De12a12 { get { return rad12a12.Checked; } }

        public bool De06a06 { get { return rad6a6.Checked; } }

        public bool A24Hrs { get { return rad6a62.Checked; } }

        public frmReporteAjusteParametros(int numReporte)
        {
            InitializeComponent();

            this.numReporte = numReporte;
            this.Text = string.Format("Reporte {0}", numReporte.ToString("00"));
            foreach (Control item in this.Controls)
            {
                if (item is Panel && Convert.ToInt32(item.Tag) == numReporte)
                {
                    item.Visible = true;
                    item.Dock = DockStyle.Fill;
                    break;
                }
            }

            bool habilitado = Configuraciones.GetValorVariable(Configuraciones.ListaVariables[2]).Equals("No", StringComparison.OrdinalIgnoreCase);
            deFechaIni.Enabled = habilitado;
            deFechaFin.Enabled = habilitado;
            deFechaIni02.Enabled = habilitado;
        }

        public frmReporteAjusteParametros()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //this.DialogResult = DialogResult.Cancel;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (numReporte == 1 && FechaIni > FechaFin)
            {
                MessageBox.Show("La fecha inicial no debe ser mayor a la final.", "Rango de fechas es incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void rad6a6_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
