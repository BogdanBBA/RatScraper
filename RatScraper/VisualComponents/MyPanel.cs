using System.Drawing;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Custom-configured BBA panel.
    /// </summary>
    public class MyPanel : Panel
    {
        public MyPanel()
            : base()
        {
        }

        private bool drawPanelAccent;
        public bool DrawPanelAccent
        {
            get { return this.drawPanelAccent; }
            set { this.drawPanelAccent = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(MyGUIs.Background.Normal.Color);
            if (this.drawPanelAccent)
                e.Graphics.DrawRectangle(MyGUIs.Accent.Normal.Pen, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
