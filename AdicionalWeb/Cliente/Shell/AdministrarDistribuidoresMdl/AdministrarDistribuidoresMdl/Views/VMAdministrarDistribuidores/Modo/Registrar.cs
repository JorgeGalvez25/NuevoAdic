using System;
using System.Text;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VMAdministrarDistribuidores
    {
        internal Func<int> ObtenerConsecutivo;

        private void InicializarRegistrar()
        {
            if (this.ObtenerConsecutivo == null)
            {
                this.ObtenerConsecutivo = new Func<int>(Consecutivo);
            }

            this.ObtenerConsecutivo.BeginInvoke(ValorClaveConsecutivo, null);
            this.chkActivo.BeginSafe(delegate
                {
                    this.chkActivo.Visible = false;
                    this.chkActivo.Checked = true;
                });

            this.BeginSafe(CrearEventosResgistrar);
        }

        internal void CrearEventosResgistrar()
        {
            this.txtClave.Validated += this.txtClave_Validated;
            this.txtClave.DoubleClick += this.txtClave_DoubleClick;
        }
        internal void QuitarEventosRegistro()
        {
            this.txtClave.Validated -= this.txtClave_Validated;
        }

        internal bool ValidarControlesRegistrar(ref string msj)
        {
            if (string.IsNullOrEmpty(this.txtClave.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "Clave");
                this.txtClave.Focus();
                return false;
            }
            if (!this.ValidarClave(ref msj))
            {
                this.txtClave.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtDescripcion.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "Descripción");
                this.txtDescripcion.Focus();
                return false;
            }

            if ((this.gridControl1.DataSource as ListaStringGridColumn).Count <= 0)
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "E-Mail(s)");
                this.txtEMail.Focus();
                return false;
            }

            if (!this.ValidarEmail((this.gridControl1.DataSource as ListaStringGridColumn).ToString(), ref msj))
            {
                this.txtEMail.Focus();
                return false;
            }

            return true;
        }
        internal void ValorClaveConsecutivo(IAsyncResult result)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                this.txtClave.BeginSafe(delegate
                    {
                        int consecutivo = this.ObtenerConsecutivo.EndInvoke(result);
                        this.txtClave.Text = consecutivo.ToString("D3");
                    });
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                this.txtClave.BeginSafe(delegate { this.txtClave.Properties.ReadOnly = false; });
            }
        }

        #region Eventos

        private void OnGuardarRegistrar(bool cerrar)
        {
            try
            {
                string msj = string.Empty;
                if (this.ValidarControlesRegistrar(ref msj))
                {
                    this.CrearEntidad();
                    if (this.Entidad.Clave == 0) { this.Entidad.Clave = this.Consecutivo(); }

                    var item = this._presenter.Obtener(new FiltroAdministrarDistribuidores() { Clave = this.Entidad.Clave });

                    if (item != null)
                    {
                        StringBuilder sb = new StringBuilder(string.Format(ListadoMensajes.Error_Valor_Existe, string.Empty, this.Entidad.Clave).Trim());
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

                    var entidad = this.Insertar(this.Entidad);
                    if (entidad != null)
                    {
                        this._presenter.DisparaEvento();
                        this.Entidad = new AdministrarDistribuidores();
                        this.Entidad.Clave = this.ObtenerConsecutivo();
                        this.EntidadAux = this.Entidad.Clonar();
                        this.InicializarControlesEntidad(this.Entidad);
                        if (cerrar)
                        {
                            this.QuitarEventosRegistro();
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

        private void txtClave_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtClave.Text.Trim())) { return; }

            string msj = string.Empty;
            this.txtClave.ErrorText = string.Empty;

            if (!this.ValidarClave(ref msj))
            {
                Mensaje.MensajeError(msj);
                this.txtClave.ErrorText = msj;
                return;
            }
        }
        private void txtClave_DoubleClick(object sender, EventArgs e)
        {
            this.txtClave.BeginSafe(delegate { this.txtClave.Properties.ReadOnly = true; });
            this.ObtenerConsecutivo.BeginInvoke(ValorClaveConsecutivo, null);
        }

        #endregion
    }
}
