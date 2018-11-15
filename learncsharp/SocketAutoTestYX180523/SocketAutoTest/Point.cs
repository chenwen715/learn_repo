using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class Point
    {
        public int pBarcode
        {
            set
            {
                pBarcode =value;
            }
            get
            {
                return int.Parse(currentX.ToString() + currentY.ToString());
            }
        }
        /// <summary>
        /// 点在地图上的横坐标x
        /// </summary>
        public int mapX;

        /// <summary>
        /// 点在地图上的纵坐标y
        /// </summary>
        public int mapY;

        /// <summary>
        /// 点在CAD图上的横坐标x
        /// </summary>
        public int currentX;

        /// <summary>
        /// 点在CAD图上的纵坐标y
        /// </summary>
        public int currentY;

        
        //public int pointType;

        /// <summary>
        /// 点属性
        /// </summary>
        public int pointDescription;
        //public int pointNo;

        /// <summary>
        /// 点处的站台号，无站台为0
        /// </summary>
        public int intStationCode;

        /// <summary>
        /// 点处的料台是上料还是下料：1、上料台；2、下料台；其余为0
        /// </summary>
        public int wsType;

        /// <summary>
        /// 点在CAD图上的当前点的前一点x
        /// </summary>
        public int beforex;

        /// <summary>
        /// 点在CAD图上的当前点的前一点y
        /// </summary>
        public int beforey;

        /// <summary>
        /// 是否被占用
        /// </summary>
        public bool isOccuppy;

        /// <summary>
        /// 占用的小车
        /// </summary>
        public string occupyAgv;
    }
}
