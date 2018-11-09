using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180327_basic
{
    class people
    {
        string name="yura";
        public int age;
        double weight;
        double height;

        public int GetAge(int a)
        {
            return a;
        }
        public void SetAge(int b)
        {
            this.age = b;
        }
        public string GetName()
        {
             
            return this.name;
        }
    }
}
