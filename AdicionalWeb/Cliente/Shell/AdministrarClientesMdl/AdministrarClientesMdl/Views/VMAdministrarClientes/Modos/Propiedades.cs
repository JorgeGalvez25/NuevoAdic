using System;
using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarClientes
    {
        private void InicializarPropiedades()
        {
            this.Entidad.Desface = 1;
            this.Entidad.HorasCorte = 24;
            this.Entidad.Zona = ZonasCambioPrecio.None;
            this.Entidad.MonitorearCambioPrecio = "No";
            this.Entidad.MonitorearTransmisiones = "No";
            this.Entidad.Enlaces = new ListaEnlacesAdministrarClientes();

            this.EntidadAux = this.Entidad.Clonar();
            this.BeginSafe(this.InicializarSoloLecturaPropiedades);
        }

        private void InicializarSoloLecturaPropiedades()
        {
            this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Properties.ReadOnly = true; });
            this.txtNombreComercial.BeginSafe(delegate { this.txtNombreComercial.Properties.ReadOnly = true; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.ReadOnly = true; });
            this.txtTelefono.BeginSafe(delegate { this.txtTelefono.Properties.ReadOnly = true; });
            this.txtContacto.BeginSafe(delegate { this.txtContacto.Properties.ReadOnly = true; });

            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Properties.ReadOnly = true; });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Properties.ReadOnly = true; });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Properties.ReadOnly = true; });
            this.luDistribuidor.BeginSafe(delegate { this.luDistribuidor.Properties.ReadOnly = true; });

            this.txtMatriz.BeginSafe(delegate { this.txtMatriz.Properties.ReadOnly = true; });
            this.txtHostPuerto.BeginSafe(delegate { this.txtHostPuerto.Properties.ReadOnly = true; });
        }
        private void InicializarValoresDefaultPropiedades()
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
    }
}
