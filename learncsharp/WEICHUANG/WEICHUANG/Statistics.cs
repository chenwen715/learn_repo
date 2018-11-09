using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEICHUANG
{
    class Statistics
    {
        //时间
        public string DTime { get; set; }

        //该时间段数据总条数
        public int dataCount { get; set; }

        //该时间段料号总条数
        public int totalmaterialCount { get; set; }

        //该时间段大件料号总条数
        public int lmaterialCount { get; set; }

        //该时间段小件料号总条数
        public int smaterialCount { get; set; }

        //该时间段缺少的料号总条数
        public int lostmaterialCount { get; set; }

        //该时间段整托上架任务总条数
        public int palletCount { get; set; }

        //该时间段散件上架任务总条数
        public int boxCount { get; set; }

        //该时间段散件上架任务总条数（大件）
        public int lboxCount { get; set; }

        //该时间段散件上架任务总条数（小件）
        public int sboxCount { get; set; }

        //该时间段上架任务总条数
        public int totalTaskCount { get; set; }
    }
}
