using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace UnistallNuevoAdicional
{
    public class Proceso
    {
        internal void Ejecutar(DirectoryInfo path)
        {
            System.Threading.Thread.Sleep(5000);
            path.Refresh();

            detenerAplicacion(path);
            eliminarArchivos(path);
            eliminarShorcuts();
            eliminarRegistros();
        }

        private void eliminarRegistros()
        {
            RegistryKey uninstall = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);
            while (Array.Exists(uninstall.GetSubKeyNames(), k => k.Equals("NuevoAdicional", StringComparison.OrdinalIgnoreCase)))
            {
                uninstall.DeleteSubKeyTree("NuevoAdicional");
                uninstall.Flush();
                uninstall.Close();
                uninstall = null;
                uninstall = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);
            }
            uninstall.Close();
            uninstall = null;
        }

        private void eliminarShorcuts()
        {
            eliminarShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"GasAdic.lnk"));
            eliminarShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), @"GasAdic.lnk"));
        }

        private void eliminarShortcut(string fileName)
        {
            FileInfo f = new FileInfo(fileName);
            while (f.Exists)
            {
                f.Delete();
                f.Refresh();
            }
        }

        private void eliminarArchivos(DirectoryInfo path)
        {
            while (path.Exists)
            {
                try
                {
                    path.Delete(true);
                    path.Refresh();
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(DirectoryNotFoundException))
                        path.Refresh();
                    else
                        throw e;
                }
            }

            DirectoryInfo parent = path.Parent;

            while (parent.GetFileSystemInfos().Count() == 0)
            {
                while (parent.Exists)
                {
                    try
                    {
                        parent.Delete(true);
                        parent.Refresh();
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(DirectoryNotFoundException))
                            parent.Refresh();
                        else
                            throw e;
                    }
                }

                parent = parent.Parent;
                parent.Refresh();
            }
        }

        private void detenerAplicacion(DirectoryInfo path)
        {
            string fileName = Path.Combine(path.FullName, @"Consola\NuevoAdicional.exe");
            List<Process> procesos = new List<Process>(Process.GetProcesses());
            Process p = procesos.Find(i =>
            {
                try
                {
                    return i.MainModule.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });

            if (p != null)
            {
                MessageBox.Show("Existe una instancia de la aplicación consola Nuevo Adicional en ejecución, se cerrará automáticamente para continuar con la desinstalación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                while (!p.HasExited)
                {
                    p.Kill();
                    p.WaitForExit();
                    p.Refresh();
                }
            }
        }
    }
}
