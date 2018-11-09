using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_interface2
{
    interface Rectangle
    {
        double length { get; set; }
        double width { get; set; }
        double area { get; set; }
        void ComputeArea();
    }
}
