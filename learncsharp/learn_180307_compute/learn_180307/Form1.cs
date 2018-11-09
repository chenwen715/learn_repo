using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180307 //计算器程序，暂不能进行混合运算（优先级）
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text =textBox2.Text= "0";
        }

        double num1=0,num2=0;
        string sign1="",sign2="";
        int flag = 0;
        

        private void button1_Click(object sender, EventArgs e)
        {
            GetText(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetText(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetText(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetText(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GetText(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GetText(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GetText(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GetText(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            GetText(9);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            GetText(0);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0" || flag==0)
            {
                textBox2.Text =  textBox1.Text="0.";
                flag = 1;
            }
            else
            {
                textBox2.Text += ".";
                textBox1.Text += ".";
            }
        }

        public void GetText(int num)
        {
            if (flag == 0)
            {
                textBox2.Text = num.ToString();
                num2 = Convert.ToDouble(textBox2.Text);
                //sign2 = "";
                textBox1.Text = "";
                flag = 1;
                textBox1.Text += textBox2.Text;
            }
            else if (sign2 == "=")
            {
                num1 = num2 = 0;
                textBox2.Text = num.ToString();
                num2 = Convert.ToDouble(textBox2.Text);
                sign1 = sign2 = "";
                textBox1.Text = textBox2.Text;
                
            }
            else if (sign2 != "" )
            {
                textBox2.Text = num.ToString();
                num2 = Convert.ToDouble(textBox2.Text);
                sign2 = "";
                textBox1.Text += textBox2.Text;
           
            }
            else
            {
                textBox2.Text += num.ToString();
                num2 = Convert.ToDouble(textBox2.Text);
                sign2 = "";
                textBox1.Text += num.ToString();
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            count(sign1);
            sign1 = sign2 = "+";
            textBox1.Text = textBox1.Text+  sign1;
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            count(sign1);
            sign1 = sign2 = "-";
            //textBox1.Text = textBox2.Text + sign1;
            textBox1.Text = textBox1.Text + sign1;
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            count(sign1);
            sign1 = sign2 = "*";
            //textBox1.Text = textBox2.Text + sign1;
            textBox1.Text = textBox1.Text + sign1;
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            count(sign1);
            sign1 = sign2 = "/";
            //textBox1.Text = textBox2.Text + sign1;
            textBox1.Text = textBox1.Text + sign1;
           
        }
        public void count(string sign)
        {
            if (sign == "+")
            {
                num1 += num2;
                num2 = 0;
            }
            else if (sign == "-")
            {
                num1 -= num2;
                num2 = 0;
            }
            else if (sign == "*")
            {
                num1 *= num2;
                num2 = 1;
            }
            else if (sign == "/")
            {
                num1 /= num2;
                num2 = 0;
            }
            else if (sign == "=")
            {
                num2 = 0;
                textBox1.Text = textBox2.Text;
            }
            else 
            {
                num1 = num2;
            }
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            switch (sign1)
            {
                case "+": 
                    num1 += num2;
                    sign1 = sign2 = "=";
                    
                    textBox2.Text = num1.ToString();
                    break;
                case "-": 
                    num1 -= num2;
                    sign1 = sign2 = "=";
                    
                    textBox2.Text = num1.ToString();
                    break;
                case "*": 
                    num1 *= num2;
                   
                    sign1 = sign2 = "=";
                    textBox2.Text = num1.ToString();
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        textBox2.Text = "除数不能为0";
                    }
                    else
                    {
                        num1 /= num2;
                        sign1 = sign2 = "=";
                       
                        textBox2.Text = num1.ToString();
                    }                
                    break;
                default:
                    sign1 = sign2 = "=";
                    textBox2.Text = num1.ToString();
                    break;

            }
           
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text =textBox2.Text ="0";
            num1 =num2 = 0;
           
            sign1  =sign2 = "";
            flag =0;
        }


       
    }
}
