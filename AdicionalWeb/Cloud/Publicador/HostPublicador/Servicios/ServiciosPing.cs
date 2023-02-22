using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Proveedor.Publicador;
using ImagenSoft.ModuloWeb.Servicios.WCF;
using System;
using System.Timers;

namespace HostPublicador.Servicios
{
    public class ServiciosPing
    {
        private static Timer _inner = new Timer();
        private static TimeSpan ConfigureTime = new TimeSpan(0, 10, 0);
        private static TimeSpan MinTime = new TimeSpan(0, 1, 0);

        internal string TituloMensaje = "Servicio de Pings";
        internal string IdHost = "Host Modulo Web";

        public ServiciosPing()
        {
            if (!WorkItem.Objetos<ServiciosModuloWebProveedor>.Exist())
            {
                WorkItem.Objetos<ServiciosModuloWebProveedor>.Add(new ServiciosModuloWebProveedor(new SesionModuloWeb() { Nombre = "Host Modulo Web", Sistema = "MW" }, TipoConexionUsuario.Monitor));
            }

            _inner.Elapsed += _inner_Elapsed;
        }

        public void Start(TimeSpan time)
        {
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod(TituloMensaje, "public void Start(TimeSpan time)"))
            {
                _log.LogMessage("Iniciando Servicio Pings...");
                ConfigureTime = time;
                _inner.Interval = time.TotalMilliseconds;
                _log.LogMessage("El servicio se ejecutara cada: {0}", time);

                if (!_inner.Enabled)
                {
                    _inner.Start();
                    _inner.Enabled = true;
                }

                try { _inner_Elapsed(_inner, null); }
                catch (Exception e) { _log.LogException(e); }

                _log.LogMessage("Servicio Ping iniciado.");
            }
        }

        public void Stop()
        {
            if (_inner.Enabled)
            {
                _inner.Stop();
                _inner.Enabled = false;
            }

            MensajesRegistros.Informacion(TituloMensaje, "Servicio Pings detenido.");
        }

        private void _inner_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer inner = (Timer)sender;
            using (MensajesRegistros.EnterExitMethod _log = new MensajesRegistros.EnterExitMethod(TituloMensaje, "private void _inner_Elapsed(object sender, ElapsedEventArgs e)"))
            {
                try
                {
                    inner.Stop();
                    inner.Enabled = false;

                    ServicioModuloWeb servicio = new ServicioModuloWeb();
                    if (servicio.IniciarServiciosPing())
                    {
                        _log.LogMessage("Servicio Ping Iniciado...");
                        inner.Interval = ConfigureTime.TotalMilliseconds;
                    }
                    else
                    {
                        _log.LogMessage("Servicio Ping en proceso...");
                        inner.Interval = MinTime.TotalMilliseconds;
                    }

                    //if (WorkItem.Objetos<ServiciosModuloWebProveedor>.Exist())
                    //{
                    //    ServiciosModuloWebProveedor servicio = WorkItem.Objetos<ServiciosModuloWebProveedor>.Get();
                    //    if (servicio.ServicioPings())
                    //    {
                    //        _log.LogMessage("Servicio Ping en proceso...");
                    //        inner.Interval = ConfigureTime.TotalMilliseconds;
                    //    }
                    //    else
                    //    {
                    //        _log.LogMessage("Servicio Ping...");
                    //        inner.Interval = MinTime.TotalMilliseconds;
                    //    }
                    //}
                    //else
                    //{
                    //    _log.LogMessage("Servicio Ping no existe");
                    //}
                }
                catch (Exception ex)
                {
                    _log.LogException(ex);
                }
                finally
                {
                    _inner.Start();
                    _inner.Enabled = true;
                }
            }
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
                if (_inner != null)
                {
                    _inner.Elapsed -= _inner_Elapsed;

                    if (_inner.Enabled)
                    {
                        _inner.Stop();
                        _inner.Enabled = false;
                    }

                    _inner.Close();
                    _inner.Dispose();

                    _inner = null;
                }
            }
        }

        ~ServiciosPing()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
