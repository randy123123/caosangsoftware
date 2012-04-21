<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserChangePasswordUserControl.ascx.cs" Inherits="CSSoft.CS2SPUsers.UserChangePassword.UserChangePasswordUserControl" %>
<style type="text/css">
    .PageWidthUserChangePassword{width: 400px;}
    .LableWidthUserChangePassword{width: 120px;}
    .FieldLable{background-color: #CCFFCC;}
    .Center{text-align: center;}
    .Right{text-align: right;}
    .Top{vertical-align: top;}
</style>
<table class="PageWidthUserChangePassword">
<tr>
<td class="LableWidthUserChangePassword FieldLable">User Name</td> <td><asp:TextBox ID="TextBoxUserName" runat="server" Enabled="False" Width="100%"></asp:TextBox></td>
</tr>
<tr>
<td class="FieldLable">User Login</td> <td><asp:TextBox ID="TextBoxUserLogin" runat="server" Enabled="False" Width="100%"></asp:TextBox></td>
</tr>
<tr>
<td class="FieldLable">Old Password</td> <td>
    <asp:TextBox ID="TextBoxOldPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
    </td>
</tr>
<tr>
<td class="FieldLable">New Password</td> <td>
    <asp:TextBox ID="TextBoxNewPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
    </td>
</tr>
<tr>
<td class="FieldLable">Confirm Password</td> <td>
    <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
    </td>
</tr>
<tr>
<td colspan="2" class="Center">
    <asp:Button ID="ButtonChange" runat="server" onclick="ButtonChange_Click" Text="Change" />
</td>
</tr>
</table>
<asp:Literal ID="Message" runat="server"></asp:Literal>