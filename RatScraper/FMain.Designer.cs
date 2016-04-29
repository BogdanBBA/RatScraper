namespace RatScraper
{
    partial class FMain
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
            this.titleLabel1 = new RatScraper.VisualComponents.TitleLabel();
            this.infoView1 = new RatScraper.VisualComponents.InfoView();
            this.SuspendLayout();
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
            this.titleLabel1.Size = new System.Drawing.Size(922, 90);
            this.titleLabel1.TabIndex = 0;
            this.titleLabel1.Text = "titleLabel1";
            this.titleLabel1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.titleLabel1.TextSubtitle = "Timetable helper";
            this.titleLabel1.TextTitle = "RAT Brașov";
            // 
            // infoView1
            // 
            this.infoView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.infoView1.BigBar = false;
            this.infoView1.Checked = false;
            this.infoView1.DrawBar = true;
            this.infoView1.Location = new System.Drawing.Point(12, 108);
            this.infoView1.Name = "infoView1";
            this.infoView1.Size = new System.Drawing.Size(250, 52);
            this.infoView1.TabIndex = 1;
            this.infoView1.Text = "infoView1";
            this.infoView1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.infoView1.TextDescription = "Pleci din";
            this.infoView1.TextText = "Stația";
            // 
            // FMain
            // 
            this.ClientSize = new System.Drawing.Size(946, 574);
            this.Controls.Add(this.infoView1);
            this.Controls.Add(this.titleLabel1);
            this.Name = "FMain";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VisualComponents.TitleLabel titleLabel1;
        private VisualComponents.InfoView infoView1;

    }
}

