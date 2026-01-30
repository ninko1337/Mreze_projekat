using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class ServerManager
    {
        public Dictionary<string, List<Kanal>> Serveri {  get; set; }
        public List<AktivnostKlijenta> Aktivnosti { get; set; }

        public ServerManager()
        {
            Serveri = new  Dictionary<string, List<Kanal>>();

            Aktivnosti = new List<AktivnostKlijenta>();

        }

        public void Konfigurisi()
        {
            while (true)
            {
                Console.WriteLine("=====KONFIGURACIJA SERVERA=====");
                Console.WriteLine("1. Dodaj server");
                Console.WriteLine("2. Dodaj kanal u postojeci server");
                Console.WriteLine("3. Prikazi konfiguraciju");
                Console.WriteLine("4. Izlaz iz konfiguracije");
                Console.WriteLine("Vas izbor : ");

                string izbor = Console.ReadLine();

                if (izbor == "1") DodajServer();
                else if (izbor == "2") DodajKanal();
                else if (izbor == "3") Prikazi();
                else if (izbor == "4") break;
                else Console.WriteLine("Nepostojeci izzbor");
            }

        }

        private void DodajServer()
        {
            Console.WriteLine("Unesite naziv novog servera : ");
            string naziv = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(naziv))
            {
                Console.WriteLine("Naziv ne sme biti prazan ");
                return;
            }

            naziv = naziv.Trim();

            if (Serveri.ContainsKey(naziv))
            {
                Console.WriteLine("Server sa tim imenom vec postoji");
                return;
            }

            Serveri[naziv] = new List<Kanal>();
            Console.WriteLine("Server dodat : " + naziv); 
        }

        private void DodajKanal()
        {
            if (Serveri.Count == 0)
            {
                Console.WriteLine("Prvo dodaj bar jedan server.");
                return;
            }

            Console.WriteLine("Unesi naziv servera u koji dodajes kanal : ");
            string serverIme = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(serverIme))
            {
                Console.WriteLine("Ime servera ne sme biti prazno.");
                return;
            }

            serverIme = serverIme.Trim();

            if (!Serveri.ContainsKey(serverIme))
            {
                Console.WriteLine("Server sa tim imenom ne postoji");
                return;
            }

            Console.WriteLine("Unesi naziv kanala koji zelis da dodas : ");
            string imeKanala = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(imeKanala))
            {
                Console.WriteLine("Ime kanala ne sme biti prazno.");
                return;
            }

            imeKanala = imeKanala.Trim();

            foreach (var k in Serveri[serverIme])
            {
                if (k.Naziv == imeKanala)
                {
                    Console.WriteLine("Kanal sa tim nazivom vec postoji");
                    return;
                }
            }

            Serveri[serverIme].Add(new Kanal(imeKanala));
            Console.WriteLine($"Dodat kanal {imeKanala} u server {serverIme}");
        }

        private void Prikazi()
        {
            Console.WriteLine("-----Trenutna konfiguracija-----");
            foreach (var s in Serveri)
            {
                Console.WriteLine($"Server : {s.Key} | Kanali : ");

                if(s.Value.Count == 0)
                {
                    Console.WriteLine("Nema");
                    continue;
                }

                for(int i = 0;i < s.Value.Count; i++)
                {
                    Console.Write(s.Value[i].Naziv);
                    if (i < s.Value.Count - 1)
                        Console.Write(", ");
                }
                Console.WriteLine();
            }
        }
    }
}
