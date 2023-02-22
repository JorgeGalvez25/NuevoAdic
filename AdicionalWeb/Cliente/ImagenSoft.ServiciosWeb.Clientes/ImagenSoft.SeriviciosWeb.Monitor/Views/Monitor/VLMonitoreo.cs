using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador.Monitor;
using EMonitor = ImagenSoft.ServiciosWeb.Entidades.Monitor;
using ImagenSoft.ServiciosWeb.Entidades.Monitor;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;

namespace ImagenSoft.SeriviciosWeb.Monitor.Views.Monitor
{
    public partial class VLMonitoreo : UserControl
    {
        public List<Sesion> Sesiones;
        //private Sesion sesion;
        private DatosSesion destino;
        private WorkItem workItem;

        private Proxy _proxy;
        private Proxy Proxy
        {
            get
            {
                if (this._proxy == null)
                {
                    if (this.workItem.Services.ContainsKey(typeof(Proxy)))
                    {
                        this._proxy = (this.workItem.Services[typeof(Proxy)] as Proxy);
                    }
                }

                return this._proxy;
            }
            set
            {
                if (!this.workItem.Services.ContainsKey(typeof(Proxy)))
                {
                    this.workItem.Services.Add(typeof(Proxy), value);
                }
                else
                {
                    this.workItem.Services[typeof(Proxy)] = value;
                }
            }
        }

        public VLMonitoreo()
        {
            InitializeComponent();
        }

