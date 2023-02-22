using ImagenSoft.ServiciosWeb.Entidades;
using ImagenSoft.ServiciosWeb.Entidades.Eventos;

//using PActualizador = ImagenSoft.ServiciosWeb.Proveedor.Publicador.Actualizador;
//using PCliente = ImagenSoft.ServiciosWeb.Proveedor.Publicador.Cliente;

namespace ClientTest.Servicios
{
    public class ServicioCallback
    {
        //private ProxyMonitor ChannelMonitor { get; set; }
        //private ProxyActualizador ChannelActualizador { get; set; }
        ////public PCliente.Proxy AcChannel { get; set; }

        ////public bool ActualizadorIsOpen
        ////{
        ////    get
        ////    {
        ////        return ChannelActualizador.Status == CommunicationState.Opened || ChannelActualizador.Status == CommunicationState.Created;
        ////    }
        ////}

        //public event SesionHandler OnSesionIniciada;
        //public event SesionHandler OnSesionFinalizada;
        //public event MonitorHandler OnRecibirComando;

        //private void IniciaSesion(Sesion sesion)
        //{
        //    if (this.OnSesionIniciada != null)
        //    {
        //        this.OnSesionIniciada(this, sesion);
        //    }
        //}
        //private void FinalizaSesion(Sesion sesion)
        //{
        //    if (this.OnSesionFinalizada != null)
        //    {
        //        this.OnSesionFinalizada(this, sesion);
        //    }
        //}
        //private void RecibeComando(MonitorEventArgs e)
        //{
        //    if (this.OnRecibirComando != null)
        //    {
        //        this.OnRecibirComando(this, e);
        //    }
        //}

        //public ServicioCallback()
        //{
        //    this.InicializarMonitoreo();
        //    this.InicializarActualizador();
        //}

        //public void InicializarActualizador()
        //{
        //    this.ChannelActualizador = new ProxyActualizador(); // new PActualizador.Proxy(new InstanceContext(this));
        //}

        //public void InicializarMonitoreo()
        //{
        //    this.ChannelMonitor = new ProxyMonitor(); // new PCliente.Proxy(new InstanceContext(this));
        //    {
        //        this.ChannelMonitor.OnRecibirComando += this.ChannelMonitor_OnRecibirComando;
        //        this.ChannelMonitor.OnSesionIniciada += this.ChannelMonitor_OnSesionIniciada;
        //        this.ChannelMonitor.OnSesionFinalizada += this.ChannelMonitor_OnSesionFinalizada;
        //    }
        //}

        //private void ChannelMonitor_OnSesionIniciada(object sender, Sesion sesion)
        //{
        //    this.IniciaSesion(sesion);
        //}
        //private void ChannelMonitor_OnSesionFinalizada(object sender, Sesion sesion)
        //{
        //    this.FinalizaSesion(sesion);
        //}
        //private void ChannelMonitor_OnRecibirComando(object sender, MonitorEventArgs e)
        //{
        //    this.RecibeComando(e);
        //}
    }
}
