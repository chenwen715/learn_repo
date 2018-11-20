using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    //下架站点流转信息
    class stationInfo
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public string taskNo { get; set; }

        /// <summary>
        /// 托盘号/箱号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 仓位（对应流转的各站点仓位）
        /// </summary>
        public string hubno { get; set; }

        /// <summary>
        /// 一楼出货落地提升机出口（仅流转到一楼出货区时需要该字段，到达一楼出货落地提升机出口最前面一个暂存区才发送。） 
        /// </summary>
        public string outPort { get; set; }
    }
}
