using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Globalization;

namespace Officience.IBNF.CurrencyField
{
    public partial class CurrencyWithSymbolFieldEditor : UserControl, IFieldEditor
    {
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.MaintainScrollPositionOnPostBack = true; //required to keep section within view
            if (!this.IsViewStateEnabled) { this.EnableViewState = true; }
        }
        #endregion

        #region IFieldEditor Members

        public bool DisplayAsNewSection { get { return true; } }

        #region InitializeWithField method
        public void InitializeWithField(SPField field)
        {
            EnsureChildControls();
            CurrencyWithSymbolField _f = null;
            try { _f = field as CurrencyWithSymbolField; }
            catch { }

            if (_f != null)
            {
                // this bit only happens when field is not null
                if (!IsPostBack)
                {
                    TargetWebId = _f.SiteName;
                    TargetListId = _f.CurrencyListName;
                    TargetColumnId = _f.CurrencySymbolColumn;
                    TargetNumberFormat = _f.NumberFormat;
                    TargetInputType = _f.InputType;
                }
            }

            // this bit must always happen, even when field is null
            if (!IsPostBack)
            {
                SetTargetWeb();
                SetInputType();
                txtNumberFormat.Text = TargetNumberFormat;
            }
        }

        private void SetInputType()
        {
            listInputType.Items.Add(new ListItem("Left only", "0"));
            listInputType.Items.Add(new ListItem("Right only", "1"));
            listInputType.Items.Add(new ListItem("Left and Right", "2"));
            listInputType.SelectedValue = TargetInputType;
        }
        #endregion

        public void OnSaveChange(SPField field, bool isNewField)
        {
            CurrencyWithSymbolField _f = null;
            try { _f = field as CurrencyWithSymbolField; }
            catch { }

            if (_f != null)
            {
                SPSite _site = SPControl.GetContextSite(this.Context);
                SPWeb _web = _site.OpenWeb(new Guid(listTargetWeb.SelectedItem.Value));
                _f.SiteName = _web.ID.ToString();
                _f.CurrencyListName = listTargetList.SelectedItem.Value;
                _f.CurrencySymbolColumn = listTargetColumn.SelectedItem.Value;
                _f.InputType = listInputType.SelectedItem.Value;
                _f.NumberFormat = txtNumberFormat.Text;
            }
        }
        #endregion

        #region SetTargetWeb method
        private void SetTargetWeb()
        {
            listTargetWeb.Items.Clear();
            List<ListItem> str = new List<ListItem>();

            SPSite _site = SPControl.GetContextSite(this.Context);

            SPWebCollection _webCollection = _site.AllWebs;
            string contextWebId = SPControl.GetContextWeb(this.Context).ID.ToString();
            foreach (SPWeb web in _webCollection)
            {
                if (web.DoesUserHavePermissions(
                  SPBasePermissions.ViewPages | SPBasePermissions.OpenItems | SPBasePermissions.ViewListItems))
                {
                    str.Add(new ListItem(web.Title, web.ID.ToString()));
                }
            }
            if (str.Count > 0)
            {
                str.Sort(delegate(ListItem item1, ListItem item2)
                {
                    return item1.Text.CompareTo(item2.Text);
                });

                listTargetWeb.Items.AddRange(str.ToArray());
                ListItem bitem = null;
                if (!string.IsNullOrEmpty(TargetWebId)) { bitem = listTargetWeb.Items.FindByValue(TargetWebId); }
                else { bitem = listTargetWeb.Items.FindByValue(contextWebId); }
                if (bitem != null) { listTargetWeb.SelectedIndex = listTargetWeb.Items.IndexOf(bitem); }
                else { listTargetWeb.SelectedIndex = 0; }

                SetTargetList(listTargetWeb.SelectedItem.Value);
            }

        }
        #endregion
        
        #region SetTargetList method
        private void SetTargetList(string selectedWebId)
        {
            listTargetList.Items.Clear();
            if (!string.IsNullOrEmpty(selectedWebId))
            {
                SPSite _site = SPControl.GetContextSite(this.Context);
                SPWeb _web = _site.OpenWeb(new Guid(selectedWebId));
                List<ListItem> str = new List<ListItem>();
                SPListCollection _listCollection = _web.Lists;
                foreach (SPList list in _listCollection)
                {
                    if (!list.Hidden)
                    {
                        str.Add(new ListItem(list.Title, list.ID.ToString()));
                    }
                }
                if (str.Count > 0)
                {
                    str.Sort(delegate(ListItem item1, ListItem item2)
                    {
                        return item1.Text.CompareTo(item2.Text);
                    });

                    listTargetList.Items.AddRange(str.ToArray());

                    ListItem bitem = null;
                    if (!string.IsNullOrEmpty(TargetListId)) { bitem = listTargetList.Items.FindByValue(new Guid(TargetListId).ToString()); }
                    if (bitem != null) { listTargetList.SelectedIndex = listTargetList.Items.IndexOf(bitem); }
                    else { listTargetList.SelectedIndex = 0; }

                    SetTargetColumn(selectedWebId, listTargetList.SelectedItem.Value);
                }
            }

        }
        #endregion

