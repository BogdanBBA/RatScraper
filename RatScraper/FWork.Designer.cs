namespace RatScraper
{
    partial class FWork
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.workBW = new System.ComponentModel.BackgroundWorker();
            this.titleLabel1 = new RatScraper.VisualComponents.TitleLabel();
            this.statusIV = new RatScraper.VisualComponents.InfoView();
            this.closeT = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // workBW
            // 
            this.workBW.WorkerReportsProgress = true;
            // 
            // titleLabel1
            // 
            this.titleLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.titleLabel1.BigBar = false;
            this.titleLabel1.Checked = false;
            this.titleLabel1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.titleLabel1.DrawBar = true;
            this.titleLabel1.Location = new System.Drawing.Point(12, 12);
            this.titleLabel1.Name = "titleLabel1";
            this.titleLabel1.Size = new System.Drawing.Size(1311, 90);
            this.titleLabel1.TabIndex = 0;
            this.titleLabel1.Text = "titleLabel1";
            this.titleLabel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.titleLabel1.TextSubtitle = "[Subtitle text]";
            this.titleLabel1.TextTitle = "Working...";
            // 
            // statusIV
            // 
            this.statusIV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.statusIV.BigBar = false;
            this.statusIV.Checked = false;
            this.statusIV.DrawBar = false;
            this.statusIV.Location = new System.Drawing.Point(12, 108);
            this.statusIV.Name = "statusIV";
            this.statusIV.Size = new System.Drawing.Size(1311, 52);
            this.statusIV.TabIndex = 1;
            this.statusIV.Text = "infoView1";
            this.statusIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.statusIV.TextDescription = "Currently";
            this.statusIV.TextText = "#text";
            // 
            // closeT
            // 
            this.closeT.Interval = 1500;
            this.closeT.Tick += new System.EventHandler(this.closeT_Tick);
            // 
            // FWork
            // 
            this.ClientSize = new System.Drawing.Size(1335, 172);
            this.Controls.Add(this.statusIV);
            this.Controls.Add(this.titleLabel1);
            this.Name = "FWork";
            this.Load += new System.EventHandler(this.FWork_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker workBW;
        private VisualComponents.TitleLabel titleLabel1;
        private VisualComponents.InfoView statusIV;
        private System.Windows.Forms.Timer closeT;
    }
}