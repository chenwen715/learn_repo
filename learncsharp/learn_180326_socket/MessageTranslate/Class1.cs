using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageTranslate
{
    public class Class1
    {
        public const int port = 6665;
        public string StartListen()
        {
            Socket svrSocket;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipe = new IPEndPoint(ip, port);
            svrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            svrSocket.Bind(ipe);
            svrSocket.Listen(50);
            Socket ClientSocket = svrSocket.Accept();
            ClientSocket.ReceiveTimeout = 3000;
            byte[] ReceiveData = new byte[1024];
            int DataLength = ClientSocket.Receive(ReceiveData);
            if (DataLength > 0)
            {
                string message = Encoding.ASCII.GetString(ReceiveData,0,ReceiveData.Length);
                return message;
            }
            else
            {
                return "fail";
            }
        
        }

        public string Send(string server, string strContent)
        {
            Socket clientSocket ;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(server);
            IPEndPoint ipEndPoint= new IPEndPoint(ipaddress,port);
            byte[] btContent = Encoding.ASCII.GetBytes(strContent);
            clientSocket.Send(btContent);
            clientSocket.Close();
            return "发送成功";
        }
    }
}
