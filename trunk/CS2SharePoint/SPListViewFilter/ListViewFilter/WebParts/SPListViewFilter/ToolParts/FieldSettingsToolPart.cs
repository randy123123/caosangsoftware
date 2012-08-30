using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ListViewFilter.DataObjects;
using ListViewFilter.Extensions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace ListViewFilter.WebParts.SPListViewFilter.ToolParts
{
    ///<summary>
    /// Fields configuration toolpart
    ///</summary>
    internal sealed class FieldSettingsToolPart : ToolPart
    {
        public FieldSettingsToolPart()
        {
            Title = this.LocalizedString("ToolTip_Settings");
        }

        private HiddenField _xmlField;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _xmlField = new HiddenField
                            {
                                Value = WebPart.FilterDefinitionString
                            };
            Controls.Add(_xmlField);
            var ctx = SPContext.Current;
            var web = ctx.Web;
            Controls.Add(new LiteralControl(string.Format(@"
                <script language=""javascript"">
                    function ShowSettingsModalDialog() {{
                        var url = '{0}/_layouts/ListViewFilter/FieldSettingsConfig.aspx?List={1}&Url={2}';
                        SP.UI.ModalDialog.showModalDialog(
                        {{
                            url: url,
                            title: '{3}',
                            dialogReturnValueCallback: function (dialogResult, returnValue) {{
                                if (dialogResult == SP.UI.DialogResult.OK) {{
                                    document.getElementById('{4}').value = returnValue;
                                }}
                            }}
                        }});
                        return false;
                    }}
                </script>", 
                          web.Url,
                          WebPart.ListViewWebPart.ListId, 
                          Page.Request.Url, 
                          this.LocalizedString("Caption_SettingsDialog"), 
                          _xmlField.ClientID)));
            var root = string.IsNullOrEmpty(WebPart.FilterDefinitionString)
                                ? new XElement("Filter")
                                : XElement.Parse(WebPart.FilterDefinitionString);
            var fields = root.Elements("Field")
                .Select(x => new ListFilterField(x))
                .OrderBy(x => x.Position);
            if (fields.Count() > 0)
            {
                Controls.Add(new LiteralControl("<table>"));
                foreach (var field in fields)
                {
                    Controls.Add(new LiteralControl(
                        string.Format("<tr><td>{0}</td><td>{1}</td></tr>", field.Caption, field.Type)));
                }
                Controls.Add(new LiteralControl("</table>"));
            }
            Controls.Add(
                new LiteralControl(
                    string.Format(@"<a href=""#"" onclick=""javascript: ShowSettingsModalDialog(); return false;"">{0}</a>",
                    this.LocalizedString("Link_FieldSettings"))));
        }

        private SPListViewFilter WebPart
        {
            get
            {
                return WebPartToEdit as SPListViewFilter;
            }
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            WebPart.FilterDefinitionString = _xmlField.Value;
        }
    }
}
