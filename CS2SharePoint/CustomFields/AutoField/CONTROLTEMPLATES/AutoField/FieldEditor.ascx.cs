using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Globalization;

namespace CSSoft.CS2SPCustomFields.AutoField
{
    public partial class AutoWithFormatFieldEditor : UserControl, IFieldEditor
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
            AutoWithFormatField _f = null;
            try { _f = field as AutoWithFormatField; }
            catch { }

            if (_f != null)
            {
                // this bit only happens when field is not null
                if (!IsPostBack)
                {
                    TargetFieldFormat = _f.FieldFormat;
                    TargetInitFieldMsg = _f.InitFieldMsg;
                }
            }

            // this bit must always happen, even when field is null
            if (!IsPostBack)
            {
                TextBoxFieldFormat.Text = TargetFieldFormat;
                TextBoxInitFieldMsg.Text = TargetInitFieldMsg;
            }
        }
        #endregion

        public void OnSaveChange(SPField field, bool isNewField)
        {
            AutoWithFormatField _f = null;
            try { _f = field as AutoWithFormatField; }
            catch { }

            if (_f != null)
            {
                _f.FieldFormat = CS2Convert.ReplaceXMLSpecialChars(TextBoxFieldFormat.Text);
                _f.InitFieldMsg = CS2Convert.ReplaceXMLSpecialChars(TextBoxInitFieldMsg.Text);
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
        private string TargetFieldFormat
        {
            get
            {
                object o = this.ViewState["TARGET_FIELD_FORMAT"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : AutoWithFormatField.DefaultFormat;
            }
            set { this.ViewState["TARGET_FIELD_FORMAT"] = value; }
        }
        private string TargetInitFieldMsg
        {
            get
            {
                object o = this.ViewState["TARGET_INIT_FIELD_MSG"];
                return (o != null && !string.IsNullOrEmpty(o.ToString())) ? o.ToString() : AutoWithFormatField.DefaultInitFieldMsg;
            }
            set { this.ViewState["TARGET_INIT_FIELD_MSG"] = value; }
        }
        #endregion
    }
}
