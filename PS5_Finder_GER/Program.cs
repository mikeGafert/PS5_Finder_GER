using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/***************************************************************************************
*    Title: PS 5 Finder Germany
*    Author: Mike Gafert
*    Date: 06.07.2021
*    Time: 11:00
*    Code version: 1.3.68
*    Availability: https://github.com/Gafert-IT/PS5_Finder_GER
*    License: GNU General Public License v3.0
*
***************************************************************************************/

namespace PS5_Finder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Dokumentenpfade für die Ordner und Files festlegen und die Ordner ggf. erstellen
            string myDocumentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Systempfad zu EigeneDokumente 
            string programmFolderPath = Path.Combine(myDocumentsFolderPath, "PS5 Finder"); // Pfad zum Programmordner
            if (!Directory.Exists(programmFolderPath)) { Directory.CreateDirectory(programmFolderPath); } // Falls er nicht existiert, erstellen
            string websiteCodePath = Path.Combine(programmFolderPath, "Webseitencode"); // Pfad zum Programmordner
            if (!Directory.Exists(websiteCodePath)) { Directory.CreateDirectory(websiteCodePath); } // Falls er nicht existiert, erstellen
            string logtxtPath = Path.Combine(programmFolderPath, "log.txt"); // Pfad zur allgemeinen Log datei
            string errorLogtxtPath = Path.Combine(programmFolderPath, "errorlog.txt"); // Pfad zur Fehler Log Datei
            string durchlaeufePath = Path.Combine(programmFolderPath, "durchlaeufe.txt"); // Pfad zur Durchläufe datei
            string botLogPath = Path.Combine(programmFolderPath, "botlog.txt"); // Pfad zur BotLog datei 
            string negativeKeywordsPath = ".\\Ressources\\NegativeKeyWords.txt"; // Pfad zur KeyWords datei
            string AmazonKeywordsPath = ".\\Ressources\\AmazonKeyWords.txt"; // Pfad zur KeyWords datei
            string userAgentsRessourcesPath = ".\\Ressources\\user-agents.txt"; // Pfad zur KeyWords datei
            string userAgentsLogPath = Path.Combine(programmFolderPath, "user-agentslog.txt"); // Pfad zur userAgents datei
            string urlFilePath = ".\\Ressources\\URLs.txt"; // Pfad zur URL datei
            string zugangsdatenAmazonPath = ".\\Ressources\\PwUnAmazon.txt"; // Pfad zur Zugangsdaten datei

            // Dateien einlesen, die beim Programmstart gelesen werden
            string[,] userAgentsArray = Extensions.getUserAgent(userAgentsRessourcesPath, userAgentsLogPath);

            // Konsolen Fenstergröße festlegen, Breite: Standard, Höhe: 35
#pragma warning disable CA1416 // Fehlermeldung der Plattformkompatibilität deaktivieren
            Console.SetWindowSize(Console.WindowWidth, 25);
