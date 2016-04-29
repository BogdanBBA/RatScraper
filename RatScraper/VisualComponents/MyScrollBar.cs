using System;
using System.Drawing;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    public class MyScrollBar : Control
    {
        public enum ScrollBarPosition { Bottom, Right };

        private int startPosition;
        private int visibleSize;
        private int totalSize;

        private bool mouseIsOver;
        private bool mouseIsClicked;
        private Point lastPosition;
        private Action<double> mouseDragScroll_EventHandler;

        private ScrollBarPosition position;
        public ScrollBarPosition Position
        {
            get { return this.position; }
            set { this.position = value; this.Invalidate(); }
        }

        public MyScrollBar(ScrollBarPosition position, Action<double> mouseDragScroll_EventHandler)
            : base()
        {
            this.position = position;
            this.mouseIsOver = false;
            this.mouseIsClicked = false;
            this.mouseDragScroll_EventHandler = mouseDragScroll_EventHandler;
            this.UpdateScrollBarScroll(0, 0, 0, false);
        }

        public void UpdateScrollBarScroll()
        {
            this.UpdateScrollBarScroll(this.startPosition, this.visibleSize, this.totalSize, true);
        }

        public void UpdateScrollBarScroll(int startPosition, int visibleSize, int totalSize, bool invalidate)
        {
            this.startPosition = startPosition;
            this.visibleSize = visibleSize;
            this.totalSize = totalSize;
            if (invalidate)
                this.Invalidate();
        }

        protected void DoTheMouseDragScrollThing()
        {
            float cursorPosition = this.position == ScrollBarPosition.Right ? this.lastPosition.Y : this.lastPosition.X;
            int barTotalLength = this.position == ScrollBarPosition.Right ? this.Height : this.Width;
            float barLength = (float) this.visibleSize / (this.totalSize > 0 ? this.totalSize : 1) * barTotalLength;
            cursorPosition = cursorPosition < barLength / 2 ? barLength / 2 : (cursorPosition >= barTotalLength - barLength / 2 ? barTotalLength - barLength / 2 : cursorPosition);
            this.mouseDragScroll_EventHandler((cursorPosition - barLength / 2) / this.Height);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.mouseIsOver = true;
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.lastPosition = e.Location;
            if (this.mouseIsClicked)
                this.DoTheMouseDragScrollThing();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseIsOver = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mouseIsClicked = true;
            this.DoTheMouseDragScrollThing();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouseIsClicked = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.Clear(MyGUIs.Background.GetValue(this.mouseIsOver).Color);
            int barTotalLength = this.position == ScrollBarPosition.Right ? this.Height : this.Width;
            float start = (float) this.startPosition / (this.totalSize > 0 ? this.totalSize : 1) * barTotalLength;
            float length = (float) this.visibleSize / (this.totalSize > 0 ? this.totalSize : 1) * barTotalLength;
            RectangleF barRect = this.position == ScrollBarPosition.Right ? new RectangleF(0, start, this.Width, length) : new RectangleF(start, 0, length, this.Height);
            pe.Graphics.FillRectangle(MyGUIs.Accent.GetValue(this.mouseIsClicked).Brush, barRect);
        }
    }
}
