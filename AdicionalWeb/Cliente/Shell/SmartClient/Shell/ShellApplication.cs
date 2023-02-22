using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EstandarCliente.Infrastructure.Library;
using ImagenSoft.Actualizador.Entidades;
using ImagenSoft.Actualizador.Proveedor;
using ImagenSoft.Librerias;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace EstandarCliente.Infrastructure.Shell
{
    /// <summary>
    /// Main application entry point class.
    /// Note that the class derives from CAB supplied base class FormShellApplication, and the 
    /// main form will be ShellForm, also created by default by this solution template
    /// </summary>
    class ShellApplication : SmartClientApplication<WorkItem, RibbonForm>
    {
        private static bool _Actualizar = true;

        /// <summary>
        /// Application entry point.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");

            object nombreAplicacion = null;
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            try
            {
                nombreAplicacion = appReader.GetValue("appName", typeof(string));
            }
            catch
            {
            }
            string appName = nombreAplicacion != null ? nombreAplicacion.ToString() : "Suite-ModuloWeb";

            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("NoActualizar")) _Actualizar = false;
            }
            bool unique;

            Mutex m = new Mutex(false, appName, out unique);

            if (!unique)
            {
                MessageBox.Show("Ya se encuentra ejecutando una instancia de la aplicación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                try
                {
                    //ExcepcionLogs.CargaConfiguracion();
                    //ExcepcionLogs.IniciarMetodo("SuitImagenSoft");

                    AppDomainSetup setupInfo = new AppDomainSetup();
                    setupInfo.PrivateBinPath = "bin;plugins;external,Modulos";
                    AppDomain ad = AppDomain.CreateDomain("Modulos", null, setupInfo);

                    #region Deprecated
                    //AppDomain.CurrentDomain.AppendPrivatePath("Modulos");
                    //setupInfo.ApplicationBase = @"C:\ImagenSoft Net\Estandares\SmartClient\EstandarCliente\bin\Debug\";
                    //setupInfo.ConfigurationFile = "Shell.exe.config";
                    //AppDomain newDomain = AppDomain.CreateDomain("My New AppDomain", null, setupInfo); 
                    #endregion
#if (DEBUG)
                    RunInDebugMode();
#else
                    RunInReleaseMode();
#endif

                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("ShellApplication", e);
                    ExcepcionLogs.Excepcion(e);
                    MessageBox.Show(e.Message + " " + e.Source + " " + e.TargetSite);
                }
                finally
                {
                    ExcepcionLogs.TerminarMetodo("SuitImagenSoft");
                    ExcepcionLogs.LogSistemaOperativo();
                }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            StringBuilder m = new StringBuilder().AppendLine("Ha ocurrido un error inesperado.")
                                                 .AppendLine()
                                                 .AppendLine("Detalles:")
                                                 .AppendLine()
                                                 .AppendLine(string.Format("Mensaje: {0}", e.Exception.Message))
                                                 .AppendLine(string.Format("Stack: {0}", e.Exception.StackTrace))
                                                 .AppendLine(string.Format("Source: {0}", e.Exception.Source));

            string err = m.ToString();
            try
            {
                Exception aux = e.Exception.InnerException;
                while (aux != null)
                {
                    m.AppendLine("***** Inner *****")
                     .AppendLine()
                     .AppendLine("Detalles:")
                     .AppendLine()
                     .AppendLine(string.Format("Mensaje: {0}", aux.Message))
                     .AppendLine(string.Format("Stack: {0}", aux.StackTrace))
                     .AppendLine(string.Format("Source: {0}", aux.Source));

                    aux = aux.InnerException;
                }

                System.Diagnostics.EventLog.WriteEntry("ShellApplication_Application_ThreadException", m.ToString());
                System.Diagnostics.Trace.TraceError("ShellApplication_Application_ThreadException", e.Exception);
                //ExcepcionLogs.Excepcion("ShellApplication_Application_ThreadException", e.Exception);
            }
            finally
            {
                ExcepcionLogs.TerminarMetodo("ShellApplication_Application_ThreadException");
            }

            StringBuilder ms = new StringBuilder(err).AppendLine()
                                                     .AppendLine()
                                                     .AppendLine("¿Desea continuar?");

            if (MessageBox.Show(ms.ToString(), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                Application.UseWaitCursor = false;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private static void RunInDebugMode()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (_Actualizar)
                {
                    //Actualizacion actual = new Actualizacion();
                    //actual.Actualizar();
                }

                try
                {
                    FormaSplash.Iniciar();
                    Application.DoEvents();
                    new ShellApplication().Run();
                }
                catch (Exception ex)
                {
                    Application.DoEvents();
                    System.Diagnostics.Trace.TraceError("ShellApplication_Application", ex);
                    string mensaje = string.Format("Ocurrió un Error:, {0}", ex.Message);
                    MessageBox.Show(mensaje, "Error de Ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    FormaSplash.Cerrar();
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("ShellApplication_Application", ex);
                if (ex.Message != "Actualizando")
                {
                    string mensaje = string.Format("Falló Intento de Actualizar Sistema, {0}", ex.Message);
                    MessageBox.Show(mensaje, "Error de Actualización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
            }
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainUnhandledException);
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (_Actualizar)
                {
                    //Actualizacion actual = new Actualizacion();
                    //actual.Actualizar();
                }

                try
                {
                    FormaSplash.Iniciar();
                    Application.DoEvents();
                    new ShellApplication().Run();
                }
                catch (Exception ex)
                {
                    Application.DoEvents();
                    System.Diagnostics.Trace.TraceError("ShellApplication_Application", ex);
                    string mensaje = string.Format("Ha ocurrido un Error, {0}", ex.Message);
                    MessageBox.Show(mensaje, "Error de Ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    HandleException(ex);
                    Application.Exit();
                }
                finally
                {
                    FormaSplash.Cerrar();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("ShellApplication_Application", ex);
                if (ex.Message != "Actualizando")
                {
                    string mensaje = string.Format("Falló Intento de Actualizar Sistema, {0}", ex.Message);
                    MessageBox.Show(mensaje, "Error de Actualización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            ExceptionPolicy.HandleException(ex, "Default Policy");
            MessageBox.Show("An unhandled exception occurred, and the application is terminating. For more information, see your Application event log.");
            Application.Exit();
        }
    }

    public class FormaSplash
    {
        public static SplashForm splash;

        static FormaSplash()
        {
            splash = new SplashForm();
        }

        public static void Iniciar()
        {
            splash.Show();
            splash.IniciarEfecto();
        }

        public static void Cerrar()
        {
            if (splash != null)
            {
                if (splash.Visible)
                {
                    splash.EfectoCerrar();
                }
                splash.Close();
                splash.Dispose();
                splash = null;
            }
        }
    }

    internal class Actualizacion
    {
        public void Actualizar()
        {
            ListaEnsamblado ensamblados = new ListaEnsamblado();
            try
            {
                ensamblados = ServiciosActualizador.CompararVersiones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            if (ensamblados.Count == 0) { return; }

            string rutaActualizador = System.Configuration.ConfigurationSettings.AppSettings["RutaActualizador"];
            string aplicacion = Path.Combine(rutaActualizador, "ImagenSoft.Actualizador.exe");

            System.IO.Directory.SetCurrentDirectory(rutaActualizador);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = aplicacion;
            p.StartInfo.Arguments = Application.ExecutablePath;
            p.Start();
            //p.WaitForExit();
            throw new Exception("Actualizando");
        }
    }
}
