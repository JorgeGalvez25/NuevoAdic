using ImagenSoft.Extensiones;
using ImagenSoft.ModuloWeb.Entidades.Enumeradores;
using ImagenSoft.ModuloWeb.Interfaces.Publicador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace ImagenSoft.ModuloWeb.Proveedor.Conexion
{
    public class ServiciosConexion
    {
        private int puertoSalida;
        private string scheme;
        private TipoConexionUsuario _tipoConexion;
        public bool IsLocalHost { get; set; }

        private static Dictionary<string, Uri> _cacheHost = new Dictionary<string, Uri>();
        private static Dictionary<string, Binding> _cache = new Dictionary<string, Binding>();

        private bool esPruebas
        {
            get
            {
                return (ConfigurationManager.AppSettings["Pruebas"] ?? "No").IEquals("Si");
            }
        }

        private bool IsLocal(string host)
        {
            try
            {
                // get host IP addresses
                //System.Net.IPAddress[] hostIPs = System.Net.Dns.GetHostAddresses(host);
                System.Net.IPAddress[] hostIPs = System.Net.Dns.EndGetHostAddresses(System.Net.Dns.BeginGetHostAddresses(host, null, null));
                // get local IP addresses
                //System.Net.IPAddress[] localIPs = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());

                System.Net.IPAddress[] localIPs = System.Net.Dns.EndGetHostAddresses(System.Net.Dns.BeginGetHostAddresses(System.Net.Dns.GetHostName(), null, null));

                // test if any host IP equals to any local IP or to localhost
                foreach (System.Net.IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (System.Net.IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    bool exist = false;
                    ImagenSoft.Extensiones.Asyncrono.Parallel.ForEach(localIPs, (localIP, opc) =>
                    //foreach (System.Net.IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP))
                        {
                            exist = true;
                            opc.Break();
                        }
                    }).WaitOne();
                    return exist;
                }
            }
            catch { }
            return false;
        }

        public ServiciosConexion()
            : this(TipoConexionUsuario.ZonaNormal)
        {
        }

        public ServiciosConexion(TipoConexionUsuario tipo)
        {
            this.IsLocalHost = false;
            this._tipoConexion = tipo;
            Uri uri = new Uri(ConfigurationManager.AppSettings["ModuloWeb"] ?? "net.pipe://127.0.0.1");
            switch (tipo)
            {
                case TipoConexionUsuario.Monitor:

                    if (this.esPruebas)
                    {
                        this.puertoSalida = 710;
                    }
                    else
                    {
                        switch (uri.Scheme)
                        {
                            case "net.tcp":
                                this.puertoSalida = 8210;
                                break;
                            case "http":
                                this.puertoSalida = 221;
                                break;
                            case "net.pipe":
                            default:
                                break;
                        }
                    }

                    this.scheme = "Monitor";
                    break;
                case TipoConexionUsuario.ZonaFronteriza:
                case TipoConexionUsuario.ZonaNormal:
                    this.puertoSalida = this.esPruebas ? 711 : 8211;
                    this.scheme = "Cliente";
                    break;
                case TipoConexionUsuario.UsuarioWeb:

                    if (this.esPruebas)
                    {
                        this.puertoSalida = 712;
                    }
                    else
                    {
                        switch (uri.Scheme)
                        {
                            case "net.tcp":
                                this.puertoSalida = 822;
                                break;
                            case "http":
                                this.puertoSalida = 222;
                                break;
                            case "net.pipe":
                            default:
                                break;
                        }
                    }

                    this.puertoSalida = this.esPruebas ? 712 : 8212;
                    this.scheme = "VolWeb";
                    break;
                default:
                    break;
            }
        }

        public ServiciosConexion(TipoConexionUsuario tipo, string scheme)
            : this(tipo)
        {
            this.scheme = scheme;
        }

        private static Uri _hostCfg;
        public Uri HostCfg
        {
            get
            {
                if (_hostCfg == null)
                {
                    Uri uri = new Uri(ConfigurationManager.AppSettings["ModuloWeb"] ?? "net.pipe://127.0.0.1");
                    this.IsLocalHost = this.IsLocal(uri.Host);

                    _hostCfg = (this.IsLocalHost)
                                        ? new Uri("net.pipe://" + uri.Host + uri.AbsolutePath)
                                        : uri;
                }

                return _hostCfg;
            }
        }

        public Uri HostMonitor
        {
            get
            {
                string key = string.Format("{0}-{1}", this._tipoConexion, this.puertoSalida);
                if (_cacheHost.ContainsKey(key))
                {
                    return _cacheHost[key];
                }

                UriBuilder ub = new UriBuilder();
                {
                    ub.Host = this.HostCfg.Host;
                    if (this.HostCfg.Scheme.Equals("net.tcp") ||
                        (this.HostCfg.Scheme.Equals("http") || this.HostCfg.Scheme.Equals("https")))
                    {
                        ub.Port = this.puertoSalida;/* (((this.HostCfg.Port > System.Net.IPEndPoint.MinPort) && (this.HostCfg.Port < System.Net.IPEndPoint.MaxPort))
                                             ? this.HostCfg.Port
                                             : this.puertoSalida);/**/
                    }
                    ub.Scheme = this.HostCfg.Scheme;
                    ub.Path = (this.esPruebas ? "ModuloWebPruebas/" : "ModuloWeb/") + this.scheme;
                }

                try { _cacheHost.Add(key, ub.Uri); }
                catch { }

                return ub.Uri;
            }
        }

        public Uri HostPerform
        {
            get
            {
                string key = string.Format("{0}-{1}", this._tipoConexion, this.puertoSalida);
                if (_cacheHost.ContainsKey(key))
                {
                    return _cacheHost[key];
                }
                UriBuilder ub = new UriBuilder();
                {
                    ub.Host = this.HostCfg.Host;
                    if (this.HostCfg.Scheme.Equals("http") || this.HostCfg.Scheme.Equals("https"))
                    {
                        ub.Port = this.esPruebas ? 713 : 220;
                    }
                    else if (!this.HostCfg.Scheme.Equals("net.pipe"))
                    {
                        //ub.Port = 801;
                        ub.Port = this.esPruebas ? 7130 : 821;
                    }
                    ub.Scheme = this.HostCfg.Scheme;
                    ub.Path = (this.esPruebas ? "ModuloWebPruebas/" : "ModuloWeb/") + "Perform";
                }

                try { _cacheHost.Add(key, ub.Uri); }
                catch { }

                return ub.Uri;
            }
        }

        public WSHttpBinding HttpBinding()
        {
            string key = string.Format("{0}-{1}", this._tipoConexion, typeof(WSHttpBinding).Name);
            if (_cache.ContainsKey(key))
            {
                return (WSHttpBinding)_cache[key];
            }
            WSHttpBinding _httpBinding = new WSHttpBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 0, 10, 0, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 5, 0, 0);
                TimeSpan minTime = new TimeSpan(0, 0, 1, 0, 0);

                _httpBinding.Name = "NetBinding";
                _httpBinding.CloseTimeout = minTime;
                _httpBinding.OpenTimeout = minTime;
                _httpBinding.SendTimeout = minTime;
                _httpBinding.ReceiveTimeout = maxTime;
                _httpBinding.BypassProxyOnLocal = false;
                _httpBinding.TransactionFlow = false;
                _httpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                _httpBinding.MaxBufferPoolSize = 67108864;
                _httpBinding.MaxReceivedMessageSize = 2147483647;
                _httpBinding.MessageEncoding = WSMessageEncoding.Text;
                _httpBinding.TextEncoding = Encoding.UTF8;
                _httpBinding.UseDefaultWebProxy = true;
                _httpBinding.AllowCookies = false;

                _httpBinding.ReaderQuotas.MaxDepth = 32;
                _httpBinding.ReaderQuotas.MaxStringContentLength = 67108864;
                _httpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
                _httpBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _httpBinding.ReaderQuotas.MaxNameTableCharCount = 67108864;

                _httpBinding.ReliableSession.Ordered = true;
                _httpBinding.ReliableSession.InactivityTimeout = minTime;
                _httpBinding.ReliableSession.Enabled = true;

                _httpBinding.Security.Mode = SecurityMode.None;
                _httpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                _httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }
            try { _cache.Add(key, _httpBinding); }
            catch { }
            return _httpBinding;
        }

        public BasicHttpBinding BasicHttpBinding()
        {
            string key = string.Format("{0}-{1}", this._tipoConexion, typeof(BasicHttpBinding).Name);
            if (_cache.ContainsKey(key))
            {
                return (BasicHttpBinding)_cache[key];
            }

            BasicHttpBinding _httpBasicBinding = new BasicHttpBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 0, 10, 0, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 5, 0, 0);
                TimeSpan minTime = new TimeSpan(0, 0, 1, 0, 0);

                _httpBasicBinding.Name = "NetBinding";
                _httpBasicBinding.CloseTimeout = minTime;
                _httpBasicBinding.OpenTimeout = minTime;
                _httpBasicBinding.SendTimeout = minTime;
                _httpBasicBinding.ReceiveTimeout = maxTime;
                _httpBasicBinding.BypassProxyOnLocal = false;
                _httpBasicBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                _httpBasicBinding.MaxBufferPoolSize = 67108864;
                _httpBasicBinding.MaxReceivedMessageSize = 2147483647;
                _httpBasicBinding.MessageEncoding = WSMessageEncoding.Text;
                _httpBasicBinding.TextEncoding = Encoding.UTF8;
                _httpBasicBinding.UseDefaultWebProxy = true;
                _httpBasicBinding.AllowCookies = false;

                _httpBasicBinding.ReaderQuotas.MaxDepth = 32;
                _httpBasicBinding.ReaderQuotas.MaxStringContentLength = 67108864;
                _httpBasicBinding.ReaderQuotas.MaxArrayLength = 2147483647;
                _httpBasicBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _httpBasicBinding.ReaderQuotas.MaxNameTableCharCount = 67108864;

                _httpBasicBinding.Security.Mode = BasicHttpSecurityMode.None;
                _httpBasicBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            try { _cache.Add(key, _httpBasicBinding); }
            catch { }

            return _httpBasicBinding;
        }

        public NetTcpBinding NetTcpBinding()
        {
            string key = string.Format("{0}-{1}", this._tipoConexion, typeof(NetTcpBinding).Name);
            if (_cache.ContainsKey(key))
            {
                return (NetTcpBinding)_cache[key];
            }

            NetTcpBinding _netTcpBinding = new NetTcpBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 0, 10, 0, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 5, 0, 0);

                _netTcpBinding.ListenBacklog = 1000;
                _netTcpBinding.Name = "NetBinding";
                _netTcpBinding.MaxConnections = 7100;
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
                _netTcpBinding.ReliableSession.InactivityTimeout = new TimeSpan(0, 0, 0, 1, 0);

                _netTcpBinding.ReaderQuotas.MaxArrayLength = 67108864;
                _netTcpBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _netTcpBinding.ReaderQuotas.MaxStringContentLength = 67108864;

                _netTcpBinding.Security.Mode = SecurityMode.None;
                _netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                _netTcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                _netTcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            }

            try { _cache.Add(key, _netTcpBinding); }
            catch { }

            return _netTcpBinding;
        }

        public NetNamedPipeBinding NetNamedPipeBinding()
        {
            string key = string.Format("{0}-{1}", this._tipoConexion, typeof(NetNamedPipeBinding).Name);
            if (_cache.ContainsKey(key))
            {
                return (NetNamedPipeBinding)_cache[key];
            }
            NetNamedPipeBinding _netNamedPipeBinding = new NetNamedPipeBinding();
            {
                TimeSpan maxTime = new TimeSpan(0, 0, 2, 0);
                TimeSpan normalTime = new TimeSpan(0, 0, 1, 0);
                TimeSpan minTime = new TimeSpan(0, 0, 0, 30);

                _netNamedPipeBinding.Name = "NetPipeBinding";
                _netNamedPipeBinding.MaxConnections = 7100;
                _netNamedPipeBinding.MaxBufferPoolSize = 67108864;
                _netNamedPipeBinding.MaxBufferSize = 67108864;
                _netNamedPipeBinding.MaxReceivedMessageSize = 67108864;
                _netNamedPipeBinding.TransferMode = TransferMode.Buffered;
                _netNamedPipeBinding.ReceiveTimeout = normalTime;
                _netNamedPipeBinding.CloseTimeout = minTime;
                _netNamedPipeBinding.OpenTimeout = maxTime;
                _netNamedPipeBinding.SendTimeout = normalTime;

                _netNamedPipeBinding.ReaderQuotas.MaxArrayLength = 67108864;
                _netNamedPipeBinding.ReaderQuotas.MaxBytesPerRead = 67108864;
                _netNamedPipeBinding.ReaderQuotas.MaxStringContentLength = 67108864;

                _netNamedPipeBinding.Security.Mode = NetNamedPipeSecurityMode.None;
                _netNamedPipeBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            }

            try { _cache.Add(key, _netNamedPipeBinding); }
            catch { }

            return _netNamedPipeBinding;
        }

        public Binding GetMonitorBinding()
        {
            Binding binding = null;

            switch (this.HostCfg.Scheme)
            {
                case "http":
                case "https":
                    binding = this.HttpBinding();
                    break;
                case "net.tcp":
                    binding = this.NetTcpBinding();
                    break;
                case "net.pipe":
                    binding = this.NetNamedPipeBinding();
                    break;
            }

            return binding;
        }
        public Binding GetPerformBinding()
        {
            Binding binding = null;
            TimeSpan maxTime = new TimeSpan(0, 0, 1, 0, 0);

            switch (this.HostPerform.Scheme)
            {
                case "http":
                case "https":
                    binding = this.HttpBinding();
                    break;
                case "net.tcp":
                    NetTcpBinding bind = this.NetTcpBinding();
                    bind.ReliableSession.InactivityTimeout = maxTime;
                    binding = bind;
                    break;
                case "net.pipe":
                    binding = this.NetNamedPipeBinding();
                    break;
            }

            TimeSpan minTime = new TimeSpan(0, 0, 0, 30, 0);
            binding.CloseTimeout = minTime;
            binding.OpenTimeout = maxTime;
            binding.ReceiveTimeout = maxTime;
            binding.SendTimeout = maxTime;

            return binding;
        }

        public ChannelFactory<IModuloWeb> GetChannelMonitor()
        {
            Binding binding = this.GetMonitorBinding();
            return new ChannelFactory<IModuloWeb>(binding, this.HostMonitor.ToString());
        }
        public ChannelFactory<IModuloWebPerform> GetChannelPerform()
        {
            Binding binding = this.GetPerformBinding();
            return new ChannelFactory<IModuloWebPerform>(binding, this.HostPerform.ToString());
        }
    }
}
