﻿using com.force.json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace learn_180522_webservice
{
    public partial class Form1 : Form
    {

        DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
        DAL_Comn_Excel ce = new DAL_Comn_Excel();
        List<LoadData> TmpLTs = new List<LoadData>();//tmp上架任务
        List<LoadData> TmpULTs = new List<LoadData>();//tmp下架任务
        List<LoadData> TmpPCLs = new List<LoadData>();//整托库存
        List<LoadData> TmpBLCs = new List<LoadData>();//散件库存
        List<LoadData> sendLoadTasksList = new List<LoadData>();//上架任务
        List<UnLoadData> sendUnloadTasksList = new List<UnLoadData>();//下架任务
        List<PNPB> pnpbsList = new List<PNPB>();//料号最小单包对照
        private Thread ThWhile;

        string fileName="";
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LogWrite.WriteResultMain += new LogWrite.ResultLogDelegate(WriteResult);
            LogWrite.WriteMain += new LogWrite.WriteLogDelegate(WriteLog);
            textBox3.Text = Properties.Settings.Default.url1;
            timer1.Enabled = true;  //启动计时器
            timer1.Interval = 500; //设置计时器时间间隔，单位为ms
            timer2.Enabled = true;  //启动计时器
            timer2.Interval = 500; //设置计时器时间间隔，单位为ms
            progressBar1.Visible = false;
            textBox2.Visible = false;
            InitTask();


        }

        private void WriteLog(string log)
        {
            richTextBox2.Text += log;
        }

        private void WriteResult(string log)
        {
            textBox4.Text = log;
        }

        //初始化下载未发送的任务（当天任务）
        private void InitTask()
        {
            try
            {
                int loadTaskNumber=0,unloadTaskNumber=0;
                int t = (DateTime.Now.Day - 1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                string selectString = string.Format(@"select * from dbo.T_Task_Load where state=0 AND Times>={0} AND Times<{1}", t, (DateTime.Now.Day + 1) * 86400);
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
                        if(sendLoadTasksList.Find(a=>a.taskNo==pclt.taskNo)==null)
                        {
                            sendLoadTasksList.Add(pclt);
                            loadTaskNumber++;
                        }
                        else
                        {
                            continue;
                        }                      
                    }
                    richTextBox1.Text += DateTime.Now.ToString() + " 下载当天" + loadTaskNumber + "条上架任务成功" + "\n";
                    richTextBox1.Text += "======等待下发上架任务======\n";
                }
                string selectString1 = string.Format(@"select * from dbo.T_Task_UnLoad where state=0 AND Times>={0} AND Times<{1}", t, (DateTime.Now.Day + 1) * 86400);
                DataSet ds1 = comnsql.SelectGet(Properties.Settings.Default.sql, selectString1);
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        UnLoadData ult = new UnLoadData();
                        ult.times = Convert.ToInt32(dr["Times"].ToString());
                        ult.state = Convert.ToInt32(dr["State"].ToString());
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
                        ult.station = dr["Station"].ToString();
                        if(sendUnloadTasksList.Find(a=>a.taskNo==ult.taskNo)==null)
                        {
                            sendUnloadTasksList.Add(ult);
                            unloadTaskNumber++;
                        } 
                        else
                        {
                            continue;
                        }     
                        
                    }
                    richTextBox1.Text += DateTime.Now.ToString() + " 下载当天" + unloadTaskNumber + "条下架任务成功" + "\n";
                    richTextBox1.Text += "======等待下发下架任务======\n";
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
                richTextBox1.Text += DateTime.Now.ToString() + "开始读取Excel数据\n";
                GetSheetData();
            }
            
        }
        
        //读取Excel数据
        public void GetSheetData()
        {
            Microsoft.Office.Interop.Excel.Workbook wb;
            try
            {               
                //先将所有任务写到数据库（WMS任务）中，发送成功的任务flag变为1，下次只发flag为0的值               
                if (string.IsNullOrEmpty(fileName))
                {
                    MessageBox.Show("请选择文件");
                    return;
                }
                wb = ce.Open(fileName);
                if (fileName.Contains("WarehouseData"))
                {
                    ManageLoadData(wb);
                }
                else if (fileName.Contains("Unloading")) 
                {
                    ManageUnLoadData(wb);
                }             
                wb = null;
                ce.Close();
                button2.Enabled = true;
                textBox2.Text = "完成";
                textBox1.Text = "";
                progressBar1.Visible = false;
                textBox2.Visible = false;
                richTextBox1.Text += DateTime.Now.ToString() + " 读取Excel任务完成" + "\n";
                richTextBox1.Text += "======等待下发任务======\n";

            }
            catch (Exception Ex)
            {
                wb = null;
                ce.Close();
                throw new Exception(Ex.ToString());
            }
            
            
        }

        //处理下架数据
        private void ManageUnLoadData(Microsoft.Office.Interop.Excel.Workbook wb)
        {
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            string sheetName = wb.Worksheets[1].Name;
            Microsoft.Office.Interop.Excel.Worksheet ws = wb.Worksheets[sheetName];
            int finishNumber = 0;
            int totalNumber = ws.UsedRange.CurrentRegion.Rows.Count;
            button2.Enabled = false;
            finishNumber++;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalNumber - 1;
            progressBar1.Step = 1;
            int day = 1;
            string sql = string.Format(@"SELECT TOP 1 TaskNo FROM {0} WHERE TaskNo LIKE '%{1}%' ORDER BY TaskNo DESC","dbo.T_Task_UnLoad",date);
            string taskno = "";
            if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
            {
                taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
            }

            string number = "";
            for (int i = 1; i <= totalNumber; i++)
            {
                 if (!string.IsNullOrEmpty(taskno))
                {
                    number = date + (int.Parse(taskno.Substring(taskno.Length - 4, 4)) + i + 1).ToString().PadLeft(4, '0');
                }
                else
                {
                    number = date + ((i + 1).ToString().PadLeft(4, '0'));

                }
                string TaskNo = "ULTaskNo" + number;
                if (i == 1)
                {
                    if (ce.GetCellValue(ws, i, 1) == "时间" && ce.GetCellValue(ws, i, 2) == "C仓H/LOT" && ce.GetCellValue(ws, i, 3) == "保税H"
                        && ce.GetCellValue(ws, i, 4) == "料H" && ce.GetCellValue(ws, i, 6) == "C库数量" && ce.GetCellValue(ws, i, 7) == "最小单包"
                        && ce.GetCellValue(ws, i, 8) == "业务类型")
                    {
                        continue;
                    }
                    else
                    {
                        string error = ws.Name  + "中数据不正确";
                        wb = null;
                        ce.Close();
                        throw new Exception(error);
                    }
                }
                else
                {
                    UnLoadData unloadTask = new UnLoadData();
                    unloadTask.state = 0;
                    unloadTask.times = Convert.ToInt32(ce.GetCellValue(ws, i, 1));
                    unloadTask.outNo = ce.GetCellValue(ws, i, 2);
                    unloadTask.bdNo = ce.GetCellValue(ws, i, 3);
                    unloadTask.pNo = ce.GetCellValue(ws, i, 4);
                    unloadTask.outNumber = Convert.ToInt32(ce.GetCellValue(ws, i, 6));
                    unloadTask.miniPerBag = Convert.ToInt32(ce.GetCellValue(ws, i, 7));
                    unloadTask.businessType = ce.GetCellValue(ws, i, 8);
                    unloadTask.taskNo=TaskNo;
                    Random rnd = new Random();
                    unloadTask.isPipeline=Convert.ToBoolean(rnd.Next(2));
                    sendUnloadTasksList.Add(unloadTask);

                    PNPB pnpb = new PNPB();
                    pnpb.state=0;
                    pnpb.pNo = ce.GetCellValue(ws, i, 4);
                    pnpb.miniPerBag = Convert.ToInt32(ce.GetCellValue(ws, i, 7));
                    if (pnpbsList.Count == 0)
                    {
                        pnpbsList.Add(pnpb);
                    }
                    else
                    {
                        if (pnpbsList.Find(a => a.pNo == pnpb.pNo)!=null)
                        {
                            continue;
                        }
                        else
                        {
                            pnpbsList.Add(pnpb);
                        }
                    }
                    string insertString = string.Format(@"INSERT INTO dbo.T_Biz_PNo_MiniNumber (pNo, miniPerBag)VALUES ('{0}', {1})", pnpb.pNo, pnpb.miniPerBag);
                    int pnpbN=comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                    if (pnpbN == 1)
                    {
                        pnpb.state = 1;
                    }
                    string selectString = string.Format(@"SELECT top 1 cShelfDtl, cBoxNo,cBDNo,cPnNo  FROM dbo.T_Biz_Container_Bulks WHERE (CHARINDEX('{0}',cBDNo)>0) AND (CHARINDEX('{1}',cPnNo)>0)", unloadTask.bdNo, unloadTask.pNo);
                    DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, selectString);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        string snall = dr["cBoxNo"].ToString();
                        string bdnoall = dr["cBDNo"].ToString();
                        string[] snTemp = snall.Split(',');
                        string sn = snTemp[Array.IndexOf(bdnoall.Split(','), unloadTask.bdNo)];
                        string shelf = dr["cShelfDtl"].ToString();
                        string insertString1 = string.Format(@"INSERT INTO dbo.T_Task_UnLoad (Times, State, TaskNo, TaskType, TaskState, Priority, OutNo, PlanningNo, SN, HubNo, IsUnpackTray, IsBoxLable, IsMinpackLable, IsPipeline, SN_OLD, Station, CreateTime)
VALUES ({0}, 0, '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', '{8}', 0, 0, -1, {9}, '{10}', '', GETDATE())"
                            , unloadTask.times, unloadTask.taskNo, "OUT", "NORMAL", 1, unloadTask.outNo, "", sn, shelf, Convert.ToInt16(unloadTask.isPipeline), sn);
                        comnsql.DataOperator(Properties.Settings.Default.sql, insertString1);
                        unloadTask.taskType = "OUT";
                        unloadTask.taskState = "NORMAL";
                        unloadTask.priority = 1;
                        unloadTask.palletOrBoxNo = sn;
                        unloadTask.hubno = shelf;
                        unloadTask.SN_OLD = sn;
                        unloadTask.state = 1;

                        finishNumber++;
                        progressBar1.PerformStep();
                        while (unloadTask.times >= day * 86400)
                        {
                            day++;
                        }
                        textBox2.Text = string.Format(@"正在下载第{0}天的下架任务，已完成{1}/{2}", day, finishNumber, totalNumber);
                    }
                    else
                    {
                        richTextBox1.Text += DateTime.Now.ToString() + "出仓号：" + unloadTask.outNo + "保税号：" + unloadTask.bdNo + "料号：" + unloadTask.pNo + "在库存中不存在\n";
                    }
                   
                }
            }
        }

        //处理上架数据
        private void ManageLoadData(Microsoft.Office.Interop.Excel.Workbook wb)
        {
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            for (int n = 1; n <= wb.Worksheets.Count; n++)
            {
                string sheetName = wb.Worksheets[n].Name;
                Microsoft.Office.Interop.Excel.Worksheet ws = wb.Worksheets[sheetName];
                int finishNumber = 0;
                int totalNumber = ws.UsedRange.CurrentRegion.Rows.Count;
                button2.Enabled = false;
                finishNumber++;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = totalNumber - 1;
                progressBar1.Step = 1;
                bool isSuccess = false;
                for (int i = 1; i <= totalNumber; i++)
                {
                    if (i == 1)
                    {
                        if ((((sheetName == "PalletInitialInventoryConstant" || sheetName == "PalletInitiaInventoryNoConstant") && ce.GetCellValue(ws, i, 1).ToUpper() == "Pallet.NO".ToUpper())
                            || (sheetName == "BulksInitial" && ce.GetCellValue(ws, i, 1).ToUpper() == "Carton ID".ToUpper()))
                            && ce.GetCellValue(ws, i, 2).ToUpper() == "bdno".ToUpper() && ce.GetCellValue(ws, i, 4).ToUpper() == "pn".ToUpper() ||
                            (((sheetName == "PalletLoadingNoConstant" || sheetName == "PalletLoadingConstant") && ce.GetCellValue(ws, i, 2).ToUpper() == "Pallet.NO".ToUpper())
                            || sheetName == "BulksLoading" && ce.GetCellValue(ws, i, 2).ToUpper() == "Carton.No".ToUpper())
                            && ce.GetCellValue(ws, i, 1) == "时间" && ce.GetCellValue(ws, i, 3).ToUpper() == "保税编H".ToUpper() && ce.GetCellValue(ws, i, 5).ToUpper() == "料H".ToUpper())
                        {
                            continue;
                        }
                        else
                        {
                            string error = ws.Name + ce.GetCellValue(ws, i, 1) + ce.GetCellValue(ws, i, 2) + ce.GetCellValue(ws, i, 4) + "中数据不正确";
                            wb = null;
                            ce.Close();
                            throw new Exception(error);
                        }
                    }
                    else
                    {
                        LoadData pclt = new LoadData();
                        pclt.state = 0;
                        if (sheetName.Contains("NoConstant"))
                        {
                            pclt.isHengWen = false;
                        }
                        else
                        {
                            pclt.isHengWen = true;
                        }
                        if (sheetName == "PalletInitiaInventoryNoConstant")
                        {
                            pclt.palletOrBoxNo = "PXN" + date.Remove(0, 2) + ce.GetCellValue(ws, i, 1).PadLeft(6, '0');
                        }
                        else if (sheetName == "PalletInitialInventoryConstant")
                        {
                            pclt.palletOrBoxNo = "PXC" + date.Remove(0, 2) + ce.GetCellValue(ws, i, 1).PadLeft(6, '0');
                        }
                        else
                        {
                            pclt.palletOrBoxNo = "PX" + date.Remove(0, 2) + ce.GetCellValue(ws, i, 2).PadLeft(6, '0');
                        }
                        if (sheetName.Contains("Pallet") && sheetName.Contains("Initia"))
                        {

                            if (ce.GetCellValue(ws, i, 8) == "Y")
                            {
                                pclt.isOverWeight = true;
                            }
                            else if (ce.GetCellValue(ws, i, 8) == "N")
                            {
                                pclt.isOverWeight = false;
                            }
                        }
                        else if (sheetName.Contains("Pallet") && !sheetName.Contains("Initia"))
                        {
                            pclt.times = int.Parse(ce.GetCellValue(ws, i, 1));
                            if (ce.GetCellValue(ws, i, 9) == "Y")
                            {
                                pclt.isOverWeight = true;
                            }
                            else if (ce.GetCellValue(ws, i, 9) == "N")
                            {
                                pclt.isOverWeight = false;
                            }
                        }
                        else if (sheetName.Contains("Bulks") && sheetName.Contains("Initia"))
                        {
                            pclt.palletOrBoxNo = "CX" + date.Remove(0, 2) + ce.GetCellValue(ws, i, 1).PadLeft(6, '0');
                        }
                        else if (sheetName.Contains("Bulks") && !sheetName.Contains("Initia"))
                        {
                            pclt.palletOrBoxNo = "CX" + date.Remove(0, 2) + ce.GetCellValue(ws, i, 2).PadLeft(6, '0');
                            pclt.times = int.Parse(ce.GetCellValue(ws, i, 1));
                        }
                        if (sheetName.Contains("Initia"))
                        {
                            pclt.bdNo = ce.GetCellValue(ws, i, 2);
                            pclt.pNo = ce.GetCellValue(ws, i, 4);
                            pclt.number = int.Parse(ce.GetCellValue(ws, i, 7));
                            pclt.numberDtl = ce.GetCellValue(ws, i, 7);
                        }
                        else
                        {
                            pclt.bdNo = ce.GetCellValue(ws, i, 3);
                            pclt.pNo = ce.GetCellValue(ws, i, 5);
                            pclt.number = int.Parse(ce.GetCellValue(ws, i, 8));
                            pclt.numberDtl = ce.GetCellValue(ws, i, 8);
                        }

                        int[] height = { 1299, 1800, 1300 };
                        Random r = new Random();
                        int b = r.Next(0, height.Length);
                        pclt.height = height[b];
                        if (sheetName == "BulksInitial")
                        {
                           isSuccess= InsertContainer("Bulks",pclt);
                        }
                        else if (sheetName == "BulksLoading")
                        {
                            isSuccess = InsertSql(sheetName, pclt);
                        }
                        else if (sheetName == "PalletInitialInventoryConstant")
                        {
                            isSuccess = InsertContainer("Pallet", pclt);
                        }
                        else if (sheetName == "PalletInitiaInventoryNoConstant")
                        {
                            isSuccess = InsertContainer("Pallet", pclt);
                        }
                        else if (sheetName == "PalletLoadingConstant")
                        {
                            isSuccess = InsertSql(sheetName, pclt);
                        }
                        else if (sheetName == "PalletLoadingNoConstant")
                        {
                            isSuccess = InsertSql(sheetName, pclt);
                        }
                        if (totalNumber==i&&TmpLTs.Count != 0)
                        {
                            string insertString = string.Format(@"insert into {0} (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD,Number,NumberDtl,CreateTime) values
({1},0,'{2}','{3}','{4}',{5},{6},{7},'{8}','{9}',{10},{11},'{12}',{14},'{15}','{13}')", "dbo.T_Task_Load", 
TmpLTs[0].times, TmpLTs[0].taskNo, TmpLTs[0].palletOrBoxNo, "[\"\"]", Convert.ToInt16(TmpLTs[0].isHengWen), 1, -1, TmpLTs[0].bdNo, TmpLTs[0].pNo, TmpLTs[0].weight, TmpLTs[0].height, "", DateTime.Now.ToString(), TmpLTs[0].number, TmpLTs[0].numberDtl);
                            comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                            TmpLTs.RemoveAt(0);
                        }
                        if (!isSuccess)
                        {
                            throw new Exception(sheetName+"第"+i+"条数据插入数据库失败");
                        }
                    }
                   
                        finishNumber++;
                        progressBar1.PerformStep();
                        textBox2.Text = string.Format(@"{0}:完成{1}/{2}", sheetName, finishNumber, totalNumber);
          
                }

            }
        }

        #region
        //将从Excel中读取的数据插入内存中     
        //private void InsertList(string sheetName, List<LoadData> List, LoadData task)
        //{
        //    if (List.Count == 0)
        //    {
        //        List.Add(task);
        //    }
        //    else
        //    {
        //        for (int j = 0; j < List.Count(); j++)
        //        {
        //            if (List[j].palletOrBoxNo == task.palletOrBoxNo && List[j].isHengWen == task.isHengWen)
        //            {
        //                List[j].bdNo += "," + task.bdNo;
        //                List[j].pNo += "," + task.pNo;
        //                List[j].number = List[j].number + task.number;
        //                List[j].numberDtl += "," + task.number.ToString();
        //                if (sheetName.Contains("Pallet"))
        //                {
        //                    if (List[j].isOverWeight || task.isOverWeight)
        //                    {
        //                        List[j].isOverWeight = true;
        //                    }
        //                    else
        //                    {
        //                        List[j].isOverWeight = false;
        //                    }
        //                    if (List[j].height < task.height)
        //                    {
        //                        List[j].height = task.height;
        //                    }
        //                }
        //                break;
        //            }
        //            else
        //            {
        //                if (j == (List.Count() - 1))
        //                {
        //                    List.Add(task);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        //插入数据库任务表
        private bool InsertSql(string sheetName, LoadData list)
        {
            string tableName = "dbo.T_Task_Load";      
            string date = DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            string sql = string.Format(@"SELECT TOP 1 TaskNo FROM {0} WHERE TaskNo LIKE '%{1}%' ORDER BY TaskNo DESC", tableName,date);


            string taskno = "";
            if (comnsql.GetData(Properties.Settings.Default.sql, sql) != null)
            {
                taskno = comnsql.GetData(Properties.Settings.Default.sql, sql).ToString();
            }
            string number = "";
           
            if (!string.IsNullOrEmpty(taskno))
            {
                number = date + (int.Parse(taskno.Substring(taskno.Length - 4, 4)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                number = date + "0001";

            }
            int weight;
            if (list.isOverWeight)
            {
                weight = 500;
                list.weight = weight;
            }
            else
            {
                weight = 200;
                list.weight = weight;
            }
            if (TmpLTs.Count == 0)
            {
                string TaskNo = "LTaskNo" + number;
                list.taskNo = TaskNo;                
            }
            else if (TmpLTs.Count == 1 && TmpLTs[0].palletOrBoxNo == list.palletOrBoxNo && TmpLTs[0].times == list.times && TmpLTs[0].isHengWen == list.isHengWen)
            {
                list.taskNo = TmpLTs[0].taskNo;
                if (list.isOverWeight||TmpLTs[0].isOverWeight)
                {
                    weight = 500;
                    list.weight = weight;
                }
                else
                {
                    weight = 200;
                    list.weight = weight;
                }
                list.bdNo = TmpLTs[0].bdNo + "," + list.bdNo;
                list.pNo = TmpLTs[0].pNo + "," + list.pNo;
                list.number = TmpLTs[0].number + list.number;
                list.numberDtl = TmpLTs[0].numberDtl + "," + list.numberDtl;
                if (list.height < TmpLTs[0].height)
                {
                    list.height = TmpLTs[0].height;
                }
            }
            else
            {
                list.taskNo = TmpLTs[0].taskNo.Substring(0, TmpLTs[0].taskNo.Length - 4) + (int.Parse(TmpLTs[0].taskNo.Substring(TmpLTs[0].taskNo.Length - 4, 4)) + 1).ToString().PadLeft(4, '0');
            }
           
            if (TmpLTs.Count == 1 && TmpLTs[0].palletOrBoxNo != list.palletOrBoxNo)
            {
                string insertString = string.Format(@"insert into {0} (Times, State, TaskNo, SN, PlanningNo, IsHengwen, IsBLOCK, TaskType, BDNO, Pn, Weight, Height, SN_OLD,Number,NumberDtl,CreateTime) values
({1},0,'{2}','{3}','{4}',{5},{6},{7},'{8}','{9}',{10},{11},'{12}',{14},'{15}','{13}')", tableName, TmpLTs[0].times, TmpLTs[0].taskNo, TmpLTs[0].palletOrBoxNo, "[\"\"]", Convert.ToInt16(TmpLTs[0].isHengWen), 1, -1, TmpLTs[0].bdNo, TmpLTs[0].pNo, TmpLTs[0].weight, TmpLTs[0].height, "", DateTime.Now.ToString(), TmpLTs[0].number, TmpLTs[0].numberDtl);
                int a = comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                if (a == 1)
                {
                    TmpLTs.RemoveAt(0);
                    TmpLTs.Add(list);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (TmpLTs.Count > 0)
                {
                    TmpLTs.RemoveAt(0);
                }              
                TmpLTs.Add(list);
                return true;
            }
           
           
        }
        
        //插入数据库库存
        private bool InsertContainer(string sheetName, LoadData list)
        {
            //整托
            if (sheetName == "Pallet")
            {
                string warecell="";
                if (TmpPCLs.Count == 0 || TmpPCLs.Count == 1 && TmpPCLs[0].palletOrBoxNo != list.palletOrBoxNo)
                {
                    SqlParameter[] paras = new SqlParameter[7];
                    paras[0] = new SqlParameter("@PalletNo", SqlDbType.NVarChar, 50);
                    paras[1] = new SqlParameter("@IsNormalT", SqlDbType.Bit);
                    paras[2] = new SqlParameter("@Weight", SqlDbType.Int);
                    paras[3] = new SqlParameter("@Height", SqlDbType.Int);
                    paras[4] = new SqlParameter("@TaskType", SqlDbType.NVarChar);
                    paras[5] = new SqlParameter("@WareCell", SqlDbType.NVarChar, 50);
                    paras[6] = new SqlParameter("@RoadWay", SqlDbType.Int);
                    paras[0].Value = list.palletOrBoxNo;
                    paras[1].Value = list.isHengWen;
                    paras[2].Value = list.weight;
                    paras[3].Value = list.height;
                    paras[4].Value = "taskPallet_Entrance_LK";
                    paras[5].Direction = ParameterDirection.Output;
                    paras[6].Direction = ParameterDirection.Output;
                    warecell = comnsql.ProcOutput(Properties.Settings.Default.sql1, "P_GetWareCell", paras, "@WareCell").ToString();
                }
                else
                {
                    warecell = TmpPCLs[0].hubno;
                }
                
                list.hubno = warecell;
                string updateString = string.Format(@" 
UPDATE dbo.T_Biz_Container_Pallet
SET  wcState = 'wcState_Store'
, StoreType = 'Container_Pallet'
, PalletNo = '{0}'
, UpdateTime = getdate()
WHERE wcName ='{1}'", list.palletOrBoxNo, warecell);
                int b = comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                
               string insertString = string.Format(@" 
INSERT INTO dbo.T_Biz_Container_PalletDtl (Warecell, PalletNo, BDNo, PNo, Number, IsOverWeight, IsHengWen, IsQualified, CreateTime)
VALUES ('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, 1, getdate())", warecell,list.palletOrBoxNo,list.bdNo,list.pNo,list.number,
                                                                 Convert.ToInt16(list.isOverWeight),Convert.ToInt16(list.isHengWen));
                int bb = comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                if (b==1&&bb==1)
                {
                    TmpPCLs.Add(list);
                    list.state = 1;
                    string updateString1 = string.Format(@"UPDATE T_Base_WareCell SET wcState = 'wcState_Store' WHERE wcName = '{0}'", warecell);
                    comnsql.DataOperator(Properties.Settings.Default.sql1, updateString1);
                    if (TmpPCLs.Count > 1)
                    {
                        TmpPCLs.RemoveRange(0, TmpPCLs.Count - 1);
                    }
                    return true;
                }
                return false;                      
            }
            //散件
            else if (sheetName == "Bulks")
            {
               
                string updateZwmsString = "";
                string deleteWcsString = "";
                string updateWmsString = "";
                string shelfdtlno = "", shelfno="";
                if (TmpBLCs.Count == 0 || TmpBLCs.Count == 1 && TmpBLCs[0].palletOrBoxNo != list.palletOrBoxNo)
                {
                    SqlParameter[] paras = new SqlParameter[2];
                    paras[0] = new SqlParameter("@Station", SqlDbType.NVarChar, 50);
                    paras[1] = new SqlParameter("@ShelfNo", SqlDbType.NVarChar, 50);
                    paras[0].Value = "wstest";
                    paras[1].Direction = ParameterDirection.Output;
                    shelfno = comnsql.ProcOutput(Properties.Settings.Default.sql1, "P_GetShelfOut_InArea", paras, "@ShelfNo").ToString();
                    string[] dtl = { "-01", "-02" };
                    Random r = new Random();
                    int n = r.Next(0, dtl.Length);
                    shelfdtlno = shelfno + dtl[n];
                }
                else
                {
                    shelfno = TmpBLCs[0].shelf;
                    shelfdtlno = TmpBLCs[0].hubno;
                    
                }
                list.hubno = shelfdtlno;
                list.shelf = shelfno;
                                  
                string select = string.Format(@"SELECT cBoxNo FROM dbo.T_Biz_Container_Bulks where cShelfDtl='{0}'", shelfdtlno);
                DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, select);
                DataRow dr = ds.Tables[0].Rows[0];
                string cBoxNo = "";
                if (string.IsNullOrEmpty(dr["cBoxNo"].ToString()))
                {
                    cBoxNo = list.palletOrBoxNo;
                }
                else if (!string.IsNullOrEmpty(dr["cBoxNo"].ToString()) && !dr["cBoxNo"].ToString().Contains(list.palletOrBoxNo))
                {
                    cBoxNo = dr["cBoxNo"].ToString() + "," + list.palletOrBoxNo;
                }
                else
                {
                    cBoxNo = dr["cBoxNo"].ToString();
                }  
                updateWmsString += string.Format(@"UPDATE dbo.T_Biz_Container_Bulks SET cState = 'wcState_Store', cType = 'Container_Box', cBoxNo = '{0}', cUpdateTime = GETDATE() WHERE cShelfDtl= '{1}'"
                    , cBoxNo, shelfdtlno);
                int d = comnsql.DataOperator(Properties.Settings.Default.sql, updateWmsString);
                string insertString = string.Format(@" 
INSERT INTO dbo.T_Biz_Container_BulksDtl (ShelfNo, BoxNo, BDNo, PNo, Number, Weight, IsQualified, CreateTime)
VALUES ('{0}','{1}', '{2}', '{3}', {4}, 0, 1, getdate())",shelfdtlno,list.palletOrBoxNo,list.bdNo,list.pNo,list.number);
                int dd = comnsql.DataOperator(Properties.Settings.Default.sql, insertString);
                if (d == 1 && dd==1)
                {
                    TmpBLCs.Add(list);
                    updateZwmsString += string.Format(@"UPDATE dbo.T_Base_Shelf SET sfState = 'sfState_Store',sfObjective = '' WHERE wcFName = '{0}' ", shelfno);
                    updateZwmsString += string.Format(@"UPDATE dbo.T_Base_ShelfDtl SET wcBoxNo = '{0}', wcState = 'wcState_Store', wcEditTime = GETDATE() WHERE wcName='{1}' ", list.palletOrBoxNo, shelfdtlno);
                    updateZwmsString += string.Format(@"INSERT INTO dbo.T_Biz_Container
	    ( cBoxNo ,
	        IsQualified ,
			cType,
	        cState ,
	        cWeight ,
	        cWareCell ,
	        cNotes ,
	        cCreateWho ,
	        cCreateTime
	    )
VALUES  ( '{0}' , 
	        '1' , 
			'Container_Box',
	        'Container_Store' ,
	        0 ,
	        '{1}' , 
	        '' , 
	        'WMS' , 
	        GETDATE()
	    )", list.palletOrBoxNo, shelfdtlno);
                    comnsql.DataOperator(Properties.Settings.Default.sql1, updateZwmsString);
                    deleteWcsString += "DELETE FROM dbo.I_Wcs_Acs_Interface WHERE EndStation='wstest'";
                    comnsql.DataOperator(Properties.Settings.Default.sqlwcs, deleteWcsString);

                    list.state = 1;
                    if (TmpBLCs.Count > 1)
                    {
                        TmpBLCs.RemoveRange(0, TmpBLCs.Count - 1);
                    }
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    
        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            int a = (DateTime.Now.Day-1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            for (int i = 0; i < sendLoadTasksList.Count; i++)
            {
                if (sendLoadTasksList[i].times == a && sendLoadTasksList[i].state == 0)
                {
                    SendLoadTask(sendLoadTasksList[i]);
                }
            }
            for (int i = 0; i < sendUnloadTasksList.Count; i++)
            {
                if (sendUnloadTasksList[i].times == a && sendUnloadTasksList[i].state == 0)
                {
                    SendUnLoadTask(sendUnloadTasksList[i]);
                }
            }
          
        }

        //到指定时间下发下架任务
        private void SendUnLoadTask(UnLoadData task)
        {
            int loadSuccessNumber = 0;
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""taskType"":""{1}"",
""taskState"":""{2}"",
""priority"":{3},
""OUTNO"":""{4}"",
""planningNo"":"""",
""SN"":""{5}"",
""hubno"":""{6}"",
""isUnpackTray"":0,
""isBoxLable"":0,
""isMinpackLable"":-1,
""isPipeline"":{7},
""SN_OLD"":""{8}"",
""station"":"""",
""opTime"":""{9}""}}]",task.taskNo ,task.taskType,task.taskState,task.priority,task.outNo, task.palletOrBoxNo,task.hubno,task.isPipeline, task.palletOrBoxNo, DateTime.Now.ToString());

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url + "/unloadTasks";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded";
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
                    richTextBox1.Text += DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                    loadSuccessNumber++;
                    string updateString = string.Format(@"UPDATE dbo.T_Task_UnLoad set state=1,SendTime ='{0}' where TaskNo='{1}'", DateTime.Now.ToString(), task.taskNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                    task.state = 1;
                    sendUnloadTasksList.Remove(sendUnloadTasksList.Find(a => a.taskNo == task.taskNo));
                }
                else
                {
                    richTextBox1.Text += DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
            //MessageBox.Show("下发" + loadSuccessNumber + "条上架任务成功");
        
        }

        //到指定时间下发上架任务
        private void SendLoadTask(LoadData task)
        {
            int loadSuccessNumber = 0;
            string str = string.Format(@"[{{""taskNo"":""{0}"",
""SN"":""{1}"",
""planningNo"":{2},
""isHengwen"":{3},
""isBLOCK"":1,
""taskType"":{4},
""BDNO"":""{6}"",
""pn"":""{7}"",
""weight"":{8},
""height"":{9},
""SN_OLD"":"""",
""opTime"":""{5}""}}]",task.taskNo ,task.palletOrBoxNo, "[\"\"]", Convert.ToInt16(task.isHengWen), -1, DateTime.Now.ToString(), task.bdNo, task.pNo,task.weight , task.height);

            JSONArray jsarray = new JSONArray(str);
            string url = Properties.Settings.Default.url+"/loadTasks";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded";
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
                    richTextBox1.Text += DateTime.Now.ToString() + " 任务下发成功，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                    loadSuccessNumber++;
                    string updateString = string.Format(@"UPDATE dbo.T_Task_Load set state=1,SendTime ='{0}' where TaskNo='{1}'", DateTime.Now.ToString(), task.taskNo);
                    comnsql.DataOperator(Properties.Settings.Default.sql, updateString);
                    task.state = 1;
                    sendLoadTasksList.Remove(sendLoadTasksList.Find(a => a.taskNo == task.taskNo));
                }
                else
                {
                    richTextBox1.Text += DateTime.Now.ToString() + " 任务下发失败，任务号：" + task.taskNo + "\n" + "========" + resp + "\n";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
            }
            //MessageBox.Show("下发" + loadSuccessNumber + "条上架任务成功");
        }

        //每天23：50下载下一天新数据
        private void timer2_Tick(object sender, EventArgs e)
        {
            //int a = (DateTime.Now.Day-1) * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
            //if (a == (DateTime.Now.Day * 86400 - 10))
            //{
            //    timer2.Interval =10000;
            //    InitTask();
            //}
            timer2.Interval = 10000;
            InitTask();
        }

        //启动监听
        private void button1_Click(object sender, EventArgs e)
        {
            WebServiceHost host = new WebServiceHost(typeof(SNInfoWcf), new Uri(textBox3.Text));
            host.Open();
            MessageBox.Show(textBox3.Text + "已经开始监听");
            button1.Enabled = false;
        }
    }
}
