using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace UnistallNuevoAdicional
{
    static class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string name = args.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];

                string resourceName = Array.Find(Assembly.GetExecutingAssembly().GetManifestResourceNames(), i => i.Contains(name));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool unique = true;
            Mutex m = new Mutex(false, "UninstallNuevoAdicional", out unique);

            if (unique)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                MessageBox.Show("Existe una instancia del programa de desinstalación del Nuevo Adicional ejecutandose en éste equipo, no es posible ejecutar mas de una instancia.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }
        }
    }
}
