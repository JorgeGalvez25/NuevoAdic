using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace PruebaServicioHost
{
    partial class srvHostAdic : ServiceBase
    {
        private ServiceHost host;

        public srvHostAdic()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(typeof(Servicios.Adicional.ServiciosAdicional));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
            host = null;
        }
    }
}
