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
        /// <summary>Provides several types of mathematical functions that dictate the type of animation progression.</summary>
        public enum EasingFunctions { Linear, QuadraticOut, ExponentialOut, CircularOut };

        /// <summary>Retains whether the mouse cursor is over this control.</summary>
        protected bool mouseIsOver = false;
        /// <summary>Retains whether the mouse cursor is clicked on this control.</summary>
        protected bool mouseIsClicked = false;
        /// <summary>Retains whether this control is checked (this can mean different things depending on the specific implementation).</summary>
        protected bool isChecked = false;

        /// <summary>Indicates whether the control is to allow time-controlled animation.</summary>
        protected bool supportsAnimation = false;
        /// <summary>The duration in milliseconds of the control animation sequence.</summary>
        private int animationDuration;
        /// <summary>The intended interval in milliseconds between the animation frames.</summary>
        private int animationRefreshInterval;
        /// <summary>The number of frames of the animation (this should be calculated automatically in SetAnimationParameters()).</summary>
        private int animationFrameCount;
        /// <summary>The easing function to be used by this control for the animation progression.</summary>
        protected EasingFunctions easingFunction;
        /// <summary>The number of frames drawn so far for the current/latest animation (is modified in the animation tick or the animation start/stop methods).</summary>
        protected int animationFramesDrawn = 0;
        /// <summary>The starting numeric position for the animation (the position can mean different things depending on the specific implementation).</summary>
        protected double animationStartPosition = 0;
        /// <summary>The current numeric position for the current animation (the position is calculated for the current animation tick in function of the start and end position values, and the easing function used).</summary>
        protected double animationCurrentPosition = 0;
        /// <summary>The final numeric position for the animation (the position can mean different things depending on the specific implementation).</summary>
        protected double animationEndPosition = 0;
        /// <summary>The animation timer used to repaint the control periodically during an animation sequence.</summary>
        private Timer animationTimer = new Timer();

        /// <summary>Constructs a new MyAppBaseControl control, initialized with default values.</summary>
        public MyAppBaseControl()
            : base()
        {
            this.BackColor = MyGUIs.Background.Normal.Color;
            this.animationTimer.Tick += animationTimer_Tick;
            this.SetAnimationParameters(false, EasingFunctions.Linear, 1000, 50);
        }

        /// <summary>Gets or sets a value indicating whether this control is checked (this can mean different things depending on the specific implementation).</summary>
        public bool Checked
        {
            get { return this.isChecked; }
            set { if (this.isChecked != value) { this.isChecked = value; this.Invalidate(); } }
        }

        /// <summary>Gets or sets a value indicating whether the control is to allow time-controlled animation.</summary>
        public bool SupportsAnimation
        {
            get { return this.supportsAnimation; }
            set
            {
                if (this.supportsAnimation == value)
                    return;
                if (!value)
                    this.animationTimer.Enabled = false;
                this.supportsAnimation = value;
                this.Invalidate();
            }
        }

        /// <summary>Sets animation-related parameter values. Should be called from the constructor (at least); 
        /// don't forget to also start/stop the animation (probably from the mouse enter/leave methods) and to consider the current animation position inside OnPaint().</summary>
        /// <param name="supportsAnimation">whether the control is to allow time-controlled animation</param>
        /// <param name="easingFunction">the easing function to be used by this control for the animation progression</param>
        /// <param name="animationDuration">the duration in milliseconds of the control animation sequence</param>
        /// <param name="animationRefreshInterval">the intended interval in milliseconds between the animation frames</param>
        public void SetAnimationParameters(bool supportsAnimation, EasingFunctions easingFunction, int animationDuration, int animationRefreshInterval)
        {
            this.supportsAnimation = supportsAnimation;
            this.easingFunction = easingFunction;
            this.animationDuration = animationDuration;
            this.animationRefreshInterval = animationRefreshInterval;
            this.animationFrameCount = (int) ((double) this.animationDuration / this.animationRefreshInterval);
            this.animationTimer.Interval = this.animationRefreshInterval;
        }

        /// <summary>The mouseIsOver field is set to true, the base class OnMouseEnter() method is called, and the control is invalidated.</summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.mouseIsOver = true;
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        /// <summary>The mouseIsOver field is set to false, the base class OnMouseLeave() method is called, and the control is invalidated.</summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseIsOver = false;
            base.OnMouseLeave(e);
            this.Invalidate();
        }

        /// <summary>The mouseIsClicked field is set to true, the base class OnMouseDown() method is called, and the control is invalidated.</summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            this.mouseIsClicked = true;
            base.OnMouseDown(mevent);
            this.Invalidate();
        }

        /// <summary>The mouseIsClicked field is set to false, the base class OnMouseUp() method is called, and the control is invalidated.</summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            this.mouseIsClicked = false;
            base.OnMouseUp(mevent);
            this.Invalidate();
        }

        /// <summary>Sets the animation start and end positions, and (besides re-initializing some other variables) turns on the animation timer.
        /// Note: the start/end positions can be any numeric value (and their order does not matter - although it will probably affect the direction of the animation); 
        /// what they mean depends on the specific implementation of the class (derived from MyAppBaseControl).</summary>
        /// <param name="animationStartPosition">the position from which the animation starts</param>
        /// <param name="animationEndPosition">the position at which the animation ends</param>
        protected void StartAnimation(double animationStartPosition, double animationEndPosition)
        {
            if (!this.supportsAnimation)
                return;
            this.animationTimer.Enabled = false;
            this.animationFramesDrawn = 0;
            this.animationStartPosition = animationStartPosition;
            this.animationCurrentPosition = animationStartPosition;
            this.animationEndPosition = animationEndPosition;
            this.animationTimer_Tick(null, null);
            this.animationTimer.Enabled = true;
        }

        /// <summary>Sets the current animation position value to that of the end position, and disables the animation timer. Does not invalidate the control.</summary>
        protected void StopAnimation()
        {
            this.animationCurrentPosition = this.animationEndPosition;
            this.animationTimer.Enabled = false;
        }

        /// <summary>The animation timer tick. Updates the current animation position, increases the drawn frame count, stops the animation if necessary, and invalidates the control.</summary>
        protected void animationTimer_Tick(object sender, EventArgs e)
        {
            this.animationCurrentPosition = MyAppBaseControl.EaseTransition(this.easingFunction, this.animationStartPosition, this.animationEndPosition, this.animationFramesDrawn, this.animationFrameCount);
            if (++this.animationFramesDrawn > this.animationFrameCount)
                this.StopAnimation();
            this.Invalidate();
        }

        /// <summary>Calculates the value of the current animation position, given the type of animation progression, and the position and frame values.
        /// Note: the start/end positions can be any numeric value; what they mean depends on the specific implementation of the class (derived from MyAppBaseControl).
        /// The numeric order of the positions does not matter, but the frame values must be 0 &lt;= currentFrame &lt;= totalFrames.</summary>
        /// <param name="function">the easing function to be used by this control for the animation progression</param>
        /// <param name="startPosition">the position from which the animation starts</param>
        /// <param name="endPosition">the position at which the animation ends</param>
        /// <param name="currentFrame">the current frame of the animation for which to calculate the position</param>
        /// <param name="totalFrames">the number of frames that will be drawn in total in this animation sequence</param>
        public static double EaseTransition(EasingFunctions function, double startPosition, double endPosition, double currentFrame, double totalFrames)
        {
            //Console.WriteLine("startPosition={0}, endPosition={1}, currentFrame={2}, totalFrames={3}; result={4}", startPosition, endPosition, currentFrame, totalFrames, (int) (-(endPosition - startPosition) * currentFrame * (currentFrame - 2) + startPosition));
            double changeInPosition = endPosition - startPosition;
            switch (function)
            {
                case EasingFunctions.Linear:
                    return startPosition + (currentFrame / totalFrames) * (endPosition - startPosition);
                case EasingFunctions.QuadraticOut:
                    currentFrame /= totalFrames;
                    return -changeInPosition * currentFrame * (currentFrame - 2) + startPosition;
                case EasingFunctions.ExponentialOut:
                    return changeInPosition * (-Math.Pow(2, -10 * currentFrame / totalFrames) + 1) + startPosition;
                case EasingFunctions.CircularOut:
                    currentFrame = (currentFrame / totalFrames) - 1;
                    return changeInPosition * Math.Sqrt(1 - currentFrame * currentFrame) + startPosition;
                default:
                    return -1.0;
            }
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

        /// <summary>Clears the drawing area with the color specified at MyGUIs.Background.GetValue(this.mouseIsOver).Color.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Console.WriteLine("{0}(Name='{1}').OnPaint() (:{2}.{3})", this.GetType().Name, this.Name, DateTime.Now.TimeOfDay.Seconds, DateTime.Now.TimeOfDay.Milliseconds);
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
    }

    /// <summary>
    /// A color-brush-pen resource class.
    /// </summary>
    public class ColorResource
    {
        public Color Color { get; private set; }
        public HSLColor HSLColor { get; private set; }
        public Brush Brush { get; private set; }
        public Pen Pen { get; private set; }

        public ColorResource(Color color)
        {
            this.SetColorAndUpdateResource(color);
        }

        public void SetColorAndUpdateResource(Color color)
        {
            this.Color = color;
            this.HSLColor = new HSLColor(color);
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
