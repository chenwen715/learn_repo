using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_partial
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.Print1();
            a.Print2();
            Console.ReadKey();
        }
    }
    partial class A
    {
        public void Print1()
        {
            Console.WriteLine("1988");
        }
    }
    partial class A
    {
        public void Print2()
        {
            Console.WriteLine("good");
        }
    }
}
