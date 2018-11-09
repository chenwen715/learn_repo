using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageTranslate
{
    public class TCP
    {
        public static Socket BuildServer(string Ip, int Port)
        {
            IPAddress ip = IPAddress.Parse(Ip);
            IPEndPoint ipe = new IPEndPoint(ip, Port);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(ipe);
            return s;
        }

        /// <summary>
        /// 服务侦听
        /// </summary>
        /// <param name="Backlog">挂起的连接请求</param>
        /// <param name="SvrSocket">服务套接字</param>
        public static void ServerListen(int Backlog, Socket SvrSocket)
        {
            SvrSocket.Listen(Backlog);//开始监听

            while (true)
            {
                //if (Commond.IsClose)
                //    break;

                //服务器监听，建立socket
                Socket ClientSocket = SvrSocket.Accept();
                ClientSocket.ReceiveTimeout = 3000;
                //创建线程
                Thread thSocket = new Thread(new ParameterizedThreadStart(MultiSocket));
                //线程启动，参数为socket
                thSocket.Start(ClientSocket);
                //MultiSocket(ClientSocket);
            }
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="ClientSocket">客户端对象</param>
        public static void MultiSocket(Object cSocket)
        {
            Socket ClientSocket = cSocket as Socket;

            byte[] ReceiveData = new byte[1024];
            try
            {
                //自动断线时间为30秒
                ClientSocket.ReceiveTimeout = 30000;
                if (ClientSocket.Poll(30000000, SelectMode.SelectRead))
                {
                    //连续收发
                    while (true)
                    {
                        int DataLength = ClientSocket.Receive(ReceiveData);
                        if (DataLength <= 0)
                        {
                            Thread.ResetAbort();
                            ClientSocket.Close();
                        }
                        else
                        {
                            //处理

                        }
                    }
                }
                else
                {
                    ClientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                //写到日志和前台
                ClientSocket.Close();
                ClientSocket.Dispose();
                LogWrite.WriteError(ex.Message);
            }
        }

        /// <summary>
        /// Tcp指令发送
        /// </summary>
        /// <param name="ClientSocket">自定义Socket对象</param>
        /// <returns>发送结果</returns>
        public static void Send(Socket socket)
        {

        }
    }
 }