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
        static void Main(string[] args)
        {
            ConnectAgent_platform = new NAT_Platform();
            ConnectAgent_platform.Agent_Connect();
            while (true)
            {
                string messages = Console.ReadLine();
                ConnectAgent_platform.SendTest("Ins01" + messages.ToString());
            }
        }
    }
}
