using System;
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
    //return value: string, separated by ';'

    public class getUserLoginsByGroupName : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(getUserLoginsByGroupName));

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

        public static DependencyProperty UserGroupNameProperty =
            DependencyProperty.Register("UserGroupName",
            typeof(string),
            typeof(getUserLoginsByGroupName));

        [Description("User Group Name of current site collection")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string UserGroupName
        {
            get
            {
                return ((string)
                (base.GetValue(UserGroupNameProperty)));
            }
            set
            {
                base.SetValue(UserGroupNameProperty, value);
            }
        }

        public static DependencyProperty UserLoginNameListProperty =
            DependencyProperty.Register("UserLoginNameList",
            typeof(string),
            typeof(getUserLoginsByGroupName));

        [Description("User Login name list (separated by semi colon)")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public string UserLoginNameList
        {
            get
            {
                return (string)base.GetValue(UserLoginNameListProperty);
            }
            set
            {
                base.SetValue(UserLoginNameListProperty, value);
            }
        }

        //public SPWorkflowActivationProperties _WorkflowProperties = new SPWorkflowActivationProperties();

        public bool GroupExists(SPWeb web, string name)
        {
            var groupExists = false;
            foreach (SPGroup group in web.SiteGroups.Cast<SPGroup>().Where(siteGroup => siteGroup.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                groupExists = true;
            }
            return groupExists;
        }

        public SPGroup ReturnGroup(SPWeb web, String name)
        {
            return web.SiteGroups.Cast<SPGroup>().FirstOrDefault(siteGroup => siteGroup.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "getUserLoginsByGroupName", description, string.Empty);
            });
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

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            bool bReturn = false;
            SPGroup objSPGroup = null;
            List<SPUser> objListUsers = new List<SPUser>();
            string strUserLoginNameList = string.Empty;

            try
            {
                //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                //System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
                //DateTime oCreationTime = fileInfo.CreationTime;

                //WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"assembly: oCreationTime={0}, FullName={1}, Length={2}", oCreationTime, fileInfo.FullName, fileInfo.Length));

                //WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, @"custom activity getUserLoginsByGroupName() begin");
                ///WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"web url = {0}", __Context.Web.Url));

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    bReturn = GroupExists(__Context.Web, UserGroupName);
                });

                if (bReturn == false)
                {
                    WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"User Group Name ({0}) doesn't exist!", UserGroupName));
                    return ActivityExecutionStatus.Faulting;
                }

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    objSPGroup = ReturnGroup(__Context.Web, UserGroupName);
                    objListUsers = getListUsers(__Context.Web, objSPGroup);
                    foreach (SPUser objSPUser in objListUsers)
                    {
                        if (string.IsNullOrEmpty(strUserLoginNameList) == false)
                            strUserLoginNameList += @";";
                        strUserLoginNameList += objSPUser.LoginName;
                    }
                });
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return ActivityExecutionStatus.Faulting;
            }

            UserLoginNameList = strUserLoginNameList;
            //WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"strUserLoginNameList = {0}", strUserLoginNameList));

            return ActivityExecutionStatus.Closed;
        }

    }
}
