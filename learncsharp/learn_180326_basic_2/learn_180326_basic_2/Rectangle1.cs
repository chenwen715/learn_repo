using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180326_basic_2
{
    class Rectangle1:Rectangle
    {
        public double s;
        public void ComputeArea()
        {
            s = width * length;
        }
    }
}
