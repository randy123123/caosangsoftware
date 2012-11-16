using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;
using System.IO;

namespace Visigo.Sharepoint.FormsBasedAuthentication.UserSettings
{
    public partial class UserSettingsUserControl : UserControl
    {
        public SPUser CurrentUser { get { return SPContext.Current.Web.CurrentUser; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = "";
            if (!Page.IsPostBack)
            {
                lblUserName.Text = GetName(CurrentUser.LoginName);
                txtFullName.Text = CurrentUser.Name;
                txtEmailAddress.Text = CurrentUser.Email;

                SPListItem userItem = SPContext.Current.Web.SiteUserInfoList.GetItemById(CurrentUser.ID);
                if (userItem["Picture"] != null)
                {
                    SPFieldUrlValue valuePicture = new SPFieldUrlValue(Convert.ToString(userItem["Picture"]));
                    imgPicture.ImageUrl = valuePicture.Url;
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                try
                {
                    SPListItem userItem = SPContext.Current.Web.SiteUserInfoList.GetItemById(CurrentUser.ID);
                    userItem["Title"] = txtFullName.Text;
                    userItem["EMail"] = txtEmailAddress.Text;

                    if (filePicture.HasFile)
                    {
                        string filePath = String.Format("{0}{1}", UserPicturePath, Path.GetExtension(filePicture.FileName));
                        filePicture.SaveAs(filePath);
                        SPFieldUrlValue valuePicture = new SPFieldUrlValue();
                        valuePicture.Description = txtFullName.Text;
                        valuePicture.Url = String.Format("{0}{1}", UserPictureUrl, Path.GetFileName(filePath));
                        userItem["Picture"] = valuePicture;
                    }
                    userItem.SystemUpdate(false);
                    Response.Redirect(Request.RawUrl);
                }
                catch (Exception ex)
                {
                    Message.Text += String.Format("<div class='ErrorMessage' title='Error'>{0}</div>", ex.Message);
                }
            }
        }

        private bool IsValid()
        {

            return Message.Text.Length == 0;
        }

        public string GetName(string name)
        {
            if (name.Contains("|"))
                return name.Substring(name.LastIndexOf('|') + 1);
            else if (name.Contains("\\"))
                return name.Substring(name.LastIndexOf('\\') + 1);
            else if (name.Contains("#"))
                return name.Substring(name.LastIndexOf('#') + 1);
            else return name;
        }
        
        public string UserPictureUrl
        {
            get
            {
                return String.Format("/_layouts/FBA/Images/{0}/", SPContext.Current.Web.ID.ToString());
            }
        }
        
        public string UserPicturePath
        {
            get
            {
                string path = Path.Combine(@"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\FBA\Images\", SPContext.Current.Web.ID.ToString());
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return Path.Combine(path, lblUserName.Text);
            }
        }
    }
}
