using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Search.Query;
using System.Security.Principal;
using System.Security.Permissions;
using CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.Field;
using Microsoft.SharePoint;
using System.Data;
using CoolStuffs.Sharepoint.Constants;

namespace CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers
{
    public class ChildDropDownListFieldControl : BaseFieldControl
    {
        protected DropDownList ChildDropDownList;
        SPSite site;
        SPList list;
        bool listNotFound;
        protected string childSiteUrl;
        protected string childListName;
        protected string childListTextField;
        protected string childListValueField;
        protected string childJoinField;
      
        public void SetDataSource(string parentSelectedValue)
        {
            //impersonate SharePoint App Pool Account by passing a null pointer as the id
            WindowsIdentity CurrentIdentity = WindowsIdentity.GetCurrent();
            WindowsImpersonationContext ImpersonationContext = WindowsIdentity.Impersonate(IntPtr.Zero);
            WindowsIdentity.GetCurrent().Impersonate();

            this.ChildDropDownList.Items.Clear();

            childSiteUrl = Field.GetCustomProperty("ChildSiteUrl").ToString();
            childListName = Field.GetCustomProperty("ChildListName").ToString();
            childListTextField = Field.GetCustomProperty("ChildListTextField").ToString();
            childListValueField = Field.GetCustomProperty("ChildListValueField").ToString();
            childJoinField = Field.GetCustomProperty("ChildJoinField").ToString();

            try
            {
                site = new SPSite(childSiteUrl);
            }
            catch
            {
                //Alert.Show("Site:" + this.childSiteUrl + " either cannot be found or you dont have permission to access it setting it to current Site...");
                site = SPContext.Current.Site;
            }

            try
            {
                SPWeb currentWeb = site.OpenWeb();
                list = CommonOperation.GetSPList(childListName, currentWeb);
            }
            catch
            {
                listNotFound = true;
            }

            if (!listNotFound)
            {
                string caml = @"<Where>
                                <Eq>
                                    <FieldRef Name='{0}'/><Value Type='Text'>{1}</Value>
                                </Eq></Where>";

                SPQuery query = new SPQuery();

                query.Query = string.Format(caml, childJoinField, parentSelectedValue);

                SPListItemCollection results = list.GetItems(query);
                DataTable  dt = results.GetDataTable();
                if (dt != null)
                {
                    DataView dv = new DataView(dt);
                    //if the title field has been renamed it doesn't seem to pick up the changed name 
                    //so need to get the internal name for the field
                    string childListTextFieldInternal = childListTextField;
                    string childListValueFieldInternal = childListValueField;
                    
                    if (!String.Equals(childListTextFieldInternal, childListValueFieldInternal))
                    {
                        String[] columnCollection = { childListTextFieldInternal, childListValueFieldInternal };
                        this.ChildDropDownList.DataSource = dv.ToTable(true, columnCollection);
                    }
                    else
                    {
                        this.ChildDropDownList.DataSource = dv.ToTable(true, childListTextFieldInternal);
                    }
                    this.ChildDropDownList.DataTextField = childListTextFieldInternal;
                    this.ChildDropDownList.DataValueField = childListValueFieldInternal;
                    this.ChildDropDownList.DataBind();

                }

                ImpersonationContext = WindowsIdentity.Impersonate(CurrentIdentity.Token);
                WindowsIdentity.GetCurrent().Impersonate();
            }
        }


        protected override string DefaultTemplateName
        {
            get
            {
                return "ChildDropDownListFieldControl";
            }
        }

        public override object Value
        {
            get
            {
                EnsureChildControls();
                return ChildDropDownList.SelectedValue;
            }

            set
            {
                EnsureChildControls();
                ChildDropDownList.SelectedValue = (string)this.ItemFieldValue;
            }
        }


        public override void Focus()
        {
            EnsureChildControls();
            ChildDropDownList.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

          
            ChildDropDownList = (DropDownList)TemplateContainer.FindControl("ChildDropDownList");

            if (ChildDropDownList == null)
                throw new ArgumentException("ChildDropDownList is null. Corrupted CountryListFieldControl.ascx file.");

            ChildDropDownList.TabIndex = TabIndex;
            ChildDropDownList.CssClass = CssClass;
            ChildDropDownList.ToolTip = Field.Title + " Parent";
        }
    }
}