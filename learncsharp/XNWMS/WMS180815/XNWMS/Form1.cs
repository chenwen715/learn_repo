﻿using com.force.json;
using XNWMS.InfoModel_S;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XNWMS.Common;
using Newtonsoft.Json;

namespace XNWMS
{
    public partial class Form1 : Form
    {
        #region
        DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
        DAL_Comn_Log comnlog = new DAL_Comn_Log();
        List<LoadData> PCLTs = new List<LoadData>();//恒温上架任务
        List<LoadData>LTAll = new List<LoadData>();
        List<LoadData> PNLTs = new List<LoadData>();//非恒温上架任务
        List<LoadData> BLTs = new List<LoadData>();//散件上架任务
        List<LoadData> PCLCs = new List<LoadData>();//恒温库存处理过
        List<LoadData> PCLCAlls = new List<LoadData>();//恒温库存未处理
        List<LoadData> PNLCs = new List<LoadData>();//非恒温库存处理过
        List<LoadData> PNLCAlls = new List<LoadData>();//非恒温库存未处理
        List<LoadData> BLCs = new List<LoadData>();//散件库存未处理
        List<LoadData> BLCAlls = new List<LoadData>();//散件库存处理过
        List<LoadData> insertSendLoadTasksList = new List<LoadData>();
        public static List<LoadData> sendLoadTasksList = new List<LoadData>();//上架任务
        public static List<UnLoadData> insertSendUnloadTasksList = new List<UnLoadData>();//未填充托盘号的下架任务 
        List<UnLoadData> sendUnloadTasksList = new List<UnLoadData>();//下架任务
        List<UnLoadData> FillUnloadTasksList = new List<UnLoadData>();//需要填充的下架任务
        public static List<DeliveryTasks> sendDeliveryTasksList = new List<DeliveryTasks>();//暂存区任务
        List<PNPB> pnpbsList = new List<PNPB>();//料号最小单包对照
        List<UnLoadData> remainUnloadTasksList = new List<UnLoadData>();//遗留下架任务
        List<LoadData> remainLoadTasksList = new List<LoadData>();//遗留上架任务
        System.Threading.Timer threadTimer1;
        System.Threading.Timer threadTimer2;
        System.Threading.Timer threadTimer3;
        System.Threading.Timer threadTimer4;
        string fileName = "";
        public Control ctrl = null;
        WebServiceHost host = null;
        #endregion

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int a = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            Control.CheckForIllegalCrossThreadCalls = false;
            LogWrite.WriteResultMain += new LogWrite.ResultLogDelegate(WriteResult);
            LogWrite.WriteMain += new LogWrite.WriteLogDelegate(WriteLog);
            LogWrite.WriteMain1 += new LogWrite.WriteLog1Delegate(WriteLog1);
            textBox3.Text = Properties.Settings.Default.url1;
            progressBar1.Visible = false;
            textBox2.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            InitTimer();
            InitLoadTask();
            InitUnLoadTask(a, (DateTime.Now.Day + 1) * 86400);
            InitDeliveryTask(a, (DateTime.Now.Day + 1) * 86400);
            Initpnpb();
            startListen();
            button1.Text = "正在监听";
        }

        private void WriteLog(string log)
        {
            richTextBox2.Text += log;
        }
        private void WriteLog1(string log)
        {
            richTextBox1.Text += log;
        }
        private void WriteResult(string log)
        {
            textBox4.Text = log;
        }

        //初始化定时器
        private void InitTimer()
        {
            threadTimer1 = new System.Threading.Timer(new TimerCallback(TimerUp1), null, 0, 1000);//定时下发任务给wcs，1s检测1次
            //threadTimer2 = new System.Threading.Timer(new TimerCallback(TimerUp2), null, 0, 120000);//下载新任务
            //threadTimer3 = new System.Threading.Timer(new TimerCallback(TimerUp3), null, 0, 2000000);//定时创建整托下架任务，2000s发10条，不用
            //threadTimer4 = new System.Threading.Timer(new TimerCallback(TimerUp4), null, 0, 120000);//定时发送遗留任务，120s发10条上架，10条下架
        }
        
        //定时发任务给wcs
        private void TimerUp1(object value)
        {
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            List<LoadData> tmplt = sendLoadTasksList.FindAll(a => a.times == t && a.state == 0);
            if (tmplt.Count != 0)
            {
                for (int i = 0; i < tmplt.Count; i++)
                {
                    SendLoadTask(tmplt[i]);
                }

            }
            List<UnLoadData> tmpult = insertSendUnloadTasksList.FindAll(a => a.times == (t - 2) && a.state == 0);
            if (tmpult.Count != 0)
            {
                for (int j = 0; j < tmpult.Count; j++)
                {
                    FillUnLoadTask(tmpult[j]);
                }
                insertSendUnloadTasksList.RemoveAll(a=>a.state==1);
            }

            List<DeliveryTasks> tmpdt = sendDeliveryTasksList.FindAll(a => a.Times == t && a.state == 0);
            if (tmpdt.Count != 0)
            {
                for (int z= 0; z < tmpdt.Count; z++)
                {
                    SendDeliveryTask(tmpdt[z]);
                }
            }
        }

        //定时下载新数据
        private void TimerUp2(object value)
        {
            int a = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            if ((DateTime.Now.Day * 86400 - 120) < a && DateTime.Now.Day * 86400 >=a)
            {
                InitLoadTask();
                InitUnLoadTask(a, (DateTime.Now.Day + 1) * 86400);
                InitDeliveryTask(a, (DateTime.Now.Day + 1) * 86400);
            }
        }
       
        //初始化下载未发送的上架任务（当天及第二天任务）
        private void InitLoadTask()
        {
            try
            {
                int loadTaskNumber = 0;
                int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                //string selectString = string.Format(@"select * from dbo.T_Task_Load where state=0 AND Times>={0} AND Times<{1}", t-10, (DateTime.Now.Day + 1) * 86400);
                string selectString = string.Format(@"select * from dbo.T_Task_Load where state=0 AND Times>={0} AND Times<{1}", t,(DateTime.Now.Day + 1) * 86400);
                DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, selectString);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LoadData pclt = new LoadData();
                        pclt.times = Convert.ToInt32(dr["Times"].ToString());
                        pclt.state = Convert.ToInt32(dr["State"].ToString());
                        pclt.taskNo = dr["TaskNo"].ToString();
                        pclt.palletOrBoxNo = dr["SN"].ToString();
                        pclt.isHengWen = Convert.ToBoolean(dr["IsHengwen"]);
                        pclt.bdNo = dr["BDNO"].ToString();
                        pclt.pNo = dr["Pn"].ToString();
                        pclt.weight = Convert.ToInt32(dr["Weight"]);
                        pclt.height = Convert.ToInt32(dr["Height"]);
                        pclt.palletOrBoxNo_Old = dr["SN_OLD"].ToString();
                        if (sendLoadTasksList.Find(a => a.taskNo == pclt.taskNo) == null)
                        {
                            sendLoadTasksList.Add(pclt);
                            loadTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 下载当天" + loadTaskNumber + "条上架任务成功" + "\n");
                    comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 下载当天" + loadTaskNumber + "条上架任务成功" + "\n");
                    LogWrite.WriteLogToMain1("======等待下发上架任务======\n");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
        }

        //初始化下载未发送的下架任务（两天内）
        private void InitUnLoadTask(int t1,int t2)
        {
            try
            {
                int unloadTaskNumber = 0,unFillUnLoadTaskNumber=0;
                int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                //string selectString1 = string.Format(@"select * from dbo.T_Task_UnLoad where state=0 AND Times>={0} AND Times<{1} and isnull(HubNo,'')<>'' and isnull(SN,'')<>''", t, (DateTime.Now.Day + 1) * 86400);
                string selectString1 = string.Format(@"select * from dbo.T_Task_UnLoad where State=0 AND Times>={0} AND Times<{1} AND isnull(Sign,'')<>''", t1,t2);
                DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, selectString1);
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        UnLoadData ult = new UnLoadData();
                        ult.times = Convert.ToInt32(dr["Times"].ToString());
                        ult.state = Convert.ToInt32(dr["State"].ToString());
                        ult.sign = dr["Sign"].ToString();
                        ult.taskNo = dr["TaskNo"].ToString();
                        ult.taskType = dr["TaskType"].ToString();
                        ult.taskState = dr["TaskState"].ToString();
                        ult.priority = Convert.ToInt32(dr["Priority"].ToString());
                        ult.outNo = dr["OutNo"].ToString();
                        ult.planningNo = dr["PlanningNo"].ToString();
                        ult.palletOrBoxNo = dr["SN"].ToString();
                        ult.hubno = dr["HubNo"].ToString();
                        ult.isUnpackTray = Convert.ToBoolean(dr["IsUnpackTray"]);
                        ult.isBoxLable = Convert.ToBoolean(dr["IsBoxLable"]);
                        ult.isMinpackLable = Convert.ToBoolean(dr["IsMinpackLable"]);
                        ult.isPipeline = Convert.ToBoolean(dr["IsPipeline"]);
                        ult.SN_OLD = dr["SN_OLD"].ToString();
                        ult.bdNo= dr["BDNo"].ToString();
                        ult.pNo= dr["PNo"].ToString();
                        ult.station = dr["Station"].ToString();
                        if (sendUnloadTasksList.Find(a => a.taskNo == ult.taskNo) == null)
                        {
                            sendUnloadTasksList.Add(ult);
                            unloadTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }

                    }
                    if (unloadTaskNumber != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 新下载" + unloadTaskNumber + "条下架任务成功" + "\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 新下载" + unloadTaskNumber + "条下架任务成功" + "\n");
                        LogWrite.WriteLogToMain1("======等待下发下架任务======\n");
                    }
                }

