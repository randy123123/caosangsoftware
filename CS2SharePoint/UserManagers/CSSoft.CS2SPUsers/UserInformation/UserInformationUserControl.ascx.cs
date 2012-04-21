using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Text;

namespace CSSoft.CS2SPUsers.UserInformation
{
    public partial class UserInformationUserControl : UserControl
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                WindowsImpersonationContext aspContext = null;

                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                aspContext = identity.Impersonate();

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

                UserPrincipal u = new UserPrincipal(ctx);
                u = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, identity.User.ToString());
                StringBuilder userInfoBuilder = new StringBuilder();
                userInfoBuilder.AppendFormat("Display Name: {0}<br>", u.DisplayName);
                userInfoBuilder.AppendFormat("Email Address: {0}<br>", u.EmailAddress);
                userInfoBuilder.AppendFormat("Last Logon: {0}<br>", u.LastLogon);
                userInfoBuilder.AppendFormat("Last Password Set: {0}<br>", u.LastPasswordSet);
                userInfoBuilder.AppendFormat("Last Bad Password Attempt: {0}<br>", u.LastBadPasswordAttempt);
                userInfoBuilder.AppendFormat("Account Expiration Date: {0}<br>", u.AccountExpirationDate);
                LiteralUserInformation.Text = userInfoBuilder.ToString();
            });
        }
    }
}
