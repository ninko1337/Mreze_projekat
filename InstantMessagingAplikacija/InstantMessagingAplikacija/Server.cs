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
            //konfiguracija servera 
            var manager = new ServerManager();

            manager.Konfigurisi();

            int udpPort = 9000;
            int tcpPort = 9001;

            //pokretanje udplistenera
            UdpListener udp = new UdpListener(udpPort, tcpPort);
            new Thread(udp.Start).Start();

            //pokretanje tcp servera
            TcpServer tcp = new TcpServer(tcpPort, manager);
            new Thread(tcp.Start).Start();

            Console.WriteLine($"[SISTEM] UDP Server sluša na portu {udpPort}");
            Console.WriteLine($"[SISTEM] TCP Server sluša na portu {tcpPort}");
            Console.WriteLine("Server radi, pritisnite Enter za izlaz...");

            Console.ReadLine();

        }

    }
}
