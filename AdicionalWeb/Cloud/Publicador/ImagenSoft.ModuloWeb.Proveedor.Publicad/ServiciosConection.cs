using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Channels;
using ImagenSoft.ServiciosWeb.Interfaces.Publicador;

namespace ImagenSoft.ServiciosWeb.Proveedor.Publicador
{
    public class ServiciosConexion
    {
        public ServiciosConexion()
        {

        }

        private Uri _hostCfg;
        public Uri HostCfg
        {
            get
            {
                if (_hostCfg == null)
                {
                    _hostCfg = new Uri(ConfigurationManager.AppSettings["ServiciosWeb"]);
                }

                return _hostCfg;
            }
        }

        private Uri _hostMonitor;
        public Uri HostMonitor
        {
            get
            {
                if (_hostMonitor == null)
                {
                    UriBuilder ub = new UriBuilder();
                    {
                        ub.Host = this.HostCfg.Host;
                        if (!this.HostCfg.Scheme.Equals("net.pipe"))
                        {
                            ub.Port = 8010;
                        }
                        ub.Scheme = this.HostCfg.Scheme;
                        ub.Path = "ServiciosWeb/Monitor";
                    }
                    _hostMonitor = ub.Uri;
                }

                return _hostMonitor;
            }
        }

        private Uri _hostPerform;
        public Uri HostPerform
        {
            get
            {
                if (_hostPerform == null)
                {
                    UriBuilder ub = new UriBuilder();
                    {
                        ub.Host = this.HostCfg.Host;
                        if (!this.HostCfg.Scheme.Equals("net.pipe"))
                        {
                            ub.Port = 801;
                        }
                        ub.Scheme = this.HostCfg.Scheme;
                        ub.Path = "ServiciosWeb/Perform";
                    }
                    _hostPerform = ub.Uri;
                }

                return _hostPerform;
            }
        }

        private NetTcpBinding _netTcpBinding;
        public NetTcpBinding NetTcpBinding()
        {
            if (_netTcpBinding != null) { return _netTcpBinding; }
            _netTcpBinding = new NetTcpBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 1, 0, 0, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 5, 0, 0);

                _netTcpBinding.ListenBacklog = 100;
                _netTcpBinding.Name = "NetBinding";
                _netTcpBinding.MaxConnections = 100;
                _netTcpBinding.MaxBufferPoolSize = 67108864;
                _netTcpBinding.MaxBufferSize = 67108864;
                _netTcpBinding.MaxReceivedMessageSize = 67108864;
                _netTcpBinding.TransferMode = TransferMode.Buffered;
                _netTcpBinding.ReceiveTimeout = maxTime;
                _netTcpBinding.CloseTimeout = normalTime;
                _netTcpBinding.OpenTimeout = normalTime;
                _netTcpBinding.SendTimeout = maxTime;
                _netTcpBinding.PortSharingEnabled = true;

                _netTcpBinding.ReliableSession.Enabled = true;
                _netTcpBinding.ReliableSession.InactivityTimeout = new TimeSpan(0, 20, 0, 10, 0);

                _netTcpBinding.ReaderQuotas.MaxArrayLength = 67108864;
                _netTcpBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _netTcpBinding.ReaderQuotas.MaxStringContentLength = 67108864;

                _netTcpBinding.Security.Mode = SecurityMode.None;
                _netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                _netTcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                _netTcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            }

            return _netTcpBinding;
        }

        private NetNamedPipeBinding _netNamedPipeBinding;
        public NetNamedPipeBinding NetNamedPipeBinding()
        {
            if (_netNamedPipeBinding != null) { return _netNamedPipeBinding; }
            _netNamedPipeBinding = new NetNamedPipeBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 1, 0, 0, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 5, 0, 0);

                _netNamedPipeBinding.Name = "NetBinding";
                _netNamedPipeBinding.MaxConnections = 100;
                _netNamedPipeBinding.MaxBufferPoolSize = 67108864;
                _netNamedPipeBinding.MaxBufferSize = 67108864;
                _netNamedPipeBinding.MaxReceivedMessageSize = 67108864;
                _netNamedPipeBinding.TransferMode = TransferMode.Buffered;
                _netNamedPipeBinding.ReceiveTimeout = maxTime;
                _netNamedPipeBinding.CloseTimeout = normalTime;
                _netNamedPipeBinding.OpenTimeout = normalTime;
                _netNamedPipeBinding.SendTimeout = maxTime;

                _netNamedPipeBinding.ReaderQuotas.MaxArrayLength = 67108864;
                _netNamedPipeBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _netNamedPipeBinding.ReaderQuotas.MaxStringContentLength = 67108864;

                _netNamedPipeBinding.Security.Mode = NetNamedPipeSecurityMode.None;
                _netNamedPipeBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            }

            return _netNamedPipeBinding;
        }

        private ChannelFactory<IServiciosWeb> channel1;
        private ChannelFactory<IServiciosWebPerform> channel2;

        public ChannelFactory<IServiciosWeb> GetChannelMonitor()
        {
            if (channel1 != null &&
               (channel1.State == CommunicationState.Opened || channel1.State == CommunicationState.Opening))
            {
                return channel1;
            }

            if (channel1 != null)
            {
                try { channel1.Abort(); }
                catch
                {
                    try { channel1.Close(); }
                    catch { }
                }

                channel1 = null;
            }

            Binding binding = null;

            switch (this.HostCfg.Scheme)
            {
                case "net.tcp":
                    binding = NetTcpBinding();
                    break;
                case "net.pipe":
                    binding = NetNamedPipeBinding();
                    break;
            }

            channel1 = new ChannelFactory<IServiciosWeb>(binding, this.HostMonitor.ToString());
            return channel1;
        }
        public ChannelFactory<IServiciosWebPerform> GetChannelPerform()
        {
            if (channel2 != null &&
               (channel2.State == CommunicationState.Opened || channel2.State == CommunicationState.Opening))
            {
                return channel2;
            }

            if (channel2 != null)
            {
                try { channel2.Abort(); }
                catch
                {
                    try { channel2.Close(); }
                    catch { }
                }

                channel2 = null;
            }

            Binding binding = null;

            switch (this.HostCfg.Scheme)
            {
                case "net.tcp":
                    binding = NetTcpBinding();
                    break;
                case "net.pipe":
                    binding = NetNamedPipeBinding();
                    break;
            }

            channel2 = new ChannelFactory<IServiciosWebPerform>(binding, this.HostPerform.ToString());
            return channel2;
        }
    }
}
