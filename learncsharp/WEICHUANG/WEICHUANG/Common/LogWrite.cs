﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEICHUANG.Common
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

        public delegate void WriteLog1Delegate(string log);
        public static WriteLog1Delegate WriteMain1;
        public static bool WriteLogToMain1(string content)
        {
            if (WriteMain1 != null)
                WriteMain1(content);
            return true;
        }
    }
}

