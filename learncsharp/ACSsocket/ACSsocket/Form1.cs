using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACSsocket
{
    public partial class Form1 : Form
    {
        public static Form1 f = null;
        public Form1()
        {
            InitializeComponent();
            f = this;
        }

        public static rTypeEnum responseType = 0;
        public static Boolean flag = true;//true允许通讯，false不允许通讯
        public static string sid = "";
        private Thread thSocket;
        private Socket SvrSocket;
        private void Form1_Load(object sender, EventArgs e)
        {
            //和AGV通讯
            thSocket = new Thread(SocketBuild);
            thSocket.IsBackground = true;
            thSocket.Start();
        }

        private void SocketBuild()
        {
            if (SvrSocket != null)
            {
                SvrSocket.Close();
                SvrSocket = null;
            }
            try
            {
                IPAddress ip = IPAddress.Parse(Properties.Settings.Default.IP);
                IPEndPoint ipe = new IPEndPoint(ip, Properties.Settings.Default.port);
                SvrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SvrSocket.Bind(ipe);

                TCP.ServerListen(Properties.Settings.Default.ContactNum, SvrSocket);

            }
            catch (Exception Ex)
            {
                Log.MessageError("SocketBuild", "创建AGV监听线程失败：" + Ex.ToString());
                //throw Ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                responseType = rTypeEnum.Repeat;
            }
            else if (radioButton3.Checked) 
            {
                responseType = rTypeEnum.Finish;
            }
            else if (radioButton4.Checked)
            {
                responseType = rTypeEnum.NewAction;
                radioButton1.Checked = true;
            }
            else
            {
                responseType = 0;
                showMessage();
            }            
        }

        public static void showMessage()
        {
            MessageBox.Show("请选择回复报文类型");
            flag = false;
        }

        delegate void SetTextCallBack(string text);
        public static void SetText(string text)
        {
            if (f.richTextBox2.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                f.Invoke(stcb, new object[] { text });
            }
            else
            {
               f.richTextBox2.Text += text;
            }
        }

        delegate void SetTextCallBack1(string text);
        public static void SetText1(string text)
        {
            if (f.richTextBox1.InvokeRequired)
            {
                SetTextCallBack1 stcb = new SetTextCallBack1(SetText1);
                f.Invoke(stcb, new object[] { text });
            }
            else
            {
                f.richTextBox1.Text += text;
            }
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();

        } 
    }
}
