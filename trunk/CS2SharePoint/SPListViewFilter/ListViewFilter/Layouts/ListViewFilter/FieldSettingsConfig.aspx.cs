using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ListViewFilter.DataObjects;
using ListViewFilter.WebParts.SPListViewFilter;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace ListViewFilter.Layouts.ListViewFilter
{
    ///<summary>
    /// Field configuration page
    ///</summary>
    public partial class FieldSettingsConfig : LayoutsPageBase
    {
        protected SPWeb CurrentWeb
        {
            get
            {
                return _currentWeb ?? (_currentWeb = SPContext.Current.Web);
            }
        }

        protected SPList CurrentList
        {
            get
            {
                return _currentList ?? (_currentList = CurrentWeb.Lists[ListId]);
            }
        }

        private Guid ListId
        {
            get
            {
                var id = Request.QueryString["List"];
                return new Guid(id);
            }
        }

        private static readonly string[] ExistsFields = new[]
                                                            {
                                                                "ID", 
                                                                "Attachments",
                                                                "Edit",
                                                                "LinkTitleNoMenu",
                                                                "LinkTitle",
                                                                "DocIcon",
                                                                "ContentType",
                                                                "FolderChildCount",
                                                                "ItemChildCount"
                                                            };

        protected IEnumerable<SPField> CurrentListFields
        {
            get
            {
                var names = SelectedFields.Select(f => f.InternalName);
                return CurrentList.Fields
                    .Cast<SPField>()
                    .Where(f => !f.Hidden)
                    .Where(f => !ExistsFields.Contains(f.InternalName))
                    .Where(f => !names.Contains(f.InternalName))
                    .OrderBy(f => f.Title);
            }
        }

        protected IEnumerable<SPField> CurrentListAllFields
        {
            get
            {
                return CurrentList.Fields
                    .Cast<SPField>()
                    .Where(f => !f.Hidden)
                    .Where(f => !ExistsFields.Contains(f.InternalName))
                    .OrderBy(f => f.Title);
            }
        }

        protected SPField GetSPField(string internalName)
        {
            return CurrentList.Fields
                .Cast<SPField>()
                .Where(f => f.InternalName == internalName)
                .FirstOrDefault();
        }

        protected IEnumerable<ListFilterField> SelectedFields
        {
            get
            {
                var root = string.IsNullOrEmpty(FilterXml.Value)
                                ? new XElement("Filter")
                                : XElement.Parse(FilterXml.Value);
                var fields = root.Elements("Field")
                    .Select(x => new ListFilterField(x))
                    .OrderBy(x => x.Position);
                return fields;
            }
        }

        protected IEnumerable<ListFilterField> NotSelectedFields
        {
            get
            {
                return CurrentListFields
                    .Select(f => new ListFilterField
                                     {
                                         Caption = f.Title,
                                         InternalName = f.InternalName,
                                         Position = 0,
                                         Type = FilterType.Text
                                     })
                    .OrderBy(f => f.Caption);
            }
        }

        protected int SelectedFieldsCount
        {
            get
            {
                return SelectedFields.Count();
            }
        }

        protected int CurrentListAllFieldsCount
        {
            get
            {
                return CurrentListAllFields.Count();
            }
        }

        protected SPListViewFilter WebPart
        {
            get
            {

                if (_webPart == null)
                {
                    var url = Page.Request.QueryString["url"];
                    var limitedManager = SPControl.GetContextWeb(Context)
                        .GetLimitedWebPartManager(url, PersonalizationScope.Shared);
                    _webPart = limitedManager.WebParts
                        .Cast<WebPart>()
                        .OfType<SPListViewFilter>()
                        .FirstOrDefault();
                }
                return _webPart;
            }
        }

        protected static string LocalizedString(string key)
        {
            var id = "$Resources:" + key;
            var lcid = (uint)Thread.CurrentThread.CurrentUICulture.LCID;
            return SPUtility.GetLocalizedString(id, "ListViewFilter", lcid);
        }

        private SPListViewFilter _webPart;
        private SPWeb _currentWeb;
        private SPList _currentList;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                FilterXml.Value = WebPart.FilterDefinitionString;
            }
            DataBind();
        }

        protected void SaveButtonClick(object sender, EventArgs e)
        {
            var form = Request.Form;
            var keys = form.AllKeys.Where(x => x.StartsWith("FieldName"));
            var qnt = keys.Count();
            var root = new XElement("Filter");
            for (var i = 0; i < qnt; i++)
            {
                var flag = form["FieldSelectedFlag" + i] != null;
                if (!flag) continue;

                var intName = form["FieldName" + i];
                var caption = form["FieldCaption" + i];
                var type = Convert.ToInt32(form["FieldType" + i]);
                var position = Convert.ToInt32(form["ViewOrder" + i]);

                root.Add(new XElement("Field",
                                      new XAttribute("InternalName", intName),
                                      new XAttribute("Caption", caption),
                                      new XAttribute("Type", type),
                                      new XAttribute("Position", position)));
            }
            FilterXml.Value = root.ToString();
        }
    }
}
