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

namespace ACSsocket
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public static List<Motion> motionL = new List<Motion>();
        Hashtable t = new Hashtable();
        private void Form2_Load(object sender, EventArgs e)
        {
            string[] dtype = {"行走","顶升","下降","直接顶升","直接下降","充电","取消充电","原地旋转","左弧","右弧","旋转左弧出","旋转右弧出"};
            int dt = 1;
            foreach (string d in dtype)
            {
                t.Add(d, dt);
                dt++;
            }
            motionL.Clear();
            textBox6.Text = Form1.sid;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                textBox1.Text = "";
            }
            else
            {
                textBox1.Text = t[comboBox1.Text].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("请输入任务号");
            }
            else if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请选择动作类型");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("请输入码值1");
            }
            else if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("请输入码值2/3/4和其他数据\n若不输入则默认填入0");
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    textBox3.Text = "0";
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                }
                else if (string.IsNullOrEmpty(textBox4.Text))
                {
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                }
                else
                {
                    textBox5.Text = "0";
                }
                string[] tx = { textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text, textBox11.Text, textBox12.Text, textBox13.Text, textBox14.Text };
                for (int k = 0; k < tx.Length; k++)
                {
                    if (string.IsNullOrEmpty(tx[k]))
                    {
                        switch (k)
                        {
                            case 0:
                                textBox7.Text = "0";
                                break;
                            case 1:
                                textBox8.Text = "0";
                                break;
                            case 2:
                                textBox9.Text = "0";
                                break;
                            case 3:
                                textBox10.Text = "0";
                                break;
                            case 4:
                                textBox11.Text = "0";
                                break;
                            case 5:
                                textBox12.Text = "0";
                                break;
                            case 6:
                                textBox13.Text = "0";
                                break;
                            case 7:
                                textBox14.Text = "0";
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
            else
            {
                for(int j=0;j<4;j++)
                {
                    Motion m = new Motion();
                    m.sTaskNo = textBox6.Text;
                    m.sTaskType = Convert.ToInt16(textBox1.Text);
                    switch (j)
                    {
                        case 0: 
                            m.barcode = textBox2.Text;
                            break;
                        case 1: 
                            m.barcode = textBox3.Text;
                            break;
                        case 2:
                            m.barcode = textBox4.Text;
                            break;
                        case 3: 
                            m.barcode = textBox5.Text;
                            break;
                        default:
                            break;
                    }
                    if (j == 0)
                    {
                        m.x = Convert.ToInt16(textBox7.Text);
                        m.y = Convert.ToInt16(textBox8.Text);
                        m.xLength = Convert.ToInt16(textBox9.Text);
                        m.yLength = Convert.ToInt16(textBox10.Text);
                        m.OriAgv = Convert.ToInt16(textBox11.Text);
                        m.pointType = Convert.ToInt16(textBox12.Text);
                        m.AntiCollision = Convert.ToInt16(textBox13.Text);
                        m.OriDial = Convert.ToInt16(textBox14.Text);
                    }
                    motionL.Add(m);
                }   
                this.Close();
            }          
        }
    }
}
