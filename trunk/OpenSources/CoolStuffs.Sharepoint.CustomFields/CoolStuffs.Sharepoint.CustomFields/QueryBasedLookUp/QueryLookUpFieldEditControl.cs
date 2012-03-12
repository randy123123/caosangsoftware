using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.Field;
using System.Web.UI;
using CoolStuffs.Sharepoint.Constants;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SharePoint.Utilities;
using System.Reflection;

namespace CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.FieldControllers
{
    public class QueryLookUpFieldEditControl : System.Web.UI.UserControl, IFieldEditor
    {
        
        #region private/protected local members
        protected System.Web.UI.WebControls.TextBox txtSiteURL;
        protected System.Web.UI.WebControls.Button btnLoadLists;
        protected DropDownList ddlLookUpListName;
        protected DropDownList ddlLookUpDisplayColumnText;
        protected DropDownList ddlLookUpDisplayColumnValue;
        protected DropDownList ddlFilterOnColumn;
        protected DropDownList ddlFilterAssignmentType;
        protected System.Web.UI.WebControls.TextBox txtFilterValue;
        protected DropDownList ddlAndOrStatement;
        protected DropDownList ddlIsNull;
        protected System.Web.UI.WebControls.DropDownList ddlDynamicValues;
        protected System.Web.UI.WebControls.Label lblViewMyQuery;
        protected System.Web.UI.WebControls.Button btnBuildQuery;
        protected System.Web.UI.WebControls.Button btnClearQuery;
        private string siteUrl = default(string);
        private string lookUpListName = default(string);
        private string lookUpDisplayColumnText = default(string);
        private string lookUpDisplayColumnValue = default(string);
        private string actualQuery = default(string);
        private string sqlQuery = default(string);
        private string badQueryFlag = default(string);
        private bool invalidDateTimeValue = false;
        private bool webAppNotFound = false;
        string fieldValueTemplate = @"<FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value>";
        string fieldNullValueTemplate = @"<FieldRef Name='{0}'/>";
        string assignmentTemplate = "<{0}>{1}</{0}>";
        string actualAssignment;
        string columnCAMLDataType = "";
        string[] seperator = { "{0}" };
        StringSplitOptions stringSplitOptions;
        SPSite site;
        SPWeb web;
        SPList list;
        protected string[] operatorCollectionInnerCAML = { "Eq,=", "Geq,>=", "Gt,>", "Leq,<=", "Lt,<", "Neq,!=", "Contains,contains", "BeginsWith,begins_With"};
        protected string[] operatorCollectionUIDisplay = { "Equal to", "Greater then equal to", "Greater than", "Less then equal to", "Less than", "Not equal to", "Contains", "Begins With" };
        protected string queryTemplate = @"<FieldRef Name = '{0}'/><Value Type = '{1}'>{2}</Value>";
        #endregion

        #region IFieldEditor/System.Web.UI.UserControl overriden Methods

        /// <summary>
        /// IFieldEditor overriden member
        /// </summary>
        public bool DisplayAsNewSection
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// IFieldEditor Overriden member
        /// Called before the control loads
        /// </summary>
        /// <param name="field"></param>
        public void InitializeWithField(SPField field)
        {
            QueryLookUp queryLookUp = field as QueryLookUp;
            if (queryLookUp != null)
            {
                try
                {
                    this.site = new SPSite(queryLookUp.SiteUrl);
                    this.siteUrl = queryLookUp.SiteUrl;
                }
                catch
                {
                    this.siteUrl = SPContext.Current.Site.Url;
                }
                this.lookUpListName = queryLookUp.LookUpListName;
                this.lookUpDisplayColumnText = queryLookUp.LookUpDisplayColumnText;
                this.lookUpDisplayColumnValue = queryLookUp.LookUpDisplayColumnValue;
                if (string.IsNullOrEmpty((string)ViewState["camlQuery"]) && string.IsNullOrEmpty((string)ViewState["sqlQuery"]))
                {
                    this.actualQuery = queryLookUp.ActualQuery;
                    this.sqlQuery = queryLookUp.SQLQuery;
                }
                if (string.IsNullOrEmpty(this.actualQuery) && string.IsNullOrEmpty(this.sqlQuery))
                {
                    GetStateBags();
                }
                else
                {
                    SetStateBags(false);
                }

                this.badQueryFlag = queryLookUp.BadQueryFlag;
                try
                {
                    EnsureChildControls();
                }
                catch(Exception ex)
                {
                    //Alert.Show(ex.Message);
                    this.txtSiteURL.Text = this.siteUrl;
                    webAppNotFound = true;
                }

                if (string.IsNullOrEmpty(this.txtSiteURL.Text) && !string.IsNullOrEmpty(this.siteUrl))
                {
                    this.txtSiteURL.Text = this.siteUrl;
                    LoadLookUpListsInDropDown();
                    this.ddlLookUpListName.Items.FindByText(this.lookUpListName).Selected = true;
                    LoadListColumnsInDropDowns();
                    LoadStaticDropDowns();
                    this.ddlLookUpDisplayColumnText.Items.FindByText(this.lookUpDisplayColumnText).Selected = true;
                    this.ddlLookUpDisplayColumnValue.Items.FindByText(this.lookUpDisplayColumnValue).Selected = true;
                    this.lblViewMyQuery.ToolTip = this.sqlQuery;
                }
            }
            else
            {
                this.lblViewMyQuery.ToolTip = "No Query Build Yet.select the filter column and Type in its value. then click \"Build Query\"";
            }

            
        }

