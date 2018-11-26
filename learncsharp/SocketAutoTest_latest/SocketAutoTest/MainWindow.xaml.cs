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
        //private string host = "127.0.0.1";
        private string host = Properties.Settings.Default.Host;
        string sqlConnect = Properties.Settings.Default.Sql;
        int port = 2223;
        static int errorNum = 0;
        List<AgvClass> Alist = new List<AgvClass>();
        static bool isClose = false;

        int sleepTime =2000;


        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            string sql = string.Format(@"SELECT agv.*,Barcode,son.SID,son.State sTaskState,c.ItemName  FROM dbo.T_Base_AGV agv
LEFT JOIN dbo.T_Task_Son son ON agv.AgvNo = son.AgvNo AND son.State = 2
LEFT JOIN dbo.T_Type_Config c ON son.STaskType = c.ItemNo");
            DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);

            Random rr = new Random();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgvClass agvclass = new AgvClass();
                agvclass.IsEnable = bool.Parse(dr["IsEnable"].ToString());
                if (agvclass.IsEnable == true)
                {
                    agvclass.AgvBar = int.Parse(dr["Barcode"].ToString());
                    agvclass.AgvName = dr["AgvNo"].ToString();

                    agvclass.CurrentBarCode = int.Parse(dr["Barcode"].ToString());

                    agvclass.State = byte.Parse(dr["state"].ToString());
                    agvclass.Ding = byte.Parse(dr["Height"].ToString());

                    agvclass.CBarCode = 0;
                    //agvclass.CurrentBarCode = 0;
                    agvclass.ReSend = null;
                    agvclass.ReturnBytes = null;

                    int v = (int)double.Parse(dr["currentcharge"].ToString());
                    //if (v == 102)
                    //    agvclass.V = rr.Next(7000,9900);
                    //else
                    agvclass.V = v * 100;

                    string sid = dr["SID"].ToString();
                    if (!string.IsNullOrEmpty(sid))
                    {
                        agvclass.Sid = sid;
                        //agvclass.Dtype = int.Parse(dr["Dtype"].ToString());
                    }
                    else
                    {
                        agvclass.Sid = "0";
                       
                        //agvclass.Dtype = 0;
                    }

                    Alist.Add(agvclass);
                }
            }
        }

        private void BtnAutoStart_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Alist.Count;i++ )
            {
                ThWhile = new Thread(DoWhile);
                ThWhile.IsBackground = true;
                ThWhile.Start(Alist[i]);
            }
        }
        
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
                Thread.Sleep(sleepTime);
            }
        }

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
            socket.Send(listindex.ReturnBytes, listindex.ReturnBytes.Length, 0);//发送信息
            DateTime ts1 = DateTime.Now;
            DateTime ts2 = DateTime.Now;
            byte[] recvBytes = new byte[1024];
            int bytes;
            string str1 = "";
            string sql1 = "";
            try
            {
                
                bytes = socket.Receive(recvBytes, 0, recvBytes.Length, 0);//从服务器端接受返回信息
                ts2 = DateTime.Now;
                socket.Close();

               
                if (list.LastReturnBytes != null && list.LastReturnBytes[1] == 3)
                {
                    sql1 = string.Format("SELECT top 2 a.agvno,a.barcode AS b1,a.state,a.currentcharge,a.height,t.taskstate,t.taskno,t.tasktype,t.barcode AS b2,s.sid,p.id,t.shelfno,shelf.barcode as b4,shelf.currentbarcode as b5,shelf.IsLocked,point.BarCode AS b3,point.IsOccupy,point.OccupyAgvNo, t.EndTime" +
                       " FROM T_Base_AGV a" +
                       " LEFT JOIN T_Task t ON a.agvno=t.agvno" +
                       " LEFT JOIN T_Task_Son s ON t.taskno=s.taskno" +
                       " LEFT JOIN T_Base_PathList p ON s.sid=p.sid" +
                       " LEFT JOIN T_Base_Shelf shelf ON t.shelfno=shelf.shelfno" +
                       " LEFT JOIN T_Base_Point point ON t.Barcode=point.BarCode" +
                       " WHERE a.agvno='{0}' ORDER BY t.submittime desc", list.AgvName);
                    DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
                    DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql1);
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["EndTime"].ToString() != "")
                    {
                        str1 = "\n" + "***" + list.AgvName + "***" + " 小车当前码值：" + dr["b1"].ToString() + " 小车当前状态：" + dr["state"].ToString() + " 小车当前顶升状态：" + dr["height"].ToString()
                        + "\n" + "***" + list.AgvName + "***" + " 小车当前任务状态：" + dr["taskstate"].ToString() + " 小车当前任务号：" + dr["taskno"].ToString() + " 小车当前任务类型：" + dr["tasktype"].ToString()
                        + "\n" + "***" + list.AgvName + "***" + " 小车当前子任务：" + dr["sid"].ToString()
                        + "\n" + "***" + list.AgvName + "***" + " 小车当前子任务路径：" + dr["id"].ToString()
                        + "\n" + "***" + list.AgvName + "***" + " 小车当前任务货架号：" + dr["shelfno"].ToString() + " 小车当前任务货架固定码值：" + dr["b4"].ToString() + " 小车当前任务货架当前码值：" + dr["b5"].ToString() + " 小车当前任务货架状态（是否锁定）：" + dr["IsLocked"].ToString()
                        + "\n" + "***" + list.AgvName + "***" + " 小车当前点码值：" + dr["b3"].ToString() + " 小车当前点码值是否被占：" + dr["IsOccupy"].ToString() + " 占用小车当前点码值的Agv：" + dr["OccupyAgvNo"].ToString();
                        if (ds.Tables[0].Rows.Count == 2)
                        {

                            DataRow dr1 = ds.Tables[0].Rows[1];
                            str1 += "\n" + "***" + list.AgvName + "***" + " 小车上一条任务终点码值：" + dr1["b2"].ToString() + " 小车上一条任务终点码值是否被占：" + dr1["IsOccupy"].ToString() + " 占用小车上一条任务终点码值的Agv：" + dr1["OccupyAgvNo"].ToString();

                        }
                    }

                }
         
                if ((bytes != 73 && bytes != 4) )
                {
                    list.ReturnBytes = null;
                }
                else
                {
                    list.ReturnBytes = recvBytes;
                }
                if (list.ReturnBytes != null) {
                    System.Console.WriteLine("*******" + list.AgvName + "*******" + list.ReturnBytes[7] + "*******" + bytes);
                }
                else
                {
                    System.Console.WriteLine("*******" + list.AgvName + "*******" + bytes);
                }
            
            }
            catch
            {
                list.ReturnBytes = null;
                
            }
            

            string str = "";
            
            if (list.ReturnBytes != null&&list.ReturnBytes.Length >2) 
            {
                str = list.AgvName + ":" + BitConverter.ToInt32(list.ReSend, 13) + " 电量：" + BitConverter.ToInt16(list.ReSend, 17) + " 状态：" + list.ReSend[19]
                   + " 顶升：" + list.ReSend[20] + " 任务号：" + BitConverter.ToInt32(list.ReSend, 26) + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond
                   + " 任务状态：" + list.ReSend[30] + " 接收功能码：" + list.ReturnBytes[1] + " 动作类型：" + list.ReturnBytes[7]
                   + " 动作1码值：" + BitConverter.ToInt32(list.ReturnBytes, 8) + " 动作2码值：" + BitConverter.ToInt32(list.ReturnBytes, 24)
                   + " 动作3码值：" + BitConverter.ToInt32(list.ReturnBytes, 40) + " 动作2码值：" + BitConverter.ToInt32(list.ReturnBytes, 56)
                   + " 接收时间：" + ts2.ToString() + ":" + ts2.Millisecond + str1;
            }
            else
            {
               str = list.AgvName + ":" + BitConverter.ToInt32(list.ReSend, 13) + " 电量：" + BitConverter.ToInt16(list.ReSend, 17) + " 状态：" + list.ReSend[19]
                   + " 顶升：" + list.ReSend[20] + " 任务号：" + BitConverter.ToInt32(list.ReSend, 26) + " 任务状态：" + list.ReSend[30]
                   + " 发送时间：" + ts1.ToString() + ":" + ts1.Millisecond + str1;
            }

            System.Console.WriteLine(str);
            list.LastReturnBytes = list.ReturnBytes;
            return list;
           
        }

        /// <summary>
        /// 获取数据
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
                    list.V -= (double)50 / 36;
                    break;
                case 13:
                    list.V -= (double)150 / 36;
                    break;
                case 14:
                    list.V += (double)(GetVol(list.V));
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
                    list.ReSend[17] = Dv[0];
                    list.ReSend[18] = Dv[1];
                    list.CurrentBarCode = BitConverter.ToInt32(list.ReSend, 13);
                    byte c = CRC(list.ReSend, 32);
                    list.ReSend[31] = c;
                    list.ReturnBytes = list.ReSend;
                    return list;
                }
                else
                {
                    list.Error = 0;
                    list.State = 11;
                    list.Sid = "";
                    list.Dtype = 0;
                    list.ReturnBytes = new byte[] { 0, 0 };
                }


            }
            if (list.ReturnBytes == null)
            {
                if (list.ReSend != null)
                {
                    byte[] Dv = BitConverter.GetBytes((int)list.V);
                    list.ReSend[17] = Dv[0];
                    list.ReSend[18] = Dv[1];
                    list.CurrentBarCode = BitConverter.ToInt32(list.ReSend, 13);
                    byte c = CRC(list.ReSend, 32);
                    list.ReSend[31] = c;
                    list.ReturnBytes = list.ReSend;
                    return list;
                }
                else
                    list.ReturnBytes = new byte[] { 0, 0 };
            }
           
            switch (list.ReturnBytes[1])
            {
                case 0://心跳
                    list = HeartBeat(list, 0);
                    break;
                case 1://新动作
                    //任务完成
                    //显示新任务内容
                    list = HeartBeat(list, 1);
                    break;
                case 2://重发
                    if (list.State == 13)
                    {
                        string sql = string.Format("update T_Base_AGV set Ct=Ct+1 where AgvNo='{0}'", list.AgvName);
                        DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
                        DalSql.SelectGet(sqlConnect, sql);
                    }
                     byte[] Dv = BitConverter.GetBytes((int)list.V);
                    list.ReSend[17] = Dv[0];
                    list.ReSend[18] = Dv[1];
                    list.CurrentBarCode = BitConverter.ToInt32(list.ReSend, 13);
                    byte c = CRC(list.ReSend, 32);
                    list.ReSend[31] = c;
                    list.ReturnBytes = list.ReSend;
                    //list = HeartBeat(list, 2);
                    break;
                case 3://任务完成
                    list = HeartBeat(list,3);
                    break;
            }
            return list;
            
        }
        private AgvClass HeartBeat(AgvClass list, byte flag)
        {
            byte actiontype = 0;
            byte[] data = new byte[100];
            data[0] = 0;
            data[1] = flag;
            data[2] = 18;
            byte[] agvNo = Encoding.ASCII.GetBytes(list.AgvName);
            for (int index = 3; index < 3 + agvNo.Length; index++)
            {
                data[index] = agvNo[index - 3];
            }

            //电量
            byte[] Dv = BitConverter.GetBytes((int)list.V);
            data[17] = Dv[0];
            data[18] = Dv[1];

            data[19] = list.State;//自身状态
            data[20] = list.Ding; //顶升

            ////当前码值
            //byte[] BarCode = BitConverter.GetBytes(list.CurrentBarCode);
            //data[13] = BarCode[0];
            //data[14] = BarCode[1];
            //data[15] = BarCode[2];
            //data[16] = BarCode[3];
            ////System.Console.WriteLine(list.AgvName + ':'+list.CurrentBarCode);
            ////if (list.CurrentBarCode > 10000)
            ////{
            ////    throw new Exception("wrong");
            ////}

            byte[] DataReceive = list.ReturnBytes;
            byte[] Dataresend = list.ReSend;

            //UInt32 ReceivebarCode = 0;
            //UInt32 SendReceivebarCode = 0;
            //try
            //{
            //    ReceivebarCode = BitConverter.ToUInt32(DataReceive, 8);
            //    SendReceivebarCode = BitConverter.ToUInt32(Dataresend, 13);
            //}
            //catch
            //{
            //    ReceivebarCode = 0;
            //    SendReceivebarCode = 0;
            //}
            
            

            //新动作
            if (flag == 1)
            {

                data[30] = 0;
                actiontype = DataReceive[7];
                UInt32 barCode1 = BitConverter.ToUInt32(DataReceive, 8);
                Int16 xpos1 = BitConverter.ToInt16(DataReceive, 12);
                Int16 ypos1 = BitConverter.ToInt16(DataReceive, 14);
                Int16 xdis1 = BitConverter.ToInt16(DataReceive, 16);
                Int16 ydis1 = BitConverter.ToInt16(DataReceive, 18);
                byte ptype1 = DataReceive[21];
                byte slatype1 = DataReceive[23];


                UInt32 barCode2 = BitConverter.ToUInt32(DataReceive, 24);
                Int16 xpos2 = BitConverter.ToInt16(DataReceive, 28);
                Int16 ypos2 = BitConverter.ToInt16(DataReceive, 30);
                Int16 xdis2 = BitConverter.ToInt16(DataReceive, 32);
                Int16 ydis2 = BitConverter.ToInt16(DataReceive, 34);
                byte ptype2 = DataReceive[37];
                byte slatype2 = DataReceive[39];

                //获取第二个点的条码
                list.CurrentBarCode = BitConverter.ToInt32(DataReceive, 24);
                list.CBarCode = list.CurrentBarCode;



                //当前未读到码，将上一个码值赋给小车
                if (list.CurrentBarCode == 0)
                {
                    list.CurrentBarCode = BitConverter.ToInt32(DataReceive, 8);

                }

            }
            //当前码值
            byte[] BarCode = BitConverter.GetBytes(list.CurrentBarCode);
            data[13] = BarCode[0];
            data[14] = BarCode[1];
            data[15] = BarCode[2];
            data[16] = BarCode[3];
            //System.Console.WriteLine(list.AgvName + ':'+list.CurrentBarCode);
            //if (list.CurrentBarCode > 10000)
            //{
            //    throw new Exception("wrong");
            //}

            //当前agv所在点的点类型
//            int a = 0, b = 0;

//            if (flag == 3)
//            {
                
//                string sql = string.Format(@"SELECT agv.*,point.PointType ,c.ItemName FROM dbo.T_Base_AGV agv
//                                            LEFT JOIN dbo.T_Base_Point point ON agv.Barcode = point.BarCode 
//                                            LEFT JOIN dbo.T_Task_Son son ON agv.AgvNo = son.AgvNo
//                                            LEFT JOIN dbo.T_Type_Config c ON son.STaskType = c.ItemNo where agv.AgvNo='{0}'", list.AgvName);
//                DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
//                DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);
//                DataRow dr = ds.Tables[0].Rows[0];
//                a = int.Parse(dr["PointType"].ToString());
//                b = int.Parse(dr["ItemName"].ToString());
//            }
           
            //小车的顶升及忙碌状态
            if (actiontype == 2)
                list.Ding = 3;
            else if (actiontype == 3)
                list.Ding = 1;
            else if (actiontype == 6)
                list.State = 14;
            else if (actiontype == 7 || (list.State != 14&&actiontype == 0))
                list.State = 11;
            //if (b == 2)
            //    list.Ding = 3;
            //else if (b == 3)
            //    list.Ding = 1;
            //else if (b == 6)
            //    list.State = 14;
            if ((data[1] == 3 || data[1] == 2) && list.State != 14)
            {
                list.State = 11;
            }
            //else if (data[1] == 3 && list.State == 14)
            else if (list.State == 14)
            {
                list.State = 14;
            }
            else if (data[1] == 1  && list.State != 14)
            {
                list.State = 13;
            }
            //else if (data[1] == 1  && list.State == 14)
            //{
            //    list.State = 14;
            //}
            //if (data[1] == 3)
            //    list.State = 11;
            //else if (data[1] == 1)
            //{ list.State = 13; }
            if (list.State == 13)
            {
                //故障
                Random err = new Random();
                int error = err.Next(1, 3600);
                if (error == 1)
                {
                    list.Error = 1;
                    list.ErrorTime = DateTime.Now;
                    errorNum++;
                }
            }
            data[21] = list.Error;
            data[19] = list.State;
            data[20] = list.Ding;
            //是否将任务号也发送回去
            byte[] DTaskNo = new byte[4];

            if (flag == 3)
            {
                DTaskNo = BitConverter.GetBytes(0);
            }
            else if (list.State == 13 && list.CurrentBarCode != 0|| (list.State == 14 && flag!=3))
            {
                if (flag == 0)
                {
                    DTaskNo = BitConverter.GetBytes(int.Parse(list.Sid));
                    data[30] = 1;
                }
                else
                {
                    DTaskNo = BitConverter.GetBytes(BitConverter.ToInt32(DataReceive, 3));
                    data[30] = 1;
                }
            }
            
            for (int index = 26; index < 26 + DTaskNo.Length; index++)
            {
                data[index] = DTaskNo[index - 26];
            }

            byte c = CRC(data, 32);
            data[31] = c;

            list.ReturnBytes = data;
          
            

            return list;
        }

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

        private double GetVol(double vol)
        {
            if (vol < 65)
                return 45;
            else if (vol >= 65 && vol < 75)
                return 30;
            else
                return 20;
        }

        private void BtnAutoStop_Click(object sender, RoutedEventArgs e)
        {
            isClose = false;
            this.Close();
        }

        private void CheckState_Click(object sender, RoutedEventArgs e)
        {
            if (Combox_Address.Text == "" || Combox_Address.Text == null)
            {
                MessageBox.Show("AgvName 不能为空！！！");
            }
            else
            {
                for (int i = 0;i < Alist.Count; i++)
                {
                    if (Alist[i].AgvName == Combox_Address.Text)
                    {
                        Tbox_CurrtBarCode.Text = Alist[i].CBarCode.ToString();

                        ShelfBe.Text = Alist[i].Ding.ToString();
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


            else if (ShelfBe.Text == "" || ShelfBe.Text == null)
            {
                MessageBox.Show("顶升状态 不能为空！！！");
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
                        Alist[i].AgvBar = int.Parse(Tbox_CurrtBarCode.Text);
                        Alist[i].Ding = byte.Parse( ShelfBe.Text);
                        Alist[i].V = int.Parse( Tbox_Voltage.Text);
                        MessageBox.Show("修改成功！！！");
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (Tbox_CurrtBarCode.Text == "" || Tbox_CurrtBarCode.Text == null)
                {
                    MessageBox.Show("未找到此 Agv！！！");
                }
            }

        }

        private ServiceReference1.ModelContractClient Client = new ServiceReference1.ModelContractClient();
        private void Socket_Out_Click(object sender, RoutedEventArgs e)
        {
            string sql = string.Format("SELECT TOP 1 CposCode FROM dbo.Cpos WHERE ShelfNo = '{0}'", tBox_ShelfNo.Text);
            DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
            string index = "";

            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.Sql, sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                index = dr["CposCode"].ToString();
                break;
            }
            string strResult = Client.NewTask("001", index,
          "1234", "3", float.Parse("12"),
          "Server", tBox_EBarCode.Text);

            MessageBox.Show(strResult);
        }

        private void Socket_In_Click(object sender, RoutedEventArgs e)
        {
            string strResult = Client.MoveBack(tBox_ShelfNoIn.Text, tBox_EBarCode2.Text, "1","1");
            MessageBox.Show(strResult);
        }
    }
}
