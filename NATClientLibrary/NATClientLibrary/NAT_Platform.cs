using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace NATClientLibrary
{
    public class NAT_Platform
    {
        private LocalIPDetect IpDetector;
        private Thread ConnectThread;
        private const String Agent_Ipaddress = "127.0.0.1";
        private const int Agent_Port = 15000;
        private TcpClient ConnectServer;
        public NAT_Platform()
        {
            IpDetector = new LocalIPDetect();
            //Agent_Ipaddress = IpDetector.getLocalIpAddress();
        }

        public bool Agent_Connect()
        {
            new Thread(new ThreadStart(Agent_Connect_Thread)).Start();
            return true;
        }

        private void Agent_Connect_Thread()
        {
            IPEndPoint ipont = new IPEndPoint(IPAddress.Parse(Agent_Ipaddress), Agent_Port);
            ConnectServer = new TcpClient();
            ConnectServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //IPEndPoint Bindipont = new IPEndPoint(IPAddress.Parse(Agent_Ipaddress), Agent_Port);
            //ConnectServer.Client.Bind(new IPEndPoint(IPAddress.Parse(BindIp), BindPort));
            ConnectServer.Connect(ipont);
            byte[] data = Encoding.UTF8.GetBytes("Ins01" + "Message Send!");
            ConnectServer.Client.Send(data);



            //ConnectServer.Connect(ipont);
            //Thread sendToserver = new Thread(new ThreadStart(MessageSends));
            //Thread ReceiveMessageThread = new Thread(new ThreadStart(ReceiveMessage));
            //ReceiveMessageThread.Start();
            //Thread.Sleep(2000);
            //sendToserver.Start();
        }

        public void SendTest(string Message)
        {
            byte[] data = Encoding.UTF8.GetBytes(Message);
            ConnectServer.Client.Send(data);
        }
    }
}
