using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180317
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入内容");
            }
            else
            {
                listView1.Items.Add(textBox1.Text.Trim());
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要移除的项");
            }
            else
            {
                for (int i= listView1.SelectedItems.Count; i >0 ; i--)
                {
                    listView1.Items.Remove(listView1.SelectedItems[i-1]);
                }
            }
        }
    }
}
