using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    class PNPB
    {
        /// <summary>
        /// 料号
        /// </summary>
        public string pNo { get; set; }

        /// <summary>
        /// 最小单包
        /// </summary>
        public int miniPerBag { get; set; }

        /// <summary>
        /// 状态 0：未写入数据库 1：已写入数据库
        /// </summary>
        public int state { get; set; }
    }
}
