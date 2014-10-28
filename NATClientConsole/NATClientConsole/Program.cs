using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NATClientLibrary;

namespace NATClientConsole
{
    class Program
    {
        private static NAT_Platform ConnectAgent_platform;
        private static bool AgentConnectFlag;
        private static bool ServerConnectFlag;
        static void Main(string[] args)
        {
            AgentConnectFlag = false;
            ServerConnectFlag = false;

            Console.Write("----------------------------------------\n");
            Console.Write("NATClient [Version 1 2014.10.12]\n");
            Console.Write("Prepare to start....\n");

            ConnectAgent_platform = new NAT_Platform();

            Console.Write("----------------------------------------\n");
            Console.Write("Try connect to Local Agent....\n");

            AgentConnectFlag = ConnectAgent_platform.Agent_Connect();

            if (AgentConnectFlag)
            {
                Console.Write("connect to Local Agent Success.\n");

                ServerConnectFlag = ConnectAgent_platform.Agent_ConnectServer();
                if(ServerConnectFlag)
                    Console.Write("connect to Server Success.\n");
                else
                    Console.Write("connect to Local Agent Fail.\n");
            }
            else
            {
                Console.Write("connect to Local Agent Fail.\n");
            }

            // Pause program
            Console.Read();


        }
    }
}
