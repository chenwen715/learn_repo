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

namespace learn_180326_web
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Empty;
            WebRequest webRequest = WebRequest.Create(textBox1.Text);
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            richTextBox1.Text = "请求数据内容长度：" + webRequest.ContentLength;
            richTextBox1.Text += "\n请求的协议方法：" + webRequest.Method;
            richTextBox1.Text += "\n访问Internet的网络代理：" + webRequest.Proxy;
            richTextBox1.Text += "\n与该请求关联的Internet URI：" + webRequest.RequestUri;
            richTextBox1.Text += "\n超时时间：" + webRequest.Timeout;
            WebResponse webResponse = webRequest.GetResponse();
            richTextBox1.Text += "\n响应该请求的Internet URI：" + webResponse.ResponseUri;
            Stream stream = webResponse.GetResponseStream();
            StreamReader sReader = new StreamReader(stream);
            richTextBox1.Text += "\n" + sReader.ReadToEnd();
            sReader.Close();
            stream.Close();
            webResponse.Close();
        }
    }
}
