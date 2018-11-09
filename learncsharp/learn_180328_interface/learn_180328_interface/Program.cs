using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180328_interface
{
    class Program:People,Teacher,Student
    {
        string name = "";

        public string Name 
        { 
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int Age { get; set; }
        public void Teach()
        {
            Console.WriteLine(Name+"\t"+Age+"\t老师");
        }
        public void Learn()
        {
            Console.WriteLine(Name + "\t" + Age + "\t学生");
        }
        static void Main(string[] args)
        {
            Program pro = new Program();
            Teacher tec = pro;
            tec.Name = "wang";
            tec.Age = 30;
            tec.Teach();
            Student stu = pro;
            stu.Name = "liu";
            stu.Age = 15;
            stu.Learn();
            Console.ReadKey();

        }
    }
}
