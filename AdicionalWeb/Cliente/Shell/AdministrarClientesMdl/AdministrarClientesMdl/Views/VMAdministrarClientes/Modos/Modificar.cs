using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarClientes
    {
        private void InicializarModificar()
        {
            this.Entidad.Desface = 1;
            this.Entidad.HorasCorte = 24;
            this.Entidad.Zona = ZonasCambioPrecio.None;
            this.Entidad.MonitorearCambioPrecio = "No";
            this.Entidad.MonitorearTransmisiones = "No";
            this.Entidad.Enlaces = new ListaEnlacesAdministrarClientes();

            this.EntidadAux = this.Entidad.Clonar();
            this.BeginSafe(this.InicializarSoloLecturaModificar);
            this.BeginSafe(this.InicializarControlesModificar);
            this.BeginSafe(this.InicializarValoresDefaultModificar);
            this.BeginSafe(this.CrearEventosModificar);
        }

        private void InicializarControlesModificar()
        {
            this.txtNoEstacion.BeginSafe(delegate
                {
                    this.txtNoEstacion.Properties.Mask.EditMask = this.rgNoEstacion.ToString();
                    this.txtNoEstacion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                    this.txtNoEstacion.Properties.Mask.ShowPlaceHolders = false;
                    this.txtNoEstacion.Properties.Mask.UseMaskAsDisplayFormat = true;
                    this.txtNoEstacion.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
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
        private void InicializarSoloLecturaModificar()
        {
            Permisos varModificar = null;
            if (this.Permiso != null)
            {
                varModificar = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Operaciones.OPERACION_MODIFICAR, StringComparison.CurrentCultureIgnoreCase));
            }

            //this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Properties.ReadOnly = true; });
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Properties.ReadOnly = true; });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Properties.ReadOnly = true; });

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
        }
        private void InicializarValoresDefaultModificar()
        {
            this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Text = this.Entidad.NoEstacion; });
            this.txtNombreComercial.BeginSafe(delegate { this.txtNombreComercial.Text = this.Entidad.NombreComercial; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Text = this.Entidad.EMail; });
            this.txtTelefono.BeginSafe(delegate { this.txtTelefono.Text = this.Entidad.Telefono; });
            this.txtContacto.BeginSafe(delegate { this.txtContacto.Text = this.Entidad.Contacto; });

            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Text = this.Entidad.FechaAlta.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Text = this.Entidad.FechaUltimaConexion.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Checked = this.Entidad.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase); });

            this.txtMatriz.BeginSafe(delegate { this.txtMatriz.Text = string.IsNullOrEmpty(this.Entidad.Matriz) ? string.Empty : this.Entidad.Matriz; });
            this.txtHostPuerto.BeginSafe(delegate { this.txtHostPuerto.Text = string.IsNullOrEmpty(this.Entidad.Host) ? string.Empty : string.Format("{0}:{1}", this.Entidad.Host, this.Entidad.Puerto); });
        }

        private bool ValidarControlesModificar(ref string msj)
        {
            if (!this.ValidarNombreComercial(ref msj, true))
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
            else if (!this.ValidarMatriz(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarHost(ref msj, true))
            {
                return false;
            }

            return true;
        }

        private void CrearEventosModificar()
        {
            this.txtEMail.Validating += this.txtEMailModificar_Validating;
            this.txtHostPuerto.Validating += this.txtHostPuerto_Validating;
            this.txtMatriz.Properties.ButtonClick += this.Properties_ButtonClick;
        }
        private void QuitarEventosModificar()
        {
            this.txtEMail.Validating -= this.txtEMailModificar_Validating;
            this.txtHostPuerto.Validating -= this.txtHostPuerto_Validating;
            this.txtMatriz.Properties.ButtonClick -= this.Properties_ButtonClick;
        }

        #region Eventos

        private void OnGuardarModificar(bool cerrar)
        {
            try
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                string msj = string.Empty;

                if (this.ValidarControlesModificar(ref msj))
                {
                    this.CrearEntidad();

                    var item = this._presenter.Obtener(new FiltroAdministrarClientes()
                        {
                            NoEstacion = this.Entidad.NoEstacion
                        });

                    if (item == null)
                    {
                        Mensaje.MensajeError(ListadoMensajes.Error_Valor_No_Existe, this.Entidad.NoEstacion);
                        return;
                    }

                    if (this.Modificar(this.Entidad))
                    {
                        this._presenter.DisparaEvento();
                        this.EntidadAux = this.Entidad.Clonar();

                        if (cerrar)
                        {
                            this.QuitarEventosModificar();
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
        }

        private void txtEMailModificar_Validating(object sender, CancelEventArgs e)
        {
            string msj = string.Empty;
            e.Cancel = !this.ValidarEMail(ref msj, false);

            if (e.Cancel)
            {
                this.txtEMail.ErrorText = msj;
                Mensaje.MensajeError(msj);
            }
        }

        #endregion
    }
}
