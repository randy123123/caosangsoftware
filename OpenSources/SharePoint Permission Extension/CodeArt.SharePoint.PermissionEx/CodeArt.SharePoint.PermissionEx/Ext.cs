using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{
    static class Ext
    {
        public static bool IsCurrentUserInGroups(this SPWeb web, string[] groupNames)
        {
            var currentUser = web.CurrentUser;
            foreach (SPGroup g in currentUser.Groups)
            {
                if (groupNames.Contains(g.Name))
                    return true;
            }
            foreach (string g in groupNames)
            {
                var group = web.Groups[g];
                if (group.ContainsCurrentUser)
                    return true;
            }
            return false;
        }
    }
}
