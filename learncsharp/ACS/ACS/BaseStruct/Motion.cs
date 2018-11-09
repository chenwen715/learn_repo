using System.ComponentModel;

namespace ACS
{
    public class Motion
    {
        /// <summary>
        /// 动作类型
        /// </summary>
        public STaskType sTaskType;
       
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
    }
}
