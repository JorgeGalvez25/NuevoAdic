using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarClientes
    {
        private void InicializarRegistrar()
        {
            this.EntidadAux = this.Entidad.Clonar();
            this.BeginSafe(this.InicializarSoloLecturaRegistrar);
            this.BeginSafe(this.InicializarValoresDefaultRegistrar);
            this.BeginSafe(this.InicializarControlesRegistrar);
            this.BeginSafe(this.CrearEventosRegistro);
        }

        private void InicializarControlesRegistrar()
        {
            this.txtNoEstacion.BeginSafe(delegate
                {
                    this.txtNoEstacion.Properties.Mask.ShowPlaceHolders = false;
                    this.txtNoEstacion.Properties.Mask.UseMaskAsDisplayFormat = false;
                    this.txtNoEstacion.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
                    this.txtNoEstacion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    this.txtNoEstacion.Properties.Mask.EditMask = this.rgNoEstacion.ToString();
                });
            this.txtEMail.BeginSafe(delegate
                {
                    this.txtEMail.Properties.Mask.ShowPlaceHolders = false;
                    this.txtEMail.Properties.Mask.UseMaskAsDisplayFormat = false;
                    this.txtEMail.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
                    this.txtEMail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    this.txtEMail.Properties.Mask.EditMask = this.rgEmail.ToString();
                });
            this.txtTelefono.BeginSafe(delegate
                {
                    this.txtTelefono.Properties.Mask.ShowPlaceHolders = false;
                    this.txtTelefono.Properties.Mask.UseMaskAsDisplayFormat = false;
                    this.txtTelefono.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
                    this.txtTelefono.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    this.txtTelefono.Properties.Mask.EditMask = @"([0-9]{7,15})";
                });
        }
        private void InicializarSoloLecturaRegistrar()
        {
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Properties.ReadOnly = true; });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Properties.ReadOnly = true; });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Visible = false; });
        }
        private void InicializarValoresDefaultRegistrar()
        {
            this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Text = "E"; });
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Text = this._presenter.ObtenerFechaHoraServidor().ToString("dd/MM/yyyy HH:mm:ss"); });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Text = System.Data.SqlTypes.SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Checked = true; });
        }

        private bool ValidarControlesRegistrar(ref string msj)
        {
            if (!this.ValidarNoEstacion(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarNombreComercial(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarEMail(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarTelefono(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarContacto(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarMatriz(ref msj, false))
            {
                return false;
            }
            else if (!this.ValidarHost(ref msj, false))
            {
                return false;
            }

            return true;
        }

        private void CrearEventosRegistro()
        {
            this.txtEMail.Validating += this.txtEMailRegistrar_Validating;
            this.txtHostPuerto.Validating += this.txtHostPuerto_Validating;
            this.txtNoEstacion.Validating += this.txtNoEstacionRegistrar_Validating;
            this.txtNoEstacion.Validated += this.txtNoEstacion_Validated;
            this.txtNoEstacion.PreviewKeyDown += this.txtNoEstacion_PreviewKeyDown;

            this.txtMatriz.Properties.ButtonClick += this.Properties_ButtonClick;
        }
        private void QuitarEventosRegistro()
        {
            this.txtEMail.Validating -= this.txtEMailRegistrar_Validating;
            this.txtHostPuerto.Validating -= this.txtHostPuerto_Validating;
            this.txtNoEstacion.Validating -= this.txtNoEstacionRegistrar_Validating;
            this.txtNoEstacion.PreviewKeyDown -= this.txtNoEstacion_PreviewKeyDown;

            this.txtMatriz.Properties.ButtonClick -= this.Properties_ButtonClick;
        }

        #region Eventos

        private void OnGuardarRegistrar(bool cerrar)
        {
            try
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                string msj = string.Empty;

                if (this.ValidarControlesRegistrar(ref msj))
                {
                    this.CrearEntidad();

                    var item = this._presenter.Obtener(new FiltroAdministrarClientes()
                        {
                            NoEstacion = this.Entidad.NoEstacion
                        });

                    if (item != null)
                    {
                        StringBuilder sb = new StringBuilder(string.Format(ListadoMensajes.Error_Valor_Existe, string.Empty, this.Entidad.NoEstacion).Trim());

                        if (item.Activo.Equals("No", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (sb.ToString().EndsWith("."))
                            {
                                int idx = sb.ToString().LastIndexOf(".");
                                sb = sb.Remove(idx, 1);
                            }

                            sb = sb.AppendFormat(", {0}", string.Format(ListadoMensajes.Error_Valor_Inactivo, "pero").Trim());
                        }

                        Mensaje.MensajeError(sb.ToString());
                        return;
                    }

                    if (this.Insertar(this.Entidad))
                    {
                        this.Entidad = new AdministrarClientes()
                            {
                                FechaAlta = this.ObtenerFechaHoraServidor()
                            };
                        this.EntidadAux = this.Entidad.Clonar();
                        this.InicializarControlesEntidad(this.Entidad);

                        if (cerrar)
                        {
                            this.BotonCerrarClick();
                        }
                    }
                }
                else
                {
                    Mensaje.MensajeError(msj);
                }
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }
            finally
            {
                this._presenter.DisparaEvento();
            }
        }

        private void txtHostPuerto_Validating(object sender, CancelEventArgs e)
        {
            string msj = string.Empty;
            e.Cancel = !this.ValidarHost(ref msj, false);

            if (e.Cancel)
            {
                this.txtHostPuerto.ErrorText = msj;
                Mensaje.MensajeError(msj);
            }
        }
        private void txtEMailRegistrar_Validating(object sender, CancelEventArgs e)
        {
            string msj = string.Empty;
            e.Cancel = !this.ValidarEMail(ref msj, false);

            if (e.Cancel)
            {
                this.txtEMail.ErrorText = msj;
                Mensaje.MensajeError(msj);
            }
        }
        private void txtNoEstacionRegistrar_Validating(object sender, CancelEventArgs e)
        {
            string msj = string.Empty;
            e.Cancel = !this.ValidarNoEstacion(ref msj, false);

            if (e.Cancel)
            {
                this.txtNoEstacion.ErrorText = msj;
                Mensaje.MensajeError(msj);
                return;
            }
        }

        private void txtNoEstacion_Validated(object sender, EventArgs e)
        {
            if (this.StringIsNullOrEmpty(this.txtMatriz.Text))
            {
                this.txtMatriz.Text = this.txtNoEstacion.Text;
            }
        }

        #endregion
    }
}
