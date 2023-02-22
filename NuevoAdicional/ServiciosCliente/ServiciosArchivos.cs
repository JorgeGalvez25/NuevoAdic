using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiciosCliente
{
    public class ServiciosArchivos : IServiciosGenArchivos
    {
        public RemObjects.SDK.BinMessage message;
        public RemObjects.SDK.IpHttpClientChannel clientChannel;
        
        private void InicilizaServiciosArchivos()
        {
            this.message = new RemObjects.SDK.BinMessage();
            this.clientChannel = new RemObjects.SDK.IpHttpClientChannel();

            this.clientChannel.Password = "";
            this.clientChannel.UserName = "";
            this.clientChannel.TargetUrl = "http://127.0.0.1:8031/bin";
            //string servidor = (string)System.Configuration.ConfigurationSettings.AppSettings["ServidorMaster"];
            //this.clientChannel.TargetUrl = servidor;

            this.message.ContentType = "application/octet-stream";
            this.message.SerializerInstance = null;
        }

        public ServiciosArchivos()
        {
            InicilizaServiciosArchivos();
        }

        public int Sum(int A, int B)
        {
            throw new NotImplementedException();
        }

        public DateTime GetServerTime()
        {
            throw new NotImplementedException();
        }

        public bool SetRegenerarArchivosVolumetricos(DateTime AFecha, int ACorte, out string AMensajeError)
        {
            AMensajeError = string.Empty;
            IServiciosGenArchivos servicio = (IServiciosGenArchivos)CoServiciosGenArchivos.Create(message, clientChannel);
            return servicio.SetRegenerarArchivosVolumetricos(AFecha, ACorte, out AMensajeError);
        }
    }
}
