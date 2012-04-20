using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Globalization;

namespace CSSoft.CS2SPCustomFields.AutoField
{
    public partial class AutoWithFormatFieldControl : BaseFieldControl
    {
        protected string parentFieldFormat;

        public override object Value
        {
            get
            {
                EnsureChildControls();
                return LiteralFieldFormat.Text;
            }
            set
            {
                EnsureChildControls();
                LiteralFieldFormat.Text = (string)this.ItemFieldValue;
            }
        }
        public override void Validate()
        {
            base.Validate();
        }
        public override void Focus()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            parentFieldFormat = Field.GetCustomProperty("FieldFormat").ToString();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

            LiteralFieldFormat = new Literal();
            this.Controls.Add(LiteralFieldFormat);
        }
    }
}
