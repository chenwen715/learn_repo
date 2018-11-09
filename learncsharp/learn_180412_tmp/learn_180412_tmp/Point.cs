using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180412_tmp
{
    public class Point
    {
        public string point;
        public int x
        {
            get
            {
                return int.Parse(point.PadLeft(4, '0').Substring(0, 2));
            }
            set
            {
                x = value;
            }
        }
        public int y
        {
            get
            {
                return int.Parse(point.PadLeft(4, '0').Substring(2, 2));
            }
            set
            {
                y = value;
            }
        }
        public int pointType;
        public int pointDescription;
        public int pointNo;
    }
}
