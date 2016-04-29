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
            switch (text)
            {
                case "Luni - Vineri":
                    return WeekDayCategoryType.Weekday;
                case "Sambata":
                    return WeekDayCategoryType.Saturday;
                case "Duminica":
                    return WeekDayCategoryType.Sunday;
                case "Luni - Vineri (vacanta)":
                    return WeekDayCategoryType.SummerWeekday;
                default:
                    return WeekDayCategoryType.Unknown;
            }
        }

        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "type", this.Type.ToString());
            node.AddAttribute(doc, "stopTimeCount", this.StopTimes.Count);
            StringBuilder sb = new StringBuilder();
            foreach (StopTime stopTime in this.StopTimes)
            {
                sb.Append(';').Append(stopTime.Time.Hours).Append(':').Append(stopTime.Time.Minutes);
                if (stopTime.Asterisk)
                    sb.Append('*');
            }
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
        public HalfRoute()
            : base()
        {
        }

        public XmlNode ToXml(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.AddAttribute(doc, "stopCount", this.Count);
            foreach (RouteStop routeStop in this)
                node.AppendChild(routeStop.ToXml(doc, "RouteStop"));
            return node;
        }

        public override string ToString()
        {
            return string.Format("HalfRoute: {0} route stops", this.Count);
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

        public string LoadDatabase(string path)
        {
            try
            {
                this.Stops.Clear();
                this.Routes.Clear();

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
