using System;
using System.Threading;
using System.Windows.Forms;

namespace DAY1
{
   class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
////            #region
////            int[] num = { 10, 9, 1, 2, 8, 7, 3, 4, 6, 5 };
////            foreach (int a in num)
////            {
////                Console.WriteLine(a);
////                Console.ReadKey();
////            }
////#endregion

//            Thread thread1 = new Thread(new ThreadStart(create1));
//            thread1.Priority = ThreadPriority.Lowest;
//            Thread thread2 = new Thread(new ThreadStart(create2));
//            thread2.Priority = ThreadPriority.Highest;
//            thread1.Start();
//            thread2.Start();
//            //printsentence(thread1,1);
//            //printsentence(thread2,2);
//            ////thread1.Abort();
//            //printsentence(thread1, 1);
//            //printsentence(thread2, 2);
//            //thread1.Abort();
//            //thread2.Abort();

        
        


//        private static void create1()
//        {
           
//            System.Console.Write("线程1"+"\n");  
            
//        }

//        private static void create2()
//        {

//            System.Console.Write("线程2" + "\n");

//        }

//        public static void printsentence(Thread th,int a)
//        {
//            string str = String.Empty;
//            str = th.ThreadState.ToString();
//            System.Console.Write(str+"\t"+a);
//            System.Console.ReadLine();
//        }
        
    }

}
