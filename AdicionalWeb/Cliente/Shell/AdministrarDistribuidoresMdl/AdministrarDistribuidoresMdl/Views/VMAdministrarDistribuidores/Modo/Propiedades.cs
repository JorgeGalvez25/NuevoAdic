using ImagenSoft.Extensiones;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VMAdministrarDistribuidores
    {
        private void InicializarPropiedades()
        {
            this.BeginSafe(this.SetReadOnlyPropiedades);
            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
        }

        internal void SetReadOnlyPropiedades()
        {
            this.txtClave.BeginSafe(delegate { this.txtClave.Properties.ReadOnly = true; });
            this.txtDescripcion.BeginSafe(delegate { this.txtDescripcion.Properties.ReadOnly = true; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.ReadOnly = true; });
            this.txtEMail.BeginSafe(delegate { this.btnAdd.Enabled = false; });
            this.txtEMail.BeginSafe(delegate { this.btnDelete.Enabled = false; });
            this.txtEMail.BeginSafe(delegate
                {
                    for (int i = 0; i < this.gridView1.Columns.Count; i++)
                    {
                        if (this.gridView1.Columns[i].ColumnEdit == null)
                        {
                            this.gridView1.Columns[i].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                        }
                        (this.gridView1.Columns[i].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).ReadOnly = true;
                    }
                });
            this.chkActivo.BeginSafe(delegate { this.chkActivo.Properties.ReadOnly = true; });
        }
    }
}
