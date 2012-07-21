using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace CSSoft.SendMailService
{
    public partial class FormStatus : Form
    {
        Thread sendEmailThread;
        public SPWeb _web = null;
        bool allowUnsafeUpdatesOfSite = false;
        public FormStatus()
        {
            InitializeComponent();
        }
        public SPWeb Web
        {
            get
            {
                if (_web == null) _web = OpenWeb(ServiceSettings.Default.SiteUrl);
                return _web;
            }
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
            CloseWeb();
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
                ReadEmailTemplateAndSend();
                Waiting(ServiceSettings.Default.WaitingTime);
            }
        }

        private void Waiting(int second)
        {
            for (int i = second; i >= 0; i--)
            {
                WriteLine("Next send in {0} second(s)", i);
                Thread.Sleep(1000);
            }
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

        public void CloseWeb()
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
        public void WriteLine(string fomat, params object[] args)
        {
            WriteLine(String.Format(fomat, args));
        }
        public void WriteLine(string text)
        {
            UpdateForm(text);
        }
        private void ReadEmailTemplateAndSend()
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = "<Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Ready send</Value></Eq></Where>";
                query.RowLimit = 1;
                SPListItemCollection items = Web.Lists[ServiceSettings.Default.ListEmailTemplate].GetItems(query);
                if (items != null && items.Count > 0)
                {
                    SPListItem emailTemplate = items[0];
                    emailTemplate["Status"] = "Sending...";
                    emailTemplate.SystemUpdate(false);
                    string key = CS2Convert.ToString(emailTemplate["Key"]);
                    WriteLine("Start send mail: {0}", key);
                    SPFieldLookupValueCollection contacts = CS2Convert.ToLookupValueCollection(emailTemplate["To"]);
                    if (contacts.Count > 0)
                    {
                        SPList ListContacts = Web.Lists[ServiceSettings.Default.ListContacts];
                        foreach (SPFieldLookupValue contact in contacts)
                        {
                            try
                            {
                                SPListItem itemContact = ListContacts.GetItemById(contact.LookupId);
                                SendMailToContact(itemContact, emailTemplate);
                            }
                            catch (Exception ex)
                            {
                                WriteLine(ex.Message);
                            }
                        }
                    }
                    emailTemplate["Status"] = "Sended";
                    emailTemplate.SystemUpdate(false);
                    WriteLine("Send mail '{0}' completed", key);
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);            
            }
        }

        private void SendMailToContact(SPListItem contact, SPListItem emailTemplate)
        {
            string emailTo = CS2Convert.ToString(contact[SPBuiltInFieldId.EMail]);
            if (!String.IsNullOrEmpty(emailTo))
            {
                string subject = CS2Convert.ToString(emailTemplate.Title);
                string body = CS2Convert.ToString(emailTemplate[SPBuiltInFieldId.Body]);

                //Build email from template
                Regex regex = new Regex("{(?<Property>[^}]*)");

                //Subject
                MatchCollection matchs = regex.Matches(subject);

                foreach (Match match in matchs)
                {
                    string property = (match.Groups["Property"].Value);
                    string propertyValue = "";
                    try
                    {
                        propertyValue = CS2Convert.ToString(contact[property]);
                    }
                    catch { }
                    subject = subject.Replace(String.Format("{{{0}}}", property), propertyValue);
                }
                    
                //Body
                matchs = regex.Matches(body);

                foreach (Match match in matchs)
                {
                    string property = (match.Groups["Property"].Value);
                    string propertyValue = "";
                    try
                    {
                        propertyValue = CS2Convert.ToString(contact[property]);
                    }
                    catch { }
                    body = body.Replace(String.Format("{{{0}}}", property), propertyValue);
                }
                body = body.Replace("src=\"/", String.Format("src=\"{0}/", ServiceSettings.Default.PublicUrl));
                SmtpClient SmtpServer = new SmtpClient(ServiceSettings.Default.SMTP);
                SmtpServer.Port = ServiceSettings.Default.Port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ServiceSettings.Default.Email, ServiceSettings.Default.Password);
                SmtpServer.EnableSsl = ServiceSettings.Default.DefaultCredentials;
                
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ServiceSettings.Default.Email);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
                WriteLine("Sending to '{0}'", emailTo);
                Thread.Sleep(ServiceSettings.Default.TimeSleep);

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
                labelStatus.Text = String.Format("[{0}] {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), lableStatusText);
            }
        }
    }
}
