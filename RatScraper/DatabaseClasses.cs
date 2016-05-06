using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RatScraper
{
    /// <summary>
    /// Defines an object with a string ID.
    /// </summary>
    public abstract class ObjectWithID
    {
        /// <summary>Gets and privately sets the ID of the object. Should be unique.</summary>
        public string ID { get; private set; }

        /// <summary>Constructs a new ObjectWithID object from the given string ID parameter.</summary>
        public ObjectWithID(string id)
        {
            this.ID = id;
        }

        /// <summary>Generates an XmlNode from this object.</summary>
        /// <param name="doc">the XmlDocument used to create the element</param>
        /// <param name="name">the name of the XmlNode to be created</param>
        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "ID", this.ID);
            return node;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ObjectWithID))
                return false;
            return this.ID.Equals((obj as ObjectWithID).ID);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.ID;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ObjectWithName : ObjectWithID
    {
        public string Name { get; private set; }

        public ObjectWithName(string id, string name)
            : base(id)
        {
            this.Name = name;
        }

        public new XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = base.ToXml(doc, name);
            node.AddAttribute(doc, "name", this.Name);
            return node;
        }

        public override string ToString()
        {
            return string.Format("{0}. {1}", this.ID, this.Name);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StopTime
    {
        public TimeSpan Time { get; private set; }
        public bool Asterisk { get; private set; }

        public StopTime(TimeSpan time, bool asterisk)
        {
            this.Time = time;
            this.Asterisk = asterisk;
        }

        public static StopTime Parse(string text)
        {
            bool asterisk = text.Contains('*');
            string[] parts = text.Replace("*", "").Split(':');
            return new StopTime(new TimeSpan(Int32.Parse(parts[0]), Int32.Parse(parts[1]), 0), asterisk);
        }

        public string ToSaveString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Time.Hours).Append(':').Append(this.Time.Minutes);
            if (this.Asterisk)
                sb.Append('*');
            return sb.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0:D2}:{1:D2}", this.Time.Hours, this.Time.Minutes);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeekDayCategory
    {
        public enum WeekDayCategoryType { Unknown, Weekday, Saturday, Sunday, SaturdaySunday, SummerWeekday };

        public WeekDayCategoryType Type { get; private set; }
        public List<StopTime> StopTimes { get; private set; }

        public WeekDayCategory(WeekDayCategoryType type)
            : this(type, new List<StopTime>())
        {
        }

        public WeekDayCategory(WeekDayCategoryType type, List<StopTime> stopTimes)
        {
            this.Type = type;
            this.StopTimes = stopTimes;
        }

        public static WeekDayCategoryType ParseCategoryType(string text)
        {
            switch (text.ToLowerInvariant())
            {
                case "luni - vineri":
                    return WeekDayCategoryType.Weekday;
                case "sambata":
                    return WeekDayCategoryType.Saturday;
                case "duminica":
                    return WeekDayCategoryType.Sunday;
                case "luni - vineri (vacanta)":
                    return WeekDayCategoryType.SummerWeekday;
                default:
                    return WeekDayCategoryType.Unknown;
            }
        }

        public static WeekDayCategory Parse(XmlNode node)
        {
            WeekDayCategoryType type = (WeekDayCategoryType) Enum.Parse(typeof(WeekDayCategoryType), node.Attributes["type"].Value);
            List<StopTime> stopTimes = new List<StopTime>();
            string[] stopTimeStrings = node.Attributes["stopTimes"].Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string stopTimeString in stopTimeStrings)
                stopTimes.Add(StopTime.Parse(stopTimeString));
            return new WeekDayCategory(type, stopTimes);
        }

        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "type", this.Type.ToString());
            node.AddAttribute(doc, "stopTimeCount", this.StopTimes.Count);
            StringBuilder sb = new StringBuilder();
            foreach (StopTime stopTime in this.StopTimes)
                sb.Append(';').Append(stopTime.ToSaveString());
            node.AddAttribute(doc, "stopTimes", sb.Length > 0 ? sb.ToString().Substring(1) : "");
            return node;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} stop times)", this.Type, this.StopTimes.Count);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Stop : ObjectWithName
    {
        public Stop(string id, string name)
            : base(id, name)
        {
        }

        public static Stop Parse(XmlNode node)
        {
            string id = node.Attributes["ID"].Value;
            string name = node.Attributes["name"].Value;
            return new Stop(id, name);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RouteStop
    {
        public Stop Stop { get; private set; }
        public List<WeekDayCategory> WeekDayCategories { get; private set; }

        public RouteStop(Stop stop)
            : this(stop, new List<WeekDayCategory>())
        {
        }

        public RouteStop(Stop stop, List<WeekDayCategory> weekDayCategories)
        {
            this.Stop = stop;
            this.WeekDayCategories = weekDayCategories;
        }

        public static RouteStop Parse(XmlNode node, ListOfIDObjects<Stop> stops)
        {
            Stop stop = stops.GetItemByID(node.Attributes["stopID"].Value);
            List<WeekDayCategory> weekDayCategories = new List<WeekDayCategory>();
            XmlNodeList weekDayCategoryNodes = node.SelectNodes("WeekDayCategory");
            foreach (XmlNode weekDayCategoryNode in weekDayCategoryNodes)
                weekDayCategories.Add(WeekDayCategory.Parse(weekDayCategoryNode));
            return new RouteStop(stop, weekDayCategories);
        }

        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "stopID", this.Stop.ID);
            node.AddAttribute(doc, "weekDayCategoryCount", this.WeekDayCategories.Count);
            foreach (WeekDayCategory weekDayCategory in this.WeekDayCategories)
                node.AppendChild(weekDayCategory.ToXml(doc, "WeekDayCategory"));
            return node;
        }

        public override string ToString()
        {
            return string.Format("Stop {0} ({1} weekday categories)", this.Stop != null ? this.Stop.Name : "(null)", this.WeekDayCategories.Count);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HalfRoute : List<RouteStop>
    {
        public string Name { get; internal set; }
        internal Route Route { get; set; }

        public HalfRoute(string name)
            : base()
        {
            this.Name = name;
            this.Route = null;
        }

        public int GetIndexOfRouteStopByStop(Stop stop)
        {
            for (int index = 0; index < this.Count; index++)
                if (this[index].Stop.Equals(stop))
                    return index;
            return -1;
        }

        public RouteStop GetRouteStopByStop(Stop stop)
        {
            int index = this.GetIndexOfRouteStopByStop(stop);
            return index != -1 ? this[index] : null;
        }

        public static HalfRoute Parse(XmlNode node, ListOfIDObjects<Stop> stops)
        {
            HalfRoute result = new HalfRoute(node.Attributes["name"].Value);
            XmlNodeList routeStopNodes = node.SelectNodes("RouteStop");
            foreach (XmlNode routeStopNode in routeStopNodes)
                result.Add(RouteStop.Parse(routeStopNode, stops));
            return result;
        }

        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "name", this.Name);
            node.AddAttribute(doc, "stopCount", this.Count);
            foreach (RouteStop routeStop in this)
                node.AppendChild(routeStop.ToXml(doc, "RouteStop"));
            return node;
        }

        public override string ToString()
        {
            return string.Format("HalfRoute '{0}': {1} route stops", this.Name, this.Count);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Route : ObjectWithName
    {
        public HalfRoute Outgoing { get; private set; }
        public HalfRoute Incoming { get; private set; }

        public Route(string id, string name, HalfRoute outgoing, HalfRoute incoming)
            : base(id, name)
        {
            this.Outgoing = outgoing;
            this.Incoming = incoming;
        }

        public static Route Parse(XmlNode node, ListOfIDObjects<Stop> stops)
        {
            string id = node.Attributes["ID"].Value;
            string name = node.Attributes["name"].Value;
            HalfRoute outgoing = HalfRoute.Parse(node.SelectSingleNode("Outgoing"), stops);
            HalfRoute incoming = HalfRoute.Parse(node.SelectSingleNode("Incoming"), stops);
            Route result = new Route(id, name, outgoing, incoming);
            result.Outgoing.Route = result;
            result.Incoming.Route = result;
            return result;
        }

        public new XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = base.ToXml(doc, name);
            if (this.Outgoing != null)
                node.AppendChild(this.Outgoing.ToXml(doc, "Outgoing"));
            if (this.Incoming != null)
                node.AppendChild(this.Incoming.ToXml(doc, "Incoming"));
            return node;
        }

        public override string ToString()
        {
            return string.Format("{0}. {1} ({2}+{3} stops)", this.ID, this.Name, this.Outgoing.Count, this.Incoming.Count);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Database
    {
        public ListOfIDObjects<Stop> Stops { get; private set; }
        public ListOfIDObjects<Route> Routes { get; private set; }

        public Database()
        {
            this.Stops = new ListOfIDObjects<Stop>();
            this.Routes = new ListOfIDObjects<Route>();
        }

        public bool AddUniqueStop(string name, string id = null)
        {
            if (id != null)
                if (this.Stops.GetItemByID(id) != null)
                    return false;
                else
                {
                    this.Stops.Add(new Stop(id, name));
                    return true;
                }
            else
                if (this.Stops.GetItemByName(name) != null)
                    return false;
                else
                {
                    id = this.Stops.GetUniqueNumericID(3, false);
                    this.Stops.Add(new Stop(id, name));
                    return true;
                }
        }

        public List<HalfRoute> GetHalfRoutesByStop(Stop stop)
        {
            List<HalfRoute> result = new List<HalfRoute>();
            foreach (Route route in this.Routes)
            {
                if (route.Outgoing.GetRouteStopByStop(stop) != null)
                    result.Add(route.Outgoing);
                if (route.Incoming.GetRouteStopByStop(stop) != null)
                    result.Add(route.Incoming);
            }
            return result;
        }

        public string LoadDatabase(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Paths.DatabaseFile);

                this.Stops.Clear();
                XmlNodeList nodes = doc.SelectNodes("DATABASE/STOPS/Stop");
                foreach (XmlNode node in nodes)
                    this.Stops.Add(Stop.Parse(node));

                this.Routes.Clear();
                nodes = doc.SelectNodes("DATABASE/ROUTES/Route");
                foreach (XmlNode node in nodes)
                    this.Routes.Add(Route.Parse(node, this.Stops));

                return string.Empty;
            }
            catch (Exception E) { return E.ToString(); }
        }

        public string SaveDatabase(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.AppendChild(doc.CreateElement("DATABASE"));
                root.AddAttribute(doc, "lastSaved", DateTime.Now.ToString("HH:mm:ss, 'on' dddd, d MMMM yyyy"));

                XmlNode node = root.AppendChild(doc.CreateElement("STOPS"));
                node.AddAttribute(doc, "count", this.Stops.Count);
                foreach (Stop stop in this.Stops)
                    node.AppendChild(stop.ToXml(doc, "Stop"));

                node = root.AppendChild(doc.CreateElement("ROUTES"));
                node.AddAttribute(doc, "count", this.Routes.Count);
                foreach (Route route in this.Routes)
                    node.AppendChild(route.ToXml(doc, "Route"));

                doc.Save(path);

                return string.Empty;
            }
            catch (Exception E) { return E.ToString(); }
        }
    }
}