        #region SetTargetColumn method

        private void SetTargetColumn(string webId, string selectedListId)
        {
            listTargetColumn.Items.Clear();
            if (!string.IsNullOrEmpty(webId) && !string.IsNullOrEmpty(selectedListId))
            {
                SPSite _site = SPControl.GetContextSite(this.Context);
                SPWeb _web = _site.OpenWeb(new Guid(webId));
                SPList list = _web.Lists[new Guid(selectedListId)];
                SPFieldCollection fields = list.Fields;
                List<ListItem> str = new List<ListItem>();
                foreach (SPField f in fields)
                {
                    if (CanFieldBeDisplayed(f))
                    {
                        str.Add(new ListItem(
                          string.Format(CultureInfo.InvariantCulture, "{0}", f.Title), f.StaticName.ToString()));
                    }
                }
                if (str.Count > 0)
                {
                    str.Sort(delegate(ListItem item1, ListItem item2)
                    {
                        return item1.Text.CompareTo(item2.Text);
                    });

                    listTargetColumn.Items.AddRange(str.ToArray());

                    ListItem bitem = null;
                    if (!string.IsNullOrEmpty(TargetColumnId)) { bitem = listTargetColumn.Items.FindByValue(TargetColumnId); }
                    if (bitem != null) { listTargetColumn.SelectedIndex = listTargetColumn.Items.IndexOf(bitem); }
                    else { listTargetColumn.SelectedIndex = 0; }
                }

            }
        }
        #endregion

        #region CanFieldBeDisplayed method
        private bool CanFieldBeDisplayed(SPField f)
        {
            bool retval = false;
            if (f != null && !f.Hidden && (Array.IndexOf<string>(
              EXCLUDED_FIELDS, f.InternalName) < 0))
            {
                switch (f.Type)
                {
                    case SPFieldType.Computed:
                        if (((SPFieldComputed)f).EnableLookup) { retval = true; }
                        break;
                    case SPFieldType.Calculated:
                        if (((SPFieldCalculated)f).OutputType == SPFieldType.Text) { retval = true; }
                        break;
                    default:
                        retval = true;
                        break;
                }
            }

            return retval;
        }
        readonly string[] EXCLUDED_FIELDS = new string[]{
          "_Author","_Category", "_CheckinComment", "_Comments", "_Contributor", "_Coverage", "_DCDateCreated",
          "_DCDateModified", "_EditMenuTableEnd", "_EditMenuTableStart", "_EndDate", "_Format",
          "_HasCopyDestinations", "_IsCurrentVersion", "_LastPrinted", "_Level", "_ModerationComments",
          "_ModerationStatus", "_Photo", "_Publisher", "_Relation", "_ResourceType", "_Revision",
          "_RightsManagement", "_SharedFileIndex", "_Source", "_SourceUrl", "_Status", "ActualWork",
          "AdminTaskAction", "AdminTaskDescription", "AdminTaskOrder", "AssignedTo", "Attachments",
          "AttendeeStatus", "Author", "BaseAssociationGuid", "BaseName", "Birthday", "Body",
          "BodyAndMore", "BodyWasExpanded", "Categories", "CheckoutUser", "Comment", "Comments", "Completed",
          "Created", "Created_x0020_By", "Created_x0020_Date", "DateCompleted", "DiscussionLastUpdated",
          "DiscussionTitle", "DocIcon", "DueDate", "Editor", "EmailBody", "EmailCalendarDateStamp",
          "EmailCalendarSequence", "EmailCalendarUid", "EndDate", "EventType", "Expires",
          "ExtendedProperties", "fAllDayEvent", "File_x0020_Size", "File_x0020_Type", "FileDirRef",
          "FileLeafRef", "FileRef", "FileSizeDisplay", "FileType", "FormData", "FormURN", "fRecurrence",
          "FSObjType", "FullBody", "Group", "GUID", "HasCustomEmailBody", "Hobbies", "HTML_x0020_File_x0020_Type",
          "IMAddress", "ImageCreateDate", "ImageHeight", "ImageSize", "ImageWidth", "Indentation", "IndentLevel",
          "InstanceID", "IsActive", "IsSiteAdmin", "ItemChildCount", "Keywords", "Last_x0020_Modified", "LessLink",
          "LimitedBody", "LinkDiscussionTitle", "LinkDiscussionTitleNoMenu", "LinkFilename", "LinkFilenameNoMenu",
          "LinkIssueIDNoMenu", "LinkTitle", "LinkTitleNoMenu", "MasterSeriesItemID", "MessageBody", "MessageId",
          "MetaInfo", "Modified", "Modified_x0020_By", "MoreLink", "Notes", "Occurred", "ol_Department",
          "ol_EventAddress", "owshiddenversion", "ParentFolderId", "ParentLeafName", "ParentVersionString",
          "PendingModTime", "PercentComplete", "PermMask", "PersonViewMinimal", "Picture", "PostCategory",
          "Priority", "ProgId", "PublishedDate", "QuotedTextWasExpanded", "RecurrenceData", "RecurrenceID",
          "RelatedIssues", "RelevantMessages", "RepairDocument", "ReplyNoGif", "RulesUrl", "ScopeId", "SelectedFlag",
          "SelectFilename", "ShortestThreadIndex", "ShortestThreadIndexId", "ShortestThreadIndexIdLookup",
          "ShowCombineView", "ShowRepairView", "StartDate", "StatusBar", "SystemTask", "TaskCompanies",
          "TaskDueDate", "TaskGroup", "TaskStatus", "TaskType", "TemplateUrl", "ThreadIndex", "Threading",
          "ThreadingControls", "ThreadTopic", "Thumbnail", "TimeZone", "ToggleQuotedText", "TotalWork",
          "TrimmedBody", "UniqueId", "VirusStatus", "WebPage", "WorkAddress", "WorkflowAssociation",
          "WorkflowInstance", "WorkflowInstanceID", "WorkflowItemId", "WorkflowListId", "WorkflowVersion",
          "xd_ProgID", "xd_Signature", "XMLTZone", "XomlUrl"
        };
        #endregion

