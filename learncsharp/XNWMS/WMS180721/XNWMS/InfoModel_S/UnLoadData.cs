using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    class UnLoadData
    {
        /// <summary>
        /// 从表格读取数据的标志，一行一个值
        /// </summary>
        public string sign { get; set; } 

        /// <summary>
        /// 任务号
        /// </summary>
        public string taskNo { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string taskType { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string taskState { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// 出仓号
        /// </summary>
        public string outNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string planningNo { get; set; }

        /// <summary>
        /// 托盘号/箱号
        /// </summary>
        public string palletOrBoxNo { get; set; }

        /// <summary>
        /// 仓位
        /// </summary>
        public string hubno { get; set; }

        /// <summary>
        /// 是否拆托
        /// </summary>
        public bool isUnpackTray { get; set; }

        /// <summary>
        /// 是否外箱制标
        /// </summary>
        public bool isBoxLable { get; set; }

        /// <summary>
        /// 是否最小包装制标
        /// </summary>
        public bool isMinpackLable { get; set; }

        /// <summary>
        /// 是否流水线制标
        /// </summary>
        public bool isPipeline { get; set; }

        /// <summary>
        /// 原始托盘号/箱号
        /// </summary>
        public string SN_OLD { get; set; }

        /// <summary>
        /// 增值服务楼层信息
        /// </summary>
        public string station { get; set; }

        /// <summary>
        /// 保税号
        /// </summary>
        public string bdNo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string pNo { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int outNumber { get; set; }

        /// <summary>
        /// 最小单包
        /// </summary>
        public int miniPerBag { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string businessType { get; set; }

        /// <summary>
        /// 时间（距当月第一天的秒数）
        /// </summary>
        public int times { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 任务编号，唯一标识
        /// </summary>
        public int id { get; set; }
        
    }
}
