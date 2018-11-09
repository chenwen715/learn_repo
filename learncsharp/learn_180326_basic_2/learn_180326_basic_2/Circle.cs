using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180326_basic_2
{
    class Circle
    {
        private const double pi = 3.14;
        public double r {get;set;}
        public double s;
        public void ComputeArea(double r)
        {
            s = pi * r * r;
        }
    }
}
