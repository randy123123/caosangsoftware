using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace EFSPWFActivities
{
    public class getLocalTimeFromUTC : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getLocalTimeFromUTC));

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

        public static DependencyProperty LocalDatetimeProperty =
            DependencyProperty.Register("LocalDatetime",
            typeof(DateTime),
            typeof(getLocalTimeFromUTC));

        [Description("Local Datetime")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime LocalDatetime
        {
            get
            {
                return ((DateTime)(base.GetValue(LocalDatetimeProperty)));
            }
            set
            {
                base.SetValue(LocalDatetimeProperty, value);
            }
        }

        public static DependencyProperty UtcDatetimeProperty =
            DependencyProperty.Register("UtcDatetime",
            typeof(DateTime),
            typeof(getLocalTimeFromUTC));

        [Description("UTC Datetime")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime UtcDatetime
        {
            get
            {
                return ((DateTime)(base.GetValue(UtcDatetimeProperty)));
            }
            set
            {
                base.SetValue(UtcDatetimeProperty, value);
            }
        }

        public void WriteDebugInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
#if DEBUG
            System.Reflection.Assembly objAssembly = null;
            objAssembly = this.GetType().Assembly;
            FileInfo objFileInfo = new FileInfo(objAssembly.Location);
            string strVersionInfo = string.Empty;
            strVersionInfo = string.Format(@"debug - {0} - {1} - ", objFileInfo.CreationTime, objAssembly.GetName().Version);
            WriteInfoToHistoryLog(web, workflow, strVersionInfo + description);
#endif
        }

        public static void WriteInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
            TimeSpan ts = new TimeSpan();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getLocalTimeFromUTC", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"UtcDatetime={0}", UtcDatetime));
                LocalDatetime = __Context.Web.RegionalSettings.TimeZone.UTCToLocalTime(UtcDatetime);
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"LocalDatetime={0}", LocalDatetime));
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
