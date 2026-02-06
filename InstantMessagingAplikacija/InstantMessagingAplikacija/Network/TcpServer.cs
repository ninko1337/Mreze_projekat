using Server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Network
{
    internal class TcpServer
    {
        private readonly int _tcpPort;
        private readonly ServerManager _serverManager;

        public TcpServer(int tcpPort, ServerManager serverManager)
        {
            _tcpPort = tcpPort;
            _serverManager = serverManager;
        }

        public void Start()
        {
            var listener = new TcpListener(IPAddress.Any, _tcpPort);
            listener.Start();
            Console.WriteLine($"[SERVER][TCP] listening na portu {_tcpPort}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("[SEVER][TCP] Klijent je povezan na server");

                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (client)
            using (NetworkStream ns = client.GetStream())
            {
                try
                {
                    //1- korisnik salje nadimak
                    string nadimakLinija = PrimiString(ns);
                    string nadimak = "NEPOZNATO";
                    if (!string.IsNullOrWhiteSpace(nadimakLinija) && nadimakLinija.StartsWith("NADIMAK "))
                        nadimak = nadimakLinija.Substring(8).Trim();

                    //2 - Posalji listu svih servera koji postoje
                    string serveriLista = string.Join(",", _serverManager.Serveri.Keys);
                    PosaljiString(ns, "SERVERI : " + serveriLista);

                    //3 - Prima korisnik izbor servera sad
                    string serverLine = PrimiString(ns);
                    //za debug
                    Console.WriteLine($"[DEBUG SERVER] Primio sam liniju: '{serverLine}'");

                    if (string.IsNullOrWhiteSpace(serverLine) || !serverLine.StartsWith("SERVER "))
                    {
                        PosaljiString(ns, "ERROR greska prilikom biranja servera");
                        return;
                    }

                    string izabraniServer = serverLine.Substring(7).Trim();
                    if (!_serverManager.Serveri.ContainsKey(izabraniServer))
                    {
                        PosaljiString(ns, "ERROR ne postoji takav server");
                        return;
                    }

                    //4 - salje se lista svih kanala na tom serveru
                    var kanali = _serverManager.Serveri[izabraniServer].Select(k => k.Naziv);
                    PosaljiString(ns, "KANALI : " + string.Join(",", kanali));

                    //5 - prima se izbor kanala
                    string kanalLinija = PrimiString(ns);
                    if (string.IsNullOrWhiteSpace(kanalLinija) || !kanalLinija.StartsWith("KANAL "))
                    {
                        PosaljiString(ns, "ERROR greska prilikom biranja kanala");
                        return;
                    }

                    string odabraniKanal = kanalLinija.Substring(6).Trim();

                    //6 - prima se poruka koju klijent salje u taj kanal
                    string porukaLinija = PrimiString(ns);
                    if (string.IsNullOrWhiteSpace(porukaLinija) || !porukaLinija.StartsWith("PORUKA "))
                    {
                        PosaljiString(ns, "ERROR greska prilikom slanja poruke");
                        return;
                    }

                    string poruka = porukaLinija.Substring(7).Trim();

                    //7 - ispis u formatu koji se trazi 
                    // treba da bude [Datum/Vreme] - [Server]:[Kanal]:[poruka]-[nadimak]
                    string vreme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Console.WriteLine($"[{vreme}]-[{izabraniServer}]:[{odabraniKanal}]:[{poruka}]-[{nadimak}]");

                    PosaljiString(ns, "Ok");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"[SERVER][TCP] greska u komunikaciji -  {ex.Message}");
                }
            }
        }

        private static void PosaljiString(NetworkStream ns , string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text + "\n");
            ns.Write(data, 0, data.Length);
        }

        private static string PrimiString(NetworkStream ns)
        {
            List<byte> bytes = new List<byte>();
            byte[] buffer = new byte[1];

            while (true)
            {
                int procitao = ns.Read(buffer, 0, 1);
                if (procitao <= 0) return null;
                if (buffer[0] == (byte)'\n') break;
                bytes.Add(buffer[0]);

            }

            return Encoding.UTF8.GetString(bytes.ToArray()).TrimEnd('\r');
        }
    }
}
