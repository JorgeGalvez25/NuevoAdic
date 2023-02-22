using System;
using System.Windows.Forms;
using Adicional.Entidades;
using Persistencia;

namespace NuevoAdicional.EntradaTanques
{
    public partial class frmTanquesListado : Form
    {
        private ListaTanques lista;
        public int Id;
        public string NombreEst;
        public ServiciosCliente.IServiciosCliente pServiciosCliente;

        public frmTanquesListado()
        {
            InitializeComponent();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.dtFecha.EditValue = DateTime.Now.Date;
            pServiciosCliente = Configuraciones.ListaCanales[Id];
            if (!Configuraciones.CanalEstaActivo(Id, false))
            {
                pServiciosCliente = Configuraciones.AbrirCanalCliente(Id);
            }
            this.lista = pServiciosCliente.TanquesObtenerTodos(new FiltroTanques() { Fecha = this.dtFecha.DateTime.Date });
            this.InicializarTabla();
            this.CrearEventos();
        }

        private void InicializarTabla()
        {
            this.gridControl1.DataSource = this.lista;
            this.gridControl1.SuspendLayout();
            this.gridView1.BeginInit();

            this.gridControl1.UseEmbeddedNavigator = false;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowFooter = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateFocusedItem;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = true;
            this.gridView1.OptionsCustomization.AllowRowSizing = false;

            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].Visible = false;
            }

            this.gridView1.Columns["Folio"].VisibleIndex = 0;
            this.gridView1.Columns["Fecha"].VisibleIndex = 1;
            this.gridView1.Columns["FechaHora"].VisibleIndex = 2;
            this.gridView1.Columns["Corte"].VisibleIndex = 3;
            this.gridView1.Columns["Tanque"].VisibleIndex = 4;
            this.gridView1.Columns["Combustible"].VisibleIndex = 5;
            this.gridView1.Columns["VolumenRecepcion"].VisibleIndex = 6;
            this.gridView1.Columns["Generado"].VisibleIndex = 6;

            this.gridView1.Columns["FechaHora"].Caption = "Fecha Hora";
            this.gridView1.Columns["VolumenRecepcion"].Caption = "Volumen Recepción";

            this.gridView1.Columns["FechaHora"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            (this.gridView1.Columns["FechaHora"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            (this.gridView1.Columns["FechaHora"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss tt";

            this.gridView1.Columns["VolumenRecepcion"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            (this.gridView1.Columns["VolumenRecepcion"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            (this.gridView1.Columns["VolumenRecepcion"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatString = "N2";

            this.gridControl1.ResumeLayout();
            this.gridView1.EndInit();
            this.gridControl1.Refresh();
            this.gridView1.BestFitColumns();

            if (this.gridControl1.ContextMenuStrip != null)
            {
                this.gridControl1.ContextMenuStrip.Items.Clear();
            }
        }

        private void CrearEventos()
        {
            this.dtFecha.EditValueChanged += this.dtFecha_EditValueChanged;
            this.btnCerrar.Click += this.btnCerrar_Click;
            this.btnEliminar.Click += this.btnEliminar_Click;
            this.btnInsertar.Click += this.btnInsertar_Click;
            this.btnModificar.Click += this.btnModificar_Click;
        }

        #region Eventos

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            using (frmTanquesRegistrar registro = new frmTanquesRegistrar())
            {
                registro.servicio = pServiciosCliente;
                registro.ShowDialog();
                this.lista = pServiciosCliente.TanquesObtenerTodos(new FiltroTanques() { Fecha = this.dtFecha.DateTime.Date });
                this.gridControl1.DataSource = this.lista;
                this.gridControl1.Refresh();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var seleccion = (Tanques)this.gridView1.GetFocusedRow();
            if (seleccion == null) { return; }
            if (this.MensajeConfirmacion("¿Eliminar entrada?") == DialogResult.Yes)
            {
                try
                {
                    this.pServiciosCliente.TanquesEliminar(new FiltroTanques() { Folio = seleccion.Folio, Fecha = seleccion.Fecha }, Configuraciones.NombreUsuario);
                }
                finally
                {
                    this.dtFecha_EditValueChanged(null, null);
                }
            }
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            var tanque = (Tanques)this.gridView1.GetFocusedRow();

            if (tanque == null)
            {
                MensajeInformacion("Seleccione un registro para modificar.");
                return;
            }
            using (frmTanquesModificar modificar = new frmTanquesModificar(tanque))
            {
                modificar.servicio = pServiciosCliente;
                modificar.ShowDialog();
                this.lista = pServiciosCliente.TanquesObtenerTodos(new FiltroTanques() { Fecha = this.dtFecha.DateTime.Date });
                this.gridControl1.DataSource = this.lista;
                this.gridControl1.Refresh();
            }
        }
        private void dtFecha_EditValueChanged(object sender, EventArgs e)
        {
            this.lista = this.pServiciosCliente.TanquesObtenerTodos(new FiltroTanques() { Fecha = this.dtFecha.DateTime.Date });
            this.gridControl1.DataSource = this.lista;
            this.gridView1.BestFitColumns();
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
        private DialogResult MensajeConfirmacion(string msj)
        {
            return MessageBox.Show(msj, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
    }
}
