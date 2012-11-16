<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSettingsUserControl.ascx.cs" Inherits="Visigo.Sharepoint.FormsBasedAuthentication.UserSettings.UserSettingsUserControl" %>
<style type="text/css">
    .col-right
    {
        text-align: right;
    }
</style>
<asp:UpdatePanel runat="server" ID="UpdatePanelUserSettings" ChildrenAsTriggers="true">
<ContentTemplate>
<table style="width:600px">
    <tr>
        <td width="100px" class="col-right">
            <b>User Name</b>
        </td>
        <td>
            <asp:Label ID="lblUserName" runat="server" Width="100%"></asp:Label>
        </td>
    </tr>
	<tr>
		<td class="col-right">
            <b>Full Name</b>
        </td>
		<td>
            <asp:TextBox ID="txtFullName" runat="server" Width="100%"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="col-right">
            <b>Email Address</b>
        </td>
		<td>
            <asp:TextBox ID="txtEmailAddress" runat="server" Width="100%"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="col-right">
            <b>Picture</b>
        </td>
		<td>
            <asp:Image ID="imgPicture" ImageUrl="/_layouts/FBA/Images/NO_PIC.GIF" runat="server" />
            <br />
            <asp:FileUpload ID="filePicture" runat="server" Width="100%" />
		</td>
	</tr>
	<tr>
        <td class="col-right">&nbsp;</td>
		<td>
            <asp:Button ID="btnUpdate" runat="server" CssClass="btn_90" 
                onclick="btnUpdate_Click" OnClientClick="$('#ImageLoading').show();" 
                Text="Update" />
            <asp:Literal ID="Message" runat="server"></asp:Literal>
        </td>
	</tr>
</table>
</ContentTemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="btnUpdate" />
</Triggers>
</asp:UpdatePanel>