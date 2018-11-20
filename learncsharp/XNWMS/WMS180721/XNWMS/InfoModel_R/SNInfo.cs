using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS
{
    //材积重量信息
    class SNInfo
    {

        /// <summary>
        /// 托盘号/箱号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 保税号
        /// </summary>
        public string BDNO { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public float length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public float width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public float height { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public float weight { get; set; }
    }
}
