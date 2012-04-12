using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace wfTest.Workflow1
{
    public sealed partial class Workflow1
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            this.createItemActivity1 = new Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity();
            this.applyActivation1 = new Microsoft.SharePoint.WorkflowActions.ApplyActivation();
            this.createTask1 = new Microsoft.SharePoint.WorkflowActions.CreateTask();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // createItemActivity1
            // 
            activitybind1.Name = "applyActivation1";
            activitybind1.Path = "__Context";
            activitybind2.Name = "Workflow1";
            activitybind2.Path = "createItemActivity1_ItemProperties1";
            activitybind3.Name = "Workflow1";
            activitybind3.Path = "applyActivation1___Context1.ListId";
            this.createItemActivity1.Name = "createItemActivity1";
            activitybind4.Name = "Workflow1";
            activitybind4.Path = "createItemActivity1_NewItemId1";
            this.createItemActivity1.Overwrite = false;
            this.createItemActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity.@__ContextProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.createItemActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity.ItemPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.createItemActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity.ListIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.createItemActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity.NewItemIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // applyActivation1
            // 
            activitybind5.Name = "Workflow1";
            activitybind5.Path = "applyActivation1___Context1";
            activitybind6.Name = "Workflow1";
            activitybind6.Path = "applyActivation1___WorkflowProperties1";
            this.applyActivation1.Name = "applyActivation1";
            this.applyActivation1.SetBinding(Microsoft.SharePoint.WorkflowActions.ApplyActivation.@__ContextProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.applyActivation1.SetBinding(Microsoft.SharePoint.WorkflowActions.ApplyActivation.@__WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // createTask1
            // 
            correlationtoken1.Name = "Task1Token";
            correlationtoken1.OwnerActivityName = "Workflow1";
            this.createTask1.CorrelationToken = correlationtoken1;
            this.createTask1.ListItemId = -1;
            this.createTask1.Name = "createTask1";
            this.createTask1.SpecialPermissions = null;
            activitybind7.Name = "Workflow1";
            activitybind7.Path = "createTask1_TaskId1";
            activitybind8.Name = "Workflow1";
            activitybind8.Path = "createTask1_TaskProperties1";
            this.createTask1.MethodInvoking += new System.EventHandler(this.createTask1_MethodInvoking);
            this.createTask1.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.createTask1.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            activitybind10.Name = "Workflow1";
            activitybind10.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "Workflow1";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken2;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind9.Name = "Workflow1";
            activitybind9.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // Workflow1
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.createTask1);
            this.Activities.Add(this.applyActivation1);
            this.Activities.Add(this.createItemActivity1);
            this.Name = "Workflow1";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.WithKey.CreateItemActivity createItemActivity1;

        private Microsoft.SharePoint.WorkflowActions.ApplyActivation applyActivation1;

        private Microsoft.SharePoint.WorkflowActions.CreateTask createTask1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;























    }
}
