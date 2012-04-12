using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Workflow;

namespace EFSBWFActivities
{
    [ToolboxItemAttribute(true)]
    public class getFieldValueFromPreviousVersionSB
    {
        #region Logging
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
                        web.CurrentUser, TimeSpan.Zero, "getFieldValueFromPreviousVersionSB", strMessage, string.Empty);
                }
            }
        }


        #endregion

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string FieldName)
        {
            Hashtable results = new Hashtable();
            //results["Except"] = string.Empty;

            //LogDebugInfo(context, string.Format("getFieldValueFromPreviousVersionSB.Execute() begin... strFieldName={0}", strFieldName));

            string strFieldValue = string.Empty;
            SPList objSPList = null;
            SPListItem objSPListItem = null;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        objSPList = objSPWeb.Lists[context.ListId];
                        objSPListItem = objSPList.GetItemById(context.ItemId);
                        if (objSPListItem == null)
                        {
                            Log(context, string.Format(@"current item (url: {0}, ItemId: {1}) is invalid", context.CurrentItemUrl, context.ItemId));
                            return results;
                        }
                        if (objSPListItem.Fields.ContainsField(FieldName) == false)
                        {
                            Log(context, string.Format(@"Specified field ({0}) is invalid in list (1)", FieldName, objSPListItem.ParentList.Title));
                            return results;
                        }

                        SPListItemVersionCollection objVerisionColl = objSPListItem.Versions;
                        if (objVerisionColl.Count > 1)
                        {
                            SPListItemVersion objSPListItemPrevious = objVerisionColl[1];
                            strFieldValue = Convert.ToString(objSPListItemPrevious[FieldName]);
                        }

                        results["FieldValue"] = strFieldValue;

                        //LogDebugInfo(context, string.Format("getFieldValueFromPreviousVersionSB.Execute() completed."));
                    }
                }
            }
            catch (Exception ex)
            {
                Log(context, string.Format(@"ex.Message = {0}", ex.Message));
                Log(context, string.Format(@"ex.StackTrace = {0}", ex.StackTrace));
                //results["Result"] = "Failure";
                //results["Except"] = ex.ToString();
            }

            return results;
        }
    }
}
