using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Adicional.Entidades;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Persistencia;

namespace NuevoAdicional.EntradaTanques
{
    public partial class frmTanquesRegistrar : Form
    {
        public ServiciosCliente.IServiciosCliente servicio; 
        private ListaDpvgTanq lstTanques = new ListaDpvgTanq();

        public frmTanquesRegistrar()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DateTime fechaActual = DateTime.Now;
            this.dtFechaHora.EditValue = fechaActual;
            this.dtFechaHoraInicial.EditValue = fechaActual;
            this.dtFechaHoraFinal.EditValue = fechaActual.AddMinutes(30);

            this.lstTanques = servicio.ObtenerTanques();

            this.InicializarTanques();
            this.spnTanques_ValueChanged(null, null);
            this.CrearEventos();
        }

        private void InicializarTanques()
        {
            this.spnTanques.Properties.MaxValue = lstTanques.Max(p => p.Tanque);
        }

        private void CrearEventos()
        {
            this.btnAceptar.Click += this.btnAceptar_Click;
            this.btnCancelar.Click += this.btnCancelar_Click;
            this.spnTanques.ValueChanged += this.spnTanques_ValueChanged;
            this.txtVolumenFinal.Validating += this.txtVolumenFinal_Validating;
            this.txtVolumenInicial.Validating += this.txtVolumenInicial_Validating;
        }
        private Tanques obtenerEntidad()
        {
            Tanques entidad = new Tanques();

            entidad.Fecha = this.dtFechaHora.DateTime.Date;
            entidad.FechaHora = this.dtFechaHora.DateTime;
            entidad.FechaHoraInicial = this.dtFechaHoraInicial.DateTime;
            entidad.FechaHoraFinal = this.dtFechaHoraFinal.DateTime;
            entidad.Tanque = decimal.ToInt32(this.spnTanques.Value);
            var com = this.ObtenerCombustible();
            entidad.Combustible = (com == null) ? 0 : com.Combustible;

            double dVal = 0D;
            double.TryParse(this.RemoveAll(this.txtVolumenInicial.Text, ",", string.Empty), out dVal);
            entidad.VolumenInicial = dVal;

            dVal = 0D;
            double.TryParse(this.RemoveAll(this.txtVolumenFinal.Text, ",", string.Empty), out dVal);
            entidad.VolumenFinal = dVal;

            dVal = 0D;
            double.TryParse(this.RemoveAll(this.txtTemperatura.Text, ",", string.Empty), out dVal);
            entidad.Temperatura = dVal;

            //dVal = 0D;
            //double.TryParse(this.RemoveAll(this.txtVolumenRecepcion.Text, ",", string.Empty), out dVal);
            //entidad.VolumenRecepcion = dVal;
            entidad.VolumenRecepcion = entidad.VolumenFinal - entidad.VolumenInicial;

            return entidad;
        }

        #region Validaciones

        private bool ValidarTanque(ref string msj)
        {
            if (string.IsNullOrEmpty(this.spnTanques.Text))
            {
                msj = "Tanque no puede ser vacio";
                return false;
            }

            if (this.spnTanques.Value == 0M)
            {
                msj = "Valor inválido para tanque";
                return false;
            }

            return true;
        }
        private bool ValidarFechaHora(ref string msj)
        {
            if (string.IsNullOrEmpty(this.dtFechaHora.Text))
            {
                msj = "Fecha/Hora no puede ser vacio";
                return false;
            }

            return true;
        }
        private bool ValidarFechaHoraFinal(ref string msj)
        {
            if (string.IsNullOrEmpty(this.dtFechaHoraFinal.Text))
            {
                msj = "Fecha/Hora final no puede ser vacio";
                return false;
            }

            return true;
        }
        private bool ValidarFechaHoraInicial(ref string msj)
        {
            if (string.IsNullOrEmpty(this.dtFechaHoraFinal.Text))
            {
                msj = "Fecha/Hora inicial no puede ser vacio";
                return false;
            }

            return true;
        }

