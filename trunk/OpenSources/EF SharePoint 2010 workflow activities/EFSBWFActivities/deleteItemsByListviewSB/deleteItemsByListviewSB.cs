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
    public class deleteItemsByListviewSB
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
                        web.CurrentUser, TimeSpan.Zero, "deleteItemsByListviewSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string ListViewTitle, string ListTitle)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, string.Format("begin... ListViewTitle={0}", ListViewTitle));

            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            SPView objSPView = null;
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
                        foreach (SPView item in objSPList.Views)
                        {
                            if (item.Title.Equals(ListViewTitle, StringComparison.InvariantCultureIgnoreCase))
                            {
                                objSPView = item;
                                break;
                            }
                        }
                        if (objSPView == null)
                        {
                            Log(context, string.Format(@"View ({0}) doesn't exist in list({1}) under web {2}", ListViewTitle, ListTitle, objSPWeb.ServerRelativeUrl));
                            return result;
                        }

                        previousAllowUnsafeUpdates = objSPWeb.AllowUnsafeUpdates;
                        if (previousAllowUnsafeUpdates == false)
                        {
                            objSPWeb.AllowUnsafeUpdates = true;
                        }
                        //objSPField = objSPList.Fields[FieldName];

                        objSPListItemCollection = objSPList.GetItems(objSPView);
                        LogDebugInfo(context, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());

                        for (int intIndex = objSPListItemCollection.Count - 1; intIndex > -1; intIndex--)
                        {
                            objSPListItemCollection.Delete(intIndex);
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
