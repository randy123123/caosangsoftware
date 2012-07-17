using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace CSSoft.EmailMarketing
{
    public partial class SendMailToCustomersUserControl : UserControl
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string ListEmailTemplate = "Email Template";
        public const string ListContacts = "Contacts";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadEmailTemplate();
            }
        }

        private void LoadEmailTemplate()
        {
            EmailTemplate.Items.Clear();
            EmailTemplate.Items.Add(new ListItem("-", "0"));
            SPList emailTemplate = CS2Web.GetList(ListEmailTemplate);
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                          <And>
                             <Neq>
                                <FieldRef Name='Status' />
                                <Value Type='Choice'>Ready send</Value>
                             </Neq>
                             <Neq>
                                <FieldRef Name='Status' />
                                <Value Type='Choice'>Sending...</Value>
                             </Neq>
                          </And>
                       </Where>";
            SPListItemCollection items = emailTemplate.GetItems(query);
            if (items != null && items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    EmailTemplate.Items.Add(new ListItem(CS2Convert.ToString(item["Key"]), item.ID.ToString()));
                }
            }
            EnableControls();
        }

        private void EnableControls()
        {
            bool enableButton = EmailTemplate.Items.Count > 1 && EmailTemplate.SelectedValue != "0";
            ImageButtonStartReview.Enabled = enableButton;
            ImageButtonStartSendMail.Enabled = enableButton;
            ActionPanel.Visible = enableButton;
            ReviewMail.Visible = enableButton;
        }

        protected void EmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmailTemplate.SelectedValue != "0")
            {
                SPListItem item = CS2Web.GetList(ListEmailTemplate).GetItemById(CS2Convert.ToInt(EmailTemplate.SelectedValue));
                Subject.Text = CS2Convert.ToString(item.Title);
                Body.Text = CS2Convert.ToString(item[SPBuiltInFieldId.Body]);
                Msg.Text = "";
            } 
            EnableControls();
        }

        protected void ImageButtonStartReview_Click(object sender, ImageClickEventArgs e)
        {
            if (EmailTemplate.SelectedValue != "0")
            {
                SPListItem item = CS2Web.GetList(ListEmailTemplate).GetItemById(CS2Convert.ToInt(EmailTemplate.SelectedValue));
                string subject = CS2Convert.ToString(item.Title);
                string body = CS2Convert.ToString(item[SPBuiltInFieldId.Body]);

                SPList contactsList = CS2Web.GetList(ListContacts);
                SPQuery query = new SPQuery();
                query.RowLimit = 1;
                //query.Query = "<Where><Eq><FieldRef Name='Publish' /><Value Type='Boolean'>1</Value></Eq></Where>";
                SPListItemCollection contacts = contactsList.GetItems(query);
                if (contacts != null && contacts.Count > 0)
                {
                    SPListItem contact = contacts[0];
                    Regex regex = new Regex("{(?<Property>[^}]*)");

                    //Subject
                    MatchCollection matchs = regex.Matches(subject);

                    foreach (Match match in matchs)
                    {
                        string property = (match.Groups["Property"].Value);
                        string propertyValue = "";
                        try
                        {
                            propertyValue = CS2Convert.ToString(contact[property]);
                        }
                        catch { }
                        subject = subject.Replace(String.Format("{{{0}}}", property), propertyValue);
                    }
                    
                    //Body
                    matchs = regex.Matches(body);

                    foreach (Match match in matchs)
                    {
                        string property = (match.Groups["Property"].Value);
                        string propertyValue = "";
                        try
                        {
                            propertyValue = CS2Convert.ToString(contact[property]);
                        }
                        catch { }
                        body = body.Replace(String.Format("{{{0}}}", property), propertyValue);
                    }
                }
                Subject.Text = subject;
                Body.Text = body;
            }
        }

        protected void ImageButtonStartSendMail_Click(object sender, ImageClickEventArgs e)
        {
            if (EmailTemplate.SelectedValue != "0")
            {
                SPListItem item = CS2Web.GetList(ListEmailTemplate).GetItemById(CS2Convert.ToInt(EmailTemplate.SelectedValue));
                item["Status"] = "Ready send";
                item.SystemUpdate();
                Msg.Text = String.Format("<br/>Email template '{0}' is sending.<br/>", CS2Convert.ToString(item["Key"]));
                EmailTemplate.Items.Remove(EmailTemplate.SelectedItem);
                EnableControls();
            }
        }
    }
}
