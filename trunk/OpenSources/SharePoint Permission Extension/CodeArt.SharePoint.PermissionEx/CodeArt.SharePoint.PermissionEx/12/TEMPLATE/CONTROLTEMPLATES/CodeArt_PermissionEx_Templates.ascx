<%@ Control Language="C#"   AutoEventWireup="false" %>
<%@Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.WebControls"%>
<%@Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.Utilities"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>

<%@ Register Assembly="CodeArt.SharePoint.PermissionEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f" 
Namespace="CodeArt.SharePoint.PermissionEx" TagPrefix="codeArt" %>
<SharePoint:RenderingTemplate ID="ListForm" runat="server">
	<Template>
		<SPAN id='part1'>
		
			<SharePoint:InformationBar runat="server"/>
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbltop" RightButtonSeparator="&nbsp;" runat="server">
					<Template_RightButtons>
						<SharePoint:NextPageButton runat="server"/>
						<SharePoint:SaveButton runat="server"/>
						<SharePoint:GoBackButton runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			<SharePoint:FormToolBar runat="server"/>
			<TABLE class="ms-formtable" style="margin-top: 8px;" border=0 cellpadding=0 cellspacing=0 width=100%>
			<SharePoint:ChangeContentType runat="server"/>
			<SharePoint:FolderFormFields runat="server"/>			
 
            <codeArt:EditControlListFieldIterator runat="server"/>

			<SharePoint:ApprovalStatus runat="server"/>
			<SharePoint:FormComponent TemplateName="AttachmentRows" runat="server"/>
			</TABLE>
			<table cellpadding=0 cellspacing=0 width=100%><tr><td class="ms-formline"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td></tr></table>
			<TABLE cellpadding=0 cellspacing=0 width=100% style="padding-top: 7px"><tr><td width=100%>
			<SharePoint:ItemHiddenVersion runat="server"/>
			<SharePoint:ParentInformationField runat="server"/>
			<SharePoint:InitContentType runat="server"/>
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbl" RightButtonSeparator="&nbsp;" runat="server">
					<Template_Buttons>
						<SharePoint:CreatedModifiedInfo runat="server"/>
					</Template_Buttons>
					<Template_RightButtons>
						<SharePoint:SaveButton runat="server"/>
						<SharePoint:GoBackButton runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</td></tr></TABLE>
		</SPAN>
 
		<SharePoint:AttachmentUpload runat="server"/>
<hr/>
	</Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="FileFormFields" runat="server">
	<Template>
			<codeArt:EditControlListFieldIterator runat="server"/>
	</Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="ViewSelector" runat="server">
	<Template>
		<table border=0 cellpadding=0 cellspacing=0 style='margin-right: 4px'>
		<tr>
		   <td nowrap class="ms-listheaderlabel"><SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,view_selector_view%>" EncodeMethod='HtmlEncode'/>&nbsp;</td>
		   <td nowrap class="ms-viewselector" id="onetViewSelector" onmouseover="this.className='ms-viewselectorhover'" onmouseout="this.className='ms-viewselector'" runat="server">
				<codeArt:PermissionListViewSelector MenuAlignment="Right" AlignToParent="true" runat="server" id="ViewSelectorMenu" />
			</td>
		</tr>
		</table>
	</Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="DocumentLibraryViewToolBar" runat="server">
	<Template>
		<wssuc:ToolBar CssClass="ms-menutoolbar" EnableViewState="false" id="toolBarTbl" ButtonSeparator="<img src='/_layouts/images/blank.gif' alt=''>" RightButtonSeparator="&nbsp;&nbsp;" runat="server">
			<Template_Buttons>
			<codeart:NewMenuWithPermission  runat="server" AccessKey="<%$Resources:wss,tb_NewMenu_AK%>"/>
				<%--<SharePoint:NewMenu ID="NewMenu1" Visible="false" AccessKey="<%$Resources:wss,tb_NewMenu_AK%>" runat="server"/>--%>
				<SharePoint:UploadMenu ID="UploadMenu1" AccessKey="<%$Resources:wss,tb_UploadMenu_AK%>" runat="server"/>
				<SharePoint:ActionsMenu ID="ActionsMenu1" AccessKey="<%$Resources:wss,tb_ActionsMenu_AK%>" runat="server"/>
				<SharePoint:SettingsMenu ID="SettingsMenu1" AccessKey="<%$Resources:wss,tb_SettingsMenu_AK%>" runat="server"/>
			</Template_Buttons>
			<Template_RightButtons>
				  <SharePoint:PagingButton ID="PagingButton1" runat="server"/>
				  <SharePoint:ListViewSelector ID="ListViewSelector1" runat="server"/>
			</Template_RightButtons>
		</wssuc:ToolBar>
	</Template>
</SharePoint:RenderingTemplate>s

<SharePoint:RenderingTemplate ID="ViewToolBar" runat="server">
	<Template>
		<wssuc:ToolBar CssClass="ms-menutoolbar" EnableViewState="false" id="toolBarTbl" ButtonSeparator="<img src='/_layouts/images/blank.gif' alt=''>" RightButtonSeparator="&nbsp;&nbsp;" runat="server">
			<Template_Buttons>
				<codeart:NewMenuWithPermission  runat="server" AccessKey="<%$Resources:wss,tb_NewMenu_AK%>"/>
				<%--<SharePoint:NewMenu AccessKey="<%$Resources:wss,tb_NewMenu_AK%>" runat="server" />--%>
				<SharePoint:ActionsMenu AccessKey="<%$Resources:wss,tb_ActionsMenu_AK%>" runat="server" />
				<SharePoint:SettingsMenu AccessKey="<%$Resources:wss,tb_SettingsMenu_AK%>" runat="server" />
			</Template_Buttons>
			<Template_RightButtons>
				  <SharePoint:PagingButton runat="server"/>
				  <SharePoint:ListViewSelector runat="server"/>
			</Template_RightButtons>
		</wssuc:ToolBar>
	</Template>
</SharePoint:RenderingTemplate>