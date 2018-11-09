using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180402_Graphics_2
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
            Pen pen = new Pen(Color.Black, 2);
            int a=20;
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    //double y = a * Math.Round(Math.Sin(i * 90), 2);
                    g.DrawArc(pen, new Rectangle(a * i + 20, a, 20, 2 * a), 180, 180);
                }
                else
                {
                    g.DrawArc(pen, new Rectangle(a * i + 20, a, 20, 2 * a), 0, 180);
                }
               
            }
        }
    }
}
