using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace learn_180326_socket_station
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread ThWhile;
        //private string host = "127.0.0.1";
        private string host = Properties.Settings.Default.Host;
        int port = 2024;
        List<StationClass> Alist = new List<StationClass>();
        static bool isClose = false;
        int sleepTime = 500;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {


        }
    }
}
