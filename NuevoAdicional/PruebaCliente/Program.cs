using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Adicional.Entidades;
using ServiciosCliente;
using System.ServiceProcess;

namespace PruebaCliente
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceHost hostConsola = new ServiceHost(typeof(ServiciosCliente.ServiciosCliente));
            //hostConsola.Open();
            //Console.WriteLine("Servicio cliente esta corriendo");

            //ServiceHost hostAdicional = new ServiceHost(typeof(Servicios.Adicional.ServiciosAdicional));
            //hostAdicional.Open();
            //Console.WriteLine("Servicio Adicional esta corriendo");

            //Console.ReadLine();

            //Console.WriteLine("Cerrando servicio cliente");
            //hostConsola.Close();

            //Console.WriteLine("Cerrando servicio Adicional");
            //hostAdicional.Close();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new srvClienteAdic() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
