using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    //码托信息
    class palletPackInfo
    {
        /// <summary>
        /// 托盘号
        /// </summary>
        public string SN_Pallet { get; set; }

        /// <summary>
        /// 散箱号
        /// </summary>
        public List<string> SN_BoxList { get; set; }


    }
}
