using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using EstandarCliente.AdministrarClientesMdl.Properties;
using EstandarCliente.AdministrarClientesMdl.Views.VMAdministrarClientes.Modal;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.Infrastructure.Interface.Constants;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web;
using EstandarCliente.CargadorVistas.Properties;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarGrupos : UserControl,
                                               IVMAdministrarGrupos,
                                               IConfiguraMenu,
                                               IServicioBotonCerrarTab
    {
        public string Titulo
        {
            get
            {
                return this.titulo1.TituloMenu;
            }
            set
            {
                this.titulo1.TituloMenu = value;
            }
        }
        public string Invoker
        {
            get;
            set;
        }

        private SesionModuloWeb Sesion;

        private ModoModulo Modo;
        private BackgroundWorker thread;
        private AdministrarClientes matriz;
        private AdministrarClientes AuxMatriz;

        private Regex rgNoEstacion = new Regex(@"E[0-9]{5}", RegexOptions.Compiled);
        private Regex rgEmail = new Regex(@"([a-zA-Z0-9_\-\.]{1,40})@([a-zA-Z0-9_\-\.]{1,34})\.([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.Compiled);
        private Regex rgIPPort = new Regex(@"(?=^.{1,254}$)(^(?:(?!\d+\.)[a-zA-Z0-9_\-]{1,63}\.?)+(?:[a-zA-Z]{2,})(:\d{1,5})?$)|((\d{1,3}.\d{0,3}.\d{1,3}.\d{1,3})(:\d{1,5})?)", RegexOptions.Compiled);

        //private bool evtCerrar;
        private bool evtGuardar;
        private bool evtCancelar;
        private bool evtGuardarCerrar;

        private IServiciosMenuAplicacion servicioMenu;

        public VMAdministrarGrupos(AdministrarClientes cliente, SesionModuloWeb sesion, VMAdministrarGruposPresenter presenter, string modo)
        {
            this.InitializeComponent();
            this._presenter = presenter;
            this.matriz = cliente;
            this.AuxMatriz = cliente.Clonar();

            this.Sesion = sesion;

            this.thread = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

            this.thread.DoWork += this.thread_DoWork;
            this.thread.RunWorkerCompleted += thread_RunWorkerCompleted;

            this.txtEstacion.Properties.ReadOnly = true;

            switch (modo)
            {
                case ConstantesModulo.OPCIONES.REGISTRAR:
                    this.Modo = ModoModulo.Registrar;
                    break;
                case ConstantesModulo.OPCIONES.MODIFICAR:
                    this.Modo = ModoModulo.Modificar;
                    break;
                case ConstantesModulo.OPCIONES.ELIMINAR:
                    this.Modo = ModoModulo.Eliminar;
                    break;

                case ConstantesModulo.OPCIONES.REGISTRAR_GRUPO:
                    this.Modo = ModoModulo.RegistrarGrupo;
                    break;
                case ConstantesModulo.OPCIONES.MODIFICAR_GRUPO:
                    this.Modo = ModoModulo.ModificarGrupo;
                    break;

                case ConstantesModulo.OPCIONES.PROPIEDADES:
                default:
                    this.Modo = ModoModulo.Propiedades;
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);
            this.thread.RunWorkerAsync();
            this.btnEliminarRegistro.Enabled = false;

            this.txtNoEstacionMatriz.BeginSafe(delegate { this.txtNoEstacionMatriz.Text = string.IsNullOrEmpty(this.matriz.Matriz) ? this.matriz.NoEstacion : this.matriz.Matriz; });
            this.txtNombreGrupo.BeginSafe(delegate { this.txtNombreGrupo.Text = string.IsNullOrEmpty(this.matriz.NombreGrupo) ? this.matriz.NombreComercial : this.matriz.NombreGrupo; });

            this.BeginSafe(this.Inicializar);
            this.BeginSafe(this.inicilizarCatalogo);
            this.BeginSafe(this.crearEventos);

            this.txtNoEstacionMatriz.Focus();
            this.txtNoEstacionMatriz.SelectAll();
        }

        private void Inicializar()
        {
            this.txtNoEstacionMatriz.BeginSafe(delegate
            {
                this.txtNoEstacionMatriz.Properties.Mask.EditMask = this.rgNoEstacion.ToString();
                this.txtNoEstacionMatriz.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                this.txtNoEstacionMatriz.Properties.Mask.ShowPlaceHolders = false;
                this.txtNoEstacionMatriz.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtNoEstacionMatriz.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            });
        }

        private void crearEventos()
        {
            this.txtNoEstacionMatriz.Validating += this.txtNoEstacionMatriz_Validating;

            this.btnUsuarios.Click += this.btnUsuarios_Click;
            this.btnAgregarRegistro.Click += this.btnAgregarRegistro_Click;
            this.btnEliminarRegistro.Click += this.btnEliminarRegistro_Click;

            this.txtEstacion.ButtonClick += this.txtEstacion_ButtonClick;

            this.gridView2.BeforeLeaveRow += this.gridView2_BeforeLeaveRow;
            this.gridView2.FocusedRowChanged += this.gridView2_FocusedRowChanged;
        }
        private void quitarEventos()
        {
            this.btnUsuarios.Click -= this.btnUsuarios_Click;
            this.btnAgregarRegistro.Click -= this.btnAgregarRegistro_Click;
            this.btnEliminarRegistro.Click -= this.btnEliminarRegistro_Click;

            this.txtEstacion.ButtonClick -= this.txtEstacion_ButtonClick;

            this.gridView2.BeforeLeaveRow -= this.gridView2_BeforeLeaveRow;
            this.gridView2.FocusedRowChanged -= this.gridView2_FocusedRowChanged;
        }
        private void inicilizarCatalogo()
        {
            this.gridControl1.DataSource = new ListaAdministrarClientes();
            this.gridControl1.SuspendLayout();
            this.gridView2.BeginInit();

            this.gridControl1.UseEmbeddedNavigator = false;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsCustomization.AllowGroup = false;
            this.gridView2.OptionsMenu.EnableColumnMenu = false;
            this.gridView2.OptionsMenu.EnableFooterMenu = false;
            this.gridView2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView2.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowFooter = false;
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.ShowDetailButtons = false;
            this.gridView2.OptionsCustomization.AllowColumnMoving = false;
            this.gridView2.OptionsCustomization.AllowColumnResizing = false;
            this.gridView2.OptionsCustomization.AllowRowSizing = false;

            for (int i = 0; i < this.gridView2.Columns.Count; i++)
            {
                this.gridView2.Columns[i].Visible = false;
            }

            this.gridView2.Columns["NoEstacion"].VisibleIndex = 0;
            this.gridView2.Columns["NombreComercial"].VisibleIndex = 1;
            //this.gridView2.Columns["Membrecia"].VisibleIndex = 2;
            //this.gridView2.Columns["FechaMembrecia"].VisibleIndex = 3;
            this.gridView2.Columns["Activo"].VisibleIndex = 4;
            this.gridView2.Columns["EsMatriz"].VisibleIndex = 5;

            this.gridView2.Columns["NoEstacion"].MinWidth = 10;
            this.gridView2.Columns["NoEstacion"].Width = 55;
            this.gridView2.Columns["NombreComercial"].MinWidth = 300;
            this.gridView2.Columns["Membrecia"].MinWidth = 10;
            this.gridView2.Columns["Membrecia"].Width = 67;
            this.gridView2.Columns["FechaMembrecia"].MinWidth = 10;
            //this.gridView2.Columns["FechaMembrecia"].Width = 100;
            //this.gridView2.Columns["Activo"].MinWidth = 10;
            this.gridView2.Columns["Activo"].Width = 60;
            this.gridView2.Columns["EsMatriz"].MinWidth = 10;
            this.gridView2.Columns["EsMatriz"].Width = 60;

            this.gridView2.Columns["NoEstacion"].Caption = "Estación";
            this.gridView2.Columns["NombreComercial"].Caption = "Nombre Comercial";
            //this.gridView2.Columns["Membrecia"].Caption = "Membrecia";
            //this.gridView2.Columns["FechaMembrecia"].Caption = "Fecha Membrecia";
            this.gridView2.Columns["Activo"].Caption = "Activo";
            this.gridView2.Columns["EsMatriz"].Caption = "Es Matriz";

            this.gridView2.Columns["Activo"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = "Si";
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = "No";
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = string.Empty;
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView2.Columns["Activo"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.gridView2.Columns["EsMatriz"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = true;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = false;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = false;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_red;
            (this.gridView2.Columns["EsMatriz"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            //this.gridView2.Columns["Membrecia"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueChecked = (bool?)true;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueUnchecked = (bool?)false;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).ValueGrayed = (bool?)null;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureChecked = Resources.bullet_ball_glass_green;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureGrayed = Resources.bullet_ball_glass_grey;
            //(this.gridView2.Columns["Membrecia"].ColumnEdit as DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit).PictureUnchecked = Resources.bullet_ball_glass_red;

            this.gridView2.OptionsBehavior.AutoExpandAllGroups = true;

            this.gridControl1.ResumeLayout();
            this.gridView2.EndInit();
            this.gridControl1.Refresh();
            this.gridView2.BestFitColumns();

            this.gridView2.Columns["NoEstacion"].BestFit();
            //this.gridView2.Columns["Membrecia"].BestFit();
            //this.gridView2.Columns["FechaMembrecia"].BestFit();
            this.gridView2.Columns["Activo"].BestFit();
            this.gridView2.Columns["EsMatriz"].BestFit();
        }

        private void OnGuardarModificar(bool cerrar)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                string msj = string.Empty;

                if (!ValidarControles(ref msj))
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    Mensaje.MensajeError(msj);
                    return;
                }

                bool isValid = true;

                int aux = this.gridView2.FocusedRowHandle;
                this.gridView2.FocusedRowHandle = -1;
                Application.DoEvents();
                this.gridView2.FocusedRowHandle = aux;

                (this.gridControl1.DataSource as ListaAdministrarClientes).ForEach(p =>
                {
                    if (this.StringIsNullOrEmpty(p.NombreGrupo))
                    {
                        p.NombreGrupo = this.txtNombreGrupo.Text;
                    }

                    if (!this.ValidarControlesRegistrar(p, ref msj))
                    {
                        isValid = false;
                    }
                });

                if (!isValid)
                {
                    Mensaje.MensajeError(msj);
                    return;
                }

                StringBuilder sb = new StringBuilder("Ocurrio un error en los siguientes elementos:").AppendLine();
                (this.gridControl1.DataSource as ListaAdministrarClientes).ForEach(p =>
                    {
                        if (!this._presenter.Modificar(p))
                        {
                            sb.AppendFormat("- {0}", p.NoEstacion).AppendLine();
                            isValid = false;
                        }
                    });

                if (!isValid)
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    sb.AppendLine("¿Desea continuar?");
                    if (Mensaje.MensajeConf(sb.ToString().Trim()))
                    {
                        if (cerrar)
                        {
                            this.quitarEventos();
                            this.BotonCerrarClick();
                        }
                    }
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                }
                else if (cerrar)
                {
                    this.quitarEventos();
                    this.BotonCerrarClick();
                }

                this.btnEliminarRegistro.Enabled = false;
            }
            catch (Exception e)
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                Mensaje.MensajeError(e.Message);
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                this._presenter.DisparaEvento();
            }
        }
        private void OnGuardarRegistrar(bool cerrar)
        {
            try
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                string msj = string.Empty;

                if (!ValidarControles(ref msj))
                {
                    Mensaje.MensajeError(msj);
                    return;
                }

                bool isValid = true;

                int aux = this.gridView2.FocusedRowHandle;
                this.gridView2.FocusedRowHandle = -1;
                Application.DoEvents();
                this.gridView2.FocusedRowHandle = aux;
                ListaAdministrarClientes dataSource = (this.gridControl1.DataSource as ListaAdministrarClientes);
                string noGrupo = this.txtNombreGrupo.Text;
                dataSource.ForEach(p =>
                    {
                        if (isValid)
                        {
                            // Si no tiene nombre colocale el nombre del grupo
                            if (this.StringIsNullOrEmpty(p.NombreGrupo))
                            {
                                p.NombreGrupo = noGrupo;
                            }

                            if (!this.ValidarControlesRegistrar(p, ref msj))
                            {
                                isValid = false;
                            }
                        }
                    });

                if (!isValid)
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    Mensaje.MensajeError(msj);
                    return;
                }

                StringBuilder sb = new StringBuilder("Ocurrio un error en los siguientes elementos:").AppendLine();
                AdministrarClientes item = null;
                FiltroAdministrarClientes f = new FiltroAdministrarClientes();
                dataSource.ForEach(p =>
                    {
                        f.NoEstacion = p.NoEstacion;
                        item = this._presenter.Obtener(f);

                        if (item == null)
                        {
                            if (!this._presenter.Insertar(p))
                            {
                                sb.AppendFormat("- {0}", p.NoEstacion).AppendLine();
                                isValid = false;
                            }
                        }
                        else
                        {
                            if (!this._presenter.Modificar(p))
                            {
                                sb.AppendFormat("- {0}", p.NoEstacion).AppendLine();
                                isValid = false;
                            }
                        }
                    });

                if (!isValid)
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    sb.AppendLine("¿Desea continuar?");
                    if (Mensaje.MensajeConf(sb.ToString().Trim()))
                    {
                        if (cerrar)
                        {
                            this.quitarEventos();
                            this.BotonCerrarClick();
                        }
                    }
                    this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
                }
                else if (cerrar)
                {
                    this.quitarEventos();
                    this.BotonCerrarClick();
                }

                this.btnEliminarRegistro.Enabled = false;
            }
            catch (Exception e)
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                Mensaje.MensajeError(e.Message);
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                this._presenter.DisparaEvento();
            }
        }

        private bool ValidarControles(ref string msj)
        {
            if (!this.ValidarCtrlHost(ref msj, false))
            {
                return false;
            }
            else if (!this.ValidarCtrlEMail(ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarCtrlContacto(ref msj, true))
            {
                return false;
            }

            return true;
        }

        private bool ValidarCtrlHost(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtHost.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Host y puerto");
                    this.txtHost.Focus();
                    this.txtHost.SelectAll();
                    return false;
                }
            }

            if (!this.StringIsNullOrEmpty(this.txtHost.Text) && !this.rgIPPort.IsMatch(this.txtHost.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "Host y puerto");
                this.txtHost.Focus();
                this.txtHost.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarCtrlEMail(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtEMail.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "E-Mail");
                    this.txtEMail.Focus();
                    this.txtEMail.SelectAll();
                    return false;
                }
            }

            if (!this.StringIsNullOrEmpty(this.txtEMail.Text) && !this.rgEmail.IsMatch(this.txtEMail.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "E-Mail");
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarCtrlContacto(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtContacto.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Contacto");
                    this.txtContacto.Focus();
                    this.txtContacto.SelectAll();
                    return false;
                }
            }
            return true;
        }
        private bool ValidarNoEstacionMatriz(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtNoEstacionMatriz.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "No. Estación");
                    this.txtNoEstacionMatriz.Focus();
                    this.txtNoEstacionMatriz.SelectAll();
                    return false;
                }

            }
            if (!this.StringIsNullOrEmpty(this.txtNoEstacionMatriz.Text) && !this.rgNoEstacion.IsMatch(this.txtNoEstacionMatriz.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "No. Estación. Formato (E#####)");
                this.txtNoEstacionMatriz.Focus();
                this.txtNoEstacionMatriz.SelectAll();
                return false;
            }

            return true;
        }

        private bool ValidarControlesRegistrar(AdministrarClientes p, ref string msj)
        {
            if (!this.ValidarNombreGrupo(p, ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarHost(p, ref msj, false))
            {
                return false;
            }
            else if (!this.ValidarEMail(p, ref msj, true))
            {
                return false;
            }
            else if (!this.ValidarContacto(p, ref msj, true))
            {
                return false;
            }

            return true;
        }
        private bool ValidarHost(AdministrarClientes p, ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(p.Host))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Host y puerto");
                    this.txtHost.Focus();
                    this.txtHost.SelectAll();
                    return false;
                }
            }

            if (!this.StringIsNullOrEmpty(p.Host) && !this.rgIPPort.IsMatch(p.Host.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "Host y puerto");
                this.txtHost.Focus();
                this.txtHost.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarEMail(AdministrarClientes p, ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(p.EMail))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "E-Mail");
                    this.txtEMail.Focus();
                    this.txtEMail.SelectAll();
                    return false;
                }
            }

            if (!this.StringIsNullOrEmpty(p.EMail) && !this.rgEmail.IsMatch(p.EMail))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "E-Mail");
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarContacto(AdministrarClientes p, ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(p.Contacto))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Contacto");
                    this.txtContacto.Focus();
                    this.txtContacto.SelectAll();
                    return false;
                }
            }
            return true;
        }
        private bool ValidarNombreGrupo(AdministrarClientes p, ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(p.NombreGrupo))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Nombre del grupo");
                    this.txtNombreGrupo.Focus();
                    this.txtNombreGrupo.SelectAll();
                    return false;
                }
            }

            return true;
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AdministrarClientes cliente = (AdministrarClientes)this.gridView2.GetRow(this.gridView2.FocusedRowHandle);

            if (cliente == null)
            {
                Mensaje.MensajeError("Seleccione una estacion, o agregue una al grupo.");
                return;
            }

            ListaAdministrarUsuariosClientes lista = this._presenter.UsuariosClienteObtenerTodos(new ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes()
                 {
                     NoEstacion = cliente.NoEstacion
                 });

            ListaAdministrarUsuariosClientes lstNueva = new ListaAdministrarUsuariosClientes();
            lstNueva.AddRange(lista.Where(p => p != null && p.NoEstacion.IEquals(cliente.NoEstacion)));

            using (MdlUsuarios usuarios = new MdlUsuarios(cliente, lstNueva, this._presenter))
            {
                usuarios.Text = string.Format(usuarios.Text, cliente.NoEstacion);
                usuarios.ShowDialog(this);
            }
        }
        private void btnAgregarRegistro_Click(object sender, EventArgs e)
        {
            if (rgNoEstacion.IsMatch(this.txtEstacion.Text.Trim()))
            {
                var item = this._presenter.Obtener(new FiltroAdministrarClientes()
                {
                    NoEstacion = this.txtEstacion.Text
                });

                if (item != null)
                {
                    this.txtEstacion.Text = string.Empty;
                    this.txtNombreEstacion.Text = string.Empty;
                    item.Matriz = this.matriz.NoEstacion;
                    item.NombreGrupo = string.IsNullOrEmpty(this.matriz.NombreGrupo) ? this.matriz.NombreComercial : this.matriz.NombreGrupo;
                    (this.gridControl1.DataSource as ListaAdministrarClientes).Add(item);
                    this.gridView2.RefreshData();
                    this.gridControl1.Refresh();
                    this.btnEliminarRegistro.Enabled = true;
                }
            }
        }
        private void btnEliminarRegistro_Click(object sender, EventArgs e)
        {
            AdministrarClientes cliente = (AdministrarClientes)this.gridView2.GetRow(this.gridView2.FocusedRowHandle);

            if (cliente != null)
            {
                (this.gridControl1.DataSource as ListaAdministrarClientes).RemoveAll(p => p.NoEstacion.Equals(cliente.NoEstacion, StringComparison.OrdinalIgnoreCase));
                this.gridView2.RefreshData();
                this.gridControl1.Refresh();
            }
        }

        private void txtNoEstacionMatriz_Validating(object sender, CancelEventArgs e)
        {
            string msj = string.Empty;
            e.Cancel = !this.ValidarNoEstacionMatriz(ref msj, false);

            if (e.Cancel)
            {
                this.txtNoEstacionMatriz.ErrorText = msj;
                Mensaje.MensajeError(msj);
                return;
            }
        }

        public void txtEstacion_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (MdlClientes modal = new MdlClientes(this.Sesion, new VLAdministrarClientesPresenter() { WorkItem = this._presenter.WorkItem }))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    this.txtEstacion.Text = modal.Result.NoEstacion;
                    this.txtNombreEstacion.Text = modal.Result.NombreComercial;
                }

                this.txtNombreEstacion.Focus();
            }
        }

        private void gridView2_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            AdministrarClientes cliente = (AdministrarClientes)this.gridView2.GetRow(e.RowHandle);
            if (cliente != null)
            {
                string[] aux = this.txtHost.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (aux.Length == 1)
                {
                    cliente.Host = aux[0];
                }
                else if (aux.Length > 1)
                {
                    cliente.Host = aux[0];
                    int puerto = 0;
                    if (!int.TryParse(aux[1], out puerto))
                    {
                        puerto = 808;
                    }
                    cliente.Puerto = puerto;
                }
                cliente.EMail = this.txtEMail.Text;
                cliente.Contacto = this.txtContacto.Text;
            }
        }
        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.gridView2.FocusedRowHandle >= 0)
            {
                AdministrarClientes cliente = (AdministrarClientes)this.gridView2.GetRow(this.gridView2.FocusedRowHandle);
                if (cliente != null)
                {
                    this.txtHost.BeginSafe(delegate { this.txtHost.Text = string.Format("{0}:{1}", cliente.Host, cliente.Puerto); });
                    this.txtEMail.BeginSafe(delegate { this.txtEMail.Text = cliente.EMail; });
                    this.txtContacto.BeginSafe(delegate { this.txtContacto.Text = cliente.Contacto; });
                }
            }
        }

        private void OnCerrar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Mensaje.MensajeConf("¿Desea cerrar el registro?"))
            {
                this.quitarEventos();
                this.BotonCerrarClick();
            }
        }
        private void OnGuardar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (evtGuardar) return;

            try
            {
                evtGuardar = true;

                switch (this.Modo)
                {
                    case ModoModulo.RegistrarGrupo:
                        this.OnGuardarRegistrar(false);
                        break;
                    case ModoModulo.ModificarGrupo:
                        this.OnGuardarModificar(false);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                evtGuardar = false;
            }
        }
        private void OnEliminar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        private void OnCancelar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (evtCancelar) return;
            try
            {
                evtCancelar = true;
                if (Mensaje.MensajeConf("¿Desea cancelar el registro actual?, perdera todos los avances"))
                {
                    this.BeginSafe(delegate
                    {
                        this.txtContacto.Text = string.Empty;
                        this.txtEMail.Text = string.Empty;
                        this.txtEstacion.Text = string.Empty;
                        this.txtHost.Text = string.Empty;
                        this.txtNoEstacionMatriz.Text = string.Empty;
                        this.txtNombreGrupo.Text = string.Empty;
                        (this.gridView2.DataSource as ListaAdministrarClientes).Clear();
                        this.gridView2.RefreshData();
                        this.gridControl1.Refresh();
                    });
                }
            }
            finally
            {
                evtCancelar = false;
            }
        }
        private void OnGuardarCerrar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (evtGuardarCerrar) return;

            try
            {
                evtGuardarCerrar = true;

                switch (this.Modo)
                {
                    case ModoModulo.RegistrarGrupo:
                        this.OnGuardarRegistrar(true);
                        break;
                    case ModoModulo.ModificarGrupo:
                        this.OnGuardarModificar(true);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                evtGuardarCerrar = false;
            }
        }

        #region Utilerias

        private bool StringIsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty((str ?? string.Empty).Trim());
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

            switch (this.Modo)
            {
                case ModoModulo.Registrar:
                case ModoModulo.RegistrarGrupo:
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar", OnGuardar, ConstantesIconos.General_Guardar, Shortcut.CtrlG);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar y Cerrar", OnGuardarCerrar, ConstantesIconos.Cgo_GuardarCerrar, Shortcut.CtrlU);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Cancelar", OnCancelar, ConstantesIconos.General_Cancelar, Shortcut.CtrlA);
                    break;
                case ModoModulo.Modificar:
                case ModoModulo.ModificarGrupo:
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar y Cerrar", OnGuardarCerrar, ConstantesIconos.Cgo_GuardarCerrar, Shortcut.CtrlU);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Cancelar", OnCancelar, ConstantesIconos.General_Cancelar, Shortcut.CtrlA);
                    break;
                case ModoModulo.Eliminar:
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Eliminar", OnEliminar, ConstantesIconos.General_Eliminar, Shortcut.CtrlE);
                    break;
            }

            servicioMenu.AgregaBotonMenuCatalogoGrupo("Otros", "Cerrar", OnCerrar, ConstantesIconos.General_Cerrar, Shortcut.CtrlF4);
        }

        #endregion

        #region IServicioBotonCerrarTab Members

        public void BotonCerrarClick()
        {
            try
            {
                if (this.thread != null)
                {
                    this.thread.RunWorkerCompleted -= this.thread_RunWorkerCompleted;
                    if (this.thread.IsBusy)
                    {
                        this.thread.CancelAsync();
                    }

                    this.thread.DoWork -= this.thread_DoWork;
                    this.thread.Dispose();
                    this.thread = null;
                }
            }
            finally
            {
                if (this.matriz.Compare(this.AuxMatriz))
                {
                    this._presenter.CloseView(this.Invoker);
                }
                else if (Mensaje.MensajeConf("¿Desea salir sin guardar los cambios?"))
                {
                    this._presenter.CloseView(this.Invoker);
                }
            }
        }

        #endregion

        private void thread_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginSafe(delegate { this.Cursor = Cursors.WaitCursor; });
            if (!string.IsNullOrEmpty(this.matriz.Matriz))
            {
                ListaAdministrarClientes lst = this._presenter.ObtenerTodosFiltro(new FiltroAdministrarClientes() { Matriz = this.matriz.Matriz });
                if (!((BackgroundWorker)sender).CancellationPending || !e.Cancel)
                {
                    if (lst != null && lst.Count() > 0)
                    {
                        e.Result = new ListaAdministrarClientes();
                        ((ListaAdministrarClientes)e.Result).AddRange(lst.Where(p => p.Matriz.Equals(this.matriz.Matriz)));
                    }
                }
            }
        }
        private void thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null)
                {
                    this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
                    Mensaje.MensajeError(e.Error.Message); return;
                }

                if (e.Result != null)
                {
                    this.gridControl1.DataSource = e.Result;
                    this.gridView2_FocusedRowChanged(null, null);
                }
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        #region IVMAdministrarGrupos Members

        public ListaAdministrarClientes ObtenerTodosFiltro(FiltroAdministrarClientes filtro)
        {
            throw new NotImplementedException();
        }

        public ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes UsuariosClienteObtenerTodos(ImagenSoft.ModuloWeb.Entidades.Web.FiltroAdministrarUsuariosClientes filtro)
        {
            throw new NotImplementedException();
        }

        public bool UsuariosClienteInsertarModificar(ImagenSoft.ModuloWeb.Entidades.Web.ListaAdministrarUsuariosClientes lista)
        {
            throw new NotImplementedException();
        }

        public bool UsuariosClienteInsertar(AdministrarUsuariosClientes entidad)
        {
            throw new NotImplementedException();
        }

        public bool UsuariosClienteModificar(AdministrarUsuariosClientes entidad)
        {
            throw new NotImplementedException();
        }

        public bool UsuariosClienteEliminar(FiltroAdministrarUsuariosClientes filtro)
        {
            throw new NotImplementedException();
        }

        public bool UsuariosClienteNuevaContrasenia(FiltroAdministrarUsuariosClientes filtro)
        {
            throw new NotImplementedException();
        }


        public bool Insertar(AdministrarClientes entidad)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(AdministrarClientes entidad)
        {
            throw new NotImplementedException();
        }

        public AdministrarClientes Obtener(FiltroAdministrarClientes filtro)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

