using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VMAdministrarUsuarios
    {
        private void InicializarPropiedades()
        {
            this.EntidadAux = (AdministrarUsuarios)this.Entidad.Clonar();
            this.BeginSafe(this.InicializarSoloLecturaPropiedades);
            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
        }

        private void InicializarSoloLecturaPropiedades()
        {
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Properties.ReadOnly = true; });
            this.txtPuesto.BeginSafe(delegate { this.txtPuesto.Properties.ReadOnly = true; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.ReadOnly = true; });
            this.txtContrasena.BeginSafe(delegate { this.txtContrasena.Properties.ReadOnly = true; });

            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Properties.ReadOnly = true; });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Properties.ReadOnly = true; });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Properties.ReadOnly = true; });
            this.chkEsDistribuidor.BeginSafe(delegate { this.chkEsDistribuidor.Properties.ReadOnly = true; });
            this.luDistribuidor.BeginSafe(delegate { this.luDistribuidor.Properties.ReadOnly = true; });
        }
    }
}
