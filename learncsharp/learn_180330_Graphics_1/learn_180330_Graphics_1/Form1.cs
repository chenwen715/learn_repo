using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180330_Graphics_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ArrayList array = new ArrayList();
        private void Form1_Load(object sender, EventArgs e)
        {
            array.Add(radioButton1);
            array.Add(radioButton2);
            array.Add(radioButton3);
            array.Add(radioButton4);
            array.Add(radioButton5);
            array.Add(radioButton6);
            array.Add(radioButton7);
            array.Add(radioButton8);
            array.Add(radioButton9);
            array.Add(radioButton10);
            array.Add(radioButton11);
            array.Add(radioButton12);
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string datastr = "server=.;database=Vote;uid=sa;pwd=abc123*";
            SqlConnection sql = new SqlConnection(datastr);
            sql.Open();
            int result, affectLines;
            for (int i = 0; i < array.Count; i++)
            {
                RadioButton rd = array[i] as RadioButton;

                if (rd.Checked)
                {
                    try
                    {
                        SqlCommand sCommand = new SqlCommand("select count from T_Constellation where Constellation='" + rd.Text+"'", sql);
                        result = int.Parse(sCommand.ExecuteScalar().ToString());
                        result += 1;
                        SqlCommand sCommand1 = new SqlCommand("update T_Constellation set count=" + result + " where Constellation='" + rd.Text + "'", sql);
                        affectLines = sCommand1.ExecuteNonQuery();
                        if (affectLines == 1)
                        {
                            MessageBox.Show("投票成功！\n\n" + "你已选择" + rd.Text);
                        }

                        return;
                    }
                    catch (Exception ex)
                    {
                       throw new Exception(ex.ToString());
                    }
                    finally
                    {
                        sql.Close();
                    }
                   
                }
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 frm = new Form3();
            frm.Show();
        }
    }
}
