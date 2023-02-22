using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.AdministrarUsuariosMdl.Views.VMAdministrarUsuarios.Modal;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Constants;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;

namespace EstandarCliente.AdministrarUsuariosMdl
{
    public partial class VMAdministrarUsuarios : UserControl,
                                                 IVMAdministrarUsuarios,
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
                    this._permiso = lst.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Modulos.USUARIOS));
                }

                return this._permiso;
            }
        }
        private AdministrarUsuarios Entidad;
        private AdministrarUsuarios EntidadAux;
        private IServiciosMenuAplicacion servicioMenu;

        internal ListaPermisos _permisosDefault;
        internal ListaPermisos PermisosDefault
        {
            get
            {
                if (_permisosDefault == null)
                {
                    _permisosDefault = new ListaPermisos();
                    var monitorTransmisiones = new Permisos();
                    {
                        monitorTransmisiones.Id = ConstantesPermisos.Modulos.MONITOR_TRANSMISIONES;
                        monitorTransmisiones.Nombre = ConstantesModulo.VISTAS.MONITOR_TRANSMISIONES_MDL.TITULO_VISTA;
                        monitorTransmisiones.Permitido = true;
                        monitorTransmisiones.SubPermisos = new ListaPermisos()
                            {
                                new Permisos()
                                    {
                                        Id = "Mostrar",
                                        Nombre = "Mostrar",
                                        Permitido = true,
                                    }
                            };
                        _permisosDefault.Add(monitorTransmisiones);
                    }
                    var programacionPrecios = new Permisos();
                    {
                        programacionPrecios.Id = ConstantesPermisos.Modulos.MONITOR_CAMBIO_PRECIOS;
                        programacionPrecios.Nombre = ConstantesModulo.VISTAS.MONITOR_CAMBIO_PRECIO_MDL.TITULO_VISTA;
                        programacionPrecios.Permitido = true;
                        programacionPrecios.SubPermisos = new ListaPermisos()
                            {
                                new Permisos()
                                    {
                                        Id = "Mostrar",
                                        Nombre = "Mostrar",
                                        Permitido = true,
                                    }
                            };
                        _permisosDefault.Add(programacionPrecios);
                    }

                    var conexiones = new Permisos();
                    {
                        conexiones.Id = ConstantesPermisos.Modulos.MONITOR_CONEXIONES;
                        conexiones.Nombre = ConstantesModulo.VISTAS.MONITOR_CONEXIONES_MDL.TITULO_VISTA;
                        conexiones.Permitido = true;
                        conexiones.SubPermisos = new ListaPermisos()
                            {
                                new Permisos()
                                    {
                                        Id = "Mostrar",
                                        Nombre = "Mostrar",
                                        Permitido = true,
                                    }
                            };
                        _permisosDefault.Add(conexiones);
                    }
                }
                return _permisosDefault;
            }
        }

        private bool evtCerrar;
        private bool evtGuardar;
        private bool evtGuardarCerrar;

        private Regex rgEmail = new Regex(@"([a-zA-Z0-9_\-\.]{1,40})@([a-zA-Z0-9_\-\.]{1,34})\.([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.Compiled);

        internal Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores> ObtenerListaDistribuidores;

        #endregion

        public VMAdministrarUsuarios(AdministrarUsuarios entidad, SesionModuloWeb sesion, VMAdministrarUsuariosPresenter presenter, string modo)
        {
            this.Entidad = entidad;
            this.EntidadAux = this.Entidad.Clone();
            this.Sesion = sesion;
            this.Presenter = presenter;

            this.evtCerrar = false;
            this.evtGuardar = false;
            this.evtGuardarCerrar = false;

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

            this.BeginSafe(delegate
                {
                    this.InicializarControles();
                    this.InicializarLuDistribuidores();
                    this.CrearEventos();

                    switch (this.Modo)
                    {
                        case ModoModulo.Registrar: this.Entidad = new AdministrarUsuarios(); this.InicializarRegistrar(); break;
                        case ModoModulo.Modificar: this.InicializarModificar(); break;
                        case ModoModulo.Propiedades:
                        default: this.InicializarPropiedades(); break;
                    }

                    this.txtNombre.Focus();
                });
        }

        private void InicializarControles()
        {
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Properties.MaxLength = 150; });
            this.txtPuesto.BeginSafe(delegate { this.txtPuesto.Properties.MaxLength = 100; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.MaxLength = 80; });
            this.txtContrasena.BeginSafe(delegate { this.txtContrasena.Properties.MaxLength = 40; });

            bool isUsrAdmin = Sesion.Usuario.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase);
            bool isAdmin = this.Entidad.Nombre.Equals("Administrador", StringComparison.CurrentCultureIgnoreCase);
            bool valid = isAdmin && isUsrAdmin;

            if (isAdmin)
            {
                this.txtNombre.BeginSafe(delegate { this.txtNombre.Properties.ReadOnly = !valid; });
                this.txtPuesto.BeginSafe(delegate { this.txtPuesto.Properties.ReadOnly = !valid; });
                this.txtContrasena.BeginSafe(delegate { this.txtContrasena.Properties.Buttons[0].Enabled = valid; });
                this.chkActivo.BeginSafe(delegate { this.chkActivo.Properties.ReadOnly = !valid; });
                this.btnPermisos.BeginSafe(delegate { this.btnPermisos.Enabled = valid; });
            }

            if (this.Entidad.IdDistribuidor == 1)
            {
                this.btnPermisos.Enabled = false;
                this.chkEsDistribuidor.Checked = false;
                this.luDistribuidor.Visible = false;
            }
            else
            {
                this.btnPermisos.Enabled = false;
                this.chkEsDistribuidor.Checked = true;
                this.luDistribuidor.Visible = true;
            }
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
                    this.luDistribuidor.Properties.DataSource = distribuidores;
                    this.luDistribuidor.EditValue = (Modo != ModoModulo.Registrar) ? this.Entidad.IdDistribuidor : 1;
                    this.luDistribuidor_EditValueChanged(null, null);
                });
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        private void CrearEventos()
        {
            this.luDistribuidor.PreviewKeyDown += this.luDistribuidor_PreviewKeyDown;
            this.chkEsDistribuidor.PreviewKeyDown += this.chkEsDistribuidor_PreviewKeyDown;

            if (this.Modo != ModoModulo.Propiedades)
            {
                this.txtNombre.PreviewKeyDown += this.txtNombre_PreviewKeyDown;
            }

            this.btnPermisos.Click += this.btnPermisos_Click;
        }
        private void QuitarEventos()
        {
            if (this.chkActivo.Visible && !this.chkActivo.Properties.ReadOnly)
            {
                this.chkActivo.PreviewKeyDown -= this.chkActivo_PreviewKeyDown;
            }

            if (this.Modo != ModoModulo.Propiedades)
            {
                this.txtNombre.PreviewKeyDown -= this.txtNombre_PreviewKeyDown;
            }
        }
        private bool StringIsNullOrEmpty(string str)
        {
            if (str == null) { return true; }
            return string.IsNullOrEmpty(str.Trim());
        }

        private void CrearEntidad()
        {
            this.Entidad.Nombre = this.txtNombre.Text;
            this.Entidad.Puesto = this.txtPuesto.Text;
            this.Entidad.Email = this.txtEMail.Text;
            if (!string.IsNullOrEmpty(this.txtContrasena.Text))
            {
                this.Entidad.Contrasena = this.txtContrasena.Text;
            }
            this.Entidad.Fecha = DateTime.Parse(this.txtFechaAlta.Text);
            this.Entidad.UltimoCambio = DateTime.Parse(this.txtFechaUltimoCambio.Text);
            this.Entidad.Activo = this.chkActivo.Checked ? "Si" : "No";
            this.Entidad.IdDistribuidor = (int)this.luDistribuidor.EditValue;

            if (this.chkActivo.Checked && this.Entidad.IdDistribuidor != 1)
            {
                this.Entidad.Permisos = this.PermisosDefault;
            }
        }
        private void InicializarControlesEntidad(AdministrarUsuarios entidad)
        {
            this.txtNombre.BeginSafe(delegate { this.txtNombre.Text = entidad.Nombre; });
            this.txtPuesto.BeginSafe(delegate { this.txtPuesto.Text = entidad.Puesto; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Text = entidad.Email; });
            this.txtContrasena.BeginSafe(delegate { this.txtContrasena.Text = entidad.Contrasena; });
            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Text = entidad.Fecha.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Text = entidad.UltimoCambio.ToString("dd/MM/yyyy HH:mm:ss"); });
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

        #region Validaciones

        private bool ValidarEMail(ref string msj)
        {
            if (this.StringIsNullOrEmpty(this.txtEMail.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "E-Mail");
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                return false;
            }

            if (!this.rgEmail.IsMatch(this.txtEMail.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "E-Mail");
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarNombre(ref string msj)
        {
            if (this.StringIsNullOrEmpty(this.txtNombre.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "Nombre");
                this.txtNombre.Focus();
                this.txtNombre.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarPuesto(ref string msj)
        {
            if (this.StringIsNullOrEmpty(this.txtPuesto.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "puesto");
                this.txtPuesto.Focus();
                this.txtPuesto.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarContrasena(ref string msj)
        {
            if (this.StringIsNullOrEmpty(this.txtContrasena.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Vacio, "contraseña");
                this.txtContrasena.Focus();
                this.txtContrasena.SelectAll();
                return false;
            }

            return true;
        }

        #endregion

        #region Eventos

        private void btnPermisos_Click(object sender, EventArgs e)
        {
            if (this.Modo != ModoModulo.Registrar)
            {
                if (this.Entidad.Clave == 0)
                {
                    Mensaje.MensajeInfo("El usuario Administrador no requiere que se asignen permisos.");
                    return;
                }
            }

            if (this.Entidad.Permisos.Count > 0)
            {
                this.Entidad.Permisos.RemoveAll(p => string.IsNullOrEmpty(p.Id));
            }

            using (ModalPermisos permisos = new ModalPermisos(this._presenter, this.Entidad))
            {
                permisos.ReadOnly = (this.Modo == ModoModulo.Propiedades);
                if (permisos.ShowDialog() == DialogResult.OK)
                {
                    this.Entidad.Permisos = permisos.Resultado;
                }
            }
        }

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
                    this.Entidad = this.EntidadAux.Clone();
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

        private void chkEsDistribuidor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                if (this.luDistribuidor.Visible && !this.luDistribuidor.Properties.ReadOnly)
                {
                    this.luDistribuidor.Focus();
                }
                else
                {
                    this.txtNombre.Focus();
                }
                e.IsInputKey = true;
            }
        }

        private void luDistribuidor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                this.txtNombre.Focus();
                e.IsInputKey = true;
            }
        }
        private void chkActivo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                if (!this.txtContrasena.Properties.ReadOnly)
                {
                    this.txtContrasena.Focus();
                }
                e.IsInputKey = true;
            }
        }
        private void txtNombre_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                if (this.luDistribuidor.Visible && !this.luDistribuidor.Properties.ReadOnly)
                {
                    this.luDistribuidor.Focus();
                }
                else
                {
                    this.chkEsDistribuidor.Focus();
                }
                e.IsInputKey = true;
            }
        }

        private void txtContrasena_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (ModalPassword modal = new ModalPassword())
            {
                if (modal.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    this.txtContrasena.Text = modal.Resultado.Clone().ToString();
                }
            }
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

        #region IServicioBotonCerrarTab Members

        public void BotonCerrarClick()
        {
            this.QuitarEventos();
            this._presenter.CloseView(this.Invoker);
            this._presenter.DisparaEvento();
        }

        #endregion

        #region IVMAdministrarUsuarios Members

        public DateTime ObtenerFechaHoraServidor()
        {
            try
            {
                return this._presenter.ObtenerFechaHoraServidor();
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return DateTime.MinValue;
            }
        }

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

        public bool Insertar(AdministrarUsuarios entidad)
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

        public bool Modificar(AdministrarUsuarios entidad)
        {
            bool resultado = false;

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

        public AdministrarUsuarios Obtener(FiltroAdministrarUsuarios filtro)
        {
            AdministrarUsuarios resultado = null;

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
    }
}

