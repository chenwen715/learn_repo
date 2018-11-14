using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateChange
{
    class SkipInfo
    {
        public string PointNo { set; get; }
        public string AreaNo { set; get; }
        public string Barcode { set; get; }
        public int skipInfo { set; get; }
        public int SkipDefault { set; get; }
        public string LockAgv { set; get; }
        public bool IsLocked { set; get; }
        public string AreaName { set; get; }
        public bool IsAllow { set; get; }
        public int Count { set; get; }
        public int flag { set; get; }
    }
}
