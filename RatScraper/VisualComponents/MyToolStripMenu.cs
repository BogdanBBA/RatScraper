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
    /// 
    /// </summary>
    public class MyContextMenuStrip : ContextMenuStrip
    {
        public MyContextMenuStrip()
            : base()
        {
            this.BackColor = MyGUIs.Background.Normal.Color;
            this.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
            this.Renderer = new MyRenderer();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyRenderer : ToolStripProfessionalRenderer
    {
        protected static readonly Font textFont = new Font("Segoe UI", 16, FontStyle.Bold);
        protected static readonly Pair<int> barHeight = new Pair<int>(2, 4);

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            e.Graphics.Clear(MyGUIs.Background.GetValue(e.Item.Selected).Color);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Size size = e.Graphics.MeasureString(e.Text, textFont).ToSize();
            e.Graphics.DrawString(e.Text, textFont, MyGUIs.Text.GetValue(e.Item.Selected).Brush,
                e.Item.Width / 2 - size.Width / 2, (e.Item.Height - barHeight.Normal) / 2 - size.Height / 2);
            e.Graphics.FillRectangle(MyGUIs.Accent.GetValue(e.Item.Selected).Brush, 1, e.Item.Height - barHeight.GetValue(e.Item.Selected), e.Item.Width - 2, barHeight.GetValue(e.Item.Selected));
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
        }
    }
}
