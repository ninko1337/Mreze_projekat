namespace ClassLibrary
{
    public class Sifrovanje
    {
        public string Sifruj(string text, int key)
        {
            char ch;
            char[] data = text.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                ch = data[i];
                if (ch >= 'a' && ch <= 'z')
                {

                    ch = (char)((ch - 'a' + key) % 26 + 'a');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    ch = (char)((ch - 'A' + key) % 26 + 'A');
                }
                else
                {
                    ch = (char)((ch - '0' + key) % 10 + '0');
                }
                
                data[i] = ch;
            }
            text = new string(data);
            return text;
        }
        public string Desifruj(string tekst, int key)
        {
            char ch;
            char[] data = tekst.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                ch = data[i];
                if (ch >= 'a' && ch <= 'z')
                {

                    ch = (char)((ch - 'a' - key) % 26 + 'a');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    ch = (char)((ch - 'A' - key) % 26 + 'A');
                }
                else
                {
                    ch = (char)((ch - '0' - key) % 10 + '0');

                }
                data[i] = ch;
            }
            tekst = new string(data);
            return tekst;
        }
    }
    



    }
