using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180326_basic_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Circle c = new Circle();
            c.r = 2;
            c.ComputeArea(c.r);
            Console.WriteLine(c.s);
            Console.ReadKey();
            Rectangle1 re = new Rectangle1();
            re.length = 5;
            re.width = 4;
            re.ComputeArea();
            Console.WriteLine(re.s);
            Console.ReadKey();
        }
    }
}
