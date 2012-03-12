<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<script type="text/C#" runat="server">
		void Page_Load(object sender, EventArgs e)
		{

		}
	    
		void GetDate(object sender, CommandEventArgs e)
		{
			DateText.Text = "You've picked " + GMDatePicker1.Date.ToShortDateString();
		}
	</script>
	
    <title>GMDatePickerDemo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family: Arial;">
		<h2>GMDatePicker Demo</h2>
    	
		<asp:Button ID="ButtonGetDate" runat="server" CssClass="Button" OnCommand="GetDate" Text="GetDate" />
		<asp:Literal ID="DateText" runat="server"></asp:Literal>		
    </form>
</body>
</html>
