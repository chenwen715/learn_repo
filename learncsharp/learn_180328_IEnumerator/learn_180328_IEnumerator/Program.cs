using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_IEnumerator
{
    class Program:IEnumerable
    {

        string[] str = new string[] { "spring", "summer", "autum", "winter" };
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < str.Length; i++)
            {
                Season s = new Season();
                s.name = str[i];
                yield return s;
            }
        }
        static void Main(string[] args)
        {
            //List<Season> seasonList=new List<Season>();
            //string[] str = new string[] { "spring", "summer", "autum", "winter" };
            //for (int i = 0; i < str.Length; i++)
            //{
            //    Season s = new Season();
            //    s.name = str[i];
            //    seasonList.Add(s);
            //}

            Program pro = new Program();
            foreach (Season sea in pro)
            {
                Console.WriteLine(sea.name);
            }
            Console.ReadKey();

        }
    }
}
