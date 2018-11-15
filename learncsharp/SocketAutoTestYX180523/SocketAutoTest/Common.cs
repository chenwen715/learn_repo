using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    class Common
    {
        public static bool[] getBooleanArray(byte b)
        {
            bool[] array = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                array[i] = Convert.ToBoolean((b & 1));
                b = (byte)(b >> 1);
            }
            return array;
        }

        public static string getString(int[] num)
        {
            string str = "";
            if (num.Length != 8) return str;
            for (int i = 7; i >= 0; i--)
            {
                str += num[i];
            }
            return str;
        }
    }
}
