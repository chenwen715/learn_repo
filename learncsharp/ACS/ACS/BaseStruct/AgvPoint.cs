using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    public class AgvPoint
    {
        public string agv;
        public Point point;
        public AgvPoint(string a,Point p)
        {
            agv = a;
            point = p;
        }
    }
}
