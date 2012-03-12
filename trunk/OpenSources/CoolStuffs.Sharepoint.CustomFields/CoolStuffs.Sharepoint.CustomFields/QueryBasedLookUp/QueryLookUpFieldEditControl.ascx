<%@ Control Language="C#" Inherits="CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.FieldControllers.QueryLookUpFieldEditControl, CoolStuffs.Sharepoint.CustomFields, Version=1.0.0.0, Culture=neutral, PublicKeyToken=968ece0422c73ea5"   AutoEventWireup="false" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>                                                            
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<wssuc:InputFormSection runat="server" id="MySections1" Title="Query Based LookUp Details">
        <Template_Description>
            <table>
                <tr>
                    <td>
                        <img src="/_layouts/images/CoolStuffs/collaborate.GIF" alt=""/>
                    </td>
                    <td>
                        <b>Usage Description : </b><br /><br />
                    </td>
                </tr>
            </table>
            <b>Step1 : </b>Type in the Site URL you want to load list from, and click the button "Load List"<br />
            <b>Step2 : </b>Select a List from the Drop Down to Load List Columns<br />
            <b>Step3 : </b>Select Display Column text and value from the respective drop down<br />
            <b>Step4 : </b>select FilterColumn, assignment type, and filter value from respective DropDowns and click button "Build Query" to form one filtering block<br />
            <b>Step5 : </b>To add one more filtering condition, select from And/Or DropDown and follow Step4<br />
            <b>Step6 : </b>To Check for presence/absence of values, select the filtercolumn from the DropDown, and select a Value from Null/notNull DropDown<br />
            <b>Step7 : </b>To Keep Track of your query, check on "View my query"<br />
        </Template_Description>
    <Template_InputFormControls>
        <wssuc:InputFormControl runat="server"
                    LabelText="Enter a sharepoint site URL and click Load Lists Button to Get the LookUp List">
                    <Template_Control>
                        <asp:Label ID="lblSiteURL" runat="server" Text="Site URL" Width="120px">
                        </asp:Label>                           
                        <br />
                        <asp:TextBox ID="txtSiteURL" runat="server"  Width="350px" Text="">
                        </asp:TextBox>  
                        <br />
                        <asp:Button ID="btnLoadLists" runat="server" Text="Load Lists" />                              
                        <br />
                        <asp:Label ID="lblLookUpListName" runat="server" Text="List Name" Width="120px" >
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlLookUpListName" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblLookUpDisplayColumnText" runat="server" Text="Display Column of LookUp DropDown" Width="200px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlLookUpDisplayColumnText" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblLookUpDisplayColumnValue" runat="server" Text="Value Column of LookUp DropDown" Width="200px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlLookUpDisplayColumnValue" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <b>Query Builder Section : </b><br/>
                        <asp:Label ID="lblFilterOnColumn" runat="server" Text="Column to filter LookUp On" Width="200px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlFilterOnColumn" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblFilterAssignmentType" runat="server" Text="Type of Operator" Width="200px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlFilterAssignmentType" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblFilterValue" runat="server" Text="Value to filter LookUp On" Width="200px">
                        </asp:Label>
                        <br />
                        <asp:TextBox ID="txtFilterValue" runat="server"  Width="350px" Text="">
                        </asp:TextBox>
                        <br />
                        <table style="font-size:small;">
                        <tr>
                        <td>
                        <span style="font-size:x-small;">And/Or Clause</span>
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlAndOrStatement" runat="server" Width="100px" >
                        </asp:DropDownList>
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <span style="font-size:x-small;">Null/Not Null</span>
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlIsNull" runat="server" Width="100px" >
                        </asp:DropDownList>
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <span style="font-size:x-small;">Current User/Time</span>
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlDynamicValues" runat="server" Width="100px" >
                        </asp:DropDownList>
                        </td>
                        </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="lblViewMyQuery" BackColor="DarkSeaGreen" runat="server" Text="View your query" Width="200px" onmouseover="showToolTip(event,this.title);return false" onmouseout="hideToolTip()">
                        </asp:Label>
                        <br /> <br />
                        <asp:Button ID="btnBuildQuery" runat="server" Text="Build Query" />&nbsp;                            
                        <asp:Button ID="btnClearQuery" runat="server" Text="Clear Query" />                              
                        <br />
                    </Template_Control>
        </wssuc:InputFormControl>
        <div id="bubble_tooltip" style="left: 400px; top: 55px">
	        <div class="bubble_top"><span></span></div>
	        <div class="bubble_middle"><span id="bubble_tooltip_content">Content is comming here as you probably can see.Content is comming here as you probably can see.</span></div>
	        <div class="bubble_bottom"></div>
        </div>
        <style type="text/css">
	body{
		background-image:url('/_layouts/images/heading3.gif');
		background-repeat:no-repeat;
		padding-top:85px;	
		font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;
		font-size:0.9em;
		line-height:130%;

	}
	a{
		color: #D60808;
		text-decoration:none;
	}
	a:hover{
		border-bottom:1px dotted #317082;
		color: #307082;
	}
	</style>
	<link rel="stylesheet" href="/_layouts/images/bubble-tooltip.css" media="screen" />
	<script type="text/javascript">
	function showToolTip(e,text){
	if(document.all)e = event;
	document.body.style.cursor='hand'
	var obj = document.getElementById('bubble_tooltip');
	var obj2 = document.getElementById('bubble_tooltip_content');
	obj2.innerHTML = text;
	obj.style.display = 'block';
	var st = Math.max(document.body.scrollTop,document.documentElement.scrollTop);
	if(navigator.userAgent.toLowerCase().indexOf('safari')>=0)st=0; 
	var leftPos = e.clientX - 100;
	if(leftPos<0)leftPos = 0;
	obj.style.left = leftPos + 'px';
	obj.style.top = e.clientY - obj.offsetHeight -1 + st + 'px';
    }	

    function hideToolTip()
    {
        document.body.style.cursor='default'
	    document.getElementById('bubble_tooltip').style.display = 'none';
    }
	</script>
    </Template_InputFormControls>
</wssuc:InputFormSection>
