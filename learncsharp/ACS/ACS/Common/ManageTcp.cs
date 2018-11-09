using System;
using System.Net.Sockets;
using System.Threading;

namespace ACS
{
    public class ManageTcp
    {
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
                if (Commond.IsClose)
                    break;

                //服务器监听，建立socket
                Socket ClientSocket = SvrSocket.Accept();
                ClientSocket.ReceiveTimeout = 3000;
                //创建线程
                Thread thSocket = new Thread(new ParameterizedThreadStart(MultiSocket));
                //线程启动，参数为socket
                thSocket.Start(ClientSocket);
            }
        }

        /// <summary>
        /// 线程-收发报文处理业务的开始
        /// </summary>
        /// <param name="ClientSocket">客户端对象</param>
        public static void MultiSocket(Object ClientSocket)
        {
            Socket cSocket = ClientSocket as Socket;

            //try
            //{
                byte[] ReceiveData = new byte[1024];
                cSocket.ReceiveTimeout = 3000;
                if (cSocket.Poll(3000000, SelectMode.SelectRead))
                {
                    int DataLength = cSocket.Receive(ReceiveData);
                    if (DataLength > 0)
                    {
                        SocketOpt SRece = new SocketOpt(cSocket, ReceiveData);

                        if (SRece != null)
                        {
                            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);

                            MsgManage.DataTranslate(SRece);


                            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                            TimeSpan ts3 = ts2.Subtract(ts1).Duration();
                            //App.ExFile.MessageLog("处理Agv指令时间", "1|" + ts2.Milliseconds + "-" + ts1.Milliseconds + "=" + ts3.Milliseconds);
                        }
                    }

                    Thread.Sleep(20);
                }
            //}
            //catch (Exception Ex)
            //{
            //    App.ExFile.MessageError("TranMultiSocket", Ex.ToString());
            //}
            //finally
            //{
            //    cSocket.Close();
            //}
        }

        /// <summary>
        /// Tcp指令发送
        /// </summary>
        /// <param name="ClientSocket">自定义Socket对象</param>
        /// <returns>发送结果</returns>
        public static void Send(SocketOpt ClientSocket)
        {
            Socket s = ClientSocket.Sct;
            //try
            //{
                s.Send(ClientSocket.Data, ClientSocket.Data.Length, 0);
            //}
            //catch (Exception Ex)
            //{
            //    throw Ex;
            //}
            //finally
            //{
            //    s.Close();
            //}
        }
    }
}
