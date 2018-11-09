using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace learn_180403_socket//服务端
{
    class Program
    {
        string host = Properties.Settings.Default.Host;
        int port = int.Parse(Properties.Settings.Default.Port);
        

        static void Main(string[] args)
        {
           
           
                Program p = new Program();
                p.Connect();
            
           
            
           
        }

        private void Connect()
        {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip,port);
            Socket s = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            s.Bind(ipe);
            s.Listen(0);
            while (true)
            {
                Socket serverSocket = s.Accept();
                Thread th = new Thread(Receive);
                th.Start(serverSocket);
            }
           
            
        }
        private void Receive(Object cSocket)
        {
            Socket serverSocket = cSocket as Socket;
        
            
            byte[] receiveByte = new byte[1024];
            string receiveStr = "";
            int bytes = serverSocket.Receive(receiveByte, receiveByte.Length, 0);
            receiveStr = Encoding.ASCII.GetString(receiveByte, 0, bytes);

            string sendStr = receiveStr + " receive successfully";
            byte[] sendByte = Encoding.ASCII.GetBytes(sendStr);
            serverSocket.Send(sendByte, sendByte.Length, 0);
            serverSocket.Close();
            
        }
        
    }
}
