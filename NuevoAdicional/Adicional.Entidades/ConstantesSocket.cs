using System;
using System.Configuration;
using System.Text;

namespace Adicional.Entidades
{
    public class ConstantesSocket
    {
        public static readonly Encoding LOCAL_ENCODING = Encoding.UTF8;

        public static int MAX_BUFFER_SIZE
        {
            get
            {
                // Si no existe la configuracion en IGas Servicios
                int max = 0;
                string strMax = ConfigurationManager.AppSettings["maxBuffSize"] ?? "1048576";
                if (!int.TryParse(strMax, out max))
                {
                    max = 1048576;// 1 Mega Mordida (MegaByte) XD
                }
                return max;
            }
        }

        public static int MAX_TIME_OUT
        {
            get
            {
                int def = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
                int max = 0;

                string strMax = ConfigurationManager.AppSettings["maxTimeout"] ?? def.ToString();

                if (!int.TryParse(strMax, out max))
                {
                    max = def;
                }
                return max;
            }
        }

        // http://www.december.com/html/spec/ascii.html

        public const byte NULL_BYTE = 0x00;

        public const byte SOH_BYTE = 0x01;

        public const byte STX_BYTE = 0x02;

        public const byte ETX_BYTE = 0x03;

        public const byte EOT_BYTE = 0x04;

        public const byte ENQ_BYTE = 0x05;

        public const byte ACK_BYTE = 0x06;

        public const byte BEL_BYTE = 0x07;

        public const byte BS_BYTE = 0x08;

        public const byte HT_BYTE = 0x09;

        public const byte NL_BYTE = 0x0A;

        public const byte VT_BYTE = 0x0B;

        public const byte NP_BYTE = 0x0C;

        public const byte CR_BYTE = 0x0D;

        public const byte SO_BYTE = 0x0E;

        public const byte SI_BYTE = 0x0F;

        public const byte DLE_BYTE = 0x10;

        public const byte DC1_BYTE = 0x11;

        public const byte DC2_BYTE = 0x12;

        public const byte DC3_BYTE = 0x13;

        public const byte DC4_BYTE = 0x14;

        public const byte NAK_BYTE = 0x15;

        public const byte SYN_BYTE = 0x16;

        public const byte ETB_BYTE = 0x17;

        public const byte CAN_BYTE = 0x18;

        public const byte EM_BYTE = 0x19;

        public const byte SUB_BYTE = 0x1A;

        public const byte ESC_BYTE = 0x1B;

        public const byte FS_BYTE = 0x1C;

        public const byte GS_BYTE = 0x1D;

        public const byte RS_BYTE = 0x1E;

        public const byte US_BYTE = 0x1F;

        public const byte SP_BYTE = 0x20;
    }
}
