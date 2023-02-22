using System;
using System.Linq;
using EstandarCliente.CargadorVistas.Properties;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VMAdministrarDistribuidores
    {
        private void InicializarModificar()
        {
            this.BeginSafe(this.SetReadOnlyModificar);
            this.BeginSafe(this.CrearEventosModificar);
            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
        }

        internal void SetReadOnlyModificar()
        {
            this.txtClave.BeginSafe(delegate { this.txtClave.Properties.ReadOnly = true; });

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
        }
        internal void CrearEventosModificar()
        {
            this.chkActivo.CheckedChanged += this.chkActivo_CheckedChanged;
        }
        internal void QuitarEventosModificar()
        {
            this.chkActivo.CheckedChanged -= this.chkActivo_CheckedChanged;
        }

        internal bool ValidarControlesModificar(ref string msj)
        {
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

        #region Eventos

        private void OnGuardarModificar(bool cerrar)
        {
            try
            {
                string msj = string.Empty;
                if (this.ValidarControlesModificar(ref msj))
                {
                    this.CrearEntidad();

                    var item = this._presenter.Obtener(new FiltroAdministrarDistribuidores()
                    {
                        Clave = this.Entidad.Clave
                    });

                    if (item == null)
                    {
                        Mensaje.MensajeError(ListadoMensajes.Error_Valor_No_Existe, this.Entidad.Clave.ToString("D3"));
                        return;
                    }

                    var entidad = this.Modificar(this.Entidad);
                    if (entidad != null)
                    {
                        this._presenter.DisparaEvento();
                        this.Entidad = entidad;
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
        private void chkActivo_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkActivo.Checked)
            {
                int distribuidor = 0;
                int.TryParse(this.txtClave.Text, out distribuidor);

                if (distribuidor == 1)
                {
                    Mensaje.MensajeWarn("No es posible desactivar al distribuidor Matriz.");
                    this.chkActivo.BeginSafe(delegate { this.chkActivo.Checked = true; });
                }
            }
        }

        #endregion
    }
}
