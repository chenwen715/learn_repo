using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace learn_180403_Thread
{


    //// 热水器
    //public class Heater 
    //{
    //   private int temperature;
    //   public delegate void BoilHandler(int param);   //声明委托
    //   public event BoilHandler BoilEvent;        //声明事件

    //   // 烧水
    //   public void BoilWater() 
    //   {
    //       for (int i = 0; i <= 100; i++) 
    //       {
    //          temperature = i;

    //          if (temperature > 95) 
    //          {
    //              if (BoilEvent != null) 
    //              { //如果有对象注册
    //                  BoilEvent(temperature);  //调用所有注册对象的方法
    //              }
    //          }

    //       }
    //       Console.ReadKey();

    //   }
    //}

    //// 警报器
    //public class Alarm 
    //{
    //   public void MakeAlert(int param) 
    //   {
    //       Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
    //   }
    //}

    //// 显示器
    //public class Display 
    //{
    //   public static void ShowMsg(int param) 
    //   { //静态方法
    //       Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", param);
    //   }
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {

    //        Heater heater = new Heater();
    //        Alarm alarm = new Alarm();
    //        heater.BoilEvent += alarm.MakeAlert;
    //        //heater.BoilEvent += (new Alarm()).MakeAlert;
    //        heater.BoilEvent += Display.ShowMsg;
    //        heater.BoilWater();
    //    }


    //}


}