using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace PS5_Finder
{
    static class Extensions
    {
        // StreamReader
        public static string[] ReadFileToArray(string path)
        {
            string[] stringArray = new string[File.ReadAllLines(path).Length];
            using (StreamReader sr = new StreamReader(path))
            {
                stringArray = File.ReadAllLines(path); // Die Datei wird Zeile für Zeile ausgelsen               
            }

            return stringArray;
        }

        public static string[,] ReadFileTo2DArray(string path)
        {
            string[,] stringArray = new string[File.ReadAllLines(path).Length, 2];
            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; i < File.ReadAllLines(path).Length; i++)
                {
                    string zeile = sr.ReadLine(); // Die Datei wird Zeile für Zeile ausgelsen
                    string[] subs = zeile.Split(",", 2); // Zerlegt die Zeile 1 mal in maxmial 2 Teile
                    stringArray[i, 0] = subs[0];
                    stringArray[i, 1] = subs[1];
                }
            }
            return stringArray;
        }

        public static List<Webseite> ReadURLFile(string urlPath)
        {
            List<Webseite> WebseitenListe = new List<Webseite>();
            using (StreamReader sr = new StreamReader(urlPath))
            {
                while (!sr.EndOfStream) // weitermachen, solange noch nicht alle Daten ausgelesen wurden
                {
                    string zeile = sr.ReadLine(); // Die Datei wird Zeile für Zeile ausgelsen                    
                    string[] subs = zeile.Split(','); // Zerlegt die Zeile
                    if (subs[0] == "inaktiv")
                    {
                        subs[0] = "false";
                    }
                    else if (subs[0] == "aktiv")
                    {
                        subs[0] = "true";
                    }
                    WebseitenListe.Add(new Webseite(Convert.ToBoolean(subs[0]), subs[1], subs[2], subs[3], false));
                }
            }

            return WebseitenListe;
        }

        public static int ReadSingleEntryFromFile(string Path)
        {
            if (!File.Exists(Path))
            {
                // Create a file to write to.
                using StreamWriter sw = File.CreateText(Path);
            }
            using (StreamReader sr = new StreamReader(Path))
            {
                return Convert.ToInt32(sr.ReadLine());
            }
        }

        // Streamwriter
        public static void WriteErrorLog(List<Webseite> WebseitenListe, string errorLogtxtPath, int i, Exception ex)
        {
            using (StreamWriter sw = new StreamWriter(errorLogtxtPath, true))
            {
                DateTime dateError = DateTime.Now;
                sw.WriteLine($"{dateError}, {WebseitenListe[i].Name}\n{ex}");
            }
        }

        public static void WriteLogfile(List<Webseite> WebseitenListe, string logtxtPath, int i)
        {
            using (StreamWriter sw = new StreamWriter(logtxtPath, true))
            {
                sw.WriteLine($"{i + 1}.Seite gestartet: {WebseitenListe[i].Name}\tModell: {WebseitenListe[i].Modell}\tVerfügbar: {WebseitenListe[i]}");
            }
        }

        public static void WriteSingleIntToFile(string Path, int tempInt)
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(tempInt);
            }
        }

        public static void WriteUserAgentCounter(string userAgentsPath, string[,] userAgentsArray, int tempRandom)
        {
            int userAgentCount = Convert.ToInt32(userAgentsArray[tempRandom, 0]);

            userAgentCount++;

            userAgentsArray[tempRandom, 0] = userAgentCount.ToString();
            using (StreamWriter sw = new StreamWriter(userAgentsPath))
            {
                for (int j = 0; j < userAgentsArray.Length / 2; j++)
                {
                    sw.WriteLine($"{userAgentsArray[j, 0]},{userAgentsArray[j, 1]}");
                }
            }
        }

        public static void writeUserAgentStatsLog(string userAgentsPath, string[,] userAgentsArray, int i, List<Webseite> WebseitenListe, int randomIndex)
        {
            int userAgentCount = Convert.ToInt32(userAgentsArray[randomIndex, 0]);

            userAgentCount++;

            userAgentsArray[randomIndex, 0] = userAgentCount.ToString();
            using (StreamWriter sw = new StreamWriter(userAgentsPath))
            {
                for (int j = 0; j < userAgentsArray.Length / 2; j++)
                {
                    sw.WriteLine($"{userAgentsArray[j, 0]},{userAgentsArray[j, 1]}");
                }
            }

        }

        public static void writeWebsiteToTxtFile(string websiteCodePath, int i, List<Webseite> WebseitenListe, string webData)
        {
            string tempPath = Path.Combine(websiteCodePath, $"{WebseitenListe[i].Name} {WebseitenListe[i].Modell}.txt");
            using (StreamWriter sw = new StreamWriter(tempPath))
            {
                sw.WriteLine(webData);
            }
        }

        public static void writeUnPwHowToFile(string zugangsdatenUnPwHowToPath)
        {
            using (StreamWriter sw = new StreamWriter(zugangsdatenUnPwHowToPath))
            {
                sw.Write("## How to add Username and Password:\n\n" +
                            "## Open the \"PwUn\"-File related to the website you want to use autobuy\n" +
                            "## Add ONE Useraccount per line\n" +
                            "## seperate Username/E-Mail Address and Password by \",\"\n\n" +
                            "## for Example:\n" +
                            "user@mail.com, P@ssword\n" +
                            "user2@anotheremail.net, myP@ssword\n" +
                            "thebestuser, myveryownP@ssword");
            }
        }

        // andere Methoden
        public static string[,] getUserAgent(string userAgentsRessourcesPath, string userAgentsPath)
        {
            string[,] userAgentsArray = new string[File.ReadAllLines(userAgentsRessourcesPath).Length, 2];
            if (!File.Exists(userAgentsPath))
            {
                string[] tempArray = Extensions.ReadFileToArray(userAgentsRessourcesPath);

                using (StreamWriter sw = new StreamWriter(userAgentsPath))
                {
                    for (int i = 0; i < userAgentsArray.Length / 2; i++)
                    {
                        sw.WriteLine($"0,{tempArray[i]}");
                    }
                }
            }
            else
            {
                userAgentsArray = Extensions.ReadFileTo2DArray(userAgentsPath);
            }

            return userAgentsArray;
        }

        /// <summary>
        /// Gibt einen Wert zurück, der angibt ob ein- oder mehrere System.String-Objekte in dieser Zeichenfolge vorkommen.
        /// </summary>
        /// <param name="webseiteAlsString">Die Zeichenfolge in der gesucht werden soll.</param>
        /// <param name="suchbegriffe">Die zu suchenden Teilzeichenfolgen.</param>
        /// <param name="modus">Der Modus, wie die Teilzeichenketten mit dieser Zeichenfolge verglichen werden sollen.</param>        
        /// <returns>true wenn die Teilzeichenfolgen in den angegebenen Modi in dieser Zeichenfolge vorkommen, andernfalls false.</returns>
        public static bool Contains(this string webseiteAlsString, string[] suchbegriffe, StringComparison modus)
        {
            if (string.IsNullOrWhiteSpace(webseiteAlsString) || suchbegriffe.Length == 0)
                return false;
            foreach (string suchbegriff in suchbegriffe)
                if (webseiteAlsString.ToLower().IndexOf(suchbegriff.ToLower(), modus) > -1 && !string.IsNullOrWhiteSpace(suchbegriff))
                    return true;
            return false;
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public static void findCaptcha(List<Webseite> WebseitenListe, int i, string webData)
        {
            if (webData.Contains("captcha", StringComparison.CurrentCulture) && webData.Length < 15000) // Falls die Webseite aus weniger als 15000 Zeichen besteht, könnte es eine Captcha Seite sein
            {
                Console.Beep();
                Extensions.OpenUrl(WebseitenListe[i].Url);
                Console.WriteLine("vermutlich Captcha gefunden");
            }
        }

        public static void wait10SecForMMS(List<Webseite> WebseitenListe, int i)
        {// Bei MMS muss 10 Sekunden zwischen den Abfragen gewartet werden                        
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
        }

        public static void UrlDateiSchreiben(string urlFilePath, string UserURLsPath)
        {
            string[] tempStringArray = ReadFileToArray(urlFilePath);


            using (StreamWriter sw = new StreamWriter(UserURLsPath))
            {
                for (int i = 0; i < tempStringArray.Length; i++)
                {
                    sw.WriteLine(tempStringArray[i]);
                }
            }
        }

    }
}