using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WEICHUANG.Common;

namespace WEICHUANG
{
    public partial class Form1 : Form
    {

        string fileName = "";
        DAL_Comn_Sql comnsql = new DAL_Comn_Sql();
        DAL_Comn_Log comnlog = new DAL_Comn_Log();
        List<Material> existM = new List<Material>();
        List<LoadTask> LT = new List<LoadTask>();

        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            LogWrite.WriteMain += new LogWrite.WriteLogDelegate(WriteLog);
            InitMaterial();
            progressBar1.Visible = false;
        }

        private void WriteLog(string log)
        {
            richTextBox1.Text += log;
        }

        private void button1_Click(object sender, EventArgs e)
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
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 开始读取Excel文件：" + textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1] + "数据\n");
                comnlog.MessageLog("任务处理", " 开始读取Excel文件：" + textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1] + "数据\n");
                button1.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && (!button1.Enabled))
            {
                GetExcelData();
            }
        }

        private void GetExcelData()
        {
             try
             {               
                 if (string.IsNullOrEmpty(fileName))
                 {
                    MessageBox.Show("请选择文件");
                    return;
                 }
                 ManageData();
                 button1.Enabled = true;              
                 string file = textBox1.Text.Split('\\')[textBox1.Text.Split('\\').Length - 1];
                 textBox1.Text = "";
                 LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 处理Excel文件:" + file + " 完成" + "\n");
                 comnlog.MessageLog("任务处理", " 处理Excel文件:" + file + " 完成" + "\n");
             }
             catch (Exception Ex)
             {
                 throw new Exception(Ex.ToString());
             }
        }

        private void ManageData()
        {
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
            
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

            for (int n = 0; n < schemaTable.Rows.Count; n++)
            {
                if (schemaTable.Rows[n][2].ToString().Trim().EndsWith("$") && schemaTable.Rows[n][2].ToString().Trim().StartsWith("Material"))
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 1;
                    progressBar1.Visible = true;
                    ManageProdunct(schemaTable, n, strConn,conn);                    
                }
                if (schemaTable.Rows[n][2].ToString().Trim().EndsWith("$") && schemaTable.Rows[n][2].ToString().Trim().StartsWith("LoadTask"))
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = 1;
                    progressBar1.Visible = true;
                    ManageLoadTasks(schemaTable, n, strConn, conn);
                }
            }
        }

        private void ManageLoadTasks(DataTable schemaTable, int n, string strConn, OleDbConnection conn)
        {
            string lostInfo = "";
            List<LoadTask> LTList = new List<LoadTask>();          
            List<Statistics> StatisticsList = new List<Statistics>();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = new DataSet();
            string[] ltElements = { "Times", "DTime", "TaskNo", "PalletOrBoxNo", "CPN", "Category", "Quantity", "CreateTime", "SendTime", "FinishTime", "Flag" };
            string sheetName = schemaTable.Rows[n][2].ToString().Trim();
            strExcel = string.Format("select * from [{0}]", sheetName);
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            myCommand.Fill(ds, sheetName);
            conn.Close();
            DataTable dt = ds.Tables[sheetName];
            //int finishNumber = 0;
            int totalNumber = dt.Rows.Count;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalNumber;
            progressBar1.Step = 1;
            DataTable dtlt = new DataTable();
            dtlt.Columns.Add("Times", typeof(int));
            dtlt.Columns.Add("DTime", typeof(string));
            dtlt.Columns.Add("TaskNo", typeof(string));
            dtlt.Columns.Add("PalletOrBoxNo", typeof(string));
            dtlt.Columns.Add("CPN", typeof(string));
            dtlt.Columns.Add("Category", typeof(string));
            dtlt.Columns.Add("Quantity", typeof(int));
            dtlt.Columns.Add("CreateTime", typeof(string));
            dtlt.Columns.Add("SendTime", typeof(string));
            dtlt.Columns.Add("FinishTime", typeof(string));
            dtlt.Columns.Add("Flag", typeof(int));
            for (int i = 0; i < totalNumber; i++)
            {
                LoadTask lt = new LoadTask();
                lt.Times = Convert.ToInt32(dt.Rows[i][0]);
                lt.DTime = dt.Rows[i][1].ToString();
                lt.TaskNo = "";
                lt.PalletOrBoxNo = "";
                lt.CPN = dt.Rows[i][3].ToString();
                string Cate = dt.Rows[i][4].ToString();
                if (Cate == "大件")
                {
                    lt.Category = "L";
                }
                else
                {
                    lt.Category = "S";
                }
                lt.Quantity = Convert.ToInt32(dt.Rows[i][5]);
                if (StatisticsList.FindAll(a => a.DTime == lt.DTime).Count == 0)
                {
                    Statistics s = new Statistics();
                    s.DTime = lt.DTime;
                    s.dataCount = 1;
                    StatisticsList.Add(s);
                }
                else
                {
                    StatisticsList.Find(a => a.DTime == lt.DTime).dataCount += 1;
                }
                if (LTList.FindAll(a => a.Times==lt.Times&&a.Category==lt.Category&&a.CPN == lt.CPN ).Count == 0)
                {
                    LTList.Add(lt);
                }
                else
                {
                    LTList.Find(a => a.CPN == lt.CPN && a.Times == lt.Times && a.Category == lt.Category).Quantity += lt.Quantity;
                }
                progressBar1.PerformStep();
            }
            progressBar1.Visible = false;
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 读取" + LTList.Count + "条上架数据完成\n");
            comnlog.MessageLog("任务处理", " 读取" + LTList.Count + "条上架数据完成\n");
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 正在处理上架数据\n");
            comnlog.MessageLog("任务处理", " 正在处理上架数据\n");

            for (int j = 0; j < StatisticsList.Count; j++)
            {
                List<LoadTask> tmpLTList = LTList.FindAll(a => a.DTime == StatisticsList[j].DTime);               
              
                int lostCPNCount = 0;
                for (int z = 0; z < tmpLTList.Count; z++)
                {
                    if (existM.Find(a => a.CPN == tmpLTList[z].CPN) == null)
                    {
                        //comnlog.MessageLog("料号缺少信息", "无料号为" + tmpLTList[z].CPN + "的产品信息\n");
                        lostCPNCount += 1;
                        //lostInfo += DateTime.Now.ToString() + "无料号为" + tmpLTList[z].CPN + "的产品信息\n";
                        continue;
                    }
                    else
                    {
                        tmpLTList[z].Category = existM.Find(a => a.CPN == tmpLTList[z].CPN).Category;
                        int cpb = existM.Find(a => a.CPN == tmpLTList[z].CPN).QuantityPerBox;
                        int boxNo = tmpLTList[z].Quantity / cpb;
                        int lastNo = tmpLTList[z].Quantity - boxNo * cpb;
                        if (tmpLTList[z].Category == "S")
                        {
                            CreateBoxTask(LT, tmpLTList[z], cpb, boxNo, lastNo);
                            //LTList.Remove(tmpLTList[z]);
                        }
                        else
                        {
                            
                            if (lastNo != 0)
                            {
                                boxNo += 1;
                            }
                            int palletNo = boxNo / existM.Find(a => a.CPN == tmpLTList[z].CPN).BoxCountPerPallet;                        
                            int lastBoxNo1 = boxNo % existM.Find(a => a.CPN == tmpLTList[z].CPN).BoxCountPerPallet;
                            if (lastBoxNo1 > 0 && lastBoxNo1 < 5)
                            {
                                if (lastNo != 0)
                                {
                                    CreateBoxTask(LT, tmpLTList[z], cpb, (lastBoxNo1 - 1), lastNo);
                                }
                                else
                                {
                                    CreateBoxTask(LT, tmpLTList[z], cpb, lastBoxNo1, lastNo);
                                }

                            }
                            if ((palletNo > 0 || lastBoxNo1 > 4))
                            {
                                CreatePalletTask(LT, tmpLTList[z], cpb * existM.Find(a => a.CPN == tmpLTList[z].CPN).BoxCountPerPallet, palletNo , lastBoxNo1,lastNo);
                            }
                            //LTList.Remove(tmpLTList[z]);
                        }
                    }                
                }
                LTList.RemoveAll(a => a.DTime == StatisticsList[j].DTime);
                StatisticsList[j].totalmaterialCount = tmpLTList.Count;
                StatisticsList[j].smaterialCount = tmpLTList.FindAll(a => a.Category == "S").Count;
                StatisticsList[j].lmaterialCount = tmpLTList.FindAll(a => a.Category == "L").Count;
                StatisticsList[j].palletCount = LT.FindAll(a => a.DTime == StatisticsList[j].DTime&&a.PalletOrBoxNo.StartsWith("P")).Count;
                StatisticsList[j].boxCount = LT.FindAll(a => a.DTime == StatisticsList[j].DTime && a.PalletOrBoxNo.StartsWith("C")).Count;
                StatisticsList[j].totalTaskCount = LT.FindAll(a => a.DTime == StatisticsList[j].DTime).Count;
                StatisticsList[j].lboxCount = LT.FindAll(a => a.DTime == StatisticsList[j].DTime && a.PalletOrBoxNo.StartsWith("C")&&a.Category=="L").Count;
                StatisticsList[j].sboxCount = LT.FindAll(a => a.DTime == StatisticsList[j].DTime && a.PalletOrBoxNo.StartsWith("C") && a.Category == "S").Count;
                StatisticsList[j].lostmaterialCount = lostCPNCount;
                //comnlog.MessageLog("料号缺少信息", lostInfo);
            }
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 处理上架数据完成\n");
            comnlog.MessageLog("任务处理", " 处理上架数据完成\n");
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 正在插入上架任务\n");
            comnlog.MessageLog("任务处理", " 正在插入上架任务\n");

            for (int h = 0; h < LT.Count; h++)
            {
                DataRow drlt = dtlt.NewRow();
                drlt["Times"] = LT[h].Times;
                drlt["DTime"] = LT[h].DTime;
                drlt["TaskNo"] = LT[h].TaskNo;
                drlt["PalletOrBoxNo"] = LT[h].PalletOrBoxNo;
                drlt["CPN"] = LT[h].CPN;
                drlt["Category"] = LT[h].Category;
                drlt["Quantity"] = LT[h].Quantity;
                drlt["CreateTime"] = DateTime.Now.ToString();
                drlt["SendTime"] =null;
                drlt["FinishTime"] = null;
                drlt["Flag"] = 0;
                dtlt.Rows.Add(drlt);
            }

            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(Properties.Settings.Default.sql))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "T_Task_Load";
                for (int j = 0; j < ltElements.Length; j++)
                {
                    sqlBC.ColumnMappings.Add(ltElements[j], ltElements[j]);
                }
                sqlBC.WriteToServer(dtlt);
            }            
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 插入" + LT.Count + "条上架任务完成\n");
            comnlog.MessageLog("任务处理", " 插入" + LT.Count + "条上架任务完成\n");
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 正在插入统计\n");
            comnlog.MessageLog("任务处理", " 正在插入统计\n");
            LT.Clear();

            DataTable dts = new DataTable();          
            dts.Columns.Add("DTime", typeof(string));
            dts.Columns.Add("DataCount", typeof(int));
            dts.Columns.Add("TotalmaterialCount", typeof(int));
            dts.Columns.Add("LmaterialCount", typeof(int));
            dts.Columns.Add("SmaterialCount", typeof(int));
            dts.Columns.Add("LostmaterialCount", typeof(int));
            dts.Columns.Add("PalletCount", typeof(int));
            dts.Columns.Add("BoxCount", typeof(int));
            dts.Columns.Add("LBoxCount", typeof(int));
            dts.Columns.Add("SBoxCount", typeof(int));
            dts.Columns.Add("TotalTaskCount", typeof(int));
            dts.Columns.Add("CreateTime", typeof(string));
            for (int k = 0; k < StatisticsList.Count; k++)
            {
                DataRow drs = dts.NewRow();
                drs["DTime"] = StatisticsList[k].DTime;
                drs["DataCount"] = StatisticsList[k].dataCount;
                drs["TotalmaterialCount"] = StatisticsList[k].totalmaterialCount;
                drs["LmaterialCount"] = StatisticsList[k].lmaterialCount;
                drs["SmaterialCount"] = StatisticsList[k].smaterialCount;
                drs["LostmaterialCount"] = StatisticsList[k].lostmaterialCount;
                drs["PalletCount"] = StatisticsList[k].palletCount;
                drs["BoxCount"] = StatisticsList[k].boxCount;
                drs["LBoxCount"] = StatisticsList[k].lboxCount;
                drs["SBoxCount"] = StatisticsList[k].sboxCount;
                drs["TotalTaskCount"] = StatisticsList[k].totalTaskCount;
                drs["CreateTime"] = DateTime.Now.ToString();
                dts.Rows.Add(drs);
            }
            string[] sElements = { "DTime", "DataCount", "TotalmaterialCount", "LmaterialCount", "SmaterialCount","LostmaterialCount", "PalletCount", "BoxCount", "LBoxCount","SBoxCount","TotalTaskCount", "CreateTime" };
            
            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(Properties.Settings.Default.sql))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "T_Base_Statistics";
                for (int j = 0; j < sElements.Length; j++)
                {
                    sqlBC.ColumnMappings.Add(sElements[j], sElements[j]);
                }
                sqlBC.WriteToServer(dts);
            }
            LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 插入" + StatisticsList.Count + "条统计数据完成\n");
            comnlog.MessageLog("任务处理", " 插入" + StatisticsList.Count + "条统计数据完成\n");
            StatisticsList.Clear();
        }

        private void CreatePalletTask(List<LoadTask> LT, LoadTask loadTask, int p, int palletNo, int lastNo1,int lastNo)
        {
            int num = palletNo;
            if (lastNo1 >4)
            {
                num += 1;
            }
            for (int n = 0; n < num; n++)
            {
                LoadTask tmplt = new LoadTask();
                tmplt.CPN = loadTask.CPN;
                tmplt.Category = loadTask.Category;
                tmplt.DTime = loadTask.DTime;
                tmplt.Times = loadTask.Times;
                if (LT.FindAll(a => a.DTime.Split(' ')[0] == tmplt.DTime.Split(' ')[0]).Count == 0)
                {
                    tmplt.TaskNo = "LT" + tmplt.DTime.Split('/')[0].PadLeft(4, '0') + tmplt.DTime.Split('/')[1].PadLeft(2, '0') + tmplt.DTime.Split('/')[2].Split(' ')[0].PadLeft(2, '0') + "000001";
                }
                else
                {
                    tmplt.TaskNo = LT.OrderByDescending(a => a.TaskNo).FirstOrDefault().TaskNo.Substring(0, 10)
                        + (Convert.ToInt32(LT.OrderByDescending(a => a.TaskNo).FirstOrDefault().TaskNo.Substring(10, 6)) + 1).ToString().PadLeft(6, '0');          
                }
                if (LT.FindAll(a => a.PalletOrBoxNo.StartsWith("PX") && a.DTime.Split(' ')[0] == tmplt.DTime.Split(' ')[0]).Count == 0)
                {
                    tmplt.PalletOrBoxNo = "PX" + tmplt.DTime.Split('/')[0].PadLeft(4, '0') + tmplt.DTime.Split('/')[1].PadLeft(2, '0') + tmplt.DTime.Split('/')[2].Split(' ')[0].PadLeft(2, '0') + "000001";
                }
                else
                {
                    tmplt.PalletOrBoxNo = LT.FindAll(a => a.PalletOrBoxNo.StartsWith("PX")).OrderByDescending(a => a.PalletOrBoxNo).FirstOrDefault().PalletOrBoxNo.Substring(0, 10)
                    + (Convert.ToInt32(LT.FindAll(a => a.PalletOrBoxNo.StartsWith("PX")).OrderByDescending(a => a.PalletOrBoxNo).FirstOrDefault().PalletOrBoxNo.Substring(10, 6)) + 1).ToString().PadLeft(6, '0');
                }
                if (n == (palletNo - 1) && lastNo1 == 0 && lastNo != 0)
                {
                    tmplt.Quantity = loadTask.Quantity - (palletNo-1) * p;
                }
                else if (n < palletNo)
                {
                    tmplt.Quantity = p;
                }
                else
                {
                    tmplt.Quantity = loadTask.Quantity - palletNo*p;
                }
                LT.Add(tmplt);
            }
        }

        private void CreateBoxTask(List<LoadTask> LT, LoadTask loadTask, int cpb, int boxNo, int lastNo)
        {
            int num=boxNo;
            if (lastNo != 0)
            {
                num += 1;
            }
            for (int n = 0; n < num; n++)
            {
                LoadTask tmplt = new LoadTask();
                tmplt.CPN = loadTask.CPN;
                tmplt.Category = loadTask.Category;
                tmplt.DTime = loadTask.DTime;
                tmplt.Times = loadTask.Times;
                if (LT.FindAll(a => a.DTime.Split(' ')[0] == tmplt.DTime.Split(' ')[0]).Count == 0)
                {
                    tmplt.TaskNo = "LT" + tmplt.DTime.Split('/')[0].PadLeft(4, '0') + tmplt.DTime.Split('/')[1].PadLeft(2, '0') + tmplt.DTime.Split('/')[2].Split(' ')[0].PadLeft(2, '0') + "000001";
                }
                else
                {
                    tmplt.TaskNo = LT.OrderByDescending(a => a.TaskNo).FirstOrDefault().TaskNo.Substring(0, 10)
                        + (Convert.ToInt32(LT.OrderByDescending(a => a.TaskNo).FirstOrDefault().TaskNo.Substring(10, 6)) + 1).ToString().PadLeft(6, '0');                          
                }
                if (LT.FindAll(a => a.PalletOrBoxNo.StartsWith("CX") && a.DTime.Split(' ')[0] == tmplt.DTime.Split(' ')[0]).Count == 0)
                {
                    tmplt.PalletOrBoxNo = "CX" + tmplt.DTime.Split('/')[0].PadLeft(4, '0') + tmplt.DTime.Split('/')[1].PadLeft(2, '0') + tmplt.DTime.Split('/')[2].Split(' ')[0].PadLeft(2, '0') + "000001";
                }
                else
                {
                    tmplt.PalletOrBoxNo = LT.FindAll(a => a.PalletOrBoxNo.StartsWith("CX")).OrderByDescending(a => a.PalletOrBoxNo).FirstOrDefault().PalletOrBoxNo.Substring(0, 10)
                   + (Convert.ToInt32(LT.FindAll(a => a.PalletOrBoxNo.StartsWith("CX")).OrderByDescending(a => a.PalletOrBoxNo).FirstOrDefault().PalletOrBoxNo.Substring(10, 6)) + 1).ToString().PadLeft(6, '0');
                }          
                if (n < boxNo)
                {
                    tmplt.Quantity = cpb;
                }
                else
                {
                    tmplt.Quantity = lastNo;
                }
                LT.Add(tmplt);
            }    
        }


        private void ManageProdunct(DataTable schemaTable, int n, string strConn, OleDbConnection conn)
        {
            List<Material> addM = new List<Material>();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = new DataSet();
            string[] mElements = { "CPN", "Length", "Width", "Height", "QuantityPerBox", "WeightPerBox", "BoxCountPerPallet", "Product", "Category", "AreaEvaluate" };
            string sheetName = schemaTable.Rows[n][2].ToString().Trim();
            strExcel = string.Format("select * from [{0}]", sheetName);
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            myCommand.Fill(ds, sheetName);
            conn.Close();
            DataTable dt = ds.Tables[sheetName];
            //int finishNumber = 0;
            int totalNumber = dt.Rows.Count;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalNumber;
            progressBar1.Step = 1;
            DataTable dtm = new DataTable();
            dtm.Columns.Add("CPN", typeof(string));
            dtm.Columns.Add("Length", typeof(int));
            dtm.Columns.Add("Width", typeof(int));
            dtm.Columns.Add("Height", typeof(int));
            dtm.Columns.Add("QuantityPerBox", typeof(int));
            dtm.Columns.Add("WeightPerBox", typeof(string));
            dtm.Columns.Add("BoxCountPerPallet", typeof(int));
            dtm.Columns.Add("Product", typeof(string));
            dtm.Columns.Add("Category", typeof(string));
            dtm.Columns.Add("AreaEvaluate", typeof(string));
            dtm.Columns.Add("CreateTime", typeof(string));
            for (int i = 0; i < totalNumber; i++)
            {
                Material m = new Material();
                m.CPN = dt.Rows[i][0].ToString();
                m.Length = Convert.ToInt32(dt.Rows[i][1]);
                m.Width = Convert.ToInt32(dt.Rows[i][2]);
                m.Height = Convert.ToInt32(dt.Rows[i][3]);
                m.QuantityPerBox = Convert.ToInt32(dt.Rows[i][4]);
                m.WeightPerBox = (Convert.ToDouble(dt.Rows[i][5])).ToString();
                m.BoxCountPerPallet = Convert.ToInt32(dt.Rows[i][6]);
                m.Product = dt.Rows[i][7].ToString();
                m.Category = dt.Rows[i][8].ToString();
                m.AreaEvaluate = areaDataChange(dt.Rows[i][9].ToString());
                if (existM.FindAll(a => a.CPN == m.CPN).Count == 0 && addM.FindAll(a => a.CPN == m.CPN).Count == 0)
                {
                    addM.Add(m);
                    DataRow drm = dtm.NewRow();
                    drm["CPN"] = m.CPN;
                    drm["Length"] = m.Length;
                    drm["Width"] = m.Width;
                    drm["Height"] = m.Height;
                    drm["QuantityPerBox"] = m.QuantityPerBox;
                    drm["WeightPerBox"] = m.WeightPerBox;
                    drm["BoxCountPerPallet"] = m.BoxCountPerPallet;
                    drm["Product"] = m.Product;
                    drm["Category"] = m.Category;
                    drm["AreaEvaluate"] = m.AreaEvaluate;
                    drm["CreateTime"] = DateTime.Now.ToString();
                    dtm.Rows.Add(drm);
                }
                progressBar1.PerformStep();
            }

            using (System.Data.SqlClient.SqlBulkCopy sqlBC = new System.Data.SqlClient.SqlBulkCopy(Properties.Settings.Default.sql))
            {
                sqlBC.BatchSize = 100000;
                sqlBC.BulkCopyTimeout = 60;
                sqlBC.DestinationTableName = "T_Base_Material";
                for (int j = 0; j < mElements.Length; j++)
                {
                    sqlBC.ColumnMappings.Add(mElements[j], mElements[j]);
                }
                sqlBC.ColumnMappings.Add("CreateTime", "CreateTime");
                sqlBC.WriteToServer(dtm);
            }
            if (addM.Count != 0)
            {
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 新添加" + addM.Count + "条产品信息完成\n");
                comnlog.MessageLog("任务处理", " 新添加" + addM.Count + "条产品信息完成\n");
            }
            else
            {
                LogWrite.WriteLogToMain(DateTime.Now.ToString() + " 无新产品信息需添加\n");
                comnlog.MessageLog("任务处理", " 无新产品信息需添加\n");
            }

            existM.AddRange(addM);
            addM.Clear();
            progressBar1.Visible = false;  
        }

        private void InitMaterial()
        {
            string ss = string.Format("SELECT * FROM dbo.T_Base_Material");
            DataSet ds = comnsql.SelectGet(Properties.Settings.Default.sql, ss);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Material m = new Material();
                m.CPN = dr["CPN"].ToString();
                m.Length = Convert.ToInt32(dr["Length"]);
                m.Width = Convert.ToInt32(dr["Width"]);
                m.Height = Convert.ToInt32(dr["Height"]);
                m.QuantityPerBox = Convert.ToInt32(dr["QuantityPerBox"]);
                m.WeightPerBox =  dr["WeightPerBox"].ToString();
                m.BoxCountPerPallet = Convert.ToInt32(dr["BoxCountPerPallet"]);
                m.Product = dr["Product"].ToString();
                m.Category = dr["Category"].ToString();
                m.AreaEvaluate = dr["AreaEvaluate"].ToString();
                if (existM.FindAll(a => a.CPN == m.CPN).Count == 0)
                {
                    existM.Add(m);
                }
            }
        }

        public string areaDataChange(string number)
        {
            if (number.StartsWith("0.25"))
            {
                return "1/4";
            }
            else if (number.StartsWith("0.125"))
            {
                return "1/8";
            }
            else if (number.StartsWith("0.0625"))
            {
                return "1/16";
            }
            else if (number.StartsWith("0.083"))
            {
                return "1/12";
            }
            else if (number.StartsWith("0.041"))
            {
                return "1/24";
            }
            else
            {
                return "1/48";
            }
        }

    }
}
