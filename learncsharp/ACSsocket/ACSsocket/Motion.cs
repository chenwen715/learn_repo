using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACSsocket
{
    public class Motion
    {
        /// <summary>
        /// 动作号
        /// </summary>
        public string sTaskNo;

        /// <summary>
        /// 动作类型
        /// </summary>
        //public STaskType sTaskType;
        public int sTaskType;

        /// <summary>
        /// 码值
        /// </summary>
        public string barcode;

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
        /// 点属性
        /// </summary>
        public int pointType;

        /// <summary>
        /// 点状态
        /// </summary>
        public int state;
        /// <summary>
        /// 车头方向
        /// </summary>
        public int OriAgv;
        /// <summary>
        /// 转盘方向
        /// </summary>
        public int OriDial;
        /// <summary>
        /// 防撞方向
        /// </summary>
        public int AntiCollision;
    }
}