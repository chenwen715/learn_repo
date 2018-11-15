using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class WorkStation
    {
        public int wsNo { get; set; }
        public string wsControlName { get; set; }
        public int wsType { get; set; }
        public bool IsLoadingWork { get; set; }
        public bool IsLoadingFinish { get; set; }
        public bool IsUnloadingRequest { get; set; }
        public bool IsUnloadingAreaHaveGoods { get; set; }
        public bool IsloadingAreaHaveGoods { get; set; }
        public bool IsError { get; set; }
        public int LoadingCount { get; set; }
        public int UnLoadingCount { get; set; }

    }
}