        /// <summary>
        /// IFieldEditor Overriden member
        /// Called when the save button on the create Edit column page
        /// is clicked for a column
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isNewField"></param>
        public void OnSaveChange(SPField field, bool isNewField)
        {
            this.siteUrl = this.txtSiteURL.Text;
            this.lookUpListName = this.ddlLookUpListName.SelectedValue;
            this.lookUpDisplayColumnText = this.ddlLookUpDisplayColumnText.SelectedValue;
            this.lookUpDisplayColumnValue = this.ddlLookUpDisplayColumnValue.SelectedValue;
            stringSplitOptions = new StringSplitOptions();
            if (this.actualQuery.Split(seperator, stringSplitOptions).Length > 1)
            {
                //Alert.Show("Check for the correctness of the query!!");
                this.badQueryFlag = "1";
            }
            else
            {
                this.badQueryFlag = "0";
            }
            QueryLookUp ParentField = field as QueryLookUp;

            if (isNewField)
            {
                ParentField.UpdateMyCustomProperty("SiteUrl", this.siteUrl);
                ParentField.UpdateMyCustomProperty("LookUpListName", this.lookUpListName);
                ParentField.UpdateMyCustomProperty("LookUpDisplayColumnText", this.lookUpDisplayColumnText);
                ParentField.UpdateMyCustomProperty("LookUpDisplayColumnValue", this.lookUpDisplayColumnValue);
                ParentField.UpdateMyCustomProperty("ActualQuery", this.actualQuery);
                ParentField.UpdateMyCustomProperty("SQLQuery", this.sqlQuery);
                ParentField.UpdateMyCustomProperty("BadQueryFlag", this.badQueryFlag);
            }

            else
            {
                ParentField.SiteUrl = this.siteUrl;
                ParentField.LookUpListName = this.lookUpListName;
                ParentField.LookUpDisplayColumnValue = this.lookUpDisplayColumnValue;
                ParentField.LookUpDisplayColumnText = this.lookUpDisplayColumnText;
                ParentField.ActualQuery = this.actualQuery;
                ParentField.SQLQuery = this.sqlQuery;
                ParentField.BadQueryFlag = this.badQueryFlag;
            }
        }

