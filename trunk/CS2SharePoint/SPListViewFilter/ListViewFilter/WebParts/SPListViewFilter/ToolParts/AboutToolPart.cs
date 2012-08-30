using System.Reflection;
using System.Web.UI;
using ListViewFilter.Extensions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace ListViewFilter.WebParts.SPListViewFilter.ToolParts
{
    ///<summary>
    ///</summary>
    public class AboutToolPart : ToolPart
    {
        ///<summary>
        ///</summary>
        public AboutToolPart()
        {
            Title = this.LocalizedString("ToolTip_About");
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var lcid = SPContext.Current.Web.UICulture.LCID;
            var ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Controls.Add(new LiteralControl(string.Format(
                    @"<b>SPListViewFilter {0}</b>
                      <p>{2}</p>
                      <p>
                        <a href=""http://sharepointsolutions.ru/solutions/splistviewfilter/?ver={0}&lcid={1}"" target=""_blank"">{3}</a>
                      </p>", 
                ver,
                lcid, 
                this.LocalizedString("Link_SolutionDescription"),
                this.LocalizedString("Link_SolutionDetails")
                )));
        }
    }
}
