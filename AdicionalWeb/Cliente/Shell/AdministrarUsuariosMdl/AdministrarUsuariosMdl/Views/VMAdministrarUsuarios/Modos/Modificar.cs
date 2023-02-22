using System;
using System.ComponentModel;
using System.Linq;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using System.Windows.Forms;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VMAdministrarUsuarios
    {
        private void InicializarModificar()
        {
            this.EntidadAux = this.Entidad.Clonar();
            this.BeginSafe(this.InicializarSoloLecturaModificar);
            this.BeginSafe(this.InicializarControlesModificar);
            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
            this.BeginSafe(this.CrearEventosModificar);
        }

        private void InicializarControlesModificar()
        {
            this.txtEMail.BeginSafe(delegate
                {
                    this.txtEMail.Properties.Mask.ShowPlaceHolders = false;
                    this.txtEMail.Properties.Mask.UseMaskAsDisplayFormat = false;
                    this.txtEMail.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
                    this.txtEMail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    this.txtEMail.Properties.Mask.EditMask = this.rgEmail.ToString();
                });

            if (this.Entidad.Clave == 0)
            {
                this.chkEsDistribuidor.BeginSafe(delegate
                {
                    this.chkEsDistribuidor.Checked = true;
                    this.chkEsDistribuidor.Properties.ReadOnly = true;
                });
                this.luDistribuidor.BeginSafe(delegate
                {
                    this.luDistribuidor.Visible = true;
                    this.luDistribuidor.Properties.ReadOnly = true;
                });
            }
            else if (this.Entidad.Clave > 0 && this.Entidad.IdDistribuidor > 0)
            {
                this.chkEsDistribuidor.BeginSafe(delegate { this.chkEsDistribuidor.Checked = true; });
                this.luDistribuidor.BeginSafe(delegate { this.luDistribuidor.Visible = true; });

                if (this.Entidad.IdDistribuidor > 1)
                {
                    this.btnPermisos.BeginSafe(delegate { this.btnPermisos.Enabled = false; });
                }
            }
        }
        private void InicializarSoloLecturaModificar()
        {
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Properties.ReadOnly = true; });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Properties.ReadOnly = true; });

            Permisos varModificar = null;
            if (this.Permiso != null)
            {
                varModificar = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Operaciones.OPERACION_MODIFICAR, StringComparison.CurrentCultureIgnoreCase));
            }

            this.chkActivo.BeginSafe(delegate
                {
                    if (this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.chkActivo.Properties.ReadOnly = false;
                    }
                    else
                    {
                        if (varModificar != null)
                        {
                            var varActivar = varModificar.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Opciones.OPCION_ACTIVAR, StringComparison.CurrentCultureIgnoreCase));
                            this.chkActivo.Properties.ReadOnly = ((varActivar != null) ? !varActivar.Permitido : true);
                        }
                        else
                        {
                            this.chkActivo.Properties.ReadOnly = true;
                        }
                    }
                });
            this.txtContrasena.BeginSafe(delegate
                {
                    if (this.Modo == EstandarCliente.CargadorVistas.Constants.ModoModulo.Registrar)
                    {
                        this.txtContrasena.Properties.Buttons[0].Enabled = true;
                    }
                    else
                    {
                        if (this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.txtContrasena.Properties.Buttons[0].Enabled = true;
                        }
                        else
                        {
                            if (varModificar != null)
                            {
                                var varContrasena = varModificar.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Opciones.OPCION_CAMBIAR_CONTRASEÑA, StringComparison.CurrentCultureIgnoreCase));
                                this.txtContrasena.Properties.Buttons[0].Enabled = ((varContrasena != null) ? varContrasena.Permitido : true);
                            }
                            else
                            {
                                this.txtContrasena.Properties.Buttons[0].Enabled = false;
                            }
                        }
                    }
                });
        }

        private bool ValidarControlesModificar(ref string msj)
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

            return true;
        }

        private void CrearEventosModificar()
        {
            this.txtEMail.Validating += this.txtEMailModificar_Validating;
            this.txtContrasena.ButtonClick += this.txtContrasena_ButtonClick;
            this.chkActivo.CheckedChanged += this.chkActivo_CheckedChanged;
            this.luDistribuidor.EditValueChanged += this.luDistribuidor_EditValueChanged;
            this.chkEsDistribuidor.CheckedChanged += this.chkEsDistribuidor_CheckedChanged;
        }
        private void QuitarEventosModificar()
        {
            this.txtEMail.Validating -= this.txtEMailModificar_Validating;
            this.txtContrasena.ButtonClick -= this.txtContrasena_ButtonClick;
            this.chkActivo.CheckedChanged -= this.chkActivo_CheckedChanged;
        }

        #region Eventos

        private void OnGuardarModificar(bool cerrar)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                string msj = string.Empty;
                if (this.ValidarControlesModificar(ref msj))
                {
                    this.CrearEntidad();

                    if (this.Entidad.Clave == 0)
                    {
                        this.Entidad.IdDistribuidor = 1;
                        this.Entidad.Permisos = new ListaPermisos();
                    }

                    var item = this._presenter.Obtener(new FiltroAdministrarUsuarios()
                        {
                            Clave = this.Entidad.Clave
                        });

                    if (item == null)
                    {
                        this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                        Mensaje.MensajeError(ListadoMensajes.Error_Valor_No_Existe, this.Entidad.Clave);
                        return;
                    }

                    if (this.Modificar(this.Entidad))
                    {
                        this._presenter.DisparaEvento();
                        this.EntidadAux = this.Entidad.Clone();
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
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                this._presenter.DisparaEvento();
            }
        }

        private void chkActivo_CheckedChanged(object sender, EventArgs e)
        {
            if (Sesion.Clave == this.Entidad.Clave)
            {
                Mensaje.MensajeWarn(ListadoMensajes.Error_No_se_puede_Eliminar_SP);
                this.chkActivo.Checked = this.Entidad.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        private void txtEMailModificar_Validating(object sender, CancelEventArgs e)
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
