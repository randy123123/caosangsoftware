using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Globalization;

namespace CSSoft.CS2SPCustomFields.CurrencyField
{
    public partial class CurrencyWithSymbolFieldControl : BaseFieldControl
    {
        protected string parentSiteName;
        protected string parentCurrencyListName;
        protected string parentCurrencySymbolColumn;
        protected string parentInputType;
        protected string parentNumberFormat;

        public override object Value
        {
            get
            {
                EnsureChildControls();
                decimal currencyValue = ToDecimal(CurrencyValue.Text);
                return String.Format("{0} {1} {2}", 
                    CurrencySymbolLeft.Visible ? CurrencySymbolLeft.SelectedValue : "",
                    currencyValue.ToString(parentNumberFormat),
                    CurrencySymbolRight.Visible ? CurrencySymbolRight.SelectedValue : "");
            }
            set
            {
                EnsureChildControls();
                string[] fieldValue =  ((string)this.ItemFieldValue).Split(' ');
                if (fieldValue.Length == 3)
                {
                    string currencySymbolLeft = fieldValue[0];
                    string currencyValue = fieldValue[1];
                    string currencySymbolRight = fieldValue[2];

                    CurrencyValue.Text = currencyValue;
                    CurrencySymbolLeft.SelectedValue = currencySymbolLeft;
                    CurrencySymbolRight.SelectedValue = currencySymbolRight;
                }
                else
                { 
                    CurrencyValue.Text = (string)this.ItemFieldValue;
                    CurrencySymbolLeft.SelectedIndex = 1;
                    CurrencySymbolRight.SelectedIndex = 0;                
                }
            }
        }
        public override void Validate()
        {
            //base.Validate();
            if (ControlMode == SPControlMode.Display || !IsValid) { return; }
            base.Validate();
            if (CurrencyValue.Text.Length > 0 && !IsDecimal(CurrencyValue.Text))
            {
                this.ErrorMessage = "The value of this field is not a valid number.";//Field.Title + " must be a mumber value.";
                IsValid = false;
                return;
            }
        }
        public bool IsDecimal(string decimalNumber)
        {
            if (decimalNumber.Contains(","))
                decimalNumber = decimalNumber.Replace(",", "");
            decimal d; return Decimal.TryParse(decimalNumber, out d);
        }
        public Decimal ToDecimal(string decimalNumber)
        {
            if (decimalNumber.Contains(","))
                decimalNumber = decimalNumber.Replace(",", "");
            if (IsDecimal(decimalNumber))
                return Decimal.Parse(decimalNumber);
            else return 0;
        }
        public override void Focus()
        {
            EnsureChildControls();
            CurrencyValue.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;
            base.CreateChildControls();

            parentSiteName = Field.GetCustomProperty("SiteName").ToString();
            parentCurrencyListName = Field.GetCustomProperty("CurrencyListName").ToString();
            parentCurrencySymbolColumn = Field.GetCustomProperty("CurrencySymbolColumn").ToString();
            parentInputType = Field.GetCustomProperty("InputType").ToString();
            parentNumberFormat = Field.GetCustomProperty("NumberFormat").ToString();

            if (ControlMode == Microsoft.SharePoint.WebControls.SPControlMode.Display)
                return;

            CurrencyValue = new TextBox();
            CurrencySymbolLeft = new DropDownList();
            CurrencySymbolRight = new DropDownList();
            if (parentInputType == "2") /*Left & Right*/
            {
                CurrencySymbolLeft.AutoPostBack = true;
                CurrencySymbolRight.AutoPostBack = true;
                CurrencySymbolLeft.SelectedIndexChanged += new EventHandler(CurrencySymbolLeft_SelectedIndexChanged);
                CurrencySymbolRight.SelectedIndexChanged += new EventHandler(CurrencySymbolRight_SelectedIndexChanged);
            }
            this.Controls.Add(CurrencySymbolLeft);
            this.Controls.Add(CurrencyValue);
            this.Controls.Add(CurrencySymbolRight);

            //CurrencyValue.TabIndex = TabIndex;
            //CurrencyValue.CssClass = CssClass;
            //CurrencyValue.ToolTip = Field.Title;
            //CurrencySymbolLeft.TabIndex = ++TabIndex;
            //CurrencySymbolLeft.CssClass = CssClass;
            //CurrencySymbolLeft.ToolTip = Field.Title;
            //CurrencySymbolRight.TabIndex = ++TabIndex;
            //CurrencySymbolRight.CssClass = CssClass;
            //CurrencySymbolRight.ToolTip = Field.Title;


            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(new Guid(parentSiteName)))
                {
                    SPList list = web.Lists[new Guid(parentCurrencyListName)];
                    // populate it with the values from the central master page list.
                    SPDataSource dataSource = new SPDataSource();
                    dataSource.List = list;
                    CurrencySymbolLeft.DataSource = dataSource;
                    CurrencySymbolLeft.DataTextField = parentCurrencySymbolColumn;
                    CurrencySymbolLeft.DataValueField = parentCurrencySymbolColumn;
                    CurrencySymbolLeft.DataBind();
                    if (parentInputType == "0") /*LeftOnly*/
                    {
                        CurrencySymbolRight.Visible = false;
                    }
                    else
                    {
                        CurrencySymbolLeft.Items.Insert(0, new ListItem("", ""));
                    }
                    CurrencySymbolRight.DataSource = dataSource;
                    CurrencySymbolRight.DataTextField = parentCurrencySymbolColumn;
                    CurrencySymbolRight.DataValueField = parentCurrencySymbolColumn;
                    CurrencySymbolRight.DataBind();
                    if (parentInputType == "1") /*RightOnly*/
                    {
                        CurrencySymbolLeft.Visible = false;
                    }
                    else
                    {
                        CurrencySymbolRight.Items.Insert(0, new ListItem("", ""));
                        CurrencySymbolRight.SelectedIndex = 1;
                    }
                }
            }
        }

        void CurrencySymbolLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrencySymbolLeft.SelectedIndex == 0) { CurrencySymbolRight.SelectedIndex = 1; }
            else { CurrencySymbolRight.SelectedIndex = 0; }
        }

        void CurrencySymbolRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrencySymbolRight.SelectedIndex == 0) { CurrencySymbolLeft.SelectedIndex = 1; }
            else { CurrencySymbolLeft.SelectedIndex = 0; }
        }
    }
}
