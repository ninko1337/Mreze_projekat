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
            Console.Write("Unesi ime ili nadimak : ");
            string ime = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(ime))
            {
                Console.WriteLine("Ime ili nadimak ne mogu biti prazni , unesi opet : ");
                ime = Console.ReadLine();
            }

            ime = ime.Trim();

            string resp = UdpClientService.Prijava("127.0.0.1",9000);
            Console.WriteLine("Od servera : " + resp);

            Console.ReadLine();
        }
    }
}
