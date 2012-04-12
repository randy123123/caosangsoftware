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
    public class updateItemsByCAML : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(updateItemsByCAML));
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
        //    typeof(string), typeof(updateItemsByCAML));

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
        //    typeof(int), typeof(updateItemsByCAML));

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

        public static DependencyProperty ListTitleProperty = DependencyProperty.Register("ListTitle",
            typeof(string), typeof(updateItemsByCAML));

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

        public static DependencyProperty FieldNameProperty = DependencyProperty.Register("FieldName",
            typeof(string), typeof(updateItemsByCAML));

        [Description("Field Name")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string FieldName
        {
            get
            {
                return ((string)
                (base.GetValue(FieldNameProperty)));
            }
            set
            {
                base.SetValue(FieldNameProperty, value);
            }
        }

        public static DependencyProperty FieldValueProperty = DependencyProperty.Register("FieldValue",
            typeof(string), typeof(updateItemsByCAML));

        [Description("Field Value")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string FieldValue
        {
            get
            {
                return ((string)
                (base.GetValue(FieldValueProperty)));
            }
            set
            {
                base.SetValue(FieldValueProperty, value);
            }
        }

        //<Where><Contains><FieldRef Name='field1' /><Value Type='Text'>aa</Value></Contains></Where>
        public static DependencyProperty VarCAMLProperty = DependencyProperty.Register("VarCAML",
            typeof(string), typeof(updateItemsByCAML));

        [Description("CAML variable")]
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

        public static DependencyProperty NewVersionProperty = DependencyProperty.Register("NewVersion",
            typeof(string), typeof(updateItemsByCAML));

        [Description("New Version (yes or no)")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string NewVersion
        {
            get
            {
                return ((string)
                (base.GetValue(NewVersionProperty)));
            }
            set
            {
                base.SetValue(NewVersionProperty, value);
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "updateItemsByCAML", description, string.Empty);
            });
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSite objSPSite = (SPSite)__Context.Site;
            SPWeb objSPWeb = (SPWeb)__Context.Web;
            //SPField objSPField = null;

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Site.ServerRelativeUrl=" + objSPSite.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"Web.ServerRelativeUrl=" + objSPWeb.ServerRelativeUrl);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ListTitle=" + ListTitle);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"FieldName=" + FieldName);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"FieldValue=" + FieldValue);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"VarCAML=" + VarCAML);
            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"NewVersion=" + NewVersion);

            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            SPQuery objSPQuery = new SPQuery();
            bool previousAllowUnsafeUpdates = false;

            try
            {
                objSPList = objSPWeb.Lists[new Guid(ListTitle)];
                if (objSPList == null)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"list title(" + ListTitle + ") is invalid");
                    return ActivityExecutionStatus.Faulting;
                }
                if (objSPList.Fields.ContainsField(FieldName) == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"field name(" + FieldName + ") is invalid");
                    return ActivityExecutionStatus.Faulting;
                }

                previousAllowUnsafeUpdates = objSPWeb.AllowUnsafeUpdates;
                if (previousAllowUnsafeUpdates == false)
                {
                    objSPWeb.AllowUnsafeUpdates = true;
                }
                //objSPField = objSPList.Fields[FieldName];

                //objSPQuery.ViewFields = string.Format("<FieldRef Name='{0}' />", FieldName);
                objSPQuery.Query = VarCAML;
                objSPQuery.RowLimit = 0;
                objSPListItemCollection = objSPList.GetItems(objSPQuery);
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());
                foreach (SPListItem item in objSPListItemCollection)
                {
                    item[FieldName] = FieldValue;
                    if (NewVersion.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //item.UpdateOverwriteVersion();
                        item.SystemUpdate(false);
                    }
                    else
                    {
                        item.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"updateItemsByCAML exception.");
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.Message=" + ex.Message);
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"ex.StackTrace=" + ex.StackTrace);
                return ActivityExecutionStatus.Faulting;
            }
            finally
            {
                if (previousAllowUnsafeUpdates != objSPWeb.AllowUnsafeUpdates)
                {
                    objSPWeb.AllowUnsafeUpdates = previousAllowUnsafeUpdates;
                }
            }

            WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"updateItemsByCAML activity completed.");
            return ActivityExecutionStatus.Closed;
        }
    }
}
