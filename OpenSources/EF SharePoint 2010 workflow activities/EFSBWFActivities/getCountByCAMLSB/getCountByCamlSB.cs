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
    public class getCountByCamlSB
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
                        web.CurrentUser, TimeSpan.Zero, "getCountByCamlSB", strMessage, string.Empty);
                }
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string ListTitle, string VarCAML)
        {
            Hashtable result = new Hashtable();

            LogDebugInfo(context, @"ListTitle=" + ListTitle);
            LogDebugInfo(context, @"VarCAML=" + VarCAML);

            SPList objSPList = null;
            SPListItemCollection objSPListItemCollection = null;
            SPQuery objSPQuery = new SPQuery();

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPList = objSPWeb.Lists[new Guid(ListTitle)];
                        if (objSPList == null)
                        {
                            Log(context, @"list GUID(" + ListTitle + ") is invalid");
                            return result;
                        }

                        objSPQuery.Query = VarCAML;
                        objSPQuery.RowLimit = 0;
                        objSPQuery.ViewFields = @"<FieldRef Name='ID' />";
                        objSPListItemCollection = objSPList.GetItems(objSPQuery);
                        LogDebugInfo(context, @"objSPListItemCollection.Count=" + objSPListItemCollection.Count.ToString());

                        result["ItemsCount"] = objSPListItemCollection.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(context, @"ex.Message=" + ex.Message);
                Log(context, @"ex.StackTrace=" + ex.StackTrace);
                return result;
            }

            LogDebugInfo(context, @"activity completed.");
            return result;
        }
    }
}
