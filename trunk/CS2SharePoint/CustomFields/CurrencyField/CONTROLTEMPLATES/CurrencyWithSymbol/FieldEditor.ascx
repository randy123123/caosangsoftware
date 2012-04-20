<%@ Control Language="C#" AutoEventWireup="false" CompilationMode="Always" Inherits="CSSoft.CS2SPCustomFields.CurrencyField.CurrencyWithSymbolFieldEditor, CSSoft.CurrencyField, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c99e7a59c8706863" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<wssuc:InputFormSection runat="server" id="CurrencyFieldSection" Title="Special Column Settings">
  <Template_InputFormControls>
    <wssuc:InputFormControl runat="server" LabelText="Specify detailed options for the currency field column">
      <Template_Control>
          <div style="width: 100%; text-align: left; border-width: 0px;">
            <table style="width: 100%; border-width: 0px; border-collapse: collapse;" cellpadding="0" cellspacing="0">
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>Get currency symbol from this site:</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:DropDownList runat="server" ID="listTargetWeb" AutoPostBack="true" OnSelectedIndexChanged="SelectedTargetWebChanged" />
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>In this list:</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:DropDownList runat="server" ID="listTargetList" AutoPostBack="true" OnSelectedIndexChanged="SelectedTargetListChanged" />
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>In this column</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:DropDownList runat="server" ID="listTargetColumn" />
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>Input type</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:DropDownList runat="server" ID="listInputType" />
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>Number format</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:TextBox runat="server" ID="txtNumberFormat" />
                </td>
              </tr>
            </table>
          </div>
        </Template_Control>
    </wssuc:InputFormControl>
  </Template_InputFormControls>
</wssuc:InputFormSection>
