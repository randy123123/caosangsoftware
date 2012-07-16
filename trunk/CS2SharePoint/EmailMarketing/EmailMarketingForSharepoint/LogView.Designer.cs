namespace EmailMarketingForSharepoint
{
    partial class LogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogView));
            this.EmailMarketing = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowHide = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonHide = new System.Windows.Forms.Button();
            this.labelMsg = new System.Windows.Forms.Label();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailMarketing
            // 
            this.EmailMarketing.BalloonTipText = "Email Marketing For SharePoint";
            this.EmailMarketing.BalloonTipTitle = "The tools call send email of GoldenWave";
            this.EmailMarketing.ContextMenuStrip = this.contextMenu;
            this.EmailMarketing.Icon = ((System.Drawing.Icon)(resources.GetObject("EmailMarketing.Icon")));
            this.EmailMarketing.Text = "Email Marketing For SharePoint";
            this.EmailMarketing.Visible = true;
            this.EmailMarketing.DoubleClick += new System.EventHandler(this.EmailMarketing_DoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowHide,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // ShowHide
            // 
            this.ShowHide.Name = "ShowHide";
            this.ShowHide.Size = new System.Drawing.Size(152, 22);
            this.ShowHide.Text = "Show/Hide";
            this.ShowHide.Click += new System.EventHandler(this.ShowHide_Click);
            // 
            // showHideToolStripMenuItem
            // 
            this.showHideToolStripMenuItem.Name = "showHideToolStripMenuItem";
            this.showHideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showHideToolStripMenuItem.Text = "Show/Hide";
            // 
            // buttonHide
            // 
            this.buttonHide.BackgroundImage = global::EmailMarketingForSharepoint.Properties.Resources._2012_07_03_151726_1;
            this.buttonHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonHide.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonHide.FlatAppearance.BorderSize = 0;
            this.buttonHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHide.ForeColor = System.Drawing.Color.Transparent;
            this.buttonHide.Location = new System.Drawing.Point(434, 0);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(108, 48);
            this.buttonHide.TabIndex = 0;
            this.buttonHide.Text = "Hide";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // labelMsg
            // 
            this.labelMsg.BackColor = System.Drawing.Color.Transparent;
            this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMsg.Location = new System.Drawing.Point(12, 13);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(416, 21);
            this.labelMsg.TabIndex = 1;
            this.labelMsg.Text = "Working...";
            this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // LogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EmailMarketingForSharepoint.Properties.Resources._2012_07_03_151726;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(542, 48);
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.labelMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Views";
            this.Load += new System.EventHandler(this.LogView_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LogView_MouseMove);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon EmailMarketing;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem showHideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowHide;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

