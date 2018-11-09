using System.Collections.Generic;

namespace ACS
{
    public class STask
    {
        /// <summary>
        /// 子任务排序
        /// </summary>
        public int serialNo;

        /// <summary>
        /// 子任务号
        /// </summary>
        public int sID;
        
        /// <summary>
        /// 主任务号
        /// </summary>
        public string taskNo;
        
        /// <summary>
        /// 子任务任务类型
        /// </summary>
        public STaskType sTaskType;

        /// <summary>
        /// 子任务小车信息
        /// </summary>
        public Agv agv;
        
        /// <summary>
        /// 子任务起点信息
        /// </summary>
        public Point beginPoint;
        
        /// <summary>
        /// 子任务终点信息
        /// </summary>
        public Point endPoint;

        /// <summary>
        /// 转盘转向度数：0：不转向。1：转向90.2:转180.3：转270
        /// </summary>
        public int _DialDirection;

        /// <summary>
        /// 方向。1：不转向。2、3、4转向。
        /// </summary>
        public int _AgvDirection;

        
        /// <summary>
        /// 子任务路径点集合
        /// </summary>
        public List<PathPoint> pathList;
        
        /// <summary>
        /// 是否旋转
        /// </summary>
        public bool notCircle;
        
        /// <summary>
        /// 子任务终点方向
        /// </summary>
        public int dialDirection;

        /// <summary>
        /// 转盘方向
        /// </summary>
        public int agvDirection;

        /// <summary>
        /// 子任务任务状态
        /// 0:初始化；1：已下载；2：已分配路径；3：已执行；99：已完成
        /// </summary>
        public TaskState state;
    }
}
