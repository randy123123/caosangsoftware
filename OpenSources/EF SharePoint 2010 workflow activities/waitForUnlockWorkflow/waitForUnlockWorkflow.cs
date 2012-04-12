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
    public class waitForUnlockWorkflow : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(waitForUnlockWorkflow));

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

        public static DependencyProperty NumberOfSecondsProperty = DependencyProperty.Register("NumberOfSeconds",
            typeof(Int32), typeof(waitForUnlockWorkflow));

        [Description("NumberOfSeconds")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public Int32 NumberOfSeconds
        {
            get
            {
                return ((Int32)
                (base.GetValue(NumberOfSecondsProperty)));
            }
            set
            {
                base.SetValue(NumberOfSecondsProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "waitForUnlockWorkflow", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            bool bReturn = false;
            SPList objSPListCurrent = null;
            SPListItem objSPListItemCurrent = null;

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    objSPListCurrent = __Context.Web.Lists[new Guid(__Context.ListId)];
                    objSPListItemCurrent = objSPListCurrent.GetItemById(__Context.ItemId);

                    int iAttempts = 0;
                    bReturn = false;
                    while (iAttempts < NumberOfSeconds)
                    {
                        SPWorkflow objSPWorkflow = objSPListItemCurrent.Workflows[this.WorkflowInstanceId];
                        if (objSPWorkflow.IsLocked == false)
                        {
                            bReturn = true;
                            break;
                        }
                        iAttempts++;
                        Thread.Sleep(1000);
                    }

                });

                if (bReturn == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"Workflow is locked. objSPListItemCurrent.Url = {0}) ", objSPListItemCurrent.Url));
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
