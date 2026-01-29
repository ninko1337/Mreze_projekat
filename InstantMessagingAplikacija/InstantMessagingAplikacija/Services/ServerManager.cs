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
        public Dictionary<string , List<Kanal>> Serveri {  get; set; }
        public List<AktivnostKlijenta> Aktivnosti { get; set; }

        public ServerManager()
        {
            Serveri = new  Dictionary<string , List<Kanal>>();

            Aktivnosti = new List<AktivnostKlijenta>();

            Serveri["Main Server"] = new List<Kanal>
            {
                new Kanal("general"),

                new Kanal("random")
            };

            
        }
    }
}
