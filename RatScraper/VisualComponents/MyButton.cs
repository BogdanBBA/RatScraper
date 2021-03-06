﻿using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Custom-configured BBA button.
    /// </summary>
    public class MyButton : MyAppBaseControl
    {
        public static readonly Pair<int> BarHeight = new Pair<int>(2, 4);

        public MyButton()
            : base()
        {
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Segoe UI", 10);
            this.SetAnimationParameters(true, EasingFunctions.CircularOut, 400, 20);
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
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

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.supportsAnimation)
                this.StartAnimation(this.animationCurrentPosition, this.Width);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.supportsAnimation)
                this.StartAnimation(this.animationCurrentPosition, 0);
            base.OnMouseLeave(e);
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
            {
                e.Graphics.FillRectangle(this.supportsAnimation ? MyGUIs.Accent.Normal.Brush : MyGUIs.Accent[this.mouseIsOver].Brush, 1, this.Height - BarHeight.GetValue(this.bigBar), this.Width - 2, BarHeight.GetValue(this.bigBar));
                if (this.supportsAnimation)
                    e.Graphics.FillRectangle(MyGUIs.Accent.Highlighted.Brush, 1, this.Height - BarHeight.GetValue(this.bigBar), (int) this.animationCurrentPosition, BarHeight.GetValue(this.bigBar));
            }
        }
    }
}
