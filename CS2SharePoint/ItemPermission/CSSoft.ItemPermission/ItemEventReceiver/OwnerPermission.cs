using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace CSSoft.ItemPermission.ItemEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class OwnerPermission : SPItemEventReceiver
    {
        /// <summary>
        /// Check permission of current user for action on item
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
       private bool CheckPermissionToAction(SPItemEventProperties properties)
       {
           SPUser user = properties.Web.SiteUsers[properties.UserLoginName];
           if (user.LoginName.ToLower().Equals("sharepoint\\system") || user.IsSiteAdmin || user.ID == new SPFieldUserValue(properties.Web, (string)properties.ListItem["Author"]).LookupId)
               return true;
           else
               return false;
       }

       /// <summary>
       /// An item is being updated.
       /// </summary>
       public override void ItemUpdating(SPItemEventProperties properties)
       {
           if (CheckPermissionToAction(properties))
           {
               base.ItemUpdating(properties);
           }
           else
           {
               properties.Cancel = true;
               properties.ErrorMessage = String.Format("Bạn không có quyền CẬP NHẬT dòng '{0}'!", properties.ListItem.Title);
           }
       }

       /// <summary>
       /// An item is being deleted.
       /// </summary>
       public override void ItemDeleting(SPItemEventProperties properties)
       {
           if (CheckPermissionToAction(properties))
           {
               base.ItemDeleting(properties);
           }
           else
           {
               properties.Cancel = true;
               properties.ErrorMessage = String.Format("Bạn không có quyền XÓA dòng '{0}'!", properties.ListItem.Title);
           }
       }


    }
}
