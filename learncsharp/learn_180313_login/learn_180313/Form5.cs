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
    public partial class Form5 : Form //注册
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("用户名不能为空");
                textBox2.Text = textBox3.Text="";
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("密码不能为空");
                textBox3.Text = "";
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("确认密码不能为空");
            }
            else if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("两次密码不一致请重新输入");
                textBox2.Text = textBox3.Text = "";
            }
            else
            {
                SqlConnection sql = new SqlConnection("server=.;database=XueXi;uid=sa;pwd=abc123*");
                sql.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("select * from T_User", sql);
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == textBox1.Text)
                    {
                        MessageBox.Show("已存在该用户名");
                        textBox1.Text = textBox2.Text = textBox3.Text = "";
                        return;
                    }
                }
                string insertstr = "insert into dbo.T_User (Name,Password) values (\'" + textBox1.Text + "\', \'" + textBox2.Text + "\')";
                SqlCommand cmd1 = new SqlCommand(insertstr, sql);
                int j = cmd1.ExecuteNonQuery();
                
                if (j == 1)
                {
                    DialogResult dialogr=MessageBox.Show("注册成功，点击确定进入登录页面","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    if (dialogr == DialogResult.OK)
                    {
                        this.Hide();
                        Form7 frm = new Form7();
                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("失败，请重新注册");
                    textBox1.Text = textBox2.Text = textBox3.Text = "";
                }
                sql.Close();
            }
        }
    }
}
