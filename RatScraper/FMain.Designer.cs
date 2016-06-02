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
            this.components = new System.ComponentModel.Container();
            this.titleTL = new RatScraper.VisualComponents.TitleLabel();
            this.stopFilter1IV = new RatScraper.VisualComponents.InfoView();
            this.routeIV = new RatScraper.VisualComponents.InfoView();
            this.stopFilter1TB = new System.Windows.Forms.TextBox();
            this.clearFilter1B = new RatScraper.VisualComponents.MyButton();
            this.moreB = new RatScraper.VisualComponents.MyButton();
            this.exitB = new RatScraper.VisualComponents.MyButton();
            this.updateB = new RatScraper.VisualComponents.MyButton();
            this.stops1P = new RatScraper.VisualComponents.MyPanel();
            this.stopFilter1T = new System.Windows.Forms.Timer(this.components);
            this.routeP = new RatScraper.VisualComponents.MyPanel();
            this.stopTimeP = new RatScraper.VisualComponents.MyPanel();
            this.stopTimeIV = new RatScraper.VisualComponents.InfoView();
            this.stops2P = new RatScraper.VisualComponents.MyPanel();
            this.clearFilter2B = new RatScraper.VisualComponents.MyButton();
            this.stopFilter2TB = new System.Windows.Forms.TextBox();
            this.stopFilter2IV = new RatScraper.VisualComponents.InfoView();
            this.stopFilter2T = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // titleTL
            // 
            this.titleTL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.titleTL.BigBar = false;
            this.titleTL.Checked = false;
            this.titleTL.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.titleTL.DrawBar = true;
            this.titleTL.Location = new System.Drawing.Point(12, 12);
            this.titleTL.Name = "titleTL";
            this.titleTL.Size = new System.Drawing.Size(1176, 90);
            this.titleTL.SupportsAnimation = true;
            this.titleTL.TabIndex = 0;
            this.titleTL.Text = "titleLabel1";
            this.titleTL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.titleTL.TextSubtitle = "Timetable helper";
            this.titleTL.TextTitle = "RAT Brașov";
            // 
            // stopFilter1IV
            // 
            this.stopFilter1IV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.stopFilter1IV.BigBar = false;
            this.stopFilter1IV.Checked = false;
            this.stopFilter1IV.DrawBar = true;
            this.stopFilter1IV.Location = new System.Drawing.Point(12, 108);
            this.stopFilter1IV.Name = "stopFilter1IV";
            this.stopFilter1IV.Size = new System.Drawing.Size(240, 52);
            this.stopFilter1IV.SupportsAnimation = true;
            this.stopFilter1IV.TabIndex = 1;
            this.stopFilter1IV.Text = "infoView1";
            this.stopFilter1IV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stopFilter1IV.TextDescription = " ";
            this.stopFilter1IV.TextText = "De unde pleci?";
            // 
            // routeIV
            // 
            this.routeIV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.routeIV.BigBar = false;
            this.routeIV.Checked = false;
            this.routeIV.DrawBar = true;
            this.routeIV.Location = new System.Drawing.Point(504, 108);
            this.routeIV.Name = "routeIV";
            this.routeIV.Size = new System.Drawing.Size(368, 52);
            this.routeIV.SupportsAnimation = true;
            this.routeIV.TabIndex = 2;
            this.routeIV.Text = "infoView2";
            this.routeIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.routeIV.TextDescription = " ";
            this.routeIV.TextText = "Ce iei?";
            // 
            // stopFilter1TB
            // 
            this.stopFilter1TB.BackColor = System.Drawing.SystemColors.WindowText;
            this.stopFilter1TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stopFilter1TB.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopFilter1TB.ForeColor = System.Drawing.Color.White;
            this.stopFilter1TB.Location = new System.Drawing.Point(12, 166);
            this.stopFilter1TB.Name = "stopFilter1TB";
            this.stopFilter1TB.Size = new System.Drawing.Size(204, 23);
            this.stopFilter1TB.TabIndex = 3;
            this.stopFilter1TB.Tag = "Caută";
            this.stopFilter1TB.TextChanged += new System.EventHandler(this.stopFilter1TB_TextChanged);
            // 
            // clearFilter1B
            // 
            this.clearFilter1B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.clearFilter1B.BigBar = false;
            this.clearFilter1B.Checked = false;
            this.clearFilter1B.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearFilter1B.DrawBar = false;
            this.clearFilter1B.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.clearFilter1B.Image = null;
            this.clearFilter1B.Location = new System.Drawing.Point(222, 166);
            this.clearFilter1B.Name = "clearFilter1B";
            this.clearFilter1B.Size = new System.Drawing.Size(30, 23);
            this.clearFilter1B.SupportsAnimation = true;
            this.clearFilter1B.TabIndex = 4;
            this.clearFilter1B.Text = "×";
            this.clearFilter1B.Click += new System.EventHandler(this.clearSearch1B_Click);
            // 
            // moreB
            // 
            this.moreB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.moreB.BigBar = false;
            this.moreB.Checked = false;
            this.moreB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moreB.DrawBar = true;
            this.moreB.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moreB.Image = null;
            this.moreB.Location = new System.Drawing.Point(496, 638);
            this.moreB.Name = "moreB";
            this.moreB.Size = new System.Drawing.Size(200, 50);
            this.moreB.SupportsAnimation = true;
            this.moreB.TabIndex = 7;
            this.moreB.Text = "[ MORE ]";
            // 
            // exitB
            // 
            this.exitB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.exitB.BigBar = false;
            this.exitB.Checked = false;
            this.exitB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitB.DrawBar = true;
            this.exitB.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitB.Image = null;
            this.exitB.Location = new System.Drawing.Point(702, 638);
            this.exitB.Name = "exitB";
            this.exitB.Size = new System.Drawing.Size(200, 50);
            this.exitB.SupportsAnimation = true;
            this.exitB.TabIndex = 8;
            this.exitB.Text = "EXIT";
            this.exitB.Click += new System.EventHandler(this.exitB_Click);
            // 
            // updateB
            // 
            this.updateB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.updateB.BigBar = false;
            this.updateB.Checked = false;
            this.updateB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.updateB.DrawBar = true;
            this.updateB.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateB.Image = null;
            this.updateB.Location = new System.Drawing.Point(290, 638);
            this.updateB.Name = "updateB";
            this.updateB.Size = new System.Drawing.Size(200, 50);
            this.updateB.SupportsAnimation = true;
            this.updateB.TabIndex = 9;
            this.updateB.Text = "UPDATE";
            this.updateB.Click += new System.EventHandler(this.updateB_Click);
            // 
            // stops1P
            // 
            this.stops1P.DrawPanelAccent = false;
            this.stops1P.Location = new System.Drawing.Point(12, 195);
            this.stops1P.Name = "stops1P";
            this.stops1P.Size = new System.Drawing.Size(240, 437);
            this.stops1P.TabIndex = 10;
            // 
            // stopFilter1T
            // 
            this.stopFilter1T.Interval = 400;
            this.stopFilter1T.Tick += new System.EventHandler(this.stopFilter1T_Tick);
            // 
            // routeP
            // 
            this.routeP.DrawPanelAccent = false;
            this.routeP.Location = new System.Drawing.Point(504, 166);
            this.routeP.Name = "routeP";
            this.routeP.Size = new System.Drawing.Size(368, 466);
            this.routeP.TabIndex = 11;
            // 
            // stopTimeP
            // 
            this.stopTimeP.DrawPanelAccent = false;
            this.stopTimeP.Location = new System.Drawing.Point(878, 166);
            this.stopTimeP.Name = "stopTimeP";
            this.stopTimeP.Size = new System.Drawing.Size(310, 466);
            this.stopTimeP.TabIndex = 13;
            // 
            // stopTimeIV
            // 
            this.stopTimeIV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.stopTimeIV.BigBar = false;
            this.stopTimeIV.Checked = false;
            this.stopTimeIV.DrawBar = true;
            this.stopTimeIV.Location = new System.Drawing.Point(878, 108);
            this.stopTimeIV.Name = "stopTimeIV";
            this.stopTimeIV.Size = new System.Drawing.Size(310, 52);
            this.stopTimeIV.SupportsAnimation = true;
            this.stopTimeIV.TabIndex = 12;
            this.stopTimeIV.Text = "infoView2";
            this.stopTimeIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stopTimeIV.TextDescription = " ";
            this.stopTimeIV.TextText = "Fii în stație la:";
            // 
            // stops2P
            // 
            this.stops2P.DrawPanelAccent = false;
            this.stops2P.Location = new System.Drawing.Point(258, 195);
            this.stops2P.Name = "stops2P";
            this.stops2P.Size = new System.Drawing.Size(240, 437);
            this.stops2P.TabIndex = 17;
            // 
            // clearFilter2B
            // 
            this.clearFilter2B.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.clearFilter2B.BigBar = false;
            this.clearFilter2B.Checked = false;
            this.clearFilter2B.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearFilter2B.DrawBar = false;
            this.clearFilter2B.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.clearFilter2B.Image = null;
            this.clearFilter2B.Location = new System.Drawing.Point(468, 166);
            this.clearFilter2B.Name = "clearFilter2B";
            this.clearFilter2B.Size = new System.Drawing.Size(30, 23);
            this.clearFilter2B.SupportsAnimation = true;
            this.clearFilter2B.TabIndex = 16;
            this.clearFilter2B.Text = "×";
            this.clearFilter2B.Click += new System.EventHandler(this.clearFilter2B_Click);
            // 
            // stopFilter2TB
            // 
            this.stopFilter2TB.BackColor = System.Drawing.SystemColors.WindowText;
            this.stopFilter2TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stopFilter2TB.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopFilter2TB.ForeColor = System.Drawing.Color.White;
            this.stopFilter2TB.Location = new System.Drawing.Point(258, 166);
            this.stopFilter2TB.Name = "stopFilter2TB";
            this.stopFilter2TB.Size = new System.Drawing.Size(204, 23);
            this.stopFilter2TB.TabIndex = 15;
            this.stopFilter2TB.Tag = "Oriunde";
            this.stopFilter2TB.TextChanged += new System.EventHandler(this.stopFilter2TB_TextChanged);
            // 
            // stopFilter2IV
            // 
            this.stopFilter2IV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.stopFilter2IV.BigBar = false;
            this.stopFilter2IV.Checked = false;
            this.stopFilter2IV.DrawBar = true;
            this.stopFilter2IV.Location = new System.Drawing.Point(258, 108);
            this.stopFilter2IV.Name = "stopFilter2IV";
            this.stopFilter2IV.Size = new System.Drawing.Size(240, 52);
            this.stopFilter2IV.SupportsAnimation = true;
            this.stopFilter2IV.TabIndex = 14;
            this.stopFilter2IV.Text = "infoView1";
            this.stopFilter2IV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stopFilter2IV.TextDescription = " ";
            this.stopFilter2IV.TextText = "Unde mergi?";
            // 
            // stopFilter2T
            // 
            this.stopFilter2T.Interval = 400;
            this.stopFilter2T.Tick += new System.EventHandler(this.stopFilter2T_Tick);
            // 
            // FMain
            // 
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.stops2P);
            this.Controls.Add(this.clearFilter2B);
            this.Controls.Add(this.stopFilter2TB);
            this.Controls.Add(this.stopFilter2IV);
            this.Controls.Add(this.stopTimeP);
            this.Controls.Add(this.stopTimeIV);
            this.Controls.Add(this.routeP);
            this.Controls.Add(this.stops1P);
            this.Controls.Add(this.updateB);
            this.Controls.Add(this.exitB);
            this.Controls.Add(this.moreB);
            this.Controls.Add(this.clearFilter1B);
            this.Controls.Add(this.stopFilter1TB);
            this.Controls.Add(this.routeIV);
            this.Controls.Add(this.stopFilter1IV);
            this.Controls.Add(this.titleTL);
            this.Name = "FMain";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VisualComponents.TitleLabel titleTL;
        private VisualComponents.InfoView stopFilter1IV;
        private VisualComponents.InfoView routeIV;
        private System.Windows.Forms.TextBox stopFilter1TB;
        private VisualComponents.MyButton clearFilter1B;
        private VisualComponents.MyButton moreB;
        private VisualComponents.MyButton exitB;
        private VisualComponents.MyButton updateB;
        private VisualComponents.MyPanel stops1P;
        private System.Windows.Forms.Timer stopFilter1T;
        private VisualComponents.MyPanel routeP;
        private VisualComponents.InfoView stopTimeIV;
        private VisualComponents.MyPanel stopTimeP;
        private VisualComponents.MyPanel stops2P;
        private VisualComponents.MyButton clearFilter2B;
        private System.Windows.Forms.TextBox stopFilter2TB;
        private VisualComponents.InfoView stopFilter2IV;
        private System.Windows.Forms.Timer stopFilter2T;

    }
}

