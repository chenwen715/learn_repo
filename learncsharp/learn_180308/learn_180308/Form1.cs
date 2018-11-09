using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace learn_180308//小写汉字金额转换为大写汉字金额
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string beforePrice=textBox1.Text;
            string afterPricr="";
            string tmp = "";
            string temp1 = "";
            for (int i = 0; i < beforePrice.Length; i++)
            {
                tmp = beforePrice.Substring(i,1);
                temp1 = String.Format(ChangePrice(tmp),tmp);
                if (temp1 == "wrong")
                {
                    textBox2.Text = "输入不正确，请重新输入";
                    return;//return跳出方法，或使用下面的跳出循环后继续执行后面的语句

                    //afterPricr = "输入不正确，请重新输入";
                    //break;
                }
                afterPricr += temp1;
            }
            textBox2.Text = afterPricr;
        }

        public string ChangePrice(string a)
        {
            switch (a)
            {
                case "一": return "壹";                   
                case "二": return "贰";                   
                case "三": return "叁";                  
                case "四": return "肆";                   
                case "五": return "伍";                    
                case "六": return "陆";                   
                case "七": return "柒";                    
                case "八": return "捌";                   
                case "九": return "玖";
                case "十": return "拾";
                case "百": return "佰";
                case "千": return "仟";
                case "万": return "萬";
                case "亿": return "亿";
                case "点": return "点";
                default: return "wrong";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
