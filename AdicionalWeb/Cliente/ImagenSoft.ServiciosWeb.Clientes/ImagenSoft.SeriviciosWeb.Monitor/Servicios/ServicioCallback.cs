using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Forms;
using System.Windows.Threading;
using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Proveedor.Publicador.Monitor;
using ECliente = ImagenSoft.ServiciosWeb.Entidades.Clientes;
using EMonitor = ImagenSoft.ServiciosWeb.Entidades.Monitor;
using IMonitor = ImagenSoft.ServiciosWeb.Interfaces.Publicador.Monitor;
using ImagenSoft.ServiciosWeb.Entidades.Eventos;

namespace ImagenSoft.SeriviciosWeb.Monitor.Servicios
{
    public class ServicioCallback : IMonitor.IServiciosWebCallback, IDisposable
    {
        #region IServiciosWebCallback Members

        public event SesionMonitorHandler OnSesionIniciada;
        public event SesionMonitorHandler OnSesionInCliente;
        public event SesionMonitorHandler OnSesionOutCliente;
        public event SesionMonitorHandler OnActualizarListado;
        public event MonitorComandoHandler OnRecibirComando;
        public event MonitorComandoHandler OnResultadoComando;

        private void iniciaSesion(MonitorEventArgs e)
        {
            if (this.OnSesionIniciada != null)
            {
                this.OnSesionIniciada(this, e);
            }
        }
        private void iniciaClienteSesion(MonitorEventArgs e)
        {
            if (this.OnSesionInCliente != null)
            {
                this.OnSesionInCliente(this, e);
            }
        }
        private void finalizaClienteSesion(MonitorEventArgs e)
        {
            if (this.OnSesionOutCliente != null)
            {
                this.OnSesionOutCliente(this, e);
            }
        }
        private void actualizaListado(MonitorEventArgs e)
        {
            if (this.OnActualizarListado != null)
            {
                this.OnActualizarListado(this, e);
            }
        }
        private void recibeComando(MonitorComandoEventArgs e)
        {
            if (this.OnRecibirComando != null)
            {
                this.OnRecibirComando(this, e);
            }
        }
        private void resultadoComando(MonitorComandoEventArgs e)
        {
            if (this.OnResultadoComando != null)
            {
                this.OnResultadoComando(this, e);
            }
        }

        public ServicioCallback()
        {
            this.Channel = new Proxy(new InstanceContext(this));
        }

        public void SesionIniciada(Sesion sesion, List<Sesion> lstCliente)
        {
            this.iniciaSesion(new MonitorEventArgs()
                {
                    Sesion = sesion,
                    ListaClientes = lstCliente
                });
        }

        public void RecibirComando(Sesion sesion, Sesion cliente, ECliente.Operaciones operacion)
        {
            this.recibeComando(new MonitorComandoEventArgs()
                {
                    Sesion = sesion,
                    Cliente = cliente,
                    Operacion = operacion
                });
        }

        public void ResultadoComando(Sesion sesion, EMonitor.ResultadoOperacion resultado)
        {
            this.resultadoComando(new MonitorComandoEventArgs()
                {
                    Sesion = sesion,
                    Resultado = resultado
                });
        }

        public void InicioCliente(Sesion sesion, Sesion cliente)
        {
            this.InicioCliente
        }

        public void InicioMonitor(Sesion sesion, Sesion monitor)
        {
        }

        public void CerroCliente(Sesion sesion, Sesion cliente)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new MethodInvoker(() =>
                {
                    int idx = ServicioCallback.Sesiones.FindIndex(p => p.CompareTo(cliente) == 0);

                    if (idx >= 0 && ServicioCallback.Sesiones.Count > idx)
                    {
                        ServicioCallback.Sesiones.RemoveAt(idx);
                    }

                    if (OnSesionOutCliente != null)
                    {
                        OnSesionOutCliente(sesion, cliente);
                    }
                }), DispatcherPriority.Normal, null);
        }

        public void CerroMonitor(Sesion sesion, Sesion monitor)
        {
        }

        public void ActualizarListado(List<Sesion> lstClientes)
        {
            if (OnActualizarListado != null)
            {
                OnActualizarListado(lstClientes);
            }
        }

        public void SesionFinaliza(Sesion sesion)
        {
        }

        #endregion

        public void DisposeProxy()
        {
            this.Channel.DisposeConnection();
            this.Channel = null;
        }

        //public static void DisposeCallback()
        //{
        //    if (Sesiones != null)
        //    {
        //        Sesiones.Clear();
        //    }

        //    Sesiones = null;
        //    OnRecibirComando = null;
        //    OnSesionIniciada = null;
        //    OnSesionInCliente = null;
        //    OnSesionOutCliente = null;
        //    OnResultadoComando = null;
        //}

        #region IServiciosWebCallback Members

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.OnActualizarListado != null)
            {
                this.OnActualizarListado = null;
            }

            if (this.OnRecibirComando != null)
            {
                this.OnRecibirComando = null;
            }

            if (this.OnResultadoComando != null)
            {
                this.OnResultadoComando = null;
            }

            if (this.OnSesionInCliente != null)
            {
                this.OnSesionInCliente = null;
            }

            if (this.OnSesionIniciada != null)
            {
                this.OnSesionIniciada = null;
            }

            if (this.OnSesionOutCliente != null)
            {
                this.OnSesionOutCliente = null;
            }
        }

        #endregion
    }
}
