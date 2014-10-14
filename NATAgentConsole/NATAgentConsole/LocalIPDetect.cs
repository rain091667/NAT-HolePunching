using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace NATAgentConsole
{
    class LocalIPDetect
    {
        // Get Local Ip address
        public string getLocalIpAddress()
        {
            string strHostName = Dns.GetHostName();
            string myLocalIpAddress;
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    myLocalIpAddress = ipaddress.ToString();
                    return myLocalIpAddress;
                }
            }
            return "Fail to Get Local Ip";
        }

        public IList<IPEndPoint> GetUsedIPEndPoint()
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPointTCP = ipGlobalProperties.GetActiveTcpListeners();
            IPEndPoint[] ipEndPointUDP = ipGlobalProperties.GetActiveUdpListeners();
            TcpConnectionInformation[] tcpConnectionInformation = ipGlobalProperties.GetActiveTcpConnections();

            IList<IPEndPoint> allIPEndPoint = new List<IPEndPoint>();
            foreach (IPEndPoint iep in ipEndPointTCP) allIPEndPoint.Add(iep);
            foreach (IPEndPoint iep in ipEndPointUDP) allIPEndPoint.Add(iep);
            foreach (TcpConnectionInformation tci in tcpConnectionInformation) allIPEndPoint.Add(tci.LocalEndPoint);

            return allIPEndPoint;
        }

        // 判断指定的网络端点（只判断端口）是否被使用  
        public bool IsUsedIPEndPoint(int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        // 判断指定的网络端点（判断IP和端口）是否被使用  
        public bool IsUsedIPEndPoint(string ip, int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Address.ToString() == ip && iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
