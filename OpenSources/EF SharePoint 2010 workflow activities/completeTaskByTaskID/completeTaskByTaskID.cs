using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace EFSPWFActivities
{
    public class completeTaskByTaskID : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(completeTaskByTaskID));

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

        public static DependencyProperty TaskListNameProperty =
            DependencyProperty.Register("TaskListName",
            typeof(string),
            typeof(completeTaskByTaskID));

        [Description("Task list name")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string TaskListName
        {
            get
            {
                return ((string)(base.GetValue(TaskListNameProperty)));
            }
            set
            {
                base.SetValue(TaskListNameProperty, value);
            }
        }


        public static DependencyProperty TaskIDProperty =
            DependencyProperty.Register("TaskID",
            typeof(string),
            typeof(completeTaskByTaskID));

        [Description("Task ID")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string TaskID
        {
            get
            {
                return (string)base.GetValue(TaskIDProperty);
            }
            set
            {
                base.SetValue(TaskIDProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "completeTaskByTaskID", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            bool bReturn = false;
            SPList objSPListCurrent = null;
            SPListItem objSPListItemCurrent = null;
            SPList objSPListTask = null;
            SPListItem objSPListItemTask = null;
            Hashtable taskHash = new Hashtable();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    objSPListCurrent = __Context.Web.Lists[new Guid(__Context.ListId)];
                    objSPListItemCurrent = objSPListCurrent.GetItemById(__Context.ItemId);
                    objSPListTask = __Context.Web.Lists[new Guid(TaskListName)];

                    int iAttempts = 0;
                    while (iAttempts < 10)
                    {
                        SPWorkflow objSPWorkflow = objSPListItemCurrent.Workflows[this.WorkflowInstanceId];
                        if (objSPWorkflow.IsLocked == false)
                            break;
                        iAttempts++;
                        Thread.Sleep(5000);
                    }

                    if (iAttempts == 10)
                    {
                        WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"Task (ID = {0}) is locked.", TaskID));
                        bReturn = false;
                    }
                    else
                    {
                        int iTaskID = int.Parse(TaskID);
                        objSPListItemTask = objSPListTask.GetItemById(iTaskID);

                        if (objSPListItemTask == null)
                        {
                            WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, "No task is found by ID " + TaskID);
                            bReturn = false;
                        }
                        else
                        {
                            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, "Task is found by ID " + TaskID);

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
                });

                if (bReturn == false)
                {
                    return ActivityExecutionStatus.Faulting;
                }
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return ActivityExecutionStatus.Faulting;
            }

            return ActivityExecutionStatus.Closed;
        }
    }
}
