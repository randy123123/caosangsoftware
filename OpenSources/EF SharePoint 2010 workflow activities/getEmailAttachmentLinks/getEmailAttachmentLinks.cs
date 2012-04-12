using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.SharePoint.Utilities;

namespace EFSPWFActivities
{
    public class getEmailAttachmentLinks : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getEmailAttachmentLinks));
        [Description("Context")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WorkflowContext __Context
        {
            get
            {
                return ((WorkflowContext)(base.GetValue(__ContextProperty)));
            }
            set
            {
                base.SetValue(__ContextProperty, value);
            }
        }

        public static DependencyProperty EmailLibTitleProperty = DependencyProperty.Register("EmailLibTitle",
            typeof(string), typeof(getEmailAttachmentLinks));

        [Description("Email Library Title")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string EmailLibTitle
        {
            get
            {
                return ((string)
                (base.GetValue(EmailLibTitleProperty)));
            }
            set
            {
                base.SetValue(EmailLibTitleProperty, value);
            }
        }

        public static DependencyProperty EmailIDProperty = DependencyProperty.Register("EmailID",
            typeof(string), typeof(getEmailAttachmentLinks));

        [Description("Email Item ID")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string EmailID
        {
            get
            {
                return ((string)
                (base.GetValue(EmailIDProperty)));
            }
            set
            {
                base.SetValue(EmailIDProperty, value);
            }
        }

        public static DependencyProperty AttachmentLinksHTMLProperty = DependencyProperty.Register("AttachmentLinksHTML",
            typeof(string), typeof(getEmailAttachmentLinks));

        [Description("Attachment Links as HTML")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string AttachmentLinksHTML
        {
            get
            {
                return (string)base.GetValue(AttachmentLinksHTMLProperty);
            }
            set
            {
                base.SetValue(AttachmentLinksHTMLProperty, value);
            }
        }

        public static void WriteDebugInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
#if DEBUG
            WriteInfoToHistoryLog(web, workflow, description);
#endif
        }

        public static void WriteInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
            TimeSpan ts = new TimeSpan();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getEmailAttachmentLinks", description, string.Empty);
            });
        }

        public string getCAML(SPListItem objSPListItem, int iEmailID)
        {
            string strCAML = string.Empty;
            //2011-03-03T12:23:00Z
            DateTime dtCreated = objSPListItem.File.TimeCreated;
            int iTimeScopeInSeconds = 60;
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"iTimeScopeInSeconds=" + iTimeScopeInSeconds.ToString());

            DateTime dtDateTo = dtCreated.AddSeconds(iTimeScopeInSeconds);
            dtDateTo = objSPListItem.Web.RegionalSettings.TimeZone.UTCToLocalTime(dtDateTo);

            strCAML = string.Format("<Where><And><Leq><FieldRef Name='Created' /><Value Type='DateTime' IncludeTimeValue='True'>{0}</Value></Leq><Gt><FieldRef Name='ID' /><Value Type='Counter'>{1}</Value></Gt></And></Where>",
                SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtDateTo), iEmailID);

            strCAML += @"<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"strCAML=" + strCAML);

            return strCAML;
        }

        public string getFileLink(SPFile objSPFile)
        {
            string strLinkFormat = @"<a href=""{1}"">{0}</a>";
            string strName = objSPFile.Name;
            strName = SPEncode.UrlEncode(strName);
            string strUrl = objSPFile.ServerRelativeUrl;
            //strUrl = SPEncode.UrlEncode(strUrl);
            string strLinkSingle = string.Format(strLinkFormat, strName, strUrl);

            return strLinkSingle;
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSite objSPSite = (SPSite)__Context.Site;
            SPWeb objSPWeb = (SPWeb)__Context.Web;

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Site.ServerRelativeUrl=" + objSPSite.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Web.ServerRelativeUrl=" + objSPWeb.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"EmailLibTitle=" + EmailLibTitle);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"EmailID=" + EmailID);

            int iEmailID = int.MinValue;
            SPList objSPList = null;
            SPFolder objSPFolder = null;
            SPListItem objSPListItemEmail = null;
            SPListItemCollection objSPListItemCollection = null;
            SPQuery objSPQuery = new SPQuery();
            string strCAML = string.Empty;
            string strLinkSingle = string.Empty;
            string strAllLinks = string.Empty;
            string strLinkSeparator = @"<br />";
            string strEmailFrom = string.Empty;
            string strEmailSubject = string.Empty;

            try
            {
                objSPList = objSPWeb.Lists.TryGetList(EmailLibTitle);
                if (objSPList == null)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"email library title(" + EmailLibTitle + ") is invalid");
                    return ActivityExecutionStatus.Faulting;
                }
                if (int.TryParse(EmailID, out iEmailID) == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"email item id(" + EmailID + ") is invalid");
                    return ActivityExecutionStatus.Faulting;
                }
                if (objSPList.Fields.ContainsFieldWithStaticName("EmailFrom") == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"email library (" + EmailLibTitle + ") is not configured to receive emails, it doesn't have 'EmailFrom' field.");
                    return ActivityExecutionStatus.Faulting;
                }
                if (objSPList.Fields.ContainsFieldWithStaticName("EmailSubject") == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"email library (" + EmailLibTitle + ") is not configured to receive emails, it doesn't have 'EmailSubject' field.");
                    return ActivityExecutionStatus.Faulting;
                }
                objSPListItemEmail = objSPList.GetItemById(iEmailID);
                if (objSPListItemEmail == null)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"cannot find the email item based on document library title (%1) and id(%2)", EmailLibTitle, EmailID));
                    return ActivityExecutionStatus.Faulting;
                }
                strEmailFrom = Convert.ToString(objSPListItemEmail["EmailFrom"]);
                strEmailSubject = Convert.ToString(objSPListItemEmail["EmailSubject"]);

                objSPFolder = objSPListItemEmail.File.ParentFolder;
                strCAML = getCAML(objSPListItemEmail, iEmailID);
                objSPQuery.Query = strCAML;
                objSPQuery.Folder = objSPFolder;

                objSPListItemCollection = objSPList.GetItems(objSPQuery);
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());

                foreach (SPListItem item in objSPListItemCollection)
                {
                    if (item.File.Name.EndsWith(".eml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        strAllLinks = string.Empty;
                        continue;
                    }

                    if (strEmailFrom.Equals(Convert.ToString(item["EmailFrom"]), StringComparison.InvariantCultureIgnoreCase) == false)
                        continue;
                    if (strEmailSubject.Equals(Convert.ToString(item["EmailSubject"]), StringComparison.InvariantCultureIgnoreCase) == false)
                        continue;

                    strLinkSingle = getFileLink(item.File);

                    if (string.IsNullOrEmpty(strAllLinks) == false)
                        strAllLinks += strLinkSeparator;
                    strAllLinks += strLinkSingle;
                }
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"getEmailAttachmentLinks exception.");
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.Message=" + ex.Message);
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.StackTrace=" + ex.StackTrace);
                return ActivityExecutionStatus.Faulting;
            }

            AttachmentLinksHTML = strAllLinks;

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"getEmailAttachmentLinks activity completed.");
            return ActivityExecutionStatus.Closed;
        }
    }
}
