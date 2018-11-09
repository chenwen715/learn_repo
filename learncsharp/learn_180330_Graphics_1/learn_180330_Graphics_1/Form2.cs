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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string[] colstr = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "天秤座", "处女座",
            "天蝎座","射手座", "摩羯座", "水瓶座",  "双鱼座" };
        ArrayList labelArray;
        private void Form2_Load(object sender, EventArgs e)
        {
            Graphics gra = this.CreateGraphics();
            try
            {
                labelArray = new ArrayList() { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label12 };
                Font font=new Font("宋体",16,FontStyle.Bold);
                for (int i = 0; i < labelArray.Count; i++)
                {
                    //Label temLabel = labelArray[i] as Label;
                    //temLabel.Location = new Point(33, (30 * (i + 2) + 3));
                    //temLabel.Text = colstr[i];
                    gra.DrawString(colstr[i], font, new SolidBrush(Color.Black), new PointF(33, (30 * (i + 2) + 3)));
                }


                int sum = Method.ConnectSql("SELECT COUNT(1) FROM dbo.T_Constellation");
               
                DataSet ds = Method.GetData("select * from T_Constellation");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    string colstr1 = dr["Constellation"].ToString();
                    int count = int.Parse(dr["Count"].ToString());
                    for (int j = 0; j < colstr.Length; j++)
                    {
                        if (colstr1 == colstr[j])
                        {
                            Rectangle re = new Rectangle(110, (30 * (j + 2) + 3), (count * 1000 / sum), 12);
                            SolidBrush sBrush = new SolidBrush(Color.Blue);
                            Pen pen = new Pen(sBrush, 2);
                            gra.DrawRectangle(pen, re);
                            gra.FillRectangle(sBrush, re);
                            //return;
                        }
                    }
                }
   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
 
        }
    }
}
