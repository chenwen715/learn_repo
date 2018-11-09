using System.Net.Sockets;

namespace ACS
{
    public class SocketOpt
    {
        /// <summary>
        /// 封装的报文数据
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 客户端对象
        /// </summary>
        public Socket Sct;

        public Agv agv;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SClient">客户端对象</param>
        /// <param name="Rec">报文信息</param>
        public SocketOpt(Socket SClient, byte[] Rec)
        {
            Sct = SClient;
            Data = Rec;
        }
    }
}
