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
    public class getCurrentDateTime : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getCurrentDateTime));

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

        public static DependencyProperty varCurrentDatetimeProperty =
            DependencyProperty.Register("varCurrentDatetime",
            typeof(string),
            typeof(getCurrentDateTime));

        [Description("Current Datetime workflow variable")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string varCurrentDatetime
        {
            get
            {
                return (string)base.GetValue(varCurrentDatetimeProperty);
            }
            set
            {
                base.SetValue(varCurrentDatetimeProperty, value);
            }
        }

        public static DependencyProperty timeFormatProperty =
            DependencyProperty.Register("timeFormat",
            typeof(string),
            typeof(getCurrentDateTime));

        [Description("Time Format (yyyy/MM/dd HH:mm:ss)")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string timeFormat
        {
            get
            {
                return ((string)(base.GetValue(timeFormatProperty)));
            }
            set
            {
                base.SetValue(timeFormatProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getCurrentDateTime", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            string strCurrentDatetime = string.Empty;
            string strFormat = timeFormat.Trim();

            try
            {
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"timeFormat2 = {0}", timeFormat));

                if (string.IsNullOrEmpty(strFormat) || strFormat.Equals("default", StringComparison.InvariantCultureIgnoreCase))
                {
                    strCurrentDatetime = DateTime.Now.ToString(string.Empty);
                }
                else
                {
                    strCurrentDatetime = DateTime.Now.ToString(strFormat);
                }
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return ActivityExecutionStatus.Faulting;
            }

            varCurrentDatetime = strCurrentDatetime;
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"strCurrentDatetime = {0}", strCurrentDatetime));

            return ActivityExecutionStatus.Closed;
        }

    }
}
