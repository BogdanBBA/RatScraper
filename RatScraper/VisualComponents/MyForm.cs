using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatScraper.VisualComponents
{
    /// <summary>
    /// Custom BBA-configured extension of the Form class.
    /// </summary>
    public partial class MyForm : Form
    {
        /// <summary>The pen used to draw the form accent.</summary>
        protected static readonly Pen formAccentPen = new Pen(MyGUIs.Accent.Normal.Color, 2);

        /// <summary>A Point variable used for moving the form with child controls registered for that purpose with MyForm.RegisterControlsToMoveForm().</summary>
        protected Point downPoint = Point.Empty;

        /// <summary>Reference to the main form of the application. Null only if this is the actual main form.</summary>
        protected FMain mainForm;
        
        [Obsolete("Constructor used only for the designer view. Calls this(FMain mainForm).", true)]
        /// <summary>Constructs a new MyForm object with default attributes.</summary>
        private MyForm()
            : this(null)
        {
        }

        /// <summary>Constructs a new MyForm object with default attributes.</summary>
        public MyForm(FMain mainForm)
            : base()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ControlBox = false;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.ForeColor = MyGUIs.Text.Normal.Color;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            this.drawFormAccent = true;
            this.mainForm = mainForm;
        }

        private bool drawFormAccent;
        /// <summary>Gets or sets a value indicating whether to draw accent graphics to the form.</summary>
        public bool DrawFormAccent
        {
            get { return this.drawFormAccent; }
            set { this.drawFormAccent = value; this.Invalidate(); }
        }

        /// <summary>Updates the information displayed on the form for the given argument. To be overridden in derived classes if necessary.
        /// If you see this documentation from a derived class, then a refresh call is not necessary.</summary>
        public virtual void RefreshInformation(object item)
        { }

        /// <summary>Assigns mouse event handlers to the given controls so that when the user clicks and drags any of those controls, the form will move with them.</summary>
        public void RegisterControlsToMoveForm(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.MouseDown += this.ForMoving_MouseDown;
                control.MouseMove += this.ForMoving_MouseMove;
                control.MouseUp += this.ForMoving_MouseUp;
            }
        }

        private void ForMoving_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            this.downPoint = new Point(e.X, e.Y);
        }

        private void ForMoving_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.downPoint == Point.Empty)
                return;
            this.Location = new Point(this.Left + e.X - this.downPoint.X, this.Top + e.Y - this.downPoint.Y);
        }

        private void ForMoving_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            this.downPoint = Point.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(MyGUIs.Background.Normal.Color);
            if (this.drawFormAccent)
                e.Graphics.DrawRectangle(formAccentPen, 3, 3, this.Width - 6, this.Height - 6);
        }
    }
}
