using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiciosCliente;
using Adicional.Entidades;

namespace NuevoAdicional
{
    public partial class frmObtenerCalibracion : Form
    {
        public int Calibracion;
        private int idEstacion;
        private int posision;
        private char modo;
        private Historial historial;
        private ServiciosCliente.IServiciosCliente serviciosCliente;
        private Servicios.Adicional.IServiciosAdicional servicioAdicional;

        public frmObtenerCalibracion(Historial pHistorial, int idEstacion)
        {
            InitializeComponent();
            this.idEstacion = idEstacion;
            this.posision = pHistorial.Posicion;
            this.historial = pHistorial;
            this.Text = string.Format("Calibración de posición: {0}  Manguera: {1}", pHistorial.Posicion, pHistorial.Manguera);
            numericCalibracion.Value = pHistorial.Calibracion;
            comboModo.Text = "Porcentaje";
            modo = 'P';
        }

        private void comboModo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboModo.SelectedIndex != 0)
            {
                label3.Visible = true;
                txtJarra.Visible = true;
                numericCalibracion.DecimalPlaces = 4;
                CalculaLitros(modo);
            }
            else
            {
                numericCalibracion.DecimalPlaces = 0;
                if (txtJarra.Visible)
                    CalculaPorcentaje();
                label3.Visible = false;
                txtJarra.Visible = false;
            }

            switch (comboModo.SelectedIndex)
            {
                case 0: label1.Text = "Porcentaje";
                    modo = 'P';
                    break;
                case 1: label1.Text = "Jarra";
                    modo = 'J';
                    break;
                case 2: label1.Text = "Display";
                    modo = 'D';
                    break;
            }
        }

        private void CalculaLitros(char modo)
        {
            if (modo == 'P')
            {
                numericCalibracion.Value = Convert.ToDecimal(txtJarra.Text) + ((numericCalibracion.Value / 10000) * Convert.ToDecimal(txtJarra.Text)) *
                    (comboModo.SelectedIndex == 1 ? 1 : -1);
            }
        }


        private void CalculaPorcentaje()
        {
            numericCalibracion.Value = (((numericCalibracion.Value - Convert.ToDecimal(txtJarra.Text)) / Convert.ToDecimal(txtJarra.Text)) * 10000) *
                (modo == 'D' ? -1 : 1);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (comboModo.SelectedIndex == 0)
                Calibracion = decimal.ToInt32(numericCalibracion.Value);
            else
            {
                numericCalibracion.DecimalPlaces = 0;
                CalculaPorcentaje();
                Calibracion = decimal.ToInt32(numericCalibracion.Value);
            }
            DialogResult = DialogResult.OK;
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            this.AcceptButton = btnAplicar;
            if (comboModo.SelectedIndex == 0)
                Calibracion = decimal.ToInt32(numericCalibracion.Value);
            else
            {
                numericCalibracion.DecimalPlaces = 0;
                CalculaPorcentaje();
                Calibracion = decimal.ToInt32(numericCalibracion.Value);
            }
            historial.Calibracion = Calibracion;
            servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
            servicioAdicional.HistorialActualizar(historial);
            serviciosCliente = Configuraciones.ListaCanales[idEstacion];
            if (serviciosCliente.CalibrarPosicion(posision))
                MessageBox.Show("Comando aplicado con éxito.");
            else
                MessageBox.Show("No fue posible aplicar el comando.");
            DialogResult = DialogResult.OK;
        }
    }
}
