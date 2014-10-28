using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using LocalIpDetectLibrary;

namespace NATAgentConsole
{
    class Program
    {
        private static LocalIPDetect IpDetector;
        private static string LocalServerIp;
        private static Thread ClientRemoteThread;
        private static Random Random_PortCreator;
        private static string Command;
        private static string Proxy_ServerIp = "219.84.180.175";
        
        static void Main(string[] args)
        {
            Console.Write("----------------------------------------\n");
            Console.Write("NATAgent [Version 1 2014.10.12]\n");
            Console.Write("Prepare to start....\n");
            Console.Write("----------------------------------------\n");

            IpDetector = new LocalIPDetect();
            Random_PortCreator = new Random();
            LocalServerIp = IpDetector.getLocalIpAddress();
            Console.Write("Agent Local Ip address: " + LocalServerIp + "\n");
            ClientRemoteThread = new Thread(Client_RemoteListenThread);
            ClientRemoteThread.Start();
            Console.Write("Agent Start....\n");
            Console.Write("Wait for client connection...\n");
            Console.Write("----------------------------------------\n");

            /*
            while (true)
            {
                Console.Write("\nAgent Command.>");
                Command = Console.ReadLine();
                if (Command == "?")
                {
                    Console.Write("\nAgent Command Help:>");
                    Console.Write("\nlsconnect\t\t列出所有建立連線(包含Clinet與Server端)");
                }
            }
            */
            /*Console.Write("\r\nPress any key to continue....");
            Console.Read();*/
        }

        private static void Client_RemoteListenThread()
        {
            System.Net.IPAddress theIPAddress;
            theIPAddress = System.Net.IPAddress.Parse("127.0.0.1");
            TcpListener myTcpListener = new TcpListener(theIPAddress, 15000);
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
                        if (Instructions == "Ins01")
                        {
                            //Client_Socket;
                            //Console.Write(The_messages + "\n");
                            ProxyServerConnection ClientConnections = new ProxyServerConnection(Client_Socket, Proxy_ServerIp);
                            ClientConnections.StartClientListen();

                            //new Thread(Client_SingleThread).Start(Client_Socket);
                            Console.Write("Client Connect: " + Client_Socket.RemoteEndPoint.ToString() + "\n");
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
