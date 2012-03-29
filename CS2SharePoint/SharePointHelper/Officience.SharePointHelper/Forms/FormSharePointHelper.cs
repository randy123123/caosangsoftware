using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Reflection;

namespace Officience.SharePointHelper
{
    public partial class FormSharePointHelper : Form
    {
        #region Members
        public const int LOG_ROW = 23;
        public const string FILE_CONFIG = @"ServerUrl.Config";
        StringBuilder fullLog = new StringBuilder();
        int outputItemCount = -1;
        public SPWeb Web = null;
        bool allowUnsafeUpdatesOfSite = false;
        ServerConfig serverConfig;        
        #endregion Members

        #region Forms
        public FormSharePointHelper()
        {
            InitializeComponent();
        }
        private void FormSharePointHelper_Load(object sender, EventArgs e)
        {
            this.saveFileDialogExport.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            serverConfig = new ServerConfig();
            if (File.Exists(FILE_CONFIG))
            {
                string s = File.ReadAllText(FILE_CONFIG);
                serverConfig = GenericSerialize<ServerConfig>.DeSerialize(s);
                foreach (Server server in serverConfig.ListServers)
                {
                    int index = comboBoxServer.Items.Add(server.Url);
                    if (server.Default)
                        comboBoxServer.SelectedIndex = index;
                }
            }
            ActiveControls(false);
            labelWorking.Visible = false;
        }

        private void ActiveControls(bool enable)
        {
            comboBoxServer.Enabled = !enable;
            menuFuntions.Enabled = buttonExport.Enabled = enable;
            contextMenuItems.Enabled = enable;
            if (enable)
                buttonConnect.Text = "Disconnect";
            else
                buttonConnect.Text = "Connect";
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (buttonConnect.Text == "Connect")
            {
                ProgressBarInit(String.Format("Connecting to {0}", comboBoxServer.Text), 5);
                AddWebUrlIfNotExist();
                StartFuntion();
                ProgressBarNext();
                Web = OpenWeb(comboBoxServer.Text);
                ProgressBarNext();
                AddDefineFunctions();
                ProgressBarNext();
                AddHanlderForFunctions();
                ProgressBarNext();
                EndFuntion();
                ProgressBarNext();
                ActiveControls(true);
            }
            else
            {
                ProgressBarInit(String.Format("Disconnecting to {0}", comboBoxServer.Text), 4);
                StartFuntion();
                ProgressBarNext();
                CloseWeb(comboBoxServer.Text);
                ProgressBarNext();
                menuFuntions.DropDownItems.Clear();
                ProgressBarNext();
                EndFuntion();
                ProgressBarNext();
                ActiveControls(false);
            }
        }

        private void AddDefineFunctions()
        {
            Assembly thisAsm = Assembly.GetExecutingAssembly();
            List<Type> types = thisAsm.GetTypes().Where
                        (t => ((typeof(IFormFunction).IsAssignableFrom(t)
                             && t.IsClass && !t.IsAbstract))).ToList();
            foreach (Type t in types)
            {
                IFormFunction formFunction = (IFormFunction)Activator.CreateInstance(t);
                formFunction.SetForm(this);
                formFunction.DefineFunctions();
            }
        }

        private void AddWebUrlIfNotExist()
        {
            Server ser = serverConfig.ListServers.FirstOrDefault(item => item.Url == comboBoxServer.Text);
            if (ser == null)
            {
                string filePath = FILE_CONFIG;
                serverConfig.ListServers.ForEach(item => item.Default = false);
                serverConfig.ListServers.Add(new Server(comboBoxServer.Text, true));
                //Update file in bin folder
                if (File.Exists(filePath))
                    File.Delete(filePath);
                File.AppendAllText(filePath, GenericSerialize<ServerConfig>.Serialize(serverConfig));
                //Update file in solution folder
                filePath = String.Format(@"..\..\{0}", filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    File.AppendAllText(filePath, GenericSerialize<ServerConfig>.Serialize(serverConfig));
                }
            }
        }
        private void menuFuntions_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            StartFuntion();
            WriteLine("'{0}' --> Start at '{1}'", e.ClickedItem.Text, DateTime.Now.ToString("hh:mm:ss.fff tt"));
            this.Refresh();
        }
        private void AddHanlderForFunctions()
        {
            foreach (ToolStripItem item in menuFuntions.DropDownItems)
                item.Click += new EventHandler(menuItemAfterClick);
        }

