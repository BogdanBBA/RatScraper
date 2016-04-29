using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Defines a fancy scroll panel capable of showing controls occupiyng more than the screen space available.
    /// </summary>
    public class MyScrollPanel
    {
        private Panel containerPanel;
        private MyPanel movingPanel;
        private MyScrollBar scrollBar;
        private List<Control> controls;
        private int scrollBarWidth;
        private int currentScrollTop;

        /// <summary>Gets or sets the amount in pixels that the scroll panel should scroll on a mouse wheel event.</summary>
        public int ScrollAmountInPixels { get; set; }

        /// <summary>Constructs a new MyScrollPanel object from the given parameters.</summary>
        /// <param name="containerPanel">the panel which will contain this MyScrollPanel and which will constitute the 'outer panel'</param>
        /// <param name="scrollBarPosition">the position of the scroll bar relative to the contents</param>
        /// <param name="scrollBarWidth">the width in pixels of the scroll bar</param>
        /// <param name="scrollAmountInPixels">the amount in pixels that the scroll panel should scroll on a mouse wheel event</param>
        public MyScrollPanel(Panel containerPanel, MyScrollBar.ScrollBarPosition scrollBarPosition, int scrollBarWidth, int scrollAmountInPixels)
        {
            this.containerPanel = containerPanel;
            //this.containerPanel.MouseWheel += this.MouseWheelScroll_EventHandler;

            this.movingPanel = new MyPanel();
            this.movingPanel.Parent = this.containerPanel;
            this.movingPanel.DrawPanelAccent = false;
            this.movingPanel.BorderStyle = BorderStyle.None;
            //this.movingPanel.MouseWheel += this.MouseWheelScroll_EventHandler;

            this.scrollBar = new MyScrollBar(scrollBarPosition, this.ScrollBarDragScroll_EventHandler);
            this.scrollBar.Parent = this.containerPanel;

            this.controls = new List<Control>();

            this.scrollBarWidth = scrollBarWidth;
            this.currentScrollTop = 0;
            this.ScrollAmountInPixels = scrollAmountInPixels;

            this.ContainerResizeHasHappened();
        }

        /// <summary>Gets the size of the content-usable space (without the scroll bar) displayed on screen at a given moment.
        /// Actual space used by the contained controls can be either smaller or larger than this value.</summary>
        public Size VisibleSize
        {
            get
            {
                return this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right
                    ? new Size(this.containerPanel.Width - this.scrollBar.Width, this.containerPanel.Height)
                    : new Size(this.containerPanel.Width, this.containerPanel.Height - this.scrollBar.Width);
            }
        }

        /// <summary>Gets the size of the space used by the contained controls.
        /// Depending on this returned size and the visible size of the MyScrollPanel, not all controls may be visible on screen at a given moment.</summary>
        public Size ContentsSize
        {
            get { return this.movingPanel.Size; }
        }

        /// <summary>Resizes and refreshes the scroll bar (as the contained controls will keep their bounds).</summary>
        public void ContainerResizeHasHappened()
        {
            this.scrollBar.Bounds = this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right
                ? new Rectangle(this.containerPanel.Width - this.scrollBarWidth, 0, this.scrollBarWidth, this.containerPanel.Height)
                : new Rectangle(0, this.containerPanel.Height - this.scrollBarWidth, this.containerPanel.Width, this.scrollBarWidth);

            this.scrollBar.UpdateScrollBarScroll(this.currentScrollTop,
                this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.VisibleSize.Height : this.VisibleSize.Width,
                this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.movingPanel.Height : this.movingPanel.Width,
                true);
        }

        /// <summary>Adds a control to this scroll panel at the given location, and resizes the inner panel if 'refresh' is set to true.
        /// Code-wise, if 'refresh' is true, this calls UpdatePanelSize (which itself calls RefreshScroll).</summary>
        public Control AddControl(Control control, Point location, bool refresh)
        {
            this.controls.Add(control);
            control.Parent = this.movingPanel;
            control.Location = location;
            if (refresh)
                this.UpdatePanelSize();
            return control;
        }

        /// <summary>Checks whether this MyScrollPanel contains the given control.</summary>
        public bool ContainsControl(Control control)
        {
            return this.controls.Contains(control);
        }

        /// <summary>Scrolls the inner panel so that the given control is in view.</summary>
        /// <param name="control">the control to view</param>
        public void ScrollToViewControl(Control control)
        {
            if (!this.ContainsControl(control))
                return;

            if (this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right)
            {
                if (control.Bottom > this.currentScrollTop + this.VisibleSize.Height)
                {
                    this.currentScrollTop = control.Bottom - this.VisibleSize.Height;
                    this.RefreshScroll();
                }
                else if (control.Top < this.currentScrollTop)
                {
                    this.currentScrollTop = control.Top;
                    this.RefreshScroll();
                }
            }
            else if (this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Bottom)
            {
                if (control.Right > this.currentScrollTop + this.VisibleSize.Width)
                {
                    this.currentScrollTop = control.Right - this.VisibleSize.Width;
                    this.RefreshScroll();
                }
                else if (control.Left < this.currentScrollTop)
                {
                    this.currentScrollTop = control.Left;
                    this.RefreshScroll();
                }
            }
        }

        /// <summary>Resizes the 'inner panel' that contains all the controls associated with this scroll panel.
        /// Code-wise, this is called by AddControl and in turn calls RefreshScroll.</summary>
        public void UpdatePanelSize()
        {
            Point upperLeft = Point.Empty, lowerRight = Point.Empty;
            foreach (Control control in this.controls)
                if (control.Visible)
                {
                    upperLeft = Utils.MinimumPointValues(upperLeft, control.Location);
                    lowerRight = Utils.MaximumPointValues(lowerRight, new Point(control.Right, control.Bottom));
                }
            this.movingPanel.Size = new Size(lowerRight.X - upperLeft.X, lowerRight.Y - upperLeft.Y);

            if (this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right
                && this.currentScrollTop + this.VisibleSize.Height > this.ContentsSize.Height)
                this.currentScrollTop = Math.Max(0, Math.Min(this.currentScrollTop, this.ContentsSize.Height - this.VisibleSize.Height));

            else if (this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Bottom
                && this.currentScrollTop + this.VisibleSize.Width > this.ContentsSize.Width)
                this.currentScrollTop = Math.Max(0, Math.Min(this.currentScrollTop, this.ContentsSize.Width - this.VisibleSize.Width));

            this.RefreshScroll();
            this.scrollBar.Visible = this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.movingPanel.Height > this.containerPanel.Height : this.movingPanel.Width > this.containerPanel.Width;
        }

        /// <summary>Updates the location of the 'inner panel' that contains all the controls associated with this scroll panel and refreshes the scroll bar.
        /// Code-wise, this is called by UpdatePanelSize, ScrollBarDragScroll_EventHandler and MouseWheelScroll_EventHandler.</summary>
        public void RefreshScroll()
        {
            this.movingPanel.Location = this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right
                ? new Point(0, -this.currentScrollTop)
                : new Point(-this.currentScrollTop, 0);

            this.scrollBar.UpdateScrollBarScroll(this.currentScrollTop,
                this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.VisibleSize.Height : this.VisibleSize.Width,
                this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.movingPanel.Height : this.movingPanel.Width,
                true);
        }

        /// <summary>This is the event that is called by the scroll bar when an mouse drag scroll update is needed. Unless you're working with the scroll bar, don't mind it.</summary>
        protected void ScrollBarDragScroll_EventHandler(double percentage)
        {
            this.currentScrollTop = (int) ((this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right ? this.ContentsSize.Height : this.ContentsSize.Width) * percentage);
            this.RefreshScroll();
        }

        /// <summary>The mouse wheel scroll event handler to be set to the containing form, so that the panel will scroll with the mouse.
        /// Code-wise, this calls RefreshScroll.</summary>
        public void MouseWheelScroll_EventHandler(object sender, MouseEventArgs e) // e.Delta > 0 means "scroll up"
        {
            //Console.WriteLine(sender.ToString());
            KeyValuePair<int, int> sizes = this.scrollBar.Position == MyScrollBar.ScrollBarPosition.Right
                ? new KeyValuePair<int, int>(this.movingPanel.Height, this.containerPanel.Height)
                : new KeyValuePair<int, int>(this.movingPanel.Width, this.containerPanel.Width);
            int amount = Math.Sign(e.Delta) * -this.ScrollAmountInPixels;

            int newScrollTop = this.currentScrollTop + amount;
            int scrollTopLimit = sizes.Key - sizes.Value;
            this.currentScrollTop = sizes.Key > sizes.Value ? (newScrollTop <= 0 ? 0 : (newScrollTop >= scrollTopLimit ? scrollTopLimit : newScrollTop)) : 0;
            this.RefreshScroll();
        }
    }
}
