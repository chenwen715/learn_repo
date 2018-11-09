using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_T
{
    class Program
    {
        static void Main(string[] args)
        {
            Store s = new Store();
            int[] num = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] str = new string[]{ "a", "b" };
            s.StoreNum<int>(num);
            s.StoreNum<string>(str);
            Console.ReadKey();
        }
    }

    class Store
    {
        public void StoreNum<T>(T[] t)
        {
            T[] num=new T[10];
            int i = 0;
            foreach(T n in t)
            {
                Console.WriteLine(n);
                i++;
               
            }
            
        }
    }
}
