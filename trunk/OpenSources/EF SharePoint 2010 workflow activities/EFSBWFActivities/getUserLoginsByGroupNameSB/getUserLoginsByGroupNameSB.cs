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
    public class getUserLoginsByGroupNameSB
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
                        web.CurrentUser, TimeSpan.Zero, "getUserLoginsByGroupNameSB", strMessage, string.Empty);
                }
            }
        }

        public static bool GroupExists(SPWeb web, string name)
        {
            var groupExists = false;
            foreach (SPGroup group in web.SiteGroups.Cast<SPGroup>().Where(siteGroup => siteGroup.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                groupExists = true;
            }
            return groupExists;
        }

        public static SPGroup ReturnGroup(SPWeb web, String name)
        {
            return web.SiteGroups.Cast<SPGroup>().FirstOrDefault(siteGroup => siteGroup.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static List<SPUser> getListUsers(SPWeb web, SPPrincipal group)
        {
            try
            {
                web.Site.CatchAccessDeniedException = false;
                var users = new List<SPUser>();
                foreach (SPUser user in web.SiteUsers)
                {
                    using (var userContextSite = new SPSite(web.Site.ID, user.UserToken))
                    {
                        try
                        {
                            using (var userContextWeb = userContextSite.OpenWeb(web.ID))
                            {
                                try
                                {
                                    if (userContextWeb.SiteGroups[group.Name].ContainsCurrentUser)
                                        users.Add(user);
                                }
                                catch (SPException)
                                {                             // group not found, continue
                                }
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {                     // user does not have right to open this web, continue
                        }
                    }
                }
                return users;
            }
            finally
            {
                web.Site.CatchAccessDeniedException = true;
            }
        }

        public static Hashtable Execute(SPUserCodeWorkflowContext context, string UserGroupName)
        {
            Hashtable result = new Hashtable();
            bool bReturn = false;
            SPGroup objSPGroup = null;
            List<SPUser> objListUsers = new List<SPUser>();
            string strUserLoginNameList = string.Empty;

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb objSPWeb = site.OpenWeb())
                    {
                        bReturn = GroupExists(objSPWeb, UserGroupName);
                        if (bReturn == false)
                        {
                            LogDebugInfo(context, string.Format(@"User Group Name ({0}) doesn't exist!", UserGroupName));
                            return result;
                        }

                        objSPGroup = ReturnGroup(objSPWeb, UserGroupName);
                        objListUsers = getListUsers(objSPWeb, objSPGroup);
                        foreach (SPUser objSPUser in objListUsers)
                        {
                            if (string.IsNullOrEmpty(strUserLoginNameList) == false)
                                strUserLoginNameList += @";";
                            strUserLoginNameList += objSPUser.LoginName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(context, @"ex.Message=" + ex.Message);
                Log(context, @"ex.StackTrace=" + ex.StackTrace);
            }

            return result;
        }
    }
}
