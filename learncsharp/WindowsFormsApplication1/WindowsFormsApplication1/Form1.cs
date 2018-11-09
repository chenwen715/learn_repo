using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            byte a = byte.Parse(comboBox1.Text);
            if (a == 0)
            {
                byte[] data = new byte[100];
                data[0] = 0;
                data[1] = a;
                data[2] = byte.Parse(textBox1.Text);
                byte[] agvNo = Encoding.ASCII.GetBytes(textBox2.Text);
                for (int i = 3; i < 3 + agvNo.Length; i++)
                {
                    data[i] = agvNo[i - 3];
                }
                byte[] BarCode = Encoding.ASCII.GetBytes(textBox3.Text);
                data[13] = BarCode[0];
                data[14] = BarCode[1];
                data[15] = BarCode[2];
                data[16] = BarCode[3];
                byte[] Dv = Encoding.ASCII.GetBytes(textBox3.Text);
                data[17] = Dv[0];
                data[18] = Dv[1];

            }
            

        }

      
    }
}
