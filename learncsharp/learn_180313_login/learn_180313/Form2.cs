using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180313
{
    public partial class Form2 : Form  //登录
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("用户名不能为空");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            
            string conString = "server=.;database=XueXi;uid=sa;pwd=abc123*";
            SqlConnection conn = new SqlConnection(conString);
            conn.Open();
            //if (conn.State == ConnectionState.Open)
            //{
            //    MessageBox.Show("连接成功");
            //}
            SqlCommand cmd = new SqlCommand("select * from T_User", conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            SqlCommandBuilder scb = new SqlCommandBuilder(sda);
            int i=0;
            if (ds.Tables[0].Rows.Count==0)
            {
                MessageBox.Show("用户名不存在");
                textBox1.Text = textBox2.Text = "";
                conn.Close();
                return;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               
                if (dr["Name"].ToString() == textBox1.Text)
                {
                    if (dr["Password"].ToString() == textBox2.Text)
                    {
                        MessageBox.Show("登录成功");
                        this.Hide();
                        Form1 frm = new Form1();
                        frm.Show();
                       
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误，请重新输入");
                        textBox1.Text = textBox2.Text = "";
                    }
                    
                    return;
                }
                i++;
                if (i == ds.Tables[0].Rows.Count)
                {
                    MessageBox.Show("用户名不存在");
                    textBox1.Text = textBox2.Text = "";
                }
            }
            
       
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 frm = new Form5();
            frm.Show();

        }

        

    }
}
