using RatScraper.VisualComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RatScraper
{
    /// <summary>
    /// A pair of values (a more convenient form of KeyValuePair).
    /// </summary>
    /// <typeparam name="T">the data type of the values (unrestricted)</typeparam>
    public class Pair<T>
    {
        private KeyValuePair<T, T> pair;

        public Pair(T normal, T highlighted)
        {
            this.pair = new KeyValuePair<T, T>(normal, highlighted);
        }

        public T Normal
        {
            get { return this.pair.Key; }
            set { this.pair = new KeyValuePair<T, T>(value, this.pair.Value); }
        }

        public T Highlighted
        {
            get { return this.pair.Value; }
            set { this.pair = new KeyValuePair<T, T>(this.pair.Key, value); }
        }

        public T this[bool highlighted]
        { get { return this.GetValue(highlighted); } }

        public T GetValue(bool highlighted)
        {
            return highlighted ? this.Highlighted : this.Normal;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", this.Normal.ToString(), this.Highlighted.ToString());
        }
    }
    /// <summary>
    /// A pair of values (a more convenient form of KeyValuePair) identical to class Pair, except more football-match relevant way.
    /// </summary>
    /// <typeparam name="T">the data type of the values (still unrestricted)</typeparam>
    public class PairT<T>
    {
        private KeyValuePair<T, T> pair;

        public PairT(T home, T away)
        {
            this.pair = new KeyValuePair<T, T>(home, away);
        }

        public T Home
        {
            get { return this.pair.Key; }
            set { this.pair = new KeyValuePair<T, T>(value, this.pair.Value); }
        }

        public T Away
        {
            get { return this.pair.Value; }
            set { this.pair = new KeyValuePair<T, T>(this.pair.Key, value); }
        }

        public T this[bool home]
        { get { return this.GetValue(home); } }

        public T GetValue(bool home)
        {
            return home ? this.Home : this.Away;
        }
    }

    /// <summary>
    /// Defines a list of objects that are descended from ObjectWithID, containing only unique IDs.
    /// </summary>
    public class ListOfIDObjects<TYPE> : List<TYPE> where TYPE : ObjectWithID
    {
        /// <summary>Adds the given item to this list if it is unique (that is, if there is no other item in this list with the same ID).</summary>
        public new void Add(TYPE item)
        {
            if (this.GetIndexOfItemByID(item.ID) == -1)
                base.Add(item);
        }

        /// <summary>Iterates over the given list of items and calls this.Add(item) for each one.</summary>
        public new void AddRange(IEnumerable<TYPE> items)
        {
            foreach (TYPE item in items)
                this.Add(item);
        }

        /// <summary>Returns a unique numeric (a string left-padded with zeroes) ID for this list.</summary>
        public string GetUniqueNumericID(int length, bool ascending)
        {
            string format = "D" + length;
            for (int iID = ascending ? 0 : (int) Math.Pow(10, length) - 1; true; iID += (ascending ? 1 : -1))
                if (this.GetIndexOfItemByID(iID.ToString(format)) == -1)
                    return iID.ToString(format);
        }

        /// <summary>Searches the current list for an item with the given ID, and return its index if it is found, or -1 otherwise.</summary>
        public int GetIndexOfItemByID(string id)
        {
            for (int iItem = 0; iItem < this.Count; iItem++)
                if (this[iItem].ID.Equals(id))
                    return iItem;
            return -1;
        }

        /// <summary>Searches the current list for an item with the given item's ID, and return its index if it is found, or -1 otherwise.</summary>
        public int GetIndexOfItem(TYPE item)
        {
            return this.GetIndexOfItemByID(item.ID);
        }

        /// <summary>Searches the current list for an item with the given ID, and returns it if it is found, or null otherwise.</summary>
        public TYPE GetItemByID(string id)
        {
            int index = this.GetIndexOfItemByID(id);
            return index != -1 ? this[index] : null;
        }

        /// <summary>Searches the current list for an item with the given item's ID, and returns it if it is found, or null otherwise.</summary>
        public TYPE GetItem(TYPE item)
        {
            return item != null ? this.GetItemByID(item.ID) : null;
        }

        /// <summary>Searches the current list for an item whose type is based in ObjectWithName and which name is the same as the one given, and returns it if it is found, or null otherwise.</summary>
        public TYPE GetItemByName(string name)
        {
            foreach (TYPE item in this)
                if (item is ObjectWithName && (item as ObjectWithName).Name.Equals(name))
                    return item;
            return null;
        }

        /// <summary>Searches the current list for an item whose type is based in ObjectWithName and which name is the same as the one given, and returns it if it is found, or null otherwise.</summary>
        public ListOfIDObjects<TYPE> GetItemsByName(string name)
        {
            ListOfIDObjects<TYPE> result = new ListOfIDObjects<TYPE>();
            foreach (TYPE item in this)
                if (item is ObjectWithName && (item as ObjectWithName).Name.Equals(name))
                    result.Add(item);
            return result;
        }

        /// <summary>Generates and returns a new list with all of this list's items added to it.</summary>
        public ListOfIDObjects<TYPE> GetDeepCopy()
        {
            ListOfIDObjects<TYPE> result = new ListOfIDObjects<TYPE>();
            foreach (TYPE item in this)
                result.Add(item);
            return result;
        }

        /// <summary>Performs a swap between the items at the given indexes.</summary>
        public void SwapItemsAtPositions(int indexA, int indexB)
        {
            if (indexA >= 0 && indexA < this.Count && indexB >= 0 && indexB < this.Count)
            {
                TYPE aux = this[indexA];
                this[indexA] = this[indexB];
                this[indexB] = aux;
            }
        }
    }

    /// <summary>
    /// Utility functions and extension methods.
    /// </summary>
    public static class Utils
    {
        internal const string NullString = "null";
        internal const string DefaultSeparator = ";";
        internal static readonly Random Random = new Random();

        public static bool IsNumber(string text)
        {
            double number;
            return double.TryParse(text, out number);
        }

        public static void AddAttribute(this XmlNode node, XmlDocument doc, string key, string value)
        {
            XmlAttribute attribute = doc.CreateAttribute(key);
            attribute.Value = value != null ? value : Utils.NullString;
            node.Attributes.Append(attribute);
        }

        public static void AddAttribute(this XmlNode node, XmlDocument doc, string key, int value)
        { node.AddAttribute(doc, key, value.ToString()); }

        public static void AddAttribute(this XmlNode node, XmlDocument doc, string key, ulong? value)
        { node.AddAttribute(doc, key, value.HasValue ? value.Value.ToString() : null); }

        public static void AddAttribute(this XmlNode node, XmlDocument doc, string key, DateTime? value)
        { node.AddAttribute(doc, key, value.HasValue ? value.Value.ToString() : null); }

        public static string DecodeNullableString(string text)
        { return text == null || text.Equals(Utils.NullString) ? null : text; }

        public static DateTime? DecodeNullableDateTime(string text)
        { return text.Equals(Utils.NullString) ? (DateTime?) null : DateTime.Parse(text); }

        public static ulong? DecodeNullableUnsignedLong(string text)
        { return text.Equals(Utils.NullString) ? (ulong?) null : ulong.Parse(text); }

        public static Point MinimumPointValues(Point a, Point b)
        { return new Point(a.X < b.X ? a.X : b.X, a.Y < b.Y ? a.Y : b.Y); }

        public static Point MaximumPointValues(Point a, Point b)
        { return new Point(a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y); }

        public static string Plural(string singularForm, long quantity, bool includeQuantity)
        {
            string form = quantity == 1 ? singularForm : singularForm + "s";
            return includeQuantity ? Utils.FormatNumber(quantity) + " " + form : form;
        }

        public static Size ScaleRectangle(int width, int height, int maxWidth, int maxHeight)
        {
            var ratioX = (double) maxWidth / width;
            var ratioY = (double) maxHeight / height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int) (width * ratio);
            var newHeight = (int) (height * ratio);

            return new Size(newWidth, newHeight);
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, InterpolationMode mode, bool disposeOldImage)
        {
            Size newSize = ScaleRectangle(image.Width, image.Height, maxWidth, maxHeight);
            Image newImage = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = mode;
            g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            if (disposeOldImage)
                image.Dispose();
            return newImage;
        }

        public static Image GetScaledImageOrScaledDefault(string imagePath, int maxWidth, int maxHeight, InterpolationMode mode, Image defaultImg)
        {
            try
            { return Utils.ScaleImage(new Bitmap(imagePath), maxWidth, maxHeight, mode, true); }
            catch (Exception)
            { return Utils.ScaleImage(defaultImg, maxWidth, maxHeight, mode, false); }
        }

        public static string FormatNumber(long number)
        {
            return number.ToString("#,##0");
        }

        public static string FormatDuration(TimeSpan duration)
        {
            return (int) duration.TotalMinutes + ":" + duration.Seconds.ToString("D2");
        }

        public static Image ConvertToGrayscale(Image original)
        {
            //create a blank bitmap the same size as original and get a graphics object 
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][] {
                    new float[] { .3f, .3f, .3f, 0, 0 },
                    new float[] { .59f, .59f, .59f, 0, 0 },
                    new float[] { .11f, .11f, .11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 } });

            //create some image attributes and set the color matrix attribute
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image using the grayscale color matrix
            g.DrawImage(original,
                new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height,
                GraphicsUnit.Pixel,
                attributes);

            //dispose the Graphics object and return the result
            g.Dispose();
            return newBitmap;
        }

        public static void SizeAndPositionControlsInPanel<TYPE>(System.Windows.Forms.Panel container, IList<TYPE> controls, bool horizontally, int padding) where TYPE : Control
        {
            int newControlSize = (int) (((horizontally ? container.Width : container.Height) - (controls.Count - 1) * padding) / (double) controls.Count);
            for (int index = 0, lastPos = 0; index < controls.Count; index++, lastPos += newControlSize + padding)
            {
                controls[index].Parent = container;
                if (horizontally)
                    controls[index].SetBounds(lastPos, 0, newControlSize, container.Height);
                else
                    controls[index].SetBounds(0, lastPos, container.Width, newControlSize);
            }
        }

        public static void CheckControlAndUncheckAllOthers<TYPE>(this List<TYPE> list, TYPE controlToCheck) where TYPE : MyAppBaseControl
        {
            foreach (TYPE item in list)
                if (item.Checked != false)
                    item.Checked = false;
            if (controlToCheck != null)
                controlToCheck.Checked = true;
        }

        public static void RemoveAllClickEvents(this Control control)
        {
            FieldInfo f1 = typeof(Control).GetField("EventMouseWheel", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = f1.GetValue(control);
            PropertyInfo pi = control.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList) pi.GetValue(control, null);
            list.RemoveHandler(obj, list[obj]);
        }

        public static void SwapItemsAtPositions<TYPE>(this List<TYPE> list, int posA, int posB)
        {
            if (posA >= 0 && posA < list.Count && posB >= 0 && posB < list.Count)
            {
                TYPE aux = list[posA];
                list[posA] = list[posB];
                list[posB] = aux;
            }
        }

        public static string GetListString(this List<string> list, string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in list)
                sb.Append(item).Append(separator);
            if (sb.Length > 0)
                sb = sb.Remove(sb.Length - separator.Length, separator.Length);
            return sb.ToString();
        }

        public static XmlNode SelectSingleNode(this XmlNode node, string name, string attributeName, string attributeValue = null)
        {
            if (node != null && node.HasChildNodes)
                foreach (XmlNode child in node.ChildNodes)
                    if (child.Attributes[attributeName] != null && (attributeValue == null || child.Attributes[attributeName].Value.Equals(attributeValue)))
                        return child;
            return null;
        }
    }

    /// <summary>
    /// Represents colors by their HSL (hue-saturation-luminosity) values rather than the traditional RGB (red-green-blue) values.
    /// </summary>
    public class HSLColor
    {
        // Private data members below are on scale 0-1. They are scaled for use externally based on scale.
        private double hue = 1.0;
        private double saturation = 1.0;
        private double luminosity = 1.0;

        private const double scale = 255.0;

        public double Hue
        {
            get { return hue * scale; }
            set { hue = CheckRange(value / scale); }
        }

        public double Saturation
        {
            get { return saturation * scale; }
            set { saturation = CheckRange(value / scale); }
        }

        public double Luminosity
        {
            get { return luminosity * scale; }
            set { luminosity = CheckRange(value / scale); }
        }

        private double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        public string ToRGBString()
        {
            Color color = (Color) this;
            return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int) (255 * r), (int) (255 * g), (int) (255 * b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }

        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }

        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5)  //<=??
                temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
            return temp2;
        }

        public static implicit operator HSLColor(Color color)
        {
            HSLColor hslColor = new HSLColor();
            hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            return hslColor;
        }

        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = (HSLColor) Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        public HSLColor() { }

        public HSLColor(Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }

        public HSLColor(int red, int green, int blue)
        {
            SetRGB(red, green, blue);
        }

        public HSLColor(double hue, double saturation, double luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }
    }

    /// <summary>
    /// Contains static data that must be initialized immediately after the app starts, such as images and fonts.
    /// </summary>
    public static class StaticData
    {
        /*public static SortedDictionary<string, Bitmap> Images { get; private set; }
        public static PrivateFontCollection PVC { get; private set; }
        public static int FontExoLight_Index { get; private set; }
        public static int FontExo_Index { get; private set; }
        public static int FontExoBold_Index { get; private set; }

        static StaticData()
        {
            StaticData.Images = new SortedDictionary<string, Bitmap>();
        }

        public static string LoadData()
        {
            try
            {
                StaticData.Images.Add(Paths.LogoImageFile, new Bitmap(Paths.LogoImageFile));
                StaticData.Images.Add(Paths.UnknownTeamImageFile, new Bitmap(Paths.UnknownTeamImageFile));
                StaticData.Images.Add(Paths.KnockoutImageFile, new Bitmap(Paths.KnockoutImageFile));

                StaticData.PVC = new PrivateFontCollection();
                StaticData.PVC.AddFontFile(Paths.FontsFolder + "exo2-xlite.ttf");
                StaticData.FontExoLight_Index = 0;
                StaticData.PVC.AddFontFile(Paths.FontsFolder + "exo2.ttf");
                StaticData.FontExo_Index = 0;
                StaticData.PVC.AddFontFile(Paths.FontsFolder + "exo2-bold.ttf");
                StaticData.FontExoBold_Index = 0;

                return "";
            }
            catch (Exception E)
            {
                return "ERROR: StaticImages.LoadImages()\n\n" + E.ToString();
            }
        }*/
    }

    /// <summary>
    /// Contains folder and file paths (relative to the app executable) that are needed for the application, as well as related utility methods.
    /// </summary>
    public static class Paths
    {
        public static readonly string ProgramFilesFolder = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName + "/program-files/";
        public static readonly string DownloadsFolder = ProgramFilesFolder + "Downloads/";

        public static readonly string DatabaseFile = ProgramFilesFolder + "database.xml";

        public static readonly string[] Folders = { ProgramFilesFolder, DownloadsFolder };
        public static readonly string[] Files = { DatabaseFile };

        /// <summary>Checks that all files and folders in the respective static string lists of the Paths class exist.</summary>
        /// <returns>an empty string if execution ended successfully, or the error description otherwise</returns>
        public static string CheckPaths(bool tryToCreateMissingFolders)
        {
            string phase = "initializing";
            try
            {
                phase = "checking folders";

                foreach (string folder in Folders)
                {
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    if (!Directory.Exists(folder))
                        throw new Exception("Folder '" + folder + "' does not exist!");
                }

                foreach (string file in Files)
                    if (!File.Exists(file))
                        throw new Exception("File '" + file + "' does not exist!");

                return "";
            }
            catch (Exception E)
            {
                return "ERROR: Paths.CheckPaths(), phase '" + phase + "'\n\n" + E.ToString();
            }
        }
    }
}