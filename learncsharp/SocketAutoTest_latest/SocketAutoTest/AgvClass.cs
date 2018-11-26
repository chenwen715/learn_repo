using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAutoTest
{
    public class AgvClass
    {
        public byte[] ReturnBytes { get; set; }
        public int CurrentBarCode { get; set; }
        public byte[] ReSend { get; set; }
        public int CBarCode { get; set; }
        public string Sid { get; set; }
        public int Dtype { get; set; }
        public int STaskState { get; set; }
        public string AgvName { get; set; }
        public int AgvBar { get; set; }
        public double V { get; set; }
        public byte Ding { get; set; }

        public byte State { get; set; }
        public bool IsEnable { get; set; }
        public byte Error { get; set; }
        public DateTime ErrorTime { get; set; }
        public byte[] LastReturnBytes { get; set; }
        public int ErrorWait()
        {
            TimeSpan ts1 = new TimeSpan(ErrorTime.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts3 = ts2.Subtract(ts1).Duration();
            return ts3.Minutes * 60 + ts3.Seconds;
        }
    }
}
