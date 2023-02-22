using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EstadoDispensarios
{
    public class ServicioDisp
    {
        public RemObjects.SDK.BinMessage message;
        public RemObjects.SDK.IpHttpClientChannel clientChannel;

        public ServicioDisp()
        {
                this.message = new RemObjects.SDK.BinMessage();
                this.clientChannel = new RemObjects.SDK.IpHttpClientChannel();

                this.clientChannel.Password = "";
                this.clientChannel.UserName = "";
                string servidor = (string)System.Configuration.ConfigurationSettings.AppSettings["ServidorFACELE"];
                this.clientChannel.TargetUrl = servidor;
                this.message.ContentType = "application/octet-stream";
                this.message.SerializerInstance = null;
        }

        public string GetComprobanteElectronicoExistente(int AEstacion, string ASerie, int AFolio)
        {
            ISrvDispensarios servicio = (ISrvDispensarios)CoSrvDispensarios.Create(message, clientChannel);
            return servicio.DameDispensarios();
        }
    }
}
