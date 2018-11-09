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

namespace learn_180320
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Hashtable hs = new Hashtable();
        string index = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "0")
            {
                
                 byte[] b=new byte[Convert.ToInt16(comboBox3.Text)];
                 b[0] = 0;
                 b[1] = 0;
                 b[2] = 27;
                 byte[]  agvno = System.Text.Encoding.ASCII.GetBytes("A02_"+textBox2.Text.PadLeft(3, '0'));
                 for (int i = 3; i < (3+agvno.Length); i++)
                 {
                     b[i] = agvno[i - 3];
                 }
                 b[13] = b[14] = 0;    
                 byte[] vol = System.Text.Encoding.ASCII.GetBytes(textBox3.Text);
                 for (int i = 15; i < (15+vol.Length); i++)
                 {
                     b[i] = vol[i - 15];
                 }
                 byte[] state = System.Text.Encoding.ASCII.GetBytes(comboBox4.Text);
                 b[19] = state[0];
                 b[20] = state[1];
                 b[21] = System.Text.Encoding.ASCII.GetBytes(comboBox2.Text)[0];
                 byte[] erroeCode = System.Text.Encoding.ASCII.GetBytes(textBox5.Text);
                 for (int i = 22; i < (22+erroeCode.Length); i++)
                 {
                     b[i] = vol[i - 22];
                 }
                 b[27] = CRC(b,28);                
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int[] key1=new int[]{0,1,2,3,4,5,8,9,10,11};
            int[] value1 =new int[]{28,68,16,69,19,16,26,17,16,18};
            
            for (int i = 0; i < 10; i++)
            {
                hs.Add(key1[i], value1[i]);
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = Int16.Parse(comboBox1.Text);
            comboBox3.Text = hs[a].ToString();
        }
    }
}
