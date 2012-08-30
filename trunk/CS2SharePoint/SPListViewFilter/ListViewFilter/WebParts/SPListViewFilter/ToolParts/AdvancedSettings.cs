using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebPartPages;
using ListViewFilter.Extensions;

namespace ListViewFilter.WebParts.SPListViewFilter.ToolParts
{
    public class AdvancedSettings : ToolPart
    {
        public AdvancedSettings()
        {
            Title = this.LocalizedString("ToolTip_Advanced");
        }

        private CheckBox _sqlCheckBox;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var title = this.LocalizedString("Text_UseSqlQueries");
            _sqlCheckBox = new CheckBox();
            _sqlCheckBox.Text = title;
            _sqlCheckBox.Checked = WebPart.UseSqlQueries;
            Controls.Add(_sqlCheckBox);
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
            WebPart.UseSqlQueries = _sqlCheckBox.Checked;
        }
    }
}
