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
    public class StopView : MyAppBaseControl
    {
        public const int AccentBarHeight = 2;
        public const int DefaultControlHeight = 26;

        private Stop stop;

        public StopView()
            : base()
        {
            this.Font = new Font("Segoe UI Light", 13, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
        }

        public Stop Stop
        {
            get { return this.stop; }
            set { this.stop = value; this.Invalidate(); }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            string text = this.stop != null ? this.stop.Name : "[stop == null]";
            SizeF size = e.Graphics.MeasureString(text, this.Font);
            e.Graphics.DrawString(text, this.Font, MyGUIs.Accent[this.mouseIsOver].Brush, new PointF(0, this.Height / 2f - size.Height / 2));

            if (this.isChecked)
                e.Graphics.FillRectangle(MyGUIs.Accent[!this.mouseIsClicked].Brush, 0, this.Height - StopView.AccentBarHeight, this.Width, StopView.AccentBarHeight);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StopViewManager
    {
        public MyScrollPanel MyScrollPanel { get; private set; }
        private EventHandler onMouseEnter;
        private EventHandler onMouseLeave;
        private EventHandler onClick;
        public List<StopView> StopViews { get; private set; }

        public StopViewManager(Panel containerPanel, EventHandler onMouseEnter, EventHandler onMouseLeave, EventHandler onClick)
            : base()
        {
            this.MyScrollPanel = new MyScrollPanel(containerPanel, MyScrollBar.ScrollBarPosition.Right, 16, 100);
            this.onMouseEnter = onMouseEnter;
            this.onMouseLeave = onMouseLeave;
            this.onClick = onClick;
            this.StopViews = new List<StopView>();
        }

        public void SetStops(ListOfIDObjects<Stop> stops)
        {
            for (int iSV = stops.Count; iSV < this.StopViews.Count; iSV++)
                this.StopViews[iSV].Hide();

            for (int iSV = 0; iSV < stops.Count; iSV++)
            {
                StopView stopView = null;
                if (iSV < this.StopViews.Count)
                    stopView = this.StopViews[iSV];
                else
                {
                    stopView = new StopView() { Size = new Size(this.MyScrollPanel.VisibleSize.Width, StopView.DefaultControlHeight) };
                    stopView.MouseEnter += this.onMouseEnter;
                    stopView.MouseLeave += this.onMouseLeave;
                    stopView.Click += this.onClick;
                    this.StopViews.Add(stopView);
                    this.MyScrollPanel.AddControl(stopView, new Point(0, iSV * StopView.DefaultControlHeight), false);
                }

                stopView.Stop = stops[iSV];
                stopView.Show();
            }

            this.MyScrollPanel.UpdatePanelSize();
        }
    }
}
