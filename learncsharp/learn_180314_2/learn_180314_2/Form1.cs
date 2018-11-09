using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180314_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.AcceptButton = button1;
           
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = textBox1.Text;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.Font = new Font(button1.Font, FontStyle.Bold);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.Font = new Font(button1.Font, FontStyle.Regular);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //richTextBox1.SelectionFont = new Font("楷体", 12, FontStyle.Bold);
            //richTextBox1.SelectionColor = Color.Blue;

            //richTextBox1.Text = "百度一下，你就知道：http://www.baidu.com";

            //实现首行缩进
            richTextBox1.SelectionIndent = 36;//9pt（点）=12px（像素），第一行与左边隔3个字的大小
            richTextBox1.SelectionRightIndent = 12;//每一行与右边隔1个字的大小
            richTextBox1.SelectionHangingIndent = -24;//第二行开始与第一行差2个字的大小

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            numericUpDown1.DecimalPlaces = 2;
           

        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectAll();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            
            label4.Text = "你选择了："+comboBox1.Text;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                MessageBox.Show("选中");
            }
            else
            {
                MessageBox.Show("未选中");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label5.Text = "数值为：" + numericUpDown1.Value;
        }

       
    }
}
