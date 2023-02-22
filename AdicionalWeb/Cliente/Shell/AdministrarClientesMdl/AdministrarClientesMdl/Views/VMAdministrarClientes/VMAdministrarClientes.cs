using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using EstandarCliente.AdministrarClientesMdl.Views.VMAdministrarClientes.Modal;
using EstandarCliente.CargadorVistas.Constants;
using EstandarCliente.CargadorVistas.Properties;
using EstandarCliente.Infrastructure.Interface.Constants;
using EstandarCliente.Infrastructure.Interface.Services;
using ImagenSoft.Extensiones;
using ImagenSoft.Librerias;
using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;

namespace EstandarCliente.AdministrarClientesMdl
{
    public partial class VMAdministrarClientes : UserControl,
                                                 IVMAdministrarClientes,
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
                    this._permiso = lst.FirstOrDefault(p => p.Id.Equals(ConstantesPermisos.Modulos.CLIENTES));
                }

                return this._permiso;
            }
        }
        private AdministrarClientes Entidad;
        private AdministrarClientes EntidadAux;
        private IServiciosMenuAplicacion servicioMenu;
        private readonly Object _lock = new Object();

        private bool evtCerrar;
        private bool evtGuardar;
        private bool evtCancelar;
        private bool evtGuardarCerrar;
        private Regex rgNoEstacion = new Regex(@"E[0-9]{5}", RegexOptions.Compiled);
        private Regex rgEmail = new Regex(@"([a-zA-Z0-9_\-\.]{1,40})@([a-zA-Z0-9_\-\.]{1,34})\.([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.Compiled);
        private Regex rgIPPort = new Regex(@"(?=^.{1,254}$)(^(?:(?!\d+\.)[a-zA-Z0-9_\-]{1,63}\.?)+(?:[a-zA-Z]{2,})(:\d{1,5})?$)|((\d{1,3}.\d{0,3}.\d{1,3}.\d{1,3})(:\d{1,5})?)", RegexOptions.Compiled);
        private Regex rgTelefono = new Regex(@"((?<lada>([0-9]{3})?[\-|\s]?)?(?<telefono>([0-9|\-|\s]{6,9})))|" +
                                             @"((?<lada>([0-9]{2})?[\-|\s]?(?<codigo>[0-9]{3})?[\-|\s]?)?(?<telefono>([0-9|\-|\s]{6,9})))|" +
                                             @"(((?<codigoPais>[\+]?[0-9]{2,3})?[\-|\s]?(?<region>[0-9]{3,4})?[\-|\s]?)?((?<lada>([0-9]{3})?[\-|\s]?)?(?<telefono>([0-9|\-|\s]{6,9}))))", RegexOptions.Compiled);

        internal Func<FiltroAdministrarDistribuidores, ListaAdministrarDistribuidores> ObtenerListaDistribuidores;
        #endregion

        public VMAdministrarClientes(AdministrarClientes cliente, SesionModuloWeb sesion, VMAdministrarClientesPresenter presenter, string modo)
        {
            this.Entidad = cliente;
            if (this.Entidad != null)
            {
                this.Entidad.Desface = 1;
                this.Entidad.HorasCorte = 24;
                this.Entidad.Zona = ZonasCambioPrecio.None;
                this.Entidad.MonitorearCambioPrecio = "No";
                this.Entidad.MonitorearTransmisiones = "No";
                this.Entidad.Enlaces = new ListaEnlacesAdministrarClientes();
            }
            this.Sesion = sesion;
            this.Presenter = presenter;

            this.evtCerrar = false;
            this.evtGuardar = false;
            this.evtCancelar = false;
            this.evtGuardarCerrar = false;

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
                case ConstantesModulo.OPCIONES.PROPIEDADES:
                default:
                    this.Modo = ModoModulo.Propiedades;
                    break;
            }

            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this._presenter.OnViewReady();
            base.OnLoad(e);

            this.BeginSafe(delegate
                {
                    this.BeginSafe(this.InicializarControles);
                    this.BeginSafe(this.InicializarLuDistribuidores);
                    this.BeginSafe(this.CrearEventos);

                    switch (this.Modo)
                    {
                        case ModoModulo.Registrar:
                            this.Entidad = new AdministrarClientes();
                            {
                                this.Entidad.Desface = 1;
                                this.Entidad.HorasCorte = 24;
                                this.Entidad.Zona = ZonasCambioPrecio.None;
                                this.Entidad.MonitorearCambioPrecio = "No";
                                this.Entidad.MonitorearTransmisiones = "No";
                                this.Entidad.Enlaces = new ListaEnlacesAdministrarClientes();
                            }
                            this.InicializarRegistrar();
                            Application.DoEvents();
                            this.txtNoEstacion.Focus();
                            break;
                        case ModoModulo.Modificar:
                            this.InicializarModificar();
                            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
                            Application.DoEvents();
                            this.txtNombreComercial.Focus();
                            break;
                        case ModoModulo.Propiedades:
                            this.InicializarPropiedades();
                            this.BeginSafe(delegate { this.InicializarControlesEntidad(this.Entidad); });
                            break;
                        default:
                            break;
                    }
                });
        }

        private void InicializarControles()
        {
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Properties.MaxLength = 80; });
            this.txtTelefono.BeginSafe(delegate { this.txtTelefono.Properties.MaxLength = 15; });
            this.txtContacto.BeginSafe(delegate { this.txtContacto.Properties.MaxLength = 80; });
            this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Properties.MaxLength = 15; });
            this.txtMatriz.BeginSafe(delegate
                {
                    this.txtMatriz.Properties.MaxLength = 15;
                    this.txtMatriz.Properties.ReadOnly = true;
                });
            this.txtNombreComercial.BeginSafe(delegate { this.txtNombreComercial.Properties.MaxLength = 250; });
        }
        private void InicializarLuDistribuidores()
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
        private void InicializarControlesEntidad(AdministrarClientes entidad)
        {
            this.txtNoEstacion.BeginSafe(delegate { this.txtNoEstacion.Text = entidad.NoEstacion; });
            this.txtNombreComercial.BeginSafe(delegate { this.txtNombreComercial.Text = entidad.NombreComercial; });
            this.txtEMail.BeginSafe(delegate { this.txtEMail.Text = entidad.EMail; });
            this.txtTelefono.BeginSafe(delegate { this.txtTelefono.Text = entidad.Telefono; });
            this.txtContacto.BeginSafe(delegate { this.txtContacto.Text = entidad.Contacto; });

            this.txtFechaAlta.BeginSafe(delegate { this.txtFechaAlta.Text = entidad.FechaAlta.ToString("dd/MM/yyyy HH:mm:ss"); });
            this.txtFechaUltimoCambio.BeginSafe(delegate { this.txtFechaUltimoCambio.Text = entidad.FechaUltimaConexion.ToString("dd/MM/yyyy HH:mm:ss"); });

            this.txtMatriz.BeginSafe(delegate { this.txtMatriz.Text = string.IsNullOrEmpty(entidad.Matriz) ? string.Empty : entidad.Matriz; });
            this.txtHostPuerto.BeginSafe(delegate { this.txtHostPuerto.Text = string.Format("{0}:{1}", entidad.Host ?? string.Empty, entidad.Puerto); });

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
                    });
            }
            finally
            {
                this.BeginSafe(delegate { this.Cursor = Cursors.Default; });
            }
        }

        private void CrearEventos()
        {
            //if (this.Modo != ModoModulo.Propiedades)
            //{
            //    this.txtNoEstacion.PreviewKeyDown += this.txtNoEstacion_PreviewKeyDown;

            //    this.txtMatriz.Properties.ButtonClick += new ButtonPressedEventHandler(Properties_ButtonClick);
            //}
        }
        private void QuitarEventos()
        {
            //if (this.Modo != ModoModulo.Propiedades)
            //{
            //    this.txtNoEstacion.PreviewKeyDown -= this.txtNoEstacion_PreviewKeyDown;
            //}
        }
        private void CrearEntidad()
        {
            this.Entidad.NoEstacion = this.txtNoEstacion.Text;
            this.Entidad.NombreComercial = this.txtNombreComercial.Text;
            this.Entidad.Contacto = this.txtContacto.Text;
            this.Entidad.EMail = this.txtEMail.Text;
            this.Entidad.Telefono = this.txtTelefono.Text;
            this.Entidad.FechaAlta = DateTime.Parse(this.txtFechaAlta.Text);
            this.Entidad.FechaUltimaConexion = DateTime.Parse(this.txtFechaUltimoCambio.Text);
            this.Entidad.Activo = this.chkActivo.Checked ? "Si" : "No";
            this.Entidad.IdDistribuidor = (int)this.luDistribuidor.EditValue;

            this.Entidad.NoEstacion = this.Entidad.NoEstacion;
            this.Entidad.Matriz = this.txtMatriz.Text.Trim();

            string[] host = this.txtHostPuerto.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            this.Entidad.Puerto = 808;

            if (host.Length == 1)
            {
                this.Entidad.Host = host[0];
            }
            else if (host.Length == 2)
            {
                this.Entidad.Host = host[0];
                int puerto = 0;
                if (!int.TryParse(host[1], out puerto))
                {
                    puerto = 808;
                }

                this.Entidad.Puerto = puerto;
            }
        }

        #region Validaciones

        private bool ValidarHost(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtHostPuerto.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Host y puerto");
                    this.txtHostPuerto.Focus();
                    this.txtHostPuerto.SelectAll();
                    return false;
                }
            }

            if (!this.StringIsNullOrEmpty(this.txtHostPuerto.Text) && !this.rgIPPort.IsMatch(this.txtHostPuerto.Text.Trim()))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "Host y puerto");
                this.txtHostPuerto.Focus();
                this.txtHostPuerto.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarEMail(ref string msj, bool whiteSpace)
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

            if (!this.StringIsNullOrEmpty(this.txtEMail.Text) && !this.rgEmail.IsMatch(this.txtEMail.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "E-Mail");
                this.txtEMail.Focus();
                this.txtEMail.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarMatriz(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtMatriz.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Matriz");
                    this.txtMatriz.Focus();
                    this.txtMatriz.SelectAll();
                    return false;
                }

            }
            if (!this.StringIsNullOrEmpty(this.txtMatriz.Text) && !this.rgNoEstacion.IsMatch(this.txtMatriz.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "Matriz. Formato (E#####)");
                this.txtMatriz.Focus();
                this.txtMatriz.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarTelefono(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtTelefono.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "teléfono");
                    this.txtTelefono.Focus();
                    this.txtTelefono.SelectAll();
                    return false;
                }
            }

            if (!this.rgTelefono.IsMatch(this.txtTelefono.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "teléfono");
                this.txtTelefono.Focus();
                this.txtTelefono.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarContacto(ref string msj, bool whiteSpace)
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
        private bool ValidarNoEstacion(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtNoEstacion.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "No. Estación");
                    this.txtNoEstacion.Focus();
                    this.txtNoEstacion.SelectAll();
                    return false;
                }

            }
            if (!this.StringIsNullOrEmpty(this.txtNoEstacion.Text) && !this.rgNoEstacion.IsMatch(this.txtNoEstacion.Text))
            {
                msj = string.Format(ListadoMensajes.Error_Valor_Invalido, "No. Estación. Formato (E#####)");
                this.txtNoEstacion.Focus();
                this.txtNoEstacion.SelectAll();
                return false;
            }

            return true;
        }
        private bool ValidarNombreComercial(ref string msj, bool whiteSpace)
        {
            if (whiteSpace)
            {
                if (this.StringIsNullOrEmpty(this.txtNombreComercial.Text))
                {
                    msj = string.Format(ListadoMensajes.Error_Vacio, "Nombre Comercial");
                    this.txtNombreComercial.Focus();
                    this.txtNombreComercial.SelectAll();
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Utilerias

        private bool StringIsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty((str ?? string.Empty).Trim());
        }

        #endregion

        #region Eventos

        private void Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            using (MdlClientes modal = new MdlClientes(this.Sesion, new VLAdministrarClientesPresenter() { WorkItem = this._presenter.WorkItem }))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    this.txtMatriz.Text = modal.Result.NoEstacion;
                }
            }
        }

        private void OnCerrar(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            lock (_lock)
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
            if (evtCancelar) return;
            try
            {
                evtCancelar = true;

                this.CrearEntidad();
                if (!this.EntidadAux.Compare(this.Entidad))
                {
                    if (Mensaje.MensajeConf(ListadoMensajes.Confirmacion_Cancelar))
                    {
                        this.Entidad = this.EntidadAux.Clonar();
                        this.InicializarControlesEntidad(this.Entidad);
                    }
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

        private void txtNoEstacion_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift && !e.Control)
            {
                this.txtNombreComercial.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift && !e.Control)
            {
                if (this.chkActivo.Visible && !this.chkActivo.Properties.ReadOnly)
                {
                    this.chkActivo.Focus();
                }
                else if (!this.luDistribuidor.Properties.ReadOnly)
                {
                    this.luDistribuidor.Focus();
                }

                e.IsInputKey = true;
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

            //if (this.Entidad.Compare(this.EntidadAux))
            //{
            //    if (Mensaje.MensajeConf("¿Desea salir sin guardar?"))
            //    {
            //        this.QuitarEventos();
            //        this._presenter.CloseView(this.Invoker);
            //    }
            //}
        }

        #endregion

        #region IVMAdministrarClientes Members

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

        public bool Insertar(AdministrarClientes entidad)
        {
            try
            {
                return this._presenter.Insertar(entidad);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public bool Modificar(AdministrarClientes entidad)
        {
            try
            {
                return this._presenter.Modificar(entidad);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public bool Eliminar(FiltroAdministrarClientes filtro)
        {
            try
            {
                return this._presenter.Eliminar(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return false;
            }
        }

        public AdministrarClientes Obtener(FiltroAdministrarClientes filtro)
        {
            try
            {
                return this._presenter.Obtener(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return null;
            }
        }

        public ListaAdministrarDistribuidores ObtenerDistribuidores(FiltroAdministrarDistribuidores filtro)
        {
            try
            {
                return this._presenter.ObtenerDistribuidores(filtro);
            }
            catch (Exception e)
            {
                Mensaje.MensajeError(e.Message);
                return null;
            }
        }

        #endregion

        internal class ServiciosMonitor
        {
            public ServiciosMonitor()
                : this(string.Empty, false)
            {
            }
            public ServiciosMonitor(string monitor, bool habilitado)
            {
                this.Monitor = monitor;
                this.Habilitado = habilitado;
            }

            public string Monitor { get; set; }
            public bool Habilitado { get; set; }
        }

        internal class ListaServiciosMonitor
            : List<ServiciosMonitor>
        {
            ~ListaServiciosMonitor()
            {
                this.Clear();
            }
        }
    }
}

