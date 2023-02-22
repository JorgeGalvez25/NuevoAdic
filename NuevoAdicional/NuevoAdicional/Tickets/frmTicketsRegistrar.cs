using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Adicional.Entidades;
using System.Linq;
using Persistencia;


namespace NuevoAdicional
{
    public partial class frmTicketsRegistrar : Form
    {
        private ListaCombustible _combustibles;
        public int Id;
        public ServiciosCliente.IServiciosCliente pServiciosCliente;
        public string NombreEst;

        public frmTicketsRegistrar()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pServiciosCliente = Configuraciones.ListaCanales[Id];
            if (!Configuraciones.CanalEstaActivo(Id, false))
            {
                pServiciosCliente = Configuraciones.AbrirCanalCliente(Id);
            }
            this._combustibles = this.pServiciosCliente.ObtenerCombustibles();
            this.IncializarControles();

            this.CrearEventos();
            this.luCombustible_EditValueChanged(null, null);
            this.dtFecha.EditValue = DateTime.Now;
            this.chkFecha_CheckedChanged(null, null);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.spnPoscionCarga.Focus();
        }

        #region Inicilizacion

        private void IncializarControles()
        {
            this.IncializarFolio();
            this.IncializarPrecio();
            this.InicializarCombo();
            this.InicializarPosicionCarga();
        }

        private void IncializarFolio()
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

            this.txtFolio_DoubleClick(null, null);
        }
        private void IncializarPrecio()
        {
            this.txtPrecio.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
            this.txtPrecio.Properties.Mask.BeepOnError = false;
            this.txtPrecio.Properties.Mask.EditMask = @"n3";
            this.txtPrecio.Properties.Mask.IgnoreMaskBlank = true;
            this.txtPrecio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPrecio.Properties.Mask.SaveLiteral = false;
            this.txtPrecio.Properties.Mask.ShowPlaceHolders = false;
            this.txtPrecio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPrecio.Properties.MaxLength = 15;
        }

        private void InicializarCombo()
        {
            this.InicializarLookUp(this.luCombustible);
            this.luCombustible.Properties.DataSource = (from l in this._combustibles.OrderBy(p => p.Clave)
                                                        select new
                                                        {
                                                            Display = l.Nombre,
                                                            Value = l.Clave,
                                                            l.Precio
                                                        }).ToArray();
            this.luCombustible.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.luCombustible.EditValue = this._combustibles.Select(p => p.Clave).Min();
        }
        private void InicializarPosicionCarga()
        {
            int valor = 0;

            if (this._combustibles != null && this._combustibles.Count > 0)
            {
                valor = this._combustibles[0].MaxPosCarga;
            }

            this.spnPoscionCarga.Properties.MaxValue = valor;
        }

        #endregion

        private void CrearEventos()
        {
            this.txtFolio.Validating += this.txtFolio_Validating;
            this.txtFolio.DoubleClick += this.txtFolio_DoubleClick;
            this.txtVolumen.Validating += this.txtVolumen_Validating;

            this.luCombustible.EditValueChanged += this.luCombustible_EditValueChanged;

            this.btnAceptar.Click += this.btnAceptar_Click;

            this.chkFecha.CheckedChanged += this.chkFecha_CheckedChanged;
        }
        private void QuitarEventos()
        {
            this.txtFolio.Validating -= this.txtFolio_Validating;
            this.txtVolumen.Validating -= this.txtVolumen_Validating;
            this.chkFecha.CheckedChanged -= this.chkFecha_CheckedChanged;
        }
        private Ticket ObtenerEntidad()
        {
            var entidad = new Ticket();
            var combustible = this.ObtenerCombustibleLookUp();

            int iVal = 0;
            int.TryParse(this.txtFolio.Text, out iVal);
            entidad.Folio = iVal;
            double dVal = 0D;
            dVal = 0D;
            double.TryParse(this.RemoveAll(this.txtPrecio.Text, ",", string.Empty), out dVal);
            entidad.Precio = dVal;

            dVal = 0D;
            double.TryParse(this.RemoveAll(this.txtVolumen.Text, ",", string.Empty), out dVal);
            entidad.Volumen = dVal;

            //double.TryParse(this.RemoveAll(this.txtImporte.Text, ",", string.Empty), out dVal);
            //entidad.Importe = dVal;
            entidad.Importe = entidad.Precio * entidad.Volumen;

            //entidad.Precio = ((combustible == null) ? 0 : combustible.Precio);
            entidad.Posicion = decimal.ToInt32(this.spnPoscionCarga.Value);
            entidad.Combustible = combustible == null ? 0 : combustible.Clave;
            if (dtFecha.Enabled)
                entidad.Fecha = dtFecha.DateTime;
            entidad.NoAplicar = this.chkNoAjustar.Checked;

            return entidad;
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
                    MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarPrecio(ref msj))
                {
                    this.txtPrecio.Focus();
                    MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarVolumen(ref msj))
                {
                    this.txtVolumen.Focus();
                    MensajeInformacion(msj);
                    return;
                }

                if (!this.ValidarImporte(ref msj))
                {
                    this.txtImporte.Focus();
                    MensajeInformacion(msj);
                    return;
                }

                var entidad = ObtenerEntidad();
                var result = pServiciosCliente.TicketRegistrar(entidad, Configuraciones.NombreUsuario);

                if (result == null)
                {
                    this.MensajeInformacion("No fue posible registrar el ticket {0:D6}", entidad.Folio);
                }
                else if (result.Folio != entidad.Folio)
                {
                    this.MensajeInformacion("El ticket fue registrado con el folio {0:D6} y no con el folio {1:D6}", result.Folio, entidad.Folio);
                    this.Close();
                }
                else
                {
                    this.MensajeInformacion("El ticket {0:D6} fue registrado correctamente", entidad.Folio);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                this.MensajeError(ex.Message);
            }
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

                if (result != null)
                {
                    e.Cancel = true;
                    this.txtFolio.ErrorText = "Ya existe el Folio del Ticket";
                    this.MensajeInformacion(this.txtFolio.ErrorText);
                    this.txtFolio_DoubleClick(null, null);
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
            //System.Text.RegularExpressions.Regex.Match(this.txtVolumen.Text, @"^([0-9]|,[0-9])+(.[0-9]+)?$");
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
                this.txtImporte.Text = importe.ToString("N3");
            }
        }

        private void txtFolio_DoubleClick(object sender, EventArgs e)
        {
            this.txtFolio.Text = this.pServiciosCliente.TicketConsecutivo().ToString("D8");
        }

        private void luCombustible_EditValueChanged(object sender, EventArgs e)
        {
            var combustible = ObtenerCombustibleLookUp();
            this.txtPrecio.Text = ((combustible == null) ? 0 : combustible.Precio).ToString("N3");
            this.txtVolumen_Validating(null, null);
        }

        private void chkFecha_CheckedChanged(object sender, EventArgs e)
        {
            this.dtFecha.Enabled = this.chkFecha.Checked;
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

        private Combustible ObtenerCombustibleLookUp()
        {
            return this._combustibles.Find(p => p.Clave == (int)this.luCombustible.EditValue);
        }
    }
}
