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
    public class enableNewLineForRichText : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(enableNewLineForRichText));

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

        public static DependencyProperty varSourceStringProperty =
            DependencyProperty.Register("varSourceString",
            typeof(string),
            typeof(enableNewLineForRichText));

        [Description("source string")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string varSourceString
        {
            get
            {
                return ((string)(base.GetValue(varSourceStringProperty)));
            }
            set
            {
                base.SetValue(varSourceStringProperty, value);
            }
        }

        public static DependencyProperty varDestStringProperty =
            DependencyProperty.Register("varDestString",
            typeof(string),
            typeof(enableNewLineForRichText));

        [Description("destination string")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string varDestString
        {
            get
            {
                return (string)base.GetValue(varDestStringProperty);
            }
            set
            {
                base.SetValue(varDestStringProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "enableNewLineForRichText", description, string.Empty);
            });
        }

        public static string FormattingForRichText(string strBody)
        {
            string strReturn = string.Empty;

            if (string.IsNullOrEmpty(strBody))
                return string.Empty;

            strReturn = strBody.Trim();
            if (strReturn.IndexOf(Environment.NewLine) > 0)
            {
                strReturn = strReturn.Replace(Environment.NewLine, @"</div><div>");
                strReturn = @"<div>" + strReturn + @"</div>";
            }
            if (strReturn.IndexOf("\n") > 0)
            {
                strReturn = strReturn.Replace("\n", @"</div><div>");
                strReturn = @"<div>" + strReturn + @"</div>";
            }

            return strReturn;
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"varSourceString = {0}, varDestString={1}", varSourceString, varDestString));

                varDestString = FormattingForRichText(varSourceString);
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
