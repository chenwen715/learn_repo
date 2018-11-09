using N1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180306
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.CountAge();
           
        }
    }
}
namespace N1
{
    class A
    {
        public void CountAge()//方法名首字母大写，方法参数首字母小写
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int a = 1; a < 6; a++)
                {
                    int b, multiAge;
                    int[] n = new int[1];
                    b = 13 - a - i;
                    multiAge = a * b * i;
                    if (multiAge > 24 && multiAge < 65 && b > 5)
                    {
                        //Console.WriteLine("age: " + i + " 、 " + a + " 、 " + b+ " 、 "+multiAge);
                        Console.WriteLine("children age:{0}、{1}、{2}，father age:{3} ",i , a , b ,multiAge);
                    }
                }
            }
            Console.ReadLine();
        }
    }
    class Car
    {
        string carColor;//变量首字母小写

    }
}