        public VLMonitoreo(WorkItem workItem)
            : this()
        {
            this.workItem = workItem;
            if (Sesiones == null)
            {
                this.Sesiones = new List<Sesion>();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Dispatcher.CurrentDispatcher.BeginInvoke(new MethodInvoker(() =>
                {
                    this.pnlFooter.Visible = false;
                    Point size = new Point(10, 10);
                    this.EditarCliente(false, this.txtRazonSocial, this.txtTipoSesion);

                    try
                    {
                        this.groupControl1.BeginInit();
                        this.gridView1.BeginInit();

                        //this.lstClientes.ToolTipController = this.ToolTipController;

                        this.gridView1.OptionsBehavior.Editable = false;
                        this.gridView1.OptionsCustomization.AllowColumnMoving = false;
                        this.gridView1.OptionsCustomization.AllowColumnResizing = false;
                        this.gridView1.OptionsCustomization.AllowFilter = false;
                        this.gridView1.OptionsCustomization.AllowGroup = true;
                        this.gridView1.OptionsCustomization.AllowRowSizing = false;
                        this.gridView1.OptionsCustomization.AllowSort = false;
                        this.gridView1.OptionsDetail.AllowExpandEmptyDetails = false;
                        this.gridView1.OptionsFilter.AllowFilterEditor = false;
                        this.gridView1.OptionsSelection.MultiSelect = false;
                        this.gridView1.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
                        this.gridView1.OptionsView.ColumnAutoWidth = true;
                        this.gridView1.OptionsView.ShowAutoFilterRow = false;
                        this.gridView1.OptionsView.ShowChildrenInGroupPanel = false;
                        this.gridView1.OptionsView.ShowDetailButtons = false;
                        this.gridView1.OptionsView.ShowFooter = false;
                        this.gridView1.OptionsView.ShowGroupedColumns = false;
                        this.gridView1.OptionsView.ShowGroupPanel = false;
                        this.gridView1.OptionsView.ShowIndicator = false;
                        this.gridView1.OptionsView.ShowPreview = false;
                        this.gridView1.OptionsView.ShowVertLines = false;

                        var items = this.ObtenerObjetoListado(this.Sesiones);
                        this.lstClientes.DataSource = (items ?? new List<DatosSesion>());

                        this.gridView1.Columns["ImgEstatusConn"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
                        this.gridView1.Columns["ImgEstatus"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();

                        this.gridView1.Columns["ImgEstatusConn"].VisibleIndex = 0;
                        this.gridView1.Columns["ImgEstatus"].VisibleIndex = 1;
                        this.gridView1.Columns["Usuario"].VisibleIndex = 2;

                        this.gridView1.Columns["Usuario"].MinWidth = this.lstClientes.Width - (size.X * 2) - 5;
                        this.gridView1.Columns["ImgEstatus"].MinWidth = size.X + 1;
                        this.gridView1.Columns["ImgEstatusConn"].MinWidth = size.X + 1;
                        this.gridView1.Columns["TipoSesion"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                        this.gridView1.Columns["TipoSesion"].Group();
                        this.gridView1.ExpandAllGroups();
                    }
                    finally
                    {
                        this.gridView1.EndInit();
                        this.groupControl1.EndInit();
                    }
                    CrearEventos();
                    this.InicializarProxy();
                    this.ConfigurarDatosCliente(this.workItem.Sesion.DatosSesion);
                }), DispatcherPriority.Normal, null);
        }

        #region Metodos

        private void CrearEventos()
        {
            this.btnReenviar.Click += this.btnReenviar_Click;
            this.btnEditarCliente.Click += this.btnEditarCliente_Click;
            this.btnGuardarCliente.Click += this.btnGuardarCliente_Click;

            this.gridView1.FocusedRowChanged += this.gridView1_FocusedRowChanged;
            this.Disposed += this.VLMonitoreo_Disposed;

            this.lblOcultarInfo.Click += this.lblOcultarInfo_Click;
            //this.ToolTipController.GetActiveObjectInfo += this.toolTipController1_GetActiveObjectInfo;
        }

        private void InicializarProxy()
        {
            if (this.Proxy == null) { this.Proxy = new Proxy(); }

            this.Proxy.OnError += (s, e) =>
                {
                    MessageBox.Show(e.Message);
                };

            this.Proxy.OnActualizarListado += (s, e) =>
                {
                    if (e.ListaClientes.Count > 0)
                    {
                        this.Sesiones.Clear();
                        this.Sesiones = e.ListaClientes;
                        this.ActualizarTabla(this.Sesiones);
                    }
                };

            this.Proxy.OnClienteFinalizaSesion += (s, e) =>
                {
                    this.Sesiones.RemoveAll(p => p.UUID.Equals(e.Cliente.UUID, StringComparison.CurrentCultureIgnoreCase));
                    this.ActualizarTabla(this.Sesiones);
                };
            this.Proxy.OnClienteIniciaSesion += (s, e) =>
                {
                    this.Sesiones.Add(e.Cliente);

                    if (this.Sesiones.Count > 0)
                    {
                        this.ActualizarTabla(this.Sesiones);
                    }
                };

            this.Proxy.OnMonitorFinalizaSesion += (s, e) =>
                {
                };
            this.Proxy.OnMonitorIniciaSesion += (s, e) =>
                {
                };
            this.Proxy.OnRecibeComando += (s, e) =>
                {
                    this.BuscarYCambiarEstatus(e.Cliente, e.Operacion);
                };
            this.Proxy.OnResultadoComando += (s, e) =>
                {
                    string msj = string.Format("El comando de {0} al cliente {1} tuvo como resultado...", e.Sesion.Usuario, e.Resultado);
                    MessageBox.Show(msj);
                };
            this.Proxy.OnSesionFinaliza += (s, e) =>
                {
                };
            //this.Proxy.OnSesionInicia += (s, e) =>
            //    {
            //        this.workItem.Sesion = e.Sesion;
            //        this.ConfigurarDatosCliente(this.workItem.Sesion.DatosSesion);

            //        if (e.ListaClientes.Count > 0)
            //        {
            //            this.Sesiones = e.ListaClientes;
            //            this.ActualizarTabla(this.Sesiones);
            //        }
            //    };
        }

        private void ConfigurarDatosCliente(DatosSesion cliente)
        {
            if (cliente != null)
            {
                this.txtRazonSocial.Text = cliente.RazonSocial;
                this.txtTipoSesion.Text = cliente.Usuario;

                this.lblFechaConexion.Text = cliente.InicioSesion.ToString("dd/MM/yyyy HH:mm:ss");
                this.lblPaginas.Text = cliente.Link;
                this.lblContacto.Text = cliente.Contacto;
                this.lblTelefonos.Text = cliente.Telefono;
                this.lblTipoSesion.Text = cliente.TipoSesion.ToString();

                if (cliente.Conexion != null)
                {
                    try { this.lblIPs.Text = cliente.Conexion.UriCliente.Authority; }
                    catch { }
                }

                this.btnReenviar.Visible = cliente.TipoSesion == TipoSesion.Cliente;
            }
        }

        private void EditarCliente(bool editar, params TextBox[] text)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new MethodInvoker(() =>
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (editar)
                        {
                            text[i].BorderStyle = BorderStyle.Fixed3D;
                            text[i].BackColor = System.Drawing.Color.White;
                            text[i].ReadOnly = false;
                        }
                        else
                        {
                            text[i].BorderStyle = BorderStyle.None;
                            text[i].BackColor = System.Drawing.SystemColors.Control;
                            text[i].ReadOnly = true;
                        }
                    }
                }));
        }

        #endregion

        #region Eventos

        private void lblOcultarInfo_Click(object sender, EventArgs e)
        {
            if (this.pnlFooter.Visible)
            {
                this.pnlFooter.Visible = false;
                this.lblOcultarInfo.Text = "Mostrar Información";
            }
            else
            {
                this.pnlFooter.Visible = true;
                this.lblOcultarInfo.Text = "Ocultar Información";
            }
        }

