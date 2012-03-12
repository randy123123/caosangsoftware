<%@ Assembly Name="CodeArt.SharePoint.PermissionEx.AppPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f" %>
<%@ Page MasterPageFile="~/_Layouts/Application.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="FieldPermissionSetting.aspx.cs" Inherits="CodeArt.SharePoint.PermissionEx.AppPages.FieldPermissionSetting" %>

<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register Assembly="CodeArt.SharePoint.PermissionEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f"
    Namespace="CodeArt.SharePoint.PermissionEx" TagPrefix="codeArt" %>
<%@ Import Namespace="CodeArt.SharePoint.PermissionEx" %>
<script runat="server">
    
//----------------------------------------------------------------
//Code Art .
//
//
//Author: jianyi0115@163.com
//CreateTime: 2008-1-20
//
//Versions: 
//
//----------------------------------------------------------------
         
</script>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <%=GetResource("FieldPermissionSetting")%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <style>
        .ms-authoringcontrols
        {
            width: 580px;
        }
    </style>
    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0"
        id="diidProjectPageOverview">
        <wssuc:inputformsection id="section1" title="" Description='<%#GetResource("FieldPermissionSettingDesc")%>'
            runat="server">
				<Template_InputFormControls>
					<wssuc:InputFormControl runat="server">
						<Template_Control>					 
							
<codeArt:FieldRightSettingPart runat="server" id="fSetting" />
							
						</Template_Control>
					</wssuc:InputFormControl>
				</Template_InputFormControls>
			</wssuc:inputformsection>
        <sharepoint:formdigest id="FormDigest1" runat="server" />
    </table>
</asp:Content>
