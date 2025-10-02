using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace NuevoAdicional
{
    static class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                MessageBox.Show(args.Name);

                string name = args.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];

                string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe
                ?? AppDomain.CurrentDomain.SetupInformation.PrivateBinPath
                ?? AppDomain.CurrentDomain.SetupInformation.ApplicationBase, name);

                MessageBox.Show(path);

                return Assembly.LoadFile(path);
            };
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            bool unique = true;
            Mutex m = new Mutex(false, Configuraciones.NombreAplicacion, out unique);

            if (unique)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    var pForma = new frmLogin();
                    if (ConfigurationManager.AppSettings["InicioAuto"] == "Si" || pForma.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new frmMain());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings["ModoOculto"] == "Si")
                {
                    string ruta = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    ruta = Path.GetDirectoryName(ruta);

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("X");

                    using (StreamWriter outfile = new StreamWriter(ruta + @"\levanta.txt", true))
                    {
                        outfile.Write(sb.ToString());
                    }

                    sb = null;
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("La aplicación ya se encuentra en ejecución", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(0);
                }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine("[Exception.Message]");
            text.AppendLine(e.Exception.Message);
            text.AppendLine("[Exception.StackTrace]");
            text.AppendLine(e.Exception.StackTrace);
            Exception ex = e.Exception.InnerException;
            while (ex != null)
            {
                text.AppendLine("[InnerException.Message]");
                text.AppendLine(e.Exception.InnerException.Message);
                text.AppendLine("[InnerException.StackTrace]");
                text.AppendLine(e.Exception.InnerException.StackTrace);

                ex = ex.InnerException;
            }

            MessageBox.Show(text.ToString());
        }

    }
}
