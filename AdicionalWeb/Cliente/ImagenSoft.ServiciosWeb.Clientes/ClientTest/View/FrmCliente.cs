using System;
using System.Windows.Forms;
using System.Windows.Threading;
using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador.Cliente;
using ECliente = ImagenSoft.ServiciosWeb.Entidades.Clientes;

namespace ClientTest.View
{
    public partial class FrmCliente : UserControl
    {
        private Sesion sesion;
        private Proxy Proxy;

        public FrmCliente()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    if (this.Proxy == null) { this.Proxy = new Proxy(); }
                    this.CrearEventos();
                }));
        }

        private void InicializarProxy()
        {
            if (this.Proxy == null)
            {
                this.Proxy = new Proxy();
                this.CrearEventosProxy();
            }
        }

        private void CrearEventos()
        {
            this.btnCorrecto.Click += this.btnCorrecto_Click;
            this.btnIncorrecto.Click += this.btnIncorrecto_Click;
            this.btnCerrarSesion.Click += this.btnCerrarSesion_Click;
            this.btnIniciarSesion.Click += this.btnIniciarSesion_Click;
            this.CrearEventosProxy();

            this.Disposed += this.FrmCliente_Disposed;

            //this.btnSolitarActualizacion.Click += this.btnSolitarActualizacion_Click;

            //this.ProxyMonitor.OnSesionIniciada += (s, e) =>
            //    {
            //        this.sesion = e;
            //        this.txtInfoLog.Text += string.Format("El cliente {0} ha iniciado sesion\r\n", e.Nombre);
            //    };

            //this.ProxyMonitor.OnSesionFinalizada += (s, e) =>
            //    {
            //        this.txtInfoLog.Text += string.Format("{0} - Termino su sesion\r\n", sesion.Nombre);
            //    };
            //this.ProxyMonitor.OnRecibirComando += (s, e) =>
            //    {
            //        this.txtInfoLog.Text += string.Format("{0} Comando: {1}\r\n", e.Sesion.Nombre, e.Operacion);
            //    };

            //ServicioCallback.OnSesionIniciada = new Action<Sesion>((s) =>
            //    {
            //        this.sesion = s;
            //        this.txtInfoLog.Text += string.Format("El cliente {0} ha iniciado sesion\r\n", s.Nombre);
            //    });
            //ServicioCallback.OnSesionTerminada = new Action<Sesion>((s) =>
            //    {
            //        this.txtInfoLog.Text += string.Format("{0} - Termino su sesion\r\n", sesion.Nombre);
            //    });

            //ServicioCallback.OnRecibirComando = new Action<Sesion, ImagenSoft.ServiciosWeb.Entidades.Monitor.Operaciones>((s, op) =>
            //    {
            //        this.txtInfoLog.Text += string.Format("{0} Comando: {1}\r\n", sesion.Nombre, op);
            //    });
            //ServicioCallback.OnRespuestaActualizacion = new Action<Sesion, ECliente.Respuesta>((sesion, respuesta) =>
            //    {
            //        this.sesion = sesion;
            //        if (!respuesta.Valido)
            //        {
            //            this.txtInfoLog.Text += string.Format("{0}\r\n", respuesta.Mensaje);
            //        }
            //        else
            //        {
            //            this.txtInfoLog.Text += string.Format("Actualizando...\r\n");

            //            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //            sw.Start();
            //            var result = this.Proxy.AcChannel.GetFiles(sesion, respuesta);

            //            var fAdmin = new ImagenSoft.ServiciosWeb.Manejador.Manejador();
            //            string error = string.Empty;

            //            if (!fAdmin.RecibirStream(result, respuesta.Informacion, ref error))
            //            {
            //                MessageBox.Show(error);
            //            }
            //            sw.Stop();
            //            this.txtInfoLog.Text += string.Format("Terminado en {0:HH:mm:ss.tttt}.\r\n", sw.Elapsed);

            //            if (respuesta.Informacion.Archivo != null)
            //            {
            //                respuesta.Informacion.Archivo.Flush();
            //                respuesta.Informacion.Archivo = null;
            //            }

            //            GC.Collect();
            //            GC.WaitForFullGCComplete();
            //        }
            //    });
        }

        private void CrearEventosProxy()
        {
            this.Proxy.OnSesionInicia += (s, e) =>
            {
                this.sesion = e;
                this.txtInfoLog.Text += string.Format("Inicio sesion: {0}\r\n", e.Usuario);
            };
            this.Proxy.OnRecibeComando += (s, e) =>
            {
                this.txtInfoLog.Text += string.Format("{0} Comando: {1}\r\n", e.Sesion.Usuario, e.Operacion);
            };
            this.Proxy.OnSesionFinaliza += (s, e) =>
            {
                this.txtInfoLog.Text += string.Format("Finalizo sesion: {0}\r\n", e.Usuario);

                if (this.Proxy != null)
                {
                    this.Proxy.Dispose();
                    this.Proxy = null;
                }
            };
        }
        //public void btnSolitarActualizacion_Click(object sender, EventArgs e)
        //{
        //    if (sesion == null)
        //    {
        //        sesion = new Sesion()
        //            {
        //                UUID = Guid.NewGuid().ToString(),
        //                InicioSesion = DateTime.Now,
        //                Nombre = "Chito",
        //                Password = "*****",
        //                TipoSesion = TipoSesion.Cliente
        //            };
        //    }
        //    if (!this.Proxy.ActualizadorIsOpen) { this.Proxy.InicializarActualizador(); }
        //    this.txtInfoLog.Text += string.Format("Verificando actualizaciones, espere un momento...\r\n");
        //    this.Proxy.AcChannel.SolicitarActualizacion(sesion, new ECliente.Solicitud()
        //        {
        //            Modulo = "CXC",
        //            VerPartMayorMenor = "1.0",
        //            VerPartBuildPrivado = "3.1",
        //            Version = "1.0143"
        //        });
        //}

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (sesion == null)
            {
                sesion = new Sesion()
                    {
                        Usuario = this.txtUsuario.Text,
                        Password = this.txtPassword.Text,
                        TipoSesion = TipoSesion.Cliente,
                        DatosSesion = new ImagenSoft.ServiciosWeb.Entidades.Monitor.DatosSesion()
                        {
                            Contacto = "Yo",
                            Link = "http://chito.web.com",
                            RazonSocial = "Chito's Web Developer",
                            Telefono = "(134)567-891",
                        }
                    };
            }

            this.InicializarProxy();

            this.Proxy.Conectar(this.sesion);

            this.btnIniciarSesion.Enabled = false;
            this.btnCerrarSesion.Enabled =
                this.btnIncorrecto.Enabled =
                this.btnCorrecto.Enabled = true;
        }

        private void btnIncorrecto_Click(object sender, EventArgs e)
        {
            this.Proxy.EnviarEstatus(sesion, ECliente.Operaciones.Error);
        }

        private void btnCorrecto_Click(object sender, EventArgs e)
        {
            this.Proxy.EnviarEstatus(sesion, ECliente.Operaciones.Correcto);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (this.Proxy.Desconectar(sesion))
            {
                this.txtInfoLog.Text += string.Format("Finalizo sesion: {0}\r\n", sesion.Usuario);
            }

            this.btnIniciarSesion.Enabled = true;
            this.btnCerrarSesion.Enabled =
                this.btnIncorrecto.Enabled =
                this.btnCorrecto.Enabled = false;
        }

        private void FrmCliente_Disposed(object sender, EventArgs e)
        {
            if (this.Proxy != null)
            {
                this.Proxy.Dispose();
                this.Proxy = null;
            }
        }
    }
}