        private void btnReenviar_Click(object sender, EventArgs e)
        {
            if (destino != null)
            {
                Sesion dest = new Sesion()
                    {
                        InicioSesion = destino.InicioSesion,
                        Password = string.Empty,
                        TipoSesion = destino.TipoSesion,
                        Usuario = destino.Usuario,
                        UUID = destino.UUID,
                        DatosSesion = destino
                    };
                try
                {
                    this.Proxy.EnviarComando(this.workItem.Sesion, dest, ImagenSoft.ServiciosWeb.Entidades.Monitor.Operaciones.Reenviar);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void VLMonitoreo_Disposed(object sender, EventArgs e)
        {
            try
            {
                this.Proxy.Desconectar(this.workItem.Sesion);
                System.Threading.Thread.Sleep(1000 * 5);
            }
            catch { }
            finally
            {
                if (this.Proxy != null)
                {
                    this.Proxy.Dispose();
                    this.Proxy = null;
                }
                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }

        private void btnEditarCliente_Click(object sender, EventArgs e)
        {
            this.EditarCliente(true, this.txtRazonSocial, this.txtTipoSesion);
            this.btnGuardarCliente.Visible = true;
            this.btnEditarCliente.Visible = false;
        }

        private void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            this.EditarCliente(false, this.txtRazonSocial, this.txtTipoSesion);
            this.btnGuardarCliente.Visible = false;
            this.btnEditarCliente.Visible = true;
        }

        private Thread frmModal;

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int index = e.FocusedRowHandle;
            if (index >= 0 && this.gridView1.RowCount > 0)
            {
                destino = (DatosSesion)this.gridView1.GetRow(index);
                if (frmModal != null && frmModal.IsAlive)
                {
                    frmModal.Abort();
                }
                frmModal = new Thread((p) =>
                    {
                        using (Form frm = new Form())
                        {
                            frm.SuspendLayout();
                            VLInformacion info = new VLInformacion();
                            info.SuspendLayout();
                            {
                                info.DatosInformacion = (DatosSesion)p;
                            }
                            frm.Controls.Add(info);
                            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                            frm.Size = new Size(507, 266);
                            frm.Location = new Point(this.Location.X + this.Size.Width + 2, this.Location.Y);
                            info.ResumeLayout();
                            frm.ResumeLayout();
                            frm.ShowDialog();
                        }
                    })
                    {
                        IsBackground = true
                    };
                frmModal.Start(destino);
                this.ConfigurarDatosCliente(destino);

                var x = this.gridView1.FocusedValue;
            }
        }

        #endregion

        #region AdministrarListaSesion

        private void BuscarYCambiarEstatus(Sesion s, ImagenSoft.ServiciosWeb.Entidades.Clientes.Operaciones operacion)
        {
            this.Sesiones.ForEach(p =>
                {
                    if (p.UUID.Equals(s.UUID, StringComparison.CurrentCultureIgnoreCase) ||
                        p.Usuario.Equals(s.Usuario, StringComparison.CurrentCultureIgnoreCase))
                    {
                        switch (operacion)
                        {
                            case ImagenSoft.ServiciosWeb.Entidades.Clientes.Operaciones.Correcto:
                                p.DatosSesion.EstatusEnvio = EstatusOperacion.Ok;
                                break;
                            case ImagenSoft.ServiciosWeb.Entidades.Clientes.Operaciones.Error:
                                p.DatosSesion.EstatusEnvio = EstatusOperacion.Error;
                                break;
                            case ImagenSoft.ServiciosWeb.Entidades.Clientes.Operaciones.None:
                                p.DatosSesion.EstatusEnvio = EstatusOperacion.Reenviando;
                                break;
                            default:
                                break;
                        }
                    }
                });
            this.ActualizarTabla(this.Sesiones);
        }

        private List<DatosSesion> ObtenerObjetoListado(List<Sesion> sesiones)
        {
            var resultado = sesiones.Select(p => p.DatosSesion).ToList();

            resultado.ForEach(p =>
                {
                    switch (p.EstatusEnvio)
                    {
                        case EstatusOperacion.Error:
                            p.ImgEstatus = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.Error.GetThumbnailImage(15, 15, null, IntPtr.Zero);
                            break;
                        case EstatusOperacion.Ok:
                            p.ImgEstatus = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.Info.GetThumbnailImage(15, 15, null, IntPtr.Zero);
                            break;
                        case EstatusOperacion.Reenviando:
                            p.ImgEstatus = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.Restart.GetThumbnailImage(15, 15, null, IntPtr.Zero);
                            break;
                        default:
                            break;
                    }

                    p.ImgEstatusConn = global::ImagenSoft.SeriviciosWeb.Monitor.Properties.Resources.Warning.GetThumbnailImage(15, 15, null, IntPtr.Zero);
                });

            return resultado;
        }

        private void ActualizarTabla(List<Sesion> sesiones)
        {
            ((List<DatosSesion>)this.lstClientes.DataSource).Clear();
            this.lstClientes.DataSource = this.ObtenerObjetoListado(sesiones);
            this.lstClientes.RefreshDataSource();
            this.gridView1.ExpandAllGroups();
        }

        #endregion
    }
}
