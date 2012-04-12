using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace EFSPWFActivities
{
    public class getRelevantTaskID : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getRelevantTaskID));

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

        //public static DependencyProperty __ListIdProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__ListId",
        //    typeof(string), typeof(getRelevantTaskID));

        //[Description("ListId")]
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public string __ListId
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(__ListIdProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(__ListIdProperty, value);
        //    }
        //}

        //public static DependencyProperty __ListItemProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__ListItem",
        //    typeof(int), typeof(getRelevantTaskID));

        //[Description("ListItem")]
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public int __ListItem
        //{
        //    get
        //    {
        //        return ((int)(base.GetValue(__ListItemProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(__ListItemProperty, value);
        //    }
        //}

        public static DependencyProperty TaskListNameProperty =
            DependencyProperty.Register("TaskListName",
            typeof(string),
            typeof(getRelevantTaskID));

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
            typeof(getRelevantTaskID));

        [Description("TaskID connected to the current item")]
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getRelevantTaskID", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            bool bReturn = false;
            SPList objSPListTask = null;
            SPList objSPListCurrent = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    objSPListCurrent = __Context.Web.Lists[new Guid(__Context.ListId)];
                    objSPListTask = __Context.Web.Lists[new Guid(TaskListName)];

                    SPQuery qry = new SPQuery();
                    qry.RowLimit = 1;
                    qry.Query = string.Format("<OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy><Where><Eq><FieldRef Name='WorkflowItemId' /><Value Type='Text'>{0}</Value></Eq></Where>", __Context.ItemId);

                    SPListItemCollection items = objSPListTask.GetItems(qry);
                    if (items.Count == 1)
                    {
                        TaskID = items[0].ID.ToString();
                        WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"TaskID = {0}", TaskID));
                        bReturn = true;
                    }
                    else
                    {
                        WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, "No relevant task found.");
                        bReturn = false;
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
