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
    public class HalfRouteView : MyAppBaseControl
    {
        public const int AccentBarHeight = 2;
        public const int DefaultControlHeight = 40;

        private HalfRoute halfRoute;

        public HalfRouteView()
            : base()
        {
            this.Font = new Font("Segoe UI Light", 17, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
        }

        public HalfRoute HalfRoute
        {
            get { return this.halfRoute; }
            set { this.halfRoute = value; this.Invalidate(); }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            string text = this.halfRoute != null ? (this.halfRoute.Route.ID + ". " + this.halfRoute.Name) : "[halfRoute == null]";
            SizeF size = e.Graphics.MeasureString(text, this.Font);
            e.Graphics.DrawString(text, this.Font, MyGUIs.Accent[this.mouseIsOver].Brush, new PointF(0, this.Height / 2f - size.Height / 2));

            if (this.isChecked)
                e.Graphics.FillRectangle(MyGUIs.Accent[!this.mouseIsClicked].Brush, 0, this.Height - HalfRouteView.AccentBarHeight, this.Width, HalfRouteView.AccentBarHeight);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HalfRouteViewManager
    {
        public MyScrollPanel MyScrollPanel { get; private set; }
        private EventHandler onMouseEnter;
        private EventHandler onMouseLeave;
        private EventHandler onClick;
        public List<HalfRouteView> HalfRouteViews { get; private set; }

        public HalfRouteViewManager(Panel containerPanel, EventHandler onMouseEnter, EventHandler onMouseLeave, EventHandler onClick)
            : base()
        {
            this.MyScrollPanel = new MyScrollPanel(containerPanel, MyScrollBar.ScrollBarPosition.Right, 16, 100);
            this.onMouseEnter = onMouseEnter;
            this.onMouseLeave = onMouseLeave;
            this.onClick = onClick;
            this.HalfRouteViews = new List<HalfRouteView>();
        }

        public void SetHalfRoutes(List<HalfRoute> halfRoutes)
        {
            for (int iSV = halfRoutes.Count; iSV < this.HalfRouteViews.Count; iSV++)
                this.HalfRouteViews[iSV].Hide();

            for (int iHRV = 0; iHRV < halfRoutes.Count; iHRV++)
            {
                HalfRouteView routeView = null;
                if (iHRV < this.HalfRouteViews.Count)
                    routeView = this.HalfRouteViews[iHRV];
                else
                {
                    routeView = new HalfRouteView() { Size = new Size(this.MyScrollPanel.VisibleSize.Width, StopView.DefaultControlHeight) };
                    routeView.MouseEnter += this.onMouseEnter;
                    routeView.MouseLeave += this.onMouseLeave;
                    routeView.Click += this.onClick;
                    this.HalfRouteViews.Add(routeView);
                    this.MyScrollPanel.AddControl(routeView, new Point(0, iHRV * StopView.DefaultControlHeight), false);
                }

                routeView.HalfRoute = halfRoutes[iHRV];
                routeView.Show();
            }

            this.MyScrollPanel.UpdatePanelSize();
        }
    }
}
