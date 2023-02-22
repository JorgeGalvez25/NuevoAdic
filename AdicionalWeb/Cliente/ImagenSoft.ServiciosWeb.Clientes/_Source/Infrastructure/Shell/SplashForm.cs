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

            //string rutaImagenCo = (System.Configuration.ConfigurationSettings.AppSettings["RutaImagenCo"]) != null ? (System.Configuration.ConfigurationSettings.AppSettings["RutaImagenCo"]).ToString() : @"C:\ImagenCo\";

            //if (System.IO.File.Exists(string.Concat(rutaImagenCo + @"Net\DbConfiguracion\Imagenes\Iconos\splashImage.png")))
            //{
            //    this.BackgroundImage = Utilerias.DameIcono("splashImage", @"C:\");
            //}
            //else
            //{
            //    this.BackgroundImage = Resources.splashImage;
            //}

            this.BackgroundImage = Resources.splashImage;
        }

        public void IniciarEfecto()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Opacity = this.Opacity + 0.01;
                Thread.Sleep(20);
            }
        }

        public void EfectoCerrar()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Opacity = this.Opacity - 0.01;
                Thread.Sleep(5);
            }
        }
    }
}
