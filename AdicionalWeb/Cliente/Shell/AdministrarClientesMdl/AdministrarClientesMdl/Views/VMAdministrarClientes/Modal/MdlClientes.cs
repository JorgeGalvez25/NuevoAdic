using System;
using System.ComponentModel;
using System.Windows.Forms;
using EstandarCliente.AdministrarClientesMdl.Constants;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace EstandarCliente.AdministrarClientesMdl.Views.VMAdministrarClientes.Modal
{
    public partial class MdlClientes : Form
    {
        private SesionModuloWeb Sesion;
        private VLAdministrarClientesPresenter _presenter;
        private ListaAdministrarClientes listado;
        private BackgroundWorker threadAdministrarClientes;

        public AdministrarClientes Result;

        public MdlClientes(SesionModuloWeb sesion, VLAdministrarClientesPresenter presenter)
        {
            this.Sesion = sesion;
            this._presenter = presenter;
            this.listado = new ListaAdministrarClientes();
            InitializeComponent();

            this.threadAdministrarClientes = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            this.threadAdministrarClientes.DoWork += this.threadAdministrarClientes_DoWork;
            this.threadAdministrarClientes.RunWorkerCompleted += this.threadAdministrarClientes_RunWorkerCompleted;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.BeginSafe(this.InicializarCatalogo);

            if (!this.threadAdministrarClientes.IsBusy)
            {
                this.threadAdministrarClientes.RunWorkerAsync();
            }
            this.BeginSafe(this.CrearEventos);
        }

        private void InicializarCatalogo()
        {
            this.grdClientes.DataSource = this.listado;
            this.grdClientes.SuspendLayout();
            this.gridView1.BeginInit();

            this.grdClientes.UseEmbeddedNavigator = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowFooter = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowRowSizing = false;

            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].Visible = false;
            }

            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].MinWidth = 10;
            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].Width = 55;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].MinWidth = 600;

            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].VisibleIndex = 0;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].VisibleIndex = 1;

            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].Caption = ConstantesEntidad.NO_ESTACION_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].Caption = ConstantesEntidad.NOMBRE_COMERCIAL_CAPTION;

            this.grdClientes.ResumeLayout();
            this.gridView1.EndInit();
            this.grdClientes.Refresh();
            this.gridView1.BestFitColumns();

            if (this.grdClientes.ContextMenuStrip == null)
            {
                this.grdClientes.ContextMenuStrip = new ContextMenuStrip();
            }
            this.grdClientes.ContextMenuStrip.Items.Clear();
        }

        private void CrearEventos()
        {
            this.txtNombreComercial.KeyUp += this.txtNombreComercial_KeyUp;

            this.btnAceptar.Click += this.btnAceptar_Click;
            this.btnCancelar.Click += this.btnCancelar_Click;

            this.Disposed += this.MdlClientes_Disposed;
        }
        private void QuitarEventos()
        {
            this.txtNombreComercial.KeyUp -= this.txtNombreComercial_KeyUp;

            this.btnAceptar.Click -= this.btnAceptar_Click;
            this.btnCancelar.Click -= this.btnCancelar_Click;
            this.Disposed -= this.MdlClientes_Disposed;
        }

        private void AplicarFiltros()
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                var seleccionado = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                this.grdClientes.DataSource = this.listado;

                if (seleccionado != null)
                {
                    int idx = this.listado.FindIndex(p => p.Compare(seleccionado));
                    this.gridView1.FocusedRowHandle = idx;
                }
            }
            catch { }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }
        private ListaAdministrarClientes ObtenerPorNombre(ListaAdministrarClientes lista)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            resultado.AddRange(lista.ObtenerPor(FiltrarCliente.NombreComercial, this.txtNombreComercial.Text));

            return resultado;
        }

        private void MdlClientes_Disposed(object sender, EventArgs e)
        {
            this.Sesion = null;
            this._presenter = null;
            if (this.listado != null)
            {
                this.listado.Clear();
                this.listado = null;
            }

            if (this.threadAdministrarClientes != null)
            {
                this.threadAdministrarClientes.RunWorkerCompleted -= this.threadAdministrarClientes_RunWorkerCompleted;
                if (this.threadAdministrarClientes.IsBusy)
                {
                    this.threadAdministrarClientes.CancelAsync();
                }
                this.threadAdministrarClientes.DoWork -= this.threadAdministrarClientes_DoWork;
                this.threadAdministrarClientes.Dispose();
                this.threadAdministrarClientes = null;
            }

            if (!this.grdClientes.IsDisposed && this.grdClientes.DataSource != null)
            {
                ((ListaAdministrarClientes)this.grdClientes.DataSource).Clear();
                this.grdClientes.DataSource = null;
                this.grdClientes.Dispose();
            }

            this.QuitarEventos();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.gridView1.FocusedRowHandle < 0)
            {
                Mensaje.MensajeWarn("Seleccione un elemento antes de continuar.");
                return;
            }

            this.Result = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtNombreComercial_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.threadAdministrarClientes.IsBusy) { this.threadAdministrarClientes.RunWorkerAsync(); }
        }

        private void threadAdministrarClientes_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
            var lst = this._presenter.ObtenerTodosFiltro(new FiltroAdministrarClientes());

            if (!e.Cancel)
            {
                lst = this.ObtenerPorNombre(lst);
                e.Result = lst;
            }
            else if (lst != null)
            {
                lst.Clear();
                lst = null;
            }
        }
        private void threadAdministrarClientes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null) { Mensaje.MensajeError(e.Error.Message); return; }

                if (this.listado != null) { this.listado.Clear(); }
                this.listado = ((ListaAdministrarClientes)e.Result) ?? new ListaAdministrarClientes();

                this.AplicarFiltros();
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }
    }
}
