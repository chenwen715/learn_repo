using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180327_basic
{
    class Program
    {
        static void Main(string[] args)
        {
            //A a = new A();//无static时需要先实例化对象
            //A.PrintOut();
            //A a = new A();
           
            people p = new people();
            p.SetAge(10);
            Console.WriteLine(p.GetName()+"的年龄为"+p.age);
            Console.ReadKey();
        }
    }

    class A
    {
        public A()//构造函数
        {
            Console.WriteLine("attention");
            PrintOut();
        }
        public static void PrintOut()
        {
            Console.WriteLine("this is a method sample");
        }

        
    }
}
