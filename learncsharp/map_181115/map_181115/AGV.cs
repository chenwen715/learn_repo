using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map_181115
{
    class AGV
    {
        //码值
        public string barcode;

        //x坐标
        public int x;

        //y坐标
        public int y;


        public AGV(string barcode, int x, int y)
        {
            // TODO: Complete member initialization
            this.barcode = barcode;
            this.x = x;
            this.y = y;
        }
    }
}
