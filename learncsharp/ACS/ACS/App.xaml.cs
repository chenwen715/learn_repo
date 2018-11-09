using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace ACS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial  class App : Application
    {
        public static System.Windows.Threading.Dispatcher AppDispatcher;    //线程
        public static string ConCnString = ACS.Properties.Resources.sqlConnect;   //数据库
        public static string Ip = ACS.Properties.Resources.ServiceIP;   //服务端Ip
        public static int Port = int.Parse(ACS.Properties.Resources.ServicePort);    //端口号
        public static int ServerConactNum = int.Parse(ACS.Properties.Resources.ConactNum);  //监听数量
        public static bool IsTest = bool.Parse(ACS.Properties.Resources.IsTest);    //是否展示地图

        public static Canvas Canvas_Monit = null;   //控制界面显示
        public static MainWindow WinMain = null;    //初始化主界面
        public static int GsHeight = int.Parse(ACS.Properties.Resources.GsHeight);    //点上下间隔像素
        public static int GsWidth = int.Parse(ACS.Properties.Resources.GsWidth);  //点左右间隔像素

        public static object Lock_Thread = new object();//false;   //线程安全锁
        public static object Lock_Lock = new object();//false; //路径方向锁
        
        public static Log ExFile = new Log();   //存入日志

        public static List<Agv> AgvList = new List<Agv>();   //小车集合
        public static List<Shelf> ShelfList = new List<Shelf>();    //货架集合
        public static List<Point> pointList = new List<Point>();    //地图点集合

    }
}
