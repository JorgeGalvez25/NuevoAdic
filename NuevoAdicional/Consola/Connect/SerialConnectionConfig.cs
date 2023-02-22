using System.IO.Ports;

namespace Consola.Connect
{
    public class SerialConnectionConfig
    {
        public SerialConnectionConfig()
        {
            this.BaudRate = BaudRate.B9600;
            this.DataBits = 8;
            this.Parity = Parity.None;
            this.PortName = string.Empty;
            this.StopBits = StopBits.None;
        }
        public BaudRate BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public string PortName { get; set; }
        public StopBits StopBits { get; set; }
    }

    public enum BaudRate
    {
        None = 0,
        B2400 = 2400,
        B9600 = 9600,
        B14400 = 14400,
        B19200 = 19200,
        B28800 = 28800,
        B38400 = 38400,
        B57600 = 57600,
        B115200 = 115200,
        B230400 = 230400,
        B460800 = 460800,
        B921600 = 921600
    }
}
