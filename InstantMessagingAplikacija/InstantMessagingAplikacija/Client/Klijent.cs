using ClassLibrary;
using Client.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Klijent
    {
        static void Main(string[] args)
        {
            Console.Write("Unesi ime ili nadimak : ");
            string ime = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(ime))
            {
                Console.WriteLine("Ime ili nadimak ne mogu biti prazni , unesi opet : ");
                ime = Console.ReadLine();
            }

            ime = ime.Trim();

            // udp prijava
            string resp = UdpClientService.Prijava("127.0.0.1",9000);
            Console.WriteLine("Od servera : " + resp);

            // izvlacenje tcp porta
            if(string.IsNullOrEmpty(resp) || !resp.Contains(":"))
            {
                Console.WriteLine("ERROR server nije poslao validan port");
                return;
            }

            int tcpPort = int.Parse(resp.Split(':')[1]);

            // istorija putanja
            string istorijaPath = "history.txt";

            // komunikacija sa tcp
            try
            {
                using(TcpClient tcpClient = new TcpClient("127.0.0.1",tcpPort))
                using(NetworkStream ns = tcpClient.GetStream())
                using(StreamReader reader = new StreamReader(ns,Encoding.UTF8))
                using(StreamWriter writer = new StreamWriter(ns,Encoding.UTF8) { AutoFlush = true})
                {
                    //saljem nadimak
                    writer.WriteLine("NADIMAK " + ime);

                    //prima listu servera
                    string serveriLinija = reader.ReadLine();
                    string[] serveri = serveriLinija.Substring(10).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    Console.WriteLine("\nDostupni serveri:");
                    foreach (var s in serveri) Console.WriteLine("- " + s);

                    Console.Write("Izaberi server (unesi naziv): ");
                    string izabraniServer = Console.ReadLine().Trim();

                    //saljem izbor servera
                    writer.WriteLine("SERVER " + izabraniServer);

                    //cuvanje na fajl
                    string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} | {ime} | Server: {izabraniServer}";
                    File.AppendAllLines(istorijaPath, new[] { entry });

                    //prima listu kanala
                    string kanaliLinija = reader.ReadLine();
                    string[] kanali = kanaliLinija.Substring(9).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    Console.WriteLine($"\nKanali na serveru '{izabraniServer}':");
                    foreach (var k in kanali) Console.WriteLine("- " + k);

                    Console.Write("Izaberi kanal (unesi naziv): ");
                    string izabraniKanal = Console.ReadLine().Trim();

                    //salje se izbor kanala
                    writer.WriteLine("KANAL " + izabraniKanal);

                    //salje se poruka
                    Console.Write("Unesi poruku: ");
                    string poruka = Console.ReadLine();
                    string[] nadimakCount = ime.Split(' ');
                    poruka = Sifrovanje.Sifruj(poruka, nadimakCount.Length);
                    writer.WriteLine("PORUKA " + poruka);

                    //odgovor servera
                    string status = reader.ReadLine();
                    Console.WriteLine("\nStatus od servera: " + status);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska pri tcp povezivanju - " + ex.ToString());
            }

            Console.WriteLine("\nKraj - pritisni ENTER za izlaz");
            Console.ReadLine();
        }
    }
}