                string selectString2 = string.Format(@"select o.*,m.miniPerBag from dbo.T_Task_UnLoad_Origin o
LEFT JOIN T_Biz_PNo_MiniNumber m ON m.pNo=o.PNo where State=0 AND Times>={0} AND Times<{1} AND isnull(Sign,'')<>'' ", t, (DateTime.Now.Day + 1) * 86400);
                DataSet ds2 = comnsql.SelectGet(Properties.Settings.Default.sql, selectString2);
                if (ds2.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        UnLoadData unloadTask = new UnLoadData();
                        unloadTask.state = 0;
                        unloadTask.times = Convert.ToInt32(dr["Times"]);
                        unloadTask.outNo = dr["OutNo"].ToString();
                        unloadTask.bdNo = dr["BDNo"].ToString();
                        unloadTask.pNo = dr["PNo"].ToString();
                        unloadTask.outNumber = Convert.ToInt32(dr["Number"].ToString());
                        unloadTask.miniPerBag = Convert.ToInt32(dr["miniPerBag"].ToString());
                        unloadTask.businessType = dr["BusinessType"].ToString();
                        unloadTask.sign = dr["Sign"].ToString();
                        if (Form1.insertSendUnloadTasksList.Find(a => a.sign == unloadTask.sign) == null)
                        {
                            insertSendUnloadTasksList.Add(unloadTask);
                            unFillUnLoadTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }
                        if (unFillUnLoadTaskNumber != 0)
                        {
                            LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 新下载" + unFillUnLoadTaskNumber + "条未填充下架任务成功" + "\n");
                            comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 新下载" + unFillUnLoadTaskNumber + "条未填充下架任务成功" + "\n");
                            LogWrite.WriteLogToMain1("======等待下发下架任务======\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
        }

        //初始化下载未发送的delivery任务（两天内）
        private void InitDeliveryTask(int t1, int t2)
        {
            try
            {
                int deliveryTaskNumber = 0;
                int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                string selectString1 = string.Format(@"select * from dbo.T_Task_UnLoad where state=0 AND Times>={0} AND Times<{1} AND isnull(Sign,'')=''",t1,t2);
                DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, selectString1);
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        DeliveryTasks dtask = new DeliveryTasks();
                        dtask.Times = Convert.ToInt32(dr["Times"].ToString());
                        dtask.taskNo = dr["TaskNo"].ToString();
                        dtask.taskType = dr["TaskType"].ToString();
                        string SNAll = dr["SN"].ToString();
                        string[] sn = SNAll.Split(',');
                        dtask.SN = new List<string>();
                        foreach (string s in sn)
                        {
                            if (dtask.SN.FindAll(a => a == s).Count == 0)
                            {
                                dtask.SN.Add(s);
                            }
                        }                        
                        dtask.state = 0;
                        if (Form1.sendDeliveryTasksList.Find(a => a.taskNo == dtask.taskNo) == null)
                        {
                            Form1.sendDeliveryTasksList.Add(dtask);
                            deliveryTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (deliveryTaskNumber != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 新下载" + deliveryTaskNumber + "条出货暂存区下架任务成功" + "\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 新下载" + deliveryTaskNumber + "条出货暂存区下架任务成功" + "\n");
                        LogWrite.WriteLogToMain1("======等待下发出货暂存区下架任务======\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
        }

        //打开要读取的Excel
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "请选择文件夹";
            openFileDialog1.InitialDirectory = "d:\\";
            openFileDialog1.Filter = "ext files (*.xlsx)|*.xlsx|All files(*.*)|*>**";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                fileName = textBox1.Text;
                progressBar1.Visible = true;
                textBox2.Visible = true;
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始读取Excel文件：" + textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1] + "数据\n");
                comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始读取Excel文件：" + textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1] + "数据\n");
                button2.Enabled = false;
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        //读取Excel数据
        public void GetSheetData()
        {

            try
            {
                //先将所有任务写到数据库（WMS任务）中，发送成功的任务flag变为1，下次只发flag为0的值               
                if (string.IsNullOrEmpty(fileName))
                {
                    MessageBox.Show("请选择文件");
                    return;
                }
                if (fileName.Contains("WarehouseData"))
                {
                    ManageLoadData();
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 读取上架Excel数据完成\n");
                    comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 读取上架Excel数据完成\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 读取上架Excel数据完成\n";
                    if (PCLCs.Count != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入恒温库存\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入恒温库存\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入恒温库存\n";
                        InsertContainer("Pallet", PCLCs, PCLCAlls);
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入恒温库存完成\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入恒温库存完成\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 插入恒温库存完成\n";
                    }
                    if (PNLCs.Count != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入非恒温库存\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入非恒温库存\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入非恒温库存\n";
                        InsertContainer("Pallet", PNLCs, PNLCAlls);
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入非恒温库存完成\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入非恒温库存完成\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 插入非恒温库存完成\n";
                    }
                    if (BLCs.Count != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入散件库存\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入散件库存\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入散件库存\n";
                        InsertContainer("Bulks", BLCs, BLCAlls);
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入散件库存完成\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入散件库存完成\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 插入散件库存完成\n";
                    }
                    if (BLTs.Count != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入散件上架信息\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入散件上架信息\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入散件库存\n";
                        InsertContainer("Bulktask", null, BLTs);
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入散件上架信息完成\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入散件上架信息完成\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 插入散件库存完成\n";
                    }
                    if (insertSendLoadTasksList.Count != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入上架任务\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入上架任务\n");
                        //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入上架任务\n";
                        InsertSql("load", insertSendLoadTasksList,LTAll);//插入上架任务
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入上架任务完成\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入上架任务完成\n");
                        InitLoadTask();                      
                        //richTextBox1.Text += DateTime.Now.ToString() + " 插入上架任务完成\n";
                    }
                }
                else if (fileName.Contains("Unloading"))
                {
                    ManageUnLoadData();
                }
                button2.Enabled = true;
                //button1.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                textBox2.Text = "完成";
                string file = textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1];
                textBox1.Text = "";
                progressBar1.Visible = false;
                textBox2.Visible = false;
                //radioButton1.Visible = true;
                //radioButton2.Visible = true;
                //richTextBox1.Text += DateTime.Now.ToString() + " 处理Excel文件:" + file + " 完成" + "\n";
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 处理Excel文件:" + file + " 完成" + "\n");
                comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 处理Excel文件:" + file + " 完成" + "\n");
                //richTextBox1.Text += "======等待下发任务======\n";

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
        }

        //处理多条上架数据
        private void ManageLoadData()
        {
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            DataSet ds = new DataSet();
            string strExtension = System.IO.Path.GetExtension(textBox1.Text);
            string strConn = "";
            if (strExtension == ".xls")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + textBox1.Text + ";" + "Extended Properties=Excel 8.0;";
            }
            else if (strExtension == ".xlsx")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + textBox1.Text + ";" + "Extended Properties=Excel 12.0;";
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

            for (int n = 0; n < schemaTable.Rows.Count; n++)
            {
                if (schemaTable.Rows[n][2].ToString().Trim().EndsWith("$") && schemaTable.Rows[n][2].ToString().Trim().Contains("PalletLoading"))
                {
                    string sheetName = schemaTable.Rows[n][2].ToString().Trim();
                    strExcel = string.Format("select * from [{0}]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, sheetName);
                    conn.Close();
                    DataTable dt = ds.Tables[sheetName];
                    int finishNumber = 0;
                    int totalNumber = dt.Rows.Count;
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = totalNumber;
                    progressBar1.Step = 1;
                    for (int i = 0; i < totalNumber; i++)
                    {
                        LoadData pclt = new LoadData();
                        LoadData pclt1 = new LoadData();
                        pclt = manage(pclt, sheetName, date, i, dt);
                        pclt1 = manage(pclt1, sheetName, date, i, dt);

                        if (sheetName.Contains("BulksInitial"))
                        {
                            BLCAlls.Add(pclt1);
                            InsertList(sheetName, BLCs, pclt);
                        }
                        else if (sheetName.Contains("BulksLoading"))
                        {
                            //LTAll.Add(pclt1);
                            InsertList(sheetName, BLTs, pclt);
                            //InsertList(sheetName, insertSendLoadTasksList, pclt);
                        }
                        else if (sheetName.Contains("PalletInitialInventoryConstant"))
                        {
                            PCLCAlls.Add(pclt1);
                            InsertList(sheetName, PCLCs, pclt);
                        }
                        else if (sheetName.Contains("PalletInitiaInventoryNoConstant"))
                        {
                            PNLCAlls.Add(pclt1);
                            InsertList(sheetName, PNLCs, pclt);
                        }
                        else if (sheetName.Contains("PalletLoadingConstant"))
                        {
                            LTAll.Add(pclt1);
                            //InsertList(sheetName, PCLTs, pclt);
                            InsertList(sheetName, insertSendLoadTasksList, pclt);
                        }
                        else if (sheetName.Contains("PalletLoadingNoConstant"))
                        {
                            LTAll.Add(pclt1);
                            //InsertList(sheetName, PNLTs, pclt);
                            InsertList(sheetName, insertSendLoadTasksList, pclt);
                        }

                        finishNumber++;
                        progressBar1.PerformStep();
                        if (finishNumber == totalNumber)
                        {
                            textBox2.Text = string.Format(@"{0}:读取数据完成，共{1}条", sheetName, totalNumber);
                            if (n < schemaTable.Rows.Count - 1)
                            {
                                progressBar1.Value = 0;
                                progressBar1.Maximum = 1;
                            }
                        }
                        else
                        {
                            textBox2.Text = string.Format(@"读取{0}数据中，已完成{1}/{2}", sheetName, finishNumber, totalNumber);
                        }
                    }
                }
            }
            progressBar1.Visible = false;
            textBox2.Visible = false;
        }
        
        //处理多条下架数据
        private void ManageUnLoadData()
        {
            progressBar1.Visible = false;
            textBox2.Visible = false;
            //string[] unloadt = { "Times", "State", "TaskNo", "TaskType", "TaskState", "Priority", "OutNo", "PlanningNo", "SN",
            //                       "HubNo", "IsUnpackTray", "IsBoxLable","IsMinpackLable","IsPipeline","SN_OLD" ,"Number","BusinessType","BDNo","PNo","Station","CreateTime"};
            string[] unloadt = { "Times", "State", "Sign", "OutNo","Number","BusinessType","BDNo","PNo","CreateTime"};
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            DataSet ds = new DataSet();
            string strExtension = System.IO.Path.GetExtension(textBox1.Text);
            string strConn = "";
            if (strExtension == ".xls")
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + textBox1.Text + ";" + "Extended Properties=Excel 8.0;";
            }
            else if (strExtension == ".xlsx")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + textBox1.Text + ";" + "Extended Properties=Excel 12.0;";
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            DataTable dtpnpb = new DataTable();
            dtpnpb.Columns.Add("pNo", typeof(string));
            dtpnpb.Columns.Add("miniPerBag", typeof(int));

            DataTable dtunloadTask = new DataTable();
            dtunloadTask.Columns.Add("Times", typeof(int));
            dtunloadTask.Columns.Add("State", typeof(int));
            //dtunloadTask.Columns.Add("TaskNo", typeof(string));
            dtunloadTask.Columns.Add("Sign", typeof(string));
            //dtunloadTask.Columns.Add("TaskType", typeof(string));
            //dtunloadTask.Columns.Add("TaskState", typeof(string));
            //dtunloadTask.Columns.Add("Priority", typeof(int));
            dtunloadTask.Columns.Add("OutNo", typeof(string));
            //dtunloadTask.Columns.Add("PlanningNo", typeof(string));
            //dtunloadTask.Columns.Add("SN", typeof(string));
            //dtunloadTask.Columns.Add("HubNo", typeof(string));
            //dtunloadTask.Columns.Add("IsUnpackTray", typeof(bool));
            //dtunloadTask.Columns.Add("IsBoxLable", typeof(bool));
            //dtunloadTask.Columns.Add("IsMinpackLable", typeof(bool));
            //dtunloadTask.Columns.Add("IsPipeline", typeof(bool));
            //dtunloadTask.Columns.Add("SN_OLD", typeof(string));
            dtunloadTask.Columns.Add("Number", typeof(int));
            dtunloadTask.Columns.Add("BusinessType", typeof(string));
            dtunloadTask.Columns.Add("BDNo", typeof(string));
            dtunloadTask.Columns.Add("PNo", typeof(string));
            //dtunloadTask.Columns.Add("Station", typeof(string));
            dtunloadTask.Columns.Add("CreateTime", typeof(string));
            for (int n = 0; n < schemaTable.Rows.Count; n++)
            {
                if (schemaTable.Rows[n][2].ToString().Trim().EndsWith("Unload$"))
                {
                    string sheetName = schemaTable.Rows[n][2].ToString().Trim();
                    strExcel = string.Format("select * from [{0}]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, sheetName);
                    conn.Close();
                    DataTable dt = ds.Tables[sheetName];
                    int totalNumber = dt.Rows.Count;
                    button2.Enabled = false;
                    //string sql = string.Format(@"SELECT TOP 1 TaskNo FROM {0} WHERE TaskNo LIKE '%{1}%' ORDER BY TaskNo DESC", "dbo.T_Task_UnLoad", date);
                    //string taskno = "";
                    //if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
                    //{
                    //    taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
                    //}
                    //SqlParameter[] paras = new SqlParameter[1];
                    //paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
                    //paras[0].Direction = ParameterDirection.Output;
                    //string taskno = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewUnLoadTaskNo", paras, "@NEW_ID").ToString();
                    string sql = string.Format(@"SELECT TOP 1 Sign FROM {0} WHERE Sign LIKE '%{1}%' ORDER BY Sign DESC", "dbo.T_Task_UnLoad_Origin", date);
                    string taskno = "";
                    if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
                    {
                        taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
                    }

                    string number = "";
                    for (int i = 0; i < totalNumber; i++)
                    {
                        if (!string.IsNullOrEmpty(taskno))
                        {
                            number = date + (int.Parse(taskno.Substring(taskno.Length - 6, 6)) + i + 1).ToString().PadLeft(6, '0');
                        }
                        else
                        {
                            number = date + ((i + 1).ToString().PadLeft(6, '0'));

                        }
                        string TaskNo = "UL" + number;

                        UnLoadData unloadTask = new UnLoadData();
                        unloadTask.state = 0;
                        unloadTask.times = Convert.ToInt32(dt.Rows[i][0].ToString().Split('.')[0]);
                        unloadTask.outNo = dt.Rows[i][1].ToString();
                        unloadTask.bdNo = dt.Rows[i][2].ToString();
                        unloadTask.pNo = dt.Rows[i][3].ToString();
                        unloadTask.outNumber = Convert.ToInt32(dt.Rows[i][5].ToString().Trim());
                        unloadTask.miniPerBag = Convert.ToInt32(dt.Rows[i][6].ToString().Trim());
                        unloadTask.businessType = dt.Rows[i][7].ToString();
                        unloadTask.sign = TaskNo;
                        //Random rnd = new Random();
                        //unloadTask.isPipeline = Convert.ToBoolean(rnd.Next(2));
                        insertSendUnloadTasksList.Add(unloadTask);
                       
                        DataRow drunload = dtunloadTask.NewRow();
                        drunload["Times"] = unloadTask.times;
                        drunload["State"] = unloadTask.state;
                        //drunload["TaskNo"] = unloadTask.taskNo;
                        drunload["Sign"] = unloadTask.sign;
                        //drunload["TaskType"] = "OUT";
                        //drunload["TaskState"] = "NORMAL";
                        //drunload["Priority"] = 1;
                        drunload["OutNo"] = unloadTask.outNo;
                        //drunload["PlanningNo"] = "";
                        //drunload["SN"] = "";
                        //drunload["HubNo"] = "";
                        //drunload["IsUnpackTray"] = 0;
                        //drunload["IsBoxLable"] = 0;
                        //drunload["IsMinpackLable"] = -1;
                        //drunload["IsPipeline"] = unloadTask.isPipeline;
                        //drunload["SN_OLD"] = "";
                        drunload["Number"] = unloadTask.outNumber;
                        drunload["BusinessType"] = unloadTask.businessType;
                        drunload["BDNo"] = unloadTask.bdNo;
                        drunload["PNo"] = unloadTask.pNo;
                        //drunload["Station"] = "";
                        drunload["CreateTime"] = DateTime.Now.ToString();
                        dtunloadTask.Rows.Add(drunload);

                        PNPB pnpb = new PNPB();
                        pnpb.state = 0;
                        pnpb.pNo = dt.Rows[i][3].ToString();
                        pnpb.miniPerBag = Convert.ToInt32(dt.Rows[i][6].ToString());
                        if (pnpbsList.Count == 0)
                        {
                            pnpbsList.Add(pnpb);
                            DataRow dr = dtpnpb.NewRow();
                            dr["pNo"] = dt.Rows[i][3].ToString();
                            dr["miniPerBag"] = Convert.ToInt32(dt.Rows[i][6].ToString());
                            dtpnpb.Rows.Add(dr);
                        }
                        else
                        {
                            if (pnpbsList.Find(a => a.pNo == pnpb.pNo) != null)
                            {
                                continue;
                            }
                            else
                            {
                                pnpbsList.Add(pnpb);
                                DataRow dr = dtpnpb.NewRow();
                                dr["pNo"] = dt.Rows[i][3].ToString();
                                dr["miniPerBag"] = Convert.ToInt32(dt.Rows[i][6].ToString());
                                dtpnpb.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 读取下架Excel数据完成\n");
            comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 读取下架Excel数据完成\n");
            //richTextBox1.Text += DateTime.Now.ToString() + " 读取下架Excel数据完成\n";

            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(Properties.Settings.Default.sql))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "dbo.T_Biz_PNo_MiniNumber";
                sqlBC.ColumnMappings.Add("pNo", "pNo");
                sqlBC.ColumnMappings.Add("miniPerBag", "miniPerBag");
                sqlBC.WriteToServer(dtpnpb);
                List<PNPB> tmppnpb = pnpbsList.FindAll(a => a.state == 0);
                foreach (PNPB p in tmppnpb)
                {
                    p.state = 1;
                }
            }
            LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 开始插入下架任务\n");
            comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 开始插入下架任务\n");
            //richTextBox1.Text += DateTime.Now.ToString() + " 开始插入下架任务\n";
            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(Properties.Settings.Default.sql))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "dbo.T_Task_UnLoad_Origin";
                for (int j = 0; j < unloadt.Length; j++)
                {
                    sqlBC.ColumnMappings.Add(unloadt[j], unloadt[j]);
                }
                sqlBC.WriteToServer(dtunloadTask);
            }
            string ss = @" INSERT INTO T_Tmp_Order(OutNo,State,Count) SELECT OutNo,0 ,count(1) FROM T_Task_UnLoad_Origin WHERE State<>99 GROUP BY OutNo";
            comnsql.DataOperator(Properties.Settings.Default.sql, ss);
            LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入下架任务完成\n");
            comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入下架任务完成\n");
            
            //InitUnLoadTask();
            //richTextBox1.Text += DateTime.Now.ToString() + " 插入下架任务完成\n";
        }

        //下载数据库中已有的料号-最小单包
        private void Initpnpb()
        {
            string s = string.Format("SELECT pNo, miniPerBag FROM dbo.T_Biz_PNo_MiniNumber");
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, s);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PNPB pn = new PNPB();
                pn.pNo = dr["pNo"].ToString();
                pn.miniPerBag = Convert.ToInt32(dr["miniPerBag"]);
                pn.state = 1;
                pnpbsList.Add(pn);
            }
        }    
       
        //处理单条上架任务数据
        private LoadData manage(LoadData pclt, string sheetName, string date, int i, DataTable dt)
        {
            pclt.state = 0;
            if (sheetName.Contains("NoConstant"))
            {
                pclt.isHengWen = false;
            }
            else
            {
                pclt.isHengWen = true;
            }
            if (sheetName.Contains("PalletInitiaInventoryNoConstant"))
            {
                pclt.palletOrBoxNo = "PXN" + date.Remove(0, 2) + dt.Rows[i][0].ToString().PadLeft(6, '0');
            }
            else if (sheetName.Contains("PalletInitialInventoryConstant"))
            {
                pclt.palletOrBoxNo = "PXC" + date.Remove(0, 2) + dt.Rows[i][0].ToString().PadLeft(6, '0');
            }
            else
            {
                pclt.palletOrBoxNo = "PX" + date.Remove(0, 2) + dt.Rows[i][1].ToString().PadLeft(6, '0');
            }
            if (sheetName.Contains("Pallet") && sheetName.Contains("Initia"))
            {

                if (dt.Rows[i][7].ToString() == "Y")
                {
                    pclt.isOverWeight = true;
                }
                else if (dt.Rows[i][7].ToString() == "N")
                {
                    pclt.isOverWeight = false;
                }
            }
            else if (sheetName.Contains("Pallet") && !sheetName.Contains("Initia"))
            {
                pclt.times = Convert.ToInt32(dt.Rows[i][0].ToString().Split('.')[0]);
                if (dt.Rows[i][8].ToString() == "Y")
                {
                    pclt.isOverWeight = true;
                }
                else if (dt.Rows[i][8].ToString() == "N")
                {
                    pclt.isOverWeight = false;
                }
            }
            else if (sheetName.Contains("Bulks") && sheetName.Contains("Initia"))
            {
                pclt.palletOrBoxNo = "CX" + date.Remove(0, 2) + dt.Rows[i][0].ToString().PadLeft(6, '0');
            }
            else if (sheetName.Contains("Bulks") && !sheetName.Contains("Initia"))
            {
                pclt.palletOrBoxNo = "CX" + date.Remove(0, 2) + dt.Rows[i][1].ToString().PadLeft(6, '0');
                pclt.times = Convert.ToInt32(dt.Rows[i][0].ToString().Split('.')[0]);
            }
            if (sheetName.Contains("Initia"))
            {
                pclt.bdNo = dt.Rows[i][1].ToString();
                pclt.pNo = dt.Rows[i][3].ToString();
                pclt.number = Convert.ToInt32(dt.Rows[i][6].ToString());
                pclt.numberDtl = dt.Rows[i][6].ToString();
            }
            else
            {
                pclt.bdNo = dt.Rows[i][2].ToString();
                pclt.pNo = dt.Rows[i][4].ToString();
                pclt.number = Convert.ToInt32(dt.Rows[i][7].ToString());
                pclt.numberDtl = dt.Rows[i][7].ToString();
            }

            int[] height = { 1299 };
            Random r = new Random();
            int b = r.Next(0, height.Length);
            pclt.height = height[b];

            return pclt;
        }

        //将从Excel中读取的数据插入内存中
        private void InsertList(string sheetName, List<LoadData> List, LoadData task)
        {
            if (List.Count == 0)
            {
                List.Add(task);
            }
            else
            {
                for (int j = 0; j < List.Count(); j++)
                {
                    if (List[j].palletOrBoxNo == task.palletOrBoxNo && List[j].isHengWen == task.isHengWen && List[j].times == task.times)
                    {
                        List[j].bdNo += "," + task.bdNo;
                        List[j].pNo += "," + task.pNo;
                        List[j].number = List[j].number + task.number;
                        List[j].numberDtl += "," + task.number.ToString();
                        if (sheetName.Contains("Pallet"))
                        {
                            if (List[j].isOverWeight || task.isOverWeight)
                            {
                                List[j].isOverWeight = true;
                            }
                            else
                            {
                                List[j].isOverWeight = false;
                            }
                            if (List[j].height < task.height)
                            {
                                List[j].height = task.height;
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (j == (List.Count() - 1))
                        {
                            List.Add(task);
                            break;
                        }
                    }
                }
            }
        }

        //插入数据库任务表
        private void InsertSql(string sheetName, List<LoadData> list, List<LoadData> alllist)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Times", typeof(int));
            dt.Columns.Add("State", typeof(int));
            dt.Columns.Add("TaskNo", typeof(string));
            dt.Columns.Add("SN", typeof(string));
            dt.Columns.Add("PlanningNo", typeof(string));
            dt.Columns.Add("IsHengwen", typeof(int));
            dt.Columns.Add("IsBLOCK", typeof(int));
            dt.Columns.Add("TaskType", typeof(int));
            dt.Columns.Add("BDNO", typeof(string));
            dt.Columns.Add("Pn", typeof(string));
            dt.Columns.Add("Weight", typeof(int));
            dt.Columns.Add("Height", typeof(int));
            dt.Columns.Add("SN_OLD", typeof(string));
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("NumberDtl", typeof(string));
            dt.Columns.Add("CreateTime", typeof(string));
            //string tableName = "dbo.T_Task_Load";
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            //string sql = string.Format(@"SELECT TOP 1 TaskNo FROM {0} WHERE TaskNo LIKE '%{1}%' ORDER BY TaskNo DESC", tableName, date);
            //string taskno = "";
            //if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
            //{
            //    taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
            //}
            for (int i = 0; i < list.Count; i++)
            {
                SqlParameter[] paras = new SqlParameter[1];
                paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
                paras[0].Direction = ParameterDirection.Output;
                string TaskNo = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewLoadTaskNo", paras, "@NEW_ID").ToString();

                int weight;
                if (list[i].isOverWeight)
                {
                    weight = 500;
                    list[i].weight = weight;
                }
                else
                {
                    weight = 200;
                    list[i].weight = weight;
                }
                list[i].taskNo = TaskNo;
                DataRow dr = dt.NewRow();
                dr["Times"] = list[i].times;
                dr["State"] = list[i].state;
                dr["TaskNo"] = list[i].taskNo;
                dr["SN"] = list[i].palletOrBoxNo;
                dr["PlanningNo"] = "[\"\"]";
                dr["IsHengwen"] = Convert.ToInt16(list[i].isHengWen);
                dr["IsBLOCK"] = 1;
                dr["TaskType"] = -1;
                dr["BDNO"] = list[i].bdNo;
                dr["Pn"] = list[i].pNo;
                dr["Weight"] = list[i].weight;
                dr["Height"] = list[i].height;
                dr["SN_OLD"] = "";
                dr["Number"] = list[i].number;
                dr["NumberDtl"] = list[i].numberDtl;
                dr["CreateTime"] = DateTime.Now.ToString(); 
                dt.Rows.Add(dr);
                list[i].state = 1;
            }
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.sql);
            conn.Open();
            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "dbo.T_Task_Load";
                sqlBC.ColumnMappings.Add("Times", "Times");
                sqlBC.ColumnMappings.Add("State", "State");
                sqlBC.ColumnMappings.Add("TaskNo", "TaskNo");
                sqlBC.ColumnMappings.Add("SN", "SN");
                sqlBC.ColumnMappings.Add("PlanningNo", "PlanningNo");
                sqlBC.ColumnMappings.Add("IsHengwen", "IsHengwen");
                sqlBC.ColumnMappings.Add("IsBLOCK", "IsBLOCK");
                sqlBC.ColumnMappings.Add("TaskType", "TaskType");
                sqlBC.ColumnMappings.Add("BDNO", "BDNO");
                sqlBC.ColumnMappings.Add("Pn", "Pn");
                sqlBC.ColumnMappings.Add("Weight", "Weight");
                sqlBC.ColumnMappings.Add("Height", "Height");
                sqlBC.ColumnMappings.Add("SN_OLD", "SN_OLD");
                sqlBC.ColumnMappings.Add("Number", "Number");
                sqlBC.ColumnMappings.Add("NumberDtl", "NumberDtl");
                sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                sqlBC.WriteToServer(dt);
            }
            conn.Dispose();
            list.RemoveAll(a => a.state == 1);

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Warecell", typeof(string));
            dt1.Columns.Add("PalletNo", typeof(string));
            dt1.Columns.Add("BoxNo", typeof(string));
            dt1.Columns.Add("BDNo", typeof(string));
            dt1.Columns.Add("PNo", typeof(string));
            dt1.Columns.Add("Number", typeof(int));
            dt1.Columns.Add("IsOverWeight", typeof(int));
            dt1.Columns.Add("IsHengWen", typeof(int));
            dt1.Columns.Add("IsQualified", typeof(int));
            dt1.Columns.Add("State", typeof(int));
            dt1.Columns.Add("Station", typeof(string));
            dt1.Columns.Add("Length", typeof(float));
            dt1.Columns.Add("Width", typeof(float));
            dt1.Columns.Add("Height", typeof(float));
            dt1.Columns.Add("Weight", typeof(float));
            dt1.Columns.Add("CreateTime", typeof(string));
            dt1.Columns.Add("UpdateTime", typeof(string));
            //string selectstring = string.Format(@"SELECT TOP 1 BoxNo FROM dbo.T_Biz_Container_PalletDtl WHERE BoxNo LIKE '%{0}%' ORDER BY BoxNo DESC", date);
            //string boxno = "",BoxNo1="";
            //if (comnsql.GetData(Properties.Settings.Default.sql, selectstring)!=null)
            //{
            //    boxno = comnsql.GetData(Properties.Settings.Default.sql, selectstring).ToString();
            //}
            for (int j = 0; j < alllist.Count; j++)
            {
                //if (string.IsNullOrEmpty(boxno))
                //{
                //    BoxNo1 = "CP" + date + (j + 1).ToString().PadLeft(6, '0');
                //}
                //else
                //{
                //    BoxNo1 = "CP" + date + (Convert.ToInt32(boxno.Substring(boxno.Length-6,6))+(j + 1)).ToString().PadLeft(6, '0');
                //}
                DataRow dr1 = dt1.NewRow();
                dr1["Warecell"] = "";
                dr1["PalletNo"] = alllist[j].palletOrBoxNo;
                //dr1["BoxNo"] = BoxNo1;
                dr1["BoxNo"] = "";
                dr1["BDNo"] = alllist[j].bdNo;
                dr1["PNo"] = alllist[j].pNo;
                dr1["Number"] = alllist[j].number;
                dr1["IsOverWeight"] = alllist[j].isOverWeight;
                dr1["IsHengWen"] = alllist[j].isHengWen;
                dr1["IsQualified"] = true;
                dr1["State"] = 0;
                dr1["Station"] = "";
                dr1["Length"] = 0;
                dr1["Width"] = 0;
                dr1["Height"] = 0;
                dr1["Weight"] = 0;
                dr1["CreateTime"] = DateTime.Now.ToString();
                dr1["UpdateTime"] = null;
                dt1.Rows.Add(dr1);
                alllist[j].state = 1;
            }
            SqlConnection conn1 = new SqlConnection(Properties.Settings.Default.sql);
            conn1.Open();
            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn1))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "dbo.T_Biz_Container_PalletDtl";
                sqlBC.ColumnMappings.Add("Warecell", "Warecell");
                sqlBC.ColumnMappings.Add("PalletNo", "PalletNo");
                sqlBC.ColumnMappings.Add("BoxNo", "BoxNo");
                sqlBC.ColumnMappings.Add("BDNo", "BDNo");
                sqlBC.ColumnMappings.Add("PNo", "PNo");
                sqlBC.ColumnMappings.Add("Number", "Number");
                sqlBC.ColumnMappings.Add("IsOverWeight", "IsOverWeight");
                sqlBC.ColumnMappings.Add("IsHengWen", "IsHengWen");
                sqlBC.ColumnMappings.Add("IsQualified", "IsQualified");
                sqlBC.ColumnMappings.Add("State", "State");
                sqlBC.ColumnMappings.Add("Station", "Station");
                sqlBC.ColumnMappings.Add("Length", "Length");
                sqlBC.ColumnMappings.Add("Width", "Width");
                sqlBC.ColumnMappings.Add("Height", "Height");
                sqlBC.ColumnMappings.Add("Weight", "Weight");
                sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                sqlBC.ColumnMappings.Add("UpdateTime", "UpdateTime");
                sqlBC.WriteToServer(dt1);
            }
            conn1.Dispose();
            alllist.RemoveAll(a => a.state == 1);

        }

        //插入数据库库存
        private void InsertContainer(string sheetName, List<LoadData> list, List<LoadData> listall)
        {
            try
            {
                //整托
                if (sheetName == "Pallet")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Warecell", typeof(string));
                    dt.Columns.Add("PalletNo", typeof(string));
                    dt.Columns.Add("BoxNo", typeof(string));
                    dt.Columns.Add("BDNo", typeof(string));
                    dt.Columns.Add("PNo", typeof(string));
                    dt.Columns.Add("Number", typeof(int));
                    dt.Columns.Add("IsOverWeight", typeof(int));
                    dt.Columns.Add("IsHengWen", typeof(int));
                    dt.Columns.Add("IsQualified", typeof(int));
                    dt.Columns.Add("State", typeof(int));
                    dt.Columns.Add("Station", typeof(string));
                    dt.Columns.Add("Length", typeof(float));
                    dt.Columns.Add("Width", typeof(float));
                    dt.Columns.Add("Height", typeof(float));
                    dt.Columns.Add("Weight", typeof(float));
                    dt.Columns.Add("CreateTime", typeof(string));
                    dt.Columns.Add("UpdateTime", typeof(string));
                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Warecell", typeof(string));
                    dt2.Columns.Add("PalletNo", typeof(string));
                    for (int i = 0; i < list.Count; i++)
                    {
                        SqlParameter[] paras = new SqlParameter[7];
                        paras[0] = new SqlParameter("@PalletNo", SqlDbType.NVarChar, 50);
                        paras[1] = new SqlParameter("@IsNormalT", SqlDbType.Bit);
                        paras[2] = new SqlParameter("@Weight", SqlDbType.Int);
                        paras[3] = new SqlParameter("@Height", SqlDbType.Int);
                        paras[4] = new SqlParameter("@TaskType", SqlDbType.NVarChar);
                        paras[5] = new SqlParameter("@WareCell", SqlDbType.NVarChar, 50);
                        paras[6] = new SqlParameter("@RoadWay", SqlDbType.Int);
                        paras[0].Value = list[i].palletOrBoxNo;
                        paras[1].Value = list[i].isHengWen;
                        paras[2].Value = list[i].weight;
                        paras[3].Value = list[i].height;
                        paras[4].Value = "taskPallet_Entrance_LK";
                        paras[5].Direction = ParameterDirection.Output;
                        paras[6].Direction = ParameterDirection.Output;
                        string warecell = "";
                        warecell = comnsql.ProcOutput(Properties.Settings.Default.sql1, "P_GetWareCell", paras, "@WareCell").ToString();
                        string update = string.Format(@"UPDATE XinNingWms.dbo.T_Base_WareCell SET wcState = 'wcState_Store' WHERE wcName ='{0}'", warecell);
                        comnsql.DataOperator(Properties.Settings.Default.sql1, update);

                        list[i].hubno = warecell;

                        DataRow dr2 = dt2.NewRow();
                        dr2["Warecell"] = list[i].hubno;
                        dr2["PalletNo"] = list[i].palletOrBoxNo;
                        dt2.Rows.Add(dr2);
                        list[i].state = 1;
                    }
                    for (int j = 0; j < listall.Count; j++)
                    {
                        LoadData ld = list.Find(a => a.palletOrBoxNo == listall[j].palletOrBoxNo);
                        listall[j].hubno = ld.hubno;
                        DataRow dr = dt.NewRow();
                        dr["Warecell"] = listall[j].hubno;
                        dr["PalletNo"] = listall[j].palletOrBoxNo;
                        dr["BoxNo"] = "";
                        dr["BDNo"] = listall[j].bdNo;
                        dr["PNo"] = listall[j].pNo;
                        dr["Number"] = listall[j].number;
                        dr["IsOverWeight"] = Convert.ToInt16(listall[j].isOverWeight);
                        dr["IsHengWen"] = Convert.ToInt16(listall[j].isHengWen);
                        dr["IsQualified"] = 1;
                        dr["State"] = 1;
                        dr["Station"] = "";
                        dr["Length"] = 0;
                        dr["Width"] = 0;
                        dr["Height"] = 0;
                        dr["Weight"] = 0;
                        dr["CreateTime"] = DateTime.Now.ToString();
                        dr["UpdateTime"] = null;
                        dt.Rows.Add(dr);
                        listall[j].state = 1;                      
                    }
                    SqlConnection conn = new SqlConnection(Properties.Settings.Default.sql);
                    conn.Open();
                    using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn))
                    {
                        sqlBC.BatchSize = 100000;
                        sqlBC.BulkCopyTimeout = 60;
                        sqlBC.DestinationTableName = "dbo.T_Biz_Container_PalletDtl";
                        sqlBC.ColumnMappings.Add("Warecell", "Warecell");
                        sqlBC.ColumnMappings.Add("PalletNo", "PalletNo");
                        sqlBC.ColumnMappings.Add("BoxNo", "BoxNo");
                        sqlBC.ColumnMappings.Add("BDNo", "BDNo");
                        sqlBC.ColumnMappings.Add("PNo", "PNo");
                        sqlBC.ColumnMappings.Add("Number", "Number");
                        sqlBC.ColumnMappings.Add("IsOverWeight", "IsOverWeight");
                        sqlBC.ColumnMappings.Add("IsHengWen", "IsHengWen");
                        sqlBC.ColumnMappings.Add("IsQualified", "IsQualified");
                        sqlBC.ColumnMappings.Add("State", "State");
                        sqlBC.ColumnMappings.Add("Station", "Station");
                        sqlBC.ColumnMappings.Add("Length", "Length");
                        sqlBC.ColumnMappings.Add("Width", "Width");
                        sqlBC.ColumnMappings.Add("Height", "Height");
                        sqlBC.ColumnMappings.Add("Weight", "Weight");
                        sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                        sqlBC.ColumnMappings.Add("UpdateTime", "UpdateTime");
                        sqlBC.WriteToServer(dt);
                    }
                    conn.Dispose();

                    SqlParameter[] param = { new SqlParameter("@tablename", dt2) };
                    int l = comnsql.ProcOutput(Properties.Settings.Default.sql, "p_wms_update_PalletContainer", param);
                    list.RemoveAll(c => c.state == 1);
                    listall.RemoveAll(c => c.state == 1);
                }
                //散件
                else if (sheetName == "Bulks")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ShelfNo", typeof(string));
                    dt.Columns.Add("HubNo", typeof(string));
                    dt.Columns.Add("BoxNo", typeof(string));
                    dt.Columns.Add("BDNo", typeof(string));
                    dt.Columns.Add("PNo", typeof(string));
                    dt.Columns.Add("Number", typeof(int));
                    dt.Columns.Add("Length", typeof(float));
                    dt.Columns.Add("Width", typeof(float));
                    dt.Columns.Add("Height", typeof(float));
                    dt.Columns.Add("Weight", typeof(float));
                    dt.Columns.Add("IsQualified", typeof(int));
                    dt.Columns.Add("CreateTime", typeof(string));
                    dt.Columns.Add("cType", typeof(string));
                    dt.Columns.Add("cState", typeof(string));
                    dt.Columns.Add("flag", typeof(int));

                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("shelfNoDtl", typeof(string));
                    dt2.Columns.Add("shelfNo", typeof(string));
                    dt2.Columns.Add("cNo", typeof(string));
                    List<boxhubno> bhlist = new List<boxhubno>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        SqlParameter[] paras = new SqlParameter[2];
                        paras[0] = new SqlParameter("@Station", SqlDbType.NVarChar, 50);
                        paras[1] = new SqlParameter("@ShelfNo", SqlDbType.NVarChar, 50);
                        paras[0].Value = "wstest";
                        paras[1].Direction = ParameterDirection.Output;
                        string shelfno = "";
                        //shelfno = comnsql.ProcOutput(Properties.Settings.Default.sql1, "P_GetShelfOut_InArea", paras, "@ShelfNo").ToString();
                        while (string.IsNullOrEmpty(shelfno)||list.FindAll(a => a.shelf == shelfno).Count >= 60)
                        {
                            shelfno = comnsql.ProcOutput(Properties.Settings.Default.sql1, "P_GetShelfOut_InArea", paras, "@ShelfNo").ToString();
                        }
                        string[] dtl = { "-01", "-02" };
                        Random r = new Random();
                        int n = r.Next(0, dtl.Length);
                        string shelfdtlno = shelfno + dtl[n];
                        list[i].hubno = shelfdtlno;
                        list[i].shelf = shelfno;
                        if (bhlist.Count == 0 || bhlist.Count > 0 && bhlist.Find(a => a.hubno == list[i].hubno) == null)
                        {
                            boxhubno bh = new boxhubno();
                            bh.boxNo = list[i].palletOrBoxNo;
                            bh.hubno = list[i].hubno;
                            bh.shelf = list[i].shelf;
                            bhlist.Add(bh);
                        }
                        else
                        {
                            bhlist.Find(a => a.hubno == list[i].hubno).boxNo += "," + list[i].palletOrBoxNo;
                        }
                        list[i].state = 1;
                    }
                    for (int z = 0; z < bhlist.Count; z++)
                    {
                        DataRow dr2 = dt2.NewRow();
                        dr2["shelfNoDtl"] = bhlist[z].hubno;
                        dr2["shelfNo"] = bhlist[z].shelf;
                        dr2["cNo"] = bhlist[z].boxNo;
                        dt2.Rows.Add(dr2);
                    }
                    for (int j = 0; j < listall.Count; j++)
                    {
                        LoadData ld = list.Find(a => a.palletOrBoxNo == listall[j].palletOrBoxNo);
                        listall[j].hubno = ld.hubno;
                        listall[j].shelf = ld.shelf;
                        DataRow dr = dt.NewRow();
                        dr["ShelfNo"] = listall[j].shelf;
                        dr["HubNo"] = listall[j].hubno;
                        dr["BoxNo"] = listall[j].palletOrBoxNo;
                        dr["BDNo"] = listall[j].bdNo;
                        dr["PNo"] = listall[j].pNo;
                        dr["Number"] = listall[j].number;
                        dr["Length"] = 0;
                        dr["Width"] = 0;
                        dr["Height"] = 0;
                        dr["Weight"] = listall[j].weight;
                        dr["IsQualified"] = 1;
                        dr["CreateTime"] = DateTime.Now.ToString();
                        dr["cType"] = "Container_Box";
                        dr["cState"] = "Container_Store";
                        dr["flag"] = 0;
                        dt.Rows.Add(dr);
                        listall[j].state = 1;
                    }

                    SqlConnection conn = new SqlConnection(Properties.Settings.Default.sql);
                    conn.Open();
                    using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn))
                    {
                        sqlBC.BatchSize = 100000;
                        sqlBC.BulkCopyTimeout = 60;
                        sqlBC.DestinationTableName = "dbo.T_Biz_Container_BulksDtl";
                        sqlBC.ColumnMappings.Add("ShelfNo", "ShelfNo");
                        sqlBC.ColumnMappings.Add("HubNo", "HubNo");
                        sqlBC.ColumnMappings.Add("BoxNo", "BoxNo");
                        sqlBC.ColumnMappings.Add("BDNo", "BDNo");
                        sqlBC.ColumnMappings.Add("PNo", "PNo");
                        sqlBC.ColumnMappings.Add("Number", "Number");
                        sqlBC.ColumnMappings.Add("Length", "Length");
                        sqlBC.ColumnMappings.Add("Width", "Width");
                        sqlBC.ColumnMappings.Add("Height", "Height");
                        sqlBC.ColumnMappings.Add("Weight", "Weight");
                        sqlBC.ColumnMappings.Add("IsQualified", "IsQualified");
                        sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                        sqlBC.ColumnMappings.Add("flag", "flag");
                        sqlBC.WriteToServer(dt);
                    }
                    conn.Dispose();

                    SqlConnection conn1 = new SqlConnection(Properties.Settings.Default.sql1);
                    conn1.Open();
                    using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn1))
                    {
                        sqlBC.BatchSize = 100000;
                        sqlBC.BulkCopyTimeout = 60;
                        sqlBC.DestinationTableName = "dbo.T_Biz_Container";
                        sqlBC.ColumnMappings.Add("HubNo", "cWareCell");
                        sqlBC.ColumnMappings.Add("BoxNo", "cBoxNo");
                        sqlBC.ColumnMappings.Add("Weight", "cWeight");
                        sqlBC.ColumnMappings.Add("IsQualified", "IsQualified");
                        sqlBC.ColumnMappings.Add("CreateTime", "cCreateTime");
                        sqlBC.ColumnMappings.Add("cType", "cType");
                        sqlBC.ColumnMappings.Add("cState", "cState");
                        sqlBC.WriteToServer(dt);
                    }
                    conn1.Dispose();

                    SqlParameter[] param = { new SqlParameter("@tablename", dt2) };
                    int l1 = comnsql.ProcOutput(Properties.Settings.Default.sql, "p_wms_update_BulkContainer", param);
                    //DELETE FROM XinNingWcs.dbo.I_Wcs_Acs_Interface WHERE EndStation='wstest'
                    list.RemoveAll(c => c.state == 1);
                    listall.RemoveAll(c => c.state == 1);
                }
                else if (sheetName == "Bulktask")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ShelfNo", typeof(string));
                    dt.Columns.Add("HubNo", typeof(string));
                    dt.Columns.Add("BoxNo", typeof(string));
                    dt.Columns.Add("BDNo", typeof(string));
                    dt.Columns.Add("PNo", typeof(string));
                    dt.Columns.Add("Number", typeof(int));
                    dt.Columns.Add("Length", typeof(float));
                    dt.Columns.Add("Width", typeof(float));
                    dt.Columns.Add("Height", typeof(float));
                    dt.Columns.Add("Weight", typeof(float));
                    dt.Columns.Add("IsQualified", typeof(int));
                    dt.Columns.Add("CreateTime", typeof(string));
                    dt.Columns.Add("flag", typeof(int));
                    for (int j = 0; j < listall.Count; j++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ShelfNo"] = "";
                        dr["HubNo"] = "";
                        dr["BoxNo"] = listall[j].palletOrBoxNo;
                        dr["BDNo"] = listall[j].bdNo;
                        dr["PNo"] = listall[j].pNo;
                        dr["Number"] = listall[j].number;
                        dr["Length"] = 0;
                        dr["Width"] = 0;
                        dr["Height"] = 0;
                        dr["Weight"] = listall[j].weight;
                        dr["IsQualified"] = 1;
                        dr["CreateTime"] = DateTime.Now.ToString();
                        dr["flag"] = 2;
                        dt.Rows.Add(dr);
                        listall[j].state = 1;
                    }
                    SqlConnection conn = new SqlConnection(Properties.Settings.Default.sql);
                    conn.Open();
                    using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(conn))
                    {
                        sqlBC.BatchSize = 100000;
                        sqlBC.BulkCopyTimeout = 60;
                        sqlBC.DestinationTableName = "dbo.T_Biz_Container_BulksDtl";
                        sqlBC.ColumnMappings.Add("ShelfNo", "ShelfNo");
                        sqlBC.ColumnMappings.Add("HubNo", "HubNo");
                        sqlBC.ColumnMappings.Add("BoxNo", "BoxNo");
                        sqlBC.ColumnMappings.Add("BDNo", "BDNo");
                        sqlBC.ColumnMappings.Add("PNo", "PNo");
                        sqlBC.ColumnMappings.Add("Number", "Number");
                        sqlBC.ColumnMappings.Add("Length", "Length");
                        sqlBC.ColumnMappings.Add("Width", "Width");
                        sqlBC.ColumnMappings.Add("Height", "Height");
                        sqlBC.ColumnMappings.Add("Weight", "Weight");
                        sqlBC.ColumnMappings.Add("IsQualified", "IsQualified");
                        sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                        sqlBC.ColumnMappings.Add("flag", "flag");
                        sqlBC.WriteToServer(dt);
                    }
                    conn.Dispose();
                    listall.RemoveAll(a => a.state == 1);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }




        }

        //到指定时间下发上架任务到wcs
        private void SendLoadTask(LoadData task)
        {
            int loadSuccessNumber = 0;
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""SN"":""{1}"",
""planningNo"":""{2}"",
""isHengwen"":{3},
""isBLOCK"":1,
""taskType"":{4},
""BDNO"":""{6}"",
""pn"":""{7}"",
""weight"":{8},
""height"":{9},
""SN_OLD"":""{10}"",
""opTime"":""{5}""}}]", task.taskNo, task.palletOrBoxNo, "", Convert.ToInt16(task.isHengWen), -1, DateTime.Now.ToString(), task.bdNo, task.pNo, task.weight, task.height,task.palletOrBoxNo_Old);

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url + "/loadTasks";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/json";
            request.Headers.Add("systemCode:XNWMS");
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(jsarray.ToString());
            request.ContentLength = postdatabtyes.Length;
            try
            {
                Stream requeststream = request.GetRequestStream();
                requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
                requeststream.Close();
                string resp;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    resp = sr.ReadToEnd();
                }


                if (resp.Contains("true"))
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                    loadSuccessNumber++;
                    string updateString = "";
                    updateString += string.Format(@"UPDATE dbo.T_Task_Load set state=1,SendTime =getdate() where TaskNo='{0}'", task.taskNo);
                    updateString += string.Format(@"DELETE FROM dbo.T_Task_Load WHERE TaskNo ='{0}'", task.taskNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                    task.state = 1;
                    sendLoadTasksList.Remove(sendLoadTasksList.Find(a => a.taskNo == task.taskNo));
                }
                else
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
            //MessageBox.Show("下发" + loadSuccessNumber + "条上架任务成功");
        }
        
        //到指定时间填充下架任务
        private void FillUnLoadTask(UnLoadData task)
        {
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            bool isSplit = false;
            //if (string.IsNullOrEmpty(task.hubno) && string.IsNullOrEmpty(task.palletOrBoxNo))
            if (string.IsNullOrEmpty(task.palletOrBoxNo))
            {
                SqlParameter[] paras = new SqlParameter[7];
                paras[0] = new SqlParameter("@BDNo", SqlDbType.NVarChar, 50);
                paras[1] = new SqlParameter("@PNo", SqlDbType.NVarChar, 50);
                paras[2] = new SqlParameter("@Sign", SqlDbType.NVarChar, 50);
                paras[3] = new SqlParameter("@Number", SqlDbType.Int);
                paras[4] = new SqlParameter("@IsSplit", SqlDbType.Int);
                paras[5] = new SqlParameter("@ShelfNo", SqlDbType.NVarChar, 500);
                paras[6] = new SqlParameter("@PalletOrBoxNo", SqlDbType.NVarChar, 500);
                paras[0].Value = task.bdNo;
                paras[1].Value = task.pNo;
                paras[2].Value = task.sign;
                paras[3].Value = task.outNumber;
                paras[4].Direction = System.Data.ParameterDirection.Output;
                paras[5].Direction = System.Data.ParameterDirection.Output;
                paras[6].Direction = System.Data.ParameterDirection.Output;

                List<string> result = comnsql.ProcOutput(Properties.Settings.Default.sql, "[p_wms_insert_unloadtask]", paras, "@IsSplit", "@ShelfNo", "@PalletOrBoxNo");
                //task.isUnpackTray = Convert.ToBoolean(result[0]);
                if (result[0] == "1")
                {
                    isSplit = true;
                }
                if (isSplit)
                {
                    InitUnLoadTask(0, t);
                    task.state = 1;
                }
                List<UnLoadData> tmpult = sendUnloadTasksList.FindAll(a => a.sign == task.sign);
                if (tmpult != null)
                {
                    foreach (UnLoadData ult in tmpult)
                    {
                        SendUnLoadTask(ult);
                    }
                }
            }
            else
            {
                SendUnLoadTask(task);
            }
            
        }

        //到指定时间将填充的下架任务发送给wcs
        private void SendUnLoadTask(UnLoadData task)
        {
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""taskType"":""{1}"",
""taskState"":""{2}"",
""priority"":{3},
""OUTNO"":""{4}"",
""planningNo"":"""",
""SN"":""{5}"",
""hubno"":""{6}"",
""isUnpackTray"":{10},
""isBoxLable"":{11},
""isMinpackLable"":{12},
""isPipeline"":{7},
""SN_OLD"":""{8}"",
""station"":"""",
""opTime"":""{9}""}}]", task.taskNo, task.taskType, task.taskState, task.priority, task.outNo, task.palletOrBoxNo, task.hubno, task.isPipeline, task.SN_OLD, DateTime.Now.ToString(), task.isUnpackTray, 0, -1);

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url + "/unloadTasks";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/json";
            request.Headers.Add("systemCode:XNWMS");
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(jsarray.ToString());
            request.ContentLength = postdatabtyes.Length;
            try
            {
                Stream requeststream = request.GetRequestStream();
                requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
                requeststream.Close();
                string resp;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    resp = sr.ReadToEnd();
                }


                if (resp.Contains("true"))
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n"; 
                    string updateString = "";
                    updateString += string.Format(@"UPDATE dbo.T_Task_UnLoad set state=1,SendTime =getdate() where TaskNo='{0}'",  task.taskNo);
                    updateString += string.Format(@"DELETE FROM dbo.T_Task_UnLoad WHERE TaskNo ='{0}'", task.taskNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                    task.state = 1;
                    sendUnloadTasksList.Remove(sendUnloadTasksList.Find(a => a.taskNo == task.taskNo));
                     
                }
                else
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                    string updatefailcontainer = string.Format("update dbo.T_Biz_Container_PalletDtl set State=1  ,UpdateTime=getdate() where PalletNo='{0}' and State=3", task.palletOrBoxNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updatefailcontainer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
        }

        //到指定时间下发暂存区出口任务到wcs
        private void SendDeliveryTask(DeliveryTasks task)
        {
            //string jsarray = JsonConvert.SerializeObject(task);
            string SN = "[";
            if (task.SN.Count == 1)
            {
                SN += "\"" + task.SN[0] + "\"";
            }
            else
            {
                for (int i = 0; i < task.SN.Count; i++)
                {
                    if (i == 0)
                    {
                        SN += "\"" + task.SN[i] + "\"";
                    }
                    else
                    {
                        SN += "," + "\"" + task.SN[i] + "\"";
                    }
                }
            }
            SN += "]";
            int loadSuccessNumber = 0;
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""taskType"":""{1}"",
""SN"":{2},
""opTime"":""{3}""}}]", task.taskNo, task.taskType, SN, DateTime.Now.ToString());

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url + "/deliveryTasks";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/json";
            request.Headers.Add("systemCode:XNWMS");
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(jsarray.ToString());
            request.ContentLength = postdatabtyes.Length;
            try
            {
                Stream requeststream = request.GetRequestStream();
                requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
                requeststream.Close();
                string resp;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    resp = sr.ReadToEnd();
                }


                if (resp.Contains("true"))
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                    loadSuccessNumber++;
                    string updateString = "";
                    updateString += string.Format(@"UPDATE dbo.T_Task_UnLoad set state=1,SendTime =getdate() where TaskNo='{0}'", task.taskNo);
                    updateString += string.Format(@"DELETE FROM dbo.T_Task_UnLoad WHERE TaskNo ='{0}'", task.taskNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                    task.state = 1;
                    sendDeliveryTasksList.Remove(sendDeliveryTasksList.Find(a => a.taskNo == task.taskNo &&a.state==1));
                }
                else
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    comnlog.MessageLog("任务下发", DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n");
                    //richTextBox1.Text += DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
            //MessageBox.Show("下发" + loadSuccessNumber + "条上架任务成功");


        }

        //下载已过发送时间的遗留上架任务并发送给wcs
        private void DownRemainTask()
        {
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            int loadTaskNumber = 0, count = 0;
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                count = 1;
            }
            else
            {
                count = Convert.ToInt16(textBox6.Text.Trim());
            }
            string selectString = string.Format(@"select top {1} * from dbo.T_Task_Load where state=0 AND Times<{0} order by NEWID()", t,count);
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, selectString);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LoadData pclt = new LoadData();
                    pclt.times = Convert.ToInt32(dr["Times"].ToString());
                    pclt.state = Convert.ToInt32(dr["State"].ToString());
                    pclt.taskNo = dr["TaskNo"].ToString();
                    pclt.palletOrBoxNo = dr["SN"].ToString();
                    pclt.isHengWen = Convert.ToBoolean(dr["IsHengwen"]);
                    pclt.bdNo = dr["BDNO"].ToString();
                    pclt.pNo = dr["Pn"].ToString();
                    pclt.weight = Convert.ToInt32(dr["Weight"]);
                    pclt.height = Convert.ToInt32(dr["Height"]);
                    pclt.palletOrBoxNo_Old = dr["SN_OLD"].ToString();
                    if (remainLoadTasksList.Find(a => a.taskNo == pclt.taskNo) == null)
                    {
                        remainLoadTasksList.Add(pclt);
                        loadTaskNumber++;
                    }
                    else
                    {
                        continue;
                    }
                }
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 下载" + loadTaskNumber + "条遗留上架任务成功" + "\n");
                comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 下载" + loadTaskNumber + "条遗留上架任务成功" + "\n");
                LogWrite.WriteLogToMain1("======等待下发上架任务======\n");               
            }
            if (remainLoadTasksList.FindAll(a => a.state == 0) != null)
            {
                foreach (LoadData loadt in remainLoadTasksList)
                {
                    SendLoadTask(loadt);
                }
                remainLoadTasksList.RemoveAll(a => a.state == 1);
            }
            else
            {
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 无遗留上架任务可下发" + "\n");
            }          
        }

        //下载已过发送时间的遗留下架任务并发送给wcs
        private void DownRemainTask1()
        {
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;           
            int unloadTaskNumber = 0,count = 0; 
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                count = 1;
            }
            else
            {
                count = Convert.ToInt16(textBox5.Text.Trim());
            }
            InitUnLoadTask(0, t);
            if (sendUnloadTasksList.FindAll(a => a.state == 0 &&a.times<=t).Count >= count)
            {
                for (int i = 0; i < count; i++)
                {
                    SendUnLoadTask(sendUnloadTasksList.FindAll(a => a.state == 0 && a.times <= t)[i]);
                }
            }
            else
            {
                string selectString1 = string.Format(@"select top {1} o.*,m.miniPerBag from dbo.T_Task_UnLoad_Origin o
LEFT JOIN T_Biz_PNo_MiniNumber m ON m.pNo=o.PNo WHERE state=0 AND Times<{0} and  isnull(Sign,'')<>'' order by Times ", t, (count - sendUnloadTasksList.FindAll(a => a.state == 0 && a.times <= t).Count));
                DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, selectString1);
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        UnLoadData unloadTask = new UnLoadData();
                        unloadTask.state = 0;
                        unloadTask.times = Convert.ToInt32(dr["Times"]);
                        unloadTask.outNo = dr["OutNo"].ToString();
                        unloadTask.bdNo = dr["BDNo"].ToString();
                        unloadTask.pNo = dr["PNo"].ToString();
                        unloadTask.outNumber = Convert.ToInt32(dr["Number"].ToString());
                        unloadTask.miniPerBag = Convert.ToInt32(dr["miniPerBag"].ToString());
                        unloadTask.businessType = dr["BusinessType"].ToString();
                        unloadTask.sign = dr["Sign"].ToString();
                        if (remainUnloadTasksList.Find(a => a.sign == unloadTask.sign) == null)
                        {
                            remainUnloadTasksList.Add(unloadTask);
                            unloadTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (unloadTaskNumber != 0)
                    {
                        LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 新下载" + unloadTaskNumber + "条遗留未填充下架任务成功" + "\n");
                        comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 新下载" + unloadTaskNumber + "条遗留未填充下架任务成功" + "\n");
                        LogWrite.WriteLogToMain1("======等待下发下架任务======\n");
                    }
                }
                if (remainUnloadTasksList.FindAll(a => a.state == 0).Count != 0)
                {
                    foreach (UnLoadData ut in remainUnloadTasksList)
                    {
                        FillUnLoadTask(ut);
                    }
                    remainUnloadTasksList.RemoveAll(a => a.state == 1);
                }
                else
                {
                    LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 无遗留未填充下架任务可下发" + "\n");
                }
            }
        }

        //下载已过发送时间的遗留delivery任务并发送给wcs
        private void DownRemainTask2()
        {
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            InitDeliveryTask(0, t);
            if (sendDeliveryTasksList.FindAll(a => a.Times < t).Count != 0)
            {
                foreach (DeliveryTasks dt in sendDeliveryTasksList.FindAll(a => a.Times < t))
                {
                    SendDeliveryTask(dt);
                }
            }
        }

        //按钮启动监听
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "开始监听")
            {
                startListen();
                button1.Text = "正在监听";
            }
            else if (button1.Text == "正在监听")
            {
                DialogResult dia=MessageBox.Show("已启动监听，是否要停止","提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (dia == DialogResult.Yes)
                {
                    host.Close();
                    button1.Text = "开始监听";
                }
            }
            
        }

        //启动监听
        private void startListen()
        {
            host = new WebServiceHost(typeof(SNInfoWcf), new Uri(textBox3.Text));
            host.Open();
            //MessageBox.Show(textBox3.Text + "已经开始监听");
            button1.Enabled = false;
            button5.Enabled = true;
        }

        //停止监听
        private void button5_Click(object sender, EventArgs e)
        {
            host.Close();
            button1.Text = "开始监听";
            button1.Enabled = true;
            button5.Enabled = false;
        }

        //后台执行处理Excel，防止窗口假死
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !button2.Enabled)
            {
                //label3.Text = "处理Excel数据中";
                GetSheetData();
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button2.Enabled = true;
            textBox1.Text = "";
            //label3.Text = "完成数据处理";
        }
       
        #region
        //将内容定位到插入的位置（即最新的位置）
        private void richTextBoxSetting()
        {
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.Text.Length, 0);
            richTextBox1.ScrollToCaret();
        }

        //定时器，定时下发整托下架任务到数据库
        private void TimerUp3(object value)
        {
            UnLoadTasks();
        }

        //选择库存中的数据并创建下架任务
        private void UnLoadTasks()
        {
            string insertString = "";
            int taskCount=0;
            if (string.IsNullOrEmpty(textBox5.Text.Trim()))
            {
                taskCount = 1;
            }
            else
            {
                taskCount = Convert.ToInt16(textBox5.Text.Trim());
            }
            List<UnLoadData> unLoadTasks = new List<UnLoadData>();//下架任务
            int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            //string sql = string.Format(@"SELECT TOP 1 TaskNo FROM {0} WHERE TaskNo LIKE '%{1}%' ORDER BY TaskNo DESC", "dbo.T_Task_UnLoad", date);
            //string taskno = "";
            //if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
            //{
            //    taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
            //}
           
            string selectString = "";
            if (radioButton1.Checked)
            {
                selectString = string.Format(@"SELECT TOP {0} ID, Warecell, PalletNo, BDNo, PNo, Number,IsOverWeight, IsHengWen
FROM dbo.T_Biz_Container_PalletDtl where State=1 ORDER BY newid()", taskCount);
            }
            else if (radioButton2.Checked)
            {
                selectString = string.Format(@"SELECT TOP {0} ID,ShelfNo as Warecell , BoxNo as PalletNo, BDNo, PNo
FROM dbo.T_Biz_Container_BulksDtl where flag=0 ORDER BY newid()", taskCount);
            }
            else
            {
                MessageBox.Show("请选择下发【整托】还是【散件】任务");
            }
            
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, selectString);
            bool[] isUnpack = { false, false };
            bool[] isPipe = { true, false };
            Random r = new Random();
            if (ds.Tables[0].Rows.Count != 0)
            {
                int c = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] paras = new SqlParameter[1];
                    paras[0] = new SqlParameter("@NEW_ID", SqlDbType.NVarChar, 50);
                    paras[0].Direction = ParameterDirection.Output;
                    string TaskNo = comnsql.ProcOutput(Properties.Settings.Default.sql, "P_GetNewUnLoadTaskNo", paras, "@NEW_ID").ToString();

                    UnLoadData pclt = new UnLoadData();
                    pclt.id = Convert.ToInt32(dr["ID"]);
                    pclt.times = t + 3;
                    pclt.state = 0;
                    pclt.taskType = "OUT";
                    pclt.taskState = "NORMAL";
                    pclt.priority = 1;
                    pclt.outNo = r.Next(0, 101).ToString();
                    pclt.palletOrBoxNo = dr["PalletNo"].ToString();
                    pclt.hubno = dr["Warecell"].ToString();  
                    int n1 = r.Next(0, isUnpack.Length);
                    int n2 = r.Next(0, isPipe.Length);
                    pclt.isUnpackTray = isUnpack[n1];
                    pclt.isPipeline = isPipe[n2];
                    pclt.taskNo = TaskNo;
                    pclt.bdNo = dr["BDNo"].ToString();
                    pclt.pNo = dr["PNo"].ToString();
                    if (unLoadTasks.Find(a => a.taskNo == pclt.taskNo) == null)
                    {
                        unLoadTasks.Add(pclt);
                        c++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (unLoadTasks.Count == 0)
            {
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 库存中无数据或库存中数据均已下发下架任务\n");
                comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 库存中无数据或库存中数据均已下发下架任务\n");
            }
            else
            {
                foreach (UnLoadData task in unLoadTasks)
                {
                    insertString += string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Number, BusinessType, BDNo, PNo, Station, CreateTime) 
VALUES ({0}, 0, '{1}', '{2}', '{3}', {4}, '{5}', '', '{6}', '{7}', {8}, 0, -1, {11}, '', 1, '', '{9}', '{10}', '', getdate())"
                        , task.times, task.taskNo, task.taskType, task.taskState, task.priority, task.outNo, task.palletOrBoxNo, task.hubno, Convert.ToInt16(task.isUnpackTray), task.bdNo, task.pNo, Convert.ToInt16(task.isPipeline));
                    string updateString = "";
                    if (radioButton1.Checked)
                    {
                        updateString = string.Format(@"UPDATE dbo.T_Biz_Container_PalletDtl SET State = 3
WHERE PalletNo = '{0}' AND Warecell='{1}' AND ID={2} AND State = 1", task.palletOrBoxNo, task.hubno, task.id);
                    }
                    else if (radioButton2.Checked)
                    {
                        updateString = string.Format(@"UPDATE T_Biz_Container_BulksDtl SET State = 3
WHERE BoxNo = '{0}' AND ShelfNo='{1}' AND ID={2} AND flag=1", task.palletOrBoxNo, task.hubno, task.id);
                    }
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                }
                int successNumber = comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                LogWrite.WriteLogToMain1(DateTime.Now.ToString() + " 插入" + successNumber + "条下架任务完成" + "\n");
                comnlog.MessageLog("任务处理", DateTime.Now.ToString() + " 插入" + successNumber + "条下架任务完成" + "\n");
                InitUnLoadTask(t-20,t+20);
            }          
        }

        //定时器自动下遗留任务
        private void TimerUp4(object state)
        {
            DownRemainTask();
            DownRemainTask1();
        }

        //手动下下架任务
        private void button3_Click(object sender, EventArgs e)
        {
            DownRemainTask1();
            //UnLoadTasks();
            textBox5.Text = "";
        }

        //手动下上架任务
        private void button4_Click(object sender, EventArgs e)
        {
            DownRemainTask();
            textBox6.Text = "";
        }

        //清除控件中的内容
        private void 清除内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ctrl.Text = "";
            ctrl = null;
        }

        //获取点击的控件
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ctrl = ((System.Windows.Forms.ContextMenuStrip)sender).SourceControl;
        }
        #endregion       

        private void button6_Click(object sender, EventArgs e)
        {
            DownRemainTask2();
        }
    }
}
