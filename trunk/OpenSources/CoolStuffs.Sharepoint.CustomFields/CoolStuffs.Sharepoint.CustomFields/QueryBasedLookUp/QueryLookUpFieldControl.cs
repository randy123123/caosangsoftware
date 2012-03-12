using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Security.Principal;
using Microsoft.SharePoint.Utilities;
using CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.Field;
using System.Data;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Web;
using CoolStuffs.Sharepoint.Constants;
using System.Web.Security;
using CoolStuffs.Sharepoint.CustomFields;
using System.Text.RegularExpressions;

namespace CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.FieldControllers
{
    public class QueryLookUpFieldControl : BaseFieldControl
    {
        #region private/protected variable
        protected DropDownList queryLookUpDropDown;
        protected string siteUrl;
        protected string lookUpListName;
        protected string lookUpDisplayColumnText;
        protected string lookUpDisplayColumnValue;
        protected string actualQuery;
        protected string badQueryFlag = "0";
        protected string exceptionStatement = @"Cannot populate column values in drop down. The View ""All Items"" doesn't exist";
        private SPSite siteCollection;
        private SPWeb site;
        private DataTable dt;
        private DataView dv;
        private SPList list;
        private SPQuery query;
        //private string viewName;
        string whereTemplate = "<Where>{0}</Where>";
        bool listNotFound = false;
        #endregion

        #region Overriden BaseFieldControl Methods

        /// <summary>
        /// Called before the control is rendered
        /// </summary>
        protected override void CreateChildControls()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                //impersonate SharePoint App Pool Account by passing a null pointer as the id
                WindowsIdentity CurrentIdentity = WindowsIdentity.GetCurrent();
                WindowsImpersonationContext ImpersonationContext = WindowsIdentity.Impersonate(IntPtr.Zero);
                WindowsIdentity.GetCurrent().Impersonate();

                if (Field == null) return;
                base.CreateChildControls();

                if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                    return;

                queryLookUpDropDown = (DropDownList)TemplateContainer.FindControl("QueryLookUpDropDown");

                if (queryLookUpDropDown == null)
                    throw new ArgumentException("queryLookUpDropDown is null. Corrupted QueryLookUpFieldControl.ascx file.");

                queryLookUpDropDown.TabIndex = TabIndex;
                queryLookUpDropDown.ToolTip = Field.Title;
                queryLookUpDropDown.CssClass = CssClass;
                this.siteUrl = Field.GetCustomProperty("SiteUrl").ToString();
                this.lookUpListName = Field.GetCustomProperty("LookUpListName").ToString();
                this.lookUpDisplayColumnText = Field.GetCustomProperty("LookUpDisplayColumnText").ToString();
                this.lookUpDisplayColumnValue = Field.GetCustomProperty("LookUpDisplayColumnValue").ToString();
                this.actualQuery = Field.GetCustomProperty("ActualQuery").ToString();
                this.badQueryFlag = Field.GetCustomProperty("BadQueryFlag").ToString();

                try
                {
                    siteCollection = new SPSite(this.siteUrl);
                }
                catch
                {
                    //Alert.Show("Site:" + this.siteUrl + " either cannot be found or you dont have permission to access it setting it to current Site...");
                    siteCollection = SPContext.Current.Site;
                }
                site = siteCollection.OpenWeb();
                site.AllowUnsafeUpdates = true;
                site.Update();
                SPList userInfoList = site.SiteUserInfoList;
                if (!userInfoList.AllowEveryoneViewItems)
                {
                    userInfoList.IrmEnabled = false;
                    userInfoList.AllowEveryoneViewItems = true;
                    userInfoList.Update();
                }

