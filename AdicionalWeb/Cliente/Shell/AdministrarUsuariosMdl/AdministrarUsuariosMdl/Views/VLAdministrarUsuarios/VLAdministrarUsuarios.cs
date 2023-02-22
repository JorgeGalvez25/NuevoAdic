using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.AdministrarUsuariosMdl.Constants;
using EstandarCliente.AdministrarUsuariosMdl.Properties;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VLAdministrarUsuarios : UserControl,
                                                 IVLAdministrarUsuarios,
                                                 IConfiguraMenu,
                                                 IServicioBotonCerrarTab
    {
        public string Titulo
        {
            get { return this.titulo1.TituloMenu; }
            set { this.titulo1.TituloMenu = value; }
        }

        private SesionModuloWeb Sesion;
        private ListaAdministrarUsuarios listado;
        private IServiciosMenuAplicacion servicioMenu;
        private BackgroundWorker threadAdministrarUsuarios;
        private Permisos _permiso;
        private Permisos Permiso
        {
            get
            {
                if (this._permiso == null)
                {
                    var lst = new ListaPermisos();
                    lst.FromXML(this.Sesion.Usuario.Variables[0]);
                    this._permiso = lst.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Modulos.USUARIOS));
                }

                return this._permiso;
            }
        }

        private bool evtCerrar;
        private bool evtEliminar;
        private bool evtRegistrar;
        private bool evtModificar;
        private bool evtPropiedades;

        internal Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores> ObtenerListaDistribuidores;

        public VLAdministrarUsuarios(SesionModuloWeb sesion, VLAdministrarUsuariosPresenter presenter)
        {
            this.Sesion = sesion;
            this.Presenter = presenter;
            this.listado = new ListaAdministrarUsuarios();
            this.InitializeComponent();

            this.threadAdministrarUsuarios = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
            this.threadAdministrarUsuarios.DoWork += this.threadAdministrarUsuario_DoWork;
            this.threadAdministrarUsuarios.RunWorkerCompleted += this.threadAdministrarUsuario_RunWorkerCompleted;

            this.evtCerrar = false;
            this.evtEliminar = false;
            this.evtModificar = false;
            this.evtPropiedades = false;
            this.evtRegistrar = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);

            this.BeginSafe(this.InicializaControles);
            this.BeginSafe(this.InicializarLuDistribuidores);
            this.BeginSafe(this.InicializarCatalogo);
            if (!this.threadAdministrarUsuarios.IsBusy)
            {
                this.threadAdministrarUsuarios.RunWorkerAsync();
            }
            this.BeginSafe(this.CrearEventos);
        }

        private void CrearEventos()
        {
            this.txtNombre.KeyUp += this.txtNombre_KeyUp;

            this.luActivos.EditValueChanged += this.luActivos_EditValueChanged;
            this.luDistribuidor.EditValueChanged += this.luDistribuidor_EditValueChanged;
        }

        private void InicializaControles()
        {
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Properties.MaxLength = 150; });
            this.luActivos.BeginSafe(delegate
            {
                this.luActivos.Properties.DataSource = new[]
                    {
                        new {
                            Icono = default(Bitmap),
                            Display = "Todos",
                            Value = EstatusProgramado.Todos
                        },
                        new {
                            Icono = Resources.bullet_ball_glass_green,
                            Display = "Si",
                            Value = EstatusProgramado.Si
                        },
                        new {
                            Icono = Resources.bullet_ball_glass_red,
                            Display = "No",
                            Value = EstatusProgramado.No
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

                this.luActivos.Properties.DisplayMember = "Display";
                this.luActivos.Properties.ValueMember = "Value";
                this.luActivos.Properties.ShowFooter = false;
                this.luActivos.Properties.PopupFormMinSize = new Size(this.luActivos.Size.Width, this.luActivos.Size.Width);
                this.luActivos.Properties.PopupSizeable = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowColumnMoving = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowColumnResizing = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowFilter = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowGroup = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowRowSizing = false;
                this.gridLookUpEdit1View.OptionsCustomization.AllowSort = false;
                this.gridLookUpEdit1View.OptionsView.ShowColumnHeaders = false;
                this.gridLookUpEdit1View.OptionsView.ShowIndicator = false;

                this.gridLookUpEdit1View.BestFitColumns();
                this.luActivos.EditValue = EstatusProgramado.Todos;
            });
        }
        private void InicializarCatalogo()
        {
            this.grdListado.DataSource = this.listado;
            this.grdListado.SuspendLayout();
            this.gridView1.BeginInit();

            this.grdListado.UseEmbeddedNavigator = false;
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

            //this.gridView1.Columns[ConstantesEntidad.CLAVE].MinWidth = 100;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE].MinWidth = 250;
            this.gridView1.Columns[ConstantesEntidad.PUESTO].MinWidth = 120;
            this.gridView1.Columns[ConstantesEntidad.E_MAIL].MinWidth = 100;
            this.gridView1.Columns[ConstantesEntidad.E_MAIL].Width = 200;
            this.gridView1.Columns[ConstantesEntidad.FECHA].MinWidth = 70;
            this.gridView1.Columns[ConstantesEntidad.FECHA].Width = 75;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].MinWidth = 100;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].MinWidth = 50;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].MinWidth = 67;

            //this.gridView1.Columns[ConstantesEntidad.CLAVE].VisibleIndex = 0;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE].VisibleIndex = 1;
            this.gridView1.Columns[ConstantesEntidad.PUESTO].VisibleIndex = 2;
            this.gridView1.Columns[ConstantesEntidad.E_MAIL].VisibleIndex = 3;
            this.gridView1.Columns[ConstantesEntidad.FECHA].VisibleIndex = 4;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].VisibleIndex = 5;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].VisibleIndex = 6;

            //this.gridView1.Columns[ConstantesEntidad.CLAVE].Caption = ConstantesEntidad.CLAVE_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE].Caption = ConstantesEntidad.NOMBRE_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.PUESTO].Caption = ConstantesEntidad.PUESTO_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.E_MAIL].Caption = ConstantesEntidad.E_MAIL_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.FECHA].Caption = ConstantesEntidad.FECHA_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].Caption = ConstantesEntidad.ACTIVO_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].Caption = ConstantesEntidad.DISTRIBUIDOR_CAPTION;

            //this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            //(this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatString = "{0:D6}";
            //(this.gridView1.Columns[ConstantesEntidad.CLAVE].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemTextEdit).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;

            this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.grdListado.ResumeLayout();
            this.gridView1.EndInit();
            this.grdListado.Refresh();
            this.gridView1.BestFitColumns();
            this.configurarMenuContextual();
        }
        internal void InicializarLuDistribuidores()
        {
            if (this.ObtenerListaDistribuidores == null)
            {
                this.ObtenerListaDistribuidores = new Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores>(ObtenerDistribuidores);
            }

            this.luDistribuidor.BeginSafe(delegate
            {
                LookUpColumnInfo columna = new LookUpColumnInfo();
                {
                    columna.FieldName = "Clave";
                    columna.Caption = "Clave";
                    columna.FormatString = "D3";
                    columna.FormatType = DevExpress.Utils.FormatType.Custom;
                    columna.Visible = true;
                    this.luDistribuidor.Properties.Columns.Add(columna);
                }

                columna = new LookUpColumnInfo();
                {
                    columna.FieldName = "Descripcion";
                    columna.Caption = "Descripcion";
                    this.luDistribuidor.Properties.Columns.Add(columna);
                }
                this.luDistribuidor.Properties.DisplayMember = "Descripcion";
                this.luDistribuidor.Properties.ValueMember = "Clave";
                this.luDistribuidor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                this.luDistribuidor.Properties.PopupSizeable = false;
                this.luDistribuidor.Properties.ShowFooter = false;
                this.luDistribuidor.Properties.ShowHeader = false;
            });

            this.ObtenerListaDistribuidores.BeginInvoke(new FiltroAdministrarDistribuidores() { Activo = "Si" }, AsyncObtenerDistribuidores, null);
        }
        internal void AsyncObtenerDistribuidores(IAsyncResult result)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                this.luDistribuidor.BeginSafe(delegate
                {
                    var distribuidores = this.ObtenerListaDistribuidores.EndInvoke(result);
                    distribuidores.Insert(0, new AdministrarDistribuidores()
                    {
                        Clave = 0,
                        Descripcion = "Todos"
                    });
                    this.luDistribuidor.Properties.DataSource = distribuidores;
                    this.luDistribuidor.EditValue = 0;
                    this.luDistribuidor_EditValueChanged(null, null);
                });
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        #region Filtros

        private void AplicarFiltros()
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                var seleccionado = (AdministrarUsuarios)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                this.grdListado.DataSource = this.listado;

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
        private ListaAdministrarUsuarios ObtenerPorNombre(ListaAdministrarUsuarios lista)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();
            resultado.AddRange(lista.ObtenerPorNombre(this.txtNombre.Text));

            return resultado;
        }
        private ListaAdministrarUsuarios ObtenerPorActivo(ListaAdministrarUsuarios lista)
        {
            string find = string.Empty;
            var aplicado = (EstatusProgramado)this.luActivos.EditValue;

            switch (aplicado)
            {
                case EstatusProgramado.Si:
                    find = "Si";
                    break;
                case EstatusProgramado.No:
                    find = "No";
                    break;
            }

            return lista.ObtenerPorActivo(find);
        }
        private ListaAdministrarUsuarios ObtenerOrdenNombre(ListaAdministrarUsuarios lista)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();
            resultado.AddRange(lista.OrderBy(p => p.Nombre));
            return resultado;
        }
        private ListaAdministrarUsuarios ObtenerPorDistribuidor(ListaAdministrarUsuarios lista)
        {
            int value = (int)(this.luDistribuidor.EditValue ?? 0);
            if (value <= 0) return lista;
            ListaAdministrarUsuarios result = new ListaAdministrarUsuarios();
            result.AddRange(lista.Where(p => p.IdDistribuidor == value));
            return result;
        }

        #endregion

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

                AdministrarUsuarios usuario = (AdministrarUsuarios)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                if (usuario == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos a eliminar.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                if (usuario.Clave == this.Sesion.Clave || usuario.Clave == 0)
                {
                    Mensaje.MensajeInfo(ListadoMensajes.Error_No_se_puede_Eliminar_SP);
                    return;
                }

                if (usuario.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    Mensaje.MensajeError(ListadoMensajes.Error_Registro_Activo);
                    return;
                }

                if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Eliminar_02, usuario.Nombre))
                {
                    this._presenter.Eliminar(new FiltroAdministrarUsuarios()
                        {
                            Clave = usuario.Clave,
                            Activo = usuario.Activo
                        });

                    if (!this.threadAdministrarUsuarios.IsBusy)
                    {
                        this.threadAdministrarUsuarios.RunWorkerAsync();
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
                AdministrarUsuarios cliente = new AdministrarUsuarios();
                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente, ConstantesModulo.OPCIONES.REGISTRAR, ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.VISTA_LISTADO);
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
                AdministrarUsuarios usuario = (AdministrarUsuarios)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                if (usuario == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos a modificar.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                this._presenter.EjecutarServiciosMantenimientoPaleta(usuario.Clone(), ConstantesModulo.OPCIONES.MODIFICAR, ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.VISTA_LISTADO);
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
                AdministrarUsuarios usuario = (AdministrarUsuarios)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

                if (usuario == null)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                this._presenter.EjecutarServiciosMantenimientoPaleta(usuario, ConstantesModulo.OPCIONES.PROPIEDADES, ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.VISTA_LISTADO);
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
        private void tsmEliminar_Click(object sender, EventArgs e)
        {
            this.OnEliminar(sender, null);
        }
        private void tsmModificar_Click(object sender, EventArgs e)
        {
            this.OnModificar(sender, null);
        }
        private void tsmPropiedades_Click(object sender, EventArgs e)
        {
            this.OnPropiedades(sender, null);
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.threadAdministrarUsuarios.IsBusy) { this.threadAdministrarUsuarios.RunWorkerAsync(); }
        }
        private void luActivos_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.threadAdministrarUsuarios.IsBusy) { this.threadAdministrarUsuarios.RunWorkerAsync(); }
        }
        private void luDistribuidor_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.threadAdministrarUsuarios.IsBusy) { this.threadAdministrarUsuarios.RunWorkerAsync(); }
        }

        private void threadAdministrarUsuario_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
            var lst = this._presenter.ObtenerTodos(new FiltroAdministrarUsuarios());

            if (!e.Cancel)
            {
                lst = this.ObtenerPorActivo(lst);
                lst = this.ObtenerPorNombre(lst);
                lst = this.ObtenerOrdenNombre(lst);
                lst = this.ObtenerPorDistribuidor(lst);
                e.Result = lst;
            }
            else if (lst != null)
            {
                lst.Clear();
                lst = null;
            }
        }
        private void threadAdministrarUsuario_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null) { Mensaje.MensajeError(e.Error.Message); return; }

                if (this.listado != null) { this.listado.Clear(); }
                this.listado = ((ListaAdministrarUsuarios)e.Result) ?? new ListaAdministrarUsuarios();

                this.AplicarFiltros();
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        #endregion

        #region IServicioBotonCerrarTab Members

        public void BotonCerrarClick()
        {
            this.threadAdministrarUsuarios.RunWorkerCompleted -= this.threadAdministrarUsuario_RunWorkerCompleted;

            if (this.threadAdministrarUsuarios.IsBusy)
            {
                this.threadAdministrarUsuarios.CancelAsync();
            }

            this.threadAdministrarUsuarios.DoWork -= this.threadAdministrarUsuario_DoWork;

            this.threadAdministrarUsuarios.Dispose();
            this.threadAdministrarUsuarios = null;

            if (this.listado != null)
            {
                this.listado.Clear();
                this.listado = null;
            }

            this._presenter.OnCloseView();
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

            //this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.IMPRESION,r ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, OnCerrar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Doc_Imprimir, Shortcut.CtrlP);

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
        }

        private void configurarMenuContextual()
        {
            if (this.grdListado.ContextMenuStrip == null)
            {
                this.grdListado.ContextMenuStrip = new ContextMenuStrip();
            }
            this.grdListado.ContextMenuStrip.Items.Clear();

            bool activar = this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase);

            this.grdListado.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.REGISTRAR, "Ins", Imagenes.mc_add, this.tsmInsertar_Click, activar));
            this.grdListado.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.MODIFICAR, "Ctrl+M", Imagenes.mc_edit, this.tsmModificar_Click, activar));
            this.grdListado.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.ELIMINAR, "Ctrl+E", Imagenes.mc_delete, this.tsmEliminar_Click, activar));
            //this.grdListado.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            //this.grdListado.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, "Ctrl+P", Imagenes.mc_printer, this.tsmImprimirListado_Click));
            this.grdListado.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.grdListado.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.PROPIEDADES, "F12", Imagenes.mc_propiedades, this.tsmPropiedades_Click, true));
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

        #region IVLAdministrarUsuarios Members

        public int Consecutivo()
        {
            int resultado = 0;

            try
            {
                resultado = this._presenter.Consecutivo();
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public bool Eliminar(FiltroAdministrarUsuarios filtro)
        {
            bool resultado = false;

            try
            {
                resultado = this._presenter.Eliminar(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public ListaAdministrarUsuarios ObtenerTodos(FiltroAdministrarUsuarios filtro)
        {
            ListaAdministrarUsuarios resultado = new ListaAdministrarUsuarios();

            try
            {
                resultado = this._presenter.ObtenerTodos(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            ListaAdministrarDistribuidores resultado = new ListaAdministrarDistribuidores();
            try
            {
                resultado = this._presenter.ObtenerDistribuidores(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        #endregion

        [EventSubscription(ConstantesModulo.VISTAS.ADMINISTRAR_USUARIOS_MDL.EVENT_HANDLER, ThreadOption.UserInterface)]
        public void OnEvtAdministrarUsuarios(object sender, EventArgs eventArgs)
        {
            if (!this.threadAdministrarUsuarios.IsBusy)
            {
                this.threadAdministrarUsuarios.RunWorkerAsync();
            }
        }
    }
}

