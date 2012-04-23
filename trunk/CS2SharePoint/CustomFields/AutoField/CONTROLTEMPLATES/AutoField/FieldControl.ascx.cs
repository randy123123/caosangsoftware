﻿using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                if (String.IsNullOrEmpty(LiteralFieldFormat.Text) || LiteralFieldFormat.Text == "[Auto]")
                {
                    LiteralFieldFormat.Text = GenerateAutoValue();
                }
                return LiteralFieldFormat.Text;
            }
            set
            {
                EnsureChildControls();
                LiteralFieldFormat.Text = (string)this.ItemFieldValue;
                if(String.IsNullOrEmpty(LiteralFieldFormat.Text)) LiteralFieldFormat.Text = "[Auto]";
            }
        }

        private string GenerateAutoValue()
        {
            string result = parentFieldFormat;
            IEnumerable<string> getFormat = CS2Regex.Substring(parentFieldFormat, "[", "]");
            foreach (string format in getFormat)
                result = result.Replace(String.Format("[{0}]", format), GetValue(format));
            return result;
        }
        private const string GroupConfig = "AutoField";
        private string GetValue(string format)
        {
            IEnumerable<string> getValueFormat = CS2Regex.Substring(format, "(", ")");
            string valueFormat = "";
            bool hasFormat = getValueFormat.Count() != 0;
            if (hasFormat) valueFormat = getValueFormat.FirstOrDefault();
            if (format.StartsWith("Today"))
            {
                if (!hasFormat) valueFormat = "yyyyMMdd";
                return DateTime.Today.ToString(valueFormat);
            }
            else if (format.StartsWith("ItemCountInDate"))
            {
                //Read data config
                CS2ConfigList config = new CS2ConfigList();
                string countingDate = config.GetConfig(GroupConfig, String.Format("LIST_{0}_Date", this.ListId));
                string countingValue = config.GetConfig(GroupConfig, String.Format("LIST_{0}_Count", this.ListId));
                //Load data
                DateTime date = String.IsNullOrEmpty(countingDate) ? DateTime.Today : CS2Convert.ToDateTime(countingDate).Value;
                int count = 0;
                if (date == DateTime.Today)
                    count = CS2Convert.ToInt(countingValue);
                count += 1;
                if (String.IsNullOrEmpty(countingDate) || date != DateTime.Today)
                    config.SetConfig(GroupConfig, String.Format("LIST_{0}_Date", this.ListId), DateTime.Today.ToString());
                config.SetConfig(GroupConfig, String.Format("LIST_{0}_Count", this.ListId), count.ToString());
                if (hasFormat)
                    return count.ToString(valueFormat);
                else
                    return count.ToString();
            }
            else return format;
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