        /// <summary>
        /// overriden method to create all the child controls
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.btnLoadLists.Click += new EventHandler(btnLoadLists_Click);
            this.ddlLookUpListName.AutoPostBack = true;
            this.ddlLookUpListName.EnableViewState = true;
            this.ddlAndOrStatement.AutoPostBack = true;
            this.ddlIsNull.AutoPostBack = true;
            this.ddlLookUpListName.SelectedIndexChanged += new EventHandler(ddlLookUpListName_SelectedIndexChanged);
            this.btnBuildQuery.Click += new EventHandler(btnBuildQuery_Click);
            this.btnClearQuery.Click += new EventHandler(btnClearQuery_Click);
            this.ddlAndOrStatement.SelectedIndexChanged += new EventHandler(ddlAndOrStatement_SelectedIndexChanged);
            this.ddlIsNull.SelectedIndexChanged += new EventHandler(ddlIsNull_SelectedIndexChanged);
            this.ddlDynamicValues.SelectedIndexChanged += new EventHandler(ddlDynamicValues_SelectedIndexChanged);
            this.ddlDynamicValues.AutoPostBack = true;
            GetStateBags();
            if (!string.IsNullOrEmpty(this.sqlQuery))
            {
                lblViewMyQuery.ToolTip = this.sqlQuery;
            }
            else
            {
                lblViewMyQuery.ToolTip = "No Query Build Yet.select the filter column and Type in its value. then click \"Build Query\"";
            }
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.siteUrl))
                {
                    this.txtSiteURL.Text = this.siteUrl;
                    this.LoadLookUpListsInDropDown();
                    this.ddlLookUpListName.SelectedValue = this.lookUpListName;
                    this.LoadListColumnsInDropDowns();
                    this.LoadStaticDropDowns();
                    this.ddlLookUpDisplayColumnText.SelectedValue = this.lookUpDisplayColumnText;
                    this.ddlLookUpDisplayColumnValue.SelectedValue = this.lookUpDisplayColumnValue;
                    if (string.Equals(this.badQueryFlag, "1"))
                    {
                        //Alert.Show("Check for the correctness of query!!");
                    }
                    this.lblViewMyQuery.ToolTip = this.sqlQuery;
                }
                else
                {
                    this.LoadStaticDropDowns();
                    this.lblViewMyQuery.ToolTip = "No Query Build Yet.Load lookup list,select the filter column, and Type in its value. then click \"Build Query\"";
                    this.txtSiteURL.Text = SPContext.Current.Site.Url;
                }

            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// IsNull/IsNotNull dropDown event handler,
        /// On  Index change, builds a block of query to check for value = null or value != null
        /// based on the selected Value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ddlIsNull_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetStateBags();
            //this.columnCAMLDataType = this.GetValueType(this.ddlFilterOnColumn.SelectedItem.Text);
            this.actualAssignment = this.GetAssignmentStatement(this.fieldNullValueTemplate, this.ddlFilterOnColumn.SelectedValue, "", "", this.ddlIsNull.SelectedValue);
            stringSplitOptions = new StringSplitOptions();
            if ((this.actualQuery.Split(seperator, stringSplitOptions).Length == 2) || (string.Equals(this.sqlQuery.ToLower(), "where")))
            {
                this.sqlQuery += " " + this.ddlFilterOnColumn.SelectedValue + " " + this.ddlFilterAssignmentType.SelectedValue.Split(',')[1] + " " + ddlIsNull.SelectedValue;
            }

            this.actualQuery = string.Format(this.actualQuery, this.actualAssignment);
            this.SetStateBags(false);
            this.lblViewMyQuery.ToolTip = this.sqlQuery;
            this.ddlIsNull.SelectedIndex = 0;
        }

        /// <summary>
        /// Cleans the Query, cleans the ViewState, and resets the 
        /// local query variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClearQuery_Click(object sender, EventArgs e)
        {
            ClearState();
        }

        /// <summary>
        /// AndOrStatement event handler, On Click
        /// builds a block of CAML and, Or statement
        /// depending on the selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ddlAndOrStatement_SelectedIndexChanged(object sender, EventArgs e)
        {
            stringSplitOptions = new StringSplitOptions();
            GetStateBags();
            if (this.actualQuery.Split(seperator, stringSplitOptions).Length == 1)
            {
                this.actualQuery = string.Format(this.assignmentTemplate, this.ddlAndOrStatement.SelectedValue, this.actualQuery + "{0}");
                this.sqlQuery += " " + this.ddlAndOrStatement.SelectedValue;

            }
            else
            {
                //Alert.Show("You have either selected And/Or Consequetively or No previous query before And/Or present!!");
            }

            SetStateBags(false);
            this.lblViewMyQuery.ToolTip = this.sqlQuery;
            this.ddlAndOrStatement.SelectedIndex = 0;
        }

        /// <summary>
        /// Build Query Button Click event handler
        /// On Click,Build a Block of CAML query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBuildQuery_Click(object sender, EventArgs e)
        {
            this.GetStateBags();
            this.columnCAMLDataType = GetValueType(ddlFilterOnColumn.SelectedItem.Text);
            ConstantLibrary constantLibrary = new ConstantLibrary();
            if (!string.IsNullOrEmpty(this.txtFilterValue.Text))
            {

                this.actualAssignment = GetAssignmentStatement(this.fieldValueTemplate, this.ddlFilterOnColumn.SelectedValue, this.columnCAMLDataType, this.CheckForDateTimeValue(this.txtFilterValue.Text.Replace("{",constantLibrary.OpeningCurlyBraces).Replace("}",constantLibrary.ClosingCurlyBraces)), this.ddlFilterAssignmentType.SelectedValue.Split(',')[0]);
            }
            else
            {
                this.actualAssignment = GetAssignmentStatement(this.fieldNullValueTemplate, this.ddlFilterOnColumn.SelectedValue, this.columnCAMLDataType, "", "IsNull");
            }

            stringSplitOptions = new StringSplitOptions();
            if ((this.actualQuery.Split(seperator, stringSplitOptions).Length == 2) || (string.Equals(this.sqlQuery.ToLower(), "where")))
            {
                if (!string.IsNullOrEmpty(this.txtFilterValue.Text))
                {
                    if (!invalidDateTimeValue)
                    {
                        this.sqlQuery += " " + this.ddlFilterOnColumn.SelectedValue + " " + this.ddlFilterAssignmentType.SelectedValue.Split(',')[1] + " " + this.txtFilterValue.Text;
                    }
                    else
                    {
                        this.sqlQuery += " " + this.ddlFilterOnColumn.SelectedValue + " " + this.ddlFilterAssignmentType.SelectedValue.Split(',')[1] + " 01/01/2001 12:00:00AM";
                        this.txtFilterValue.Text = "01/01/2001 12:00:00AM";
                    }

                }
                else
                {
                    this.sqlQuery += " " + this.ddlFilterOnColumn.SelectedValue + " " + this.ddlFilterAssignmentType.SelectedValue.Split(',')[1] + " null";
                }

            }

            this.actualQuery = string.Format(this.actualQuery, this.actualAssignment);
            SetStateBags(false);
            lblViewMyQuery.ToolTip = this.sqlQuery;
        }

        /// <summary>
        /// Index Changed Event of Dynamic values dropdown
        /// placess a unique identifier value to be replaced
        /// by current value during the Field Load in Edit Item
        /// page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ddlDynamicValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetStateBags();
            this.columnCAMLDataType = this.GetValueType(this.ddlFilterOnColumn.SelectedItem.Text);
            this.actualAssignment = this.GetAssignmentStatement(this.fieldValueTemplate, this.ddlFilterOnColumn.SelectedValue, this.columnCAMLDataType, this.ddlDynamicValues.SelectedValue, this.ddlFilterAssignmentType.SelectedValue.Split(',')[0]);
            this.stringSplitOptions = new StringSplitOptions();
            if ((this.actualQuery.Split(this.seperator, this.stringSplitOptions).Length == 2) || (string.Equals(this.sqlQuery.ToLower(), "where")))
            {
                this.sqlQuery += " " + this.ddlFilterOnColumn.SelectedValue + " " + this.ddlFilterAssignmentType.SelectedValue.Split(',')[1] + " [" + this.ddlDynamicValues.SelectedItem.Text + "]";
            }

            this.actualQuery = string.Format(this.actualQuery, this.actualAssignment);
            this.SetStateBags(false);
            this.lblViewMyQuery.ToolTip = this.sqlQuery;
            this.ddlDynamicValues.SelectedIndex = 0;
        }

        /// <summary>
        /// Index changed event handler of the lookUp List DropDown,
        /// Loads all the columns of the selected List in their respective drop downs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ddlLookUpListName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListColumnsInDropDowns();
        }

        /// <summary>
        /// Load List button click event handler. On click, Loads
        /// all the Lists in a drop down of the Site entered in textbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLoadLists_Click(object sender, EventArgs e)
        {
            LoadLookUpListsInDropDown();
        }

        #endregion

        #region custom local methods

        /// <summary>
        /// clears all the ViewStates and resets the members : sqlQuery and actualQuery
        /// </summary>
        private void ClearState()
        {
            this.SetStateBags(true);
            this.lblViewMyQuery.ToolTip = "First Build a query";
            this.GetStateBags();
            this.SetStateBags(false);
        }

        /// <summary>
        /// Checks for valid input value, if it happens to be DateTime,
        /// converts it to ISO8601 format to be used in CAML query.
        /// If entered value is incorrect, sets to a default value
        /// </summary>
        /// <param name="filterValue">the value coming from txtFilterValue TextBox</param>
        /// <returns>ISO8601 DateTime in string</returns>
        private string CheckForDateTimeValue(string filterValue)
        {
            if (string.Equals(this.columnCAMLDataType.ToLower(), "datetime"))
            {
                try
                {
                    return SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(this.txtFilterValue.Text));
                }
                catch
                {
                    //Alert.Show("Please enter a valid datetime value!. Reseting the value...");
                    this.invalidDateTimeValue = true;
                    return "2001-01-01T00:00:000Z";
                }
            }
            else
            {
                return filterValue;
            }
            
        }

        /// <summary>
        /// Gets the CAML datatype of the Column to Filter on
        /// from the system datatime of the column.
        /// </summary>
        /// <param name="filterColumn">InnerName of the sharepoint column to filter on</param>
        /// <returns>datatype in string</returns>
        private string GetValueType(string filterColumn)
        {
            site = new SPSite(this.txtSiteURL.Text);
            web = site.OpenWeb();
            list = CommonOperation.GetSPList(this.ddlLookUpListName.SelectedValue, web);
 	        SPField field = list.Fields[filterColumn];
            try
            {
                return GetCAMLDataTypeFromSystemDataType(field.FieldValueType.Name);
            }
            catch
            {
                return "Text";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnSystemDataType"></param>
        /// <returns></returns>
        private string GetCAMLDataTypeFromSystemDataType(string columnSystemDataType)
        {
            switch(columnSystemDataType)
            {
                case "String":
                    return "Text";

                case "SPFieldUserValue":
                    return "User";

                case "Double":
                    return "Integer";

                case "Boolean":
                    return "Boolean";
                    
                case "DateTime":
                    return "DateTime";

               default:
                    return "Text";

            }
        }

        /// <summary>
        /// Get the Assignment statement formed by the FilterColumn, its assignment type,
        /// and the filter value
        /// </summary>
        /// <param name="TemplateType">Template to be used to form a CAML block : fieldValueTemplate or fieldNullValueTemplate</param>
        /// <param name="filterOnColumn">Column Name in string</param>
        /// <param name="dataType">Column's CAML equivalent datatype : Text, Integer, User, Boolean, etc.</param>
        /// <param name="filterValue">Value to Filter the column on </param>
        /// <param name="assignmentType"> Type of operation performed: Equal,Not Equal,greater than equal,less than equal,less than,greater than,And,Or,Contains,BeginsWith</param>
        /// <returns>string of a CAML statement</returns>
        private string GetAssignmentStatement(string TemplateType, string filterOnColumn, string dataType, string filterValue, string assignmentType)
        {
            return string.Format(this.assignmentTemplate, assignmentType, string.Format(TemplateType, filterOnColumn, dataType, filterValue));
        }

        /// <summary>
        /// Clears all the column Drop Downs
        /// </summary>
        private void ClearAllColumnDropDowns()
        {
            ddlFilterOnColumn.Items.Clear();
            ddlLookUpDisplayColumnText.Items.Clear();
            ddlLookUpDisplayColumnValue.Items.Clear();
        }

        /// <summary>
        /// Load static values in respective drop downs
        /// </summary>
        private void LoadStaticDropDowns()
        {
            this.ddlFilterAssignmentType.Items.Clear();
            this.ddlAndOrStatement.Items.Clear();
            for (int i = 0; i < this.operatorCollectionInnerCAML.Length; i++)
            {
                ListItem assignmentItem = new ListItem(this.operatorCollectionUIDisplay[i], this.operatorCollectionInnerCAML[i]);
                this.ddlFilterAssignmentType.Items.Add(assignmentItem);
            }

            this.ddlAndOrStatement.Items.Add("---");
            this.ddlAndOrStatement.Items.Add("And");
            this.ddlAndOrStatement.Items.Add("Or");
            this.ddlAndOrStatement.SelectedIndex = 0;
            this.ddlIsNull.Items.Add("---------");
            this.ddlIsNull.Items.Add("IsNull");
            this.ddlIsNull.Items.Add("IsNotNull");
            this.ddlIsNull.SelectedIndex = 0;
            this.ddlDynamicValues.Items.Add("----");
            ConstantLibrary constantLibrary = new ConstantLibrary();
            PropertyInfo[] properties =  constantLibrary.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyValue = property.GetValue(constantLibrary, null).ToString();

                if (!(propertyValue.StartsWith("_")) && (!propertyValue.EndsWith("_")))
                {
                    ListItem item = new ListItem(property.Name, propertyValue);
                    this.ddlDynamicValues.Items.Add(item);
                }
            }
            this.ddlDynamicValues.SelectedIndex = 0;
        }

        /// <summary>
        /// Load Columns of the selected List in DropDown
        /// </summary>
        private void LoadListColumnsInDropDowns()
        {
            site = null;
            web = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (this.ddlLookUpListName.SelectedItem != null)
                    {
                        //try
                        //{
                        //    EnsureChildControls();
                        //    webAppNotFound = true;
                        //}
                        //catch (Exception ex)
                        //{
                        //    this.txtSiteURL.Text = ex.Message;
                        //}
                        ddlLookUpDisplayColumnText.Items.Clear();
                        ddlLookUpDisplayColumnValue.Items.Clear();
                        ddlFilterOnColumn.Items.Clear();
                        //if (!webAppNotFound)
                        //{
                            site = new SPSite(txtSiteURL.Text);
                        //}
                        //else
                        //{
                        //    site = SPContext.Current.Site;
                        //}
                        web = site.OpenWeb();
                        web.AllowUnsafeUpdates = true;
                        web.Update();
                        SPList userInfoList = web.SiteUserInfoList;
                        if (!userInfoList.AllowEveryoneViewItems)
                        {
                            userInfoList.IrmEnabled = false;
                            userInfoList.AllowEveryoneViewItems = true;
                            userInfoList.Update();

                        }
                        
                            this.list = CommonOperation.GetSPList(ddlLookUpListName.SelectedValue, web);
                        
                        foreach (SPField field in this.list.Fields)
                        {
                            ListItem listItem = new ListItem(field.Title, field.InternalName);
                            ddlLookUpDisplayColumnText.Items.Add(listItem);
                            ddlLookUpDisplayColumnValue.Items.Add(listItem);
                            ddlFilterOnColumn.Items.Add(listItem);
                        }
                    }
                });
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
        /// Load Lists of the Site entered in textBox in a DropDown
        /// </summary>
        private void LoadLookUpListsInDropDown()
        {
            
            site = null;
            web = null;
            string exceptionMessage = "";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    try
                    {
                        EnsureChildControls();
                    }
                    catch (Exception ex)
                    {
                        this.txtSiteURL.Text = ex.Message;
                        webAppNotFound = true;
                    }

                    this.ddlLookUpListName.Items.Clear();
                    if (!webAppNotFound)
                    {
                        this.site = new SPSite(this.txtSiteURL.Text);
                    }
                    else
                    {
                        this.site = SPContext.Current.Site;
                    }

                    if (this.site != null)
                    {
                        this.web = site.OpenWeb();
                        foreach (SPList list in web.Lists)
                        {
                            if (!list.Hidden)
                            {
                                ListItem item = new ListItem(list.Title, list.DefaultViewUrl);
                                this.ddlLookUpListName.Items.Add(item);
                            }
                        }
                        this.ddlLookUpListName.Items.Add("User Information List");
                    }
                    else
                    {
                        MessageBox.Show("make sure URL: " + this.txtSiteURL.Text + "is correct or you have appropriate permissions to access the Site," + exceptionMessage, "tahoma", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    }
                });
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
        /// Set ViewState Values with either those in local query variables,
        /// or clear ViewState
        /// </summary>
        /// <param name="isClearState">flag whether to clear ViewState or Not</param>
        private void SetStateBags(bool isClearState)
        {
            if (isClearState)
            {
                ViewState["sqlQuery"] = null;
                ViewState["camlQuery"] = null;
            }
            else
            {
                ViewState["sqlQuery"] = this.sqlQuery;
                ViewState["camlQuery"] = this.actualQuery;
            }

        }

        /// <summary>
        /// Set local query variables with the values in ViewState
        /// </summary>
        private void GetStateBags()
        {
            if ((ViewState["camlQuery"] != null) && (ViewState["sqlQuery"] != null))
            {
                this.actualQuery = (string)ViewState["camlQuery"];
                this.sqlQuery = (string)ViewState["sqlQuery"];
            }
            else
            {
                this.actualQuery = "{0}";
                this.sqlQuery = "Where";
            }
        }

        #endregion
    }
}
