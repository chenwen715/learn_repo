using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Class1
    {
        static void Main(string[] args)
        {
            List<String> testList = new List<String>();
            for (int i = 1; i < 13; i++)
            {
                string index = "[FCFC";
                index += Convert.ToString(1, 16).PadLeft(4, '0');
                index += Convert.ToString(i, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(60, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(40, '0');
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(16, '0'); //69为被转值
                index += "FDFD]";
                testList.Add(index);
            }
            foreach (String a in testList)
            {
                System.Console.WriteLine(a);
            }
            System.Console.ReadKey();
            
        }

        private static object StrToHex(int p)
        {
            throw new NotImplementedException();
        }
        public string HexToStr(string mHex) // 返回十六进制代表的字符串
        {
            mHex = mHex.Replace(" ", "");
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        }
        public string StrToHex(string mStr) //返回处理后的十六进制字符串
        {
            return BitConverter.ToString(
            ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", "");
        }

        public static object index { get; set; }
    }
}
