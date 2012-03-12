using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Data;
using CoolStuffs.Sharepoint.Constants;

namespace CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers
{
    public class ParentDropDownListFieldControl : BaseFieldControl
    {
        protected DropDownList ParentDropDownList;
        SPSite site;
        SPList list;
        protected DropDownList ChildDropDownList;
        protected bool listNotFound;
        protected string parentSiteUrl;
        protected string parentListName;
        protected string parentListTextField;
        protected string parentListValueField;

      
        void ParentDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChildDropDownListFieldControl child = (ChildDropDownListFieldControl) FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent; 
            child.SetDataSource(ParentDropDownList.SelectedValue);
        }

        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;
            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        protected override string DefaultTemplateName
        {
            get
            {
                return "ParentDropDownListFieldControl";
            }
        }

        public override object Value
        {
            get
            {
                EnsureChildControls();
                return ParentDropDownList.SelectedValue;
            }

            set
            {
                EnsureChildControls();
                ParentDropDownList.SelectedValue = (string)this.ItemFieldValue;
                ChildDropDownListFieldControl child = (ChildDropDownListFieldControl)FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent;
                child.SetDataSource(ParentDropDownList.SelectedValue);
            }
        }

        public override void Focus()
        {
            EnsureChildControls();
            ParentDropDownList.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

            ParentDropDownList = (DropDownList)TemplateContainer.FindControl("ParentDropDownList");

            if (ParentDropDownList == null)
                throw new ArgumentException("ParentDropDownList is null. Corrupted CountryListFieldControl.ascx file.");

            ParentDropDownList.TabIndex = TabIndex;
            ParentDropDownList.CssClass = CssClass;
            ParentDropDownList.ToolTip = Field.Title + " Parent";

            parentSiteUrl = Field.GetCustomProperty("ParentSiteUrl").ToString();
            parentListName = Field.GetCustomProperty("ParentListName").ToString();
            parentListTextField = Field.GetCustomProperty("ParentListTextField").ToString();
            parentListValueField = Field.GetCustomProperty("ParentListValueField").ToString();
            
            try
            {
                site = new SPSite(parentSiteUrl);
            }
            catch
            {
                //Alert.Show("Site:" + this.parentSiteUrl + " either cannot be found or you dont have permission to access it setting it to current Site...");
                site = SPContext.Current.Site;
            }

            try
            {
                SPWeb currentWeb = site.OpenWeb();
                list = CommonOperation.GetSPList(parentListName, currentWeb);
            }
            catch
            {
                listNotFound = true;
            }

            if (!listNotFound)
            {
                // populate it with the values from the central master page list.
                DataView dv = new DataView(list.Items.GetDataTable());
                if (!String.Equals(this.parentListTextField, this.parentListValueField))
                {
                    String[] columnCollection = { this.parentListTextField, this.parentListValueField };
                    this.ParentDropDownList.DataSource = dv.ToTable(true, columnCollection);
                }
                else
                {
                    this.ParentDropDownList.DataSource = dv.ToTable(true, parentListTextField);
                }
                
                this.ParentDropDownList.DataTextField = parentListTextField;
                this.ParentDropDownList.DataValueField = parentListValueField;
                this.ParentDropDownList.DataBind();

                this.ParentDropDownList.AutoPostBack = true;

                this.ParentDropDownList.SelectedIndexChanged += new EventHandler(ParentDropDownList_SelectedIndexChanged);

                ChildDropDownListFieldControl child = (ChildDropDownListFieldControl)FindControlRecursive(this.Page, "ChildDropDownList").Parent.Parent;
                child.SetDataSource(ParentDropDownList.SelectedValue);
            }
        }


    }

}