using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CSSoft.SharePointSnippet.WebpartConfig
{
    public partial class WebpartConfigUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBoxWebpartProperty.Text = Request.RawUrl;
        }
    }
}