        void menuItemAfterClick(object sender, EventArgs e)
        {
            WriteLine("'{0}' --> Done at '{1}'!", (sender as ToolStripItem).Text, DateTime.Now.ToString("hh:mm:ss.fff tt"));
            EndFuntion();
        }
        private void FormSharePointHelper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonConnect.Enabled == false)
                CloseWeb(comboBoxServer.Text);
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            StartFuntion();
            if (saveFileDialogExport.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialogExport.FileName);
                sw.WriteLine(GetLogs());
                sw.Close();
            }
            EndFuntion();
        }

        private void menuCopyLogs_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetLogs());
        }

        private string GetLogs()
        {
            return fullLog.ToString();
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();
            if (outputItemCount >= 0)
                for (int i = 0; i <= listBoxOutput.SelectedItems.Count - 1; i++)
                    text.AppendLine(listBoxOutput.SelectedItems[i].ToString());
            Clipboard.SetText(text.ToString());
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EndFuntion()
        {
            labelWorking.Visible = false;
            menuFuntions.Enabled = buttonExport.Enabled = buttonExit.Enabled = true;
        }

        private void StartFuntion()
        {
            labelWorking.Visible = true;
            menuFuntions.Enabled = buttonExport.Enabled = buttonExit.Enabled = false;
            this.Refresh();
        }
        #endregion Forms

        #region Commons
        public ToolStripItem AddFunctions(string Text)
        {
            return menuFuntions.DropDownItems.Add(Text);
        }
        public void WriteLine(string fomat, params object[] args)
        {
            WriteLine(String.Format(fomat, args));
        }
        public void WriteLine(string text)
        {
            outputItemCount++;
            string log = string.Format("[{0}] {1}", outputItemCount.ToString("0000"), text);
            fullLog.AppendLine(log);
            listBoxOutput.Items.Add(log);
            if (outputItemCount > LOG_ROW) listBoxOutput.Items.RemoveAt(0);
            listBoxOutput.ClearSelected();
            if (outputItemCount > LOG_ROW)
                listBoxOutput.SelectedIndex = LOG_ROW;
            else
                listBoxOutput.SelectedIndex = outputItemCount;
            this.Refresh();
            Application.DoEvents();
        }
        public SPWeb OpenWeb(string webUrl)
        {
            try
            {
                WriteLine("OpenWeb('{0}')", webUrl);
                SPWeb web = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(webUrl))
                    {
                        allowUnsafeUpdatesOfSite = site.AllowUnsafeUpdates;
                        site.AllowUnsafeUpdates = true;
                        web = site.OpenWeb();
                    }
                }
                );
                WriteLine("OpenWeb Completed");
                return web;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                return null;
            }
        }
        public void CloseWeb(string webUrl)
        {
            try
            {
                WriteLine("CloseWeb");
                if (Web != null)
                {
                    Web.Site.AllowUnsafeUpdates = allowUnsafeUpdatesOfSite;
                    Web.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
        public SPList List(string listName)
        {
            WriteLine(String.Format("Open list: {0}", listName));
            return Web.Lists[listName];
        }
        public int ProgressBarValue { get; set; }
        public int ProgressBarMaxValue { get; set; }
        public void ProgressBarInit(int value)
        {
            ProgressBarInit(string.Empty, value);
        }
        public void ProgressBarInit(string lable,int value)
        {
            if (value <= 0) return;
            ProgressBarValue = -1;
            ProgressBarMaxValue = value;
            progressBar.Minimum = 0;
            progressBar.Maximum = ProgressBarMaxValue;
            progressBar.Visible = true;
            progressBarLabel.Tag = lable;
            progressBarLabel.Visible = true;
            ProgressBarNext();
        }
        public void ProgressBarNext()
        {
            ProgressBarValue++;
            progressBarLabel.Text = String.Format("Progress: '{0}' - Completed: {1}%", progressBarLabel.Tag, ProgressBarValue * 100 / ProgressBarMaxValue);
            if (ProgressBarValue == ProgressBarMaxValue)
            {
                progressBar.Value = ProgressBarMaxValue;
                this.Refresh();
                Application.DoEvents();
                System.Threading.Thread.Sleep(800);
                //Reset progress bar value
                ProgressBarValue = 0;
                progressBarLabel.Text = string.Empty;
                progressBar.Visible = false;
                progressBarLabel.Visible = false;
            }
            progressBar.Value = ProgressBarValue;
            this.Refresh();
            Application.DoEvents();
        }

        public void ProgressBarClear()
        {
            ProgressBarValue = ProgressBarMaxValue - 1;
            ProgressBarNext();
        }
        #endregion Commons

        private void comboBoxServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonConnect_Click(null,null);
            }
        }
    }
}
