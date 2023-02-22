using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Proveedor.Publicador;
using System;
using System.Configuration;
using System.Timers;

namespace HostPublicador.Servicios
{
    public class NotificacionDistribuidores : IDisposable
    {
        private Timer ServicioEnvioCorreo = new Timer();
        public NotificacionDistribuidores()
        {
            if (!WorkItem.Objetos<ServiciosModuloWebProveedor>.Exist())
            {
                WorkItem.Objetos<ServiciosModuloWebProveedor>.Add(new ServiciosModuloWebProveedor(new Sesion() { Nombre = "Host Modulo Web", Sistema = "MW" }, TipoConexionUsuario.Monitor));
            }

            ServicioEnvioCorreo.Elapsed -= ServicioEnvioCorreo_Elapsed;
            ServicioEnvioCorreo.Elapsed += ServicioEnvioCorreo_Elapsed;
        }
        internal string TituloMensaje = "Host Modulo Web - Proceso Enviar Correo Distribuidores";

        public void Iniciar()
        {
            MensajesRegistros.Informacion(TituloMensaje, "Iniciando notificaciones.");
            TimeSpan time = CalcularFecha0Horas();
            ServicioEnvioCorreo.Interval = time.TotalMilliseconds;
            MensajesRegistros.Informacion(TituloMensaje, string.Format("Las notificaciones se ejecutaran en: {0}", time));

            if (!ServicioEnvioCorreo.Enabled)
            {
                ServicioEnvioCorreo.Start();
                ServicioEnvioCorreo.Enabled = true;
            }

            MensajesRegistros.Informacion(TituloMensaje, "Notificaciones iniciadas.");
        }

        public void Detener()
        {
            if (ServicioEnvioCorreo.Enabled)
            {
                ServicioEnvioCorreo.Stop();
                ServicioEnvioCorreo.Enabled = false;
            }

            MensajesRegistros.Informacion(TituloMensaje, "Notificaciones detenidas.");
        }

        private void ServicioEnvioCorreo_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!(ConfigurationManager.AppSettings["NotificarDistribuidores"] ?? "No").Equals("Si", StringComparison.CurrentCultureIgnoreCase))
            {
                ServicioEnvioCorreo.Stop();
                ServicioEnvioCorreo.Enabled = false;
                return;
            }

            try
            {
                ServicioEnvioCorreo.Stop();
                ServicioEnvioCorreo.Enabled = false;
                if (WorkItem.Objetos<ServiciosModuloWebProveedor>.Exist())
                {
                    ServiciosModuloWebProveedor servicio = WorkItem.Objetos<ServiciosModuloWebProveedor>.Get();
                    servicio.ProcesarNotificaciones();
                }
            }
            catch (Exception ex)
            {
                MensajesRegistros.Error(TituloMensaje, string.Format("Mensaje: {0}\r\nStack: {1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                TimeSpan time = CalcularFecha0Horas();
                ServicioEnvioCorreo.Interval = time.TotalMilliseconds;
                MensajesRegistros.Informacion(TituloMensaje, string.Format("Las notificaciones se ejecutaran en: {0}", time));
                ServicioEnvioCorreo.Start();
                ServicioEnvioCorreo.Enabled = true;
            }
        }

        internal TimeSpan CalcularFecha0Horas()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CfgEnvios"]))
            {
                int segundos = 15;
                int.TryParse(ConfigurationManager.AppSettings["CfgEnvios"], out segundos);
                return new TimeSpan(0, 0, 0, 0, 1000 * (segundos <= 0 ? 15 : segundos));
            }
            DateTime actual = DateTime.Now.AddMinutes(1);
            DateTime fecha = actual.AddDays(1).Date;

            TimeSpan tiempo = fecha - actual;
            return tiempo;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }
        private void Dispose(bool disposed)
        {
            if (disposed)
            {
                if (ServicioEnvioCorreo != null)
                {
                    ServicioEnvioCorreo.Elapsed -= ServicioEnvioCorreo_Elapsed;

                    if (ServicioEnvioCorreo.Enabled)
                    {
                        ServicioEnvioCorreo.Stop();
                        ServicioEnvioCorreo.Enabled = false;
                    }

                    ServicioEnvioCorreo.Close();
                    ServicioEnvioCorreo.Dispose();

                    ServicioEnvioCorreo = null;
                }
            }
        }

        ~NotificacionDistribuidores()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
