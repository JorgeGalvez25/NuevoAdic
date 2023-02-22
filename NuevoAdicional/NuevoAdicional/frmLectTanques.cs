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
    public partial class frmLectTanques : Form
    {
        private ListaDpvgTanq tanques;
        public int Id;
        public ServiciosCliente.IServiciosCliente pServiciosCliente;

        public frmLectTanques()
        {
            InitializeComponent();
        }

        private void frmLectTanques_Load(object sender, EventArgs e)
        {
            pServiciosCliente = Configuraciones.ListaCanales[Id];
            if (!Configuraciones.CanalEstaActivo(Id, false))
            {
                pServiciosCliente = Configuraciones.AbrirCanalCliente(Id);
            }
            tanques = pServiciosCliente.ObtenerTanques();

            tanques.ForEach(t => cmbTanques.Properties.Items.Add(t.Tanque.ToString("00") + " - " + t.Nombre));

            if (cmbTanques.Properties.Items.Count > 0)
                cmbTanques.SelectedIndex = 0;

            dtFecha.DateTime = DateTime.Today;
            txtLectura.Text = "0";
        }

        private void chkPorTurno_CheckedChanged(object sender, EventArgs e)
        {
            spnTurno.Enabled = chkPorTurno.Checked;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            LecturaTanque entidad = new LecturaTanque();
            double lectura;
            entidad.Fecha = dtFecha.DateTime.Date;
            if (!double.TryParse(txtLectura.Text, out lectura))
            {
                MessageBox.Show("El valor de la lectura no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (lectura <= 0)
            {
                MessageBox.Show("El valor de la lectura debe ser mayor a cero.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            entidad.Lectura = lectura;
            entidad.Tanque = Convert.ToInt32(cmbTanques.Text.Substring(0, 2));
            if (chkPorTurno.Checked)
                entidad.Turno = Convert.ToInt32(spnTurno.Text);
            if (pServiciosCliente.RegistrarLectura(entidad))
                MessageBox.Show("Lectura registrada con éxito.");
            else
                MessageBox.Show("Ya existe una lectura registrada este día");
            txtLectura.Text = "";
        }
    }
}
