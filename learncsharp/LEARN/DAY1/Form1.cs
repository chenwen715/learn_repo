using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAY1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("please input the host address!!!");
            }
            else
            {
                textBox2.Text = textBox3.Text = textBox4.Text = "";
                IPAddress[] ips = Dns.GetHostAddresses(textBox1.Text);
                foreach(IPAddress ip in ips){
                     textBox2.Text = ip.ToString();
                }
               
                textBox3.Text = Dns.GetHostName();
                textBox4.Text = Dns.GetHostEntry(Dns.GetHostName()).HostName;
            }
        }

       
    }
}
