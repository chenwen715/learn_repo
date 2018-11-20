using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNWMS.InfoModel_R;

namespace XNWMS
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
        public List<BoxModel> SN_BoxList { get; set; }


    }
}
