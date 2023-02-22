using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Entidades.Monitor;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador.Monitor;
using ImagenSoft.SeriviciosWeb.Monitor.Views.Monitor;

namespace ImagenSoft.SeriviciosWeb.Monitor.Views.Login
{
    public partial class VLLogin : UserControl
    {
        private Proxy _proxy;
        private Proxy proxy
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
                if (this.workItem.Services.ContainsKey(typeof(Proxy)))
                {
                    this.workItem.Services.Add(typeof(Proxy), value);
                }
                else
                {
                    this.workItem.Services[typeof(Proxy)] = value;
                }
            }
        }
        private WorkItem workItem;

        public VLLogin()
        {
            this.InitializeComponent();
        }
        public VLLogin(WorkItem workItem)
            : this()
        {
            this.workItem = workItem;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            base.OnLoad(e);
            //this.CentrarPanel();
            this.CrearEventos();
            this.CentrarPanel();
            this.CrearEventosProxy();
        }

        private void CentrarPanel()
        {
            //this.pnlLogin.SuspendLayout();
            //this.pnlLogin.Location = new Point(this.ClientSize.Width / 2 - this.pnlLogin.Size.Width / 2,
            //                                   this.ClientSize.Height / 2 - this.pnlLogin.Size.Height / 2);
            //this.pnlLogin.Anchor = AnchorStyles.None;
            //this.pnlLogin.ResumeLayout();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.EliminarEventosProxy();
            this.Visible = false;
        }

        private void CrearEventos()
        {
            this.btnLogin.Click += this.btnLogin_Click;
            this.Resize += this.VLLogin_Resize;
        }

        private void VLLogin_Resize(object sender, EventArgs e)
        {
            this.CentrarPanel();
        }
        private void CrearEventosProxy()
        {
            this.proxy.OnError += this.proxy_OnError;
            this.proxy.OnSesionInicia += this.proxy_OnSesionInicia;
        }
        private void EliminarEventosProxy()
        {
            this.proxy.OnError -= this.proxy_OnError;
            this.proxy.OnSesionInicia -= this.proxy_OnSesionInicia;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.workItem.Sesion == null)
            {
                this.workItem.Sesion = new Sesion()
                    {
                        TipoSesion = TipoSesion.Monitor,
                        DatosSesion = new DatosSesion()
                        {
                            Contacto = "ImagenSoft",
                            Telefono = string.Empty,
                            Link = "www.igas.mx",
                            RazonSocial = "Imagen y Sistemas Computacionales S. C.",
                            TipoSesion = TipoSesion.Monitor
                        }
                    };
            }

            this.workItem.Sesion.Password = this.txtPassword.Text;
            this.workItem.Sesion.Usuario = this.txtUsuario.Text;
            this.workItem.Sesion.DatosSesion.Usuario = this.txtUsuario.Text;

            try
            {
                proxy.Conectar(this.workItem.Sesion);
                //(this.workItem.Services[typeof(Proxy)] as Proxy).Conectar(this.workItem.Sesion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void proxy_OnError(object sender, Exception e)
        {
            MessageBox.Show(e.Message);
        }
        private void proxy_OnSesionInicia(object sender, ImagenSoft.ServiciosWeb.Entidades.Eventos.MonitorEventArgs e)
        {
            this.OnLeave(new EventArgs());

            if (!this.Parent.Controls.ContainsKey("Monitor"))
            {
                this.workItem.Sesion = e.Sesion;
                this.Parent.Controls.Add(new VLMonitoreo(this.workItem) { Name = "Monitor", Dock = DockStyle.Fill, Sesiones = e.ListaClientes });
            }

            this.Parent.Controls["Monitor"].Show();
        }
    }
}
