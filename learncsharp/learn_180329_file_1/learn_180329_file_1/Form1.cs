using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180329_file_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = string.Empty;
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                if (fs.Length == 0)
                {
                    richTextBox2.Text = "文件中无内容";
                    fs.Close();
                }
                else
                {
                    StreamReader sReader = new StreamReader(fs);
                    richTextBox2.Text = sReader.ReadToEnd();
                    sReader.Close();
                    fs.Close();

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || richTextBox1.Text == null)
            {
                MessageBox.Show("请输入要写入的内容");
                return;
            }
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Append, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(fs);
                sWriter.WriteLine(richTextBox1.Text);
                sWriter.Close();
                fs.Close();
                richTextBox1.Text = "";
            }
        }
    }
}
