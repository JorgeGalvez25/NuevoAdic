using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.AdministrarClientesMdl.Constants;
using EstandarCliente.AdministrarClientesMdl.Properties;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VLAdministrarClientes : UserControl,
                                                 IVLAdministrarClientes,
                                                 IConfiguraMenu,
                                                 IServicioBotonCerrarTab
    {
        public string Titulo
        {
            get { return this.titulo1.TituloMenu; }
            set { this.titulo1.TituloMenu = value; }
        }

        private readonly object _lock = new object();
        private SesionModuloWeb Sesion;
        private ListaAdministrarClientes listado;
        private IServiciosMenuAplicacion servicioMenu;
        private BackgroundWorker threadAdministrarClientes;
        private Permisos _permiso;
        private Permisos Permiso
        {
            get
            {
                if (this._permiso == null)
                {
                    var lst = new ListaPermisos();
                    lst.FromXML(this.Sesion.Usuario.Variables[0]);
                    this._permiso = lst.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Modulos.CLIENTES));
                }

                return this._permiso;
            }
        }
        internal Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores> ObtenerListaDistribuidores;

        private IAsyncResult asyncResultLuActivo;
        private IAsyncResult asyncResultObtenerListaDistribuidores;

        private bool evtCerrar;
        private bool evtEliminar;
        private bool evtRegistrar;
        private bool evtModificar;
        private bool evtPropiedades;
        private bool evtRegistrarGrupo;
        private bool evtModificarGrupo;

        public VLAdministrarClientes(SesionModuloWeb sesion, VLAdministrarClientesPresenter presenter)
        {
            this.Sesion = sesion;
            this.Presenter = presenter;
            this.listado = new ListaAdministrarClientes();

            //var x = this.listado.Compare(new ListaAdministrarClientes());
            this.servicioMenu = this._presenter.WorkItem.RootWorkItem.Services.Get<IServiciosMenuAplicacion>();
            this.InitializeComponent();
            this.chkEnGrupos.Checked = true;
            this.threadAdministrarClientes = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
            this.threadAdministrarClientes.DoWork += this.threadAdministrarClientes_DoWork;
            this.threadAdministrarClientes.RunWorkerCompleted += this.threadAdministrarClientes_RunWorkerCompleted;

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
            this.BeginSafe(this.CreaMenu);
            this.BeginSafe(this.InicializaCombos);
            this.BeginSafe(this.InicializarLuDistribuidores);
            this.BeginSafe(this.InicializarCatalogo);

            if (!this.threadAdministrarClientes.IsBusy)
            {
                this.threadAdministrarClientes.RunWorkerAsync();
            }
            this.BeginSafe(this.CrearEventos);
        }

        private void CrearEventos()
        {
            this.txtNombreComercial.KeyUp += this.txtNombreComercial_KeyUp;

            this.luActivo.EditValueChanged += this.luActivo_EditValueChanged;

            this.luDistribuidor.EditValueChanged += this.luDistribuidor_EditValueChanged;
            this.chkEnGrupos.CheckedChanged += this.chkEnGrupos_CheckedChanged;
        }
        private void QuitarEventos()
        {
            this.txtNombreComercial.KeyUp -= this.txtNombreComercial_KeyUp;

            this.luActivo.EditValueChanged -= this.luActivo_EditValueChanged;
            this.luDistribuidor.EditValueChanged += this.luDistribuidor_EditValueChanged;
        }

        private void InicializaCombos()
        {
            asyncResultLuActivo = this.luActivo.BeginInvoke(new MethodInvoker(() =>
                 {
                     this.luActivo.BeginSafe(delegate
                     {
                         this.luActivo.Properties.DataSource = new[]
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
                         this.luActivo.EditValue = EstatusProgramado.Si;
                     });
                 }));
        }
        private void InicializarCatalogo()
        {
            this.grdClientes.DataSource = this.listado;//.FirstOrDefault() ?? new AdministrarClientes();//lst;
            this.grdClientes.SuspendLayout();
            this.gridView1.BeginInit();

            this.grdClientes.UseEmbeddedNavigator = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
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
            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].Width = 75;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].MinWidth = 300;
            this.gridView1.Columns[ConstantesEntidad.CONEXION].MinWidth = 10;
            this.gridView1.Columns[ConstantesEntidad.CONEXION].Width = 67;
            //this.gridView1.Columns["Membrecia"].MinWidth = 10;
            //this.gridView1.Columns["Membrecia"].Width = 67;
            //this.gridView1.Columns["FechaMembrecia"].MinWidth = 10;
            //this.gridView1.Columns["FechaMembrecia"].Width = 100;
            //this.gridView1.Columns[ConstantesEntidad.HORAS_CORTE].MinWidth = 10;
            //this.gridView1.Columns[ConstantesEntidad.HORAS_CORTE].Width = 74;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].MinWidth = 150;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].MinWidth = 10;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].Width = 67;

            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].VisibleIndex = 0;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].VisibleIndex = 1;
            this.gridView1.Columns[ConstantesEntidad.CONEXION].VisibleIndex = 2;
            //this.gridView1.Columns["Membrecia"].VisibleIndex = 2;
            //this.gridView1.Columns["FechaMembrecia"].VisibleIndex = 3;
            this.gridView1.Columns["AuxiliarGrupo"].VisibleIndex = 4;
            //this.gridView1.Columns[ConstantesEntidad.HORAS_CORTE].VisibleIndex = 5;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].VisibleIndex = 6;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].VisibleIndex = 7;

            this.gridView1.Columns[ConstantesEntidad.NO_ESTACION].Caption = ConstantesEntidad.NO_ESTACION_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.NOMBRE_COMERCIAL].Caption = ConstantesEntidad.NOMBRE_COMERCIAL_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.CONEXION].Caption = ConstantesEntidad.CONEXION_CAPTION;
            //this.gridView1.Columns["Membrecia"].Caption = "Membrecia";
            //this.gridView1.Columns["FechaMembrecia"].Caption = "Fecha Membrecia";
            this.gridView1.Columns["AuxiliarGrupo"].Caption = "Grupo";
            //this.gridView1.Columns[ConstantesEntidad.HORAS_CORTE].Caption = ConstantesEntidad.HORAS_CORTE_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.DISTRIBUIDOR].Caption = ConstantesEntidad.DISTRIBUIDOR_CAPTION;
            this.gridView1.Columns[ConstantesEntidad.ACTIVO].Caption = ConstantesEntidad.ACTIVO_CAPTION;

            this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView1.Columns[ConstantesEntidad.ACTIVO].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView1.Columns[ConstantesEntidad.CONEXION].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            //this.gridView1.Columns["Membrecia"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = true;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = false;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = false;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            //(this.gridView1.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.gridView1.Columns["AuxiliarGrupo"].Group();

            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;

            this.grdClientes.ResumeLayout();
            this.gridView1.EndInit();
            this.grdClientes.Refresh();
            this.gridView1.BestFitColumns();

            this.configurarMenuContextual();
        }
        internal void InicializarLuDistribuidores()
        {
            if (this.ObtenerListaDistribuidores == null)
            {
                this.ObtenerListaDistribuidores = new Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores>(ObtenerDistribuidores);
            }

            asyncResultObtenerListaDistribuidores = this.ObtenerListaDistribuidores.BeginInvoke(new FiltroAdministrarDistribuidores() { Activo = "Si" }, AsyncObtenerDistribuidores, null);
        }
        internal void AsyncObtenerDistribuidores(IAsyncResult result)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                this.luDistribuidor.BeginSafe(delegate
                {
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

                    ListaAdministrarDistribuidores distribuidores = this.ObtenerListaDistribuidores.EndInvoke(result);
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

        private void AplicarFiltros()
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.AppStarting; });
                    var seleccionado = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                    this.grdClientes.BeginSafe(delegate
                    {
                        this.grdClientes.DataSource = this.listado;
                        this.grdClientes.RefreshDataSource();
                        this.grdClientes.Refresh();

                        if (seleccionado != null)
                        {
                            int idx = this.listado.FindIndex(p => p.Compare(seleccionado));
                            this.gridView1.FocusedRowHandle = idx;
                        }
                    });
                }
                catch { }
                finally
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                }
            }));
        }

        private ListaAdministrarClientes ObtenerPorNombre(ListaAdministrarClientes lista)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            resultado.AddRange(lista.ObtenerPor(FiltrarCliente.NombreComercial, this.txtNombreComercial.Text));

            return resultado;
        }
        private ListaAdministrarClientes ObtenerPorActivo(ListaAdministrarClientes lista)
        {
            var activo = (EstatusProgramado)this.luActivo.EditValue;
            if (activo == EstatusProgramado.Todos) return lista;
            return lista.ObtenerPorActivo(activo.ToString());
        }
        private ListaAdministrarClientes ObtenerPorDistribuidor(ListaAdministrarClientes lista)
        {
            int value = (int)(this.luDistribuidor.EditValue ?? 0);
            if (value <= 0) return lista;
            ListaAdministrarClientes result = new ListaAdministrarClientes();
            result.AddRange(lista.Where(p => p.IdDistribuidor == value));
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
                AdministrarClientes cliente = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

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

                if (cliente.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase))
                {
                    Mensaje.MensajeError(ListadoMensajes.Error_Registro_Activo);
                    return;
                }

                if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Eliminar_02, string.Format("{0} - {1}", cliente.NoEstacion, cliente.NombreComercial)))
                {
                    this._presenter.Eliminar(new FiltroAdministrarClientes()
                        {
                            NoEstacion = cliente.NoEstacion,
                            Clave = cliente.Clave,
                            Activo = cliente.Activo
                        });

                    if (!this.threadAdministrarClientes.IsBusy)
                    {
                        this.threadAdministrarClientes.RunWorkerAsync();
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
                AdministrarClientes cliente = new AdministrarClientes();
                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente, ConstantesModulo.OPCIONES.REGISTRAR, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);
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

                if (this.gridView1.FocusedRowHandle < 0)
                {
                    string msj = "Seleccione un registro.";

                    if (this.gridView1.RowCount <= 0)
                    {
                        msj = "No existen elementos a modificar.";
                    }

                    Mensaje.MensajeInfo(msj);

                    return;
                }

                AdministrarClientes cliente = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

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

                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente.Clonar(), ConstantesModulo.OPCIONES.MODIFICAR, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);
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

                AdministrarClientes cliente = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

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

                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente, ConstantesModulo.OPCIONES.PROPIEDADES, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);
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

        private void OnRegistrarGrupo(object sender, ItemClickEventArgs e)
        {
            if (evtRegistrarGrupo) return;
            try
            {
                evtRegistrarGrupo = true;
                this._presenter.EjecutarServiciosMantenimientoPaleta(new AdministrarClientes(), ConstantesModulo.OPCIONES.REGISTRAR_GRUPO, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtRegistrarGrupo = false;
            }
        }
        private void OnModificarGrupo(object sender, ItemClickEventArgs e)
        {
            if (evtModificarGrupo) return;

            try
            {
                evtModificarGrupo = true;

                AdministrarClientes cliente = (AdministrarClientes)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);

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

                this._presenter.EjecutarServiciosMantenimientoPaleta(cliente.Clonar(), ConstantesModulo.OPCIONES.MODIFICAR_GRUPO, ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.VISTA_LISTADO);
            }
            catch (Exception ex)
            {
                Mensaje.MensajeError(ex.Message);
            }
            finally
            {
                evtModificarGrupo = false;
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

        private void tsmInsertarGrupo_Click(object sender, EventArgs e)
        {
            this.OnRegistrarGrupo(sender, null);
        }
        private void tsmModificarGrupo_Click(object sender, EventArgs e)
        {
            this.OnModificarGrupo(sender, null);
        }

        private void chkEnGrupos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkEnGrupos.Checked)
            {
                this.gridView1.Columns["AuxiliarGrupo"].Group();
                this.gridView1.Columns["AuxiliarGrupo"].VisibleIndex = 4;
            }
            else
            {
                this.gridView1.Columns["AuxiliarGrupo"].UnGroup();
                this.gridView1.Columns["AuxiliarGrupo"].Visible = false;
            }
        }

        private void luZonas_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.threadAdministrarClientes.IsBusy) { this.threadAdministrarClientes.RunWorkerAsync(); }
        }
        private void luActivo_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.threadAdministrarClientes.IsBusy) { this.threadAdministrarClientes.RunWorkerAsync(); }
        }
        private void txtNombreComercial_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.threadAdministrarClientes.IsBusy) { this.threadAdministrarClientes.RunWorkerAsync(); }
        }
        private void luDistribuidor_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.threadAdministrarClientes.IsBusy) { this.threadAdministrarClientes.RunWorkerAsync(); }
        }

        private void threadAdministrarClientes_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
            if (this.asyncResultLuActivo != null && !this.asyncResultLuActivo.IsCompleted)
            {
                this.asyncResultLuActivo.AsyncWaitHandle.WaitOne();
            }

            if (this.asyncResultObtenerListaDistribuidores != null && !this.asyncResultObtenerListaDistribuidores.IsCompleted)
            {
                this.asyncResultObtenerListaDistribuidores.AsyncWaitHandle.WaitOne();
            }

            lock (_lock)
            {
                ListaAdministrarClientes lst = this._presenter.ObtenerTodosFiltro(new FiltroAdministrarClientes()
                    {
                        Activo = (EstatusProgramado)this.luActivo.EditValue == EstatusProgramado.Todos ? string.Empty : ((EstatusProgramado)this.luActivo.EditValue).ToString(),
                        Distribuidor = (int)(this.luDistribuidor.EditValue ?? 0)
                    });

                if (((BackgroundWorker)sender).CancellationPending || !e.Cancel)
                {
                    lst = this.ObtenerPorActivo(lst);
                    lst = this.ObtenerPorDistribuidor(lst);
                    lst = this.ObtenerPorNombre(lst);
                    ImagenSoft.Extensiones.Asyncrono.Parallel.ForEach(lst, (p) =>
                        {
                            var first = lst.FirstOrDefault(q => q.NoEstacion.Equals(string.IsNullOrEmpty(p.Matriz) ? p.NoEstacion : p.Matriz));
                            if (first != null)
                            {
                                p.AuxiliarGrupo = string.Format("{0} - {1}", string.IsNullOrEmpty(first.Matriz) ? first.NoEstacion : first.Matriz
                                                                           , (string.IsNullOrEmpty(first.NombreGrupo) ? first.NombreComercial : first.NombreGrupo).ToUpper());
                            }
                            else
                            {
                                p.AuxiliarGrupo = string.Format("{0} - {1}", string.IsNullOrEmpty(p.Matriz) ? p.NoEstacion : p.Matriz
                                                                           , (string.IsNullOrEmpty(p.NombreGrupo) ? p.NombreComercial : p.NombreGrupo).ToUpper());
                            }
                        }).WaitOne();

                    e.Result = lst;
                }
                else if (lst != null)
                {
                    lst.Clear();
                    lst = null;
                }
            }
        }
        private void threadAdministrarClientes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null) { Mensaje.MensajeError(e.Error.Message); return; }

                if (this.listado == null) { this.listado = new ListaAdministrarClientes(); } else { this.listado.Clear(); }
                this.listado.AddRange((((ListaAdministrarClientes)e.Result) ?? new ListaAdministrarClientes()).OrderByDescending(x => x.Conexion)
                                                                                                              .ThenBy(x => x.NoEstacion));

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
            this.threadAdministrarClientes.RunWorkerCompleted -= this.threadAdministrarClientes_RunWorkerCompleted;

            if (this.threadAdministrarClientes.IsBusy)
            {
                this.threadAdministrarClientes.CancelAsync();
            }

            this.threadAdministrarClientes.DoWork -= this.threadAdministrarClientes_DoWork;

            this.threadAdministrarClientes.Dispose();
            this.threadAdministrarClientes = null;

            this.QuitarEventos();

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

            //this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.IMPRESION, ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, OnCerrar, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Doc_Imprimir, Shortcut.CtrlP);
            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OPCIONES_GRUPO, ConstantesModulo.OPCIONES.REGISTRAR_GRUPO, OnRegistrarGrupo, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Doc_Insertar, Shortcut.CtrlI);
            this.servicioMenu.AgregaBotonMenuCatalogoGrupo(ConstantesModulo.MENUS.OPCIONES_GRUPO, ConstantesModulo.OPCIONES.MODIFICAR_GRUPO, OnModificarGrupo, EstandarCliente.Infrastructure.Interface.Constants.ConstantesIconos.Doc_Modificar, Shortcut.CtrlJ);

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

                this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.REGISTRAR_GRUPO, true);
                this.servicioMenu.HabilitarBotonSeccionMenu(ConstantesModulo.MENUS.MENU, ConstantesModulo.MENUS.OPCIONES, ConstantesModulo.OPCIONES.MODIFICAR_GRUPO, true);
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
            if (this.grdClientes.ContextMenuStrip == null)
            {
                this.grdClientes.ContextMenuStrip = new ContextMenuStrip();
            }

            this.grdClientes.ContextMenuStrip.Items.Clear();
            bool activar = this.Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase);
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.REGISTRAR, "Ins", Imagenes.mc_add, this.tsmInsertar_Click, activar));
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.MODIFICAR, "Ctrl+M", Imagenes.mc_edit, this.tsmModificar_Click, activar));
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.ELIMINAR, "Ctrl+E", Imagenes.mc_delete, this.tsmEliminar_Click, activar));
            this.grdClientes.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.REGISTRAR_GRUPO, "Ctrl+G", Imagenes.document, this.tsmInsertarGrupo_Click, activar));
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.MODIFICAR_GRUPO, "Ctrl+D", Imagenes.document_ok, this.tsmModificarGrupo_Click, activar));
            //this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.IMPRIMIR_LISTADO, "Ctrl+P", Imagenes.mc_printer, this.tsmImprimirListado_Click));
            this.grdClientes.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.grdClientes.ContextMenuStrip.Items.Add(this.agregarItemMenu(ConstantesModulo.OPCIONES.PROPIEDADES, "F12", Imagenes.mc_propiedades, this.tsmPropiedades_Click, true));
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
                        case ConstantesModulo.OPCIONES.REGISTRAR_GRUPO:
                            aux = this.Permiso.SubPermisos.FirstOrDefault(p => p.Id == ConstantesPermisos.Operaciones.OPERACION_REGISTRAR);
                            item.Enabled = ((aux != null) ? aux.Permitido : false);
                            break;
                        case ConstantesModulo.OPCIONES.MODIFICAR:
                        case ConstantesModulo.OPCIONES.MODIFICAR_GRUPO:
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

        #region IVLAdministrarClientes Members

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

        public bool Insertar(AdministrarClientes entidad)
        {
            bool resultado = false;
            try
            {
                resultado = this._presenter.Insertar(entidad);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }

            return resultado;
        }

        public bool Eliminar(FiltroAdministrarClientes filtro)
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

        public ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro)
        {
            ListaAdministrarClientes resultado = new ListaAdministrarClientes();
            try
            {
                resultado = this._presenter.ObtenerTodosFiltro(filtro);
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

        [EventSubscription(ConstantesModulo.VISTAS.ADMINISTRAR_CLIENTES_MDL.EVENT_HANDLER, ThreadOption.UserInterface)]
        public void OnevtRazonSocialFACELEI(object sender, EventArgs eventArgs)
        {
            if (!this.threadAdministrarClientes.IsBusy)
            {
                this.threadAdministrarClientes.RunWorkerAsync();
            }
        }

        private class TypeElement<T>
        {
            public string Display { get; set; }
            public T Value { get; set; }
        }
    }
}

