
namespace Adicional.Entidades
{
    public class StateObject
    {
        public StateObject()
        {
            this.WorkSocket = null;
            this.Buffer = null;
            this.BytesReceived = 0;
            this.OffSet = 0;
        }

        public System.Net.Sockets.Socket WorkSocket;

        public byte[] Buffer;

        public int BytesReceived;

        public int OffSet;
    }
}
