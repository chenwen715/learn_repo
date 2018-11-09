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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Empty;
            WebClient wClient = new WebClient();
            wClient.BaseAddress = textBox1.Text;
            wClient.Encoding = Encoding.UTF8;
            wClient.Headers.Add("Content-Type","application/x-www-form-urlencoded");
            Stream stream = wClient.OpenRead(textBox1.Text);
            StreamReader sReader = new StreamReader(stream);
            string str = String.Empty;
            while((str=sReader.ReadLine())!=null)
            {
                richTextBox1.Text += str + "\n";
            }
            
            wClient.DownloadFile(textBox1.Text, DateTime.Now.ToFileTime() + ".txt");
            MessageBox.Show("保存成功");
        }
    }
}
