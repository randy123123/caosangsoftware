﻿<%@ Control Language="C#" Debug="true"  %>
<%@Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.WebControls"%>
<SharePoint:RenderingTemplate ID="QueryLookUpFieldControl" runat="server">
    <Template>
        <asp:DropDownList ID="QueryLookUpDropDown" runat="server" />
    </Template>
</SharePoint:RenderingTemplate>