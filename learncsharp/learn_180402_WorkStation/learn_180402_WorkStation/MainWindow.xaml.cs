using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace learn_180402_WorkStation//客户端
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string host = Properties.Settings.Default.Host;
        private string port = Properties.Settings.Default.Port;
        List<WorkStationClass> wslist = new List<WorkStationClass>();
        Thread thread;
        

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                string sql = @"";

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }



        private void Connect()
        {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, int.Parse(port));
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);

            string sendStr = textBox1.Text;
            byte[] sendByte = Encoding.ASCII.GetBytes(sendStr);
            clientSocket.Send(sendByte);

            byte[] receiveByte = new byte[1024];
            string receiveStr = String.Empty;
            int bytes=clientSocket.Receive(receiveByte,receiveByte.Length,0);
            receiveStr += Encoding.ASCII.GetString(receiveByte,0,bytes);
            clientSocket.Close();
            receiveBox.Text = receiveStr;
            
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

     

       


    }
}
