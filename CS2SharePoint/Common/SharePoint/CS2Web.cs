using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using Microsoft.SharePoint.Administration;
using System.IO;

namespace CSSoft
{
    public partial class CS2Web : IDisposable
    {
        #region IDisposable
        ~CS2Web() 
        {
            Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable

        #region SPWeb
        public static SPSite CurrentSite { get { return SPContext.Current.Site; } }
        public static SPWeb CurrentWeb { get { return SPContext.Current.Web; } }
        public static SPUser CurrentUser { get { return SPContext.Current.Web.CurrentUser; } }        
        #endregion SPWeb
        
        #region SPList
        public static SPList GetRootList(string listName)
        {
            return CurrentSite.RootWeb.Lists[listName];
        }
        public static SPList GetList(string listName)
        {
            return CurrentWeb.Lists[listName];
        }
        #endregion

        #region SPEmail
        public static bool SendEmailAttachments(MailAddressCollection To, MailAddressCollection CC, MailAddressCollection Bcc, string Subject, string Body, params string[] fileAttachs)
        {
            try
            {
                //Read Sharepoint Setting
                string smtpServer = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;
                string smtpSenderAddress = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                string smtpReplyToAddress = SPAdministrationWebApplication.Local.OutboundMailReplyToAddress;
                //Init email from
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpSenderAddress);                
                if (!String.IsNullOrEmpty(smtpReplyToAddress))
                    mailMessage.ReplyTo = new MailAddress(smtpReplyToAddress);
                //To
                if (To != null)
                    foreach (MailAddress email in To)
                        mailMessage.To.Add(email);
                else
                    return false;
                //CC
                if (CC != null)
                    foreach (MailAddress email in CC)
                        mailMessage.CC.Add(email);
                //Bcc
                if (Bcc != null)
                    foreach (MailAddress email in Bcc)
                        mailMessage.Bcc.Add(email);
                //Email Contents
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = Subject;
                mailMessage.Body = Body;
                //Attachments
                if (fileAttachs != null && fileAttachs.Length > 0)
                {
                    foreach (string filePath in fileAttachs)
                    {
                        byte[] data = ReadFile(filePath);
                        MemoryStream memoryStreamOfFile = new MemoryStream(data);
                        mailMessage.Attachments.Add(new Attachment(memoryStreamOfFile, Path.GetFileName(filePath)));
                    }
                }
                //Send mail
                SmtpClient smtpClient = new SmtpClient(smtpServer);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch { return false; }
        }
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
        #endregion
    }
}
