using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180316
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            richTextBox1.Text = "巨蟹座 6.22-7.23";
            groupBox1.Text = "诗词";
            richTextBox2.SelectionColor = Color.Blue;
            tabControl1.ImageList = imageList1;
            tabPage1.ImageIndex = 0;
            tabPage1.Text = "one";
            tabPage2.ImageIndex = 1;
            tabPage2.Text = "two";
            Button btn = new Button();
            btn.Text = "新增";
            tabPage1.Controls.Add(btn);
            TabPage tp = new TabPage("3");
            tabControl1.TabPages.Add(tp);
            tp.ImageIndex = 0;
            tp.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入内容");
            }
            else if (textBox1.Text == "巨蟹座")
            {
                panel1.Show();
            }
            else
            {
                MessageBox.Show("sorry");
                textBox1.Text = "";
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)  //wrong，第一个删不掉
            //{
            //    MessageBox.Show("please select a tabpage");
            //}
            //else
            //{
            //    tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            //}
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
        }
    }
}
