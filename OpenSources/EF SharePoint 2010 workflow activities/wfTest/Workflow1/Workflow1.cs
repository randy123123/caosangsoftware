using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace wfTest.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public static DependencyProperty createTask1_TaskId1Property = DependencyProperty.Register("createTask1_TaskId1", typeof(System.Guid), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Guid createTask1_TaskId1
        {
            get
            {
                return ((System.Guid)(base.GetValue(wfTest.Workflow1.Workflow1.createTask1_TaskId1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.createTask1_TaskId1Property, value);
            }
        }

        public static DependencyProperty createTask1_TaskProperties1Property = DependencyProperty.Register("createTask1_TaskProperties1", typeof(Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public SPWorkflowTaskProperties createTask1_TaskProperties1
        {
            get
            {
                return ((Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties)(base.GetValue(wfTest.Workflow1.Workflow1.createTask1_TaskProperties1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.createTask1_TaskProperties1Property, value);
            }
        }

        public static void WriteDebugInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
            TimeSpan ts = new TimeSpan();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "debug, wfTest", description, string.Empty);
            });
        }

        private void createTask1_MethodInvoking(object sender, EventArgs e)
        {
            try
            {
                createTask1_TaskId1 = Guid.NewGuid();
                createTask1_TaskProperties1 = new SPWorkflowTaskProperties();

                SPUser user = workflowProperties.OriginatorUser;
                createTask1_TaskProperties1.AssignedTo = user.LoginName;

                createTask1_TaskProperties1.Title = "Approval request 1";
                createTask1_TaskProperties1.PercentComplete = (float)0.0;
                createTask1_TaskProperties1.DueDate = DateTime.Now.AddDays(1);
                createTask1_TaskProperties1.StartDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                WriteDebugInfoToHistoryLog(workflowProperties.Web, workflowProperties.WorkflowId, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                throw ex;
            }
        }

        public static DependencyProperty getUserLoginsByGroupName1_UserGroupName1Property = DependencyProperty.Register("getUserLoginsByGroupName1_UserGroupName1", typeof(System.String), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("EF Workflow Activities")]
        public String getUserLoginsByGroupName1_UserGroupName1
        {
            get
            {
                return ((string)(base.GetValue(wfTest.Workflow1.Workflow1.getUserLoginsByGroupName1_UserGroupName1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.getUserLoginsByGroupName1_UserGroupName1Property, value);
            }
        }

        public static DependencyProperty getUserLoginsByGroupName1_UserLoginNameList1Property = DependencyProperty.Register("getUserLoginsByGroupName1_UserLoginNameList1", typeof(System.String), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("EF Workflow Activities")]
        public String getUserLoginsByGroupName1_UserLoginNameList1
        {
            get
            {
                return ((string)(base.GetValue(wfTest.Workflow1.Workflow1.getUserLoginsByGroupName1_UserLoginNameList1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.getUserLoginsByGroupName1_UserLoginNameList1Property, value);
            }
        }

        public static DependencyProperty applyActivation1___Context1Property = DependencyProperty.Register("applyActivation1___Context1", typeof(Microsoft.SharePoint.WorkflowActions.WorkflowContext), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public WorkflowContext applyActivation1___Context1
        {
            get
            {
                return ((Microsoft.SharePoint.WorkflowActions.WorkflowContext)(base.GetValue(wfTest.Workflow1.Workflow1.applyActivation1___Context1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.applyActivation1___Context1Property, value);
            }
        }

        public static DependencyProperty applyActivation1___WorkflowProperties1Property = DependencyProperty.Register("applyActivation1___WorkflowProperties1", typeof(Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public SPWorkflowActivationProperties applyActivation1___WorkflowProperties1
        {
            get
            {
                return ((Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties)(base.GetValue(wfTest.Workflow1.Workflow1.applyActivation1___WorkflowProperties1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.applyActivation1___WorkflowProperties1Property, value);
            }
        }

        public static DependencyProperty createItemActivity1_ItemProperties1Property = DependencyProperty.Register("createItemActivity1_ItemProperties1", typeof(System.Collections.Hashtable), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Hashtable createItemActivity1_ItemProperties1
        {
            get
            {
                return ((System.Collections.Hashtable)(base.GetValue(wfTest.Workflow1.Workflow1.createItemActivity1_ItemProperties1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.createItemActivity1_ItemProperties1Property, value);
            }
        }

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            createItemActivity1_ItemProperties1 = new Hashtable();
            createItemActivity1_ItemProperties1.Add("Title", "My New Item Title");
        }

        public static DependencyProperty createItemActivity1_NewItemId1Property = DependencyProperty.Register("createItemActivity1_NewItemId1", typeof(Microsoft.SharePoint.Workflow.SPItemKey), typeof(wfTest.Workflow1.Workflow1));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public SPItemKey createItemActivity1_NewItemId1
        {
            get
            {
                return ((Microsoft.SharePoint.Workflow.SPItemKey)(base.GetValue(wfTest.Workflow1.Workflow1.createItemActivity1_NewItemId1Property)));
            }
            set
            {
                base.SetValue(wfTest.Workflow1.Workflow1.createItemActivity1_NewItemId1Property, value);
            }
        }
    }
}
