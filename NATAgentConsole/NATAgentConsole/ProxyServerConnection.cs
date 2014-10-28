using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using LocalIpDetectLibrary;
using System.Net;

namespace NATAgentConsole
{
    class ProxyServerConnection
    {
        private static LocalIPDetect IpDetector;
        private Socket ClientConnection;
        private string Proxy_ServerIp = "";
        private static Random Random_PortCreator;
        private static TcpClient ServerConnection;
        private static Thread ServerConnectionThread;
        private static bool Server_Connect_Flag;

        public ProxyServerConnection(Socket Temp_client, string Temp_ProxyServerIp)
        {
            this.ClientConnection = Temp_client;
            this.Proxy_ServerIp = Temp_ProxyServerIp;
            IpDetector = new LocalIPDetect();
            Random_PortCreator = new Random();
            Server_Connect_Flag = false;
        }

        public void StartClientListen()
        {
            new Thread(Client_SingleThread).Start();
        }

        private void Client_SingleThread()
        {
            //bool Server_Connect_Flag = false;
            int dataLength;
            do
            {
                byte[] myBufferBytes = new byte[1000];
                if (ClientConnection.Connected == true)
                {
                    try
                    {
                        dataLength = ClientConnection.Receive(myBufferBytes);
                        string getMsg = Encoding.UTF8.GetString(myBufferBytes, 0, dataLength);
                        string Instructions = getMsg.Substring(0, 5);
                        string The_messages = getMsg.Substring(5);
                        if (Instructions == "Ins02")
                        {
                            ServerConnectionThread = new Thread(Server_Connect_Thread);
                            ServerConnectionThread.Start();
                        }
                    }
                    catch (Exception e)
                    {
                        //若有連結錯誤則將錯誤訊息顯示出來
                        //myTcpListener.Stop(); //如未連線或其他例外狀況等,關閉tcplistener和socket
                        ClientConnection.Close();
                        //Console.Write("RemoteListenError! " + e.ToString() + "\n");
                        //Console.Write("\r\nPress any key to continue....");
                        //Console.Read();
                        //Environment.Exit(0);
                        //break;
                    }
                }
            } while (true);
        }

        private void Server_Connect_Thread()
        {
            IPEndPoint ipont = new IPEndPoint(IPAddress.Parse(Proxy_ServerIp), 25900);
            ServerConnection = new TcpClient();
            ServerConnection.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //IPEndPoint Bindipont = new IPEndPoint(IPAddress.Parse(Agent_Ipaddress), Agent_Port);
            //ConnectServer.Client.Bind(new IPEndPoint(IPAddress.Parse(BindIp), BindPort));

            try
            {
                ServerConnection.Connect(ipont);
                byte[] data = Encoding.UTF8.GetBytes("Ins21");
                ServerConnection.Client.Send(data);

                Server_Connect_Flag = true;

                new Thread(SendToClientMessage).Start("Ins02");
            }
            catch (Exception e)
            {
                //Console.Write("Exception: " + e.ToString());
                Server_Connect_Flag = false;
            }

            //ConnectServer.Connect(ipont);
            //Thread sendToserver = new Thread(new ThreadStart(MessageSends));
            //Thread ReceiveMessageThread = new Thread(new ThreadStart(ReceiveMessage));
            //ReceiveMessageThread.Start();
            //Thread.Sleep(2000);
            //sendToserver.Start();
        }

        private void SendToClientMessage(object Message)
        {
            string msg = (string)Message;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                ClientConnection.Send(data);

                Server_Connect_Flag = true;
            }
            catch (Exception e)
            {
                //Console.Write("Exception: " + e.ToString());
                Server_Connect_Flag = false;
            }
        }

        private static void RandomRemotePort(object client)
        {
            Socket Clients = (Socket)client;
            int random_Number = Random_PortCreator.Next(20000, 60000);
            while (IpDetector.IsUsedIPEndPoint(random_Number))
                random_Number = Random_PortCreator.Next(20000, 60000);

            Clients.Send(Encoding.UTF8.GetBytes("Ins01" + random_Number));

            System.Net.IPAddress theIPAddress;
            theIPAddress = System.Net.IPAddress.Parse("127.0.0.1");
            TcpListener myTcpListener = new TcpListener(theIPAddress, random_Number);
            myTcpListener.Start();
            int dataLength;
            Socket Client_Socket = myTcpListener.AcceptSocket();
            do
            {
                byte[] myBufferBytes = new byte[1000];
                if (Client_Socket.Connected == true)
                {
                    try
                    {
                        dataLength = Client_Socket.Receive(myBufferBytes);
                        string getMsg = Encoding.UTF8.GetString(myBufferBytes, 0, dataLength);
                        string Instructions = getMsg.Substring(0, 5);
                        string The_messages = getMsg.Substring(5);
                        if (Instructions == "Ins01")
                        {
                            //Client_Socket;
                            //Console.Write(The_messages + "\n");
                        }
                    }
                    catch (Exception e)
                    {
                        //若有連結錯誤則將錯誤訊息顯示出來
                        //myTcpListener.Stop(); //如未連線或其他例外狀況等,關閉tcplistener和socket
                        Client_Socket.Close();
                        //Console.Write("RemoteListenError! " + e.ToString() + "\n");
                        //Console.Write("\r\nPress any key to continue....");
                        //Console.Read();
                        //Environment.Exit(0);
                        //break;
                    }
                }
            } while (true);
            //IpDetector
        }
    }
}
