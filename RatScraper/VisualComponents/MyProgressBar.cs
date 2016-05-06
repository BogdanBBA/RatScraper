using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Custom-configured BBA progress bar.
    /// </summary>
    public class MyProgressBar : MyAppBaseControl
    {
        /// <summary>The vertical padding of the actual progress bar of a MyProgressBar.</summary>
        protected const int VerticalBarPadding = 6;

        private Color fontForeColor;
        private Color fontBackColor;
        private int minimum;
        private int maximum;
        private int value;
        private string valueFormat;
        private int valueBoxWidth;

        /// <summary>Constructs a new MyProgressBar object with default values.</summary>
        public MyProgressBar()
            : base()
        {
            this.minimum = 0;
            this.maximum = 100;
            this.value = 60;
            this.valueFormat = "{0}";
            this.valueBoxWidth = 64;

            this.Size = new Size(200, 40);
            this.MinimumSize = new Size(this.valueBoxWidth + 10, 2 * MyProgressBar.VerticalBarPadding + 1);

            this.BackColor = MyGUIs.Background.Highlighted.Color;
            this.ForeColor = MyGUIs.Accent.Normal.Color;
            this.Font = new Font("Segoe UI", 15, FontStyle.Regular);
            this.fontForeColor = MyGUIs.Text.Normal.Color;
            this.fontBackColor = Color.FromArgb(16, 16, 16);
        }

        /// <summary>Gets or sets the foreground color of the value text label.</summary>
        public Color FontForeColor
        {
            get { return this.fontForeColor; }
            set { this.fontForeColor = value; this.Invalidate(); }
        }

        /// <summary>Gets or sets the background color of the value text label.</summary>
        public Color FontBackColor
        {
            get { return this.fontBackColor; }
            set { this.fontBackColor = value; this.Invalidate(); }
        }

        /// <summary>Gets or sets the minimum value that this progress bar will display.</summary>
        public int Minimum
        {
            get { return this.minimum; }
            set
            {
                this.minimum = value > this.maximum ? this.maximum : value;
                this.Value = this.value;
                this.Invalidate();
            }
        }

        /// <summary>Gets or sets the maximum value that this progress bar will display.</summary>
        public int Maximum
        {
            get { return this.maximum; }
            set
            {
                this.maximum = value < this.minimum ? this.minimum : value;
                this.Value = this.value;
                this.Invalidate();
            }
        }

        /// <summary>Gets or sets the numeric value that the progress bar will graphically display, proportionally between the set minimum and maximum values.</summary>
        public int Value
        {
            get { return this.value; }
            set { this.value = value < this.minimum ? this.minimum : (this.value > this.maximum ? this.maximum : value); this.Invalidate(); }
        }

        /// <summary>Gets or sets the C# formatting string used to format the value text label.
        /// Keep in mind that 3 arguments are always passed to the string.Format method in a specific order: value, minimum, maximum.</summary>
        public string ValueFormat
        {
            get { return this.valueFormat; }
            set { this.valueFormat = value; this.Invalidate(); }
        }

        /// <summary>Gets or sets the width in pixels of the box in which the formatted value text will be shown.</summary>
        public int ValueBoxWidth
        {
            get { return this.valueBoxWidth; }
            set { this.valueBoxWidth = value > this.Width - 10 ? this.Width - 10 : (value < 10 ? 10 : value); this.Invalidate(); }
        }

        /// <summary>Utility method for setting the minimum, maximum and current value properties in one call.</summary>
        public void SetValues(int minimum, int maximum, int value)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.Value = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(MyGUIs.Background.Normal.Color);
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, MyProgressBar.VerticalBarPadding, this.Width - this.valueBoxWidth, this.Height - 2 * MyProgressBar.VerticalBarPadding);
            if (this.value > this.minimum)
                e.Graphics.FillRectangle(new SolidBrush(this.ForeColor), 0, MyProgressBar.VerticalBarPadding,
                    ((float) (this.value - this.minimum) / (this.maximum - this.minimum)) * (this.Width - this.valueBoxWidth),
                    this.Height - 2 * MyProgressBar.VerticalBarPadding);

            e.Graphics.FillRectangle(new SolidBrush(this.fontBackColor), this.Width - this.valueBoxWidth, 0, this.valueBoxWidth, this.Height);
            string text = string.Format(this.valueFormat, this.value, this.minimum, this.maximum);
            SizeF size = e.Graphics.MeasureString(text, this.Font);
            e.Graphics.DrawString(text, this.Font, new SolidBrush(this.fontForeColor), this.Width - this.valueBoxWidth / 2 - size.Width / 2, this.Height / 2 - size.Height / 2);
        }
    }
}
