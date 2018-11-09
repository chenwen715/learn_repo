using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180412_tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            //BitArray arr = new BitArray(BitConverter.GetBytes(4));
            //for (int j = 0; j < 8; j++)
            //{
            //    System.Console.WriteLine(j + ":" + arr[j]);
            //}
            //System.Console.ReadKey();
            //System.Console.WriteLine(byteToBit(4));
            //System.Console.ReadKey();
            //byte[] a = Encoding.ASCII.GetBytes(byteToBit(4));
            //for (int j = 0; j < 8; j++)
            //{
            //    System.Console.WriteLine(j + ":" + a[7 - j]);
            //}
            //Point p = new Point();
            //p.point = "0234";
            //System.Console.WriteLine(p.x+" "+p.y);
            //System.Console.ReadKey();
            //string a = "(13,2)";
            ////string[] b = a.Split(new char[] { '(', ',', ')' });
            //string[] b = a.Substring(a.IndexOf('(')+1,(a.IndexOf(')')-a.IndexOf('(')-1)).Split(new char[] {  ',' });
            //System.Console.WriteLine(b[2]);
            //System.Console.ReadKey();
            //string b = HexStringToASCII("53544B3030310000000000000000000000000000");
            //int a = Convert.ToInt16(b, 16);
            //string a = "0000000000005058323031383036313130303031";
            //System.Console.WriteLine(byteToHexStrBarcodeShow(HexStringToBytes(a), 0, HexStringToBytes(a).Length));
            //System.Console.ReadKey();
            int C = 1184 / 20;
            int d = C / 30;
            string bb = getmd("6/10 17:30");

            double a = weightDataChange((double)11.699999999999);
            TimeChange(130925);
            //int[] a = new int[] { 1, 2, 3, 4, 5, 6 };
            //Console.WriteLine(Array.IndexOf(a, 5));
            //System.Console.ReadKey();
            System.Console.WriteLine(6 / 5);
            System.Console.WriteLine(8 / 5);
        }

        private static string getmd(string p)
        {
            return "18" + p.Split('/')[0].PadLeft(2, '0') + p.Split('/')[1].Split(' ')[0].PadLeft(2, '0') + "000001";
        }

        public static String byteToBit(byte b)
        {
            return ""
                    + (byte)((b >> 7) & 0x1) + (byte)((b >> 6) & 0x1)
                    + (byte)((b >> 5) & 0x1) + (byte)((b >> 4) & 0x1)
                    + (byte)((b >> 3) & 0x1) + (byte)((b >> 2) & 0x1)
                    + (byte)((b >> 1) & 0x1) + (byte)((b >> 0) & 0x1);
        }


        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {

            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {

            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        public static string byteToHexStrBarcode(byte[] bytes, int begin, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = begin; i < len + begin; i++)
                {
                    string ss = bytes[i].ToString("X1");

                    if (returnStr == "")
                    {
                        if (ss == "0")
                            continue;
                    }

                    string ggo = HexStringToASCII(ss);
                    returnStr += ggo;
                }
            }

            if (returnStr == "")
                returnStr = "0";
            return returnStr;
        }

        public static byte[] HexStringToBytes(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return new byte[0];
            }

            if (hexStr.StartsWith("0x"))
            {
                hexStr = hexStr.Remove(0, 2);
            }

            var count = hexStr.Length;

            if (count % 2 == 1)
            {
                throw new ArgumentException("Invalid length of bytes:" + count);
            }

            var byteCount = count / 2;
            var result = new byte[byteCount];
            for (int index = 0; index < byteCount; ++index)
            {
                var tempBytes = Byte.Parse(hexStr.Substring(2 * index, 2), System.Globalization.NumberStyles.HexNumber);
                result[index] = tempBytes;
            }

            return result;
        }

        public static string byteToHexStrBarcodeShow(byte[] bytes, int begin, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = begin; i < len + begin; i++)
                {
                    string ss = bytes[i].ToString("X1");
                    if (ss == "0")
                        continue;
                    string ggo = HexStringToASCII(ss);
                    returnStr += ggo;
                }
            }

            if (returnStr == "")
                returnStr = "0";
            return returnStr;
        }

        public static void TimeChange(int time)
        {
            int day = time / 86400;
            int hour = (time - day * 86400) / 3600;
            int min = (time - day * 86400-hour*3600) / 60;
            int sec = time - day * 86400 - hour * 3600 - min * 60;
            string now=DateTime.Now.ToString();
            int a=(DateTime.Now.Day-1)*86400+DateTime.Now.Hour*3600+DateTime.Now.Minute*60+DateTime.Now.Second;
            DateTime d1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            TimeSpan t = new TimeSpan(day, hour, min, sec);
            string time1 = d1.Add(t).ToString();
            System.Console.WriteLine(a);
            System.Console.WriteLine(time1);
            System.Console.ReadKey();

        }

        public static double weightDataChange(double number)
        {
            
            string nstring = number.ToString();
            double result = number;
            if (nstring.Contains('.'))
            {
                int dot_index = nstring.IndexOf('.');
                int nine_index = nstring.IndexOf("999");
                int zero_index = nstring.IndexOf("000");
                string zs = nstring.Split('.')[0];
                string xs = nstring.Split('.')[1];
                if (nine_index != -1 && nine_index > dot_index)
                {

                    result = Math.Round(number, (nine_index - zs.Length - 1));
                }
                else if (zero_index != -1 && zero_index > dot_index)
                {
                    result = Math.Round(number, (zero_index - zs.Length - 1));
                }
            }
            return result;
        }
    } 
}
