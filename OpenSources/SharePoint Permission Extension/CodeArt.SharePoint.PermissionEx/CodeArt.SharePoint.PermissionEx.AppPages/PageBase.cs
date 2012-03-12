using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;

namespace CodeArt.SharePoint.PermissionEx.AppPages
{
    public class PageBase : LayoutsPageBase
    {
        protected override void OnPreRender(EventArgs e)
        {
            if (SPFarm.Local.BuildVersion.Major == 14)
            {
                CssRegistration.Register("forms.css");
            }
            base.OnPreRender(e);
        }

        public string GetResource(string key)
        {
            return Util.GetResource(key);
        }

  
    }
}