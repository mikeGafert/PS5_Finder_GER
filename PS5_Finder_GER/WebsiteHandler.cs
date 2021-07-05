using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PS5_Finder
{
    static class WebsiteHandlerAmazon
    {
        public static bool CheckWebsite(string[] amazonKeyWords, string webData, string botLogPath)
        {
            bool verfügbar = false;
            bool alsBotErkanntBool = webData.Contains("api-services-support@amazon.com", StringComparison.CurrentCulture);
            if (alsBotErkanntBool)
            {
                int alsBotErkanntInt = Extensions.ReadSingleEntryFromFile(botLogPath);
                alsBotErkanntInt++;
                Extensions.WriteSingleIntToFile(botLogPath, alsBotErkanntInt);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Zum {alsBotErkanntInt}. Mal als Bot erkannt.");
                Console.ForegroundColor = ConsoleColor.White;
                verfügbar = false;
            }
            else
            {
                string[] KeyWordsArray = new string[] { "Auf Lager.", "In den Einkaufswagen", "Jetzt kaufen", "Kostenlose Lieferung" };
                verfügbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            }
            return verfügbar;
        }

        public static void CheckWebsiteWithHTMLAgilityPack(string websiteCodePath, int i, List<Webseite> WebseitenListe, string[] negativeKeyWords, string webData, string userAgent)
        {
            HtmlWeb web = new HtmlWeb();
            web.UserAgent = userAgent;
            var htmlDoc = web.Load(WebseitenListe[i].Url);
            HtmlNode[] nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='availability']").Where(x => x.InnerText.Contains("Auf Lager.")).ToArray();

            // Den erhaltenen Inhalt der Webseite in eine txt Datei schrieben
            string tempPath = Path.Combine(websiteCodePath, $"htmlDoc {WebseitenListe[i].Name} {WebseitenListe[i].Modell}.txt");
            using (StreamWriter sw = new StreamWriter(tempPath))
            {
                foreach (var item in nodes)
                {
                    sw.WriteLine(item);
                }

            }

            foreach (var item in nodes)
            {
                WebseitenListe[i].Verfuegbar = !webData.Contains(negativeKeyWords, StringComparison.CurrentCulture);
                if (WebseitenListe[i].Verfuegbar)
                {
                    break;
                }
            }
        }

        public static bool StartAutobuy(string zugangsdatenAmazonPath, int piepen, char autobuyNutzen, string[,] zugangsdatenUserEingabe, char zugangsdatenDateiNutzen, int i, List<Webseite> WebseitenListe, bool success)
        {
            if (WebseitenListe[i].Verfuegbar && autobuyNutzen == 'j')
            {
                if (zugangsdatenDateiNutzen == 'j')
                {
                    // Zugangsdaten einlesen            
                    string[,] zugangsdatenAmazon = Extensions.ReadFileTo2DArray(zugangsdatenAmazonPath);
                    for (int j = 0; j < File.ReadAllLines(zugangsdatenAmazonPath).Length; j++)
                    {
                        Autobuyer Amazon = new();
                        success = Amazon.autobuyAmazonDE(WebseitenListe[i].Url, zugangsdatenAmazon[j, 0], zugangsdatenAmazon[j, 1], piepen);
                    }
                }
                else
                {
                    Autobuyer Amazon = new();
                    success = Amazon.autobuyAmazonDE(WebseitenListe[i].Url, zugangsdatenUserEingabe[0, 0], zugangsdatenUserEingabe[0, 1], piepen);
                }
            }

            return success;
        }
    }

    static class WebsiteHandlerGamestop
    {
        public static bool CheckWebsite(string[] negativeKeyWords, string webData)
        {
            string[] KeyWordsArray = new string[] { "349", "399", "449", "499", "disk" };
            bool verfuegbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            return verfuegbar;
        }
    }

    static class WebsiteHandlerSpielegrotte
    {
        public static bool CheckWebsite(string[] negativeKeyWords, string webData)
        {
            string[] KeyWordsArray = new string[] { "neu_indenkorb_2.gif", "heute versand", "2-3 Werktage", "Bestell diesen Artikel in den nächsten", "und wir verschicken ihn ohne Mehrkosten noch heute, so dass er in der Regel morgen zugestellt wird." };
            bool verfuegbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            return verfuegbar;
        }
    }

    static class WebsiteHandlerMueller
    {
        public static bool CheckWebsite(string[] negativeKeyWords, string webData)
        {
            string[] KeyWordsArray = new string[] { "Abholung in der Filiale", "Lieferung nach Hause", "In den Warenkorb", "Voraussichtlich lieferbar" };
            bool verfuegbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            verfuegbar = !webData.Contains("Vielen Dank an alle, die eine PlayStation 5 bestellt haben.", StringComparison.CurrentCulture);
            return verfuegbar;
        }
    }

    static class WebsiteHandlerMMS
    {
        public static bool CheckWebsite(string[] negativeKeyWords, string webData)
        {
            //string[] KeyWordsArray = new string[] { "Abholung in der Filiale", "Lieferung nach Hause", "In den Warenkorb", "Voraussichtlich lieferbar" };
            bool verfuegbar = webData.Contains(negativeKeyWords, StringComparison.CurrentCulture);
            verfuegbar = !webData.Contains("Dieser Artikel ist aktuell nicht verfügbar.", StringComparison.CurrentCulture);
            return verfuegbar;
        }
    }
}
