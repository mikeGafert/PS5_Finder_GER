using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;

namespace PS5_Finder
{
    static class WebsiteHandlerAmazon
    {
        public static bool CheckWebsite(string webData, string botLogPath)
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

        public static bool StartAutobuy(string zugangsdatenAmazonPath, int piepen, char autobuyNutzen, string[,] zugangsdatenUserEingabe, char zugangsdatenDateiNutzen, int i, List<Webseite> WebseitenListe, bool success)
        {
            if (WebseitenListe[i].Verfuegbar && autobuyNutzen == 'j')
            {
                if (zugangsdatenDateiNutzen == 'j')
                {
                    // Zugangsdaten einlesen
                    if (!File.Exists(zugangsdatenAmazonPath))
                    {
                        using (StreamWriter sw = new StreamWriter(zugangsdatenAmazonPath))
                        {
                            sw.Write("Username,Password");
                        }                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nBitte erst deine Zugangsdaten in ..\\Eigene Dokumente\\PS5 Finder\\PwUnAmazon.txt eingeben.");
                        Console.ForegroundColor = ConsoleColor.White;
                        success = false;
                    }
                    // Autobuy nur starten, wenn Zugangsdaten gefunden wurden
                    else
                    {
                        string[,] zugangsdatenAmazon = Extensions.ReadFileTo2DArray(zugangsdatenAmazonPath);

                        for (int j = 0; j < File.ReadAllLines(zugangsdatenAmazonPath).Length; j++)
                        {
                            Autobuyer Amazon = new();
                            success = Amazon.autobuyAmazonDE(WebseitenListe[i].Url, zugangsdatenAmazon[j, 0], zugangsdatenAmazon[j, 1], piepen);
                        }
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
            string[] KeyWordsArray = new string[] { "Abholbereit", "Lieferung", "In den Warenkorb", "Schützen Sie Ihr Gerät mit zusätzlichen Leistungen:" };
            bool verfuegbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            verfuegbar = !webData.Contains("Dieser Artikel ist aktuell nicht verfügbar.", StringComparison.CurrentCulture);
            return verfuegbar;
        }
    }

    static class WebsiteHandlerAlternate
    {
        public static bool CheckWebsite(string[] negativeKeyWords, string webData, string userAgent, string userAgentsAlternateLogPath)
        {
            string[] KeyWordsArray = new string[] { "Auf Lager", "In den Warenkorb", "Aktion verfügbar" };
            bool verfuegbar = webData.Contains(KeyWordsArray, StringComparison.CurrentCulture);
            verfuegbar = !webData.Contains("Artikel kann derzeit nicht gekauft werden", StringComparison.CurrentCulture);

            using (StreamWriter sw = new StreamWriter(userAgentsAlternateLogPath, true))
            {
                DateTime dateTime = DateTime.Now;
                sw.WriteLine($"{dateTime}: {userAgent}");
            }

            return verfuegbar;
        }
    }
}
