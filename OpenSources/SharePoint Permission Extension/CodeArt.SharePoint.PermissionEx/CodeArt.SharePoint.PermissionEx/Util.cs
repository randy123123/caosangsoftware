using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Threading;

namespace CodeArt.SharePoint.PermissionEx
{
    public static class Util
    {
        public static string GetResource(string key)
        {
            return SPUtility.GetLocalizedString("$Resources:," + key, "CodeartPermissionEx", (uint)(Thread.CurrentThread.CurrentUICulture.LCID));
        }
    }
}
