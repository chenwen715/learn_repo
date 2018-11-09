using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180307
{
    class Program
    {
        static void Main(string[] args)
        {
            //int k = (int)DateTime.Now.DayOfWeek;
            //Console.WriteLine(k);
            //Console.ReadKey();

            //string a = "4.5";
            //double b = Convert.ToDouble(a);
            //Console.WriteLine(b);
            //Console.ReadKey();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
