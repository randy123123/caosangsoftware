using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace EFSPWFActivities.Features.waitForUnlockWorkflow
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("5f5a73d5-24d8-45bd-b8d2-803db32fb2b3")]
    public class waitForUnlockWorkflowEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebService contentService = SPWebService.ContentService;
            contentService.WebConfigModifications.Add(GetConfigModification());
            contentService.Update();
            contentService.ApplyWebConfigModifications();
        }

        public SPWebConfigModification GetConfigModification()
        {
            string assemblyValue = typeof(EFSPWFActivities.waitForUnlockWorkflow).Assembly.FullName;
            string namespaceValue = typeof(EFSPWFActivities.waitForUnlockWorkflow).Namespace;
            SPWebConfigModification modification = new SPWebConfigModification(
                string.Format(CultureInfo.CurrentCulture,
                    @"authorizedType[@Assembly='{0}'][@Namespace='{1}'][@TypeName='*'][@Authorized='True']",
                    assemblyValue, namespaceValue),
                @"configuration/System.Workflow.ComponentModel.WorkflowCompiler/authorizedTypes");
            modification.Owner = "EFSPWFActivities";
            modification.Sequence = 0;
            modification.Type = SPWebConfigModification.
            SPWebConfigModificationType.EnsureChildNode;
            modification.Value = Environment.NewLine + string.Format(CultureInfo.CurrentCulture,
                @" <authorizedType Assembly=""{0}"" Namespace=""{1}"" TypeName=""*"" Authorized=""True"" /> ",
                assemblyValue, namespaceValue);
            //Trace.TraceInformation(@"getUserLoginsByGroupNameEventReceiver SPWebConfigModification value: {0}", modification.Value);
            return modification;
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
