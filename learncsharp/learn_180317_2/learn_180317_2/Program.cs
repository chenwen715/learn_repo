using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180317_2
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] a = new int[] {1,2};
            //System.Console.WriteLine("原始:");
            //foreach (int num in a)
            //{
            //    System.Console.Write(num);
            //}
            //Console.ReadLine();
            //int[] b=new int[]{3,4,5,6};
            //int[] c = new int[1024];
            //c=AddContent(a, 8, b);
            //System.Console.WriteLine("添加后:");
            //foreach (int num in c)
            //{
            //    System.Console.Write(num);
            //}
            //Console.ReadLine();


            //int[] a = new int[] { 1, 2 ,7,9,5,4};
            //System.Console.WriteLine("原始:");
            //foreach (int num in a)
            //{
            //    System.Console.Write(num);
            //}
            //Console.ReadLine();
            ////Array.Sort(a);
            ////Array.Reverse(a);
            //a=DaoXu(a);
            //foreach (int num in a)
            //{
            //    System.Console.Write(num);
            //}
            //Console.ReadLine();

           
            //Record();


            //string str = "C:\\Users\\Public\\Pictures\\Sample Pictures\\k.jpg";
            //string str1 = str.Substring(0, str.Substring(0, str.LastIndexOf("\\")).LastIndexOf("\\"));
            //str1 += @"\k.jpg";  //@后的字符串不转义直接用
            //System.Console.Write(str1);
            //Console.ReadLine();


            Hashtable hashTable = new Hashtable();
            hashTable.Add("name", "suzy");
            hashTable.Add("age", 18);
            hashTable.Add("weight", 48);
            //if (hashTable.ContainsKey("age"))
            //{
                System.Console.Write(hashTable["age"]);
            //}
            Console.ReadLine();
        }
        static int[] AddContent(int[] a, int index, int[] b)
        {
            int num = a.Length + b.Length;
            int[] c = new int[num];
            if (index > a.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    c[i] = a[i];
                }
                for (int j = a.Length; j < num; j++)
                {
                    c[j] = b[j - a.Length];
                }
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    c[i] = a[i];
                }
                for (int j = index; j < (index + b.Length); j++)
                {
                    c[j] = b[j - index];
                }
                for (int z = index + b.Length; z < num; z++)
                {
                    c[z] = a[z - b.Length];
                }
            }
            return c;
        }

        static int[] DaoXu(int[] a)
        {
            int[] b=new int[a.Length];
            Array.Sort(a);
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = a[a.Length - i-1];
            }
            a = b;
            return a;
           
        }

        static void Record()
        {
            ArrayList list = new ArrayList();
            list.Add("小王 男 1980-03-08");
            list.Add("小刘 女 1981-06-08");
            string[] str=new string[]{"小照 男 1982-03-08","小里 女 1983-06-08"};
            list.InsertRange(2,str);
            foreach (string a in list)
            {
                Console.WriteLine(a);
            }
            Console.ReadLine();
        }
    }
}
