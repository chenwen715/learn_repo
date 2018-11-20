using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    //出货暂存区下架任务结果
    class deliveryTasksResult
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public string taskNo { get; set; }

        /// <summary>
        /// 任务类型    bind：并板下架   load：装车下架
        /// </summary>
        public string taskType { get; set; }

        /// <summary>
        /// 托盘号/箱号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 拼托区仓位/出货区
        /// </summary>
        public string hubno { get; set; }

    }
}
