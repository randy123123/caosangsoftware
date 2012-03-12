//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-3-21
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
 
using Microsoft.SharePoint;
 

namespace CodeArt.SharePoint.PermissionEx
{
    
    /// <summary>
    /// 列表内容类型创建设置控件
    /// </summary>
    public class ContentTypesCreateRightSettingPart : BaseSPListWebPart
    {
        private Button _btnSubmit;
        private Button _btnCancel;
        private Table _layoutTable;

        private Dictionary<string, PeopleEditorEx> _SpecialAccountsControls =
        new Dictionary<string, PeopleEditorEx>();

        private Dictionary<string, PeopleEditorEx> _SpecialGroupsControls =
        new Dictionary<string, PeopleEditorEx>();

        protected override void CreateChildControls()
        {
            if (List == null)
                return;

            _layoutTable = new Table ();
            _layoutTable.BorderWidth = new Unit("0px");
            _layoutTable.CssClass = "ms-formtable";
            _layoutTable.CellSpacing = 0;

            this.Controls.Add(_layoutTable);
            this.AddRow(_layoutTable, "<b>"+GetResource("ContentType")+"</b>",
                "&nbsp;&nbsp;&nbsp;<b>"+GetResource("Group")+"</b>");

            ListContentTypesCreateSetting setting = ListContentTypesCreateSetting.GeSetting( base.List );

            //
            foreach (SPContentType f in List.ContentTypes)
            {

                TableRow row = new TableRow();
                _layoutTable.Rows.Add(row);
                TableCell fieldCell = new TableCell();
                fieldCell.VerticalAlign = VerticalAlign.Top;
                fieldCell.CssClass = "ms-formlabel";
                row.Cells.Add(fieldCell);

                fieldCell.Text = f.Name ;

                TableCell ctlCell = new TableCell();
                ctlCell.VerticalAlign = VerticalAlign.Top;
                ctlCell.CssClass = "ms-formbody";
                row.Cells.Add(ctlCell);              

                //this.AddHtml("指定组：", ctlCell);
                PeopleEditorEx peopleEditor2 = new PeopleEditorEx();
                peopleEditor2.MultiSelect = true;
                peopleEditor2.Rows = 1;
                peopleEditor2.Width = new Unit("300px");

                peopleEditor2.SelectionSet = "SPGroup";
                ctlCell.Controls.Add(peopleEditor2);

                if (setting != null)
                    peopleEditor2.CommaSeparatedAccounts = setting.GetContentTypeCreateGroups(f.Name);

                _SpecialGroupsControls.Add(f.Name, peopleEditor2);
            }

            _btnSubmit = new Button();
            _btnSubmit.ID = "btn1";
            _btnSubmit.Text = GetResource("OK");
            _btnSubmit.CssClass = "ms-ButtonHeightWidth";
            this.Controls.Add(_btnSubmit);

            _btnCancel = new Button();
            _btnCancel.Text = GetResource("Cancel");
            _btnCancel.CssClass = "ms-ButtonHeightWidth";

            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
            this.Controls.Add(_btnCancel);

            _btnSubmit.Click += new EventHandler(_btnSubmit_Click);
            _btnCancel.Click += new EventHandler(_btnCancel_Click);            
        }

        void _btnCancel_Click(object sender, EventArgs e)
        {
            base.RedirectToListSettingPage();
        }
                              

        void _btnSubmit_Click(object sender, EventArgs e)
        {
            ListContentTypesCreateSetting setting = new ListContentTypesCreateSetting();

            foreach (SPContentType f in List.ContentTypes)
            {
                ContentTypeCreateSetting set = new ContentTypeCreateSetting();
                setting.Add(set);

                //set.ContentTypeId = f.Id.ToString() ;
                set.ContentTypeName = f.Name;

              
                set.SpecialGroups = _SpecialGroupsControls[set.ContentTypeName].CommaSeparatedAccounts;
            }

            setting.Save(base.List);

            base.RedirectToListSettingPage();     
        }

    }
}
