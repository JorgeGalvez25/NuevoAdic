using Gurock.SmartInspect;
using ImagenSoft.ModuloWeb.Entidades;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace HostPublicador
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //System.Threading.Thread.Sleep(10 * 1000);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-MX");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-MX");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            cargarLog(AppDomain.CurrentDomain.BaseDirectory);

            ServiceBase.Run(new ServiceBase[] 
			    { 
				    new HostPublicador() 
			    });
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MensajesRegistros.Excepcion("CurrentDomain_UnhandledException", e.ExceptionObject as Exception);
        }

        private static void cargarLog(string logPath)
        {
            FileInfo info = new FileInfo(Path.Combine(logPath, "HostModuloWeb.sil"));

            if (!info.Directory.Exists)
                info.Directory.Create();

            //Connections = file(append="true", 
            //                   maxparts="10", 
            //                   maxsize="512000", 
            //                   rotate="daily", 
            //                   buffer="8192", 
            //                   async.enabled="true", 
            //                   async.queue="2048",
            //                   async.clearondisconnect="true") 
            //                   Enabled = True

            ConnectionsBuilder builder = new ConnectionsBuilder();
            builder.BeginProtocol("file");
            builder.AddOption("filename", info.FullName);
            builder.AddOption("append", true);
            builder.AddOption("maxsize", 512000);
            builder.AddOption("rotate", FileRotate.Daily); //Valores que acepta: hourly, daily, weekly y monthly
            builder.AddOption("maxparts", "10");
            builder.EndProtocol();

            SiAuto.Si.Connections = builder.Connections;
            SiAuto.Si.Enabled = true;

            SiAuto.Main.LogSeparator();
            SiAuto.Main.LogMessage("Configuracion de log terminada");
        }
    }
}
