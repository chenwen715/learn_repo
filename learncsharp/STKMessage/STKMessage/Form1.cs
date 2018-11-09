using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STKMessage
{
    public partial class Form1 : Form//友星STK报文拼接
    {
        public Form1()
        {
            InitializeComponent();
        }

        Hashtable hs = new Hashtable();
        Hashtable errorhs = new Hashtable();
        ArrayList error = new ArrayList();
        ArrayList errorname = new ArrayList();
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] type = new string[] { "状态返回", "新任务ACK", "任务清空ACK", "任务完成ACK", "当前无任务", "接收到任务", "开始执行任务",
                "到达取料位置", "执行取料任务", "取料动作完成", "到达放料位置","执行放料任务","放料动作完成","到取料位置" ,"到位置取货","到位置放货","先取货后放货"};
            int[] num = new int[] { 1,2,4,5,0, 1,2,3,4,5,6,7,8,1,2,3,4};
            for (int i = 0; i < type.Length; i++)
            {
                hs.Add(type[i], num[i]);
            }
            
            comboBox3.SelectedIndex = 1;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox10.SelectedIndex = 0;
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";
            textBox8.Text = "0";
            textBox9.Text = "0";
            textBox10.Text = "0";
            textBox11.Text = "0";
            textBox12.Text = "0";
            textBox13.Text = "0";
            textBox14.Text = "0";
            textBox15.Text = "0";
            textBox16.Text = "0";
            textBox17.Text = "0";
            textBox18.Text = "0";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox7.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox8.DropDownStyle = ComboBoxStyle.DropDownList;
        
        }

  
        private void button1_Click(object sender, EventArgs e)
        {
            error = new ArrayList() { textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text, textBox11.Text,
                textBox12.Text, textBox13.Text, textBox14.Text,textBox15.Text,textBox16.Text,textBox17.Text,textBox18.Text};
            errorname = new ArrayList() { label16.Text, label17.Text, label18.Text, label19.Text, label20.Text, label21.Text, label22.Text,
                label23.Text, label24.Text, label25.Text, label26.Text, label27.Text, label28.Text, label29.Text };
            for (int j = 0; j < 14; j++)
            {
                errorhs.Add(errorname[j], error[j]);
            }
            
            string index = "FCFC";
            if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("报文类型不能为空！");
                return;
            }
            else if (textBox2.Text == null || textBox2.Text == "")
            {
                MessageBox.Show("STK编号不能为空！");
                return;
            }
            else if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("条码不能为空！");
                return;
            }
            else
            {
                for (int i = 0; i < 14; i++)
                {
                    if (int.Parse(error[i].ToString()) > 255 || int.Parse(error[i].ToString()) < 0)
                    {
                        MessageBox.Show(errorname[i].ToString()+"请输入0-255的数");
                        for (int j = 0; j < 14; j++)
                        {
                            errorhs.Remove(errorname[j]);
                        }
                        textBox4.Text = "";
                        return;
                    }
                }
                index += Convert.ToString(int.Parse(textBox1.Text), 16).PadLeft(4, '0');
                index += Convert.ToString(int.Parse(textBox2.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(84, 16).PadLeft(4, '0'); 
                if (int.Parse(textBox1.Text) != 1)
                {
                    index += Convert.ToString(0, 16).PadLeft(4, '0');
                }
                else
                {
                    index += Convert.ToString(int.Parse(comboBox2.Text), 16).PadLeft(4, '0');
                }
                 
                index += Convert.ToString(int.Parse(comboBox3.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox4.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox5.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox6.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(textBox19.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(textBox20.Text), 16).PadLeft(4, '0'); 
                index += StrToHex(textBox3.Text).PadLeft(40, '0');
                index += Convert.ToString(int.Parse(comboBox9.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox10.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox11.Text), 16).PadLeft(4, '0'); 
                index += Convert.ToString(int.Parse(comboBox12.Text), 16).PadLeft(4, '0'); 
                if (int.Parse(textBox1.Text) != 1)
                {
                    index += Convert.ToString(0, 16).PadLeft(4, '0');
                }
                else
                {
                    index += Convert.ToString(int.Parse(comboBox13.Text), 16).PadLeft(4, '0');
                }
                index += Convert.ToString(20, 16).PadLeft(4, '0');
                index += Convert.ToString(0, 16).PadLeft(28, '0'); 
                index += Convert.ToString(int.Parse(textBox5.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox6.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox7.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox8.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox9.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox10.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox11.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox12.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox13.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox14.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox15.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox16.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox17.Text), 16).PadLeft(2, '0');
                index += Convert.ToString(int.Parse(textBox18.Text), 16).PadLeft(2, '0');
                index += "FDFD";
            }
            //textBox4.Text = index;
            string bytestring = "[";
            byte[] message = HexStringToBytes(index);
            for (int i = 0; i < message.Length; i++)
            {
                if (i != (message.Length - 1))
                {
                    bytestring += message[i] + ",";
                }
                else
                {
                    bytestring += message[i];
                }
                 

            }
            bytestring += "]";
             textBox21.Text = bytestring;
             textBox4.Text = "["+index+"]";
            for (int j = 0; j < 14; j++)
            {
                errorhs.Remove(errorname[j]);
            }
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
            //return BitConverter.ToString(
            //ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", "");
            byte[] b = ASCIIEncoding.Default.GetBytes(mStr);
            string m=BitConverter.ToString(b).Replace("-", "");
            return m;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox19.Text = hs[comboBox7.Text].ToString();
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox20.Text = hs[comboBox8.Text].ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = hs[comboBox1.Text].ToString();
            if (int.Parse(textBox1.Text) != 1)
            {

            }
        }

        public static String byteToBit(byte b)
        {
            return ""
                    + (byte)((b >> 7) & 0x1) + (byte)((b >> 6) & 0x1)
                    + (byte)((b >> 5) & 0x1) + (byte)((b >> 4) & 0x1)
                    + (byte)((b >> 3) & 0x1) + (byte)((b >> 2) & 0x1)
                    + (byte)((b >> 1) & 0x1) + (byte)((b >> 0) & 0x1);
        }

        public static byte[] HexStringToBytes(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return new byte[0];
            }

            if (hexStr.StartsWith("0x"))
            {
                hexStr = hexStr.Remove(0, 2);
            }

            var count = hexStr.Length;

            if (count % 2 == 1)
            {
                throw new ArgumentException("Invalid length of bytes:" + count);
            }

            var byteCount = count / 2;
            var result = new byte[byteCount];
            for (int index = 0; index < byteCount; ++index)
            {
                var tempBytes = Byte.Parse(hexStr.Substring(2 * index, 2), System.Globalization.NumberStyles.HexNumber);
                result[index] = tempBytes;
            }

            return result;
        }

       
    }
}
