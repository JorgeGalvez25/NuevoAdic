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
    public partial class frmAvance : Form
    {
        public string Titulo
        {
            set
            {
                this.lblTitulo.Text = value;
            }
        }

        private bool cerrar;
        private int valorMaximo;

        public frmAvance(string titulo, int valorMaximo)
        {
            InitializeComponent();

            this.lblTitulo.Text = titulo;
            this.Text = titulo;
            this.valorMaximo = valorMaximo;
            this.cerrar = true;

            this.pbAvance.Minimum = 0;
            this.pbAvance.Maximum = valorMaximo;
            this.pbAvance.Value = 0;
            this.lblAvance.Text = string.Format("{0} / {1}", this.pbAvance.Value.ToString(), this.valorMaximo.ToString());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (cerrar)
            {
                base.OnClosing(e);
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void Avanzar()
        {
            this.pbAvance.Value += 1;
            this.lblAvance.Text = string.Format("{0} / {1}", this.pbAvance.Value.ToString(), this.valorMaximo.ToString());

            Application.DoEvents();

            if (this.pbAvance.Value == this.valorMaximo)
            {
                this.cerrar = true;
                this.Close();
            }
        }
    }
}
