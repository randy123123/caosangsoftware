using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListViewFilter.DataObjects;
using ListViewFilter.Extensions;
using ListViewFilter.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace ListViewFilter.ApplicationObjects
{
    ///<summary>
    ///</summary>
    public class ControlManager
    {
        internal static Control GetControl(ListFilterField field, SPField spField, bool useSql, FilterPanelType panelType)
        {
            var container = PrepareContainer(field, spField, useSql, panelType);
            return container;
        }

        internal static string LocalizedString(string key)
        {
            var id = "$Resources:" + key;
            var lcid = SPContext.Current.Web.Language;
            return SPUtility.GetLocalizedString(id, "ListViewFilter", lcid);
        }

        /// <summary>
        /// Getting allowed filter types for list field
        /// </summary>
        /// <param name="spField">List field</param>
        /// <returns></returns>
        public static FilterType GetAllowedFilterTypes(SPField spField)
        {
            if (string.Compare(spField.TypeAsString, "TaxonomyFieldType", true) == 0)
                return FilterType.Text | FilterType.TaxonomyTerm;
            if (string.Compare(spField.TypeAsString, "TaxonomyFieldTypeMulti", true) == 0)
                return FilterType.Text | FilterType.TaxonomyMultiTerm;
            switch (spField.Type)
            {
                case SPFieldType.Boolean:
                    return FilterType.Boolean | FilterType.DropDownSingleValue
                           | FilterType.DropDownMultiValue;
                case SPFieldType.Calculated:
                    return FilterType.Text | FilterType.TextWithOptions; //TODO
                case SPFieldType.Choice:
                    return FilterType.Text | FilterType.TextWithOptions
                           | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue
                           | FilterType.AutoComplete;
                case SPFieldType.Computed:
                    return FilterType.Text | FilterType.TextWithOptions | FilterType.AutoComplete;
                case SPFieldType.Currency:
                    return FilterType.Text | FilterType.TextWithOptions
                        | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue
                        | FilterType.AutoComplete;
                case SPFieldType.URL:
                    return FilterType.Text | FilterType.TextWithOptions;
                case SPFieldType.MultiChoice:
                    return FilterType.Text | FilterType.TextWithOptions
                           | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue
                           | FilterType.AutoComplete;
                case SPFieldType.User:
                    return FilterType.PeoplePicker | FilterType.PeoplePickerMulti
                           | FilterType.DropDownSingleValue// | FilterType.DropDownMultiValue
                           | FilterType.AutoComplete;
                case SPFieldType.Text:
                    return FilterType.Text | FilterType.TextWithOptions
                           | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue
                           | FilterType.AutoComplete;
                case SPFieldType.Note:
                    return FilterType.Text | FilterType.TextWithOptions;
                case SPFieldType.DateTime:
                    return FilterType.Date | FilterType.DateRange
                        | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue;
                case SPFieldType.ModStat:
                    return FilterType.Text | FilterType.TextWithOptions
                        | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue;
                case SPFieldType.Lookup:
                    return FilterType.Text | FilterType.TextWithOptions
                        | FilterType.DropDownSingleValue | FilterType.DropDownMultiValue
                        | FilterType.AutoComplete;
                default:
                    return FilterType.Text | FilterType.TextWithOptions; //TODO
            }
        }

        private static FieldFilterContainer PrepareContainer(ListFilterField field, SPField spField, bool useSql, FilterPanelType panelType)
        {
            var container = new FieldFilterContainer
            {
                FieldInternalName = field.InternalName,
                FieldType = field.Type,
            };
            switch (field.Type)
            {
                case FilterType.Text:
                    PrepareContainerForText(container, field, spField, panelType);
                    break;
                case FilterType.TextWithOptions:
                    PrepareContainerForTextWithOptions(container, field, spField, panelType);
                    break;
                case FilterType.DropDownSingleValue:
                    PrepareContainerForDropDownSingleValue(container, field, spField, useSql, panelType);
                    break;
                case FilterType.DropDownMultiValue:
                    PrepareContainerForDropDownMultiValue(container, field, spField, useSql, panelType);
                    break;
                case FilterType.AutoComplete:
                    PrepareContainerForAutoComplete(container, field, spField, panelType);
                    break;
                case FilterType.Date:
                    PrepareContainerForDate(container, field, spField, panelType);
                    break;
                case FilterType.DateRange:
                    PrepareContainerForDateRange(container, field, spField, panelType);
                    break;
                case FilterType.PeoplePicker:
                    PrepareContainerForPeoplePicker(container, field, spField, panelType);
                    break;
                case FilterType.PeoplePickerMulti:
                    PrepareContainerForPeoplePickerMulti(container, field, spField, panelType);
                    break;
                case FilterType.Boolean:
                    PrepareContainerForBoolean(container, field, spField, panelType);
                    break;
                case FilterType.TaxonomyTerm:
                    PrepareContainerForTaxonomyTerm(container, field, spField, panelType);
                    break;
                case FilterType.TaxonomyMultiTerm:
                    PrepareContainerForTaxonomyMultiTerm(container, field, spField, panelType);
                    break;
            }
            return container;
        }

        private static void PrepareContainerForText(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("185px");

            var txtSimple = new TextBox { CssClass = "ms-long", Width = width };
            container.Controls.Add(txtSimple);
        }

        private static void PrepareContainerForTextWithOptions(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("155px");
            var uniqueId = Guid.NewGuid().ToString("N");
            var txt = new TextBox { CssClass = "ms-long", Width = width };
            var selector = new HtcMenu();
            var hiddenId = string.Format("{0}_Type", field.InternalName);
            var menuId = string.Format("{0}_{1}_Selector", uniqueId, field.InternalName);
            var menuContainerId = string.Format("{0}_{1}_Container", uniqueId, field.InternalName);
            container.Controls.Add(new LiteralControl(string.Format(@"<input type=""hidden"" id=""{0}"" name=""{0}"" value=""contains"" />", hiddenId)));
            selector.MenuID = menuId;
            selector.Title = "Selector";
            selector.Caption = "Selector";
            selector.LargeIconMode = false;
            selector.MenuChildren.Add(GetFilterTypeMenuOption(Strings.TypeEqual, hiddenId, menuContainerId, IconUrls.Equal, "eq"));
            selector.MenuChildren.Add(GetFilterTypeMenuOption(Strings.TypeNotEqual, hiddenId, menuContainerId, IconUrls.NotEqual, "neq"));
            selector.MenuChildren.Add(GetFilterTypeMenuOption(Strings.TypeBeginsWith, hiddenId, menuContainerId, IconUrls.BeginsWith, "beginswith"));
            selector.MenuChildren.Add(GetFilterTypeMenuOption(Strings.TypeContains, hiddenId, menuContainerId, IconUrls.Contains, "contains"));
            container.Controls.Add(txt);
            container.Controls.Add(new LiteralControl(
                string.Format(@"<span id=""{1}""><img style=""cursor:pointer;"" onclick=""javascript:OpenWebPartMenu('{0}', document.getElementById('{1}'), '','False'); TrapMenuClick(event); return false;"" id=""{1}_img"" src=""{2}"" /></span>",
                    menuId,
                    menuContainerId,
                    GetFilterTypeIconUrl(field.InternalName))));
            container.Controls.Add(selector);
        }

        private static void PrepareContainerForDropDownSingleValue(Control container, ListFilterField field, SPField spField, bool useSql, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("185px");
            var ddlSimple = new DropDownList
            {
                CssClass = "ms-long",
                Width = width,
                AppendDataBoundItems = true
            };
            ddlSimple.Items.Add(new ListItem(Strings.ValueAll, "-2"));
            ddlSimple.Items.Add(new ListItem(Strings.ValueNull, "-1"));

            if (useSql)
            {
                #region SQL
                if (spField is SPFieldModStat)
                {
                    ddlSimple.DataSource = spField.DistinctValues();
                }
                else
                {
                    switch (spField.Type)
                    {
                        case SPFieldType.Lookup:
                            ddlSimple.DataValueField = "Key";
                            ddlSimple.DataValueField = "Value";
                            ddlSimple.DataSource = spField.IsMulti()
                                                       ? spField.DistinctMultiLookupValues()
                                                       : spField.DistinctLookupValues();
                            break;
                        case SPFieldType.User:
                            ddlSimple.DataValueField = "Key";
                            ddlSimple.DataValueField = "Value";
                            ddlSimple.DataSource = spField.IsMulti()
                                                       ? spField.DistinctMultiUserValues()
                                                       : spField.DistinctUserValues();
                            break;
                        case SPFieldType.DateTime:
                            ddlSimple.DataSource = spField
                                .DistinctDateTimeValues(DateTime.MinValue)
                                .Where(x => x != DateTime.MinValue)
                                .Select(x => x.ToShortDateString());
                            break;
                        case SPFieldType.Boolean:
                            ddlSimple.DataSource = spField
                                .DistinctValues(false);
                            break;
                        case SPFieldType.Currency:
                            var lcid = spField.AttributeValueInteger("LCID");
                            var nfi = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
                            GetCombinedNumberFormatInfo(nfi, lcid, ((SPFieldCurrency)spField).DisplayFormat);
                            var curVals = spField
                                .DistinctValues<double>()
                                .Select(v => new { Value = v, Text = v.ToString("C", nfi) });
                            ddlSimple.DataTextField = "Text";
                            ddlSimple.DataValueField = "Value";
                            ddlSimple.DataSource = curVals;
                            break;
                        case SPFieldType.MultiChoice:
                            ddlSimple.DataSource = ((SPFieldMultiChoice)spField).Choices;
                            break;
                        case SPFieldType.Choice:
                            ddlSimple.DataSource = ((SPFieldChoice)spField).Choices;
                            break;
                        default:
                            ddlSimple.DataSource = spField.DistinctValues(string.Empty)
                                .Select(x => x.Trim());
                            break;
                    }
                }
                #endregion
            }
            else
            {
                #region API
                var list = spField.ParentList;
                var query = new SPQuery
                {
                    ViewFields = string.Format("<FieldRef Name='{0}'/>", spField.InternalName),
                    Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='TRUE'></FieldRef></OrderBy>", spField.InternalName),
                    ViewFieldsOnly = true
                };
                var items = list.GetItems(query);
                switch (spField.Type)
                {
                    case SPFieldType.Lookup:
                        ddlSimple.DataValueField = "Key";
                        ddlSimple.DataValueField = "Value";
                        var fLookup = spField as SPFieldLookup;
                        ddlSimple.DataSource = fLookup.AllowMultipleValues
                            ? items.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(f => new SPFieldLookupValueCollection(f.ToString()))
                                   .Where(x => x != null)
                                   .SelectMany(x => x.Cast<SPFieldLookupValue>())
                                   .Select(x => new KeyValuePair<int, string>(x.LookupId, x.LookupValue))
                                   .Distinct()
                            : items.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(x => new SPFieldLookupValue(x.ToString()))
                                   .Select(x => new KeyValuePair<int, string>(x.LookupId, x.LookupValue))
                                   .Distinct();
                        break;
                    case SPFieldType.User:
                        ddlSimple.DataValueField = "Key";
                        ddlSimple.DataValueField = "Value";
                        var fUser = spField as SPFieldUser;
                        ddlSimple.DataSource = fUser.AllowMultipleValues
                            ? items.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(f => new SPFieldUserValueCollection(list.ParentWeb, f.ToString()))
                                   .Where(x => x != null)
                                   .SelectMany(x => x.Cast<SPFieldUserValue>())
                                   .Select(x => new KeyValuePair<int, string>(x.LookupId, x.LookupValue))
                                   .Distinct()
                            : items.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(x => new SPFieldUserValue(list.ParentWeb, x.ToString()))
                                   .Select(x => new KeyValuePair<int, string>(x.LookupId, x.LookupValue))
                                   .Distinct();
                        break;
                    case SPFieldType.DateTime:
                        ddlSimple.DataValueField = "Key";
                        ddlSimple.DataValueField = "Value";
                        var fDate = spField as SPFieldDateTime;
                        ddlSimple.DataSource = items.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Cast<DateTime>()
                            .ToDictionary(k => k, v => v.ToShortDateString());
                        break;
                    case SPFieldType.Boolean:
                        ddlSimple.DataValueField = "Key";
                        ddlSimple.DataValueField = "Value";
                        ddlSimple.DataSource = new Dictionary<bool, bool> { { true, true }, { false, false } };
                        break;
                    case SPFieldType.Currency:
                        ddlSimple.DataTextField = "Key";
                        ddlSimple.DataValueField = "Value";
                        var lcid = spField.AttributeValueInteger("LCID");
                        var nfi = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
                        var fCurrency = spField as SPFieldCurrency;
                        GetCombinedNumberFormatInfo(nfi, lcid, ((SPFieldCurrency)spField).DisplayFormat);
                        ddlSimple.DataSource = items.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Cast<double>()
                            .Distinct()
                            .ToDictionary(k => k, v => v.ToString("C", nfi));
                        break;
                    case SPFieldType.MultiChoice:
                        ddlSimple.DataSource = ((SPFieldMultiChoice)spField).Choices;
                        break;
                    case SPFieldType.Choice:
                        ddlSimple.DataSource = ((SPFieldChoice)spField).Choices;
                        break;
                    default:
                        ddlSimple.DataSource = items.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Select(x => x.ToString())
                            .Distinct();
                        break;

                }

                #endregion
            }
            ddlSimple.DataBind();
            container.Controls.Add(ddlSimple);
        }

        internal static void GetCombinedNumberFormatInfo(NumberFormatInfo nfiFmt, int currencyLocaleId, SPNumberFormatTypes displayFormat)
        {
            var twoDecimals = displayFormat;
            if (twoDecimals == SPNumberFormatTypes.Automatic)
                twoDecimals = SPNumberFormatTypes.TwoDecimals;
            try
            {
                var info2 = new CultureInfo(currencyLocaleId);
                var numberFormat = info2.NumberFormat;
                nfiFmt.CurrencyPositivePattern = numberFormat.CurrencyPositivePattern;
                nfiFmt.CurrencyNegativePattern = numberFormat.CurrencyNegativePattern;
                nfiFmt.CurrencySymbol = numberFormat.CurrencySymbol;
            }
            catch (ArgumentException)
            {
            }
            nfiFmt.CurrencyDecimalDigits = (int)twoDecimals;
        }

        private static void PrepareContainerForDropDownMultiValue(Control container, ListFilterField field, SPField spField, bool useSql, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? "300px"
                            : "190px";
            var uniqueId = Guid.NewGuid().ToString("N");
            var selector = new HtcMenu();
            var hiddenId = string.Format("{0}_Type", field.InternalName);
            var menuId = string.Format("{0}_{1}_Selector", uniqueId, field.InternalName);
            var menuContainerId = string.Format("{0}_{1}_Container", uniqueId, field.InternalName);
            container.Controls.Add(new LiteralControl(string.Format(@"<input type=""hidden"" id=""{0}"" name=""{0}"" value=""{1}"" />", hiddenId, GetFilterDropDownMultiPostedValue(field.InternalName))));
            selector.MenuID = menuId;
            selector.Title = "Selector";
            selector.Caption = "Selector";
            selector.LargeIconMode = false;
            var items = new List<KeyValuePair<string, string>>();
            if (useSql)
            {
                #region SQL
                if (spField is SPFieldModStat)
                {
                    items = spField.DistinctValues().Select(x => new KeyValuePair<string, string>(x, x)).ToList();
                }
                else
                {
                    switch (spField.Type)
                    {
                        case SPFieldType.Lookup:
                            items = spField.IsMulti()
                                        ? spField.DistinctMultiLookupValues().Select(
                                            x => new KeyValuePair<string, string>(x.Value.ToString(), x.Value)).ToList()
                                        : spField.DistinctLookupValues().Select(
                                            x => new KeyValuePair<string, string>(x.Value.ToString(), x.Value)).ToList();
                            break;
                        case SPFieldType.User:
                            items = spField.IsMulti()
                                        ? spField.DistinctMultiUserValues().Select(
                                            x => new KeyValuePair<string, string>(x.Value.ToString(), x.Value)).ToList()
                                        : spField.DistinctUserValues().Select(
                                            x => new KeyValuePair<string, string>(x.Value.ToString(), x.Value)).ToList();
                            break;
                        case SPFieldType.DateTime:
                            items = spField
                                .DistinctDateTimeValues(DateTime.MinValue)
                                .Where(x => x != DateTime.MinValue)
                                .Select(x => x.ToShortDateString())
                                .Select(x => new KeyValuePair<string, string>(x, x))
                                .ToList();
                            break;
                        case SPFieldType.Boolean:
                            items = spField
                                .DistinctValues(false)
                                .Select(x => new KeyValuePair<string, string>(x.ToString(), x.ToString()))
                                .ToList();
                            break;
                        case SPFieldType.Currency:
                            var lcid = spField.AttributeValueInteger("LCID");
                            var nfi = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
                            GetCombinedNumberFormatInfo(nfi, lcid, ((SPFieldCurrency)spField).DisplayFormat);
                            var curVals = spField
                                .DistinctValues<double>()
                                .Select(v => new { Value = v, Text = v.ToString("C", nfi) });
                            items = curVals
                                .Select(x => new KeyValuePair<string, string>(x.Value.ToString(), x.Text))
                                .ToList();
                            break;
                        default:
                            items = spField.DistinctValues(string.Empty)
                                .Select(x => new KeyValuePair<string, string>(x, x))
                                .ToList();
                            break;
                    }

                }
                #endregion
            }
            else
            {
                #region API
                var list = spField.ParentList;
                var query = new SPQuery
                {
                    ViewFields = string.Format("<FieldRef Name='{0}'/>", spField.InternalName),
                    Query = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='TRUE'></FieldRef></OrderBy>", spField.InternalName),
                    ViewFieldsOnly = true
                };
                var listItems = list.GetItems(query);
                switch (spField.Type)
                {
                    case SPFieldType.Lookup:
                        var fLookup = spField as SPFieldLookup;
                        items = fLookup.AllowMultipleValues
                            ? listItems.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(f => new SPFieldLookupValueCollection(f.ToString()))
                                   .Where(x => x != null)
                                   .SelectMany(x => x.Cast<SPFieldLookupValue>())
                                   .Distinct()
                                   .Select(x => new KeyValuePair<string, string>(x.LookupId.ToString(), x.LookupValue))
                                   .ToList()
                            : listItems.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(f => new SPFieldLookupValue(f.ToString()))
                                   .Distinct()
                                   .Select(x => new KeyValuePair<string, string>(x.LookupId.ToString(), x.LookupValue))
                                   .ToList();
                        break;
                    case SPFieldType.User:
                        var fUser = spField as SPFieldUser;
                        items = fUser.AllowMultipleValues
                            ? listItems.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(x => new SPFieldUserValueCollection(list.ParentWeb, x.ToString()))
                                   .SelectMany(f => f.Cast<SPFieldUserValue>())
                                   .Where(x => x != null)
                                   .Distinct()
                                   .Select(x => new KeyValuePair<string, string>(x.LookupId.ToString(), x.LookupValue))
                                   .ToList()
                            : listItems.Cast<SPListItem>()
                                   .Select(x => x[spField.InternalName])
                                   .Where(x => x != null)
                                   .Select(x => new SPFieldUserValue(list.ParentWeb, x.ToString()))
                                   .Distinct()
                                   .Select(x => new KeyValuePair<string, string>(x.LookupId.ToString(), x.LookupValue))
                                   .ToList();
                        break;
                    case SPFieldType.DateTime:
                        var fDate = spField as SPFieldDateTime;
                        items = listItems.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Where(x => x != null)
                            .Cast<DateTime>()
                            .Select(x => new KeyValuePair<string, string>(x.ToString(), x.ToShortDateString()))
                            .ToList();
                        break;
                    case SPFieldType.Boolean:
                        items.Add(new KeyValuePair<string, string>(true.ToString(), true.ToString()));
                        items.Add(new KeyValuePair<string, string>(false.ToString(), false.ToString()));
                        break;
                    case SPFieldType.Choice:
                    case SPFieldType.MultiChoice:
                        var fChoice = spField as SPFieldChoice;
                        items = listItems.Cast<SPListItem>()
                               .Select(x => x[spField.InternalName])
                               .Where(x => x != null)
                               .SelectMany(x => x.ToString().Split(new[] { ";#" }, StringSplitOptions.RemoveEmptyEntries))
                               .Where(x => x != null)
                               .Distinct()
                               .Select(x => new KeyValuePair<string, string>(x, x))
                               .ToList();
                        break;
                    case SPFieldType.Currency:
                        var lcid = spField.AttributeValueInteger("LCID");
                        var nfi = (NumberFormatInfo)Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
                        var fCurrency = spField as SPFieldCurrency;
                        GetCombinedNumberFormatInfo(nfi, lcid, ((SPFieldCurrency)spField).DisplayFormat);
                        items = listItems.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Where(x => x != null)
                            .Cast<double>()
                            .Distinct()
                            .Select(x => new KeyValuePair<string, string>(x.ToString(), x.ToString("C", nfi)))
                            .ToList();
                        break;
                    default:
                        items = listItems.Cast<SPListItem>()
                            .Select(x => x[spField.InternalName])
                            .Where(x => x != null)
                            .Select(x => x.ToString())
                            .Distinct()
                            .Select(x => new KeyValuePair<string, string>(x, x))
                            .ToList();
                        break;
                }
                #endregion
            }
            var vals = GetFilterDropDownMultiPostedValue(field.InternalName);
            var valsStr = new StringBuilder(10);
            foreach (var item in items)
            {
                var flag = vals.Split('|').Contains(item.Key);
                var option = new HtcMenuOption
                                 {
                                     DisplayText = item.Value,
                                     Description = item.Key,
                                     IconSrc = "/_layouts/images/blank.gif",
                                     OnClickText = string.Format(@"SetDropDownSelection_{0}('{1}'); return false;", field.InternalName, item.Key)
                                 };
                selector.MenuChildren.Add(option);
            }
            container.Controls.Add(new LiteralControl(
                string.Format(string.Format(@"<div id=""{{1}}"" style=""cursor:pointer;display:block;width:{{2}};height:auto;min-height:30px;"" onclick=""javascript:OpenWebPartMenu('{{0}}', document.getElementById('{{1}}'), '','True'); TrapMenuClick(event); return false;"">{0}</div>", LocalizedString("Text_ClickToSelect")),
                    menuId,
                    menuContainerId,
                    width)));
            container.Controls.Add(selector);
            var script = new LiteralControl(string.Format(@"
                        <script>
	                        function SetDropDownSelection_{0}(val){{
                                var menuNode = document.getElementById('{1}');
                                var resText = '';
                                var resHtml = '';
                                for(var i=0; i < menuNode.childNodes.length; i++){{
                                    var node = menuNode.childNodes[i];
                                    if(node.nodeType == '1'){{
                                        var nodeVal = node.getAttribute('description');
                                        if(nodeVal != null){{
                                            if(nodeVal == val) {{ if(node.getAttribute('checked') == 'true') {{ node.setAttribute('checked', 'false'); }} else {{ node.setAttribute('checked', 'true'); }} }}
                                            if(node.getAttribute('checked') == 'true'){{
                                                if(resHtml.length > 0){{ resHtml = resHtml + ', ';}} if(resText.length > 0){{ resText = resText + '|';}}
                                                resHtml = resHtml + '<b>' + node.getAttribute('text') + '</b>';
                                                resText = resText + node.getAttribute('description');}}}}}}}}
                                if(resText != '') {{ document.getElementById('{2}').innerHTML = resHtml;  document.getElementById('{3}').value = resText; }}
else {{ document.getElementById('{2}').innerHTML = '{4}'; }}
                            }}
                            function SetDropDownSelectionPost_{0}(val){{
                                var items = document.getElementById('{3}').value.split('|');
                                if(items.length > 0){{
                                    var resHtml = ''; var menuNode = document.getElementById('{1}');
                                    for(var i=0; i < menuNode.childNodes.length; i++){{
                                    var node = menuNode.childNodes[i];
                                    if(node.nodeType == '1') {{ var nodeVal = node.getAttribute('description');
                                    if(nodeVal != null) {{ for(var j = 0; j < items.length; j++) {{
                                    if(nodeVal == items[j]) {{ node.setAttribute('checked', 'true'); if(resHtml.length > 0){{ resHtml = resHtml + ', ';}} resHtml = resHtml + '<b>'+node.getAttribute('text')+'</b>'; }}}}}}}}}}
                                    if(resHtml != '') {{ document.getElementById('{2}').innerHTML = resHtml; }}
                                }}
                            }};
                            if (typeof(_spBodyOnLoadFunctionNames) != 'undefined') {{ if (_spBodyOnLoadFunctionNames != null) {{ _spBodyOnLoadFunctionNames.push('SetDropDownSelectionPost_{0}'); }} }}
                    </script>",
                              field.InternalName,
                              menuId,
                              menuContainerId,
                              hiddenId, Strings.ClickToSelect));
            container.Controls.Add(script);
        }

        private static void PrepareContainerForAutoComplete(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("185px");
            var txtAutocomplete = new TextBox { CssClass = "ms-long field-" + field.InternalName, Width = width };
            container.Controls.Add(txtAutocomplete);
            var txtAutocompleteScript = new LiteralControl(string.Format(@"
                        <script>
	                        $(function() {{
                                $( "".field-{0}"" ).autocomplete({{
			                        source: ""/_layouts/ListViewFilter/Handlers/FieldAutocompleteHandler.ashx?List={1}&Field={2}"",
			                        minLength: 2
		                        }});
                            }});
	                    </script>", field.InternalName, spField.ParentList.ID, spField.Id));
            container.Controls.Add(txtAutocompleteScript);
        }

        private static void PrepareContainerForDate(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var dt = new DateTimeControl { DateOnly = true };
            container.Controls.Add(dt);
        }

        private static void PrepareContainerForDateRange(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var dateField = spField as SPFieldDateTime;
            if (dateField == null)
                return;
            var ctx = SPContext.Current;
            var localeId = Convert.ToInt32(ctx.RegionalSettings.LocaleId);
            var dtRange = new Control();
            var dtl = new DateTimeControl { DateOnly = true, LocaleId = localeId };
            var dtr = new DateTimeControl { DateOnly = true, LocaleId = localeId };
            dtRange.Controls.Add(new LiteralControl(string.Format(@"<span style=""display:block;float:left;line-height: 26px; margin-right: 4px;"">{0}</span>", LocalizedString("Text_From"))));
            dtRange.Controls.Add(dtl);
            dtRange.Controls.Add(new LiteralControl(string.Format(@"<span style=""display:block;float:left;line-height: 26px; margin-right: 4px;"">{0}</span>", LocalizedString("Text_To"))));
            dtRange.Controls.Add(dtr);
            container.Controls.Add(dtRange);
        }

        private static void PrepareContainerForPeoplePicker(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("185px");
            container.Controls.Add(new PeopleEditor { MultiSelect = false, Rows = 1, Width = width });
        }

        private static void PrepareContainerForPeoplePickerMulti(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            container.Controls.Add(new PeopleEditor { Rows = 1, Width = new Unit("360px") });
        }

        private static void PrepareContainerForBoolean(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var width = panelType == FilterPanelType.StackPanel
                            ? new Unit("300px")
                            : new Unit("165px");
            var ddlBoolean = new DropDownList
            {
                CssClass = "ms-long",
                Width = width,
                AppendDataBoundItems = true
            };
            ddlBoolean.Items.Add(new ListItem(Strings.ValueAll, "-2"));
            ddlBoolean.Items.Add(new ListItem(Strings.ValueNull, "-1"));
            ddlBoolean.Items.Add(new ListItem(Strings.ValueFalse, "0"));
            ddlBoolean.Items.Add(new ListItem(Strings.ValueTrue, "1"));
            container.Controls.Add(ddlBoolean);
        }

        private static void PrepareContainerForTaxonomyTerm(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            PrepareContainerForTaxonomyMultiTerm(container, field, spField, panelType);
        }

        private static void PrepareContainerForTaxonomyMultiTerm(Control container, ListFilterField field, SPField spField, FilterPanelType panelType)
        {
            var sspId = spField.GetCustomProperty("SspId").ToString();
            var termSetId = spField.GetCustomProperty("TermSetId").ToString();
            var addFlag =
                string.Compare(spField.GetCustomProperty("CreateValuesInEditForm").ToString(), "TRUE", true) == 0;
            var twtc = new TaxonomyWebTaggingControl
                           {
                               SSPList = sspId,
                               TermSetList = termSetId,
                               IsAddTerms = addFlag,
                               IsMulti = spField.IsMulti()
                           };
            container.Controls.Add(twtc);
        }

        private static string GetFilterTypeIconUrl(string fieldInternalName)
        {
            var type = HttpContext.Current.Request.Form[fieldInternalName + "_Type"];
            switch ((type ?? string.Empty).ToUpper())
            {
                case "EQ":
                    return IconUrls.Equal;
                case "NEQ":
                    return IconUrls.NotEqual;
                case "BEGINSWITH":
                    return IconUrls.BeginsWith;
                case "CONTAINS":
                    return IconUrls.Contains;
                default:
                    return IconUrls.Contains;
            }
        }

        private static string GetFilterDropDownMultiPostedValue(string fieldInternalName)
        {
            var val = HttpContext.Current.Request.Form[fieldInternalName + "_Type"];
            return val ?? string.Empty;
        }

        private static HtcMenuOption GetFilterTypeMenuOption(string title, string hiddenFieldId, string containerId, string imageUrl, string type)
        {
            return new HtcMenuOption
                       {
                           DisplayText = title,
                           Description = title,
                           IconSrc = imageUrl,
                           OnClickText =
                               string.Format(
                                   "document.getElementById('{0}').value = '{3}'; document.getElementById('{1}_img').src='{2}';",
                                   hiddenFieldId, containerId, imageUrl, type)
                       };
        }

        private static class IconUrls
        {
            public const string Equal = "/_layouts/images/ListViewFilter/iceq.png";
            public const string NotEqual = "/_layouts/images/ListViewFilter/icneq.png";
            public const string Contains = "/_layouts/images/ListViewFilter/iccontains.png";
            public const string BeginsWith = "/_layouts/images/ListViewFilter/icbegins.png";
        }

        private static class Strings
        {
            public static readonly string ValueNull = LocalizedString("FilterValue_IsNull");
            public static readonly string ValueAll = LocalizedString("FilterValue_All");
            public static readonly string ValueTrue = LocalizedString("FilterValue_True");
            public static readonly string ValueFalse = LocalizedString("FilterValue_False");
            public static readonly string TypeEqual = LocalizedString("FilterType_Equal");
            public static readonly string TypeNotEqual = LocalizedString("FilterType_NotEqual");
            public static readonly string TypeContains = LocalizedString("FilterType_Contains");
            public static readonly string TypeBeginsWith = LocalizedString("FilterType_BeginWith");
            public static readonly string ClickToSelect = LocalizedString("Text_ClickToSelect");
        }
    }
}
