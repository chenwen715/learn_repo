using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StateChange
{
    public partial class Form1 : Form
    {

        List<SkipInfo> skipList=new List<SkipInfo>();
        DAL_Comn_Sql DalSql = new DAL_Comn_Sql();
        Log log = new Log();
        Thread th1= null;
        Thread th2 = null;
        Thread th3 = null;
        Thread th4 = null;
        int xljgqCount = 0,  qxqCount = 0;
        int times = 120;
        int flag = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            button2.Enabled = false;
        }

        private void Init()
        {
            string sql = string.Format(@"SELECT PointNo, AreaNo, Barcode, SkipInfo, SkipDefault, LockAgv, IsLocked, AreaName
FROM dbo.T_Base_SkipInfo WHERE isnull(AreaName,'')<>''");           
            DataSet ds = DalSql.SelectGet(Properties.Settings.Default.sql, sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string Barcode = dr["Barcode"].ToString();
                if (skipList.Find(a => a.Barcode == Barcode) == null)
                {
                    SkipInfo skip = new SkipInfo();
                    skip.PointNo = dr["PointNo"].ToString();
                    skip.AreaNo = dr["AreaNo"].ToString();
                    skip.PointNo = dr["PointNo"].ToString();
                    skip.Barcode = dr["Barcode"].ToString();
                    skip.skipInfo = Convert.ToInt16(dr["SkipInfo"].ToString());
                    skip.SkipDefault = Convert.ToInt16(dr["SkipDefault"].ToString());
                    skip.LockAgv = dr["LockAgv"].ToString();
                    skip.IsLocked = Convert.ToBoolean(dr["IsLocked"].ToString());
                    skip.AreaName = dr["AreaName"].ToString();
                    skip.Count = 0;
                    if (skip.AreaName == "4" || skip.AreaName == "6" || skip.AreaName == "1")
                    {
                        skip.IsAllow = false;
                    }
                    else
                    {
                        skip.IsAllow = true;
                    }
                    skipList.Add(skip);
                }
                else
                {
                    skipList.Find(a => a.Barcode == Barcode).skipInfo = Convert.ToInt16(dr["SkipInfo"].ToString());
                }
                //Console.WriteLine(DateTime.Now.ToString() + "==init==" + Barcode + Convert.ToBoolean(skipList.Find(a => a.Barcode == Barcode).IsAllow)
                //    + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == Barcode).skipInfo));         
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                Init();
                int jcqCount = 0;
                //与升降梯交互
                DalSql.ProcOutput(Properties.Settings.Default.sql, "P_Tmp_InitData", null);
                //检测区
                
                string tmpBarcode11="";               
                string sql7 = string.Format(@"SELECT TOP 1 Barcode FROM dbo.T_Base_SkipInfo WHERE AreaName=1 AND SkipInfo = 1 order by newid()");
                if (DalSql.GetData(Properties.Settings.Default.sql, sql7) != null)
                {
                    tmpBarcode11 = DalSql.GetData(Properties.Settings.Default.sql, sql7).ToString();
                    //Console.WriteLine(DateTime.Now.ToString() + "tmpBarcode" + tmpBarcode11);
                }
                List<SkipInfo> tmpSkipList11 = new List<SkipInfo>();
                tmpSkipList11 = skipList.FindAll(a => a.AreaName == "1").ToList();
                foreach (SkipInfo s in tmpSkipList11)
                {
                    jcqCount += s.Count;
                }             
                if (!string.IsNullOrEmpty(tmpBarcode11) && jcqCount == 0)
                {
                    //Console.WriteLine(DateTime.Now.ToString() + "==1246前==" + Convert.ToBoolean(skipList.Find(a => a.Barcode == "1246").IsAllow)
                    //+ " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == "1246").skipInfo) + "数量" + jcqCount);
                    //Console.WriteLine(DateTime.Now.ToString() + "==1248前==" + Convert.ToBoolean(skipList.Find(a => a.Barcode == "1248").IsAllow)
                    //     + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == "1248").skipInfo) + "数量" + jcqCount);
                    if (!skipList.Find(a => a.Barcode==tmpBarcode11).IsAllow && (th1 == null || th1 != null && (!th1.IsAlive)))
                    {
                        th1 = new Thread(new ParameterizedThreadStart(changeJianCeQu));
                        th1.Start(skipList.Find(a => a.Barcode == tmpBarcode11));
                    }
                    SkipInfo sk = skipList.Find(a => a.IsAllow && a.AreaName == "1");
                    if (sk != null)
                    {
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "tmpBarcode" + sk.Barcode + skipList.Find(a => a.Barcode == sk.Barcode).IsAllow);
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "1246" + skipList.Find(a => a.Barcode == "1246").IsAllow);
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + "1248" + skipList.Find(a => a.Barcode == "1248").IsAllow);

                        string sql8 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo = 2 WHERE Barcode ='{0}'", sk.Barcode);
                        int affectLine = DalSql.DataOperator(Properties.Settings.Default.sql, sql8);
                        if (affectLine == 1)
                        {
                            skipList.Find(a => a.Barcode == sk.Barcode).IsAllow = false;
                            skipList.Find(a => a.Barcode == sk.Barcode).skipInfo = 2;
                            richTextBox1.Text += DateTime.Now.ToString() + "==【1机器人下料点】" + sk.Barcode + "的SkipInfo属性由1变为2\n";
                            log.MessageLog("StateChange", DateTime.Now.ToString() + "==【1机器人下料点】" + sk.Barcode + "的SkipInfo属性由1变为2\n");
                        }
                        else
                        {
                            throw new Exception("1区域改状态失败");
                        }
                    }
                }
                
               
                //加工区
                string tmpBarcode = "";
                string sql = string.Format(@"SELECT TOP 1 Barcode FROM dbo.T_Base_SkipInfo WHERE AreaName=3 AND SkipInfo = 2 ");
                if (DalSql.GetData(Properties.Settings.Default.sql, sql) != null)
                {
                    tmpBarcode = DalSql.GetData(Properties.Settings.Default.sql, sql).ToString();
                    if (skipList.Find(a => a.Barcode == tmpBarcode).IsAllow && xljgqCount == 0)
                    {
                        string sql1 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo = 1 WHERE Barcode ='{0}'", tmpBarcode);
                        int affectLine = DalSql.DataOperator(Properties.Settings.Default.sql, sql1);
                        if (affectLine == 1)
                        {
                            xljgqCount += 1;
                            skipList.Find(a => a.Barcode == tmpBarcode).skipInfo = 1;
                            //skipList.Find(a => a.Barcode == tmpBarcode).IsAllow = false;
                            if (xljgqCount == 1 && (th2 == null || th2 != null && (!th2.IsAlive)))
                            {
                                th2 = new Thread(new ThreadStart(changeXiaLiaoKou));
                                th2.Start();
                            }
                            richTextBox1.Text += DateTime.Now.ToString() + "==【3加工上料点】" + tmpBarcode + "的SkipInfo属性由2变为1\n";
                            log.MessageLog("StateChange", DateTime.Now.ToString() + "==【3加工上料点】" + tmpBarcode + "的SkipInfo属性由2变为1\n");
                        }
                        else
                        {
                            throw new Exception("3区域改状态失败");
                        }
                    }
                }

                SkipInfo tmpSkip1 = skipList.Find(a => a.AreaName == "4" && a.skipInfo == 1 && a.IsAllow);
                if (tmpSkip1 != null && xljgqCount==1)
                {
                    string sql2 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo = 2 WHERE Barcode ='{0}'", tmpSkip1.Barcode);
                    int affectLine1 = DalSql.DataOperator(Properties.Settings.Default.sql, sql2);
                    if (affectLine1 == 1)
                    {
                        tmpSkip1.IsAllow = false;
                        xljgqCount -= 1;
                        tmpSkip1.skipInfo = 2;
                        richTextBox1.Text += DateTime.Now.ToString() + "==【4加工下料点】" + tmpSkip1.Barcode + "的SkipInfo属性由1变为2\n";
                        log.MessageLog("StateChange", DateTime.Now.ToString() + "==【4加工下料点】" + tmpSkip1.Barcode + "的SkipInfo属性由1变为2\n");
                    }
                    else
                    {
                        throw new Exception("4区域改状态失败");
                    }
                }

                //清洗区
                string tmpBarcode2="";
                string sql3 = string.Format(@"SELECT TOP 1 Barcode FROM dbo.T_Base_SkipInfo WHERE AreaName=5  AND SkipInfo = 2");
                if (DalSql.GetData(Properties.Settings.Default.sql, sql3) != null)
                {
                    tmpBarcode2 = DalSql.GetData(Properties.Settings.Default.sql, sql3).ToString();
                    if (skipList.Find(a => a.Barcode == tmpBarcode2).IsAllow && qxqCount == 0)
                    {
                        string sql4 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo = 1 WHERE Barcode ='{0}'", tmpBarcode2);
                        int affectLine = DalSql.DataOperator(Properties.Settings.Default.sql, sql4);
                        if (affectLine == 1)
                        {
                            qxqCount += 1;
                            skipList.Find(a => a.Barcode == tmpBarcode2).skipInfo = 1;
                            //skipList.Find(a => a.Barcode == tmpBarcode2).IsAllow = false;
                            if (qxqCount == 1 && (th3 == null || th3 != null && (!th3.IsAlive)))
                            {
                                th3 = new Thread(new ThreadStart(changeQXXiaLiaoKou));
                                th3.Start();
                            }
                            richTextBox1.Text += DateTime.Now.ToString() + "==【5清洗上料点】" + tmpBarcode2 + "的SkipInfo属性由2变为1\n";
                            log.MessageLog("StateChange", DateTime.Now.ToString() + "==【5清洗上料点】" + tmpBarcode2 + "的SkipInfo属性由2变为1\n");

                        }
                        else
                        {
                            throw new Exception("5区域改状态失败");
                        }
                    }
                }

                SkipInfo tmpSkip2 = skipList.Find(a => a.AreaName == "6" && a.skipInfo == 1 && a.IsAllow);
                if (tmpSkip2 != null && qxqCount == 1)
                {
                    string sql5 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo = 2 WHERE Barcode ='{0}'", tmpSkip2.Barcode);
                    int affectLine1 = DalSql.DataOperator(Properties.Settings.Default.sql, sql5);
                    if (affectLine1 == 1)
                    {
                        tmpSkip2.IsAllow = false;
                        qxqCount -= 1;
                        tmpSkip2.skipInfo = 2;
                        richTextBox1.Text += DateTime.Now.ToString() + "==【6清洗下料点】" + tmpSkip2.Barcode + "的SkipInfo属性由1变为2\n";
                        log.MessageLog("StateChange", DateTime.Now.ToString() + "==【6清洗下料点】" + tmpSkip2.Barcode + "的SkipInfo属性由1变为2\n");
                    }
                    else
                    {
                        throw new Exception("6区域改状态失败");
                    }
                }

                //升降梯
                string sql11 = string.Format(@"SELECT TOP 1 s.Barcode FROM dbo.T_Base_SkipInfo s
	  LEFT JOIN dbo.T_Base_Point p ON s.Barcode = p.BarCode
	 WHERE IsLocked=0 AND SkipInfo=2
	 AND p.PointType='11'");
                object o = DalSql.GetData(Properties.Settings.Default.sql, sql11);
                if (o != null&&(!string.IsNullOrEmpty(o.ToString())) && (th4 == null || th4 != null && (!th4.IsAlive))&&flag==0)
                {
                    th4 = new Thread(new ThreadStart(changeSJT));
                    th4.Start();
                }
                if (o != null && (!string.IsNullOrEmpty(o.ToString())) && flag == 1)
                {
                    string sql111 = string.Format(@"UPDATE dbo.T_Base_SkipInfo SET SkipInfo=1 WHERE Barcode='{0}'", DalSql.GetData(Properties.Settings.Default.sql, sql11).ToString());
                    int affectLine11 = DalSql.DataOperator(Properties.Settings.Default.sql, sql111);
                    if (affectLine11 == 1)
                    {
                        flag = 0;
                        richTextBox1.Text += DateTime.Now.ToString() + "==【11升降梯】" + o.ToString() + "的SkipInfo属性由2变为1\n";
                        log.MessageLog("StateChange",DateTime.Now.ToString() + "==【11升降梯】" + o.ToString() + "的SkipInfo属性由2变为1\n");
                        richTextBoxSetting();
                    }
                    else
                    {
                        throw new Exception("11区域改状态失败");
                    }    
                }
                          
                //Console.WriteLine(DateTime.Now.ToString() +"==1246后=="+ Convert.ToBoolean(skipList.Find(a => a.Barcode == "1246").IsAllow)
                //    + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == "1246").skipInfo) + "数量"+jcqCount);
                //Console.WriteLine(DateTime.Now.ToString() + "==1248后==" + Convert.ToBoolean(skipList.Find(a => a.Barcode == "1248").IsAllow)
                //     + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == "1248").skipInfo) + "数量" + jcqCount);
                //if (th1 != null)
                //{
                //    Console.WriteLine(DateTime.Now.ToString() + "th1" + Convert.ToBoolean(th1.IsAlive));
                //}
                //if (th2 != null)
                //{
                //    Console.WriteLine(DateTime.Now.ToString() + "th2" + Convert.ToBoolean(th2.IsAlive));
                //}
                //if (th3 != null)
                //{
                //    Console.WriteLine(DateTime.Now.ToString() + "th3" + Convert.ToBoolean(th3.IsAlive));
                //}
                richTextBoxSetting();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            

        }

        private void changeSJT()
        {
            Random r = new Random();
            int sleetime = r.Next(20, 60);
            Thread.Sleep(1000 * sleetime);
            flag = 1;
        }

        private void changeQXXiaLiaoKou()
        {
            bool aa = true;
            Thread.Sleep(1000*times);
            while (aa)
            {
                SkipInfo tmpSkip = skipList.FindAll(a => a.AreaName == "6" && a.skipInfo == 1).FirstOrDefault();
                if (tmpSkip != null)
                {
                    tmpSkip.IsAllow = true;
                    aa = false;
                }
                Thread.Sleep(500);
            }          
        }


        private void changeXiaLiaoKou()
        {
            bool aa = true;
            Thread.Sleep(3000 * times);
            while (aa) 
            {
                SkipInfo tmpSkip = skipList.FindAll(a => a.AreaName == "4" && a.skipInfo == 1).FirstOrDefault();
                if (tmpSkip != null)
                {
                    tmpSkip.IsAllow = true;
                    aa = false;
                }
                Thread.Sleep(500);
            }
                   
        }

        private void changeJianCeQu(object obj)
        {
            SkipInfo skip = obj as SkipInfo;
            skip.Count += 1;
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + skip.Barcode + "检测区+1");
            skip.IsAllow = false;
            //Console.WriteLine(DateTime.Now.ToString() +" "+skip.Barcode+ "==前==" + Convert.ToBoolean(skipList.Find(a => a.Barcode == skip.Barcode).IsAllow)
            //       + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == skip.Barcode).skipInfo));
            Thread.Sleep(1000 * times);                      
            skip.Count -= 1;
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + skip.Barcode + "检测区-1");
            skip.IsAllow = true;
            //Console.WriteLine(DateTime.Now.ToString() + " " + skip.Barcode + "==后==" + Convert.ToBoolean(skipList.Find(a => a.Barcode == skip.Barcode).IsAllow)
            //       + " skipInfo " + Convert.ToInt16(skipList.Find(a => a.Barcode == skip.Barcode).skipInfo));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            button1.Text = "已启动";
            button1.Enabled = false;
            button2.Enabled = true;
            richTextBox1.Text += DateTime.Now.ToString() + "==" + "启动\n";
            log.MessageLog("StateChange", DateTime.Now.ToString() + "==" + "启动\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            button2.Text = "已结束";
            button1.Enabled = true;
            button2.Enabled = false;
            richTextBox1.Text += DateTime.Now.ToString() + "==" + "停止\n";
            log.MessageLog("StateChange", DateTime.Now.ToString() + "==" + "停止\n");
        }

        private void richTextBoxSetting()
        {
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.Text.Length, 0);
            richTextBox1.ScrollToCaret();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.MessageLog("StateChange", DateTime.Now.ToString() + "==" + "窗口已关闭\n");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
