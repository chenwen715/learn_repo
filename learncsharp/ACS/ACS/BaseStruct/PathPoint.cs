using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    public class PathPoint : IComparable
    {
        public int SID;

        /// <summary>
        /// 路径点序号
        /// </summary>
        public int serialNo;
        
        /// <summary>
        /// 是否为拐点
        /// </summary>
        public bool isCorner;

        public Point point;

        public int CompareTo(object obj)
        {
            PathPoint p = obj as PathPoint;
            if (p == null)
                throw new NotImplementedException();
            return serialNo.CompareTo(p.serialNo);
        }
    }
}
