using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public class completeTaskByTaskIdSB
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
                        web.CurrentUser, TimeSpan.Zero, "completeTaskByTaskIdSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string TaskListName, string TaskID)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, string.Format("begin... TaskListName={0}", TaskListName));

            bool bReturn = false;
            SPListItem objSPListItemCurrent = null;
            SPList objSPListTask = null;
            SPListItem objSPListItemTask = null;
            Hashtable taskHash = new Hashtable();

            bool previousAllowUnsafeUpdates = false;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPListTask = objSPWeb.Lists.TryGetList(TaskListName);
                        if (objSPListTask == null)
                        {
                            Log(context, string.Format(@"task list title({0}) is invalid", TaskListName));
                            return result;
                        }

                        previousAllowUnsafeUpdates = objSPWeb.AllowUnsafeUpdates;
                        if (previousAllowUnsafeUpdates == false)
                        {
                            objSPWeb.AllowUnsafeUpdates = true;
                        }
                        //objSPField = objSPList.Fields[FieldName];

                        int iAttempts = 0;
                        bReturn = false;
                        while (iAttempts < 100)
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

                        if (bReturn == false)
                        {
                            LogDebugInfo(context, string.Format(@"Workflow is locked. objSPListItemCurrent.Url = {0}) ", objSPListItemCurrent.Url));
                            bReturn = false;
                        }
                        else
                        {
                            int iTaskID = int.Parse(TaskID);
                            objSPListItemTask = objSPListTask.GetItemById(iTaskID);

                            if (objSPListItemTask == null)
                            {
                                LogDebugInfo(context, "No task is found by ID " + TaskID);
                                bReturn = false;
                            }
                            else
                            {
                                LogDebugInfo(context, "Task is found by ID " + TaskID);

                                taskHash[SPBuiltInFieldId.Completed] = "TRUE";
                                taskHash[SPBuiltInFieldId.PercentComplete] = 1.0f;
                                taskHash[SPBuiltInFieldId.TaskStatus] = SPResource.GetString(new CultureInfo((int)objSPListItemCurrent.Web.Language, false), "WorkflowTaskStatusComplete", new object[0]);
                                taskHash[SPBuiltInFieldId.FormData] = SPWorkflowStatus.Completed;
                                taskHash[SPBuiltInFieldId.Outcome] = "Completed";
                                taskHash[SPBuiltInFieldId.Comments] = @"N/A";

                                SPWorkflowTask.AlterTask(objSPListItemTask, taskHash, true);
                                bReturn = true;
                            }
                        }

                        objSPWeb.AllowUnsafeUpdates = previousAllowUnsafeUpdates;
                    }
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
