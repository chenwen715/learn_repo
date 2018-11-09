using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180323_wsmessage
{
    public partial class Station : Form
    {
        public Station()
        {
            InitializeComponent();
        }

        Hashtable hs = new Hashtable();
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] type = new string[] { "心跳", "申请或动作完成", "完工信号（呼叫）", "上料辊道已经动作", 
                "下料辊道已经动作", "工位上料任务完成", "工位下料任务完成" };
            int[] num = new int[] { 8, 11, 1, 2, 3, 4, 5 };
            for (int i = 0; i < type.Length; i++)
            {
                hs.Add(type[i], num[i]);
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            panel1.Visible = false;
            panel2.Visible = false;
            textBox2.Text = "0";
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = hs[comboBox1.Text].ToString();
            if (Int16.Parse(textBox3.Text) == 8)
            {
                panel1.Show();
                panel2.Visible = false;
            }
            else
            {
                panel2.Show();
                panel1.Visible = false;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = hs[comboBox6.Text].ToString();
        }

        public byte[] pinJieStationBaoWen()
        {
            byte[] data = new byte[100];
            data[0] = 0;
            data[1] = Convert.ToByte(textBox3.Text);
            data[13] = data[14] = 0;
            if (Int16.Parse(textBox3.Text) == 8)
            {
                data[2] = 26;
                data[15] = Convert.ToByte(textBox1.Text);
                data[16] = Convert.ToByte(comboBox2.Text);
                data[17] = Convert.ToByte(comboBox3.Text);
                data[18] = Convert.ToByte(comboBox4.Text);
                data[19] = Convert.ToByte(comboBox5.Text);
                byte[] error = Encoding.ASCII.GetBytes(textBox3.Text);
                for (int i = 20; i < (20 + error.Length); i++)
                {
                    data[i] = error[i-20];
                }
                data[25] = CRC(data,26);

            }
            else if(Int16.Parse(textBox3.Text) == 11)
            {
                data[2] = 18;
                data[15]=Convert.ToByte(textBox4.Text);
                data[16]=Convert.ToByte(textBox5.Text);
                data[17]=CRC(data,18);
            }
            return data;
        }

        public byte CRC(byte[] byt, int Length)
        {
            byte crc = 0;
            crc = (byte)(byt[0] ^ byt[1]);
            for (int i = 2; i < Length - 1; i++)
            {
                crc = (byte)(crc ^ byt[i]);

            }
            return crc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] stationMessage = pinJieStationBaoWen();
        }
    }
}
