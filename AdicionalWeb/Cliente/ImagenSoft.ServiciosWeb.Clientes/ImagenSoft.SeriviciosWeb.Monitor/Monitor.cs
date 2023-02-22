using System;
using System.Windows.Forms;
using ImagenSoft.SeriviciosWeb.Monitor.Views;
using ImagenSoft.SeriviciosWeb.Monitor.Views.Login;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador.Monitor;

namespace ImagenSoft.SeriviciosWeb.Monitor
{
    public partial class FrmMonitor : Form
    {
        public FrmMonitor()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.AddServices();
                    this.FormClosed += this.FrmMonitor_FormClosed;
                    this.Controls.Add(new VLLogin(MonitorWorkItem.WorkItem) { Name = "Login", Dock = DockStyle.Fill });
                }));
        }

        private void AddServices()
        {
            MonitorWorkItem.WorkItem.Services.Add(typeof(Proxy), new Proxy());
        }

        private void FrmMonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (MonitorWorkItem.WorkItem.Services.ContainsKey(typeof(Proxy)) && MonitorWorkItem.WorkItem.Services[typeof(Proxy)] != null)
                {
                    (MonitorWorkItem.WorkItem.Services[typeof(Proxy)] as Proxy).Desconectar(MonitorWorkItem.WorkItem.Sesion);
                }
            }
            catch { }
        }
    }
}
