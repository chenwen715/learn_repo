using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    public class Commond
    {
        public static bool IsClose = false;
        public static bool IsTest = true;

        public static string TestAgvNo = "";

        public static byte CRC(byte[] data, int length)
        {
            byte crc = (byte)(data[0] ^ data[1]);
            for (int i = 2; i < length - 1; i++)
            {
                crc = (byte)(crc ^ data[i]);
            }
            return crc;
        }
    }
}
