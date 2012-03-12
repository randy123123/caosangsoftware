using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.Field;


namespace CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers
{
    /// <summary>
    /// EditorControl is used to present and manage the UI for administrators adding our field to a List
    /// </summary>
    public class ChildDropDownListFieldEditControl : UserControl, IFieldEditor
    {

        //couldn't get working, just use the delimited example!!!!
        #region Properties
        protected DropDownList ddlLists;
        protected DropDownList ddlColumnValue;
        protected DropDownList ddlColumnText;
        protected DropDownList ddlColumnJoin;
        protected TextBox txtSiteURL;
        protected Button btnLoadLists;
        private string childSiteUrl = default(string);
        private string childListName = default(string);
        private string childListTextField = default(string);
        private string childListValueField = default(string);
        private string childJoinField = default(string);

        #endregion

        /// <summary>
        /// Used when editing our field.  Repopulate UI fields with stored values.  Parse delimited value into constituent parts and retrieve 
        /// Site, List and Column values.
        /// </summary>
        /// <param name="field">The field being edited</param>
        public void InitializeWithField(SPField field)
        {
            ChildDropDownListField ChildField = field as ChildDropDownListField;
            if (ChildField != null)
            {
                childSiteUrl = ChildField.ChildSiteUrl;
                childListName = ChildField.ChildListName;
                childListTextField = ChildField.ChildListTextField;
                childListValueField = ChildField.ChildListValueField;
                childJoinField = ChildField.ChildJoinField;

                EnsureChildControls();

                if (string.IsNullOrEmpty(txtSiteURL.Text) && !string.IsNullOrEmpty(childSiteUrl))
                {
                    txtSiteURL.Text = childSiteUrl;
                    loadLists();
                    ddlLists.Items.FindByValue(childListName).Selected = true;
                    refreshLookups();
                    ddlColumnText.Items.FindByValue(childListTextField).Selected = true;
                    ddlColumnValue.Items.FindByValue(childListValueField).Selected = true;
                    ddlColumnJoin.Items.FindByValue(childJoinField).Selected = true;
                }
            }
        }

        /// <summary>
        /// Event fired when our field values are being saved
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isNew"></param>
        public void OnSaveChange(SPField field, bool isNew)
        {
            childSiteUrl = this.txtSiteURL.Text;
            childListName = this.ddlLists.SelectedValue;
            childListTextField= this.ddlColumnText.SelectedValue;
            childListValueField = this.ddlColumnValue.SelectedValue;
            childJoinField = this.ddlColumnJoin.SelectedValue;

            ChildDropDownListField ChildField = field as ChildDropDownListField;

            //workaround when the field is new SetCustomProperty doesn't seem to work

            if (isNew)
            {
                ChildField.UpdateMyCustomProperty("ChildSiteUrl", childSiteUrl);
                ChildField.UpdateMyCustomProperty("ChildListName", childListName);
                ChildField.UpdateMyCustomProperty("ChildListTextField", childListTextField);
                ChildField.UpdateMyCustomProperty("ChildListValueField", childListValueField);
                ChildField.UpdateMyCustomProperty("ChildJoinField", childJoinField);
            }
            else
            {
                ChildField.ChildSiteUrl = childSiteUrl;
                ChildField.ChildListName = childListName;
                ChildField.ChildListTextField = childListTextField;
                ChildField.ChildListValueField = childListValueField;
                ChildField.ChildJoinField = childJoinField;
            }
            //ChildField.Update();
        }

        /// <summary>
        /// Indicate whether our field should be shown as a new section or appended to the end of the preceding section
        /// </summary>
        public bool DisplayAsNewSection
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.btnLoadLists.Click += new EventHandler(btnLoadLists_Click);
            this.ddlLists.AutoPostBack = true;
            this.ddlLists.EnableViewState = true;
            this.ddlLists.SelectedIndexChanged += new EventHandler(ddlLists_SelectedIndexChanged);

            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.childSiteUrl))
                {
                    txtSiteURL.Text = this.childSiteUrl;
                    loadLists();
                    ddlLists.SelectedValue = this.childListName;
                    refreshLookups();
                    ddlColumnText.SelectedValue = this.childListTextField;
                    ddlColumnValue.SelectedValue = this.childListValueField;
                    ddlColumnJoin.SelectedValue = this.childJoinField;
                }
                else
                {
                    //get current site if this is a new field
                    txtSiteURL.Text = SPContext.Current.Site.Url;
                }
            }
        }

        void ddlLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshLookups();
        }

        void btnLoadLists_Click(object sender, EventArgs e)
        {
            loadLists();
        }


        /// <summary>
        /// Handles the click event for the Load Lists button
        /// Retrieves the URL for the site and then populates the ddlLists dropdown list with the names of the available lists from that site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void loadLists()
        {
            SPSite site = null;
            SPWeb web = null;
            try
            {
                EnsureChildControls();
                ddlLists.Items.Clear();
                try
                {
                    site = new SPSite(this.txtSiteURL.Text);
                }
                catch
                {
                    site = SPContext.Current.Site;
                    this.txtSiteURL.Text = site.Url;
                }
                if (site != null)
                {
                    web = site.OpenWeb();
                    foreach (SPList list in web.Lists)
                    {
                        ListItem item = new ListItem(list.Title, list.DefaultViewUrl);
                        ddlLists.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                this.txtSiteURL.Text = ex.Message;
            }
            finally
            {
                site.Dispose();
                web.Dispose();
            }
        }

        /// <summary>
        /// Retrieve and present columns available on selected list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void refreshLookups()
        {
            SPSite site = null;
            SPWeb web = null;
            try
            {
                if (this.ddlLists.SelectedItem != null)
                {
                    EnsureChildControls();
                    ddlColumnText.Items.Clear();
                    ddlColumnValue.Items.Clear();
                    ddlColumnJoin.Items.Clear();
                    site = new SPSite(txtSiteURL.Text);
                    web = site.OpenWeb();
                    SPList list = CommonOperation.GetSPList(this.ddlLists.SelectedItem.Value, web);

                    foreach (SPField oneField in list.Fields)
                    {
                        ListItem item = new ListItem(oneField.Title, oneField.InternalName);
                        ddlColumnText.Items.Add(item);
                        ddlColumnValue.Items.Add(item);
                        ddlColumnJoin.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                this.txtSiteURL.Text = ex.Message;
            }
            finally
            {
                site.Dispose();
                web.Dispose();
            }
        }
    }
}
