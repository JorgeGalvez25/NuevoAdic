using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Constants;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarDistribuidoresMdl
{
    public partial class VMAdministrarDistribuidores : UserControl,
                                                       IVMAdministrarDistribuidores,
                                                       IConfiguraMenu,
                                                       IServicioBotonCerrarTab
    {
        #region Propiedades

        public string Titulo
        {
            get { return this.titulo1.TituloMenu; }
            set { this.titulo1.TituloMenu = value; }
        }
        public string Invoker { get; set; }

        private SesionModuloWeb Sesion;
        private ModoModulo Modo;
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

        private ListaStringGridColumn lstEMails;
        private AdministrarDistribuidores Entidad;
        private AdministrarDistribuidores EntidadAux;
        private IServiciosMenuAplicacion servicioMenu;


        internal bool evtCerrar;
        internal bool evtGuardar;
        internal bool evtEliminar;
        internal bool evtGuardarCerrar;

        internal Regex rgEmail = new Regex(@"([a-zA-Z0-9_\-\.]{1,40})@([a-zA-Z0-9_\-\.]{1,34})\.([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.Compiled);

        #endregion

        public VMAdministrarDistribuidores(AdministrarDistribuidores distribuidor, SesionModuloWeb sesion, VMAdministrarDistribuidoresPresenter presenter, string modo)
        {
            this.Entidad = distribuidor;
            this.EntidadAux = this.Entidad.Clonar();
            this.Sesion = sesion;
            this.Presenter = presenter;
            this.lstEMails = new ListaStringGridColumn();

            switch (modo)
            {
                case ConstantesModulo.OPCIONES.REGISTRAR: this.Modo = ModoModulo.Registrar; break;
                case ConstantesModulo.OPCIONES.MODIFICAR: this.Modo = ModoModulo.Modificar; break;
                case ConstantesModulo.OPCIONES.ELIMINAR: this.Modo = ModoModulo.Eliminar; break;
                case ConstantesModulo.OPCIONES.PROPIEDADES:
                default: this.Modo = ModoModulo.Propiedades; break;
            }

            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);
            this.BeginSafe(InicializarControles);

            switch (Modo)
            {
                case ModoModulo.Registrar: this.BeginSafe(this.CrearEventos); this.BeginSafe(this.InicializarRegistrar); break;
                case ModoModulo.Modificar: this.BeginSafe(this.CrearEventos); this.BeginSafe(this.InicializarModificar); break;
                case ModoModulo.Propiedades: this.BeginSafe(this.InicializarPropiedades); break;
                default:
                    break;
            }

        }

        internal void InicializarControles()
        {
            this.txtClave.BeginSafe(delegate
                {
                    this.txtClave.Properties.MaxLength = 3;
                    this.txtClave.Properties.Mask.EditMask = "D3";
                    this.txtClave.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                });

            this.txtDescripcion.BeginSafe(delegate { this.txtDescripcion.Properties.MaxLength = 150; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.MaxLength = 500; });
            this.gridControl1.BeginSafe(this.InicializarGridCorreos);
        }

        internal bool ValidarClave(ref string msj)
        {
            int clave = 0;
            int.TryParse(this.txtClave.Text, out clave);

            AdministrarDistribuidores distribuidor = this.Obtener(new FiltroAdministrarDistribuidores() { Clave = clave });

            if (distribuidor != null)
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Existe, string.Empty, clave.ToString("D3"));
                return false;
            }

            return true;
        }
        internal bool ValidarEmail(ref string msj)
        {
            return ValidarEmail(this.txtEMail.Text, ref msj);
        }
        internal bool ValidarEmail(string mail, ref string msj)
        {
            if (string.IsNullOrEmpty(mail)) { return true; }
            string[] email = mail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> invalid = new List<string>();
            if (email.Length > 0)
            {
                for (int i = 0; i < email.Length; i++)
                {
                    if (!rgEmail.IsMatch(email[i]))
                    {
                        invalid.Add(email[i]);
                    }
                }
            }

            if (invalid.Count > 0)
            {
                msj = "Correo(s) inválido(s) " + invalid.Aggregate((x, y) => x + ";" + y);
                return false;
            }

            return true;
        }
        internal bool ExisteMail(string mail, ref string msj)
        {
            List<string> invalid = new List<string>();
            var email = mail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => new StringGridColumn(p));

            foreach (var p in email)
            {
                foreach (var item in (ListaStringGridColumn)this.gridControl1.DataSource)
                {
                    if (p.Column.Equals(item.Column, StringComparison.CurrentCultureIgnoreCase))
                    {
                        invalid.Add(p.Column);
                    }
                }
            };

            if (invalid.Count > 0)
            {
                msj = "Existen correos repetidos " + invalid.Aggregate((x, y) => x + ";" + y);
                return true;
            }

            return false;
        }

        internal void CrearEventos()
        {
            this.btnAdd.Click += this.btnAdd_Click;
            this.btnDelete.Click += this.btnDelete_Click;

            this.txtEMail.Validated += this.txtEMail_Validated;
            this.txtEMail.PreviewKeyDown += this.txtEMail_PreviewKeyDown;
        }
        internal void QuitarEventos()
        {
            this.btnAdd.Click -= this.btnAdd_Click;
            this.btnDelete.Click -= this.btnDelete_Click;

            this.txtEMail.Validated -= this.txtEMail_Validated;
            this.txtEMail.PreviewKeyDown -= this.txtEMail_PreviewKeyDown;
        }

        internal void CrearEntidad()
        {
            int clave = 0;
            int.TryParse(this.txtClave.Text, out clave);
            this.Entidad.Clave = clave;
            this.Entidad.Activo = this.chkActivo.Checked ? "Si" : "No";
            this.Entidad.Descripcion = this.txtDescripcion.Text.Trim();
            this.Entidad.EMail = (this.gridControl1.DataSource as ListaStringGridColumn).ToString();
        }
        internal void InicializarGridCorreos()
        {
            this.gridControl1.DataSource = this.lstEMails;
            this.gridControl1.Refresh();
            this.gridControl1.RefreshDataSource();
            this.gridView1.RefreshData();
            try
            {
                this.gridControl1.BeginInit();
                this.gridControl1.SuspendLayout();
                this.gridView1.BeginInit();

                this.gridControl1.UseEmbeddedNavigator = false;
                this.gridView1.OptionsBehavior.Editable = true;
                this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;

                bool esPropiedades = (this.Modo == ModoModulo.Propiedades);

                this.gridView1.Columns[0].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                this.gridView1.Columns[0].OptionsColumn.AllowEdit = !esPropiedades;
                this.gridView1.Columns[0].OptionsColumn.ReadOnly = esPropiedades;
                if (!esPropiedades)
                {
                    this.gridView1.ValidateRow += this.gridView1_ValidateRow;
                }
            }
            finally
            {
                this.gridControl1.ResumeLayout();
                this.gridView1.EndInit();
                this.gridControl1.Refresh();
                this.gridView1.BestFitColumns();
                this.gridControl1.EndInit();

                if (this.gridControl1.ContextMenuStrip != null)
                {
                    this.gridControl1.ContextMenuStrip.Items.Clear();
                }
            }
        }
        private void InicializarControlesEntidad(AdministrarDistribuidores entidad)
        {
            this.txtClave.BeginSafe(delegate { this.txtClave.Text = entidad.Clave.ToString("D3"); });
            this.txtDescripcion.BeginSafe(delegate { this.txtDescripcion.Text = entidad.Descripcion; });
            this.txtEMail.BeginSafe(delegate
                {
                    this.txtEMail.Text = string.Empty;
                    if (this.gridControl1.DataSource != null)
                    {
                        (this.gridControl1.DataSource as ListaStringGridColumn).Clear();
                        this.gridControl1.DataSource = null;
                    }
                    this.lstEMails.Clear();
                    if (!string.IsNullOrEmpty(entidad.EMail.Trim()))
                    {
                        this.lstEMails.AddRange(entidad.EMail.Split(new char[] { ';' }).Select(p => new StringGridColumn(p)));
                    }
                    this.gridControl1.DataSource = this.lstEMails;
                    this.gridControl1.Refresh();
                    this.gridControl1.RefreshDataSource();
                    this.gridView1.RefreshData();
                });
            this.chkActivo.BeginSafe(delegate
                {
                    switch (Modo)
                    {
                        case ModoModulo.Registrar:
                            this.chkActivo.Checked = true;
                            break;
                        case ModoModulo.Modificar:
                        case ModoModulo.Eliminar:
                        case ModoModulo.Propiedades:
                        default:
                            this.chkActivo.Checked = entidad.Activo.Equals("Si", StringComparison.CurrentCultureIgnoreCase);
                            break;
                    }
                });
        }

        #region Eventos

        private void OnCerrar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (evtCerrar) return;
            try
            {
                evtCerrar = true;

                if (this.Modo == ModoModulo.Modificar || this.Modo == ModoModulo.Registrar)
                {
                    this.CrearEntidad();

                    if (!this.EntidadAux.Compare(this.Entidad))
                    {
                        if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Guardar))
                        {
                            this.BotonCerrarClick();
                        }
                    }
                    else
                    {
                        this.BotonCerrarClick();
                    }
                }
                else
                {
                    this.BotonCerrarClick();
                }
            }
            finally
            {
                evtCerrar = false;
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
                    case ModoModulo.Registrar:
                        this.OnGuardarRegistrar(false);
                        break;
                    case ModoModulo.Modificar:
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
        private void OnCancelar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (evtGuardar) return;
            try
            {
                evtGuardar = true;

                if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Cancelar))
                {
                    this.Entidad = this.EntidadAux.Clonar();
                    this.InicializarControlesEntidad(this.Entidad);
                }
            }
            finally
            {
                evtGuardar = false;
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
                    case ModoModulo.Registrar:
                        this.OnGuardarRegistrar(true);
                        break;
                    case ModoModulo.Modificar:
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string msj = string.Empty;
            if (!this.ValidarEmail(ref msj))
            {
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                Mensaje.MensajeError(msj);
                return;
            }

            if (!string.IsNullOrEmpty(this.txtEMail.Text.Trim()))
            {
                if (this.ExisteMail(this.txtEMail.Text, ref msj))
                {
                    Mensaje.MensajeError(msj);
                    return;
                }

                var email = this.txtEMail.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(p => new StringGridColumn(p));

                ((ListaStringGridColumn)this.gridControl1.DataSource).AddRange(email);
                this.gridControl1.Refresh();
                this.gridView1.RefreshData();
                this.txtEMail.Text = string.Empty;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var item = this.gridView1.GetFocusedValue();
            if (item != null)
            {
                if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Eliminar_02, item))
                {
                    ((ListaStringGridColumn)this.gridControl1.DataSource).RemoveAll(p => p.Column.Equals(item.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    this.gridControl1.Refresh();
                    this.gridView1.RefreshData();
                }
            }
        }

        private void txtEMail_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEMail.Text.Trim())) { return; }

            string msj = string.Empty;
            this.txtEMail.ErrorText = string.Empty;

            if (!this.ValidarEmail(ref msj))
            {
                Mensaje.MensajeError(msj);
                this.txtEMail.ErrorText = msj;
                return;
            }
        }
        private void txtEMail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Control && !e.Shift && !e.Alt)
            {
                this.btnAdd_Click(null, null);
                e.IsInputKey = true;
            }
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var items = (StringGridColumn)e.Row;

            if (items != null)
            {
                string msj = string.Empty;
                if (!this.ValidarEmail(items.Column, ref msj))
                {
                    Mensaje.MensajeError(msj);
                    e.ErrorText = msj;
                    this.gridView1.FocusedRowHandle = e.RowHandle;
                    this.gridView1.ShowEditorByMouse();

                    return;
                }

                if (this.ExisteMail(items.Column, ref msj))
                {
                    Mensaje.MensajeError(msj);
                    e.ErrorText = msj;
                    this.gridView1.FocusedRowHandle = e.RowHandle;
                    this.gridView1.ShowEditorByMouse();

                    return;
                }
            }
        }

        #endregion

        #region IServicioBotonCerrarTab Members

        public void BotonCerrarClick()
        {
            if (this.Modo != ModoModulo.Propiedades)
            {
                this.QuitarEventos();
            }

            if (_permiso != null)
            {
                _permiso = null;
            }

            if (rgEmail != null)
            {
                rgEmail = null;
            }

            this._presenter.CloseView(this.Invoker);
        }

        #endregion

        #region IVMAdministrarDistribuidores Members

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

        public AdministrarDistribuidores Insertar(AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores resultado = null;
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

        public AdministrarDistribuidores Modificar(AdministrarDistribuidores entidad)
        {
            AdministrarDistribuidores resultado = null;
            try
            {
                resultado = this._presenter.Modificar(entidad);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }
            return resultado;
        }

        public AdministrarDistribuidores Obtener(FiltroAdministrarDistribuidores filtro)
        {
            AdministrarDistribuidores resultado = null;
            try
            {
                resultado = this._presenter.Obtener(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
            }
            return resultado;
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
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar", OnGuardar, ConstantesIconos.General_Guardar, Shortcut.CtrlG);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar y Cerrar", OnGuardarCerrar, ConstantesIconos.Cgo_GuardarCerrar, Shortcut.CtrlU);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Cancelar", OnCancelar, ConstantesIconos.General_Cancelar, Shortcut.CtrlA);
                    break;
                case ModoModulo.Modificar:
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Guardar y Cerrar", OnGuardarCerrar, ConstantesIconos.Cgo_GuardarCerrar, Shortcut.CtrlU);
                    servicioMenu.AgregaBotonMenuCatalogoGrupo("Opciones", "Cancelar", OnCancelar, ConstantesIconos.General_Cancelar, Shortcut.CtrlA);
                    break;
            }

            servicioMenu.AgregaBotonMenuCatalogoGrupo("Otros", "Cerrar", OnCerrar, ConstantesIconos.General_Cerrar, Shortcut.CtrlF4);
        }

        #endregion

        private class StringGridColumn
        {
            public StringGridColumn()
                : this(string.Empty)
            {
            }
            public StringGridColumn(string column)
            {
                this.Column = column;
            }
            public string Column { get; set; }
        }

        private class ListaStringGridColumn : List<StringGridColumn>
        {
            public override string ToString()
            {
                if (this.Count <= 0) { return string.Empty; }
                return this.Select(p => p.Column).Aggregate((x, y) => x.Trim() + ";" + y.Trim());
            }

            ~ListaStringGridColumn()
            {
                this.Clear();
            }
        }
    }
}

