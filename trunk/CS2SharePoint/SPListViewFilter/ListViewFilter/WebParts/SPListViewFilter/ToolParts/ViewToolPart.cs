using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListViewFilter.Extensions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace ListViewFilter.WebParts.SPListViewFilter.ToolParts
{
    ///<summary>
    ///</summary>
    public class ViewToolPart : ToolPart
    {
        ///<summary>
        ///</summary>
        public ViewToolPart()
        {
            Title = this.LocalizedString("ToolTip_View");
        }

        private RadioButtonList _viewSelector;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _viewSelector = new RadioButtonList();
            _viewSelector.Items.Add(new ListItem(@"WrapPanel<br/><img src=""/_layouts/images/ListViewFilter/ListViewType-WrapPanel.png"" />", "0"));
            _viewSelector.Items.Add(new ListItem(@"Grid<br/><img src=""/_layouts/images/ListViewFilter/ListViewType-Grid.png"" />", "1"));
            _viewSelector.Items.Add(new ListItem(@"StackPanel<br/><img src=""/_layouts/images/ListViewFilter/ListViewType-StackPanel.png"" />", "2"));
            _viewSelector.SelectedIndex = (int) WebPart.PanelType;
            Controls.Add(_viewSelector);
            Controls.Add(new LiteralControl(""));
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
            WebPart.PanelType = (FilterPanelType) _viewSelector.SelectedIndex;
        }
    }
}
