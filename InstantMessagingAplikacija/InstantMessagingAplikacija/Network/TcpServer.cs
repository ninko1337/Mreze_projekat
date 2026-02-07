using Server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Network
{
    internal class TcpServer
    {
        private readonly int _tcpPort;
        private readonly ServerManager _serverManager;

        //lista svih aktivnih uticnica
        private List<Socket> _sviSocketi = new List<Socket>();

        private Dictionary<Socket, KlijentSesija> _sesije = new Dictionary<Socket,KlijentSesija>();

        private class KlijentSesija
        {
            public string Faza = "CEKA_NADIMAK";

            public string Nadimak;

            public string IzabraniServer;

            public string OdabraniKanal;

            public StringBuilder Bafer = new StringBuilder();
        }

        public TcpServer(int tcpPort, ServerManager serverManager)
        {
            _tcpPort = tcpPort;
            _serverManager = serverManager;
        }

        public void Start()
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, _tcpPort));
            listenSocket.Listen(10);
            listenSocket.Blocking = false;
            

            _sviSocketi.Add(listenSocket);
            Console.WriteLine($"[SERVER][TCP] Multipleksiranje aktivno na portu {_tcpPort}");

            while (true)
            {
                List<Socket> readList = new List<Socket>(_sviSocketi);

                Socket.Select(readList, null, null, 1000000);

                foreach(Socket s in readList)
                {
                    if(s == listenSocket)
                    {
                        Socket noviKlijent = listenSocket.Accept();
                        noviKlijent.Blocking = false;
                        _sviSocketi.Add(noviKlijent);
                        _sesije[noviKlijent] = new KlijentSesija();
                        Console.WriteLine("[SERVER] Novi klijent povezan.");
                    }
                    else
                    {
                        ObradiPodatke(s);
                    }
                }
            }
        }

        private void ObradiPodatke(Socket s)
        {
            KlijentSesija sesija = _sesije[s];
            byte[] buffer = new byte[1024];
            int primljeno;

            try
            {
                primljeno = s.Receive(buffer);
                if(primljeno <= 0)
                {
                    PrekiniVezu(s);
                    return;
                }

                string tekst = Encoding.UTF8.GetString(buffer, 0, primljeno);
                sesija.Bafer.Append(tekst);

                //provera dak ima ceo red

                string sadrzaj = sesija.Bafer.ToString();
                if (sadrzaj.Contains("\n"))
                {
                    string[] linije = sadrzaj.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var linija in linije)
                    {
                        IzvrsiLogiku(s, sesija, linija.Trim());
                    }
                    sesija.Bafer.Clear();
                }
            }
            catch
            {
                PrekiniVezu(s);
            }
        }

        private void IzvrsiLogiku(Socket s, KlijentSesija sesija,string linija)
        {
            switch (sesija.Faza)
            {
                case "CEKA_NADIMAK":
                    if(linija.StartsWith("NADIMAK "))
                    {
                        sesija.Nadimak = linija.Substring(8).Trim();
                        string lista = string.Join(",", _serverManager.Serveri.Keys);
                        Posalji(s, "SERVERI : " + lista);
                        sesija.Faza = "CEKA_SERVER";
                    }
                    break;

                case "CEKA_SERVER":
                    if (linija.StartsWith("SERVER "))
                    {
                        sesija.IzabraniServer = linija.Substring(7).Trim();
                        if (_serverManager.Serveri.ContainsKey(sesija.IzabraniServer))
                        {
                            var kanali = _serverManager.Serveri[sesija.IzabraniServer].Select(k => k.Naziv);
                            Posalji(s, "KANALI : " + string.Join(",", kanali));
                            sesija.Faza = "CEKA_KANAL";
                        }
                        else { Posalji(s, "ERROR ne postoji server"); PrekiniVezu(s); }
                    }
                    break;

                case "CEKA_KANAL":
                    if (linija.StartsWith("KANAL "))
                    {
                        sesija.OdabraniKanal = linija.Substring(6).Trim();
                        sesija.Faza = "CEKA_PORUKU";
                    }
                    break;

                case "CEKA_PORUKU":
                    if (linija.StartsWith("PORUKA "))
                    {
                        string poruka = linija.Substring(7).Trim();
                        string vreme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine($"[{vreme}]-[{sesija.IzabraniServer}]:[{sesija.OdabraniKanal}]:[{poruka}]-[{sesija.Nadimak}]");

                        Posalji(s, "Ok");
                        PrekiniVezu(s); // Po tvom klijentu, ovde je kraj sesije
                    }
                    break;
            }
        }

        private void Posalji(Socket s, string tekst)
        {
            s.Send(Encoding.UTF8.GetBytes(tekst + "\n"));
        }


        private void PrekiniVezu(Socket s)
        {
            _sesije.Remove(s);
            _sviSocketi.Remove(s);
            s.Close();
        }
    }
}
