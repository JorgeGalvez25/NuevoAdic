using System.Windows.Forms;
using Adicional.Entidades;
using Persistencia;

namespace NuevoAdicional.EntradaTanques
{
    public partial class frmTanquesModificar : Form
    {
        private Tanques Entidad;
        public ServiciosCliente.IServiciosCliente servicio; 

        public frmTanquesModificar(Tanques entidad)
        {
            this.Entidad = entidad;
            this.InitializeComponent();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.InicializarControles();
            this.InicializarControles();
            this.CrearEventos();
        }

        private void CrearEventos()
        {
            this.txtVolRecepcion.Validating += new System.ComponentModel.CancelEventHandler(txtVolRecepcion_Validating);
            this.btnAceptar.Click += new System.EventHandler(btnAceptar_Click);
            this.btnCancelar.Click += new System.EventHandler(btnCancelar_Click);
        }
        private void InicializarControles()
        {
            this.txtFolio.Text = this.Entidad.Folio.ToString("D6");
            this.txtFecha.Text = this.Entidad.Fecha.ToString("dd/MM/yyyy");
            this.txtFechaHora.Text = this.Entidad.FechaHora.ToString("dd/MM/yyyy HH:mm:ss tt");
            this.txtCorte.Text = this.Entidad.Corte.ToString();
            this.txtTanque.Text = this.Entidad.Tanque.ToString();

            var t = servicio.ObtenerTanques().Find(p => p.Tanque == this.Entidad.Tanque);

            this.txtCombustible.Text = string.Format("{0:D2} - {1}", this.Entidad.Combustible, ((t == null) ? string.Empty : t.Nombre));
            this.txtVolRecepcion.Text = this.Entidad.VolumenRecepcion.ToString("N3");
            this.chkGenerado.Checked = this.Entidad.Generado.Equals("Si", System.StringComparison.CurrentCultureIgnoreCase);
        }
        private void InicializarVolumenRecepcion()
        {
            this.txtVolRecepcion.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
            this.txtVolRecepcion.Properties.Mask.BeepOnError = false;
            this.txtVolRecepcion.Properties.Mask.EditMask = @"n3";
            this.txtVolRecepcion.Properties.Mask.IgnoreMaskBlank = true;
            this.txtVolRecepcion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVolRecepcion.Properties.Mask.SaveLiteral = false;
            this.txtVolRecepcion.Properties.Mask.ShowPlaceHolders = false;
            this.txtVolRecepcion.Properties.Mask.UseMaskAsDisplayFormat = false;
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnAceptar_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtVolRecepcion.Text.Trim()))
            {
                this.txtVolRecepcion.ErrorText = "No debe ser vacío.";
                MensajeError(this.txtVolRecepcion.ErrorText);
                return;
            }

            double dVal = 0D;
            double.TryParse(RemoveAll(this.txtVolRecepcion.Text, ",", string.Empty), out dVal);

            if (dVal <= 0)
            {
                this.txtVolRecepcion.ErrorText = "Valor inválido.";
                MensajeError(this.txtVolRecepcion.ErrorText);
            }

            this.Entidad.VolumenRecepcion = dVal;
            this.Entidad.Generado = this.chkGenerado.Checked ? "Si" : "No";

            servicio.TanquesModificar(Entidad, Configuraciones.NombreUsuario);

            this.Close();
        }
        private void txtVolRecepcion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtVolRecepcion.Text.Trim()))
            {
                this.txtVolRecepcion.ErrorText = "No debe ser vacío.";
                MensajeError(this.txtVolRecepcion.ErrorText);
                e.Cancel = true;
                return;
            }

            double dVal = 0D;
            double.TryParse(RemoveAll(this.txtVolRecepcion.Text, ",", string.Empty), out dVal);

            if (dVal <= 0)
            {
                this.txtVolRecepcion.ErrorText = "Valor inválido.";
                MensajeError(this.txtVolRecepcion.ErrorText);
                e.Cancel = true;
            }
        }

        private void MensajeError(string msj)
        {
            MessageBox.Show(msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private string RemoveAll(string item, string toRemplace, string with)
        {
            string result = item.Clone().ToString();

            while (result.Contains(toRemplace))
            {
                result = result.Replace(toRemplace, with);
            }

            return result;
        }
    }
}

