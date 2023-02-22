using System;
using System.ComponentModel;
using System.Text;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using System.Windows.Forms;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VMAdministrarUsuarios
    {
        private void InicializarRegistrar()
        {
            this.EntidadAux = this.Entidad.Clone();
            this.BeginSafe(this.InicializarSoloLecturaRegistrar);
            this.BeginSafe(this.InicializarValoresDefaultRegistrar);
            this.BeginSafe(this.InicializarControlesRegistrar);
            this.BeginSafe(this.CrearEventosRegistro);
        }

        private void InicializarControlesRegistrar()
        {
            this.txtEMail.BeginSafe(delegate
            {
                this.txtEMail.Properties.Mask.ShowPlaceHolders = false;
                this.txtEMail.Properties.Mask.UseMaskAsDisplayFormat = false;
                this.txtEMail.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
                this.txtEMail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                this.txtEMail.Properties.Mask.EditMask = this.rgEmail.ToString();
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
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Text = this._presenter.ObtenerFechaHoraServidor().ToString("dd/MM/yyyy HH:mm:ss.tttt"); });//DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.tttt"); });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Text = System.Data.SqlTypes.SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy HH:mm:ss.tttt"); });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Checked = true; });
        }

        private void CrearEventosRegistro()
        {
            this.txtEMail.Validating += this.txtEMailRegistrar_Validating;
            this.txtContrasena.ButtonClick += this.txtContrasena_ButtonClick;
            this.luDistribuidor.EditValueChanged += this.luDistribuidor_EditValueChanged;
            this.chkEsDistribuidor.CheckedChanged += this.chkEsDistribuidor_CheckedChanged;
        }
        private void QuitarEventosRegistro()
        {
            this.txtEMail.Validating -= this.txtEMailRegistrar_Validating;
            this.txtContrasena.ButtonClick -= this.txtContrasena_ButtonClick;
        }

        private bool ValidarControlesRegistrar(ref string msj)
        {
            if (!this.ValidarNombre(ref msj))
            {
                return false;
            }
            else if (!this.ValidarPuesto(ref msj))
            {
                return false;
            }
            else if (!this.ValidarEMail(ref msj))
            {
                return false;
            }
            else if (!this.ValidarContrasena(ref msj))
            {
                return false;
            }
            //else if (!this.ValidarTabla(ref msj))
            //{
            //    return false;
            //}

            return true;
        }

        #region Eventos

        private void OnGuardarRegistrar(bool cerrar)
        {
            try
            {
                string msj = string.Empty;
                this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                if (this.ValidarControlesRegistrar(ref msj))
                {
                    this.CrearEntidad();
                    if (this.Entidad.Clave == 0)
                    {
                        this.Entidad.Clave = this.Consecutivo();
                    }

                    var item = this._presenter.Obtener(new FiltroAdministrarUsuarios()
                        {
                            Nombre = this.Entidad.Nombre
                        });

                    if (item != null)
                    {
                        StringBuilder sb = new StringBuilder(string.Format(ListadoMensajes.Error_Valor_Existe, string.Empty, this.Entidad.Nombre).Trim());
                        if (item.Activo.Equals("No", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (sb.ToString().EndsWith("."))
                            {
                                int idx = sb.ToString().LastIndexOf(".");
                                sb = sb.Remove(idx, 1);
                            }

                            sb = sb.AppendFormat(", {0}", string.Format(ListadoMensajes.Error_Valor_Inactivo, "pero").Trim());
                        }

                        this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                        Mensaje.MensajeError(sb.ToString());
                        return;
                    }

                    if (this.Insertar(this.Entidad))
                    {
                        this._presenter.DisparaEvento();
                        this.Entidad = new AdministrarUsuarios() { Fecha = this.ObtenerFechaHoraServidor() };
                        this.EntidadAux = this.Entidad.Clone();
                        this.InicializarControlesEntidad(this.Entidad);
                        if (cerrar)
                        {
                            this.BotonCerrarClick();
                        }
                    }
                }
                else
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    Mensaje.MensajeError(msj);
                }
            }
            catch (Exception e)
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                Mensaje.MensajeError(e.Message);
            }
            finally
            {
                this._presenter.DisparaEvento();
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        private void chkEsDistribuidor_CheckedChanged(object sender, EventArgs e)
        {
            this.luDistribuidor.Visible = this.chkEsDistribuidor.Checked;
            this.luDistribuidor_EditValueChanged(null, null);
            this.btnPermisos.Enabled = !this.chkEsDistribuidor.Checked || ((int)this.luDistribuidor.EditValue <= 1);
        }
        private void luDistribuidor_EditValueChanged(object sender, EventArgs e)
        {
            int value = (int)(this.luDistribuidor.EditValue ?? 1);
            this.btnPermisos.Enabled = (value == 1);
        }
        private void txtEMailRegistrar_Validating(object sender, CancelEventArgs e)
        {
            if (this.StringIsNullOrEmpty(this.txtEMail.Text)) { return; }

            string msj = string.Empty;
            e.Cancel = !this.ValidarEMail(ref msj);

            if (e.Cancel)
            {
                this.txtEMail.ErrorText = msj;
                Mensaje.MensajeError(msj);
            }
        }

        #endregion
    }
}
