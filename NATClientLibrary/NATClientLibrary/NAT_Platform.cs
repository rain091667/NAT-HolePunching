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
        private TcpClient AgentConnection;
        private bool Agent_Connect_Flag;
        private bool Server_Connect_Flag;
        public NAT_Platform()
        {
            IpDetector = new LocalIPDetect();
            Agent_Connect_Flag = false;
            Server_Connect_Flag = false;
            //Agent_Ipaddress = IpDetector.getLocalIpAddress();
        }

        public bool Agent_Connect()
        {
            ConnectThread = new Thread(Agent_Connect_Thread);
            ConnectThread.Start();
            ConnectThread.Join();
            return Agent_Connect_Flag;
        }

        private void Agent_Connect_Thread()
        {
            IPEndPoint ipont = new IPEndPoint(IPAddress.Parse(Agent_Ipaddress), Agent_Port);
            AgentConnection = new TcpClient();
            AgentConnection.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //IPEndPoint Bindipont = new IPEndPoint(IPAddress.Parse(Agent_Ipaddress), Agent_Port);
            //ConnectServer.Client.Bind(new IPEndPoint(IPAddress.Parse(BindIp), BindPort));

            try
            {
                AgentConnection.Connect(ipont);
                byte[] data = Encoding.UTF8.GetBytes("Ins01");
                AgentConnection.Client.Send(data);

                Agent_Connect_Flag = true;

                new Thread(ReceiveThread).Start();
            }
            catch (Exception e)
            {
                //Console.Write("Exception: " + e.ToString());
                Agent_Connect_Flag = false;
            }

            //ConnectServer.Connect(ipont);
            //Thread sendToserver = new Thread(new ThreadStart(MessageSends));
            //Thread ReceiveMessageThread = new Thread(new ThreadStart(ReceiveMessage));
            //ReceiveMessageThread.Start();
            //Thread.Sleep(2000);
            //sendToserver.Start();
        }

        private void ReceiveThread()
        {
            int dataLength;
            do
            {
                byte[] myBufferBytes = new byte[1000];
                if (AgentConnection.Connected == true)
                {
                    try
                    {
                        dataLength = AgentConnection.Client.Receive(myBufferBytes);
                        string getMsg = Encoding.UTF8.GetString(myBufferBytes, 0, dataLength);
                        string Instructions = getMsg.Substring(0, 5);
                        string The_messages = getMsg.Substring(5);
                        if (Instructions == "Ins02")
                        {
                            Server_Connect_Flag = true;
                            //Client_Socket;
                            //Console.Write(The_messages + "\n");
                            //ProxyServerConnection ClientConnections = new ProxyServerConnection(Client_Socket, Proxy_ServerIp);
                            //ClientConnections.StartClientListen();

                            //new Thread(Client_SingleThread).Start(Client_Socket);
                            //Console.Write("Client Connect: " + Client_Socket.RemoteEndPoint.ToString() + "\n");
                        }
                    }
                    catch (Exception e)
                    {
                        //若有連結錯誤則將錯誤訊息顯示出來
                        //myTcpListener.Stop(); //如未連線或其他例外狀況等,關閉tcplistener和socket
                        AgentConnection.Close();
                        //Console.Write("RemoteListenError! " + e.ToString() + "\n");
                        //Console.Write("\r\nPress any key to continue....");
                        //Console.Read();
                        //Environment.Exit(0);
                        //break;
                    }
                }
            } while (true);
        }

        public bool Agent_ConnectServer()
        {
            if (!Agent_Connect_Flag)
            {
                Console.Write("Connect Status: false.\n  please connect agent first.\n");
                return false;
            }
            Thread ConnectThread = new Thread(Agent_ConnectServerThread);
            ConnectThread.Start();
            ConnectThread.Join();

            return Server_Connect_Flag;
        }

        private void Agent_ConnectServerThread()
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes("Ins02");
                AgentConnection.Client.Send(data);

                Thread.Sleep(5000);
            }
            catch (Exception e)
            {
                //Console.Write("Exception: " + e.ToString());
                Agent_Connect_Flag = false;
                Server_Connect_Flag = false;
            }
        }
    }
}
