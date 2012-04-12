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
    public class getCurrentDateTimeSB
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
                        web.CurrentUser, TimeSpan.Zero, "getCurrentDateTimeSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string timeFormat)
        {
            Hashtable results = new Hashtable();
            string strCurrentDatetime = string.Empty;

            try
            {
                LogDebugInfo(context, string.Format(@"timeFormat = {0}", timeFormat));

                if (string.IsNullOrEmpty(timeFormat) || timeFormat.Equals("default", StringComparison.InvariantCultureIgnoreCase))
                {
                    strCurrentDatetime = DateTime.Now.ToString(string.Empty);
                }
                else
                {
                    strCurrentDatetime = DateTime.Now.ToString(timeFormat);
                }
            }
            catch (Exception ex)
            {
                Log(context, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return results;
            }

            results["varCurrentDatetime"] = strCurrentDatetime;

            return results;
        }
    }
}
