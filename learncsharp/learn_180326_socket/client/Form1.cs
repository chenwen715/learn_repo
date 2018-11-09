using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket clientSocket;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipaddress, 2026);
            clientSocket.Connect(ipEndPoint);
            byte[] btContent = new byte[1024];
            btContent = Encoding.ASCII.GetBytes(textBox1.Text);
            clientSocket.Send(btContent,btContent.Length,0);
            if (richTextBox1.Text == "" || richTextBox1.Text == null)
            {
                richTextBox1.Text = textBox1.Text;
            }
            else
            {
                richTextBox1.Text += "\n"+textBox1.Text;
            }
            

            byte[] recvBytes = new byte[1024];
            int bytes;
            try
            {
                bytes =  clientSocket.Receive(recvBytes, 0, recvBytes.Length, 0);//从服务器端接受返回信息
               
                richTextBox1.Text+="\n"+Encoding.ASCII.GetString(recvBytes);
            }
            catch
            {
                 richTextBox1.Text+="\n接收失败";
            }
            finally
            {
                clientSocket.Close();
            }
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Empty;
        }
    }
}
