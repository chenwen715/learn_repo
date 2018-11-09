using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_181029_regularexpression
{
    public partial class Form1 : Form
    {

        DAL_Comn_Sql sql = new DAL_Comn_Sql();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            string selectpro = "SELECT DISTINCT province FROM dbo.T_CHINA ";
            DataSet ds = sql.SelectGet(Properties.Settings.Default.sql, selectpro);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                comboBox1.Items.Add(dr["province"]);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            string selectpro = string.Format("SELECT DISTINCT city FROM dbo.T_CHINA WHERE province = '{0}' ", comboBox1.Text);
            DataSet ds = sql.SelectGet(Properties.Settings.Default.sql, selectpro);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comboBox2.Items.Add(dr["city"]);
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Text = "";
            comboBox3.Items.Clear();
            string selectpro = string.Format("SELECT DISTINCT county FROM dbo.T_CHINA WHERE city = '{0}'AND province='{1}' ", comboBox2.Text,comboBox1.Text);
            DataSet ds = sql.SelectGet(Properties.Settings.Default.sql, selectpro);
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    comboBox3.Items.Add(dr["county"]);
                }
            }            
        }
    }
}
