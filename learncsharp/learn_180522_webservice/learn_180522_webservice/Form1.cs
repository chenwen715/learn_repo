using com.force.json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180522_webservice
{
    public partial class Form1 : Form
    {

        DAL_Comn_Sql comm = new DAL_Comn_Sql();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string m="TaskNo171127000004";
            //string s = string.Format("[{{\"taskNo\":\"{0}\"",m);
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请输入任务条数");
                return;
            }
            string number = "";
            string date=DateTime.Now.Year.ToString().PadLeft(4,'0')+DateTime.Now.Month.ToString().PadLeft(2,'0')+DateTime.Now.Day.ToString().PadLeft(2,'0');
            string sql = string.Format(@"SELECT TOP 1 TaskNo FROM dbo.T_Task_Load WHERE TaskNo LIKE '%{0}%' ORDER BY TaskNo DESC",date);
            int[] weight={300,500,100};
            int[] height={1299,1800,1300};
            Random r = new Random();
            
            string taskno = "";
            if(comm.GetData(Properties.Settings.Default.sql, sql)!=null)
            {
                taskno = comm.GetData(Properties.Settings.Default.sql, sql).ToString(); 
            }
                                
            for (int i = 0; i < int.Parse(textBox1.Text); i++)
            {
                if (!string.IsNullOrEmpty(taskno))
                {
                    number = date + (int.Parse(taskno.Substring(taskno.Length - 4, 4)) + i + 1).ToString().PadLeft(4,'0');
                }
                else
                {
                    number = date + ((i + 1).ToString().PadLeft(4, '0'));

                }
                int a = r.Next(0, weight.Length);
                int b = r.Next(0, height.Length);
                string TaskNo = "TaskNo" + number;
                string BoxOrPalletNo = "PX" + number;
                string BDNO = "BDNO" + number;
                string pn = "pn" + number;
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
""opTime"":""{5}""}}]", TaskNo, BoxOrPalletNo, "[\"\"]", 0, -1, DateTime.Now.ToString(), BDNO, pn, weight[a], height[b]);

                JSONArray jsarray = new JSONArray(str);
                string url = "http://127.0.0.1:8889/api/loadTasks";
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

                    richTextBox1.Text += TaskNo+"=="+resp+"\n";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.Replace(Environment.NewLine, ""));
                }
            }
            MessageBox.Show("下发" + textBox1.Text + "条上架任务成功");
            textBox1.Text = "";
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            string str = "[";
            for (int i = 0; i < 3; i++)
            {
                string barcode = "20180522" + i;
                string warecell = "aa_000" + i;
                str += string.Format(@"{{""BarCode"":{0},""WareCell"":""{1}""}},", barcode, warecell);
            }
            str += "]";
            string result = client.LoadTasksResult(str);
            richTextBox1.Text += result;

        }
    }
}