        private bool ValidarVolFinal(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtVolumenFinal.Text))
            {
                msj = "Volumen final no puede ser vacio";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtVolumenFinal.Text, out val);

            if (val <= 0D)
            {
                msj = "Valor inválido para volumen final";
                return false;
            }

            return true;
        }
        private bool ValidarVolInicial(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtVolumenInicial.Text))
            {
                msj = "Volumen inicial no puede ser vacio";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtVolumenInicial.Text, out val);

            if (val <= 0D)
            {
                msj = "Valor inválido para volumen inicial";
                return false;
            }

            return true;
        }
        private bool ValidarTemperatura(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtTemperatura.Text))
            {
                msj = "Temperatura no puede ser vacio";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtTemperatura.Text, out val);

            if (val <= 0D)
            {
                msj = "Valor inválido para temperatura";
                return false;
            }

            return true;
        }
        private bool ValidarVolRecepcion(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtVolumenRecepcion.Text))
            {
                msj = "Volumen de recepción no puede ser vacio";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtVolumenRecepcion.Text, out val);

            if (val <= 0D)
            {
                msj = "Valor inválido para volumen de recepción";
                return false;
            }

            return true;
        }

        #endregion

        #region Eventos

        private void spnTanques_ValueChanged(object sender, EventArgs e)
        {
            var item = this.ObtenerCombustible();

            this.lblCombustible.Text = item == null ? "-" : item.Nombre;
        }
        private void txtVolumenFinal_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtVolumenFinal.Text) &&
                !string.IsNullOrEmpty(this.txtVolumenInicial.Text))
            {
                double iVal1 = 0;
                double.TryParse(this.RemoveAll(this.txtVolumenFinal.Text, ",", string.Empty), out iVal1);
                double iVal2 = 0;
                double.TryParse(this.RemoveAll(this.txtVolumenInicial.Text, ",", string.Empty), out iVal2);

                this.txtVolumenRecepcion.Text = (iVal1 - iVal2).ToString("N3");
            }
        }
        private void txtVolumenInicial_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtVolumenFinal.Text) &&
                !string.IsNullOrEmpty(this.txtVolumenInicial.Text))
            {
                double iVal1 = 0;
                double.TryParse(this.RemoveAll(this.txtVolumenFinal.Text, ",", string.Empty), out iVal1);
                double iVal2 = 0;
                double.TryParse(this.RemoveAll(this.txtVolumenInicial.Text, ",", string.Empty), out iVal2);

                this.txtVolumenRecepcion.Text = (iVal1 - iVal2).ToString("N3");
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string msj = string.Empty;

            if (!ValidarFechaHora(ref msj))
            {
                this.dtFechaHora.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarFechaHoraInicial(ref msj))
            {
                this.dtFechaHoraInicial.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarFechaHoraFinal(ref msj))
            {
                this.dtFechaHoraFinal.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarTanque(ref msj))
            {
                this.spnTanques.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarVolInicial(ref msj))
            {
                this.txtVolumenInicial.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarVolFinal(ref msj))
            {
                this.txtVolumenFinal.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarVolRecepcion(ref msj))
            {
                this.txtVolumenRecepcion.Focus();
                MensajeInformacion(msj);
                return;
            }

            if (!ValidarTemperatura(ref msj))
            {
                this.txtTemperatura.Focus();
                MensajeInformacion(msj);
                return;
            }
            Tanques entidad = this.obtenerEntidad();
            if (servicio.TanquesRegistrar(entidad, Configuraciones.NombreUsuario))
            {
                this.MensajeInformacion("Registro exitoso");
                this.Close();
            }
            else
            {
                MensajeInformacion("No fue posible realizar el registro.");
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void MensajeError(string msj)
        {
            MessageBox.Show(msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void MensajeInformacion(string msj)
        {
            MessageBox.Show(msj, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void InicializarLookUp(LookUpEdit t)
        {
            LookUpColumnInfo columna = new LookUpColumnInfo();
            columna.FieldName = "Display";
            columna.Caption = "Display";
            t.Properties.Columns.Add(columna);

            columna = new LookUpColumnInfo();
            columna.FieldName = "Value";
            columna.Caption = "Value";
            columna.Visible = false;
            t.Properties.Columns.Add(columna);

            t.Properties.DisplayMember = "Display";
            t.Properties.ValueMember = "Value";
            t.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            t.Properties.PopupSizeable = false;
            t.Properties.ShowFooter = false;
            t.Properties.ShowHeader = false;
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

        private DpvgTanq ObtenerCombustible()
        {
            var item = this.lstTanques.Find(p => p.Tanque == this.spnTanques.Value);
            return item;
        }
    }
}