                try
                {
                    if (string.Equals(this.lookUpListName.ToLower(), "user information list"))
                    {
                        list = userInfoList;
                    }
                    else
                    {
                        list = CommonOperation.GetSPList(this.lookUpListName, site);
                    }
                }
                catch
                {
                    //Alert.Show("List:" + this.lookUpListName + " either cannot be found or you dont have permission to access .Check with administrator");
                    listNotFound = true;
                }
                if (!listNotFound)
                {
                    query = new SPQuery();
                    ConstantLibrary constantLibrary = new ConstantLibrary();
                    MembershipProvider membership = Membership.Provider;
                    string currentUserLoginName;

                    if (string.Equals(membership.Name.ToLower(), "odbcprovider"))
                    {
                        currentUserLoginName = membership.Name + ":" + HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        currentUserLoginName = HttpContext.Current.User.Identity.Name;
                    }
                    //this.actualQuery
                    if (this.actualQuery.Contains("["))
                    {
                        IEnumerable<string> fiels = GetSubStrings(this.actualQuery, "[", "]");
                        foreach (string fieldName in fiels)
                        {
                            try
                            {
                                if (fieldName.Contains(","))
                                {
                                    string[] fieldNameSplit = fieldName.Split(',');
                                    if (Item.ID == 0)
                                        this.actualQuery = this.actualQuery.Replace(String.Format("[{0}]", fieldName), fieldNameSplit[1]);
                                    else if (Item.Fields.ContainsField(fieldNameSplit[0]))
                                        this.actualQuery = this.actualQuery.Replace(String.Format("[{0}]", fieldName), (string)Item[fieldNameSplit[0]]);
                                }
                                else if (Item.Fields.ContainsField(fieldName))
                                    this.actualQuery = this.actualQuery.Replace(String.Format("[{0}]", fieldName), (string)Item[fieldName]);
                            }
                            catch { }
                        }
                    }
                    this.actualQuery = this.actualQuery.Replace(constantLibrary.CurrentUser, site.AllUsers[currentUserLoginName].Name).Replace(constantLibrary.CurrentDateTime, SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Today)).Replace(constantLibrary.OpeningCurlyBraces, "{").Replace(constantLibrary.ClosingCurlyBraces, "}");
                    this.actualQuery = string.Format(this.whereTemplate, this.actualQuery);
                    query.Query = this.actualQuery;
                    if (string.Equals(this.badQueryFlag, "0"))
                    {
                        dt = list.GetItems(query).GetDataTable();
                    }
                    else
                    {
                        try
                        {
                            dt = list.Items.GetDataTable();
                        }
                        catch
                        {
                            //Alert.Show(this.exceptionStatement);
                        }
                    }

                    dv = new DataView(dt);
                    if (!string.Equals(this.lookUpDisplayColumnText, this.lookUpDisplayColumnValue))
                    {
                        String[] columnCollection = { this.lookUpDisplayColumnText, this.lookUpDisplayColumnValue };
                        if (dt != null)
                        {
                            queryLookUpDropDown.DataSource = dv.ToTable(true, columnCollection);
                        }
                    }
                    else
                    {
                        if (dt != null)
                        {
                            queryLookUpDropDown.DataSource = dv.ToTable(true, this.lookUpDisplayColumnValue);
                        }
                    }
                    if (dt != null)
                    {
                        queryLookUpDropDown.DataTextField = this.lookUpDisplayColumnText;
                        queryLookUpDropDown.DataValueField = this.lookUpDisplayColumnValue;
                        queryLookUpDropDown.DataBind();
                    }
                }
                ImpersonationContext = WindowsIdentity.Impersonate(CurrentIdentity.Token);
                WindowsIdentity.GetCurrent().Impersonate();
            });
        }
        private IEnumerable<string> GetSubStrings(string input, string start, string end)
        {
            Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            MatchCollection matches = r.Matches(input);
            foreach (Match match in matches)
                yield return match.Groups[1].Value;
        }
        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }

        /// <summary>
        /// Checks for filter value, returns if present
        /// </summary>
        /// <param name="filterValue">the actual value</param>
        /// <returns>returned value</returns>
        private string CheckFilterValue(string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                return filterValue;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Overriden Get property to find control on the current page by ID
        /// </summary>
        protected override string DefaultTemplateName
        {
            get
            {
                return "QueryLookUpFieldControl";
            }
        }

        /// <summary>
        /// Overriden Get/Set property to get or set the value of the 
        /// actual column value of a listitem
        /// </summary>
        public override object Value
        {
            get
            {
                EnsureChildControls();
                return queryLookUpDropDown.SelectedValue;
            }
            set
            {
                EnsureChildControls();
                queryLookUpDropDown.SelectedValue = this.ItemFieldValue.ToString();
            }
        }

        /// <summary>
        /// Sets Input Focus to this control
        /// </summary>
        public override void Focus()
        {
            EnsureChildControls();
            queryLookUpDropDown.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static System.Web.UI.Control FindControlRecursive(System.Web.UI.Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;
            foreach (System.Web.UI.Control Ctl in Root.Controls)
            {
                System.Web.UI.Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        #endregion

    }
}
