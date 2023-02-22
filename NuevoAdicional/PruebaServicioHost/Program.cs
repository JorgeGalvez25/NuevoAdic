using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceProcess;

namespace PruebaServicioHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceHost host = new ServiceHost(typeof(Servicios.Adicional.ServiciosAdicional));
            //host.Open();
            //Console.WriteLine("Servicio esta corriendo");
            //Console.ReadLine();
            //Console.WriteLine("Cerrando servicio");
            //host.Close();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new srvHostAdic() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
