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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入内容后再发送");
            }
            else
            {
                //listBox1.Items.Add(DateTime.Now);
                listBox1.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择内容后再撤回");
            }
            else
            {
                
                for(int j=listBox1.SelectedItems.Count;j>0;j--){
                    listBox1.Items.Remove(listBox1.SelectedItems[j-1]);
                    
                }
        
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            listBox1.ScrollAlwaysVisible = true;
            listBox1.SelectionMode = SelectionMode.MultiExtended;
        }
    }
}
