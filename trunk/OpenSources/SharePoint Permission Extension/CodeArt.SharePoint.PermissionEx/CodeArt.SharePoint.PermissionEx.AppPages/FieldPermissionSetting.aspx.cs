using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;

namespace CodeArt.SharePoint.PermissionEx.AppPages
{
    public partial class FieldPermissionSetting : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Desc = "XXX";
        }

        protected string Desc { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            this.MasterPageFile = SPContext.Current.Web.MasterUrl;
            base.OnPreInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.DataBind();
            base.OnPreRender(e);
        }
    }
}
