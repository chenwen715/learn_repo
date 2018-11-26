using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ACSsocket
{
    class SocketOpt
    {
        /// <summary>
        /// 封装的报文数据
        /// </summary>
        public byte[] recData;

        public byte[] SendData;

        /// <summary>
        /// 客户端对象
        /// </summary>
        public Socket Sct;

        public Agv agv;

        public int dealTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SClient">客户端对象</param>
        /// <param name="Rec">报文信息</param>
        public SocketOpt(Socket SClient, byte[] Rec)
        {
            Sct = SClient;
            recData = Rec;
        }
    }
}
