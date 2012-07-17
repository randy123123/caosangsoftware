using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CSSoft.SendMailService
{
    public partial class FormStatus : Form
    {
        Thread sendEmailThread;
        public FormStatus()
        {
            InitializeComponent();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cancelToolStripMenuItem.Text != "Continue Send Email")
            {
                sendEmailThread.Abort();
                cancelToolStripMenuItem.Text = "Continue Send Email";
            }
            else
            {
                FormStatus_Load(sender, e);
                cancelToolStripMenuItem.Text = "Cancel Send Email";
            }
        }

        private void showHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                Hide();
                WindowState = FormWindowState.Minimized;
            }
        }

        private void FormStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!"Exit".Equals(this.Tag))
            {
                Hide();
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void notifyIconTaskbar_DoubleClick(object sender, EventArgs e)
        {
            showHideToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Tag = "Exit";
            sendEmailThread.Abort();
            Close();
            Dispose();
        }

        private void FormStatus_Load(object sender, EventArgs e)
        {
            sendEmailThread = new Thread(new ThreadStart(DoWork));
            sendEmailThread.Start();
        }
        public void DoWork()
        {
            while (true)
            {
                Thread.Sleep(1000);
                UpdateForm(DateTime.Now.ToString());
            }
        }
        delegate void UpdateFormDelegate(string lableStatusText);
        private void UpdateForm(string lableStatusText)
        {
            if (labelStatus.InvokeRequired)
            {
                // this is worker thread
                UpdateFormDelegate del = new UpdateFormDelegate(UpdateForm);
                labelStatus.Invoke(del, new object[] { lableStatusText });
            }
            else
            {
                // this is UI thread
                labelStatus.Text = lableStatusText;
            }
        }
    }
}
