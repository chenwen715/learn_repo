using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180330_Graphics_1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            int sum = Method.ConnectSql("SELECT sum(Count) FROM dbo.T_Constellation");
            label1.Text += "（共" + sum + "票）";
         
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            //DrawRectangle();
            DrawPie();
           

        }

        private void DrawRectangle()
        {
            pictureBox1.Visible = false;
            Graphics gra = this.CreateGraphics();
            string[] colstr = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "天秤座", "处女座",
            "天蝎座","射手座", "摩羯座", "水瓶座",  "双鱼座" };
            Color[] color = new Color[] { Color.LightBlue, Color.Yellow,Color.Green,Color.HotPink,Color.LightYellow,Color.LightSkyBlue,Color.LightGreen,
            Color.LightGray,Color.LightSeaGreen,Color.LemonChiffon,Color.YellowGreen,Color.Red};
            try
            {
                Font font = new Font("宋体", 10, FontStyle.Regular);
                Font font1 = new Font("TIMES NEW ROMAN", 8, FontStyle.Regular);
                for (int i = 0; i < colstr.Length; i++)
                {
                    gra.DrawString(colstr[i], font, new SolidBrush(Color.Black), new PointF(33, 40 * (i + 2)));
                }
                int sum = Method.ConnectSql("SELECT sum(Count) FROM dbo.T_Constellation");

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
                            gra.DrawString("(" + Math.Round((double)(count * 100) / sum, 2) + "%)", font1, new SolidBrush(Color.Black), new PointF(75, 40 * (j + 2)));
                            Rectangle re = new Rectangle(130, 40 * (j + 2), (count * 200 / sum), 12);
                            SolidBrush sBrush = new SolidBrush(color[j]);
                            Pen pen = new Pen(sBrush, 2);
                            gra.DrawRectangle(pen, re);
                            gra.FillRectangle(sBrush, re);
                            gra.DrawString(count.ToString(), font, new SolidBrush(Color.Black), new PointF(130, 40 * (j + 2)));
                            break;
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void DrawPie()
        {
            pictureBox1.Visible =false;
            //Bitmap bmp2 = new Bitmap(300, 600);
            //Graphics gra = Graphics.FromImage(bmp2);
            Graphics gra = this.CreateGraphics();
            string[] colstr = new string[] { "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "天秤座", "处女座",
            "天蝎座","射手座", "摩羯座", "水瓶座",  "双鱼座" };
            Color[] color = new Color[] { Color.LightBlue, Color.Yellow,Color.Green,Color.HotPink,Color.LightYellow,Color.LightSkyBlue,Color.LightGreen,
            Color.LightGray,Color.LightSeaGreen,Color.LemonChiffon,Color.YellowGreen,Color.Red};
            try
            {
                Font font = new Font("宋体", 10, FontStyle.Regular);
                Font font1 = new Font("TIMES NEW ROMAN", 9, FontStyle.Regular);
                //for (int i = 0; i < colstr.Length; i++)
                //{
                //    gra.DrawString(colstr[i], font, new SolidBrush(Color.Black), new PointF(33, 40 * (i + 2)));
                //}
                int sum = Method.ConnectSql("SELECT sum(Count) FROM dbo.T_Constellation");
                float angle = 0;
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
                            //gra.DrawString("(" + Math.Round((double)(count * 100) / sum, 2) + "%)", font1, new SolidBrush(Color.Black), new PointF(75, 40 * (j + 2)));
                            //Rectangle re = new Rectangle(130, 40 * (j + 2), (count * 200 / sum), 12);
                            //SolidBrush sBrush = new SolidBrush(color[j]);
                            //Pen pen = new Pen(sBrush, 2);
                            //gra.DrawRectangle(pen, re);
                            //gra.FillRectangle(sBrush, re);
                            //gra.DrawString(count.ToString(), font, new SolidBrush(Color.Black), new PointF(130, 40 * (j + 2)));
                            gra.FillPie(new SolidBrush(color[j]), 36, 100, 240,240, angle, (float)(count * 360) / sum);
                            angle += (float)(count * 360) / sum;
                            
                            if (j < 6)
                            {
                                gra.FillRectangle(new SolidBrush(color[j]), 36, 400+20*j, 10, 10);
                                gra.DrawString(colstr1 + "(" + count + "票)" + Math.Round((double)(count * 100) / sum, 2) + "%", font1, new SolidBrush(Color.Black), new PointF(50, 400 + 20 * j));
                            }
                            else
                            {
                                gra.FillRectangle(new SolidBrush(color[j]), 180, 400 + 20 * (j-6), 10, 10);
                                gra.DrawString(colstr1 + "(" + count + "票)" + Math.Round((double)(count * 100) / sum, 2) + "%", font1, new SolidBrush(Color.Black), new PointF(194, 400 + 20 * (j - 6)));
                            }
                            break;
                            
                        }
                        
                    }
                }
                //pictureBox1.Image = bmp2;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}

