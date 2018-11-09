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

namespace learn_180317_3
{
    public partial class Form1 : Form

    {
        public Form1()
        {
            InitializeComponent();
            
        }

        Hashtable ht = new Hashtable();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            if(comboBox1.Items.Count!=0)
            {
                ht.Add("百度", "www.baidu.com");
            }
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ht.ContainsKey(comboBox1.Text) && ht[comboBox1.Text]!=null)
            {
                comboBox2.Text = ht[comboBox1.Text].ToString();
            }
            else if (!ht.ContainsKey(comboBox1.Text))
            {
                ht.Add(comboBox1.Text, null);
                comboBox2.Text = "";
            }
            else
            {
                comboBox2.Text = "";
                return;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                
                comboBox2.Items.Add(comboBox2.Text);
                ht.Add(comboBox1.Text, comboBox2.Text);
            
        }
    }
}
