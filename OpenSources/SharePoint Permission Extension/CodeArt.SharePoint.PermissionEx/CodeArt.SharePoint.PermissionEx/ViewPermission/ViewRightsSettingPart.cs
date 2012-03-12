//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人:andreeyang@163.com
//创建日期: 2009-6
//
//修订记录: 
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeArt.SharePoint.PermissionEx;
using Microsoft.SharePoint;
using CodeArt.SharePoint;
using Microsoft.SharePoint.WebPartPages;


namespace CodeArt.SharePoint.PermissionEx
{

    /// <summary>
    /// List
    /// </summary>
    public class ViewRightsSettingPart : BaseSPListWebPart
    {
        private Button _btnSubmit;
        private Button _btnCancel;
        private Table _layoutTable;

        //private Dictionary<string, PeopleEditorEx> _SpecialAccountsControls =
        //new Dictionary<string, PeopleEditorEx>();

        //private Dictionary<string, PeopleEditorEx> _SpecialGroupsControls =
        //new Dictionary<string, PeopleEditorEx>();

        private Dictionary<string, PeopleEditorEx> _SpecialAccountsDisplayControls =
        new Dictionary<string, PeopleEditorEx>();

        private Dictionary<string, PeopleEditorEx> _SpecialGroupsDisplayControls =
        new Dictionary<string, PeopleEditorEx>();
        protected override void CreateChildControls()
        {

            if (List == null)
                return;

            _layoutTable = new Table();
            _layoutTable.BorderWidth = new Unit("0px");
            _layoutTable.CssClass = "ms-formtable";
            _layoutTable.CellSpacing = 0;

            this.Controls.Add(_layoutTable);

            //
            ListViewPermissionSetting setting = ListViewPermissionSetting.GetListSetting(this.List);           

            this.addDisplayControls(setting);

            _btnSubmit = new Button();
            _btnSubmit.ID = "btn1";
            _btnSubmit.Text = GetResource("OK");
            _btnSubmit.CssClass = "ms-ButtonHeightWidth";
            //this.Controls.Add(_btnSubmit);
            _btnSubmit.Click += new EventHandler(_btnSubmit_Click);

            _btnCancel = new Button();
            _btnCancel.ID = "btnCancel";
            _btnCancel.Text = GetResource("Cancel");
            _btnCancel.CssClass = "ms-ButtonHeightWidth";
            _btnCancel.Click += new EventHandler(_btnCancel_Click);

            var lastRow = this.AddRow(_layoutTable, "", "");
            lastRow.Cells[1].Controls.Add(_btnSubmit);
            lastRow.Cells[1].Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
            lastRow.Cells[1].Controls.Add(_btnCancel);
        }

        void _btnCancel_Click(object sender, EventArgs e)
        {
            base.RedirectToListSettingPage();
        }

        void addDisplayControls(ListViewPermissionSetting setting)
        {
            this.AddRow(_layoutTable, "<b>"+GetResource("View")+"</b>",
                "<b>" + GetResource("ViewPermission") + "</b>");

            foreach (SPView v in List.Views)
            {
                if (v.Hidden)
                    continue;

                TableRow row = new TableRow();
                _layoutTable.Rows.Add(row);
                TableCell fieldCell = new TableCell();
                fieldCell.VerticalAlign = VerticalAlign.Top;
                fieldCell.CssClass = "ms-formlabel";
                row.Cells.Add(fieldCell);

                fieldCell.Text = v.Title;

                TableCell ctlCell = new TableCell();
                ctlCell.VerticalAlign = VerticalAlign.Top;
                ctlCell.CssClass = "ms-formbody";
                row.Cells.Add(ctlCell);

//                this.AddHtml("<br/>", ctlCell);

                this.AddHtml(GetResource("TheseUsers") + "<br/>", ctlCell);
                PeopleEditorEx peopleEditor = new PeopleEditorEx();
                peopleEditor.MultiSelect = true;
                peopleEditor.Rows = 1;
                peopleEditor.Width = new Unit("300px");

                peopleEditor.SelectionSet = "User";//"User,SPGroup";
                ctlCell.Controls.Add(peopleEditor);
                _SpecialAccountsDisplayControls.Add(v.Title, peopleEditor);

                this.AddHtml("<br/>"+GetResource("TheseGroups")+"<br/>", ctlCell);
                PeopleEditorEx peopleEditor2 = new PeopleEditorEx();
                peopleEditor2.MultiSelect = true;
                peopleEditor2.Rows = 1;
                peopleEditor2.Width = new Unit("300px");

                peopleEditor2.SelectionSet = "SPGroup";
                ctlCell.Controls.Add(peopleEditor2);
                _SpecialGroupsDisplayControls.Add(v.Title, peopleEditor2);

                this.SetDisplayControlValue(peopleEditor, peopleEditor2, setting, v.ID);
            }
        }

        void SetDisplayControlValue(PeopleEditorEx peopleEditor,
            PeopleEditorEx peopleEditor2, ListViewPermissionSetting setting, Guid viewId)
        {
            if (setting == null || setting.Count == 0 || Page.IsPostBack)
                return;

            ViewPermission set = setting.GetByViewID( viewId );

            if (set == null) return;

            //allUser.Checked = set.AllUserCanEdit;
            //creator.Checked = set.CreatorCanEdit;
            peopleEditor.CommaSeparatedAccounts = set.SpecialAccounts;
            peopleEditor2.CommaSeparatedAccounts = set.SpecialGroups;
        }

        void _btnSubmit_Click(object sender, EventArgs e)
        {
            ListViewPermissionSetting setting = new ListViewPermissionSetting();

            foreach (SPView v in List.Views)
            {
                if (v.Hidden)
                    continue;
 
                if (null == _SpecialAccountsDisplayControls[v.Title])//新增加的视图？ 
                    continue;

                ViewPermission set = new ViewPermission();
                set.ViewName = v.Title;
                set.ViewID = v.ID;

                set.SpecialAccounts = _SpecialAccountsDisplayControls[v.Title].CommaSeparatedAccounts;
                set.SpecialGroups = _SpecialGroupsDisplayControls[v.Title].CommaSeparatedAccounts;
                setting.Add(set);            
            }

            setting.Save(base.List);

            base.RedirectToListSettingPage();
        }       
    }
}
