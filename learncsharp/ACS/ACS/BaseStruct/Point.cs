using System;
using System.Collections.Generic;

namespace ACS
{
    public class Point : IComparable
    {
        /// <summary>
        /// 区域代码
        /// </summary>
        public string areaNo;
        /// <summary>
        /// 码值
        /// </summary>
        public string barCode;
        /// <summary>
        /// X坐标
        /// </summary>
        public int x;
        /// <summary>
        /// Y坐标
        /// </summary>
        public int y;
        /// <summary>
        /// X间距
        /// </summary>
        public int xLength;
        /// <summary>
        /// Y间距
        /// </summary>
        public int yLength;

        /// <summary>
        /// 固定方向，是否允许XY的正负方向。
        /// </summary>
        public bool isXPos;
        public bool isXNeg;
        public bool isYNeg;
        public bool isYPos;

        public Point xPosPoint;
        public Point xNegPoint;
        public Point yPosPoint;
        public Point yNegPoint;

        public List<TmpDirection> listTmpDirection;

        /// <summary>
        /// 点是否可用
        /// </summary>
        public bool isEnable;
        /// <summary>
        /// 点是否被占用
        /// </summary>
        public bool isOccupy;
        /// <summary>
        /// 点被占用的小车号（行走不置）
        /// </summary>
        public string occupyAgvNo;
        /// <summary>
        /// 点被申请的小车
        /// </summary>
        public Agv lockedAgv;
        /// <summary>
        /// 点属性
        /// </summary>
        public PointType pointType;

        public Point ParentPoint;
        public double Value_F;
        public double Value_G;
        public double Value_H;

        public int CompareTo(object obj)
        {
            Point p = obj as Point;
            if (p == null)
                throw new NotImplementedException();
            return Value_F.CompareTo(p.Value_F);
        }
    }

    public class TmpDirection
    {
        /// <summary>
        /// 临时锁的方向：1、x+;2、x-;3、y+;4、y-;
        /// </summary>
        public int direction;
        public string agvNo;
    }
}
