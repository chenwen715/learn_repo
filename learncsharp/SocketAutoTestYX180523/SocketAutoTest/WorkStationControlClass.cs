using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class WorkStationControlClass
    {
        public string WorkStationControlName { get; set; }
        public List<WorkStation> ws { get; set; }

        public byte[] ReturnBytes { get; set; }
        public byte[] ReSend { get; set; }
        public byte[] LastReturnBytes { get; set; }

    }
}
