using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class AgvClass
    {
        /// <summary>
        /// 小车编号
        /// </summary>
        public string AgvName { get; set; }

        /// <summary>
        /// 小车类型：成品配送 = 1，导线桶配送 = 2
        /// </summary>
        public int AgvType { get; set; }

        /// <summary>
        /// 小车电量
        /// </summary>
        public float V { get; set; }

        /// <summary>
        /// 小车状态
        /// </summary>
        public Int16 State { get; set; }

        /// <summary>
        /// 小车是否载货
        /// </summary>
        public bool IsHaveGoods { get; set; }

        /// <summary>
        /// 小车错误信息
        /// </summary>
        public byte Error { get; set; }

        /// <summary>
        /// 小车走过的5个点的x、y值
        /// </summary>
        public byte X1 { get; set; }
        public byte Y1 { get; set; }
        public byte X2 { get; set; }
        public byte Y2 { get; set; }
        public byte X3 { get; set; }
        public byte Y3 { get; set; }
        public byte X4 { get; set; }
        public byte Y4 { get; set; }
        public byte X5 { get; set; }
        public byte Y5 { get; set; }

        /// <summary>
        /// 小车发送的动作类型
        /// 申请路口 = 1,释放路口 = 2,申请小车左侧上料 = 3,小车上料完成 = 4,申请小车左侧下料 = 5,
        /// 小车下料完成 = 6,已开始充电 = 7,已结束充电 = 8,已回家 = 9,
        /// </summary>
        public byte actionType { get; set; }

        /// <summary>
        /// 站点性质
        /// </summary>
        public byte stationType { get; set; }

        /// <summary>
        /// 站点序号
        /// </summary>
        public byte stationNo { get; set; }


        public byte[] ReturnBytes { get; set; }
        public byte[] ReSend { get; set; }
        public byte[] LastReturnBytes { get; set; }

        /// <summary>
        /// 小车子任务号
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// 小车当前路径序号
        /// </summary>
        public int RouteNo { get; set; }

        /// <summary>
        /// 小车当前码值，为小车发送的走过的5个点的第一个点，数据库中为strBarcode
        /// </summary>
        public string CurrentBarCode { get; set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }

        /// <summary>
        /// 小车任务终点
        /// </summary>
        public int DestinationNo { get; set; }

        /// <summary>
        /// 小车任务类型
        /// 小车下料 = 1,小车上料 = 2,充电 = 3,取消充电 = 4,小车下料故障恢复 = 5,小车上料故障恢复 = 6,回家 = 7
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 小车总路径数
        /// </summary>
        public int PathNo { get; set; }

        /// <summary>
        /// 小车路径点
        /// </summary>
        public List<PathPoint> pathList;

        /// <summary>
        /// 小车充电桩
        /// </summary>
        public int ChargeStation { get; set; }

        /// <summary>
        /// 小车是否可用
        /// </summary>
        public bool IsEnable { get; set; }

        public int TimeCount { get; set; }
        /// <summary>
        /// 初始化为0，到达站点发过心跳后变为1，之后发申请
        /// </summary>
        public int flag1 { get; set; }
        public int flag2 { get; set; }

        public Point lastPoint { get; set; }

        public DateTime ErrorTime { get; set; }
        public int ErrorWait()
        {
            TimeSpan ts1 = new TimeSpan(ErrorTime.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts3 = ts2.Subtract(ts1).Duration();
            return ts3.Minutes * 60 + ts3.Seconds;
        }
    }
}
