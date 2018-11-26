using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACSsocket
{
     public class AgvMsg
    {
        /// <summary>
        /// 小车发送的小车号
        /// </summary>
        public string AgvNo;
        
        /// <summary>
        /// 小车发送的当前码值
        /// </summary>
        public string Barcode;

        /// <summary>
        /// 小车发送的当前电压
        /// </summary>
        public int Voltage;

        /// <summary>
        /// 小车发送的自身状态
        /// </summary>
        public int State;

        /// <summary>
        /// 小车发送的当前顶升状态
        /// </summary>
        public HeightEnum Height;

        /// <summary>
        /// 小车发送的错误代码1
        /// </summary>
        public int ErrCode1;

        /// <summary>
        /// 小车发送的错误代码2
        /// </summary>
        public int ErrCode2;

        /// <summary>
        /// 小车发送的错误代码3
        /// </summary>
        public int ErrCode3;

        public int SID;

        public int sTaskStatus;
    }
}
