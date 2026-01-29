using Client.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Klijent
    {
        static void Main(string[] args)
        {

            string serverIp = "127.0.0.1";
            int udpPort = 9000;

            string resp = UdpClientService.Prijava(serverIp, udpPort);
            Console.WriteLine("Od servera : " + resp);
            Console.ReadLine();
        }
    }
}
