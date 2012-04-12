using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace EFSPWFActivities.Features.getSumByCAML
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("05524033-d7f7-4549-8afc-139792bd513f")]
    public class getSumByCAMLEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebService contentService = SPWebService.ContentService;
            contentService.WebConfigModifications.Add(GetConfigModification());
            contentService.Update();
            contentService.ApplyWebConfigModifications();
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}

        public SPWebConfigModification GetConfigModification()
        {
            string assemblyValue = typeof(EFSPWFActivities.getSumByCAML).Assembly.FullName;
            string namespaceValue = typeof(EFSPWFActivities.getSumByCAML).Namespace;
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
