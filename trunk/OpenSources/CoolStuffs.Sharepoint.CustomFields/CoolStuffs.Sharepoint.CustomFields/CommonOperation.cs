using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CoolStuffs.Sharepoint.CustomFields
{
    public static class CommonOperation
    {
        public static SPList GetSPList(string listName,SPWeb web)
        {
            if (!string.Equals(listName.ToLower(), "user information list"))
            {
                return web.GetListFromUrl(listName);
            }
            else
            {
                return web.SiteUserInfoList;
            }

        }
    }
}
