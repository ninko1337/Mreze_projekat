using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace Client.Network
{
    public static class UdpClientService
    {
        public static string Prijava(string serverIp, int udpPort)
        {
            Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIp), udpPort);

            string message = "PRIJAVA";

            byte[] msgBytes = Encoding.UTF8.GetBytes(message);

            sendSocket.SendTo(msgBytes, 0, msgBytes.Length, SocketFlags.None, serverEP);

            byte[] recvBuf = new byte[1024];
            EndPoint fromEP = new IPEndPoint(IPAddress.Any, 0);

            int bytes = sendSocket.ReceiveFrom(recvBuf, ref fromEP);
            string response = Encoding.UTF8.GetString(recvBuf, 0, bytes).Trim();

            sendSocket.Close();

            return response;
        }
    }
}
