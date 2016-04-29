using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Custom-configured BBA button.
    /// </summary>
    public class MyButton : MyEuroBaseControl
    {
        public static readonly Pair<int> BarHeight = new Pair<int>(2, 4);

        public MyButton()
            : base()
        {
            this.Cursor = Cursors.Hand;
            this.Font =  new Font("Segoe UI", 10);
        }

        private bool drawBar = true;
        public bool DrawBar
        {
            get { return this.drawBar; }
            set { this.drawBar = value; this.Invalidate(); }
        }

        private bool bigBar = false;
        public bool BigBar
        {
            get { return this.bigBar; }
            set { this.bigBar = value; this.Invalidate(); }
        }

        private Image image;
        public Image Image
        {
            get { return this.image; }
            set { this.image = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            Size size = e.Graphics.MeasureString(this.Text, this.Font).ToSize();
            const int imageLabelPadding = 2;

            if (this.Image != null)
                size.Width += this.Image.Width + imageLabelPadding;
            int lastLeft = this.Width / 2 - size.Width / 2;
            if (this.Image != null)
            {
                e.Graphics.DrawImage(this.Image, lastLeft, this.Height / 2 - this.Image.Height / 2);
                lastLeft += this.Image.Width + imageLabelPadding;
            }

            e.Graphics.DrawString(this.Text, this.Font, this.isChecked ? MyGUIs.Accent.Highlighted.Brush : MyGUIs.Text.GetValue(this.mouseIsClicked).Brush,
                new Point(lastLeft, this.Height / 2 - size.Height / 2));

            if (this.drawBar)
                e.Graphics.FillRectangle(MyGUIs.Accent.GetValue(this.mouseIsOver).Brush, 1, this.Height - BarHeight.GetValue(this.bigBar), this.Width - 2, BarHeight.GetValue(this.bigBar));
        }
    }
}
