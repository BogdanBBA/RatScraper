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
            this.stopFilterIV = new RatScraper.VisualComponents.InfoView();
            this.routeIV = new RatScraper.VisualComponents.InfoView();
            this.stopFilterTB = new System.Windows.Forms.TextBox();
            this.clearSearchB = new RatScraper.VisualComponents.MyButton();
            this.moreB = new RatScraper.VisualComponents.MyButton();
            this.exitB = new RatScraper.VisualComponents.MyButton();
            this.updateB = new RatScraper.VisualComponents.MyButton();
            this.stopP = new RatScraper.VisualComponents.MyPanel();
            this.stopFilterT = new System.Windows.Forms.Timer(this.components);
            this.routeP = new RatScraper.VisualComponents.MyPanel();
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
            this.titleTL.Size = new System.Drawing.Size(922, 90);
            this.titleTL.TabIndex = 0;
            this.titleTL.Text = "titleLabel1";
            this.titleTL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.titleTL.TextSubtitle = "Timetable helper";
            this.titleTL.TextTitle = "RAT Brașov";
            // 
            // stopFilterIV
            // 
            this.stopFilterIV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.stopFilterIV.BigBar = false;
            this.stopFilterIV.Checked = false;
            this.stopFilterIV.DrawBar = true;
            this.stopFilterIV.Location = new System.Drawing.Point(12, 108);
            this.stopFilterIV.Name = "stopFilterIV";
            this.stopFilterIV.Size = new System.Drawing.Size(240, 52);
            this.stopFilterIV.TabIndex = 1;
            this.stopFilterIV.Text = "infoView1";
            this.stopFilterIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.stopFilterIV.TextDescription = " ";
            this.stopFilterIV.TextText = "De unde pleci?";
            // 
            // routeIV
            // 
            this.routeIV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.routeIV.BigBar = false;
            this.routeIV.Checked = false;
            this.routeIV.DrawBar = true;
            this.routeIV.Location = new System.Drawing.Point(258, 108);
            this.routeIV.Name = "routeIV";
            this.routeIV.Size = new System.Drawing.Size(360, 52);
            this.routeIV.TabIndex = 2;
            this.routeIV.Text = "infoView2";
            this.routeIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.routeIV.TextDescription = " ";
            this.routeIV.TextText = "Ce iei?";
            // 
            // stopFilterTB
            // 
            this.stopFilterTB.BackColor = System.Drawing.SystemColors.WindowText;
            this.stopFilterTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stopFilterTB.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopFilterTB.ForeColor = System.Drawing.Color.White;
            this.stopFilterTB.Location = new System.Drawing.Point(12, 166);
            this.stopFilterTB.Name = "stopFilterTB";
            this.stopFilterTB.Size = new System.Drawing.Size(204, 23);
            this.stopFilterTB.TabIndex = 3;
            this.stopFilterTB.TextChanged += new System.EventHandler(this.stopFilterTB_TextChanged);
            // 
            // clearSearchB
            // 
            this.clearSearchB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.clearSearchB.BigBar = false;
            this.clearSearchB.Checked = false;
            this.clearSearchB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearSearchB.DrawBar = false;
            this.clearSearchB.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.clearSearchB.Image = null;
            this.clearSearchB.Location = new System.Drawing.Point(222, 166);
            this.clearSearchB.Name = "clearSearchB";
            this.clearSearchB.Size = new System.Drawing.Size(30, 23);
            this.clearSearchB.TabIndex = 4;
            this.clearSearchB.Text = "×";
            this.clearSearchB.Click += new System.EventHandler(this.clearSearchB_Click);
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
            this.moreB.Location = new System.Drawing.Point(367, 544);
            this.moreB.Name = "moreB";
            this.moreB.Size = new System.Drawing.Size(200, 50);
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
            this.exitB.Location = new System.Drawing.Point(573, 544);
            this.exitB.Name = "exitB";
            this.exitB.Size = new System.Drawing.Size(200, 50);
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
            this.updateB.Location = new System.Drawing.Point(161, 544);
            this.updateB.Name = "updateB";
            this.updateB.Size = new System.Drawing.Size(200, 50);
            this.updateB.TabIndex = 9;
            this.updateB.Text = "UPDATE";
            this.updateB.Click += new System.EventHandler(this.updateB_Click);
            // 
            // stopP
            // 
            this.stopP.DrawPanelAccent = false;
            this.stopP.Location = new System.Drawing.Point(12, 195);
            this.stopP.Name = "stopP";
            this.stopP.Size = new System.Drawing.Size(240, 329);
            this.stopP.TabIndex = 10;
            // 
            // stopFilterT
            // 
            this.stopFilterT.Interval = 400;
            this.stopFilterT.Tick += new System.EventHandler(this.stopFilterT_Tick);
            // 
            // routeP
            // 
            this.routeP.DrawPanelAccent = false;
            this.routeP.Location = new System.Drawing.Point(258, 166);
            this.routeP.Name = "routeP";
            this.routeP.Size = new System.Drawing.Size(360, 358);
            this.routeP.TabIndex = 11;
            // 
            // FMain
            // 
            this.ClientSize = new System.Drawing.Size(946, 618);
            this.Controls.Add(this.routeP);
            this.Controls.Add(this.stopP);
            this.Controls.Add(this.updateB);
            this.Controls.Add(this.exitB);
            this.Controls.Add(this.moreB);
            this.Controls.Add(this.clearSearchB);
            this.Controls.Add(this.stopFilterTB);
            this.Controls.Add(this.routeIV);
            this.Controls.Add(this.stopFilterIV);
            this.Controls.Add(this.titleTL);
            this.Name = "FMain";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VisualComponents.TitleLabel titleTL;
        private VisualComponents.InfoView stopFilterIV;
        private VisualComponents.InfoView routeIV;
        private System.Windows.Forms.TextBox stopFilterTB;
        private VisualComponents.MyButton clearSearchB;
        private VisualComponents.MyButton moreB;
        private VisualComponents.MyButton exitB;
        private VisualComponents.MyButton updateB;
        private VisualComponents.MyPanel stopP;
        private System.Windows.Forms.Timer stopFilterT;
        private VisualComponents.MyPanel routeP;

    }
}

