using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180328_file
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //WriteLog.createFileInfo(@"D:\xuex\learn_180328_file\log", "1234.txt");
            //Application.Run(new Form1());
            WriteLog.LogWrite(@"D:\xuex\learn_180328_file\log\123.txt", "ddd");
        }
    }
}
