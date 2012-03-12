//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-1-19
//
//修订记录: andreeyang@163.com
//修改日期: 2009-5
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
 
using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{
    
    /// <summary>
    /// 列表字段编辑权限设置控件
    /// </summary>
    public class FieldRightSettingPart : BaseSPListWebPart
    {
        private Button _btnSubmit;
        private Button _btnCancel;
        private Table _layoutTable;

        private Dictionary<string, CheckBox> _AllUserCanEditControls = new Dictionary<string, CheckBox>();
        private Dictionary<string, CheckBox> _CreatorCanEditControls = new Dictionary<string, CheckBox>();
        private Dictionary<string, PeopleEditorEx> _SpecialAccountsControls =
        new Dictionary<string, PeopleEditorEx>();

        private Dictionary<string, PeopleEditorEx> _SpecialGroupsControls =
        new Dictionary<string, PeopleEditorEx>();

        private Dictionary<string, CheckBox> _AllUserCanDisplayControls = new Dictionary<string, CheckBox>();
        private Dictionary<string, CheckBox> _CreatorCanDisplayControls = new Dictionary<string, CheckBox>();
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
            _layoutTable.CellPadding = 3;

            this.Controls.Add(_layoutTable);
            this.AddRow(_layoutTable, "<b>"+GetResource("Field")+"</b>",
                "<b>" + GetResource("ViewPermission") + "</b>", "<b>" + GetResource("EditPermission") + "</b>");

            ListFieldPermissionSetting setting = ListFieldPermissionSetting.GetListSetting(base.List);

            //
            foreach (SPField f in List.Fields)
            {
                if (f.Hidden || f.ReadOnlyField) continue;

                TableRow row = new TableRow();
                _layoutTable.Rows.Add(row);
                TableCell fieldCell = new TableCell();
                fieldCell.VerticalAlign = VerticalAlign.Top;
                fieldCell.CssClass = "ms-formlabel";
                row.Cells.Add(fieldCell);

                fieldCell.Text = f.Title + f.AuthoringInfo;

                addDisplayContorls(f, row, setting);

                addEditControls(f, row, setting);              

                //
            }

           // addDisplaycontrols(setting);

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
            lastRow.Cells[1].ColumnSpan = 2;
            lastRow.Cells[1].Controls.Add(_btnSubmit);
            lastRow.Cells[1].Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
            lastRow.Cells[1].Controls.Add(_btnCancel);
        }

        void _btnCancel_Click(object sender, EventArgs e)
        {
            base.RedirectToListSettingPage();
        }

        void addDisplayContorls(SPField f, TableRow row, ListFieldPermissionSetting setting)
        {
            //View
            TableCell ctlCell = new TableCell();
            ctlCell.VerticalAlign = VerticalAlign.Top;
            ctlCell.CssClass = "ms-formbody";
            row.Cells.Add(ctlCell);

            CheckBox allUser = new CheckBox();
            allUser.Text = GetResource("AllUsers");
            allUser.Checked = true;
            ctlCell.Controls.Add(allUser);
            this.AddHtml("<br/>", ctlCell);

            _AllUserCanDisplayControls.Add(f.InternalName, allUser);

            CheckBox creator = new CheckBox();
            creator.Text = GetResource("Creator");
            creator.Checked = true;
            ctlCell.Controls.Add(creator);
            this.AddHtml("<br/>", ctlCell);

            _CreatorCanDisplayControls.Add(f.InternalName, creator);

            this.AddHtml( GetResource("TheseUsers") + "<br/>", ctlCell);
            PeopleEditorEx peopleEditor = new PeopleEditorEx();
            peopleEditor.MultiSelect = true;
            peopleEditor.Rows = 1;
            peopleEditor.Width = new Unit("300px");

            peopleEditor.SelectionSet = "User";//"User,SPGroup";
            ctlCell.Controls.Add(peopleEditor);

            _SpecialAccountsDisplayControls.Add(f.InternalName, peopleEditor);

            this.AddHtml("<br/>" + GetResource("TheseGroups")+ "<br/>", ctlCell);
            PeopleEditorEx peopleEditor2 = new PeopleEditorEx();
            peopleEditor2.MultiSelect = true;
            peopleEditor2.Rows = 1;
            peopleEditor2.Width = new Unit("300px");

            peopleEditor2.SelectionSet = "SPGroup";
            ctlCell.Controls.Add(peopleEditor2);

            _SpecialGroupsDisplayControls.Add(f.InternalName, peopleEditor2);

            this.SetDisplayControlValue(allUser, creator, peopleEditor, peopleEditor2, setting, f.InternalName);
        }

        void addEditControls(SPField f ,TableRow row, ListFieldPermissionSetting setting)
        {
            //---------Edit 

            TableCell ctlCell = new TableCell();
            ctlCell.VerticalAlign = VerticalAlign.Top;
            ctlCell.CssClass = "ms-formbody";
            row.Cells.Add(ctlCell);

            CheckBox allUser = new CheckBox();
            allUser.Text = GetResource("AllUsers");
            allUser.Checked = true;
            ctlCell.Controls.Add(allUser);
            this.AddHtml("<br/>", ctlCell);

            _AllUserCanEditControls.Add(f.InternalName, allUser);

            CheckBox creator = new CheckBox();
            creator.Text = GetResource("Creator");
            creator.Checked = true;
            ctlCell.Controls.Add(creator);
            this.AddHtml("<br/>", ctlCell);

            _CreatorCanEditControls.Add(f.InternalName, creator);

            this.AddHtml(GetResource("TheseUsers") + "<br/>", ctlCell);
            PeopleEditorEx peopleEditor = new PeopleEditorEx();
            peopleEditor.MultiSelect = true;
            peopleEditor.Rows = 1;
            peopleEditor.Width = new Unit("300px");

            peopleEditor.SelectionSet = "User";//"User,SPGroup";
            ctlCell.Controls.Add(peopleEditor);

            _SpecialAccountsControls.Add(f.InternalName, peopleEditor);

            this.AddHtml("<br/>"+GetResource("TheseGroups")+"<br/>", ctlCell);
            PeopleEditorEx peopleEditor2 = new PeopleEditorEx();
            peopleEditor2.MultiSelect = true;
            peopleEditor2.Rows = 1;
            peopleEditor2.Width = new Unit("300px");

            peopleEditor2.SelectionSet = "SPGroup";
            ctlCell.Controls.Add(peopleEditor2);

            _SpecialGroupsControls.Add(f.InternalName, peopleEditor2);

            this.SetControlValue(allUser, creator, peopleEditor, peopleEditor2, setting, f.InternalName);
        }
          

        void SetControlValue(CheckBox allUser, CheckBox creator, PeopleEditorEx peopleEditor,
            PeopleEditorEx peopleEditor2, ListFieldPermissionSetting setting, string fieldName)
        {
            if (setting == null || setting.Count == 0 || Page.IsPostBack)
                return;

            FieldPermission set = setting.GetByFieldName(fieldName);

            if (set == null) return;

            allUser.Checked = set.AllUserCanEdit;
            creator.Checked = set.CreatorCanEdit;
            peopleEditor.CommaSeparatedAccounts = set.SpecialAccounts;
            peopleEditor2.CommaSeparatedAccounts = set.SpecialGroups;
        }

        void SetDisplayControlValue(CheckBox allUser, CheckBox creator,PeopleEditorEx peopleEditor,
            PeopleEditorEx peopleEditor2, ListFieldPermissionSetting setting, string fieldName)
        {
            if (setting == null || setting.Count == 0 || Page.IsPostBack)
                return;

            FieldPermission set = setting.GetByFieldName(fieldName);

            if (set == null) return;

            allUser.Checked = set.AllUserCanDisplay;
            creator.Checked = set.CreatorCanDisplay;
            peopleEditor.CommaSeparatedAccounts = set.SpecialAccountsDisplay;
            peopleEditor2.CommaSeparatedAccounts = set.SpecialGroupsDisplay;
        }


        void _btnSubmit_Click(object sender, EventArgs e)
        {
            ListFieldPermissionSetting setting = new ListFieldPermissionSetting();

            foreach (SPField f in List.Fields)
            {
                if (f.Hidden || f.ReadOnlyField) continue;

                FieldPermission set = new FieldPermission();
                setting.Add(set);

                set.FieldName = f.InternalName.ToLower();

                CheckBox allUser = _AllUserCanEditControls[f.InternalName];

                set.AllUserCanEdit = allUser.Checked;

                CheckBox creator = _CreatorCanEditControls[f.InternalName];

                set.CreatorCanEdit = creator.Checked;

                PeopleEditorEx peopleEditor = _SpecialAccountsControls[f.InternalName];

                set.SpecialAccounts = peopleEditor.CommaSeparatedAccounts;

                set.SpecialGroups = _SpecialGroupsControls[f.InternalName].CommaSeparatedAccounts;

                CheckBox allDisplayUser = this._AllUserCanDisplayControls[f.InternalName];

                set.AllUserCanDisplay = allDisplayUser.Checked;

                //PeopleEditorEx peopleDisplayEditor = this._SpecialGroupsDisplayControls[f.InternalName];

                set.SpecialAccountsDisplay = _SpecialAccountsDisplayControls[f.InternalName].CommaSeparatedAccounts;
                // peopleDisplayEditor.CommaSeparatedAccounts;
                set.SpecialGroupsDisplay = _SpecialGroupsDisplayControls[f.InternalName].CommaSeparatedAccounts;

                set.AllUserCanDisplay = _AllUserCanDisplayControls[f.InternalName].Checked;
                set.CreatorCanDisplay = _CreatorCanDisplayControls[f.InternalName].Checked;

            }

            setting.Save(base.List);

            base.RedirectToListSettingPage();
        }

        //void AddRow( Table table , params Control[] ctls)
        //{
        //    TableRow row = new TableRow();
        //    table.Rows.Add(row);

        //    foreach (Control c in ctls)
        //    {
        //        TableCell cell = new TableCell();
        //        row.Cells.Add(cell);
        //        cell.Controls.Add(c);
        //    }
        //}

        
    }
}