        #region Custom properties
        private string TargetWebId
        {
            get
            {
                object o = this.ViewState["TARGET_WEB_ID"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : string.Empty;
            }
            set { this.ViewState["TARGET_WEB_ID"] = value; }
        }

        private string TargetListId
        {
            get
            {
                object o = this.ViewState["TARGET_LIST_ID"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : string.Empty;
            }
            set { this.ViewState["TARGET_LIST_ID"] = value; }
        }

        private string TargetColumnId
        {
            get
            {
                object o = this.ViewState["TARGET_COLUMN_ID"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : string.Empty;
            }
            set { this.ViewState["TARGET_COLUMN_ID"] = value; }
        }

        private string TargetNumberFormat
        {
            get
            {
                object o = this.ViewState["TARGET_NUMBER_FORMAT"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : CurrencyWithSymbolField.DefaultCurrencyFormat;
            }
            set { this.ViewState["TARGET_NUMBER_FORMAT"] = value; }
        }

        private string TargetInputType
        {
            get
            {
                object o = this.ViewState["TARGET_INPUT_TYPE"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : CurrencyWithSymbolField.DefaultInputType;
            }
            set { this.ViewState["TARGET_INPUT_TYPE"] = value; }
        }
        #endregion

        #region SelectedTargetWebChanged method
        protected void SelectedTargetWebChanged(Object sender, EventArgs args)
        {
            if (listTargetWeb.SelectedIndex > -1)
            {
                SetTargetList(listTargetWeb.SelectedItem.Value);
                Page.SetFocus(listTargetList);
            }
        }
        #endregion

        #region SelectedTargetListChanged method
        protected void SelectedTargetListChanged(Object sender, EventArgs args)
        {
            if (listTargetList.SelectedIndex > -1)
            {
                string webId = string.Empty;
                if (listTargetWeb.Items.Count > 0)
                {
                    webId = listTargetWeb.SelectedItem.Value;
                }
                else if (!string.IsNullOrEmpty(TargetWebId)) { webId = TargetWebId; }
                SetTargetColumn(webId, listTargetList.SelectedItem.Value);
                Page.SetFocus(listTargetColumn);
            }
        }
        #endregion
    }
}
