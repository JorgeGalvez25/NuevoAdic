using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using EstandarCliente.AdministrarDistribuidoresMdl.Constants;
using EstandarCliente.AdministrarDistribuidoresMdl.Properties;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VLAdministrarDistribuidores : UserControl,
                                                       IVLAdministrarDistribuidores,
                                                       IConfiguraMenu,
                                                       IServicioBotonCerrarTab
    {
        public string Titulo
        {
            get { return this.titulo1.TituloMenu; }
            set { this.titulo1.TituloMenu = value; }
        }
        private SesionModuloWeb Sesion;
        private Permisos _permiso;
        private Permisos Permiso
        {
            get
            {
                if (this._permiso == null)
                {
                    var lst = new ListaPermisos();
                    lst.FromXML(this.Sesion.Usuario.Variables[0]);
                    this._permiso = lst.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Modulos.DISTRIBUIDORES));
                }

                return this._permiso;
            }
        }
        private BackgroundWorker hilo;
        private IServiciosMenuAplicacion servicioMenu;
        private ListaAdministrarDistribuidores lstDistribuidores;

        private bool evtCerrar;
        private bool evtEliminar;
        private bool evtRegistrar;
        private bool evtModificar;
        private bool evtPropiedades;

        public VLAdministrarDistribuidores(SesionModuloWeb sesion, VLAdministrarDistribuidoresPresenter presenter)
        {
            this.Sesion = sesion;
            this.Presenter = presenter;

            this.servicioMenu = this._presenter.WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();
            this.InitializeComponent();
            this.hilo = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
            this.hilo.DoWork += this.hilo_DoWork;
            this.hilo.RunWorkerCompleted += this.hilo_RunWorkerCompleted;
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);

            this.BeginSafe(ConfigurarTabla);
            this.BeginSafe(ConfigurarControles);
            if (!this.hilo.IsBusy) { this.hilo.RunWorkerAsync(); }
            this.BeginSafe(CrearEventos);
        }

        private void CrearEventos()
        {
            this.txtDescripcion.KeyUp += this.txtDescripcion_KeyUp;
            this.luActivo.EditValueChanged += this.luActivo_EditValueChanged;
        }
        private void QuitarEventos()
        {
            this.txtDescripcion.KeyUp -= this.txtDescripcion_KeyUp;
            this.luActivo.EditValueChanged -= this.luActivo_EditValueChanged;
        }
        private void ConfigurarTabla()
        {
            try
            {
                this.gridControl1.DataSource = new ListaAdministrarDistribuidores();// this.lstDistribuidores;

                this.gridControl1.BeginInit();
                this.gridControl1.SuspendLayout();
                this.gridView1.BeginInit();

                this.gridControl1.UseEmbeddedNavigator = false;
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

                this.gridView1.Columns[ConstantesEntidad.CLAVE].MinWidth = 50;
                this.gridView1.Columns[ConstantesEntidad.CLAVE].Width = 50;
                this.gridView1.Columns[ConstantesEntidad.DESCRIPCION].MinWidth = 350;
                this.gridView1.Columns[ConstantesEntidad.E_MAIL].MinWidth = 250;
                this.gridView1.Columns[ConstantesEntidad.ACTIVO].MinWidth = 50;
                this.gridView1.Columns[ConstantesEntidad.ACTIVO].Width = 50;

                this.gridView1.Columns[ConstantesEntidad.CLAVE].VisibleIndex = 0;
                this.gridView1.Columns[ConstantesEntidad.DESCRIPCION].VisibleIndex = 1;
                this.gridView1.Columns[ConstantesEntidad.E_MAIL].VisibleIndex = 2;
                this.gridView1.Columns[ConstantesEntidad.ACTIVO].VisibleIndex = 3;

                this.gridView1.Columns[ConstantesEntidad.CLAVE].Caption = ConstantesEntidad.CLAVE_CAPTION;
                this.gridView1.Columns[ConstantesEntidad.DESCRIPCION].Caption = ConstantesEntidad.DESCRIPCION_CAPTION;
                this.gridView1.Columns[ConstantesEntidad.E_MAIL].Caption = ConstantesEntidad.E_MAIL_CAPTION;
                this.gridView1.Columns[ConstantesEntidad.ACTIVO].Caption = ConstantesEntidad.ACTIVO_CAPTION;

                this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                (this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatString = "{0:D3}";
                (this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;

                this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
                (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;
            }
            finally
            {
                this.gridControl1.ResumeLayout();
                this.gridView1.EndInit();
                this.gridControl1.Refresh();
                this.gridView1.BestFitColumns();
                this.gridControl1.EndInit();

                this.BeginSafe(configurarMenuContextual);
            }
        }
        private void ConfigurarControles()
        {
            this.luActivo.BeginSafe(delegate
                {
                    this.luActivo.Properties.DataSource = new[]
                        {
                            new {
                                Icono = default(Bitmap),
                                Display = "Todos",
                                Value = string.Empty
                            },
                            new {
                                Icono = Resources.bullet_ball_glass_green,
                                Display = "Si",
                                Value = "Si"
                            },
                            new {
                                Icono = Resources.bullet_ball_glass_red,
                                Display = "No",
                                Value = "No"
                            }
                        };

                    this.gridLookUpEdit1View.RefreshData();

                    for (int i = 0; i < this.gridLookUpEdit1View.Columns.Count; i++)
                    {
                        this.gridLookUpEdit1View.Columns[i].Visible = false;
                    }

                    this.gridLookUpEdit1View.Columns["Icono"].VisibleIndex = 0;
                    this.gridLookUpEdit1View.Columns["Icono"].Width = 40;
                    this.gridLookUpEdit1View.Columns["Icono"].MinWidth = 35;
                    this.gridLookUpEdit1View.Columns["Display"].VisibleIndex = 1;
                    this.gridLookUpEdit1View.Columns["Display"].Width = 60;
                    this.gridLookUpEdit1View.Columns["Display"].MinWidth = 40;

                    this.gridLookUpEdit1View.Columns["Icono"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
                    (this.gridLookUpEdit1View.Columns["Icono"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit).NullText = " ";

                    this.luActivo.Properties.DisplayMember = "Display";
                    this.luActivo.Properties.ValueMember = "Value";
                    this.luActivo.Properties.ShowFooter = false;
                    this.luActivo.Properties.PopupFormMinSize = new Size(this.luActivo.Size.Width, this.luActivo.Size.Width);
                    this.luActivo.Properties.PopupSizeable = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowColumnMoving = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowColumnResizing = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowFilter = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowGroup = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowRowSizing = false;
                    this.gridLookUpEdit1View.OptionsCustomization.AllowSort = false;
                    this.gridLookUpEdit1View.OptionsView.ShowColumnHeaders = false;
                    this.gridLookUpEdit1View.OptionsView.ShowIndicator = false;

                    this.gridLookUpEdit1View.BestFitColumns();
                    this.luActivo.EditValue = string.Empty;
                });
        }

        private void AplicarFiltros()
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                var seleccionado = (AdministrarDistribuidores)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                this.gridControl1.DataSource = this.lstDistribuidores;

                if (seleccionado != null)
                {
                    int idx = this.lstDistribuidores.FindIndex(p => p.Compare(seleccionado));
                    this.gridView1.FocusedRowHandle = idx;
                }
            }
            catch { }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }
        private ListaAdministrarDistribuidores ObtenerPorActivo(ListaAdministrarDistribuidores lst)
        {
            var activo = this.luActivo.EditValue.ToString();
            if (activo.Equals("Todos")) { return lst; }
            ListaAdministrarDistribuidores result = new ListaAdministrarDistribuidores();
            result.AddRange(lst.ObtenerPorActivo(activo));
            return result;
        }
        private ListaAdministrarDistribuidores ObtenerPorNombre(ListaAdministrarDistribuidores lst)
        {
            var nombre = this.txtDescripcion.Text;
            ListaAdministrarDistribuidores result = new ListaAdministrarDistribuidores();
            result.AddRange(lst.ObtenerPorNombre(nombre));
            return result;
        }

        #region Eventos

        private void OnCerrar(object sender, ItemClickEventArgs e)
        {
            if (evtCerrar) return;
            try
            {
                evtCerrar = true;
                this.BotonCerrarClick();
            }
            finally
            {
                evtCerrar = false;
            }
        }
        private void OnEliminar(object sender, ItemClickEventArgs e)
        {
            if (evtEliminar) return;
            try
            {
                evtEliminar = true;
                AdministrarDistribuidores cliente = (AdministrarDistribuidores)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                if (cliente == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos a eliminar.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                if (cliente.Clave == 1)
                {
                    Mensaje.MensajeWarn("No es posible eliminar al distribuidor Matriz");
                    return;
                }

                if (cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    Mensaje.MensajeError(ListadoMensajes.Error_Registro_Activo);
                    return;
                }

                if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Eliminar_02, string.Format("{0:D3} - {1}", cliente.Clave, cliente.Descripcion)))
                {
                    this._presenter.Eliminar(new FiltroAdministrarDistribuidores()
                    {
                        Clave = cliente.Clave,
                        Activo = cliente.Activo
                    });

                    if (!this.hilo.IsBusy)
                    {
                        this.hilo.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtEliminar = false;
            }
        }
        private void OnRegistrar(object sender, ItemClickEventArgs e)
        {
            if (evtRegistrar) return;
            try
            {
                evtRegistrar = true;
                AdministrarDistribuidores cliente = new AdministrarDistribuidores();
                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente, ConstantesModulo.OPCIONES.REGISTRAR, ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.VISTA_LISTADO);
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtRegistrar = false;
            }
        }
        private void OnModificar(object sender, ItemClickEventArgs e)
        {
            if (evtModificar) return;

            try
            {
                evtModificar = true;

                AdministrarDistribuidores cliente = (AdministrarDistribuidores)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                if (cliente == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos a modificar.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente.Clonar(), ConstantesModulo.OPCIONES.MODIFICAR, ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.VISTA_LISTADO);
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtModificar = false;
            }
        }
        private void OnPropiedades(object sender, ItemClickEventArgs e)
        {
            if (evtPropiedades) return;
            try
            {
                evtPropiedades = true;

                AdministrarDistribuidores cliente = (AdministrarDistribuidores)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                if (cliente == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente, ConstantesModulo.OPCIONES.PROPIEDADES, ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.VISTA_LISTADO);
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtPropiedades = false;
            }
        }

        private void tsmInsertar_Click(object sender, EventArgs e)
        {
            this.OnRegistrar(sender, null);
        }
        private void tsmModificar_Click(object sender, EventArgs e)
        {
            this.OnModificar(sender, null);
        }
        private void tsmEliminar_Click(object sender, EventArgs e)
        {
            this.OnEliminar(sender, null);
        }
        private void tsmPropiedades_Click(object sender, EventArgs e)
        {
            this.OnPropiedades(sender, null);
        }
        private void tsmImprimirListado_Click(object sender, EventArgs e)
        {

        }

        private void hilo_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
            var lst = this.ObtenerTodosFiltro(new FiltroAdministrarDistribuidores());

            if (!e.Cancel)
            {
                lst = this.ObtenerPorActivo(lst);
                lst = this.ObtenerPorNombre(lst);
                e.Result = lst;
            }
            else if (lst != null)
            {
                lst.Clear();
                lst = null;
            }
        }
        private void hilo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null) { Mensaje.MensajeError(e.Error.Message); return; }

                if (this.lstDistribuidores != null) { this.lstDistribuidores.Clear(); }
                this.lstDistribuidores = ((ListaAdministrarDistribuidores)e.Result) ?? new ListaAdministrarDistribuidores();

                this.AplicarFiltros();
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        private void luActivo_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.hilo.IsBusy) { this.hilo.RunWorkerAsync(); }
        }
        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.hilo.IsBusy) { this.hilo.RunWorkerAsync(); }
        }

        #endregion

        #region IServicioBotonCerrarTab Members

        public void BotonCerrarClick()
        {
            if (this.hilo != null)
            {
                this.hilo.RunWorkerCompleted -= this.hilo_RunWorkerCompleted;

                if (this.hilo.IsBusy)
                {
                    this.hilo.CancelAsync();
                }

                this.hilo.DoWork -= this.hilo_DoWork;
                this.hilo.Dispose();
                this.hilo = null;
            }

            this.QuitarEventos();

            this._presenter.OnCloseView();
        }

        #endregion

        #region IVLAdministrarDistribuidores Members

        public bool Eliminar(FiltroAdministrarDistribuidores filtro)
        {
            bool result = false;
            try
            {
                result = this._presenter.Eliminar(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return result;
        }

        public ListaAdministrarDistribuidores ObtenerTodosFiltro(FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores result = new ListaAdministrarDistribuidores();
            try
            {
                result = this._presenter.ObtenerTodosFiltro(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return result;
        }

        #endregion

        #region IConfiguraMenu Members

        public void CreaMenu()
        {
            if (this.servicioMenu == null)
            {
                this.servicioMenu = this._presenter.WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();
            }

            this.servicioMenu.LimpiaMenuCatalogo();

            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.REGISTRAR, OnRegistrar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Tab_Insertar, Shortcut.Ins);
            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.MODIFICAR, OnModificar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.General_Modificar, Shortcut.CtrlM);
            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.ELIMINAR, OnEliminar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.General_Eliminar, Shortcut.CtrlE);

            //this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.IMPRESION, ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, OnCerrar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Doc_Imprimir, Shortcut.CtrlP);

            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OTROS, ConstantesModulo.OPCIONES.PROPIEDADES, OnPropiedades, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.General_Propiedades, Shortcut.F12);
            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OTROS, ConstantesModulo.OPCIONES.CERRAR, OnCerrar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.General_Cerrar, Shortcut.CtrlF4);

            this.habilitarBotonesMenu(true);
        }
        private void habilitarBotonesMenu(bool habilitar)
        {
            if (servicioMenu == null) return;

            if (this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase))
            {
                this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.REGISTRAR, true);
                this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.MODIFICAR, true);
                this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.ELIMINAR, true);
            }
            else
            {
                if (this.Permiso != null)
                {
                    var aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_REGISTRAR);
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.REGISTRAR, ((aux != null) ? aux.Permitido : false));

                    aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_MODIFICAR);
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.MODIFICAR, ((aux != null) ? aux.Permitido : false));

                    aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_ELIMINAR);
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.ELIMINAR, ((aux != null) ? aux.Permitido : false));
                }
                else
                {
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.REGISTRAR, false);
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.MODIFICAR, false);
                    this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.ELIMINAR, false);
                }
            }

            //this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.IMPRESION, ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, habilitar);

            this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OTROS, ConstantesModulo.OPCIONES.PROPIEDADES, habilitar);
            this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OTROS, ConstantesModulo.OPCIONES.CERRAR, true);

            //this.grdClientes.ContextMenuStrip.Items[ConstantesModulo.OPCIONES.REGISTRAR].Enabled = registrar;
            //this.grdClientes.ContextMenuStrip.Items[ConstantesModulo.OPCIONES.MODIFICAR].Enabled = habilitar;
            //this.grdClientes.ContextMenuStrip.Items[ConstantesModulo.OPCIONES.ELIMINAR].Enabled = habilitar;
            //this.grdClientes.ContextMenuStrip.Items[ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO].Enabled = habilitar;
            //this.grdClientes.ContextMenuStrip.Items[ConstantesModulo.OPCIONES.PROPIEDADES].Enabled = habilitar;
        }

        private void configurarMenuContextual()
        {
            if (this.gridControl1.ContextMenuStrip == null)
            {
                this.gridControl1.ContextMenuStrip = new ContextMenuStrip();
            }
            this.gridControl1.ContextMenuStrip.Items.Clear();
            bool activar = this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase);
            this.gridControl1.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.REGISTRAR, "Ins", Imagenes.mc_add, this.tsmInsertar_Click, activar));
            this.gridControl1.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.MODIFICAR, "Ctrl+M", Imagenes.mc_edit, this.tsmModificar_Click, activar));
            this.gridControl1.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.ELIMINAR, "Ctrl+E", Imagenes.mc_delete, this.tsmEliminar_Click, activar));
            //this.gridControl1.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            //this.gridControl1.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, "Ctrl+P", Imagenes.mc_printer, this.tsmImprimirListado_Click));
            this.gridControl1.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.gridControl1.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.PROPIEDADES, "F12", Imagenes.mc_propiedades, this.tsmPropiedades_Click, true));
        }
        private ToolStripMenuItem agregarItemMenu(string nombre, string shortcut, Image imagen, EventHandler evento, bool activar)
        {
            ToolStripMenuItem item = new ToolStripMenuItem()
            {
                Text = nombre,
                Name = nombre,
                ShortcutKeyDisplayString = shortcut,
                Image = imagen
            };
            item.Click += new EventHandler(evento);

            if (this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase))
            {
                item.Enabled = true;
            }
            else
            {
                if (this.Permiso != null)
                {
                    Permisos aux = null;
                    switch (item.Name)
                    {
                        case ConstantesModulo.OPCIONES.REGISTRAR:
                            aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_REGISTRAR);
                            item.Enabled = ((aux != null) ? aux.Permitido : false);
                            break;
                        case ConstantesModulo.OPCIONES.MODIFICAR:
                            aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_MODIFICAR);
                            item.Enabled = ((aux != null) ? aux.Permitido : false);
                            break;
                        case ConstantesModulo.OPCIONES.ELIMINAR:
                            aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_ELIMINAR);
                            item.Enabled = ((aux != null) ? aux.Permitido : false);
                            break;
                        default:
                            item.Enabled = activar;
                            break;
                    }
                }
                else
                {
                    switch (item.Name)
                    {
                        case ConstantesModulo.OPCIONES.REGISTRAR:
                        case ConstantesModulo.OPCIONES.MODIFICAR:
                        case ConstantesModulo.OPCIONES.ELIMINAR:
                            item.Enabled = false;
                            break;
                        default:
                            item.Enabled = activar;
                            break;
                    }
                }
            }

            return item;
        }

        #endregion

        [EventSubscription(ConstantesModulo.VISTAS.ADMINISTRAR_DISTRIBUIDORES_MDL.EVENT_HANDLER, ThreadOption.UserInterface)]
        public void OnEvtAdministrarDistribuidores(object sender, EventArgs eventArgs)
        {
            if (!this.hilo.IsBusy)
            {
                this.hilo.RunWorkerAsync();
            }
        }
    }
}

