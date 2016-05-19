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
            List<Tuple<string, string, Color>> routePages = new List<Tuple<string, string, Color>>();

            workBW.ReportProgress(0, @"Downloading http://www.ratbv.ro/trasee-si-orare/...");
            WebClient client = new WebClient();
            client.DownloadFile(@"http://www.ratbv.ro/trasee-si-orare/", Paths.DownloadsFolder + "trasee si orare.html");

            workBW.ReportProgress(0, @"Processing http://www.ratbv.ro/trasee-si-orare/...");
            Match routeSectionMatch = Regex.Match(File.ReadAllText(Paths.DownloadsFolder + "trasee si orare.html"), @"<div class=""box continut-pagina"">.*</div>", RegexOptions.Singleline);
            string routeSection = routeSectionMatch.Value;

            MatchCollection routeMatches = Regex.Matches(routeSection, @"<a class=""linia.*?>Linia.*?</a>.*?<span style=""color:.*?;"">", RegexOptions.Singleline);

            foreach (Match match in routeMatches)
            {
                string value = match.Value;

                string name = Regex.Match(value, @">.*?<", RegexOptions.Singleline).Value;
                name = name.Substring(1, name.Length - 2);
                string href = Regex.Match(value, @"href="".*?\""", RegexOptions.Singleline).Value;
                href = href.Substring(0, href.Length - 1).Substring(href.IndexOf('"') + 1);

                routePages.Add(new Tuple<string, string, Color>(name, @"http://www.ratbv.ro" + href, Color.White));
            }

            for (int iRoutePage = 0; iRoutePage < routePages.Count; iRoutePage++)
            {
                workBW.ReportProgress(0, @"Downloading and processing " + string.Format("{0} ({1})", routePages[iRoutePage].Item1, routePages[iRoutePage].Item2) + "...");
                DecodeTestament(client, database, routePages[iRoutePage], false, true);
            }

            string cssText = GetWebpageText(client, @"http://www.ratbv.ro/css/style.css", Paths.DownloadsFolder + "style.css", false, true);
            foreach (Route route in database.Routes)
            {
                string routeCssMatchRegex = @"\.continut-pagina +a\.linia" + route.ID.ToLowerInvariant() + @":before" + @"(, *\n\.continut-pagina +a\.linia.*?)*" + @" *\{\n *color: *\#.*?; *\n.*?\}";
                Match routeCssMatch = Regex.Match(cssText, routeCssMatchRegex);
                if (routeCssMatch.Success)
                {
                    string routeCssColor = routeCssMatch.Value.Substring(routeCssMatch.Value.IndexOf('#'));
                    routeCssColor = routeCssColor.Substring(0, routeCssColor.IndexOf(';'));
                    route.Color = ColorTranslator.FromHtml(routeCssColor.Substring(0, routeCssColor.Length));
                }
            }

            for (int iS = 0; iS < database.Stops.Count - 1; iS++)
                for (int jS = iS + 1; jS < database.Stops.Count; jS++)
                    if (database.Stops[iS].Name.CompareTo(database.Stops[jS].Name) > 0)
                        database.Stops.SwapItemsAtPositions(iS, jS);

            for (int iR = 0; iR < database.Routes.Count - 1; iR++)
                for (int jR = iR + 1; jR < database.Routes.Count; jR++)
                {
                    KeyValuePair<int, bool> iID = GetSortableRouteID(database.Routes[iR].ID), jID = GetSortableRouteID(database.Routes[jR].ID);
                    if (iID.Key == jID.Key ? Convert.ToInt32(iID.Value) > Convert.ToInt32(jID.Value) : iID.Key > jID.Key)
                        database.Routes.SwapItemsAtPositions(iR, jR);
                }

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

        private KeyValuePair<int, bool> GetSortableRouteID(string id)
        {
            if ("0123456789".Contains(id[id.Length - 1]))
                return new KeyValuePair<int, bool>(Int32.Parse(id), false);
            return new KeyValuePair<int, bool>(Int32.Parse(id.Substring(0, id.Length - 1)), true);
        }

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
            /*try
            {*/
            workBW.ReportProgress(0, "Downloading " + routeInfo.Key + " (" + halfRoutePhase + ") (old testament)...");
            if (!Directory.Exists(Paths.DownloadsFolder + routeInfo.Key))
                Directory.CreateDirectory(Paths.DownloadsFolder + routeInfo.Key);

            string file = null, lineNameCase = null;

            XmlDocument xml = new XmlDocument();

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

                Match stopTableMatch = Regex.Match(stopPageText, @"<body>.*</body>", RegexOptions.Singleline);
                xml.LoadXml(stopTableMatch.Value.Replace("&nbsp;", ";").Replace("&nbsp", ";").Replace("&#355;", "ț").Replace("&#194", "Â").Replace("&#195", "Ă"));
                XmlNodeList tabel2Nodes = xml.SelectSingleNode("body").SelectSingleNode("div", "id", "header").SelectSingleNode("div", "id", "tabele").ChildNodes;

                foreach (XmlNode tabel2Node in tabel2Nodes)
                {
                    string weekdayCategoryString = tabel2Node.SelectSingleNode("div", "id", "web_class_title").InnerText.Trim();
                    WeekDayCategory weekdayCategory = new WeekDayCategory(WeekDayCategory.ParseCategoryType(weekdayCategoryString));

                    int lastHour = -1;
                    foreach (XmlNode rowNode in tabel2Node.ChildNodes)
                    {
                        if (rowNode.InnerText.ToLowerInvariant().Contains("ora") || rowNode.InnerText.ToLowerInvariant().Contains("minutul"))
                            continue;
                        switch (rowNode.Attributes["id"].Value)
                        {
                            case "web_class_hours":
                                lastHour = Int32.Parse(rowNode.InnerText.Trim());
                                break;
                            case "web_class_minutes":
                                if (rowNode.InnerText.Contains("NU CIRCUL"))
                                    break;
                                string[] minutesParts = rowNode.InnerText.Trim().Replace(' ', ';').Replace("\n", ";").Replace("\t", ";").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string minutePart in minutesParts)
                                {
                                    StopTime stopTime = new StopTime(new TimeSpan(lastHour, Int32.Parse(minutePart.Replace("*", "")), 0), minutePart.Contains('*'));
                                    weekdayCategory.StopTimes.Add(stopTime);
                                }
                                break;
                        }
                    }

                    routeStop.WeekDayCategories.Add(weekdayCategory);
                }

                halfRoute.Add(routeStop);
            }

            return halfRoute;
            /*}
            catch (Exception E)
            {
                if (throwErrors)
                    throw E;
                else
                    Console.WriteLine(E.ToString());
                return null;
            }*/
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private void DecodeTestament(WebClient client, Database database, Tuple<string, string, Color> routeInfo, bool forceDownload, bool throwErrors)
        {
            KeyValuePair<string, string> colorless = new KeyValuePair<string, string>(routeInfo.Item1, routeInfo.Item2);
            HalfRoute outgoing = routeInfo.Item2.Contains("route")
                ? DecodeNewTestament(client, database, colorless, "tour", forceDownload, throwErrors)
                : DecodeOldTestament(client, database, colorless, "dus", forceDownload, throwErrors);
            HalfRoute incoming = routeInfo.Item2.Contains("route")
                ? DecodeNewTestament(client, database, colorless, "retour", forceDownload, throwErrors)
                : DecodeOldTestament(client, database, colorless, "intors", forceDownload, throwErrors);
            database.Routes.Add(new Route(routeInfo.Item1.Substring(routeInfo.Item1.IndexOf(' ') + 1), routeInfo.Item1, routeInfo.Item3, outgoing, incoming));
        }

        /*      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      *      */

        private void closeT_Tick(object sender, EventArgs e)
        {
            this.mainForm.stopFilterT_Tick(sender, e);
            this.mainForm.StopView_Click(null, null);
            this.Close();
        }
    }
}
