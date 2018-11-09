using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAY1
{
    class Class1
    {
        private Socket SvrSocket;
        private void socketBuild()
        {
            //转换IP
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //将IP与端口绑定
            IPEndPoint ipep = new IPEndPoint(ip, 8889);
            //创建服务器端套接字
            SvrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //将套接字与IPEndPoint绑定
            SvrSocket.Bind(ipep);
            //开始侦听
            SvrSocket.Listen(50);
            //用循环实现该线程的实时侦听
            while (true)
            {
                Socket ClientSocket = SvrSocket.Accept();
                Thread clientThread = new Thread(new ThreadStart(receivedate));
                clientThread.Start();
            }
            

        }

        private void receivedate()
        {
            
        }


    }
}
