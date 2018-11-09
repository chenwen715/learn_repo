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

namespace learn_180404_Socket_2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        Thread th;
        Thread th1;
        string displayStr = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SocketBuild()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(Properties.Settings.Default.Host);
                IPEndPoint ipe = new IPEndPoint(ip, int.Parse(Properties.Settings.Default.Port));
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ipe);
                serverSocket.Listen(10);

                while (true)
                {

                    Socket s = serverSocket.Accept();
                    message(s);
                    //th = new Thread(message);
                    //th.Start(s);
                    Dispatcher.Invoke(new Action(() =>
                    {
                       
                        DisplayWindow.Text += displayStr + "\n";
                        displayStr = "";
                    }));
                    
                   

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
           
        }

        private void message(object obj)
        {
            byte[] recvByte=new byte[1024];
            string recvString = "";
            Socket s = obj as Socket;
            try
            {
                int bytes = s.Receive(recvByte,recvByte.Length,0);
                recvString = Encoding.ASCII.GetString(recvByte,0,bytes);
                displayStr += s.RemoteEndPoint.ToString() + " : "+recvString;

                string sendStr=recvString+" receive successfully";
                byte[] sendByte=Encoding.ASCII.GetBytes(sendStr);
                s.Send(sendByte,sendByte.Length,0);
                s.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            th1 = new Thread(SocketBuild);
            th1.Start();
        }

    }
}
