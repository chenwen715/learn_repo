using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_180317
{
    class Class1
    {
        static int[] AddContent(int[] a, int index, int[] b)
        {
            int num=a.Length + b.Length - 1;
            int[] c = new int[num];
            if (index > a.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    c[i] = a[i];
                }
                for (int j = a.Length; j < num; j++)
                {
                    c[j] = b[j - a.Length];
                }
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    c[i] = a[i];
                }
                for (int j = index; j < (index + b.Length); j++)
                {
                    c[j]=b[j-index];
                }
                for(int z=index + b.Length;z<num;z++){
                    c[z] = a[z - b.Length];
                }
            }
            return c;
        }
    }
}
