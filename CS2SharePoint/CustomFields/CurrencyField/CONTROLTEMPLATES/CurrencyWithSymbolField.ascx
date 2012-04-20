<%@ Control Language="C#" Debug="true" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" %>
<SharePoint:RenderingTemplate ID="CurrencyFieldControl" runat="server">
<Template>
    <asp:TextBox ID="CurrencyValue" runat="server" />
    <asp:DropDownList ID="CurrencySymbol" runat="server"/>
</Template>
</SharePoint:RenderingTemplate>