#pragma warning restore CA1416 // Fehlermeldung der Plattformkompatibilität aktivieren     

            // UserInteraktion
            Console.Write("Wieviele Minuten soll die Schleife zwischen den Durchläufen pausieren? ");
            int pause = Convert.ToInt32(Console.ReadLine()) * 60000;

            Console.Write("Wie oft soll das Programm bei einem Fund piepen? ");
            int piepen = Convert.ToInt32(Console.ReadLine());

            // Autobuy abfragen
            Console.Write("Möchtest du auf Amazon.de die \"autobuy\" Funktion nutzen? (j/n): ");
            char autobuyNutzen = Console.ReadLine().ToLower()[0];
            //char autobuyNutzen = 'n';

            // Benutzernamen und Passwort abfragen
            string[,] zugangsdatenUserEingabe = new string[1, 2];
            char zugangsdatenDateiNutzen = 'a';
            if (autobuyNutzen == 'j')
            {
                Console.Write("Hast du deine Zugangsdaten bereits in der \"Zuzgangsdaten\" Datei gespeichert? (j/n): ");
                zugangsdatenDateiNutzen = Console.ReadLine().ToLower()[0];
                if (zugangsdatenDateiNutzen == 'n')
                {
                    Console.Write("Bitte gebe deine Amazon.de E-Mail adresse ein: ");
                    zugangsdatenUserEingabe[0, 0] = Console.ReadLine();
                    Console.Write("Bitte gebe deine Amazon.de Passwort ein: ");
                    zugangsdatenUserEingabe[0, 1] = Console.ReadLine();
                }
            }

            Console.Clear();

            // HTTP Client erstellen
            var handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(30);
            try // Fängt generelle Fehler ab
            {
                // Dauerhafte Schleife. Ja, ich will das wirklich so!            
                while (true)
                {
                    // Bisherige Durchläufe auslesen und in die Variable schreiben
                    int durchlauf = Extensions.ReadSingleEntryFromFile(durchlaeufePath);

                    // Datum und Zeit des Durchlaufs in die Log Datei schreiben
                    using (StreamWriter sw = new StreamWriter(logtxtPath, true))
                    {
                        DateTime date1 = DateTime.Now;
                        sw.WriteLine($"{durchlauf}. Durchlauf um: {date1}:");
                    }

                    // Jedes Objekt aus der WebseitenListe überprüfen                
                    for (int i = 0; i < File.ReadAllLines(urlFilePath).Length; i++)
                    {
                        // Webseitenliste erstellen und während der Laufzeit neu einlesen
                        List<Webseite> WebseitenListe = Extensions.ReadURLFile(urlFilePath);

                        // Inaktive URLs überspringen
                        if (WebseitenListe[i].Aktiv == false)
                        {
                            Console.WriteLine($"{i + 1}.Seite inaktiv: {WebseitenListe[i].Name}\tModell: {WebseitenListe[i].Modell}");
                            continue;
                        }

                        // Keyword aus der KeyWords.txt während der Laufzeit neu einlesen und in ein Array laden
                        string[] negativeKeyWords = Extensions.ReadFileToArray(negativeKeywordsPath);

                        // Konsolen-Ausgabe der angefragten Webseite
                        Console.Write($"{i + 1}.Seite gestartet: {WebseitenListe[i].Name}\tModell: {WebseitenListe[i].Modell}\tVerfügbar: ");

                        // Leeren String für die HTML daten der Webseite erzeugen bzw. bei jedem weitern Durchlauf leeren
                        string webData = "";

                        // Rückspungpunkt für einen retry mit einem anderen User-Agent
                        int retry = 0;
                    retry:

                        // Zufälligen User-Agent auswählen und Request Header Optionen setzen
                        client.DefaultRequestHeaders.Clear();
                        Random random = new Random();
                        int randomIndex = random.Next(userAgentsArray.Length / 2);
                        string userAgent = userAgentsArray[randomIndex, 1];
                        client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                        //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36"); // Statischer User-Agent
                        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        client.DefaultRequestHeaders.Add("Referer", "https://www.google.de/");
                        client.DefaultRequestHeaders.ConnectionClose = true;

                        try
                        {
                            // Webseite abfragen
                            HttpResponseMessage response = await client.GetAsync(WebseitenListe[i].Url);
                            response.EnsureSuccessStatusCode();
                            // HTML Daten laden
                            webData = await response.Content.ReadAsStringAsync();

                            // Statistische Werte für den verwendeten UserAgent in die UserAgentLogfile schreiben
                            Extensions.writeUserAgentStatsLog(userAgentsLogPath, userAgentsArray, i, WebseitenListe, randomIndex);
                        }
                        // Falls die Webseite nicht ausgelesen werden kann
                        catch (HttpRequestException ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            switch (ex.StatusCode)
                            {
                                case HttpStatusCode.Forbidden:
                                case HttpStatusCode.NotAcceptable:
                                    if (retry == 0)
                                    {
                                        retry++;
                                        goto retry;
                                    }
                                    Console.WriteLine(ex.StatusCode);
                                    Extensions.ErrorCodeNine(WebseitenListe, i);
                                    break;
                                case HttpStatusCode.TooManyRequests:
                                case HttpStatusCode.BadGateway:
                                case HttpStatusCode.BadRequest:
                                case HttpStatusCode.ServiceUnavailable:
                                    Console.WriteLine(ex.StatusCode);
                                    Extensions.ErrorCodeNine(WebseitenListe, i);
                                    break;
                                default:
                                    Console.WriteLine("kann aufgrund eines sonstigen Fehlers nicht ermittelt werden -> errorlog.txt");
                                    Extensions.ErrorCodeNine(WebseitenListe, i);
                                    break;
                            } 

                            // Eintrag des des fehlers in die errorlog.txt                                
                            Extensions.WriteErrorLog(WebseitenListe, errorLogtxtPath, i, ex);
                            continue;
                        }
                        catch (TaskCanceledException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Extensions.ErrorCodeNine(WebseitenListe, i);
                            Extensions.WriteErrorLog(WebseitenListe, errorLogtxtPath, i, ex);
                            continue;
                        }
                        Console.ForegroundColor = ConsoleColor.White;

                        // Den erhaltenen Inhalt der Webseite in eine txt Datei schrieben
                        Extensions.writeWebsiteToTxtFile(websiteCodePath, i, WebseitenListe, webData);

                        // reCaptcha erkennung (Funktioniert nicht)
                        //Extensions.findCaptcha(WebseitenListe, i, webData);

                        // Jetzt wird jeder Webseiten-string auf die Schlüsselwörter durchsucht
                        // Falls eines der Schlüsselwörter gefunden wird (true) wird das Ergebnis negiert (false)
                        // und in das und entsprechend in das vorhandene Objekt geschrieben                    
                        bool success = false; // variable für einen erfolgreichen autobuy Vorgang
                        switch (WebseitenListe[i].Name.ToLower())
                        {
                            case "amazon":
                                string[] amazonKeyWords = Extensions.ReadFileToArray(AmazonKeywordsPath);

                                WebseitenListe[i].Verfuegbar = WebsiteHandlerAmazon.CheckWebsite(amazonKeyWords, webData, botLogPath);

                                //WebsiteHandlerAmazon.CheckWebsiteWithHTMLAgilityPack(websiteCodePath, i, WebseitenListe, negativeKeyWords, webData, userAgent);

                                success = WebsiteHandlerAmazon.StartAutobuy(zugangsdatenAmazonPath, piepen, autobuyNutzen, zugangsdatenUserEingabe, zugangsdatenDateiNutzen, i, WebseitenListe, success);

                                break;

                            case "gamestop":
                                WebseitenListe[i].Verfuegbar = WebsiteHandlerGamestop.CheckWebsite(negativeKeyWords, webData);
                                break;

                            case "spielegrotte":
                                WebseitenListe[i].Verfuegbar = WebsiteHandlerSpielegrotte.CheckWebsite(negativeKeyWords, webData);
                                break;

                            case "müller":
                                WebseitenListe[i].Verfuegbar = WebsiteHandlerMueller.CheckWebsite(negativeKeyWords, webData);
                                break;

                            case "media markt":
                            case "saturn":
                                WebseitenListe[i].Verfuegbar = WebsiteHandlerMMS.CheckWebsite(negativeKeyWords, webData);   
                                break;

                            default:
                                WebseitenListe[i].Verfuegbar = !webData.Contains(negativeKeyWords, StringComparison.CurrentCulture);
                                break;
                        }

                        // Falls der Artikel verfügbar ist und nicht automatisch gekauft wurde
                        // wird die entsprechende Webseite geöffnet
                        if (WebseitenListe[i].Verfuegbar && !success)
                        {
                            // Es wird ein akustisches Signal in Form von Piepen abgespielt
                            for (int j = 0; j < piepen; j++)
                            {
                                Console.Beep();
                            }

                            switch (WebseitenListe[i].Name.ToLower())
                            {
                                case "media markt":
                                case "saturn":
                                    string[] subs = WebseitenListe[i].Url.Split('?'); // Zerlegt die Zeile um die Affiliate
                                                                                      // Links nicht öffnen zu müssen
                                    Extensions.OpenUrl(subs[0]);                                    
                                    break;
                                default:
                                    Extensions.OpenUrl(WebseitenListe[i].Url);
                                    break;
                            }                            
                        }                        

                        // Eintrag des Ergebnisses für diese Webseite in die log.txt
                        Extensions.WriteLogfile(WebseitenListe, logtxtPath, i);

                        // Ausgabe des Ergebnisses dieser Webseite
                        Console.WriteLine(WebseitenListe[i]);

                        // Bei MMS muss 10 Sekunden zwischen den Abfragen gewartet werden                        
                        switch (WebseitenListe[i].Name.ToLower())
                        {
                            case "media markt":
                            case "saturn":
                                Console.WriteLine("Programm wartet 10 Sekunden...");
                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                                Thread.Sleep(10000);

                                break;
                            default:
                                break;
                        }

                        // Webseitenliste leeren, da sie zu Beginn der nächsten schleife neu eingelesen wird
                        WebseitenListe.Clear();
                    }
                    // Endausgabe der Abfage
                    DateTime dateEnde = DateTime.Now;

                    Console.WriteLine($"\n\n\t{durchlauf}. Durchlauf beendet um: {dateEnde}" +
                        $"\n\tDas Programm piept {piepen} mal bei einem Fund." +
                        $"\n\tDer nächste Durchlauf startet in {pause / 60000} Minute(n).");

                    // Durchlauf erhöhen und Logfile aktualisieren
                    durchlauf++;
                    Extensions.WriteSingleIntToFile(durchlaeufePath, durchlauf);
                    Thread.Sleep(pause);
                    Console.Clear();
                    Console.Beep();
                }
            }
            // Fängt generelle Fehler ab            
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(errorLogtxtPath, true))
                {
                    DateTime dateError = DateTime.Now;
                    sw.WriteLine($"{dateError}\n{ex}");
                }
            }
            
        }
    }
}