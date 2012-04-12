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
    public class waitForUnlockWorkflowSB
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
                        web.CurrentUser, TimeSpan.Zero, "waitForUnlockWorkflowSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, Int32 NumberOfSeconds)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, string.Format("begin... NumberOfSeconds={0}", NumberOfSeconds));

            bool bReturn = false;
            SPList objSPListCurrent = null;
            SPListItem objSPListItemCurrent = null;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        objSPListCurrent = web.Lists[context.ListId];
                        objSPListItemCurrent = objSPListCurrent.GetItemById(context.ItemId);

                        int iAttempts = 0;
                        bReturn = false;
                        while (iAttempts < NumberOfSeconds)
                        {
                            SPWorkflow objSPWorkflow = objSPListItemCurrent.Workflows[context.WorkflowInstanceId];
                            if (objSPWorkflow.IsLocked == false)
                            {
                                bReturn = true;
                                break;
                            }
                            iAttempts++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }

                if (bReturn == false)
                {
                    Log(context, string.Format(@"Workflow is locked. objSPListItemCurrent.Url = {0}) ", objSPListItemCurrent.Url));
                }
            }
            catch (Exception ex)
            {
                Log(context, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
            }

            return result;
        }
    }
}
