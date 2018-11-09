using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180330_Graphics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Blue, 2);
            Rectangle re = new Rectangle(10, 10, 100, 100);
            g.DrawRectangle(p, re);
            SolidBrush sBrush = new SolidBrush(Color.Red);
            g.FillRectangle(sBrush, re);
            g.DrawEllipse(p, re);
            SolidBrush sBrush1 = new SolidBrush(Color.Yellow);
            g.FillEllipse(sBrush1, re);
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g1 = this.CreateGraphics();
            for (int i = 0; i < 6; i++)
            {
                HatchStyle hs = (HatchStyle)5 + i;
                HatchBrush hb = new HatchBrush(hs,Color.Blue);
                Rectangle re = new Rectangle(10, 30* (i + 5), 30 * (i +5), 50);
                g1.FillRectangle(hb,re);
            }
            g1.Dispose();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g2 = this.CreateGraphics();
            Point p1 = new Point(10, 200);
            Point p2 = new Point(100, 290);
            LinearGradientBrush lgb = new LinearGradientBrush(p1, p2, Color.Yellow, Color.Blue);
            Rectangle re = new Rectangle(10, 200, 200, 200);
            lgb.WrapMode = WrapMode.TileFlipY;
            g2.FillRectangle(lgb, re);
            g2.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Point p1 = new Point(200, 10);
            Point p2 = new Point(300, 20);
            Pen pen = new Pen(Color.Green);
            Graphics g = this.CreateGraphics();
            g.DrawLine(pen, p1, p2);
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Rectangle re = new Rectangle(110, (30 * (0 + 2) + 3), (1 * 1000 / 12), 12);
            SolidBrush sBrush = new SolidBrush(Color.Blue);
            Pen pen = new Pen(sBrush, 2);
            Graphics gra = this.CreateGraphics();
            gra.DrawRectangle(pen, re);
            gra.FillRectangle(sBrush,re);

        }

      
    }
}
