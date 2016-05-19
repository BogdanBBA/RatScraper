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
        public const int DefaultControlHeight = 60;

        private HalfRoute halfRoute;
        private Font idFont;
        private Font nameFont;
        private Font descriptionFont;

        public HalfRouteView()
            : base()
        {
            this.idFont = new Font("Segoe UI", 20, FontStyle.Bold);
            this.nameFont = new Font("Segoe UI Light", 19, FontStyle.Bold);
            this.descriptionFont = new Font("Segoe UI Light", 11, FontStyle.Regular);
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

            if (this.halfRoute == null)
            {
                e.Graphics.DrawString("[halfRoute == null]", this.Font, MyGUIs.Accent[this.mouseIsOver].Brush, PointF.Empty);
                return;
            }

            Rectangle idRect = new Rectangle(10, 10, 60, this.Height - 20);
            e.Graphics.FillRectangle(new SolidBrush(this.halfRoute.Route.Color), idRect);

            string text = this.halfRoute.Route.ID.ToUpperInvariant();
            SizeF size = e.Graphics.MeasureString(text, this.idFont);
            e.Graphics.DrawString(text, this.idFont, MyGUIs.Text.Highlighted.Brush, new PointF(idRect.Left + idRect.Width / 2f - size.Width / 2f, this.Height / 2f - size.Height / 2));

            text = this.halfRoute.Name;
            size = e.Graphics.MeasureString(text, this.nameFont);
            e.Graphics.DrawString(text, this.nameFont, MyGUIs.Text[this.mouseIsOver].Brush, new PointF(idRect.Right + 7, idRect.Top - 8));

            text = string.Format("{0} {1}", this.halfRoute.Count, this.halfRoute.Count == 1 ? "stop" : "stops");
            size = e.Graphics.MeasureString(text, this.descriptionFont);
            e.Graphics.DrawString(text, this.descriptionFont, MyGUIs.Text[this.mouseIsOver].Brush, new PointF(idRect.Right + 10, idRect.Bottom - size.Height + 5));

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
                    routeView = new HalfRouteView() { Size = new Size(this.MyScrollPanel.VisibleSize.Width, HalfRouteView.DefaultControlHeight) };
                    routeView.MouseEnter += this.onMouseEnter;
                    routeView.MouseLeave += this.onMouseLeave;
                    routeView.Click += this.onClick;
                    this.HalfRouteViews.Add(routeView);
                    this.MyScrollPanel.AddControl(routeView, new Point(0, iHRV * HalfRouteView.DefaultControlHeight), false);
                }

                routeView.HalfRoute = halfRoutes[iHRV];
                routeView.Checked = false;
                routeView.Show();
            }

            this.MyScrollPanel.UpdatePanelSize();
        }
    }
}
