using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Utilities;

namespace EFSBWFActivities
{
    [ToolboxItemAttribute(true)]
    public class getEmailAttachmentLinksSB
    {
        public static void LogDebugInfo(SPUserCodeWorkflowContext context, string strMessage)
        {
#if DEBUG
            Log(context, strMessage);
#endif
        }

        public static void Log(SPUserCodeWorkflowContext context, string strMessage)
        {
            using (SPSite site = new SPSite(context.CurrentWebUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPWorkflow.CreateHistoryEvent(web, context.WorkflowInstanceId, 0,
                        web.CurrentUser, TimeSpan.Zero, "getEmailAttachmentLinksSB", strMessage, string.Empty);
                }
            }
        }

        public static string getCAML(SPUserCodeWorkflowContext context, SPListItem objSPListItem, int iEmailID)
        {
            string strEmailSubject = string.Empty;
            string strCAML = string.Empty;
            string strCAMLTime = string.Empty;
            string strCAMLSubject = string.Empty;
            //2011-03-03T12:23:00Z
            DateTime dtCreated = objSPListItem.File.TimeCreated;
            int iTimeScopeInSeconds = 60;

            DateTime dtDateTo = dtCreated.AddSeconds(iTimeScopeInSeconds);
            dtDateTo = objSPListItem.Web.RegionalSettings.TimeZone.UTCToLocalTime(dtDateTo);

            strCAMLTime = string.Format("<And><Leq><FieldRef Name='Created' /><Value Type='DateTime' IncludeTimeValue='True'>{0}</Value></Leq><Gt><FieldRef Name='ID' /><Value Type='Counter'>{1}</Value></Gt></And>",
                SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtDateTo), iEmailID);
            strEmailSubject = Convert.ToString(objSPListItem[SPBuiltInFieldId.EmailSubject]);
            strCAMLSubject = string.Format(@"<Eq><FieldRef Name='EmailSubject' /><Value Type='Text'>{0}</Value></Eq>", strEmailSubject);
            strCAML = string.Format("<Where><And>{0}{1}</And></Where>", strCAMLTime, strCAMLSubject);

            strCAML += @"<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
            LogDebugInfo(context, @"strCAML=" + strCAML);

            return strCAML;
        }

        public static string getFileLink(SPFile objSPFile)
        {
            string strLinkFormat = @"<a href=""{1}"">{0}</a>";
            string strName = objSPFile.Name;
            //strName = SPEncode.HtmlEncode(strName);

            string strUrl = objSPFile.ServerRelativeUrl;
            string strLinkSingle = string.Format(strLinkFormat, strName, strUrl);

            return strLinkSingle;
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string EmailLibTitle, string EmailID)
        {
            Hashtable result = new Hashtable();

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
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPList = objSPWeb.Lists[new Guid(EmailLibTitle)];
                        if (objSPList == null)
                        {
                            LogDebugInfo(context, @"email library title(" + EmailLibTitle + ") is invalid");
                            return result;
                        }
                        if (int.TryParse(EmailID, out iEmailID) == false)
                        {
                            LogDebugInfo(context, @"email item id(" + EmailID + ") is invalid");
                            return result;
                        }
                        if (objSPList.Fields.Contains(SPBuiltInFieldId.EmailFrom) == false)
                        {
                            LogDebugInfo(context, @"email library (" + EmailLibTitle + ") is not configured to receive emails, it doesn't have 'EmailFrom' field.");
                            return result;
                        }
                        if (objSPList.Fields.Contains(SPBuiltInFieldId.EmailSubject) == false)
                        {
                            LogDebugInfo(context, @"email library (" + EmailLibTitle + ") is not configured to receive emails, it doesn't have 'EmailSubject' field.");
                            return result;
                        }

                        objSPListItemEmail = objSPList.GetItemById(iEmailID);
                        if (objSPListItemEmail == null)
                        {
                            Log(context, string.Format(@"cannot find the email item based on document library title (%1) and id(%2)", EmailLibTitle, EmailID));
                            return result;
                        }
                        //if (objSPListItemEmail.File.Name.EndsWith(".eml") == false)
                        //{
                        //    return ActivityExecutionStatus.Closed;
                        //}

                        strEmailFrom = Convert.ToString(objSPListItemEmail[SPBuiltInFieldId.EmailFrom]);
                        strEmailSubject = Convert.ToString(objSPListItemEmail[SPBuiltInFieldId.EmailSubject]);

                        objSPFolder = objSPListItemEmail.File.ParentFolder;
                        strCAML = getCAML(context, objSPListItemEmail, iEmailID);
                        objSPQuery.Query = strCAML;
                        objSPQuery.Folder = objSPFolder;

                        objSPListItemCollection = objSPList.GetItems(objSPQuery);
                        LogDebugInfo(context, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());

                        foreach (SPListItem item in objSPListItemCollection)
                        {
                            if (item.File.Name.EndsWith(".eml", StringComparison.InvariantCultureIgnoreCase))
                            {
                                strAllLinks = string.Empty;
                                continue;
                            }
                            if (strEmailFrom.Equals(Convert.ToString(objSPListItemEmail[SPBuiltInFieldId.EmailFrom]), StringComparison.InvariantCultureIgnoreCase) == false)
                                continue;
                            if (strEmailSubject.Equals(Convert.ToString(objSPListItemEmail[SPBuiltInFieldId.EmailSubject]), StringComparison.InvariantCultureIgnoreCase) == false)
                                continue;

                            strLinkSingle = getFileLink(item.File);

                            if (string.IsNullOrEmpty(strAllLinks) == false)
                                strAllLinks += strLinkSeparator;
                            strAllLinks += strLinkSingle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(context, @"getEmailAttachmentLinks exception.");
                Log(context, @"ex.Message=" + ex.Message);
                Log(context, @"ex.StackTrace=" + ex.StackTrace);
            }

            result["AttachmentLinksHTML"] = strAllLinks;
            return result;
        }
    }
}
