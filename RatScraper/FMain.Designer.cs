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
            this.infoView2 = new RatScraper.VisualComponents.InfoView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.clearSearchB = new RatScraper.VisualComponents.MyButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.moreB = new RatScraper.VisualComponents.MyButton();
            this.exitB = new RatScraper.VisualComponents.MyButton();
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
            this.infoView1.TextDescription = "Pleci din următoarea";
            this.infoView1.TextText = "Stație";
            // 
            // infoView2
            // 
            this.infoView2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(53)))), ((int)(((byte)(56)))));
            this.infoView2.BigBar = false;
            this.infoView2.Checked = false;
            this.infoView2.DrawBar = true;
            this.infoView2.Location = new System.Drawing.Point(268, 108);
            this.infoView2.Name = "infoView2";
            this.infoView2.Size = new System.Drawing.Size(250, 52);
            this.infoView2.TabIndex = 2;
            this.infoView2.Text = "infoView2";
            this.infoView2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.infoView2.TextDescription = "Ai următoarele";
            this.infoView2.TextText = "Opțiuni";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 172);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(214, 30);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "text here";
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
            this.clearSearchB.Location = new System.Drawing.Point(232, 172);
            this.clearSearchB.Name = "clearSearchB";
            this.clearSearchB.Size = new System.Drawing.Size(30, 30);
            this.clearSearchB.TabIndex = 4;
            this.clearSearchB.Text = "×";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 21;
            this.listBox1.Location = new System.Drawing.Point(12, 208);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(250, 319);
            this.listBox1.TabIndex = 5;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 21;
            this.listBox2.Location = new System.Drawing.Point(268, 166);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(250, 361);
            this.listBox2.TabIndex = 6;
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
            this.moreB.Location = new System.Drawing.Point(268, 544);
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
            this.exitB.Location = new System.Drawing.Point(474, 544);
            this.exitB.Name = "exitB";
            this.exitB.Size = new System.Drawing.Size(200, 50);
            this.exitB.TabIndex = 8;
            this.exitB.Text = "EXIT";
            // 
            // FMain
            // 
            this.ClientSize = new System.Drawing.Size(946, 618);
            this.Controls.Add(this.exitB);
            this.Controls.Add(this.moreB);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.clearSearchB);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.infoView2);
            this.Controls.Add(this.infoView1);
            this.Controls.Add(this.titleLabel1);
            this.Name = "FMain";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VisualComponents.TitleLabel titleLabel1;
        private VisualComponents.InfoView infoView1;
        private VisualComponents.InfoView infoView2;
        private System.Windows.Forms.TextBox textBox1;
        private VisualComponents.MyButton clearSearchB;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private VisualComponents.MyButton moreB;
        private VisualComponents.MyButton exitB;

    }
}

