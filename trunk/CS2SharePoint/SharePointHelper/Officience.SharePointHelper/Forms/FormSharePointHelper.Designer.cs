namespace Officience.SharePointHelper
{
    partial class FormSharePointHelper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSharePointHelper));
            this.labelServer = new System.Windows.Forms.Label();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.menuStripFuntions = new System.Windows.Forms.MenuStrip();
            this.menuFuntions = new System.Windows.Forms.ToolStripMenuItem();
            this.labelOutput = new System.Windows.Forms.Label();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.contextMenuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopyLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExport = new System.Windows.Forms.Button();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelWorking = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.menuStripFuntions.SuspendLayout();
            this.contextMenuItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(12, 15);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(69, 13);
            this.labelServer.TabIndex = 0;
            this.labelServer.Text = "Select server";
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Location = new System.Drawing.Point(87, 12);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(482, 21);
            this.comboBoxServer.TabIndex = 1;
            this.comboBoxServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxServer_KeyDown);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.Location = new System.Drawing.Point(575, 10);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // menuStripFuntions
            // 
            this.menuStripFuntions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStripFuntions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFuntions});
            this.menuStripFuntions.Location = new System.Drawing.Point(0, 391);
            this.menuStripFuntions.Name = "menuStripFuntions";
            this.menuStripFuntions.Size = new System.Drawing.Size(665, 24);
            this.menuStripFuntions.TabIndex = 3;
            this.menuStripFuntions.Text = "menuStrip1";
            // 
            // menuFuntions
            // 
            this.menuFuntions.Name = "menuFuntions";
            this.menuFuntions.Size = new System.Drawing.Size(197, 20);
            this.menuFuntions.Text = "--> Select your function to excute";
            this.menuFuntions.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuFuntions_DropDownItemClicked);
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(42, 39);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(39, 13);
            this.labelOutput.TabIndex = 4;
            this.labelOutput.Text = "Output";
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxOutput.ContextMenuStrip = this.contextMenuItems;
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.HorizontalScrollbar = true;
            this.listBoxOutput.Location = new System.Drawing.Point(87, 39);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.ScrollAlwaysVisible = true;
            this.listBoxOutput.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxOutput.Size = new System.Drawing.Size(482, 329);
            this.listBoxOutput.TabIndex = 5;
            // 
            // contextMenuItems
            // 
            this.contextMenuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.menuCopyLogs});
            this.contextMenuItems.Name = "contextMenuItems";
            this.contextMenuItems.Size = new System.Drawing.Size(137, 48);
            // 
            // menuCopy
            // 
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(136, 22);
            this.menuCopy.Text = "&Copy this log";
            this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // menuCopyLogs
            // 
            this.menuCopyLogs.Name = "menuCopyLogs";
            this.menuCopyLogs.Size = new System.Drawing.Size(136, 22);
            this.menuCopyLogs.Text = "Copy &Logs";
            this.menuCopyLogs.Click += new System.EventHandler(this.menuCopyLogs_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(575, 39);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 6;
            this.buttonExport.Text = "Export Logs";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "txt";
            this.saveFileDialogExport.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            this.saveFileDialogExport.InitialDirectory = "C:\\";
            this.saveFileDialogExport.RestoreDirectory = true;
            this.saveFileDialogExport.Title = "Where do you want to save the output file?";
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(575, 68);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 7;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelWorking
            // 
            this.labelWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWorking.AutoSize = true;
            this.labelWorking.Location = new System.Drawing.Point(585, 94);
            this.labelWorking.Name = "labelWorking";
            this.labelWorking.Size = new System.Drawing.Size(56, 13);
            this.labelWorking.TabIndex = 8;
            this.labelWorking.Text = "Working...";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(573, 403);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 9;
            this.progressBar.Visible = false;
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarLabel.Location = new System.Drawing.Point(0, 373);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(665, 18);
            this.progressBarLabel.TabIndex = 10;
            this.progressBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormSharePointHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 415);
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelWorking);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.labelOutput);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.menuStripFuntions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripFuntions;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(681, 453);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(681, 453);
            this.Name = "FormSharePointHelper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Officience.SharePointHelper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSharePointHelper_FormClosing);
            this.Load += new System.EventHandler(this.FormSharePointHelper_Load);
            this.menuStripFuntions.ResumeLayout(false);
            this.menuStripFuntions.PerformLayout();
            this.contextMenuItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.MenuStrip menuStripFuntions;
        private System.Windows.Forms.ToolStripMenuItem menuFuntions;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelWorking;
        private System.Windows.Forms.ContextMenuStrip contextMenuItems;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuCopyLogs;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressBarLabel;
    }
}

