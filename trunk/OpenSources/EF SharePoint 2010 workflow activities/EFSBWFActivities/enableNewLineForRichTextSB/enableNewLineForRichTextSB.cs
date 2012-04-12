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

namespace EFSBWFActivities
{
    [ToolboxItemAttribute(true)]
    public class enableNewLineForRichTextSB
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
                        web.CurrentUser, TimeSpan.Zero, "enableNewLineForRichTextSB", strMessage, string.Empty);
                }
            }
        }

        public static string FormattingForRichText(string strBody)
        {
            string strReturn = string.Empty;

            if (string.IsNullOrEmpty(strBody))
                return string.Empty;

            strReturn = strBody.Trim();
            if (strReturn.IndexOf(Environment.NewLine) > 0)
            {
                strReturn = strReturn.Replace(Environment.NewLine, @"</div><div>");
                strReturn = @"<div>" + strReturn + @"</div>";
            }
            if (strReturn.IndexOf("\n") > 0)
            {
                strReturn = strReturn.Replace("\n", @"</div><div>");
                strReturn = @"<div>" + strReturn + @"</div>";
            }

            return strReturn;
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string varSourceString)
        {
            string varDestString = string.Empty;
            Hashtable result = new Hashtable();

            try
            {
                LogDebugInfo(context, string.Format(@"varSourceString = {0}", varSourceString));

                varDestString = FormattingForRichText(varSourceString);
                LogDebugInfo(context, string.Format(@"varDestString = {0}", varDestString));
                result["varDestString"] = varDestString;
            }
            catch (Exception ex)
            {
                LogDebugInfo(context, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
            }

            return result;
        }
    }
}
