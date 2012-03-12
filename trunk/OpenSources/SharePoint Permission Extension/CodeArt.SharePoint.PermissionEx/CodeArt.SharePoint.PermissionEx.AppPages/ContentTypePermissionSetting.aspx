<%@ Assembly Name="CodeArt.SharePoint.PermissionEx.AppPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f" %>
<%@ Page MasterPageFile="~/_Layouts/Application.Master"  Language="C#" AutoEventWireup="true" 
CodeBehind="ContentTypePermissionSetting.aspx.cs"
 Inherits="CodeArt.SharePoint.PermissionEx.AppPages.ContentTypePermissionSetting" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="/_controltemplates/InputFormSection.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="/_controltemplates/InputFormControl.ascx" %> 
<%@ Register Assembly="CodeArt.SharePoint.PermissionEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f"
 Namespace="CodeArt.SharePoint.PermissionEx" TagPrefix="codeArt" %>

<script runat="server">
    
//----------------------------------------------------------------
//Code Art .
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2009-7-26
// 
//修订记录: 
//
//----------------------------------------------------------------
    
     
</script>


<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
<%=GetResource("CTPermissionSetting")%>
 </asp:Content>

 
  <asp:Content ID="Content2" contentplaceholderid="PlaceHolderPageDescription" runat="server">
<%=GetResource("CTPer_SubTitle")%>
 </asp:Content>
 
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderMain" runat="server">  

<style>
.ms-authoringcontrols{ width:580px; }
</style>

	<table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
  
			<wssuc:InputFormSection Title=""         
				Description=""
				runat="server">
				<Template_InputFormControls>
					<wssuc:InputFormControl runat="server">
						<Template_Control>
							 
							<codeArt:ContentTypesCreateRightSettingPart runat="server" id="fSetting2" />
							
						</Template_Control>
					</wssuc:InputFormControl>
				</Template_InputFormControls>
			</wssuc:InputFormSection>
 
 	 
			<SharePoint:FormDigest ID="FormDigest1" runat=server/>
		</table>

</asp:Content>