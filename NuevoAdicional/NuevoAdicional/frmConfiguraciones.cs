using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Adicional.Entidades;
using Persistencia;

namespace NuevoAdicional
{
    public partial class frmConfiguraciones : Form
    {
        private int idEstacion = 0;

        public Configuracion _Configuracion { get; set; }

        private void DespliegaEntidad()
        {
            this.txtCantidadMinima.Text = _Configuracion.Cantidad_minima.ToString("##,#0.00");
        }
        
        public frmConfiguraciones(Configuracion AConfiguracion, int idEstacion)
        {
            InitializeComponent();
            this._Configuracion = AConfiguracion;
            DespliegaEntidad();

            this.idEstacion = idEstacion;
        }

        private bool DatosCorrectos(out string AMensajeError)
        {
            AMensajeError = string.Empty;
            decimal pCantidadMinima = -1;
            decimal.TryParse(txtCantidadMinima.Text, out pCantidadMinima);

            if (pCantidadMinima < 0)
            {
                AMensajeError = "Valor de valór mínimo no es válido.";
                return false;
            }
            else
            {
                return true;
            }
        }

        private void frmConfiguraciones_Load(object sender, EventArgs e)
        {

        }

        private Configuracion ObtenerEntidad()
        {
            this._Configuracion.Cantidad_minima = decimal.Parse(txtCantidadMinima.Text);

            return _Configuracion;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!Configuraciones.ListaDerechos.ContainsKey(Convert.ToInt32((sender as Button).Tag)))
            {
                MessageBox.Show("El usuario no tiene derechos a esta opción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pMensajeError = string.Empty;
            if (DatosCorrectos(out pMensajeError))
            {
                Configuracion pConfig = ObtenerEntidad();
                Servicios.Adicional.IServiciosAdicional servicioAdicional = Configuraciones.ListaCanalesAdicional[idEstacion];
                try
                {
                    servicioAdicional.ConfiguracionActualizar(pConfig);
                }
                catch (Exception)
                {
                    servicioAdicional = Configuraciones.AbrirCanalAdicional(idEstacion);
                }
                
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(pMensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmConfiguraciones_Shown(object sender, EventArgs e)
        {
            txtCantidadMinima.Focus();
        }
    }
}