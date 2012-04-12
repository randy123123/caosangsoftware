using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Workflow;

namespace EFSBWFActivities
{
    [ToolboxItemAttribute(true)]
    public class updateItemsByCamlSB
    {
        public static void LogDebugInfo(SPUserCodeWorkflowContext context, string strMessage)
        {
#if DEBUG
            Log(context, strMessage);
#endif
        }

        public static void Log(SPUserCodeWorkflowContext context, string strMessage)
        {
            using (SPSite site = new SPSite(context.CurrentWebUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPWorkflow.CreateHistoryEvent(web, context.WorkflowInstanceId, 0,
                        web.CurrentUser, TimeSpan.Zero, "updateItemsByCamlSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string ListTitle, string FieldName, string FieldValue, string VarCAML, Boolean NewVersion)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, string.Format("begin... VarCAML={0}", VarCAML));

            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            SPQuery objSPQuery = new SPQuery();
            bool previousAllowUnsafeUpdates = false;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPList = objSPWeb.Lists[new Guid(ListTitle)];
                        if (objSPList == null)
                        {
                            Log(context, string.Format(@"list title({0}) is invalid", ListTitle));
                            return result;
                        }
                        if (objSPList.Fields.ContainsField(FieldName) == false)
                        {
                            Log(context, string.Format(@"field name({0}) is invalid in list (1)", FieldName, ListTitle));
                            return result;
                        }

                        previousAllowUnsafeUpdates = objSPWeb.AllowUnsafeUpdates;
                        if (previousAllowUnsafeUpdates == false)
                        {
                            objSPWeb.AllowUnsafeUpdates = true;
                        }
                        //objSPField = objSPList.Fields[FieldName];

                        objSPQuery.Query = VarCAML;
                        objSPListItemCollection = objSPList.GetItems(objSPQuery);
                        LogDebugInfo(context, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());
                        foreach (SPListItem item in objSPListItemCollection)
                        {
                            item[FieldName] = FieldValue;
                            if (NewVersion == false)
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
                }
            }
            catch (Exception ex)
            {
                Log(context, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
            }

            return result;
        }
    }
}
