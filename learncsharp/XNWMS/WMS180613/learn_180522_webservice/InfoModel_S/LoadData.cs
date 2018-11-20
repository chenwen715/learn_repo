using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    class LoadData
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public string taskNo { get; set; }

        /// <summary>
        /// 托盘号/箱号
        /// </summary>
        public string palletOrBoxNo { get; set; }

        /// <summary>
        /// 保税号
        /// </summary>
        public string bdNo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string pNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// 数量明细
        /// </summary>
        public string numberDtl { get; set; }

        /// <summary>
        /// 是否超重
        /// </summary>
        public bool isOverWeight { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 时间（距当月第一天的秒数）
        /// </summary>
        public int times { get; set; }

        /// <summary>
        /// 是否恒温
        /// </summary>
        public bool isHengWen { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int state { get; set; }
        
    }
}
