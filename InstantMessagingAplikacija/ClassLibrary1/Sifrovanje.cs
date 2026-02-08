using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class Sifrovanje
    {
        public static string Sifruj(string text, int key)
        {
            char[] data = text.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                char ch = data[i];
                if (ch >= 'a' && ch <= 'z')
                {
                    data[i] = (char)((ch - 'a' + key) % 26 + 'a');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    data[i] = (char)((ch - 'A' + key) % 26 + 'A');
                }
                else if (ch >= '0' && ch <= '9') // Šifruje i brojeve ako su baš cifre
                {
                    data[i] = (char)((ch - '0' + key) % 10 + '0');
                }
                // Ako je razmak ili znak, ne dira ga (nema više else koji kvari sve)
            }
            return new string(data);
        }

        public static string Desifruj(string tekst, int key)
        {
            char[] data = tekst.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                char ch = data[i];
                if (ch >= 'a' && ch <= 'z')
                {
                    // Dodajemo 26 da rezultat (ch-a-key) ne bi bio negativan pre %
                    data[i] = (char)((ch - 'a' - (key % 26) + 26) % 26 + 'a');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    data[i] = (char)((ch - 'A' - (key % 26) + 26) % 26 + 'A');
                }
                else if (ch >= '0' && ch <= '9')
                {
                    // Dodajemo 10 za cifre
                    data[i] = (char)((ch - '0' - (key % 10) + 10) % 10 + '0');
                }
            }
            return new string(data);
        }
    }




}

