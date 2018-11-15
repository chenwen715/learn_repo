using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace SocketAutoTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread ThWhile;
        private Thread THWs;
        private string host = Properties.Settings.Default.Host;
        string sqlConnect = Properties.Settings.Default.Sql;
        int port = 2024;
        WriteLog wl = new WriteLog();
        static int errorNum = 0;
        List<AgvClass> Alist = new List<AgvClass>();
        List<WorkStationControlClass> wsControlList = new List<WorkStationControlClass>();
        List<WorkStation> wsList = new List<WorkStation>();
        Path path = new Path();
        static bool isClose = false;
        int sleepTime = 1000;
        List<Point> tempList = new List<Point> { };
        Map monitorMap = new Map();
        DAL_Comn_Sql DalSql = new DAL_Comn_Sql();

        public MainWindow()
        {
            InitializeComponent();
            tempList = path.initPoint();
            InitAgv();
            InitWorkStation();
            
            
        }

        /// <summary>
        /// 初始化AGV
        /// </summary>
        private void InitAgv()
        {
            string sql = string.Format(@"SELECT a.*,st.endSonTaskStation,st.strSonTaskNo,st.sonTaskType FROM dbo.Agv a
LEFT JOIN FTask ft ON ft.taskAgv=a.strAgvNo AND ft.isTaskSubmit=1
LEFT JOIN SonTask st ON st.strSonTaskNo IN (select strSonTaskNo= min(strSonTaskNo)  from SonTask group BY strTaskNo) AND st.strTaskNo=ft.strTaskNo");
            
            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);

            Random rr = new Random();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgvClass agvclass = new AgvClass();
                agvclass.IsEnable = bool.Parse(dr["isEnable"].ToString());
                if (agvclass.IsEnable == true)
                {
                    agvclass.AgvName = dr["strAgvNo"].ToString();
                    agvclass.AgvType = int.Parse(dr["agvType"].ToString());
                    string currentBarcode= dr["strBarcode"].ToString();
                    string[] xy= currentBarcode.Substring(currentBarcode.IndexOf('(') + 1, (currentBarcode.IndexOf(')') - currentBarcode.IndexOf('(') - 1)).Split(new char[] { ',' });
                    agvclass.CurrentX =int.Parse(xy[0]);
                    agvclass.CurrentY = int.Parse(xy[1]);
                    agvclass.CurrentBarCode = agvclass.CurrentX.ToString() + agvclass.CurrentY.ToString();
                    agvclass.State = byte.Parse(dr["agvState"].ToString());

                    int v = (int)double.Parse(dr["currentCharge"].ToString());
                    if (v == 102)
                    {
                        agvclass.V = rr.Next(7000, 9900);
                    }
                    else
                    {
                        agvclass.V = v * 100;
                    }
                    agvclass.IsHaveGoods = bool.Parse(dr["agvCarry"].ToString());
                    agvclass.ChargeStation = int.Parse(dr["agvChargeStation"].ToString());
                    

                    agvclass.ReSend = null;
                    agvclass.ReturnBytes = null;
                    agvclass.TimeCount = 0;


                    string sid = dr["strSonTaskNo"].ToString();
                    if (!string.IsNullOrEmpty(sid))
                    {
                        agvclass.Sid = sid;
                        agvclass.TaskType = int.Parse(dr["sonTaskType"].ToString());
                        agvclass.DestinationNo = int.Parse(dr["endSonTaskStation"].ToString());
                        string sqlDelete = string.Format(@"DELETE FROM T_PathList_Tmp WHERE Sid='{0}'",agvclass.Sid);
                        int deleteRows=DalSql.DataOperator(Properties.Settings.Default.Sql, sqlDelete);
                        List<PathPoint> listPathPoint = new List<PathPoint>();
                        listPathPoint = path.findPath(tempList,agvclass.Sid,agvclass.DestinationNo,agvclass.TaskType,agvclass.CurrentX,agvclass.CurrentY);
                        string sql2 = "";
                        if (listPathPoint.Count != 0)
                        {
                            agvclass.PathNo = listPathPoint.Count;
                            agvclass.pathList = listPathPoint;
                            foreach (PathPoint pp in listPathPoint)
                            {
                                sql2 += string.Format(@"INSERT INTO dbo.T_PathList_Tmp SELECT Agv='{0}',Point='{1}',X={2},Y={3},RouteNo={4},sid='{5}',PointType={6},StationNo={7}",
                                    agvclass.AgvName, int.Parse(pp.point.currentX.ToString() + pp.point.currentY.ToString()),
                                    pp.point.currentX, pp.point.currentY, pp.routeNo, agvclass.Sid, pp.point.pointDescription, pp.point.intStationCode);
                            }
                            int pathPointCount = DalSql.DataOperator(Properties.Settings.Default.Sql, sql2);    
                            System.Console.WriteLine("\n" + agvclass.Sid + "子任务的路径点共有" + pathPointCount + "个");
                            agvclass.RouteNo = 1;
                        }
                        else
                        {
                            if (agvclass.State != 14)
                            {
                                agvclass.State = 11;
                            }
                            agvclass.Sid = "0";
                            agvclass.TaskType = 0;
                            agvclass.PathNo = 0;
                            agvclass.RouteNo = 1;
                            agvclass.pathList = null;
                            agvclass.DestinationNo = 0;
                            agvclass.flag1 = 0;
                            agvclass.flag2 = 0;
                        }
                        
                    }
                    else
                    {
                        if (agvclass.State != 14)
                        {
                            agvclass.State = 11;
                        }
                        agvclass.Sid = "0";
                        agvclass.TaskType = 0;
                        agvclass.PathNo = 0;
                        agvclass.RouteNo = 1;
                        agvclass.pathList=null;
                        agvclass.DestinationNo = 0;
                        agvclass.flag1 = 0;
                        agvclass.flag2 = 0;
                        
                        string sqlDelete1 = string.Format(@"DELETE FROM T_PathList_Tmp WHERE Agv='{0}'", agvclass.AgvName);
                        int deleteRows = DalSql.DataOperator(Properties.Settings.Default.Sql, sqlDelete1);
                    }

                    Alist.Add(agvclass);
                }
            }
        }

        /// <summary>
        /// 初始化料台
        /// </summary>
        private void InitWorkStation()
        {
            string sql = string.Format(@"select * from dbo.Station where siteType=2 or siteType=3");
            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WorkStation ws = new WorkStation();
                ws.wsNo = int.Parse(dr["intStationCode"].ToString());
                ws.wsControlName = dr["strStatoinName"].ToString();
                ws.wsType = int.Parse(dr["siteType"].ToString());
                int loadingState = int.Parse(dr["inState"].ToString());
                if (loadingState == 1)
                {
                    ws.IsLoadingWork = true;
                    ws.IsLoadingFinish = false;
                }
                else if (loadingState == 2)
                {
                    ws.IsLoadingWork = false;
                    ws.IsLoadingFinish = true;
                }
                else
                {
                    ws.IsLoadingWork = false;
                    ws.IsLoadingFinish = false;
                }

                int unLoadingState = int.Parse(dr["outState"].ToString());
                if (unLoadingState == 3)
                {
                    ws.IsUnloadingRequest = true;
                }
                else
                {
                    ws.IsUnloadingRequest = false;
                }
                ws.IsError = bool.Parse(dr["isStationErr"].ToString());
                ws.LoadingCount = 0;
                ws.UnLoadingCount = 0;
                wsList.Add(ws);
            }

            
            for (int i = 0; i < wsList.Count; i++)
            {
                if (i == 0)
                {
                    WorkStationControlClass wscClass = new WorkStationControlClass();
                    wscClass.WorkStationControlName = wsList[0].wsControlName;
                    wscClass.ws = new List<WorkStation>() { wsList[0] };
                    wsControlList.Add(wscClass);

                }
                else 
                {
                    for (int j = 0; j < wsControlList.Count; j++)
                    {
                        if (wsControlList[j].WorkStationControlName.Contains(wsList[i].wsControlName))
                        {
                            wsControlList[j].ws.Add(wsList[i]);
                            break;
                        }
                        if(j==wsControlList.Count-1)
                        {
                            WorkStationControlClass wscClass = new WorkStationControlClass();
                            wscClass.WorkStationControlName = wsList[i].wsControlName;
                            wscClass.ws = new List<WorkStation>() { wsList[i] };
                            wsControlList.Add(wscClass);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 一键启动AGV和料台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAutoStart_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Alist.Count; i++)
            {
                ThWhile = new Thread(DoWhile);
                ThWhile.IsBackground = true;
                ThWhile.Start(Alist[i]);
            }

            for (int j = 0; j < wsControlList.Count; j++)
            {
                THWs = new Thread(DoWhile1);
                THWs.IsBackground = true;
                THWs.Start(wsControlList[j]);
            }
            monitorMap.Show();
        }

        /// <summary>
        /// AGV持续与上位机通讯
        /// </summary>
        /// <param name="obj"></param>
        private void DoWhile(object obj)
        {
            AgvClass listbb = obj as AgvClass;
            while (true)
            {
                if (isClose)
                    break;

                Dispatcher.Invoke(new Action(() =>
                {
                    listbb = autoSend(listbb);
                }));
                ClearPointOccupy();
                Thread.Sleep(sleepTime);
            }
        }

        /// <summary>
        /// 料台持续与上位机通讯
        /// </summary>
        /// <param name="obj"></param>
        private void DoWhile1(object obj)
        {
            WorkStationControlClass listbb1 = obj as WorkStationControlClass;
            while (true)
            {
                if (isClose)
                    break;

                Dispatcher.Invoke(new Action(() =>
                {
                    listbb1 = autoSend1(listbb1);
                }));
                Thread.Sleep(sleepTime);
            }
        }

        /// <summary>
        /// AGV收发报文
        /// </summary>
        /// <param name="list">AGV类</param>
        /// <returns></returns>
        private AgvClass autoSend(AgvClass list)
        {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例
            AgvClass listindex = DataReceive(list);
            list.ReSend = listindex.ReturnBytes;


            ///创建socket并连接到服务器
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            socket.ReceiveTimeout = 3000000;
            socket.SendTimeout = 3000000;
            socket.Connect(ipe);//连接到服务器
            if (listindex.ReturnBytes[1] == 0)
            {
                int mapx;
                int mapy;
                Point p1 = tempList.Find(a => a.currentX == listindex.CurrentX && a.currentY == listindex.CurrentY);
                if (p1 != null)
                {
                    mapx = p1.mapX;
                    mapy = p1.mapY;
                }
                else
                {
                    mapx = listindex.CurrentX;
                    mapy = listindex.CurrentY;
                }                
                monitorMap.AgvChange(listindex,mapx,mapy,Properties.Settings.Default.GsHeight,Properties.Settings.Default.GsWidth);
                monitorMap.AgvShelfStatusChange(listindex);

            }
            socket.Send(listindex.ReturnBytes, listindex.ReturnBytes.Length, 0);//发送信息
            DateTime ts1 = DateTime.Now;
            DateTime ts2 = DateTime.Now;
            byte[] recvBytes = new byte[1024];
            int bytes;
            try
            {
                socket.ReceiveTimeout = 3000;
                if (socket.Poll(3000000, SelectMode.SelectRead))
                {
                    bytes = socket.Receive(recvBytes, 0, recvBytes.Length, 0);//从服务器端接受返回信息
                    ts2 = DateTime.Now;
                    socket.Close();
                    if ((bytes != 68 && bytes != 19 && bytes != 16))
                    {
                        list.ReturnBytes = null;
                    }
                    else
                    {
                        list.ReturnBytes = recvBytes;
                    }

                    #region
                    if (list.ReturnBytes != null)
                    {
                        System.Console.WriteLine("*******" + list.AgvName + "*******" + list.ReSend[1] + "*******" + list.ReSend[15] + "*******" + list.ReturnBytes[1] + "*******" + bytes + " FLAG1:" + list.flag1 + " FLAG2:" + list.flag2);
                    }
                    else
                    {
                        System.Console.WriteLine("*******" + list.AgvName + "*******" + list.ReSend[1] + "*******" + list.ReSend[15] + "*******" + bytes + " FLAG1:" + list.flag1 + " FLAG2:" + list.flag2);
                    }

                    string str = "";
                    if (list.ReturnBytes != null && bytes == 68 && list.ReSend[1]==0)
                    {
                        str = list.AgvName + ":" + list.CurrentBarCode + " 电量：" + BitConverter.ToInt32(list.ReSend, 15)
                           + " 状态：" + BitConverter.ToInt16(list.ReSend, 19) + " 有无货：" + list.ReSend[21]
                           + " 点1X：" + list.ReSend[27] + " 点1Y：" + list.ReSend[28]
                           + " 点2X：" + list.ReSend[29] + " 点2Y：" + list.ReSend[30]
                           + " 点3X：" + list.ReSend[31] + " 点3Y：" + list.ReSend[32]
                           + " 点4X：" + list.ReSend[33] + " 点4Y：" + list.ReSend[34]
                           + " 点5X：" + list.ReSend[35] + " 点5Y：" + list.ReSend[36]
                           + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond
                           + " 接收功能码：" + list.ReturnBytes[1] + " 任务类型：" + list.ReturnBytes[15] + " 目标工位号：" + list.ReturnBytes[16]
                           + " 任务号：" + Encoding.ASCII.GetString(list.ReturnBytes, 17, 50)
                           + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond + " 时间差：" + (ts2.Millisecond - ts1.Millisecond)+"ms";
                    }
                    else if (list.ReturnBytes != null && bytes == 19 && list.ReSend[1] == 3)
                    {
                        str = list.AgvName + ":" + list.CurrentBarCode + " 申请动作：" + list.ReSend[15]
                           + " 站点性质：" + list.ReSend[16] + " 站点号：" + list.ReSend[17]
                           + " 任务号：" + Encoding.ASCII.GetString(list.ReSend, 18, 50).Replace("\0", "").Trim()
                           + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond 
                           + " 接收功能码：" + list.ReturnBytes[1] + " 动作类型：" + list.ReturnBytes[15] + " 站点性质：" + list.ReturnBytes[16]
                           + " 站点号：" + list.ReturnBytes[17] + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond
                           + " 时间差：" + (ts2.Millisecond - ts1.Millisecond) + "ms";
                    }
                    else if (list.ReturnBytes != null && bytes == 16 && list.ReSend[1] == 3 )
                    {
                        str = list.AgvName + ":" + list.CurrentBarCode + " 申请动作：" + list.ReSend[15]
                           + " 站点性质：" + list.ReSend[16] + " 站点号：" + list.ReSend[17]
                           + " 任务号：" + Encoding.ASCII.GetString(list.ReSend, 18, 50).Replace("\0", "").Trim()
                           + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond
                           + " 时间差：" + (ts2.Millisecond - ts1.Millisecond) + "ms";
                    }
                    else
                    {
                        str = list.AgvName + ":" + list.CurrentBarCode + " 电量：" + BitConverter.ToInt32(list.ReSend, 15)
                           + " 状态：" + BitConverter.ToInt16(list.ReSend, 19) + " 有无货：" + list.ReSend[21]
                           + " 点1X：" + list.ReSend[27] + " 点1Y：" + list.ReSend[28]
                           + " 点2X：" + list.ReSend[29] + " 点2Y：" + list.ReSend[30]
                           + " 点3X：" + list.ReSend[31] + " 点3Y：" + list.ReSend[32]
                           + " 点4X：" + list.ReSend[33] + " 点4Y：" + list.ReSend[34]
                           + " 点5X：" + list.ReSend[35] + " 点5Y：" + list.ReSend[36]
                           + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond
                           + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond + " 时间差：" + (ts2.Millisecond - ts1.Millisecond) + "ms";
                    }

                    System.Console.WriteLine(str);
                    wl.MessageWriter("AGV"+list.AgvName,true, str );
                    wl.MessageWriter("AGV" + list.AgvName, true, "\n");
                    wl.MessageWriter("togetger", true, str);
                    wl.MessageWriter("togetger", true, "\n");

                    list.LastReturnBytes = list.ReturnBytes;
                    #endregion
                }
            }
            catch
            {
                list.ReturnBytes = null;

            }
            return list;

        }

        /// <summary>
        /// 料台收发报文
        /// </summary>
        /// <param name="list">料台控制台类</param>
        /// <returns></returns>
        private WorkStationControlClass autoSend1(WorkStationControlClass list)
        {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例
            WorkStationControlClass listindex = DataReceive(list);
            list.ReSend = listindex.ReturnBytes;
            ///创建socket并连接到服务器
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            socket.ReceiveTimeout = 3000000;
            socket.SendTimeout = 3000000;
            socket.Connect(ipe);//连接到服务器
            monitorMap.wsStateChange(listindex);
            socket.Send(listindex.ReturnBytes, listindex.ReturnBytes.Length, 0);//发送信息
            DateTime ts1 = DateTime.Now;
            DateTime ts2 = DateTime.Now;
            byte[] recvBytes = new byte[1024];
            int bytes;
            try
            {
                socket.ReceiveTimeout = 3000;
                if (socket.Poll(3000000, SelectMode.SelectRead))
                {
                    bytes = socket.Receive(recvBytes, 0, recvBytes.Length, 0);//从服务器端接受返回信息
                    ts2 = DateTime.Now;
                    socket.Close();
                    if (bytes != 56)
                    {
                        list.ReturnBytes = null;
                    }
                    else
                    {
                        list.ReturnBytes = recvBytes;
                    }
                    string wsStr = "";
                    if (list.ReturnBytes != null && Encoding.ASCII.GetString(list.ReturnBytes, 3, 10).Replace("\0", "") == "D03_067")
                    {
                        int i = 3;
                        //for (int i = 0; i < list.ws.Count; i++)
                        //{
                            //wsStr += "------" + Encoding.ASCII.GetString(list.ReturnBytes, 3, 10).Replace("\0", "") + "------\n";
                            bool[] resend = Common.getBooleanArray(list.ReSend[i + 15]);
                            bool[] returnb = Common.getBooleanArray(list.ReturnBytes[i + 15]);
                            wsStr += (i + 1) + " 号料台 ：" + " 上料运转中：" + (resend[0])
                                + " 上料已完成：" + (resend[1])
                                + " 下料请求：" + (resend[4])
                                + " 料台异常：" + (resend[5])
                                + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond
                                + " 上料请求：" + (returnb[0])
                                + " 允许下料：" + (returnb[4])
                                + " 下料完成：" + (returnb[5])
                                + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond
                                + " 时间差：" + (ts2.Millisecond - ts1.Millisecond) + "ms" + "\n";

                        //}
                    }
                    System.Console.WriteLine(wsStr);
                    wl.MessageWriter("WORKSTATION", true, wsStr);
                    wl.MessageWriter("togetger", true, wsStr);
                    wl.MessageWriter("togetger", true, "\n");
                }
            }
            catch
            {
                list.ReturnBytes = null;

            }
            return list;
        }

        /// <summary>
        /// 获取AGV数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private AgvClass DataReceive(AgvClass list)
        {
            if (list.V > 10000)
            {
                list.V = 10000;
            }
            switch (list.State)
            {
                case 11:
                    list.V -= (int)50/ 36;
                    break;
                case 13:
                    list.V -= (int)150 / 36;
                    break;
                case 14:
                    list.V += (int)GetVol(list.V);
                    break;
                default:
                    list.V = list.V;
                    break;
            }
            if (list.Error != 0)
            {
                if (list.ErrorWait() < 60)
                {
                    byte[] Dv = BitConverter.GetBytes((int)list.V);
                    list.ReSend[15] = Dv[0];
                    list.ReSend[16] = Dv[1];
                    list.ReSend[17] = Dv[2];
                    list.ReSend[18] = Dv[3];
                    byte c = CRC(list.ReSend, 38);
                    list.ReSend[37] = c;
                    list.ReturnBytes = list.ReSend;
                    return list;
                }
                else
                {
                    list.Error = 0;
                    list.State = 11;
                    list.ReturnBytes = new byte[] { 0, 0 };
                }
            }
            if (list.ReturnBytes == null || list.ReturnBytes[1]==5)//收到保持报文且无任务时，获取新的电量后发送心跳报文
            {
                if (list.ReSend != null && list.Sid == "0" )
                {
                    byte[] Dv = BitConverter.GetBytes((int)list.V);
                    list.ReSend[15] = Dv[0];
                    list.ReSend[16] = Dv[1];
                    list.ReSend[17] = Dv[2];
                    list.ReSend[18] = Dv[3];
                    byte c = CRC(list.ReSend, 38);
                    list.ReSend[37] = c;
                    list.ReturnBytes = list.ReSend;
                    return list;
                }
                else
                {
                    list.ReturnBytes = new byte[] { 0, 0 };//第一次发心跳报文或有任务时获取后面的点后发送心跳报文
                }
            }
            else if (list.ReturnBytes[1] == 4 && (list.ReturnBytes[15] == 2 || list.ReturnBytes[15] == 3))//收到工位等待或停车等待，继续发上次发送的信息
            {
                list.ReturnBytes = list.ReSend;
                return list;
            }


            switch (list.ReturnBytes[1])
            {
                case 0://AGV心跳
                    list = HeartBeat(list, 0);
                    break;
                case 1://AGV新任务
                    list = HeartBeat(list, 1);
                    break;
                case 5://保持
                    list = HeartBeat(list, 0);
                    break;
                //case 3://AGV申请动作完成
                //    list = HeartBeat(list,3);
                //    break;
                case 4://申请结果
                    list = HeartBeat(list, 0);
                    break;
            }
            return list;

        }

        /// <summary>
        /// 拼接AGV报文
        /// </summary>
        /// <param name="list"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private AgvClass HeartBeat(AgvClass list, byte flag)
        {
            byte[] data = new byte[100];
            byte[] DataReceive = list.ReturnBytes;
            byte[] Dataresend = list.ReSend;
            int pointType = 0;
            List<PathPoint> tmpListPathPoint = new List<PathPoint>();
            PathPoint currentPoint = new PathPoint();
            PathPoint lastPoint = new PathPoint();
            if (Dataresend != null)
            {
                if (DataReceive[0] == 2 && DataReceive[1] == 4 && DataReceive[15] == 1 || Dataresend[1] == 3 && Dataresend[15] == 2)
                {
                    list.RouteNo++;
                }
            }
            
            if (list.Sid != "0" && list.pathList.Count!=0)
            {
                tmpListPathPoint = list.pathList.FindAll(a => a.routeNo > (list.RouteNo - 5) && a.routeNo <=list.RouteNo && a.sid == list.Sid);
                if (list.RouteNo > list.pathList.Count)
                {
                    currentPoint = list.pathList.Find(a => a.routeNo == list.pathList.Count && a.sid == list.Sid);
                    lastPoint = list.pathList.Find(a => a.routeNo == (list.pathList.Count-1) && a.sid == list.Sid);
                }
                else
                {
                    currentPoint = list.pathList.Find(a => a.routeNo == list.RouteNo && a.sid == list.Sid);
                    if (list.RouteNo > 1)
                    {
                        lastPoint = list.pathList.Find(a => a.routeNo == (list.RouteNo - 1) && a.sid == list.Sid);
                    }
                    else
                    {
                        lastPoint = null;
                    }
                    
                }        
                pointType = currentPoint.point.pointDescription;
               
            }
            if(currentPoint.point!=null)
            {
                if (currentPoint.point.isOccuppy && currentPoint.point.occupyAgv != list.AgvName && lastPoint != null)
                {
                    currentPoint = lastPoint;
                    list.RouteNo = list.RouteNo - 1;
                }
            }
            

            if (list.Sid != "0")
            {
                if (currentPoint.point != null)
                {
                    list.CurrentBarCode = currentPoint.point.currentX.ToString() + currentPoint.point.currentY.ToString();
                    list.CurrentX = currentPoint.point.currentX;
                    list.CurrentY = currentPoint.point.currentY;

                }

                if (lastPoint != null)
                {
                    list.lastPoint = lastPoint.point;
                }
                else
                {
                    list.lastPoint = null;
                }

            }
            if (list.lastPoint != null && list.lastPoint.pBarcode != int.Parse(list.CurrentBarCode))
            {
                foreach (Point op in tempList)
                {
                    if (op.currentX == list.lastPoint.currentX && op.currentY == list.lastPoint.currentY)
                    {
                        op.isOccuppy = false;
                        op.occupyAgv = "";
                        string sql = string.Format("UPDATE dbo.T_PathPoint SET isOccupy = 0,occupyAgv='' WHERE x = {0} and y={1}", op.currentX, op.currentY);
                        DalSql.DataOperator(Properties.Settings.Default.Sql, sql);
                    }
                }
            }
            foreach (Point op in tempList)
            {
                if (op.currentX == list.CurrentX && op.currentY == list.CurrentY)
                {
                    op.isOccuppy = true;
                    op.occupyAgv = list.AgvName;
                    string sql = string.Format("UPDATE dbo.T_PathPoint SET isOccupy = 1,occupyAgv='{2}' WHERE x = {0} and y={1}", op.currentX, op.currentY, list.AgvName);
                    DalSql.DataOperator(Properties.Settings.Default.Sql, sql);
                }
            }
           

            //if (Dataresend != null)
            //{
                if (list.flag2 == 0&&list.flag1 == 1 && (list.Sid != "0" && DataReceive[1] == 0 && (pointType == 1 || pointType!=0&&list.RouteNo==list.PathNo))//路口点-申请路口，充电点-已回家pointType == 1 || pointType == 6
                    || (DataReceive[1]==1&&list.Sid == "0" && ((pointType == 2 || pointType == 3) && DataReceive[15] != 3 && DataReceive[15] != 4 && DataReceive[15] != 7//站台处收到上料、下料新任务报文
                    || pointType == 6 && (DataReceive[15] == 3 || DataReceive[15] == 4) && list.DestinationNo == currentPoint.point.pBarcode)))//充电桩处收到充电、取消充电新任务报文
                {
                    flag = 3;
                    list.flag1 = 0;//修改为发过心跳后再发申请
                }
                else if (list.flag2 == 0 && list.flag1 == 0 && (list.Sid != "0" && DataReceive[1] == 0 && (pointType == 1 || pointType != 0&& list.RouteNo == list.PathNo))//路口点-申请路口，充电点-已回家
                    || (DataReceive[1] == 1 && list.Sid == "0" && ((pointType == 2 || pointType == 3) && DataReceive[15] != 3 && DataReceive[15] != 4 && DataReceive[15] != 7//站台处收到上料、下料新任务报文
                    || pointType == 6 && (DataReceive[15] == 3 || DataReceive[15] == 4) && list.DestinationNo == currentPoint.point.pBarcode)))//充电桩处收到充电、取消充电新任务报文
                {
                    list.flag1 = 1;

                }
                if (list.CurrentBarCode != null && list.flag1 == 0)
                {
                    if ((list.Sid != "0" && currentPoint.point.pointDescription == 5) && list.flag2 == 1)//释放路口
                    {
                        flag = 3;
                        data[15] = 2;
                        list.flag2 = 0;
                    }
                    else if ((list.Sid != "0" && currentPoint.point.pointDescription == 5) && list.flag2 == 0)
                    {
                        list.flag2 = 1;
                    }
                }
                    
                    
            //}
            if (DataReceive[0] == 2 && DataReceive[1] == 4 && DataReceive[15] == 4)
            {
                list.TimeCount++;//假设收到3次工位下料辊道已工作报文后小车上料完成
                if (list.TimeCount ==3)
                {
                    data[15] = 4;
                    list.IsHaveGoods = true;
                    list.TimeCount = 0;
                    flag = 3;
                }
                else
                {
                    list.flag1 = 1;
                }
               
            }
            else if (DataReceive[0] == 2 && DataReceive[1] == 4 && DataReceive[15] == 5)
            {
                list.TimeCount++;
                if (list.TimeCount == 2)
                {
                    data[15] = 6;
                    list.IsHaveGoods = false;
                    list.TimeCount = 0;
                    flag = 3;
                }
                else
                {
                    list.flag1 = 1;
                }
            }
            if (flag == 1 && DataReceive[0] == 1)
            {
                data[1] = 0;
                list.TaskType = DataReceive[15];
                list.DestinationNo = DataReceive[16];
                list.Sid = Encoding.ASCII.GetString(DataReceive, 17, 50).Replace("\0", "");
                //list.CurrentBarCode = 1;暂不知道第一次如何获取（从数据库获取）
                int pathPointNO = DalSql.DataOperator(Properties.Settings.Default.Sql, string.Format(@"SELECT * FROM dbo.T_PathList_Tmp WHERE Sid='{0}'", list.Sid));
                if (pathPointNO != 0)
                {
                    list.pathList = path.findPath(tempList,list.Sid, list.DestinationNo, list.TaskType, list.CurrentX, list.CurrentY);
                    string sql2 = "";
                    foreach (PathPoint pp in list.pathList)
                    {
                        sql2 += string.Format(@"INSERT INTO dbo.T_PathList_Tmp SELECT Agv='{0}',Point='{1}',X={2},Y={3},RouteNo={4},sid='{5}',PointType={6},StationNo={7}",
                            list.AgvName, int.Parse(pp.point.currentX.ToString() + pp.point.currentY.ToString()),
                            pp.point.currentX, pp.point.currentY, pp.routeNo, list.Sid, pp.point.pointDescription,pp.point.intStationCode);
                    }
                    int pathPointCount = DalSql.DataOperator(Properties.Settings.Default.Sql, sql2);
                    list.PathNo = pathPointCount;
                    if (pathPointCount == 0)
                    {
                        throw new Exception(list.Sid + "子任务未找到路径");
                    }
                    System.Console.WriteLine("\n" + list.Sid + "子任务的路径点共有" + pathPointCount + "个");
                    list.RouteNo = 1;
                    currentPoint = list.pathList.Find(a => a.routeNo == list.RouteNo && a.sid == list.Sid);
                    tmpListPathPoint.Add(currentPoint);
                }
                flag = 3;
                data[15] = 10;
                list.State = 13;

            }
      
            //功能码
            data[1] = flag;
            //AGV名称
            byte[] agvNo = Encoding.ASCII.GetBytes(list.AgvName);
            for (int index = 3; index < 3 + agvNo.Length; index++)
            {
                data[index] = agvNo[index - 3];
            }
            //备用
            data[13] = 0;
            data[14] = 0;

            
           
            if (flag == 0)
            {
                //新任务时，从完整路径表中获取小车当前点到终点的路径点到临时路径表中编上序号并加上子任务号，
                //小车行走时根据路径点序号获取下一点，并且每次根据序号获取当前序号的下5个点发送给ACS
                if (list.Sid != "0")
                {
                                
                    if (list.RouteNo < list.PathNo)
                    {
                        if ((currentPoint.point.pointDescription == 0 || currentPoint.point.pointDescription == 4 || currentPoint.point.pointDescription == 5 || currentPoint.point.pointDescription == 6 && list.TaskType == 2 || currentPoint.point.pointDescription == 6 && list.TaskType == 7 && list.DestinationNo != currentPoint.point.pBarcode
                      || (currentPoint.point.pointDescription == 2 || currentPoint.point.pointDescription == 3 || currentPoint.point.pointDescription == 7 || currentPoint.point.pointDescription == 8)
                      && (list.TaskType == 7 || list.TaskType == 1 || list.TaskType == 2 )) && (list.flag1 == 0 && list.flag2 == 0))
                        {
                            list.RouteNo++;
                        }
                    }
                   
                    if (list.State != 14 || list.State == 14 && DataReceive[1] == 1 && DataReceive[15] == 4)
                    {
                        list.State = 13;
                    }
                    
                }
                if (DataReceive[0] == 2 && DataReceive[1] == 4&&DataReceive[15] == 6)
                {
                    if (list.State != 14 || list.State == 14 && Dataresend[15] == 8 && Dataresend[1]==3)
                    {
                        list.State = 11;
                    }                  
                    string sql2 = string.Format(@"DELETE FROM dbo.T_PathList_Tmp WHERE Sid='{0}' ", list.Sid);
                    int pathPointCount1 = DalSql.DataOperator(Properties.Settings.Default.Sql, sql2);
                    list.PathNo = 0;
                    list.Sid = "0";
                    list.RouteNo = 1;
                    list.pathList = null;
                }
               
                //指令分区
                data[0] = 1;
                //数据长度
                data[2] = 22;
                //电量
                byte[] Dv = BitConverter.GetBytes((int)list.V);
                data[15] = Dv[0];
                data[16] = Dv[1];
                data[17] = Dv[2];
                data[18] = Dv[3];
                //自身状态
                byte[] St = BitConverter.GetBytes((int)list.State);
                data[19] = St[0];
                data[20] = St[1];
                //有无货
                data[21] = Convert.ToByte(list.IsHaveGoods);
                //if (list.State == 13)
                //{
                //    //故障
                //    Random err = new Random();
                //    int error = err.Next(1, 3600);
                //    if (error == 1)
                //    {
                //        list.Error = 1;
                //        list.ErrorTime = DateTime.Now;
                //        errorNum++;
                //    }
                //}
                data[22] = list.Error;
                data[27] = byte.Parse(list.CurrentX.ToString());
                data[28] = byte.Parse(list.CurrentY.ToString());                  
                for (int i = 1; i < tmpListPathPoint.Count; i++)
                {
                    data[27 + 2 * i] = byte.Parse(tmpListPathPoint[tmpListPathPoint.Count - i - 1].point.currentX.ToString());
                    data[28 + 2 * i] = byte.Parse(tmpListPathPoint[tmpListPathPoint.Count - i - 1].point.currentY.ToString());
                }
                data[37] = CRC(data, 38);
                list.ReturnBytes = data;                
                return list;
            }
            else if (flag == 3)
            {
                //指令分区
                data[0] = 2;
                //数据长度
                data[2] = 53;
                if (data[15] != 2 && data[15] != 4 && data[15] != 6 && data[15] != 10)
                {
                    data[15] = GetActionType(pointType, list.TaskType,list.IsHaveGoods);
                }             
               
                if (data[15] == 0)
                {
                    throw new Exception("动作类型不正确");
                }
                if (data[15] == 7)//已开始充电，小车状态为充电中
                {
                    list.State = 14;
                }
               
                if(data[15] == 10)
                {
                    data[16] = 0;
                    data[17] = 0;
                }
                else
                {
                    data[16] = byte.Parse(currentPoint.point.pointDescription.ToString());
                    data[17] = byte.Parse(currentPoint.point.intStationCode.ToString());
                }   
                
                byte[] cTaskNum;
                cTaskNum = Encoding.ASCII.GetBytes(list.Sid);
                for (int i = 18; i < 18 + cTaskNum.Length; i++)
                {
                    data[i] = cTaskNum[i - 18];
                }
                data[68] = CRC(data, 69);
                list.ReturnBytes = data;
                return list;
            }
            {
                throw new Exception("功能码不正确");
            }

        }

        /// <summary>
        /// 获取料台数据并拼接报文
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private WorkStationControlClass DataReceive(WorkStationControlClass list)
        {
            byte[] data = new byte[100];
            byte[] DataReceive = list.ReturnBytes;
            byte[] Dataresend = list.ReSend;
            if (DataReceive == null && Dataresend == null)
            {
                list.ReturnBytes = new byte[] { 0, 0 };
            }
            else if (DataReceive == null || DataReceive[1]==5)
            {
                byte c = CRC(Dataresend, 56);
                Dataresend[55] = c;
                list.ReturnBytes = Dataresend;
                return list;
            }
           
            data[0] = 3;
            data[1] = 10;
            data[2] = 40;
            byte[] wsControlNo = Encoding.ASCII.GetBytes(list.WorkStationControlName);
            for (int i = 3; i < wsControlNo.Length + 3; i++)
            {
                data[i] = wsControlNo[i - 3];
            }
            data[13] = 0;
            data[14] = 0;
            
            for (int i = 15; i < (15 +list.ws.Count); i++)//第一次发心跳怎么拼报文，从数据库获取？
            {
                if (list.ws[i-15].wsType == 2)
                {
                    if (list.ReturnBytes[1] == 0)
                    {
                        data[i] = GetWs(i, 255, list);
                    }
                    else
                    {
                        data[i] = GetWs(i, DataReceive[i], list);
                    }
                }
                else if (list.ws[i-15].wsType == 3)
                {
                    if (list.ReturnBytes[1] == 0)
                    {
                        if (i % 2 != 0)
                        {
                            data[15+(i-15)/2] = GetWs(i, 255, list);
                        }
                        else
                        {
                            data[35 + (i - 16) / 2] = GetWs(i, 255, list);
                        }
                        
                    }
                    else
                    {
                        if (i % 2 != 0)
                        {
                            data[15 + (i - 15) / 2] = GetWs(i, DataReceive[15 + (i - 15) / 2], list);
                        }
                        else
                        {
                            data[35 + (i - 16) / 2] = GetWs(i, DataReceive[35 + (i - 16) / 2], list);
                        }
                       
                    }
                }
            }

            data[55] = CRC(data, 56);
            list.ReturnBytes = data;
            return list;
           
            
        }

        /// <summary>
        /// 获取料台byte数据
        /// </summary>
        /// <param name="i"></param>
        /// <param name="a"></param>
        /// <param name="wsClass"></param>
        /// <returns></returns>
        private byte GetWs(int i, byte a, WorkStationControlClass wsClass)
        {
            bool[] getBit = Common.getBooleanArray(a);
            int[] num = new int[8];
            byte b = 0;
            if (a != 255)
            {
                if (getBit[0])
                {
                    if (wsClass.ws[i - 15].LoadingCount < 5)
                    {
                        wsClass.ws[i - 15].IsLoadingWork = true;
                        wsClass.ws[i - 15].IsLoadingFinish = false;
                        wsClass.ws[i - 15].LoadingCount++;
                    }
                    else
                    {
                        wsClass.ws[i - 15].IsLoadingWork = false;
                        wsClass.ws[i - 15].IsLoadingFinish = true;
                        wsClass.ws[i - 15].LoadingCount = 0;
                    }
                }
                if (!getBit[0])
                {
                    wsClass.ws[i - 15].IsLoadingFinish = false;
                }
                if (getBit[5])
                {
                    wsClass.ws[i - 15].IsUnloadingRequest = false;
                }
            }
            num[0] = Convert.ToInt16(wsClass.ws[i - 15].IsLoadingWork);
            num[1] = Convert.ToInt16(wsClass.ws[i - 15].IsLoadingFinish);
            num[4] = Convert.ToInt16(wsClass.ws[i - 15].IsUnloadingRequest);
            num[5] = Convert.ToInt16(wsClass.ws[i - 15].IsError);
            string strBit = Common.getString(num);
            b = (byte)(Convert.ToInt32(strBit, 2));
            return b;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public byte CRC(byte[] byt, int Length)
        {
            byte crc = 0;
            crc = (byte)(byt[0] ^ byt[1]);
            for (int i = 2; i < Length - 1; i++)
            {
                crc = (byte)(crc ^ byt[i]);

            }
            return crc;
        }

        /// <summary>
        /// 获取AGV电量
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        private double GetVol(double vol)
        {
            if (vol < 65)
                return 45;
            else if (vol >= 65 && vol < 75)
                return 30;
            else
                return 20;
        }

        /// <summary>
        /// 获取AGV申请动作类型
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        private byte GetActionType(int pType,int taskType,bool isCarry)//充电桩处发已回家？怎么发已开始充电？（A：结合任务类型）
        {
            switch (pType)
            {
                case 1:
                    return 1;//申请路口              
                    //return 2;//释放路口
                case 2:
                case 3:
                case 7:
                case 8:
                    if (taskType == 2 && !isCarry)
                    {
                        return 3;//申请小车左侧上料
                    }
                    else if (taskType == 1 && isCarry)
                    {
                        return 5;//申请小车左侧下料
                    }
                    else if (taskType == 2 && isCarry)
                    {
                        return 4;//上料完成
                    }
                    else if (taskType == 1 && !isCarry)
                    {
                        return 6;//下料完成
                    }
                    else
                    {
                        return 0;
                    }      
                   
                case 6:
                    if (taskType == 3)
                    {
                        return 7;//已开始充电
                    }
                    else if (taskType == 7)
                    {
                        return 9;//已回家
                    }
                    else if (taskType == 4)
                    {
                        return 8;//已结束充电
                    }
                    else
                    {
                        return 0;
                    }                                        
                default:
                    return 0;//wrong
            }
        }

        /// <summary>
        /// 一键关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAutoStop_Click(object sender, RoutedEventArgs e)
        {
            isClose = false;
            this.Close();
        }

        /// <summary>
        /// 获取AGV信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckState_Click(object sender, RoutedEventArgs e)
        {
            if (Combox_Address.Text == "" || Combox_Address.Text == null)
            {
                MessageBox.Show("AgvName 不能为空！！！");
            }
            else
            {
                for (int i = 0; i < Alist.Count; i++)
                {
                    if (Alist[i].AgvName == Combox_Address.Text)
                    {
                        Tbox_CurrtBarCode.Text = Alist[i].CurrentBarCode.ToString();
                        Tbox_AgvCarry.Text = Alist[i].IsHaveGoods.ToString();
                        Tbox_Voltage.Text = Alist[i].V.ToString();
                        MessageBox.Show("查询成功！！！");
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                MessageBox.Show("未找到此 Agv！！！");
            }
        }

        /// <summary>
        /// 修改AGV信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeState_Click(object sender, RoutedEventArgs e)
        {
            if (Combox_Address.Text == "" || Combox_Address.Text == null)
            {
                MessageBox.Show("AgvName 不能为空！！！");
            }
            else if (Tbox_CurrtBarCode.Text == "" || Tbox_CurrtBarCode.Text == null)
            {
                MessageBox.Show("当前码值 不能为空！！！");
            }
            else if (Tbox_AgvCarry.Text == "" || Tbox_AgvCarry.Text == null)
            {
                MessageBox.Show("有无货 不能为空！！！");
            }
            else if (Tbox_Voltage.Text == "" || Tbox_Voltage.Text == null)
            {
                MessageBox.Show("电量 不能为空！！！");
            }
            else
            {
                for (int i = 0; i < Alist.Count; i++)
                {
                    if (Alist[i].AgvName == Combox_Address.Text)
                    {
                        Alist[i].CurrentBarCode = Tbox_CurrtBarCode.Text;
                        Alist[i].IsHaveGoods = bool.Parse(Tbox_AgvCarry.Text);
                        Alist[i].V = int.Parse(Tbox_Voltage.Text);
                        MessageBox.Show("修改成功！！！");
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }

        private ServiceReference1.ModelContractClient Client = new ServiceReference1.ModelContractClient();

        /// <summary>
        /// 完工信号下发下料请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Socket_Click(object sender, RoutedEventArgs e)
        {
            int cIndex = wsControlList.FindIndex(a => a.WorkStationControlName == ("D03_" + cBox_No.Text.PadLeft(3, '0')));
            if (cIndex != -1)
            {
                int wsNo;
                int wsIndex = wsControlList[cIndex].ws.FindIndex(a => a.wsNo == int.Parse(wsBox_No.Text));
                if (wsIndex != -1)
                {
                    if (cBox_No.Text == "67")
                    {
                        wsNo =int.Parse(wsBox_No.Text)- 50;
                    }
                    else
                    {
                        wsNo = int.Parse(wsBox_No.Text);
                    }
                    wsControlList[cIndex].ws[wsNo-1].IsUnloadingRequest = true;
                    MessageBox.Show("D03_0" + cBox_No.Text.PadLeft(2, '0') + "控制台" + "\n" + wsBox_No.Text + "号料台下料请求完成");
                    //Thread th = new Thread(Count);
                    //th.Start(wsControlList[cIndex].ws[int.Parse(wsBox_No.Text)]);
                }
                else
                {
                    MessageBox.Show(string.Format("{0}控制台无{1}号料台，下料失败", "D03_0" + cBox_No.Text.PadLeft(2, '0'), wsBox_No.Text));
                }
            }
            else
            {
                MessageBox.Show(string.Format("无{0}控制台，下料失败", "D03_0" + cBox_No.Text.PadLeft(2, '0')));
            }
            cBox_No.Text = "";
            wsBox_No.Text = "";
        }


        private void Count(Object obj)
        {
            WorkStation workStation = obj as WorkStation;
            if (workStation.wsNo == 1)//成品料台，请求下料的同时，下料区有货
            {
                
            }
            else//导线桶料台，假设下料请求5s后下料区有货
            {
                for (int i = 0; i < 5; i++)
                {
                    workStation.UnLoadingCount++;
                    Thread.Sleep(1000);
                }
                if (workStation.UnLoadingCount == 5)
                {
                   
                    workStation.UnLoadingCount = 0;
                }
            }

        }

        private void ClearPointOccupy()
        {
            List<string> listBarcode = new List<string>();
            foreach (AgvClass agv in Alist)
            {
                listBarcode.Add(agv.CurrentBarCode);
            


                List<Point> listPoint = tempList.FindAll(a => a.isOccuppy);
                foreach (Point p in listPoint)
                {
                    if (p.occupyAgv != agv.AgvName) continue;
                    if (!listBarcode.Exists(a => a == p.pBarcode.ToString()))
                    {
                        string sql = string.Format("UPDATE dbo.T_PathPoint SET isOccupy = 0,occupyAgv='' WHERE x = {0} and y={1}", p.currentX,p.currentY);
                        DalSql.DataOperator(Properties.Settings.Default.Sql,sql);

                        p.isOccuppy = false;
                        p.occupyAgv = "";
                    }
                }
            }
           
        }



    }
}


