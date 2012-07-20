<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendMailToCustomersUserControl.ascx.cs" Inherits="CSSoft.EmailMarketing.SendMailToCustomersUserControl" %>
Select email template: <br />
<asp:DropDownList ID="EmailTemplate" runat="server" 
    onselectedindexchanged="EmailTemplate_SelectedIndexChanged" AutoPostBack="true" />
<asp:ImageButton ID="AddNewTemplate" OnClientClick="return false;" 
    runat="server" ImageUrl="/_layouts/images/GMAILNEW.GIF" Visible="false" 
    ToolTip="Add new email template" />
<br />
<asp:Panel ID="ActionPanel" runat="server">
Action:<br />
<asp:ImageButton ID="ImageButtonStartReview" runat="server" 
    ImageUrl="/_layouts/images/ltslidelibrary.PNG" ToolTip="Review your mail" 
    Width="28px" onclick="ImageButtonStartReview_Click" />
&nbsp;<asp:ImageButton ID="ImageButtonStartSendMail" runat="server" 
    ImageUrl="/_layouts/images/LTDISC.PNG" ToolTip="Send mail" Width="28px" 
    onclick="ImageButtonStartSendMail_Click" />
</asp:Panel>

<asp:Literal ID="Msg" runat="server"></asp:Literal>

<asp:Panel ID="ReviewMail" runat="server" Visible="false">
<hr />
<b>Subject:</b> <asp:Literal ID="Subject" runat="server"></asp:Literal><br />
<b>Body:</b><br />
<asp:Literal ID="Body" runat="server"></asp:Literal>
</asp:Panel>