using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class StopTimeView : MyAppBaseControl
    {
        public const int AccentBarHeight = 2;
        public const int DefaultControlHeight = 60;

        private KeyValuePair<HalfRoute, StopTime> stopTimeInfo;
        private Font routeIDFont;
        private Font timeFont;
        //private Font descriptionFont;

        public StopTimeView()
            : base()
        {
            this.routeIDFont = new Font("Segoe UI", 20, FontStyle.Bold);
            this.timeFont = new Font("Segoe UI Light", 19, FontStyle.Bold);
            //this.descriptionFont = new Font("Segoe UI Light", 11, FontStyle.Regular);
            this.Cursor = Cursors.Hand;
        }

        public KeyValuePair<HalfRoute, StopTime> StopTimeInfo
        {
            get { return this.stopTimeInfo; }
            set { this.stopTimeInfo = value; this.Invalidate(); }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (this.stopTimeInfo.Key == null || this.stopTimeInfo.Value == null)
            {
                e.Graphics.DrawString("[stopTimeInfo invalid]", this.Font, MyGUIs.Accent[this.mouseIsOver].Brush, PointF.Empty);
                return;
            }

            Rectangle idRect = new Rectangle(10, 10, 60, this.Height - 20);
            e.Graphics.FillRectangle(new SolidBrush(this.stopTimeInfo.Key.Route.Color), idRect);

            string text = this.stopTimeInfo.Key.Route.ID.ToUpperInvariant();
            SizeF size = e.Graphics.MeasureString(text, this.routeIDFont);
            e.Graphics.DrawString(text, this.routeIDFont, MyGUIs.Text.Highlighted.Brush, new PointF(idRect.Left + idRect.Width / 2f - size.Width / 2f, this.Height / 2f - size.Height / 2));

            text = this.stopTimeInfo.Value.ToString();
            size = e.Graphics.MeasureString(text, this.timeFont);
            e.Graphics.DrawString(text, this.timeFont, MyGUIs.Text[this.mouseIsOver].Brush, new PointF(idRect.Right + 7, idRect.Top - 8));

            /*text = string.Format("{0} {1}", this.stopTimeInfo.Count, this.stopTimeInfo.Count == 1 ? "stop" : "stops");
            size = e.Graphics.MeasureString(text, this.descriptionFont);
            e.Graphics.DrawString(text, this.descriptionFont, MyGUIs.Text[this.mouseIsOver].Brush, new PointF(idRect.Right + 10, idRect.Bottom - size.Height + 5));*/

            if (this.isChecked)
                e.Graphics.FillRectangle(MyGUIs.Accent[!this.mouseIsClicked].Brush, 0, this.Height - HalfRouteView.AccentBarHeight, this.Width, HalfRouteView.AccentBarHeight);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StopTimeViewManager
    {
        public MyScrollPanel MyScrollPanel { get; private set; }
        private EventHandler onMouseEnter;
        private EventHandler onMouseLeave;
        private EventHandler onClick;
        public List<StopTimeView> StopTimeViews { get; private set; }

        public StopTimeViewManager(Panel containerPanel, EventHandler onMouseEnter, EventHandler onMouseLeave, EventHandler onClick)
            : base()
        {
            this.MyScrollPanel = new MyScrollPanel(containerPanel, MyScrollBar.ScrollBarPosition.Right, 16, 100);
            this.onMouseEnter = onMouseEnter;
            this.onMouseLeave = onMouseLeave;
            this.onClick = onClick;
            this.StopTimeViews = new List<StopTimeView>();
        }

        public void SetStopTimeInfos(List<KeyValuePair<HalfRoute, StopTime>> stopTimeInfos)
        {
            for (int iSV = stopTimeInfos.Count; iSV < this.StopTimeViews.Count; iSV++)
                this.StopTimeViews[iSV].Hide();

            for (int iHRV = 0; iHRV < stopTimeInfos.Count; iHRV++)
            {
                StopTimeView stopTimeView = null;
                if (iHRV < this.StopTimeViews.Count)
                    stopTimeView = this.StopTimeViews[iHRV];
                else
                {
                    stopTimeView = new StopTimeView() { Size = new Size(this.MyScrollPanel.VisibleSize.Width, StopTimeView.DefaultControlHeight) };
                    stopTimeView.MouseEnter += this.onMouseEnter;
                    stopTimeView.MouseLeave += this.onMouseLeave;
                    stopTimeView.Click += this.onClick;
                    this.StopTimeViews.Add(stopTimeView);
                    this.MyScrollPanel.AddControl(stopTimeView, new Point(0, iHRV * StopTimeView.DefaultControlHeight), false);
                }

                stopTimeView.StopTimeInfo = stopTimeInfos[iHRV];
                stopTimeView.Checked = false;
                stopTimeView.Show();
            }

            this.MyScrollPanel.UpdatePanelSize();
        }
    }
}
