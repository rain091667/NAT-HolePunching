using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocalIpDetectLibrary;
using System.Threading;
using System.Net.Sockets;


namespace NATServerConsole
{
    class Program
    {
        private static LocalIPDetect IpDetector;
        private static Random Random_PortCreator;
        private static string LocalServerIp;
        static void Main(string[] args)
        {
            Console.Write("----------------------------------------\n");
            Console.Write("NATServer [Version 1 2014.10.12]\n");
            Console.Write("Prepare to start....\n");
            Console.Write("----------------------------------------\n");

            IpDetector = new LocalIPDetect();
            Random_PortCreator = new Random();
            LocalServerIp = IpDetector.getLocalIpAddress();
            Console.Write("Server Local Ip address: " + LocalServerIp + "\n");
            new Thread(Client_ListenThread).Start();
            Console.Write("Server Start....\n");
            Console.Write("Wait for client connection...\n");
            Console.Write("----------------------------------------\n");
        }

        private static void Client_ListenThread()
        {
            System.Net.IPAddress theIPAddress;
            theIPAddress = System.Net.IPAddress.Parse(LocalServerIp);
            TcpListener myTcpListener = new TcpListener(theIPAddress, 25900);
            myTcpListener.Start();
            int dataLength;
            //Socket Client_Socket = myTcpListener.AcceptSocket();
            do
            {
                Socket Client_Socket = myTcpListener.AcceptSocket();
                byte[] myBufferBytes = new byte[1000];
                if (Client_Socket.Connected == true)
                {
                    try
                    {
                        dataLength = Client_Socket.Receive(myBufferBytes);
                        string getMsg = Encoding.UTF8.GetString(myBufferBytes, 0, dataLength);
                        string Instructions = getMsg.Substring(0, 5);
                        string The_messages = getMsg.Substring(5);
                        if (Instructions == "Ins21")
                        {
                            //Client_Socket;
                            //Console.Write(The_messages + "\n");
                            //ProxyServerConnection ClientConnections = new ProxyServerConnection(Client_Socket, Proxy_ServerIp);
                            //ClientConnections.StartClientListen();

                            //new Thread(Client_SingleThread).Start(Client_Socket);
                            Console.Write("Server Client Connect: " + Client_Socket.RemoteEndPoint.ToString() + "\n");
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
        }
    }
}
