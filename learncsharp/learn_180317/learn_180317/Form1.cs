using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180317
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Text = "Debug";
            toolStripStatusLabel1.Text=DateTime.Now.ToString();
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.ControlAlign = ContentAlignment.MiddleLeft;
            timer1.Enabled = false;
            label1.Visible = false;
            imageList1.ImageSize = new Size(240,150);
            string path1 = "C:\\Users\\Public\\Pictures\\Sample Pictures\\k.jpg";
            try
            {
                Image img1 = Image.FromFile(path1,true);
                imageList1.Images.Add(img1);
            }
            catch
            {
                throw new Exception("wrong");
            }
            string path = "C:\\Users\\Public\\Pictures\\Sample Pictures\\k.jpg";
            Image img = Image.FromFile(path, true);
            imageList2.Images.Add(img);
            imageList2.ImageSize = new Size(200, 120);

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum =100;
            timer1.Enabled = true;
            label1.Text = "加载中...";
            label1.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(toolStripProgressBar1.Value < toolStripProgressBar1.Maximum)
            {
                toolStripProgressBar1.Value++;
                label1.Text = "加载中" + toolStripProgressBar1.Value + "/" + toolStripProgressBar1.Maximum;
            }
            else
            {
                timer1.Enabled = false;
                label1.Text = "加载完成";
                toolStripProgressBar1.Visible = false;

            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[1];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[2];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (imageList2.Images.Count == 0)
            {
                MessageBox.Show("没有图像");
            }
            else
            {
                pictureBox2.Image = imageList2.Images[0];
            }

         
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //if (imageList2.Images.Count == 0)
            //{
            //    MessageBox.Show("没有图像");
            //}
            //else
            //{
            //    imageList2.Images.RemoveAt(0);

            //}
            if (pictureBox2.Image != null)
            {
                if (MessageBox.Show("是否删除", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pictureBox2.Image = null;
                    imageList2.Images.RemoveAt(0);
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("没有图片");
            }
            
        }
    }
}
