using Server.Network;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace InstantMessagingAplikacija
{
    internal class Server
    {
        static void Main(string[] args)
        {
            int udpPort = 9000;
            int tcpPort = 9001;

            UdpListener udp = new UdpListener(udpPort, tcpPort);

            new Thread(udp.Start).Start();

            Console.WriteLine("Server radi, enter za izlaz");
            Console.ReadLine();

        }

    }
}
