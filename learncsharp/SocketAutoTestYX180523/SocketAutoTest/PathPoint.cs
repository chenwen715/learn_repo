using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class PathPoint : IComparable
    {
        public string sid;

        /// <summary>
        /// 路径点序号
        /// </summary>
        public int routeNo;

        public Point point;

        public int CompareTo(object obj)
        {
            PathPoint p = obj as PathPoint;
            if (p == null)
                throw new NotImplementedException();
            return routeNo.CompareTo(p.routeNo);
        }
    }
}
