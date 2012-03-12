using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx.AppPages
{
    public partial class ContentTypePermissionSetting : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreInit(EventArgs e)
        {
            this.MasterPageFile = SPContext.Current.Web.MasterUrl;
            base.OnPreInit(e);
        }
    }
}
