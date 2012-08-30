<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="ListViewFilter" %>
<%@ Import Namespace="ListViewFilter.ApplicationObjects" %>
<%@ Import Namespace="ListViewFilter.DataObjects" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FieldSettingsConfig.aspx.cs"
    Inherits="ListViewFilter.Layouts.ListViewFilter.FieldSettingsConfig" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script language="javascript" type="text/javascript">
        function Reorder(eSelect, iCurrentField, numSelects) {
            var eForm = eSelect.form;
            var iNewOrder = eSelect.selectedIndex + 1;
            var iPrevOrder;
            var positions = new Array(numSelects);
            var ix;
            for (ix = 0; ix < numSelects; ix++) {
                positions[ix] = 0;
            }
            for (ix = 0; ix < numSelects; ix++) {
                positions[eSelect.form["ViewOrder" + ix].selectedIndex] = 1;
            }
            for (ix = 0; ix < numSelects; ix++) {
                if (positions[ix] == 0) {
                    iPrevOrder = ix + 1;
                    break;
                }
            }
            if (iNewOrder != iPrevOrder) {
                var iInc = iNewOrder > iPrevOrder ? -1 : 1
                var iMin = Math.min(iNewOrder, iPrevOrder);
                var iMax = Math.max(iNewOrder, iPrevOrder);
                for (var iField = 0; iField < numSelects; iField++) {
                    if (iField != iCurrentField) {
                        if (eSelect.form["ViewOrder" + iField].selectedIndex + 1 >= iMin &&
					eSelect.form["ViewOrder" + iField].selectedIndex + 1 <= iMax) {
                            eSelect.form["ViewOrder" + iField].selectedIndex += iInc;
                        }
                    }
                }
            }
        }

        function ReturnFilterDefinition() {
            var val = document.getElementById('<%= FilterXml.ClientID %>').value;
            window.frameElement.commonModalDialogClose(1, val);
        }
    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="padding-left: 5px">
        <table style="width: 100%" id="onetIDListForm" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tbody>
                                <tr>
                                    <td id="MSOZoneCell_WebPartWPQ2" class="s4-wpcell-plain" valign="top">
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <%=LocalizedString("Caption_Field")%>
                                                            </td>
                                                            <td>
                                                                <%=LocalizedString("Caption_Caption")%>
                                                            </td>
                                                            <td>
                                                                <%=LocalizedString("Caption_ControlType")%>
                                                            </td>
                                                            <td>
                                                                <%=LocalizedString("Caption_Position")%>
                                                            </td>
                                                        </tr>
                                                        <%
                                                            int index = 0;
                                                            foreach (ListFilterField field in SelectedFields)
                                                            {
                                                                SPField spField = GetSPField(field.InternalName);
                                                                FilterType type = ControlManager.GetAllowedFilterTypes(spField);
                                                        %>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" name="FieldSelectedFlag<%SPHttpUtility.NoEncode(index, Response.Output);%>"
                                                                    checked="checked" />
                                                            </td>
                                                            <td>
                                                                <input type="hidden" name="FieldName<%SPHttpUtility.NoEncode(index, Response.Output);%>"
                                                                    value="<%SPHttpUtility.NoEncode(field.InternalName, Response.Output);%>" />
                                                                <%SPHttpUtility.NoEncode(field.Caption, Response.Output);%>
                                                            </td>
                                                            <td>
                                                                <input type="text" name="FieldCaption<%SPHttpUtility.NoEncode(index, Response.Output);%>"
                                                                    value="<%SPHttpUtility.NoEncode(field.Caption, Response.Output);%>" />
                                                            </td>
                                                            <td>
                                                                <select name="FieldType<%SPHttpUtility.NoEncode(index, Response.Output);%>" style="width:330px;">
                                                                    <% if ((type & FilterType.Text) == FilterType.Text)
                                                                       {%>
                                                                    <option value="1" <%= field.Type == FilterType.Text ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_Text")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.TextWithOptions) == FilterType.TextWithOptions)
                                                                       {%>
                                                                    <option value="2" <%= field.Type == FilterType.TextWithOptions ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_TextWithOptions")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.DropDownSingleValue) == FilterType.DropDownSingleValue)
                                                                       {%>
                                                                    <option value="4" <%= field.Type == FilterType.DropDownSingleValue ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_DropDownList")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.DropDownMultiValue) == FilterType.DropDownMultiValue)
                                                                       {%>
                                                                    <option value="8" <%= field.Type == FilterType.DropDownMultiValue ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_DropDownListWithSettings")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.AutoComplete) == FilterType.AutoComplete)
                                                                       {%>
                                                                    <option value="16" <%= field.Type == FilterType.AutoComplete ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_Autocomplete")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.Date) == FilterType.Date)
                                                                       {%>
                                                                    <option value="32" <%= field.Type == FilterType.Date ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_Date")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.DateRange) == FilterType.DateRange)
                                                                       {%>
                                                                    <option value="64" <%= field.Type == FilterType.DateRange ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_DateRange")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.PeoplePicker) == FilterType.PeoplePicker)
                                                                       {%>
                                                                    <option value="128" <%= field.Type == FilterType.PeoplePicker ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_PeoplePicker")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.PeoplePickerMulti) == FilterType.PeoplePickerMulti)
                                                                       {%>
                                                                    <option value="256" <%= field.Type == FilterType.PeoplePickerMulti ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_PeoplePickerWithMultiSelect")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.Boolean) == FilterType.Boolean)
                                                                       {%>
                                                                    <option value="512" <%= field.Type == FilterType.Boolean ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_Boolean")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.TaxonomyTerm) == FilterType.TaxonomyTerm)
                                                                       {%>
                                                                    <option value="1024" <%= field.Type == FilterType.TaxonomyTerm ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_Taxonomy")%></option>
                                                                    <% } %>
                                                                    <% if ((type & FilterType.TaxonomyMultiTerm) == FilterType.TaxonomyMultiTerm)
                                                                       {%>
                                                                    <option value="2048" <%= field.Type == FilterType.TaxonomyMultiTerm ? @"selected=""selected""" : string.Empty %>>
                                                                        <%=LocalizedString("ControlType_TaxonomyMultiValue")%></option>
                                                                    <% } %>
                                                                </select>
                                                            </td>
                                                            <td>
                                                                <select onchange="Reorder(this, <%SPHttpUtility.NoEncode(index, Response.Output);%>, <%SPHttpUtility.NoEncode(CurrentListAllFieldsCount, Response.Output);%>)"
                                                                    name="ViewOrder<%SPHttpUtility.NoEncode(index, Response.Output);%>">
                                                                    <% for (int iIndex2 = 0; iIndex2 < CurrentListAllFieldsCount; iIndex2++)
                                                                       {
                                                                           if (iIndex2 == index)
                                                                           {
                                                                    %>
                                                                    <option value="<%SPHttpUtility.NoEncode(iIndex2+1, Response.Output);%>" selected="selected">
                                                                        <%SPHttpUtility.NoEncode(iIndex2 + 1, Response.Output);%></option>
                                                                    <%
                                                                           }
                                                                           else
                                                                           {
                                                                    %>
                                                                    <option value="<%SPHttpUtility.NoEncode(iIndex2+1, Response.Output);%>">
                                                                        <%SPHttpUtility.NoEncode(iIndex2 + 1, Response.Output);%></option>
                                                                    <%
                                                                           }
                                                                       }%>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <%
                                                                       index = index + 1;
                                                            }
                                                            foreach (ListFilterField field in NotSelectedFields)
                                                            {
                                                                SPField spField = GetSPField(field.InternalName);
                                                                FilterType type = ControlManager.GetAllowedFilterTypes(spField);
                                                        %>
                                                        <tr>
                                                            <td>
                                                                <input type="checkbox" name="FieldSelectedFlag<%SPHttpUtility.NoEncode(index, Response.Output);%>" />
                                                            </td>
                                                            <td>
                                                                <input type="hidden" name="FieldName<%SPHttpUtility.NoEncode(index, Response.Output);%>"
                                                                    value="<%SPHttpUtility.NoEncode(field.InternalName, Response.Output);%>" />
                                                                <%SPHttpUtility.NoEncode(field.Caption, Response.Output);%>
                                                            </td>
                                                            <td>
                                                                <input type="text" name="FieldCaption<%SPHttpUtility.NoEncode(index, Response.Output);%>"
                                                                    value="<%SPHttpUtility.NoEncode(field.Caption, Response.Output);%>" />
                                                            </td>
                                                            <td>
                                                                <select name="FieldType<%SPHttpUtility.NoEncode(index, Response.Output);%>" style="width:330px;">
                                                                <% 
                                                                if ((type & FilterType.Text) == FilterType.Text)
                                                                       {%><option value="1" selected="selected"><%=LocalizedString("ControlType_Text")%></option><% }
                                                                if ((type & FilterType.TextWithOptions) == FilterType.TextWithOptions)
                                                                       {%><option value="2"><%=LocalizedString("ControlType_TextWithOptions")%></option><% }
                                                                if ((type & FilterType.DropDownSingleValue) == FilterType.DropDownSingleValue)
                                                                       {%><option value="4"><%=LocalizedString("ControlType_DropDownList")%></option><% }
                                                                if ((type & FilterType.DropDownMultiValue) == FilterType.DropDownMultiValue)
                                                                       {%><option value="8"><%=LocalizedString("ControlType_DropDownListWithSettings")%></option><% }
                                                                if ((type & FilterType.AutoComplete) == FilterType.AutoComplete)
                                                                       {%><option value="16"><%=LocalizedString("ControlType_Autocomplete")%></option><% }
                                                                if ((type & FilterType.Date) == FilterType.Date)
                                                                       {%><option value="32"><%=LocalizedString("ControlType_Date")%></option><% }
                                                                if ((type & FilterType.DateRange) == FilterType.DateRange)
                                                                       {%><option value="64"><%=LocalizedString("ControlType_DateRange")%></option><% }
                                                                if ((type & FilterType.PeoplePicker) == FilterType.PeoplePicker)
                                                                       {%><option value="128"><%=LocalizedString("ControlType_PeoplePicker")%></option><% }
                                                                if ((type & FilterType.PeoplePickerMulti) == FilterType.PeoplePickerMulti)
                                                                       {%><option value="256"><%=LocalizedString("ControlType_PeoplePickerWithMultiSelect")%></option><% }
                                                                if ((type & FilterType.Boolean) == FilterType.Boolean)
                                                                       {%><option value="512"><%=LocalizedString("ControlType_Boolean")%></option><% }
                                                                if ((type & FilterType.TaxonomyTerm) == FilterType.TaxonomyTerm)
                                                                       {%><option value="1024"><%=LocalizedString("ControlType_Taxonomy")%></option><% }
                                                                if ((type & FilterType.TaxonomyMultiTerm) == FilterType.TaxonomyMultiTerm)
                                                                       {%><option value="2048"><%=LocalizedString("ControlType_TaxonomyMultiValue")%></option><% } %>
                                                                </select>
                                                            </td>
                                                            <td>
                                                                <select onchange="Reorder(this, <%SPHttpUtility.NoEncode(index, Response.Output);%>, <%SPHttpUtility.NoEncode(CurrentListAllFieldsCount, Response.Output);%>)"
                                                                    name="ViewOrder<%SPHttpUtility.NoEncode(index, Response.Output);%>">
                                                                    <% for (int iIndex2 = 0; iIndex2 < CurrentListAllFieldsCount; iIndex2++)
                                                                       {
                                                                           if (iIndex2 == index)
                                                                           {
                                                                    %>
                                                                    <option value="<%SPHttpUtility.NoEncode(iIndex2+1, Response.Output);%>" selected="selected">
                                                                        <%SPHttpUtility.NoEncode(iIndex2 + 1, Response.Output);%></option>
                                                                    <%
                                                                           }
                                                                           else
                                                                           {
                                                                    %>
                                                                    <option value="<%SPHttpUtility.NoEncode(iIndex2+1, Response.Output);%>">
                                                                        <%SPHttpUtility.NoEncode(iIndex2 + 1, Response.Output);%></option>
                                                                    <%
                                                                           }
                                                                       }%>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <%
                                                                       index = index + 1;
                                                            }
                                                        %>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="FilterXml" runat="server" />
                                        <table cellpadding="0" cellspacing="0" width="800px" style="table-layout: fixed;">
                                            <tr>
                                                <td class="ms-formline">
                                                    <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" width="800px" style="padding-top: 7px; table-layout: fixed;">
                                            <tr>
                                                <td width="100%">
                                                    <table class="ms-formtoolbar" cellpadding="2" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td width="99%" class="ms-toolbar" nowrap="nowrap">
                                                                <img src="/_layouts/images/blank.gif" width='1' height='18' alt="" />
                                                            </td>
                                                            <td class="ms-toolbar" nowrap="nowrap">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="right" width="100%" nowrap="nowrap">
                                                                            <asp:Button ID="SaveButton" runat="server" CssClass="ms-ButtonHeightWidth" OnClick="SaveButtonClick"
                                                                                Text='<%#LocalizedString("Button_Apply")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class='ms-separator'>
                                                                &#160;
                                                            </td>
                                                            <td class="ms-toolbar" nowrap="nowrap">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="right" width="100%" nowrap="nowrap">
                                                                            <input type="button" value="<%#LocalizedString("Button_OK")%>" class="ms-ButtonHeightWidth"
                                                                                onclick="javascript:ReturnFilterDefinition();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
</asp:Content>
