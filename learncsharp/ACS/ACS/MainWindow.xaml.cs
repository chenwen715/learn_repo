using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ACS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Thread thSocket;
        private Thread taskSocket;
        private Socket SvrSocket;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppDispatcher = this.Dispatcher;
            App.WinMain = this;

            //设置全屏
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.WorkArea.Height;
            Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
            App.Canvas_Monit = Canvas_Monitor;
            
            //初始化数据
            Down.InitData();

            //做任务
            taskSocket = new Thread(TaskDoing);
            taskSocket.IsBackground = true;
            taskSocket.Start();

            //和AGV通讯
            thSocket = new Thread(SocketBuild);
            thSocket.IsBackground = true;
            thSocket.Start();
        }

        void Test()
        {
            string sql = "Select * from T_Base_Point ";
            DataSet ds = DbHelperSQL.Query(sql);

            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);

            string sql1 = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string PointNo = dr["PointNo"].ToString();
                sql1 = "UPDATE dbo.T_Base_Point SET IsLocking = 0 WHERE PointNo = '" + PointNo + "';";
                DbHelperSQL.ExecuteSql(sql1);
            }

            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts3 = ts2.Subtract(ts1).Duration();
            int milli1 = ts3.Milliseconds;
            App.ExFile.MessageLog("计算差值", "1|" + ts2.Milliseconds + "-" + ts1.Milliseconds + "="  + ts3.Milliseconds);


            TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);
            string sql2 = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string PointNo = dr["PointNo"].ToString();
                sql2 += "UPDATE dbo.T_Base_Point SET IsLocking = 0 WHERE PointNo = '" + PointNo + "';";
            }
            DbHelperSQL.ExecuteSql(sql1);

            TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts6 = ts4.Subtract(ts5).Duration();
            App.ExFile.MessageLog("计算差值", "2|" + ts5.Milliseconds + "-" + ts4.Milliseconds + "=" + ts6.Milliseconds);
        }

        private void TaskDoing()
        {
            while (true)
            {
                if (Commond.IsClose)
                    break;

                Task.DoWork();
                Thread.Sleep(2000);
            }
        }

        private void SocketBuild()
        {
            if (SvrSocket != null)
            {
                SvrSocket.Close();
                SvrSocket = null;
            }
            try
            {
                IPAddress ip = IPAddress.Parse(App.Ip);
                IPEndPoint ipe = new IPEndPoint(ip, App.Port);
                //Socket 
                SvrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SvrSocket.Bind(ipe);

                ManageTcp.ServerListen(App.ServerConactNum, SvrSocket);
            }
            catch (Exception Ex)
            {
                App.ExFile.MessageError( "SocketBuild", "创建AGV监听线程失败：" + Ex.ToString());
            }
        }

        #region 界面操作

        /// <summary>
        /// 左键拖框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 最小化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        /// <summary>
        /// 最大化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Commond.IsClose = true;
            this.Close();
        }

        /// <summary>
        /// 菜单栏点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;
            if (btn == null) return;
            if (btn.Name == Btn_TaskManager.Name)
            {
                Test();
                //Framework.UI.WinTaskManager WinTaskManager = new Framework.UI.WinTaskManager();
                //WinTaskManager.Topmost = true;
                //WinTaskManager.Show();

            }
            else if (btn.Name == Btn_RunData.Name)
            {
                //Framework.UI.WinRunData WinRunData = new Framework.UI.WinRunData();
                //WinRunData.Show();
            }
            else if (btn.Name == Btn_MachineManager.Name)
            {
                //Framework.UI.WinOrganization WinOrg = new Framework.UI.WinOrganization();
                //WinOrg.Show();
            }
        }

        /// <summary>
        /// 显示路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MItem_DisplayPath_Click(object sender, RoutedEventArgs e)
        {
            //if (DGrid_ExecUnit.SelectedItem == null) return;
            //ExecUnit agv = DGrid_ExecUnit.SelectedItem as ExecUnit;
            //if (agv._EuCTasksList.Count <= 0) return;
            //agv._IsShowEuPath = true;
            //App.BllCom.ShowPath(agv._EuTaskNo);
        }

        /// <summary>
        /// 隐藏路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MItem_HidePath_Click(object sender, RoutedEventArgs e)
        {
            //if (DGrid_ExecUnit.SelectedItem == null) return;
            //ExecUnit agv = DGrid_ExecUnit.SelectedItem as ExecUnit;
            //Sharp.PathHide(agv._ExecCode);

            //agv._IsShowEuPath = false;
        }

        /// <summary>
        /// 异常信息清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TBox_EMessage.Clear();
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Commond.TestAgvNo = AgvNo.Text;
        }
    }

    public class MakeupFonts : System.Windows.Markup.MarkupExtension
    {
        private string SizeType;
        public MakeupFonts(string fontType)
        { SizeType = fontType; }
        public override object ProvideValue(IServiceProvider Isp)
        {
            double FontSize = 0d;
            switch (SizeType)
            {
                case "Title":
                    FontSize = 20;
                    break;
                case "Normal":
                    FontSize = 12;
                    break;
                case "BottomTitle":
                    FontSize = 20;
                    break;

            }
            return FontSize;
        }
    }

    //定义值转换器
    [ValueConversion(typeof(string), typeof(string))]
    //小车背景色
    public class AgvBackGroundConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.Transparent);

            if (int.Parse(value.ToString()) == 0)
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            else if (int.Parse(value.ToString()) == 1)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (int.Parse(value.ToString()) == 2)
            {
                return new SolidColorBrush(Colors.LightGreen);
            }
            else if (int.Parse(value.ToString()) == 3)
            {
                return new SolidColorBrush(Colors.LightSkyBlue);
            }
            else if (int.Parse(value.ToString()) == 4)
            {
                return new SolidColorBrush(Colors.LightYellow);
            }
            else if (int.Parse(value.ToString()) == 90)
            {
                return new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
            //return Application.res
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //定义值转换器
    [ValueConversion(typeof(string), typeof(string))]
    //小车状态说明
    public class AgvStatusConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";

            if (value.ToString() == "1")
            {
                return "低位";
            }
            else if (value.ToString() == "2")
            {
                return "中位";
            }
            else if (value.ToString() == "3")
            {
                return "高位";
            }
            else if (value.ToString() == "10")
            {
                return "自动、无码";
            }
            else if (value.ToString() == "11")
            {
                return "自动、空闲";
            }
            else if (value.ToString() == "12")
            {
                return "自动、歪码";
            }
            else if (value.ToString() == "13")
            {
                return "忙碌";
            }
            else if (value.ToString() == "14")
            {
                return "充电中";
            }
            else if (value.ToString() == "15")
            {
                return "充电完成";
            }
            else if (value.ToString() == "20")
            {
                return "手动、无码";
            }
            else if (value.ToString() == "21")
            {
                return "手动、有码";
            }
            else
            {
                return value.ToString();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    //定义值转换器
    [ValueConversion(typeof(string), typeof(string))]
    //任务异常背景色
    public class TaskErrConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.Transparent);

            if (int.Parse(value.ToString()) == 0)
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
            //return Application.res
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
