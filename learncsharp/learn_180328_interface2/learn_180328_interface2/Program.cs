using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_interface2
{
    class Program:Rectangle
    {
        public double length { get; set; }
        public double width { get; set; }
        public double area { get; set; }
        public void ComputeArea()
        {
            area = width * length;
        }
        static void Main(string[] args)
        {
            Program pro = new Program();
            Rectangle rec = pro;
            rec.length = 2;
            rec.width = 5;
            rec.ComputeArea();
            Console.WriteLine(rec.area);
            Console.ReadKey();
        }
    }
}
