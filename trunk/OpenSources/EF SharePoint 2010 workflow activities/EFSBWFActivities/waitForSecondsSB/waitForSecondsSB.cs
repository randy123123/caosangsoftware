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
    public class waitForSecondsSB
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
                        web.CurrentUser, TimeSpan.Zero, "waitForSecondsSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, Int32 NumberOfSeconds)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, string.Format("begin... NumberOfSeconds={0}", NumberOfSeconds));

            if (NumberOfSeconds >= 0 && NumberOfSeconds <= 3600)
            {
                System.Threading.Thread.Sleep(NumberOfSeconds * 1000);
            }
            else
            {
                Log(context, string.Format("NumberOfSeconds({0}) is invalid", NumberOfSeconds));
            }

            return result;
        }
    }
}
