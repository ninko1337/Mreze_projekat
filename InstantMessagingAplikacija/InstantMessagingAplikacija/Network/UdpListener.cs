using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Server.Network
{
    internal class UdpListener
    {

        private readonly int _udpPort;
        private readonly int _tcpPort;

        public UdpListener(int udpPort, int tcpPort)
        {
            _udpPort = udpPort;
            _tcpPort = tcpPort;
        }

        public void Start()
        {
            Socket recvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, _udpPort);
            recvSocket.Bind(localEP);

            Console.WriteLine($"[SERVER][UDP] Bindovao na {_udpPort},citam poruke...");

            byte[] buffer = new byte[1024];

            while (true)
            {
                EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);

                int bytes = recvSocket.ReceiveFrom(buffer, ref senderEP);
                string msg = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();

                Console.WriteLine($"[SERVER][UDP] Primio od {senderEP} : {msg}");

                if(msg == "PRIJAVA")
                {
                    string response = $"TCP:{_tcpPort}";
                    byte[] respBytes = Encoding.UTF8.GetBytes(response);

                    recvSocket.SendTo(respBytes, 0, respBytes.Length, SocketFlags.None, senderEP);

                    Console.WriteLine($"[SERVER][UDP] Poslao {response} ka {senderEP}");
                }
            }
        }
    }
}
