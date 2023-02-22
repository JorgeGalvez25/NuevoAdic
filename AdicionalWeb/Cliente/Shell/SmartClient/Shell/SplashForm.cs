using System.Threading;
using System.Windows.Forms;
using EstandarCliente.Infrastructure.Shell.Properties;

namespace EstandarCliente.Infrastructure.Shell
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            this.Opacity = 0;

            this.BackgroundImage = Resources.splashImage;
            this.lblVersion.Text = string.Format("Versión: {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Application.DoEvents();
        }

        public void UIThread(MethodInvoker code)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        public void IniciarEfecto()
        {
            //this.lblVersion.BeginSafe(delegate { this.lblVersion.Visible = false; });
            MethodInvoker delegado = new MethodInvoker(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    this.UIThread(delegate
                    {
                        this.Opacity = this.Opacity + 0.01;
                        this.Refresh();
                        Thread.Sleep(20);
                    });
                }
            });
            //delegado.BeginInvoke((async) => { this.lblVersion.BeginSafe(delegate { this.lblVersion.Visible = true; }); }, null);
            delegado.BeginInvoke(null, null);
        }

        public void EfectoCerrar()
        {
            MethodInvoker delegado = new MethodInvoker(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    this.UIThread(delegate
                    {
                        this.Opacity = this.Opacity - 0.01;
                        this.Refresh();
                        Thread.Sleep(5);
                    });
                }
                this.UIThread(Close);
            });
            delegado.BeginInvoke(null, null);
        }
    }
}
