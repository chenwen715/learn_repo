using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace StkTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<String> a = new List<string>();
            string index = "FCFC";
            if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("STK编号不能为空！");
                return;
            }
            else { 
                index += Convert.ToString(int.Parse(comboBox1.Text), 16).PadLeft(4, '0');
                index += Convert.ToString(int.Parse(textBox3.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(60, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox6.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox5.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox4.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox3.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox2.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox12.Text), 16).PadLeft(4, '0'); //69为被转值
                index += StrToHex(textBox1.Text).PadLeft(40,'0');
                index += Convert.ToString(int.Parse(comboBox11.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox8.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox9.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(int.Parse(comboBox10.Text), 16).PadLeft(4, '0'); //69为被转值
                index += Convert.ToString(0, 16).PadLeft(16, '0'); //69为被转值
                index += "FDFD";
            }
            textBox2.Text = index;
        }
        public string HexToStr(string mHex) // 返回十六进制代表的字符串
        {
            mHex = mHex.Replace(" ", "");
            if (mHex.Length <= 0) return "";
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return ASCIIEncoding.Default.GetString(vBytes);
        }
        public string StrToHex(string mStr) //返回处理后的十六进制字符串
        {
            return BitConverter.ToString(
            ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", "");
        }
    }
}
