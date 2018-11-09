using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180308
{
    class Program
    {
        static void Main(string[] args)
        {
            //int a = 3, b = 6;
            //int c = a | b;            
            //Console.WriteLine(c);
            //Console.ReadLine();

            //string s = "abc";
            //string s1 = "ABC";
            //int r = String.Compare(s,s1,false);
            //Console.WriteLine(r);
            //Console.ReadLine();

            //DateTime dt = DateTime.Now;
            //Console.WriteLine(dt);
            //Console.WriteLine("{0:d}", dt);
            //Console.ReadLine();
            


        //    int s = 0, num = 80;
        //    while(s<num){
        //        s++;
        //        if (s > 40)
        //        {
        //            break;
        //        }
        //        if (s % 2 == 0)
        //        {
                    
        //            continue;
        //        }
        //          Console.WriteLine(s); 
        //    }
        //    Console.ReadLine();

           // int i = 1, b = 1, c,d=0;
           // while (d < 30)
           // {
                
           //     if (d ==0 || d == 1)
           //     {
           //         c = 1;
           //         d++;
           //         Console.WriteLine("第{0}个数为{1}",d,c);
                    
           //         continue;
           //     }
           //     c = i + b;
           //     i = b;
           //     b = c;
           //     d++;
           //     Console.WriteLine("第{0}个数为{1}", d, c);
               
           // }
           //Console.ReadLine();
            p67();
        }

        #region
        public static void p67()
        {
            Application.Run(new Form1());
        }
        #endregion

        #region
        public static void p103()
        {
            int[,] a = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int[,] b = new int[3, 3];
            Console.WriteLine("before");

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(a[i, j] + " ");

                }
                Console.WriteLine();
            }

            Console.WriteLine("after");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    b[i, j] = a[j, i];
                    Console.Write(b[i, j]+" ");

                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        #endregion

        #region
        public static void p104()
        {
            string[,] a = new string[9, 4];
            int c=0;
            for (int i = 0; i < 9; i++)
            {
                    for (int j = 0; j < 4; j++)
                {
                    a[i,j]="【有票】";
                    Console.Write("{0}行{1}列{2}号{3}",i+1,j+1,(i*4+j+1),a[i, j] + " ");
                }
                    Console.WriteLine();
                }
                while (c == 0)
                {
                    Console.WriteLine("请以【a,b】格式输入坐标：若结束则输入“e”");
                    string zuoBiao = Console.ReadLine();
                    if (zuoBiao == "e")
                    {
                        c = 1;
                        Console.Write("结束，按任意键退出");
                        break;
                    }
                    if (zuoBiao.Split(new char[] { ',' }).Length != 2)
                    {
                        Console.WriteLine("输入不正确，请重新输入");
                        continue;
                    }
                    int a1 = Convert.ToInt32(zuoBiao.Split(new char[] { ',' })[0]);
                    int a2 = Convert.ToInt32(zuoBiao.Split(new char[] { ',' })[1]);
                    if (a1 > 9 || a2 > 4)
                    {
                        Console.WriteLine("输入值超过范围，请重新输入");
                        continue;
                    }
                    a[a1 - 1, a2 - 1] = "【已售】";
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {

                            Console.Write("{0}行{1}列{2}号{3}", i + 1, j + 1, (i * 4 + j + 1), a[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                
                }
                Console.ReadLine();

        }
        #endregion


    }
}
