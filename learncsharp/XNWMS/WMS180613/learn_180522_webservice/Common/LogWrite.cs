using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180522_webservice
{
    class LogWrite
    {
        public delegate void ResultLogDelegate(string log);
        public static ResultLogDelegate WriteResultMain;
        public static bool WriteResult(string content)
        {
            if (WriteResultMain != null)
                WriteResultMain(content);
            return true; ;
        }

        public delegate void WriteLogDelegate(string log);
        public static WriteLogDelegate WriteMain;
        public static bool WriteLogToMain(string content)
        {
            if (WriteMain != null)
                WriteMain(content);
            return true;
        }
    }
}
