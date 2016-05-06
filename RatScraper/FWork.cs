using RatScraper.VisualComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RatScraper
{
    public partial class FWork : MyForm
    {
        public enum WorkType { Update };

        private WorkType workType;

        public FWork(FMain mainForm, WorkType workType)
            : base(mainForm)
        {
            InitializeComponent();
            this.workType = workType;
        }

        private void FWork_Load(object sender, EventArgs e)
        {
            switch (this.workType)
            {
                case WorkType.Update:
                    this.titleLabel1.TextSubtitle = "Updating the database";
                    this.StartBackgroundWorker(update_DoWork, update_ProgressChanged, update_RunWorkerCompleted, this.mainForm.Database);
                    break;
                default:
                    this.titleLabel1.TextSubtitle = "Not sure what we're doing though";
                    break;
            }
            this.RegisterControlsToMoveForm(this.titleLabel1);
        }

        private void StartBackgroundWorker(DoWorkEventHandler doWork, ProgressChangedEventHandler progressChanged, RunWorkerCompletedEventHandler completed, object argument)
        {
            this.workBW.DoWork += doWork;
            this.workBW.ProgressChanged += progressChanged;
            this.workBW.RunWorkerCompleted += completed;
            this.workBW.RunWorkerAsync(argument);
        }

        private void update_DoWork(object sender, DoWorkEventArgs e)
        {
            Database database = e.Argument as Database;
            database.Stops.Clear();
            database.Routes.Clear();
            List<KeyValuePair<string, string>> routePages = new List<KeyValuePair<string, string>>();

            workBW.ReportProgress(0, @"Downloading http://www.ratbv.ro/trasee-si-orare/...");
            WebClient web = new WebClient();
            web.DownloadFile(@"http://www.ratbv.ro/trasee-si-orare/", Paths.DownloadsFolder + "trasee si orare.html");

            workBW.ReportProgress(0, @"Processing http://www.ratbv.ro/trasee-si-orare/...");
            Match routeSectionMatch = Regex.Match(File.ReadAllText(Paths.DownloadsFolder + "trasee si orare.html"), @"<div class=""box continut-pagina"">.*</div>", RegexOptions.Singleline);
            string routeSection = routeSectionMatch.Value;

            MatchCollection routeMatches = Regex.Matches(routeSection, @"<a class=""linia.*?>Linia.*?</a>", RegexOptions.Singleline);

            foreach (Match match in routeMatches)
            {
                string value = match.Value;

                string name = Regex.Match(value, @">.*?<", RegexOptions.Singleline).Value;
                name = name.Substring(1, name.Length - 2);
                string href = Regex.Match(value, @"href="".*?\""", RegexOptions.Singleline).Value;
                href = href.Substring(0, href.Length - 1).Substring(href.IndexOf('"') + 1);

                routePages.Add(new KeyValuePair<string, string>(name, @"http://www.ratbv.ro" + href));
            }

            Console.WriteLine("\n" + routePages.Count + " route pages:");
            foreach (KeyValuePair<string, string> route in routePages)
                Console.WriteLine(string.Format(" * {0} ({1})", route.Key, route.Value));

            for (int iRoutePage = 0; iRoutePage < routePages.Count; iRoutePage++)
            {
                workBW.ReportProgress(0, @"Downloading and processing " + string.Format("{0} ({1})", routePages[iRoutePage].Key, routePages[iRoutePage].Value) + "...");
                DecodeTestament(web, database, routePages[iRoutePage], false, false);
            }

            for (int iS = 0; iS < database.Stops.Count - 1; iS++)
                for (int jS = iS + 1; jS < database.Stops.Count; jS++)
                    if (database.Stops[iS].Name.CompareTo(database.Stops[jS].Name) > 0)
                        database.Stops.SwapItemsAtPositions(iS, jS);

            workBW.ReportProgress(0, @"Saving database to file...");
            string saveResult = database.SaveDatabase(Paths.DatabaseFile);
            if (!saveResult.Equals(string.Empty))
                throw new ApplicationException(saveResult);
        }

        private void update_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusIV.TextText = e.UserState as string;
        }

        private void update_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusIV.TextText = "Done.";
            closeT.Enabled = true;
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private string GetWebpageText(WebClient client, string url, string filePath, bool forceDownload, bool throwErrors)
        {
            try
            {
                if (forceDownload || !File.Exists(filePath))
                    new WebClient().DownloadFile(url, filePath);
                return File.ReadAllText(filePath);
            }
            catch (Exception E)
            {
                if (throwErrors)
                    throw E;
                else
                    Console.WriteLine(E.ToString());
                return E.ToString();
            }
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private string GetNewTestamentStationIDFromURL(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts[parts.Length - 1];
        }

        private HalfRoute DecodeNewTestament(WebClient client, Database database, KeyValuePair<string, string> routeInfo, string halfRoutePhase, bool forceDownload, bool throwErrors)
        {
            workBW.ReportProgress(0, "Downloading and processing (new testament) " + routeInfo.Key + " (" + halfRoutePhase + ")...");
            if (!Directory.Exists(Paths.DownloadsFolder + routeInfo.Key))
                Directory.CreateDirectory(Paths.DownloadsFolder + routeInfo.Key);
            string file = GetWebpageText(client,
                string.Format(@"{0}timetable/", routeInfo.Value.Replace("/tour", "/" + halfRoutePhase)),
                string.Format(@"{0}{1}\{2} main.html", Paths.DownloadsFolder, routeInfo.Key, halfRoutePhase), forceDownload, throwErrors);

            List<Tuple<string, string, bool>> stopPages = new List<Tuple<string, string, bool>>();
            Match stopListText = Regex.Match(file, @"<div class=""box butoane-statii"">.*?</div>", RegexOptions.Singleline);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stopListText.Value);
            foreach (XmlNode stopNode in xml.SelectNodes("div/a"))
            {
                stopPages.Add(new Tuple<string, string, bool>(stopNode.InnerText, @"http://www.ratbv.ro" + stopNode.Attributes["href"].Value, stopNode.Attributes["class"] != null && stopNode.Attributes["class"].Value.Equals("active")));
                database.AddUniqueStop(stopPages.Last().Item1, GetNewTestamentStationIDFromURL(stopPages.Last().Item2));
            }

            HalfRoute halfRoute = new HalfRoute("");
            foreach (Tuple<string, string, bool> stopPage in stopPages)
            {
                string stopPageText = GetWebpageText(client, stopPage.Item2, string.Format(@"{0}{1}\{2} {3}.html", Paths.DownloadsFolder, routeInfo.Key, halfRoutePhase, GetNewTestamentStationIDFromURL(stopPage.Item2)), forceDownload, throwErrors);
                Match stopTableMatch = Regex.Match(stopPageText, @"<div class=""box tabel-statii"">.*?</div>", RegexOptions.Singleline);

                xml.LoadXml(stopTableMatch.Value.Replace("&nbsp;", ";").Replace("&nbsp", ";"));
                RouteStop routeStop = new RouteStop(database.Stops.GetItemByID(GetNewTestamentStationIDFromURL(stopPage.Item2)));
                foreach (XmlNode wdcNode in xml.SelectNodes("div/table/thead/tr/td"))
                    if (!wdcNode.InnerText.Trim().ToUpperInvariant().Equals("ORA"))
                        routeStop.WeekDayCategories.Add(new WeekDayCategory(WeekDayCategory.ParseCategoryType(wdcNode.InnerText.Trim())));

                foreach (XmlNode hourTrNode in xml.SelectNodes("div/table/tr"))
                {
                    XmlNodeList hourTrColumnTds = hourTrNode.SelectNodes("td");
                    if (hourTrColumnTds.Count != routeStop.WeekDayCategories.Count + 1)
                        continue;
                    int hour = Int32.Parse(hourTrColumnTds[0].InnerText);
                    for (int iWDC = 0; iWDC < routeStop.WeekDayCategories.Count; iWDC++)
                    {
                        string minutes = hourTrColumnTds[iWDC + 1].InnerText.Replace("\n", "").Replace(" ", "");
                        foreach (string minute in minutes.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            routeStop.WeekDayCategories[iWDC].StopTimes.Add(new StopTime(new TimeSpan(hour, Int32.Parse(minute.Replace("*", "")), 0), minute.Contains('*')));
                    }
                }

                halfRoute.Add(routeStop);
            }

            if (halfRoute.Count > 0)
                halfRoute.Name = halfRoute[0].Stop.Name + " - " + halfRoute[halfRoute.Count - 1].Stop.Name;
            return halfRoute;
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private HalfRoute DecodeOldTestament(WebClient client, Database database, KeyValuePair<string, string> routeInfo, string halfRoutePhase, bool forceDownload, bool throwErrors)
        {
            try
            {
                workBW.ReportProgress(0, "Downloading " + routeInfo.Key + " (" + halfRoutePhase + ") (old testament)...");
                if (!Directory.Exists(Paths.DownloadsFolder + routeInfo.Key))
                    Directory.CreateDirectory(Paths.DownloadsFolder + routeInfo.Key);

                string file = null, lineNameCase = null;

                XmlDocument xml = new XmlDocument();

                workBW.ReportProgress(0, "Downloading " + routeInfo.Key + " (" + halfRoutePhase + ") (2) (old testament)...");
                try
                {
                    lineNameCase = routeInfo.Key.Replace("Linia ", "").ToLowerInvariant();
                    file = GetWebpageText(client,
                       string.Format(@"http://www.ratbv.ro/afisaje/{0}-{1}/div_list_ro.html", lineNameCase, halfRoutePhase),
                       string.Format(@"{0}{1}\{2} stationList.html", Paths.DownloadsFolder, routeInfo.Key, halfRoutePhase), forceDownload, throwErrors);
                }
                catch (Exception)
                {
                    lineNameCase = routeInfo.Key.Replace("Linia ", "").ToUpperInvariant();
                    file = GetWebpageText(client,
                        string.Format(@"http://www.ratbv.ro/afisaje/{0}-{1}/div_list_ro.html", lineNameCase, halfRoutePhase),
                        string.Format(@"{0}{1}\{2} stationList.html", Paths.DownloadsFolder, routeInfo.Key, halfRoutePhase), forceDownload, throwErrors);
                }

                /*string tempText = GetWebpageText(client,
                    string.Format(@"http://www.ratbv.ro/afisaje/{0}-{1}.html", lineNameCase, halfRoutePhase),
                    string.Format(@"{0}{1}\{2} main.html", Paths.DownloadsFolder, routeInfo.Key, halfRoutePhase), forceDownload, throwErrors);
                xml.LoadXml(tempText);
                Match pageTitleMatch = Regex.Match(file, @"<title>Linia.*?directi.*?</title>", RegexOptions.Singleline);
                xml.LoadXml(pageTitleMatch.Value);
                string halfRouteName = xml.ChildNodes[0].InnerText.Substring(xml.ChildNodes[0].InnerText.IndexOf("directi") + 9);*/

                workBW.ReportProgress(0, "Downloading " + routeInfo.Key + " (" + halfRoutePhase + ") (3) (old testament)...");
                List<Tuple<Stop, string>> stopPages = new List<Tuple<Stop, string>>();
                MatchCollection stopMatches = Regex.Matches(file, @"<a href=""line.*?"" target=""MainFrame"" onclick=.*?>.*?</a>", RegexOptions.Singleline);
                foreach (Match stopMatch in stopMatches)
                {
                    xml.LoadXml(stopMatch.Value);
                    string stopName = xml.ChildNodes[0].ChildNodes[0].InnerText.Trim();
                    database.AddUniqueStop(stopName);
                    Stop stop = database.Stops.GetItemByName(stopName);
                    stopPages.Add(new Tuple<Stop, string>(stop, string.Format(@"http://www.ratbv.ro/afisaje/{0}-{1}/{2}", lineNameCase, halfRoutePhase, xml.ChildNodes[0].Attributes["href"].Value)));
                }

                HalfRoute halfRoute = new HalfRoute(routeInfo.Value);
                foreach (Tuple<Stop, string> stopPage in stopPages)
                {
                    string stopPageText = GetWebpageText(client,
                        stopPage.Item2,
                        string.Format(@"{0}Linia {1}\{2} {3}.html", Paths.DownloadsFolder, lineNameCase, halfRoutePhase, stopPage.Item1.ID),
                        forceDownload, throwErrors);

                    RouteStop routeStop = new RouteStop(stopPage.Item1);

                    /*Match stopTableMatch = Regex.Match(stopPageText, @"<div class=""box tabel-statii"">.*?</div>", RegexOptions.Singleline);

                    xml.LoadXml(stopTableMatch.Value.Replace("&nbsp;", ";").Replace("&nbsp", ";"));
                    foreach (XmlNode wdcNode in xml.SelectNodes("div/table/thead/tr/td"))
                        if (!wdcNode.InnerText.Trim().ToUpperInvariant().Equals("ORA"))
                            stop.WeekDayCategories.Add(new WeekDayCategory(WeekDayCategory.ParseCategoryType(wdcNode.InnerText.Trim())));

                    foreach (XmlNode hourTrNode in xml.SelectNodes("div/table/tr"))
                    {
                        XmlNodeList hourTrColumnTds = hourTrNode.SelectNodes("td");
                        if (hourTrColumnTds.Count != stop.WeekDayCategories.Count + 1)
                            continue;
                        int hour = Int32.Parse(hourTrColumnTds[0].InnerText);
                        for (int iWDC = 0; iWDC < stop.WeekDayCategories.Count; iWDC++)
                        {
                            string minutes = hourTrColumnTds[iWDC + 1].InnerText.Replace("\n", "").Replace(" ", "");
                            foreach (string minute in minutes.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                stop.WeekDayCategories[iWDC].StopTimes.Add(new StopTime(new TimeSpan(hour, Int32.Parse(minute.Replace("*", "")), 0), minute.Contains('*')));
                        }
                    }*/

                    halfRoute.Add(routeStop);
                }

                return halfRoute;
            }
            catch (Exception E)
            {
                if (throwErrors)
                    throw E;
                else
                    Console.WriteLine(E.ToString());
                return null;
            }
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private void DecodeTestament(WebClient client, Database database, KeyValuePair<string, string> routeInfo, bool forceDownload, bool throwErrors)
        {
            HalfRoute outgoing = routeInfo.Value.Contains("route")
                ? DecodeNewTestament(client, database, routeInfo, "tour", forceDownload, throwErrors)
                : DecodeOldTestament(client, database, routeInfo, "dus", forceDownload, throwErrors);
            HalfRoute incoming = routeInfo.Value.Contains("route")
                ? DecodeNewTestament(client, database, routeInfo, "retour", forceDownload, throwErrors)
                : DecodeOldTestament(client, database, routeInfo, "intors", forceDownload, throwErrors);
            database.Routes.Add(new Route(routeInfo.Key.Substring(routeInfo.Key.IndexOf(' ') + 1), routeInfo.Key, outgoing, incoming));
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private void closeT_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
