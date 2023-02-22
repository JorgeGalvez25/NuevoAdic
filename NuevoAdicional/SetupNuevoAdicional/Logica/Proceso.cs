using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Ionic.Utils.Zip;
using Microsoft.Win32;

namespace SetupNuevoAdicional
{
    public class Proceso
    {
        public delegate void EstatusEventHandler(String e);

        public event EstatusEventHandler Estatus;

        public void Ejecutar()
        {
            DirectoryInfo path = WorkItem.Objetos<DirectoryInfo>.Get("ruta ejecutable");
            DirectoryInfo pathBD = WorkItem.Objetos<DirectoryInfo>.Get("ruta base datos");

            descargarArchivos(path, pathBD);
            configurarArchivos(path, pathBD);
            crearShorcuts(path);
            crearRegistros(path);
        }
        
        private void crearRegistros(DirectoryInfo path)
        {
            if (Estatus != null)
                Estatus.Invoke("Creando registros...");

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Constantes.UninstallConsolaResourceName);
            ZipFile zip = ZipFile.Read(stream);
            zip.ExtractAll(Environment.GetFolderPath(Environment.SpecialFolder.System), true);

            RegistryKey uninstall = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);

            while (Array.Exists(uninstall.GetSubKeyNames(), k => k.Equals("NuevoAdicional", StringComparison.OrdinalIgnoreCase)))
            {
                uninstall.DeleteSubKeyTree("NuevoAdicional");
                uninstall.Flush();
                uninstall.Close();
                uninstall = null;
                uninstall = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);
            }

            RegistryKey cliente = uninstall.CreateSubKey("NuevoAdicional");

            uninstall.Flush();

            cliente.SetValue("InstallLocation", path.FullName, RegistryValueKind.String);
            cliente.SetValue("Contact", "01 800 000 0000", RegistryValueKind.String);
            cliente.SetValue("DisplayIcon", Path.Combine(path.FullName, Constantes.FileName), RegistryValueKind.String);
            cliente.SetValue("DisplayName", "Consola Gas Adicional", RegistryValueKind.String);
            cliente.SetValue("DisplayVersion", Constantes.Version, RegistryValueKind.String);
            cliente.SetValue("HelpLink", "", RegistryValueKind.String);
            cliente.SetValue("NoModify", 1, RegistryValueKind.DWord);
            cliente.SetValue("NoRepair", 1, RegistryValueKind.DWord);
            cliente.SetValue("Publisher", "GasAdic", RegistryValueKind.String);
            cliente.SetValue("UninstallString", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), Constantes.UninstallConsolaFileName), RegistryValueKind.String);
            cliente.SetValue("URLInfoAbout", "", RegistryValueKind.String);

            cliente.Flush();

            cliente.Close();
            cliente = null;

            uninstall.Close();
            uninstall = null;

            System.Threading.Thread.Sleep(1000);
        }

        private void crearShorcuts(DirectoryInfo path)
        {
            if (Estatus != null)
                Estatus.Invoke("Creando acceso directo...");

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = null;
            string shortcutAddress = string.Empty;

            shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"GasAdic.lnk");
            if (File.Exists(shortcutAddress))
                File.Delete(shortcutAddress);
            shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = Path.Combine(path.FullName, Constantes.FileName);
            shortcut.WorkingDirectory = Path.Combine(path.FullName, Constantes.WorkingDirectory);
            shortcut.Save();
            shortcut = null;
            shortcutAddress = string.Empty;

            shortcutAddress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), @"GasAdic.lnk");
            if (File.Exists(shortcutAddress))
                File.Delete(shortcutAddress);
            shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = Path.Combine(path.FullName, Constantes.FileName);
            shortcut.WorkingDirectory = Path.Combine(path.FullName, Constantes.WorkingDirectory);
            shortcut.Save();
            shortcut = null;
            shortcutAddress = string.Empty;

            shell = null;

            System.Threading.Thread.Sleep(1000);
        }

        private void configurarArchivos(DirectoryInfo path, DirectoryInfo pathBD)
        {
            string fileName = string.Empty;
            XmlDocument xml;

            if (Estatus != null)
                Estatus.Invoke("Configurando archivos...");

            #region Consola

            fileName = Path.Combine(path.FullName, @"NuevoAdicional.exe.config");

            xml = new XmlDocument();
            xml.Load(fileName);

            foreach (XmlNode add in xml.GetElementsByTagName("add"))
            {
                if (add.Attributes["key"] == null)
                    continue;

                switch (add.Attributes["key"].Value)
                {
                    case "BaseDatos":
                        add.Attributes["value"].Value = Path.Combine(pathBD.FullName, Constantes.NombreBaseDatosCliente);
                        break;
                }
            }

            xml.Save(fileName);
            xml = null;
            fileName = string.Empty;

            #endregion

            System.Threading.Thread.Sleep(1000);
        }

        private void descargarArchivos(DirectoryInfo path, DirectoryInfo pathBD)
        {
            if (Estatus != null)
                Estatus.Invoke("Descomprimiendo archivos...");

            if (!path.Exists)
            {
                path.Create();
                path.Refresh();
            }

            // Archivos de la aplicación
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Constantes.ArchivosZipResourceName);
            ZipFile zip = ZipFile.Read(stream);
            zip.ExtractAll(path.FullName, true);

            System.Threading.Thread.Sleep(1000);

            // Base de deatos
            stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Constantes.ArchivoZipBD);
            zip = ZipFile.Read(stream);
            zip.ExtractAll(pathBD.FullName, true);

            System.Threading.Thread.Sleep(1000);
        }

    }
}
