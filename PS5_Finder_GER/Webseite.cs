using System;
using System.Threading.Tasks;

namespace PS5_Finder
{
    public class Webseite
    {
        // Eigenschaften
        public bool Aktiv;
        public string Name;
        public string Modell;
        public string Url;
        public bool Verfuegbar;
        public string InStockLabel;
        public string OutOfStockLabel;
        public string DetectedAsBotLabel;

        // Konstruktor
        public Webseite(bool aktiv, string name, string modell, string url, bool verfuegbar)
        {
            this.Aktiv = aktiv;
            this.Name = name;
            this.Modell = modell;
            this.Url = url;
            this.Verfuegbar = verfuegbar;
        }        

        public override string ToString()
        {
            if (!Verfuegbar)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return $"Nein\nURL: {Url}";                
            }          
            Console.ForegroundColor = ConsoleColor.Green;
            return $"JA!\nURL: {Url}";            
            //return $"{Name} {Modell} {Url}";
        }
    }
}

// 29c189127442fa05a1cfad3708681769faf74938