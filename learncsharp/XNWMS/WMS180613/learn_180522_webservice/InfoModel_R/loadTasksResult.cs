using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    //整板上架任务结果
    class loadTasksResult
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
        /// 目标仓位
        /// </summary>
        public string hubno { get; set; }

        /// <summary>
        /// 货架
        /// </summary>
        public string shelfNo { get; set; }

        /// <summary>
        /// 是否是托盘
        /// </summary>
        public bool isPallet { get; set; }

        /// <summary>
        /// 异常所在位置  1：堆垛机   2：入库采集区
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 整托上架异常原因
        /// </summary>
        public string remark { get; set; }
    }
}
