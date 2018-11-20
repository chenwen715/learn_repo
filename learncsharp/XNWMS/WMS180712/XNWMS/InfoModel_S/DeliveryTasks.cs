using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNWMS.InfoModel_S
{
    public class DeliveryTasks
    {
        public string taskNo { get; set; }
        public string taskType { get; set; }
        public List<string> SN { get; set; }
        public string OpTime { get; set; }
        public int state { get; set; }
    }
}
