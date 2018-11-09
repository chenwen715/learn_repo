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
    public partial class Agv : Form
    {
        public Agv()
        {
            InitializeComponent();
        }

        Hashtable hs = new Hashtable();
        private void Agv_Load(object sender, EventArgs e)
        {
            string[] type = new string[] { "心跳", "申请动作完成", "申请路口","释放路口","申请小车左侧上料","小车上料完成",
                "申请小车左侧下料","小车下料完成","已开始充电","已结束充电","汇流口","工位点" };
            int[] num = new int[] { 0, 3, 1, 2, 3, 4, 5,6,7,8,1,2 };
            for (int i = 0; i < type.Length; i++)
            {
                hs.Add(type[i], num[i]);
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            panel1.Visible = false;
            panel2.Visible = false;
            textBox5.Text ="0";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = hs[comboBox1.Text].ToString();
            if(int.Parse(textBox1.Text)==0)
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox6.Text = hs[comboBox3.Text].ToString();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox9.Text = hs[comboBox4.Text].ToString();
        }

        public byte[] pinJieAgvBaoWen()
        {
            byte[] data = new byte[100];
            data[0] = 0;
            data[1] = Convert.ToByte(textBox1.Text);
            byte[] address = Encoding.ASCII.GetBytes(textBox2.Text);
            for (int i = 3; i < (3 + address.Length); i++)
            {
                data[i] = address[i - 3];
            }
            data[13] = data[14] = 0;
            if (Int16.Parse(textBox1.Text) == 0)
            {
                data[2] = 28;
                byte[] vol=Encoding.ASCII.GetBytes(textBox3.Text);
                for (int i = 15; i < (15 + vol.Length); i++)
                {
                    data[i] = vol[i-15];
                }
                byte[] state = Encoding.ASCII.GetBytes(textBox4.Text);
                data[19] = state[0];
                data[20] = state[1];
                data[21] = Convert.ToByte(comboBox2.Text);
                byte[] error= Encoding.ASCII.GetBytes(textBox5.Text);
                for (int i = 22; i < (22+ error.Length); i++)
                {
                    data[i] = error[i-22];
                }
                data[27] = CRC(data, 28);

            }
            else if (Int16.Parse(textBox3.Text) == 3)
            {
                data[2] = 69;
                data[15] = Convert.ToByte(textBox6.Text);
                data[16] = Convert.ToByte(textBox9.Text);
                data[17] = Convert.ToByte(textBox7.Text);
                byte[] taskNo = Encoding.ASCII.GetBytes(textBox8.Text);
                for (int i = 18; i < (18 + taskNo.Length); i++)
                {
                    data[i] = taskNo[i - 18];
                }
                data[68] = CRC(data, 69);
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
            byte[] agvMessage=pinJieAgvBaoWen();
        }
        
    }
}
