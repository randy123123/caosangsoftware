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
    public class getRelevantTaskIdSB
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
                        web.CurrentUser, TimeSpan.Zero, "getRelevantTaskIdSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string TaskListName)
        {
            Hashtable result = new Hashtable();

            bool bReturn = false;
            string strTaskID = string.Empty;
            SPList objSPListTask = null;
            SPList objSPListCurrent = null;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPListCurrent = objSPWeb.Lists[context.ListId];
                        objSPListTask = objSPWeb.Lists[new Guid(TaskListName)];

                        SPQuery qry = new SPQuery();
                        qry.RowLimit = 1;
                        qry.Query = string.Format("<OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy><Where><Eq><FieldRef Name='WorkflowItemId' /><Value Type='Text'>{0}</Value></Eq></Where>", context.ItemId);

                        SPListItemCollection items = objSPListTask.GetItems(qry);
                        if (items.Count == 1)
                        {
                            strTaskID = items[0].ID.ToString();
                            result["TaskID"] = strTaskID;
                            LogDebugInfo(context, string.Format(@"TaskID = {0}", strTaskID));
                            bReturn = true;
                        }
                        else
                        {
                            Log(context, "No relevant task found.");
                            bReturn = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDebugInfo(context, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return result;
            }

            return result;
        }
    }
}
