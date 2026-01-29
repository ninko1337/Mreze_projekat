using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    internal class Poruka
    {
        public string Posaljilac {  get; set; }
        public string Vreme {  get; set; }

        public string Sadrzaj {  get; set; }
    }
}
