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
    public partial class FMain : MyForm
    {
        public Database Database { get; private set; }

        public FMain(FMain mainForm)
            : base(null)
        {
            InitializeComponent();
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            this.Database = new Database();

            this.RegisterControlsToMoveForm(this.titleLabel1);
        }

        /*private void workBW_DoWork(object sender, DoWorkEventArgs e)
        {
            Database database = e.Argument as Database;
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
                DecodeTestament(web, database, routePages[iRoutePage], false);
            }

            workBW.ReportProgress(0, @"Saving database to file...");
            string saveResult = database.SaveDatabase(Paths.DatabaseFile);
            if (!saveResult.Equals(string.Empty))
                throw new ApplicationException(saveResult);

            Console.WriteLine(database.Routes.Count(r => r.Outgoing != null && r.Incoming != null));
        }

        private void workBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusL.Text = e.UserState as string;
            Console.WriteLine("\n ### " + statusL.Text + "\n");
        }

        private void workBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusL.Text = "Done.";
        }*/

        //

        private string GetNewTestamentStationID(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts[parts.Length - 1];
        }

        private string GetNewTestamentWebpageText(WebClient client, string url, string filePath, bool forceDownload)
        {
            if (forceDownload || !File.Exists(filePath))
                new WebClient().DownloadFile(url, filePath);
            return File.ReadAllText(filePath);
        }

        private HalfRoute DecodeNewTestament(WebClient client, Database database, KeyValuePair<string, string> route, string halfRoutePhase, bool forceDownload)
        {
            //workBW.ReportProgress(0, "Downloading and processing (new testament) " + route.Key + " (" + halfRoutePhase + ")...");
            if (!Directory.Exists(Paths.DownloadsFolder + route.Key))
                Directory.CreateDirectory(Paths.DownloadsFolder + route.Key);
            string file = GetNewTestamentWebpageText(client,
                string.Format(@"{0}timetable/", route.Value.Replace("/tour", "/" + halfRoutePhase)),
                string.Format(@"{0}{1}\{2} main.html", Paths.DownloadsFolder, route.Key, halfRoutePhase), forceDownload);

            List<Tuple<string, string, bool>> stopPages = new List<Tuple<string, string, bool>>();
            Match stopListText = Regex.Match(file, @"<div class=""box butoane-statii"">.*?</div>", RegexOptions.Singleline);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stopListText.Value);
            foreach (XmlNode stopNode in xml.SelectNodes("div/a"))
            {
                stopPages.Add(new Tuple<string, string, bool>(stopNode.InnerText, @"http://www.ratbv.ro" + stopNode.Attributes["href"].Value, stopNode.Attributes["class"] != null && stopNode.Attributes["class"].Value.Equals("active")));
                database.Stops.Add(new Stop(GetNewTestamentStationID(stopPages.Last().Item2), stopPages.Last().Item1));
            }

            HalfRoute halfRoute = new HalfRoute();
            foreach (Tuple<string, string, bool> stopPage in stopPages)
            {
                string stopPageText = GetNewTestamentWebpageText(client, stopPage.Item2, string.Format(@"{0}{1}\{2} {3}.html", Paths.DownloadsFolder, route.Key, halfRoutePhase, GetNewTestamentStationID(stopPage.Item2)), forceDownload);
                Match stopTableMatch = Regex.Match(stopPageText, @"<div class=""box tabel-statii"">.*?</div>", RegexOptions.Singleline);

                xml.LoadXml(stopTableMatch.Value.Replace("&nbsp;", ";").Replace("&nbsp", ";"));
                RouteStop stop = new RouteStop(database.Stops.GetItemByID(GetNewTestamentStationID(stopPage.Item2)));
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
                }

                halfRoute.Add(stop);
            }

            return halfRoute;
        }

        private void DecodeTestament(WebClient client, Database database, KeyValuePair<string, string> route, bool forceDownload)
        {
            HalfRoute outgoing = route.Value.Contains("route") ? DecodeNewTestament(client, database, route, "tour", forceDownload) : null;
            HalfRoute incoming = route.Value.Contains("route") ? DecodeNewTestament(client, database, route, "retour", forceDownload) : null;
            database.Routes.Add(new Route(route.Key.Substring(route.Key.IndexOf(' ') + 1), route.Key, outgoing, incoming));
        }
    }
}
