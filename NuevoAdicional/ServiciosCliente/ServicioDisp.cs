using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiciosCliente
{
    public class ServicioDisp
    {
        public RemObjects.SDK.BinMessage message;
        public RemObjects.SDK.IpHttpClientChannel clientChannel;

        public ServicioDisp(string servidor)
        {
            this.message = new RemObjects.SDK.BinMessage();
            this.clientChannel = new RemObjects.SDK.IpHttpClientChannel();

            this.clientChannel.Password = "";
            this.clientChannel.UserName = "";
            this.clientChannel.TargetUrl = servidor;
            this.message.ContentType = "application/octet-stream";
            this.message.SerializerInstance = null;
        }

        public string GetEstadoPosiciones()
        {
            ISrvDispensarios servicio = CoSrvDispensarios.Create(message, clientChannel);
            return servicio.DameDispensarios();
        }

        public string EjecutaComando(string cmd)
        {
            ISrvDispensarios servicio = CoSrvDispensarios.Create(message, clientChannel);
            int folio = servicio.EjecutaComando(cmd);
            if (cmd != "CERRAR" && cmd != "FLUMIN")
            {
                string resp;
                for (int i = 1; i <= 120; i++)
                {
                    System.Threading.Thread.Sleep(350);

                    resp = servicio.ResultadoComando(folio);
                    if (resp != "*")
                        return resp;
                }
            }
            else
                return "OK";
            return "Sin respuesta";
        }
    }
}
