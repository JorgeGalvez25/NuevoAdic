using System;
using System.ComponentModel;
using System.Windows.Forms;
using Adicional.Entidades;
using Persistencia;

namespace NuevoAdicional
{
    public partial class frmTicketsModificar : Form
    {
        public int Id;
        public ServiciosCliente.IServiciosCliente pServiciosCliente;
        public string NombreEst;

        public frmTicketsModificar()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            pServiciosCliente = Configuraciones.ListaCanales[Id];
            if (!Configuraciones.CanalEstaActivo(Id, false))
            {
                pServiciosCliente = Configuraciones.AbrirCanalCliente(Id);
            }
            base.OnLoad(e);
            this.Inicializar();
            this.CrearEventos();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.txtFolio.Focus();
        }

        private void Inicializar()
        {
            this.txtFolio.Properties.MaxLength = 8;
            this.txtFolio.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
            this.txtFolio.Properties.Mask.BeepOnError = false;
            this.txtFolio.Properties.Mask.EditMask = @"\d{0,8}";
            this.txtFolio.Properties.Mask.IgnoreMaskBlank = true;
            this.txtFolio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtFolio.Properties.Mask.SaveLiteral = false;
            this.txtFolio.Properties.Mask.ShowPlaceHolders = false;
            this.txtFolio.Properties.Mask.UseMaskAsDisplayFormat = false;

            this.txtPrecio.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
            this.txtPrecio.Properties.Mask.BeepOnError = false;
            this.txtPrecio.Properties.Mask.EditMask = @"n3";
            this.txtPrecio.Properties.Mask.IgnoreMaskBlank = true;
            this.txtPrecio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPrecio.Properties.Mask.SaveLiteral = false;
            this.txtPrecio.Properties.Mask.ShowPlaceHolders = false;
            this.txtPrecio.Properties.Mask.UseMaskAsDisplayFormat = true;
        }
        private void CrearEventos()
        {
            this.txtFolio.Validating += this.txtFolio_Validating;
            this.txtVolumen.Validating += this.txtVolumen_Validating;
        }
        private void QuitarEventos()
        {
            this.txtFolio.Validating -= this.txtFolio_Validating;
            this.txtVolumen.Validating -= this.txtVolumen_Validating;
        }

        #region Validaciones

        private bool ValidarFolio(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtFolio.Text))
            {
                msj = "Folio no puede quedar vacío";
                return false;
            }

            int val = 0;
            int.TryParse(this.txtFolio.Text, out val);

            if (val == 0)
            {
                msj = "El valor del folio es inválido";
                return false;
            }

            if (val < 0)
            {
                msj = "El valor del folio no puede ser menor a 0";
                return false;
            }

            return true;
        }
        private bool ValidarPrecio(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtPrecio.Text))
            {
                msj = "Precio no puede quedar vacío";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtPrecio.Text, out val);

            if (val == 0D)
            {
                msj = "El valor de precio es inválido";
                return false;
            }

            if (val < 0D)
            {
                msj = "El valor de precio no puede ser menor a 0";
                return false;
            }

            return true;
        }
        private bool ValidarVolumen(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtVolumen.Text))
            {
                msj = "Volumen no puede quedar vacío";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtVolumen.Text, out val);

            if (val == 0D)
            {
                msj = "El valor de volumen es inválido";
                return false;
            }

            if (val < 0D)
            {
                msj = "El valor de volumen no puede ser menor a 0";
                return false;
            }

            return true;
        }
        private bool ValidarImporte(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtImporte.Text))
            {
                msj = "Importe no puede quedar vacío";
                return false;
            }

            double val = 0D;
            double.TryParse(this.txtImporte.Text, out val);

            if (val == 0D)
            {
                msj = "El valor de importe es inválido";
                return false;
            }

            if (val < 0D)
            {
                msj = "El valor de importe no puede ser menor a 0";
                return false;
            }

            return true;
        }

        #endregion

        #region Eventos

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string msj = string.Empty;
                if (!this.ValidarFolio(ref msj))
                {
                    this.txtFolio.Focus();
                    this.MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarPrecio(ref msj))
                {
                    this.txtPrecio.Focus();
                    this.MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarVolumen(ref msj))
                {
                    this.txtVolumen.Focus();
                    this.MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarImporte(ref msj))
                {
                    this.txtImporte.Focus();
                    this.MensajeInformacion(msj);
                    return;
                }

                Ticket ticket = new Ticket();
                {
                    int iVal = 0;
                    int.TryParse(this.txtFolio.Text, out iVal);
                    ticket.Folio = iVal;

                    double dVal = 0D;
                    double.TryParse(RemoveAll(this.txtPrecio.Text, ",", string.Empty), out dVal);
                    ticket.Precio = dVal;

                    dVal = 0D;
                    double.TryParse(RemoveAll(this.txtVolumen.Text, ",", string.Empty), out dVal);
                    ticket.Volumen = dVal;

                    //double.TryParse(RemoveAll(this.txtImporte.Text, ",", string.Empty), out dVal);
                    //ticket.Importe = dVal;
                    ticket.Importe = ticket.Precio * ticket.Volumen;

                    ticket.NoAplicar = this.chkNoAjustar.Checked;
                }

                if (pServiciosCliente.TicketActualizar(ticket, Configuraciones.NombreUsuario))
                {
                    this.MensajeInformacion("El ticket {0:D8} fue modificado correctamente", ticket.Folio);
                    this.Close();
                }
                else
                {
                    this.MensajeInformacion("No fue posible modificar el ticket {0:D8}", ticket.Folio);
                }
            }
            catch (Exception ex)
            {
                this.MensajeError(ex.Message);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.QuitarEventos();
            this.Close();
        }
        private void txtFolio_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFolio.Text)) { return; }

            int val = 0;
            int.TryParse(this.txtFolio.Text, out val);
            this.txtFolio.Text = val.ToString("D8");

            if (val == 0) { return; }

            if (val < 0)
            {
                e.Cancel = true;
                this.txtFolio.ErrorText = "Valor inválido";
                return;
            }

            try
            {
                var result = pServiciosCliente.TicketObtener(new FiltroTicket() { Folio = val });

                if (result == null)
                {
                    e.Cancel = true;
                    this.txtFolio.ErrorText = "No existe el Ticket";
                    this.MensajeInformacion(this.txtFolio.ErrorText);
                }
                else
                {
                    this.txtImporte.Text = result.Importe.ToString("N3");
                    this.txtPrecio.Text = result.Precio.ToString("N3");
                    this.txtVolumen.Text = result.Volumen.ToString("N3");
                }
            }
            catch (Exception ex)
            {
                this.MensajeError(ex.Message);
                return;
            }
        }
        private void txtVolumen_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtVolumen.Text.Trim()))
            {
                string aux = this.RemoveAll(this.txtPrecio.Text, ",", string.Empty);
                double precio = 0;
                double.TryParse(aux, out precio);

                aux = this.RemoveAll(this.txtVolumen.Text, ",", string.Empty);
                double volumen = 0;
                double.TryParse(aux, out volumen);
                this.txtVolumen.Text = volumen.ToString("N3");

                double importe = precio * volumen;
                this.txtImporte.Text = importe.ToString("N2");
            }
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
        private void MensajeInformacion(string format, params object[] values)
        {
            this.MensajeInformacion(string.Format(format, values));
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

