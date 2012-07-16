using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;

namespace EmailMarketingForSharepoint
{
    public partial class LogView : Form
    {
        //Khai báo API 
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        internal static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        internal static extern int ReleaseCapture(); const int HTCAPTION = 2;
        const int WM_NCLBUTTONDOWN = 0xA1;
        Thread sendEmailThread;
        public LogView()
        {
            InitializeComponent();
        }

        private void LogView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ReleaseCapture();
                int Result = SendMessage((int)this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void LogView_Load(object sender, EventArgs e)
        {
            sendEmailThread = new Thread(new ThreadStart(DoWork));
            sendEmailThread.Start();
        }
        public void DoWork()
        {
            while (true)
            {
                StartSearch();
            }
        }

        public void StartSearch()
        {
            Thread.Sleep(1000);
            labelMsg.Text = DateTime.Now.ToString();            
        }
        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.Hide();
            WindowState = FormWindowState.Minimized;
        }

        private void ShowHide_Click(object sender, EventArgs e)
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

        private void EmailMarketing_DoubleClick(object sender, EventArgs e)
        {
            ShowHide_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendEmailThread.Abort();
            Close();
            Dispose();
        }
    }
}
