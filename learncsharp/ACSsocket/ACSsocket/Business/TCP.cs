using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACSsocket
{
    class TCP
    {
        /// <summary>
        /// 服务侦听
        /// </summary>
        /// <param name="Backlog">挂起的连接请求</param>
        /// <param name="SvrSocket">服务套接字</param>
        public static void ServerListen(int Backlog, Socket SvrSocket)
        {
            SvrSocket.Listen(Backlog);//开始监听

            //while (true && Form1.flag)
            while (true)
            {

                //服务器监听，建立socket
                Socket ClientSocket = SvrSocket.Accept();
                ClientSocket.ReceiveTimeout = 3000;
                //创建线程
                Thread thSocket = new Thread(new ParameterizedThreadStart(DealAgvMsg));
                //线程启动，参数为socket
                thSocket.Start(ClientSocket);
                //MultiSocket(ClientSocket);
            }
        }

        /// <summary>
        /// 处理AGV消息
        /// </summary>
        /// <param name="ClientSocket">客户端对象</param>
        public static void DealAgvMsg(Object ClientSocket)
        {
            Socket cSocket = ClientSocket as Socket;

            try
            {
                byte[] ReceiveData = new byte[1024];
                cSocket.ReceiveTimeout = 3000;
                if (cSocket.Poll(3000000, SelectMode.SelectRead))
                {
                    int DataLength = cSocket.Receive(ReceiveData);
                    if (DataLength > 0)
                    {
                        SocketOpt SRece = new SocketOpt(cSocket, ReceiveData);
                        Form1.flag = false;

                        if (SRece != null)
                        {
                            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                            string str = Encoding.ASCII.GetString(ReceiveData, 3, 10).Replace("\0", "") + ":" + " 码值：" + BitConverter.ToInt32(ReceiveData, 13) +
                                " 电量：" + BitConverter.ToInt16(ReceiveData, 17) + " 状态：" + ReceiveData[19]
                      + " 顶升：" + ReceiveData[20] + " 任务号：" + BitConverter.ToInt32(ReceiveData, 26)
                      + " 任务状态：" + ReceiveData[30] + " 时间：" + ts1.Hours + ":" + ts1.Minutes + ":" + ts1.Seconds;
                            System.Console.WriteLine(str);
                            //string str1 = BitConverter.ToString(ReceiveData);
                            //Form1.SetText1(str1 + "\n" + str + "\n");
                            Form1.SetText1(str + "\n");

                            MsgManage.DataTranslate(SRece);


                            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                            TimeSpan ts3 = ts2.Subtract(ts1).Duration();
                            SRece.dealTime = ts3.Milliseconds - SRece.dealTime;

                            //string sql = string.Format("INSERT INTO T_Config_Record (agvNo,expDealTimeUpdate,dealTime) VALUES('{0}','{1}','{2}')", SRece.agv.agvNo, SRece.dealTime, ts3.Milliseconds);
                            //DbHelperSQL.ExecuteSql(sql);
                        }

                        Thread.Sleep(20);
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.MessageError("TranMultiSocket", Ex.ToString());
                //throw Ex;
            }
            finally
            {
                cSocket.Close();
            }
        }

        /// <summary>
        /// Tcp指令发送
        /// </summary>
        /// <param name="ClientSocket">自定义Socket对象</param>
        /// <returns>发送结果</returns>
        public static void Send(SocketOpt ClientSocket)
        {
            Socket s = ClientSocket.Sct;
            try
            {
                if (ClientSocket.SendData != null)
                {
                    s.Send(ClientSocket.SendData, ClientSocket.SendData.Length, 0);
                    TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                    //string st = BitConverter.ToInt32(ClientSocket.SendData, 3).ToString();
                    string str="";
                    if (ClientSocket.SendData[2] == 0)
                    {
                        str = " 小车：" + ClientSocket.agv.agvNo+" 功能码：" + ClientSocket.SendData[1]
                             + " 时间：" + ts1.Hours + ":" + ts1.Minutes + ":" + ts1.Seconds;
                    }else
                    {
                        str = " 小车：" + ClientSocket.agv.agvNo + " 功能码：" + ClientSocket.SendData[1] + " 数据长度：" + ClientSocket.SendData[2] + " 任务号：" 
                            + BitConverter.ToInt32(ClientSocket.SendData, 3)+ " 动作类型：" + ClientSocket.SendData[7]
                            + " 码值1：" + BitConverter.ToInt32(ClientSocket.SendData, 8) + " x坐标：" + BitConverter.ToInt16(ClientSocket.SendData, 12)
                            + " y坐标：" + BitConverter.ToInt16(ClientSocket.SendData, 14) + " x间距：" + BitConverter.ToInt16(ClientSocket.SendData, 16)
                            + " y间距：" + BitConverter.ToInt16(ClientSocket.SendData, 18) + " 车头方向：" + ClientSocket.SendData[20]
                            + " 点属性：" + ClientSocket.SendData[21] + " 防撞方向：" + ClientSocket.SendData[22] + " 转盘属性：" + ClientSocket.SendData[23]
                            + " 码值2：" + BitConverter.ToInt32(ClientSocket.SendData, 24)
                            + " 码值3：" + BitConverter.ToInt32(ClientSocket.SendData, 40)
                            + " 码值4：" + BitConverter.ToInt32(ClientSocket.SendData, 56)
                            + " 时间：" + ts1.Hours + ":" + ts1.Minutes + ":" + ts1.Seconds;
                    }                   
                    //string str1 = BitConverter.ToString(ClientSocket.SendData);
                    //Form1.SetText(str1 + "\n" + str + "\n");
                    Form1.SetText(str + "\n");

                    //记录回给小车的信息
                    //App.ExFile.MessageLog("Send" + ClientSocket.agv.agvNo, BitConverter.ToString(ClientSocket.Data));
                }
                else
                {
                    s.Send(new byte[1], 1, 0);
                }
                Form1.flag = true;
                Form1.responseType = 0;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                s.Close();
            }
        }
    }
}