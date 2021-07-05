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
                return $"Nein";
            }
            return $"JA!";
            //return $"{Name} {Modell} {Url}";
        }
    }
}
