using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.IO;
using System.Text.RegularExpressions;

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
            //Valid Full Name required
            if (String.IsNullOrEmpty(txtFullName.Text))
                Message.Text += String.Format("<div class='ErrorMessage' title='Error'>{0}</div>", "You must specify a value for 'Full Name' field.");
            //Valid Email Address
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(txtEmailAddress.Text);
            if (String.IsNullOrEmpty(txtEmailAddress.Text) || !match.Success)
                Message.Text += String.Format("<div class='ErrorMessage' title='Error'>{0}</div>", "You must specify a email value for 'Email Address' field.");
            //Value Picture
            if (filePicture.HasFile)
            {
                if (filePicture.FileContent.Length > 1000000) // 1MB approx (actually less though)
                {
                    Message.Text += String.Format("<div class='ErrorMessage' title='Error'>{0}</div>", "The system will only allow image files with the size &lt 1MB for 'Email Address' field.");
                }
                else
                {
                    string fileExt = Path.GetExtension(filePicture.FileName).ToLower();
                    if (!(fileExt.Equals(".gif") || fileExt.Equals(".jpg") || fileExt.Equals(".png") || fileExt.Equals(".jpg")))
                        Message.Text += String.Format("<div class='ErrorMessage' title='Error'>{0}</div>", "The system will only allow image files of the type GIF, JPG, JPEG, or PNG for 'Email Address' field.");
                }
            }
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
