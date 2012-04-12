using System;
using System.Collections;
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
    public class getSumByCAML : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getSumByCAML));
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

        public static DependencyProperty ListTitleProperty = DependencyProperty.Register("ListTitle",
            typeof(string), typeof(getSumByCAML));

        [Description("List Title")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string ListTitle
        {
            get
            {
                return ((string)
                (base.GetValue(ListTitleProperty)));
            }
            set
            {
                base.SetValue(ListTitleProperty, value);
            }
        }

        public static DependencyProperty VarCAMLProperty = DependencyProperty.Register("VarCAML",
            typeof(string), typeof(getSumByCAML));

        [Description("VarCAML")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string VarCAML
        {
            get
            {
                return ((string)
                (base.GetValue(VarCAMLProperty)));
            }
            set
            {
                base.SetValue(VarCAMLProperty, value);
            }
        }

        public static DependencyProperty ItemsSumProperty = DependencyProperty.Register("ItemsSum",
            typeof(int), typeof(getSumByCAML));

        [Description("Items Sum")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public int ItemsSum
        {
            get
            {
                return ((int)
                (base.GetValue(ItemsSumProperty)));
            }
            set
            {
                base.SetValue(ItemsSumProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getSumByCAML", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSite objSPSite = (SPSite)__Context.Site;
            SPWeb objSPWeb = (SPWeb)__Context.Web;

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Site.ServerRelativeUrl=" + objSPSite.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Web.ServerRelativeUrl=" + objSPWeb.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ListTitle=" + ListTitle);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"VarCAML=" + VarCAML);

            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            SPQuery objSPQuery = new SPQuery();

            try
            {
                objSPList = objSPWeb.Lists[new Guid(ListTitle)];
                if (objSPList == null)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"list title(" + ListTitle + ") is invalid");
                    return ActivityExecutionStatus.Faulting;
                }

                objSPQuery.Query = VarCAML;
                objSPQuery.RowLimit = 0;
                objSPQuery.ViewFields = @"<FieldRef Name='ID' />";
                objSPListItemCollection = objSPList.GetItems(objSPQuery);
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"objSPListItemCollection.Sum=" + objSPListItemCollection.Count.ToString());

                ItemsSum = objSPListItemCollection.Count;
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.Message=" + ex.Message);
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.StackTrace=" + ex.StackTrace);
                return ActivityExecutionStatus.Faulting;
            }

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"activity completed.");
            return ActivityExecutionStatus.Closed;
        }
    }
}
