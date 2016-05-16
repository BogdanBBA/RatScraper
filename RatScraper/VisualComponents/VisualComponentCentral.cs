using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Base control from which to derive app-specific visual controls.
    /// </summary>
    public abstract class MyAppBaseControl : Control
    {
        protected const int AnimationDuration = 400;
        protected const int AnimationRefreshInterval = 20;
        protected const int AnimationFrameCount = (int) ((double) MyAppBaseControl.AnimationDuration / MyAppBaseControl.AnimationRefreshInterval);
        public enum EasingFunctions { Linear, QuadraticOut, ExponentialOut, CircularOut };

        protected bool mouseIsOver = false;
        protected bool mouseIsClicked = false;
        protected bool isChecked = false;

        protected bool supportsAnimation = true;
        protected EasingFunctions easingFunction = EasingFunctions.Linear;
        protected int animationFramesDrawn = 0;
        protected int animationStartPosition = 0;
        protected int animationCurrentPosition = 0;
        protected int animationEndPosition = 0;
        protected Timer animationTimer = new Timer();

        public MyAppBaseControl()
            : base()
        {
            this.BackColor = MyGUIs.Background.Normal.Color;
            this.animationTimer.Interval = MyAppBaseControl.AnimationRefreshInterval;
            this.animationTimer.Tick += animationTimer_Tick;
        }

        public bool Checked
        {
            get { return this.isChecked; }
            set { if (this.isChecked != value) { this.isChecked = value; this.Invalidate(); } }
        }

        public bool SupportsAnimation
        {
            get { return this.supportsAnimation; }
            set
            {
                if (this.supportsAnimation != value)
                {
                    if (!value)
                        this.animationTimer.Enabled = false;
                    this.supportsAnimation = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.mouseIsOver = true;
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseIsOver = false;
            base.OnMouseLeave(e);
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            this.mouseIsClicked = true;
            base.OnMouseDown(mevent);
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            this.mouseIsClicked = false;
            base.OnMouseUp(mevent);
            this.Invalidate();
        }

        protected void DrawImageCell(Graphics g, Bitmap image, HorizontalAlignment alignment, double percentageStart, double percentageEnd)
        {
            RectangleF rect = new RectangleF((float) (this.Width * percentageStart), 0, (float) (this.Width * (percentageEnd - percentageStart)), this.Height);
            PointF location = new PointF(alignment == HorizontalAlignment.Left
                ? rect.Left : (alignment == HorizontalAlignment.Center ? rect.Left + rect.Width / 2 - image.Size.Width / 2 : rect.Right - image.Size.Width),
                rect.Height / 2 - image.Size.Height / 2);
            g.DrawImage(image, location);
        }

        protected void DrawTextCell(Graphics g, Font font, string text, HorizontalAlignment alignment, double percentageStart, double percentageEnd)
        {
            RectangleF rect = new RectangleF((float) (this.Width * percentageStart), 0, (float) (this.Width * (percentageEnd - percentageStart)), this.Height);
            SizeF size = g.MeasureString(text, font);
            PointF location = new PointF(alignment == HorizontalAlignment.Left
                ? rect.Left : (alignment == HorizontalAlignment.Center ? rect.Left + rect.Width / 2 - size.Width / 2 : rect.Right - size.Width),
                rect.Height / 2 - size.Height / 2);
            g.DrawString(text, font, MyGUIs.Text[this.mouseIsOver].Brush, location);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(MyGUIs.Background.GetValue(this.mouseIsOver).Color);
        }

        public static List<TYPE> CreateControlCollection<TYPE>(ICollection<string> captions, EventHandler clickEventHandler, string namePrefix, ICollection<Tuple<string, object>> customProperties)
            where TYPE : MyAppBaseControl
        {
            List<TYPE> result = new List<TYPE>();
            int lastNumber = 0;
            foreach (string caption in captions)
            {
                TYPE item = (TYPE) typeof(TYPE).GetConstructor(new Type[] { }).Invoke(new object[] { });
                item.Name = namePrefix + (++lastNumber);
                item.Text = caption;
                item.Click += clickEventHandler;
                if (customProperties != null)
                    foreach (Tuple<string, object> customProperty in customProperties)
                        typeof(TYPE).GetProperty(customProperty.Item1).SetValue(item, customProperty.Item2);
                result.Add(item);
            }
            return result;
        }

        protected void StartAnimation(int animationStartPosition, int animationEndPosition)
        {
            if (!this.supportsAnimation)
                return;
            this.animationTimer.Enabled = false;
            this.animationFramesDrawn = 0;
            this.animationStartPosition = animationStartPosition;
            this.animationCurrentPosition = animationStartPosition;
            this.animationEndPosition = animationEndPosition;
            animationTimer_Tick(null, null);
            this.animationTimer.Enabled = true;
        }

        protected void StopAnimation()
        {
            this.animationCurrentPosition = this.animationEndPosition;
            this.animationTimer.Enabled = false;
        }

        protected void animationTimer_Tick(object sender, EventArgs e)
        {
            this.animationCurrentPosition = MyAppBaseControl.EaseTransition(this.easingFunction, this.animationStartPosition, this.animationEndPosition, this.animationFramesDrawn, MyAppBaseControl.AnimationFrameCount);
            if (++this.animationFramesDrawn > MyAppBaseControl.AnimationFrameCount)
                this.StopAnimation();
            this.Invalidate();
        }

        public static int EaseTransition(EasingFunctions function, double startPosition, double endPosition, double currentFrame, double totalFrames)
        {
            //Console.WriteLine("startPosition={0}, endPosition={1}, currentFrame={2}, totalFrames={3}; result={4}", startPosition, endPosition, currentFrame, totalFrames, (int) (-(endPosition - startPosition) * currentFrame * (currentFrame - 2) + startPosition));
            double changeInPosition = endPosition - startPosition;
            switch (function)
            {
                case EasingFunctions.Linear:
                    return (int) (startPosition + (currentFrame / totalFrames) * (endPosition - startPosition));
                case EasingFunctions.QuadraticOut:
                    currentFrame /= totalFrames;
                    return (int) (-changeInPosition * currentFrame * (currentFrame - 2) + startPosition);
                case EasingFunctions.ExponentialOut:
                    return (int) (changeInPosition * (-Math.Pow(2, -10 * currentFrame / totalFrames) + 1) + startPosition);
                case EasingFunctions.CircularOut:
                    currentFrame = (currentFrame / totalFrames) - 1;
                    return (int) (changeInPosition * Math.Sqrt(1 - currentFrame * currentFrame) + startPosition);
                default:
                    return -1;
            }
        }
    }

    /// <summary>
    /// A color-brush-pen resource class.
    /// </summary>
    public class ColorResource
    {
        public Color Color { get; private set; }
        public Brush Brush { get; private set; }
        public Pen Pen { get; private set; }

        public ColorResource(Color color)
        {
            this.SetColorAndUpdateResource(color);
        }

        public void SetColorAndUpdateResource(Color color)
        {
            this.Color = color;
            this.Brush = new SolidBrush(color);
            this.Pen = new Pen(color);
        }
    }

    /// <summary>
    /// The constant (or at least static) values used for the GUIs.
    /// </summary>
    public static class MyGUIs
    {
        public static Pair<ColorResource> Background;
        public static Pair<ColorResource> Text;
        public static Pair<ColorResource> Accent;
        public static Pair<ColorResource> Category;

        static MyGUIs()
        {
            MyGUIs.Reset();
        }

        /// <summary>Initializes to default values.</summary>
        public static void Reset()
        {
            MyGUIs.Background = new Pair<ColorResource>(new ColorResource(ColorTranslator.FromHtml("#2B3538")), new ColorResource(ColorTranslator.FromHtml("#364145")));
            MyGUIs.Text = new Pair<ColorResource>(new ColorResource(Color.WhiteSmoke), new ColorResource(ColorTranslator.FromHtml("#09ACE3")));
            MyGUIs.Accent = new Pair<ColorResource>(new ColorResource(ColorTranslator.FromHtml("#FFFFFF")), new ColorResource(ColorTranslator.FromHtml("#09ACE3")));
        }
    }
}
