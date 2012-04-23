<%@ Control Language="C#" AutoEventWireup="false" CompilationMode="Always" Inherits="CSSoft.CS2SPCustomFields.AutoField.AutoWithFormatFieldEditor, CSSoft.AutoField, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c99e7a59c8706863" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<wssuc:InputFormSection runat="server" id="AutoFieldSection" Title="Special Column Settings">
  <Template_InputFormControls>
    <wssuc:InputFormControl runat="server" LabelText="Specify detailed options for the auto field column">
      <Template_Control>
          <div style="width: 100%; text-align: left; border-width: 0px;">
            <table style="width: 100%; border-width: 0px; border-collapse: collapse;" cellpadding="0" cellspacing="0">
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>Field format</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:TextBox runat="server" ID="TextBoxFieldFormat" />
                </td>
              </tr>
               <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap; padding-top: 10px;">
                  <span>Init field message</span>
                </td>
              </tr>
              <tr>
                <td class="ms-authoringcontrols" style="width: 100%; text-align: left; white-space: nowrap;">
                  <asp:TextBox runat="server" ID="TextBoxInitFieldMsg" />
                </td>
              </tr>
            </table>
          </div>
        </Template_Control>
    </wssuc:InputFormControl>
  </Template_InputFormControls>
</wssuc:InputFormSection>
