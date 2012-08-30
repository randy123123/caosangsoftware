using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace ListViewFilter.Extensions
{
    internal static class WebControlsExtensions
    {
        internal static void SetValue(this DropDownList list, string value)
        {
            list.ClearSelection();
            var items = list.Items.Cast<ListItem>().Where(i => i.Value == value);
            if (items.Count() > 0)
            {
                items.First().Selected = true;
            }
        }

        internal static string LocalizedString(this Control list, string key)
        {
            var id = "$Resources:" + key;
            var lcid = SPContext.Current.Web.Language;
            return SPUtility.GetLocalizedString(id, "ListViewFilter", lcid);
        }

        internal static bool HasResolvedEntries(this PeopleEditor editor)
        {
            return editor.ResolvedEntities.Count > 0;
        }

        internal static void Clear(this PeopleEditor editor)
        {
            editor.ResolvedEntities.Clear();
            editor.Accounts.Clear();
            editor.Entities.Clear();
            editor.CommaSeparatedAccounts = null;
            editor.ErrorMessage = string.Empty;
        }

        internal static SPFieldUserValue UserValue(this PeopleEditor editor)
        {
            var res = new SPFieldUserValue();
            var ctx = SPContext.Current;
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    using (var site = new SPSite(ctx.Site.ID))
                    {
                        using (var web = site.OpenWeb(ctx.Web.ID))
                        {
                            editor.Validate();
                            if (editor.ResolvedEntities.Count <= 0)
                            {
                            }
                            else
                            {
                                var entity = editor.ResolvedEntities[0] as PickerEntity;
                                var id = 0;
                                switch (entity.EntityData["PrincipalType"].ToString())
                                {
                                    case "User":
                                        id = Convert.ToInt32(entity.EntityData["SPUserID"]);
                                        if (id <= 0)
                                        {
                                            var u = web.EnsureUser(entity.Key);
                                            id = u.ID;
                                        }
                                        break;
                                    case "SharePointGroup":
                                        id = Convert.ToInt32(entity.EntityData["SPGroupID"]);
                                        break;
                                }
                                res = new SPFieldUserValue(web, id, entity.Key);
                            }
                        }
                    }
                });
            return res;
        }

        internal static SPFieldUserValueCollection UserValueCollection(this PeopleEditor editor)
        {
            var res = new SPFieldUserValueCollection();
            var ctx = SPContext.Current;
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    using (var site = new SPSite(ctx.Site.ID))
                    {
                        using (var web = site.OpenWeb(ctx.Web.ID))
                        {
                            editor.Validate();
                            if (editor.ResolvedEntities.Count > 0)
                            {
                                foreach (PickerEntity entity in editor.ResolvedEntities)
                                {
                                    var id = 0;
                                    switch (entity.EntityData["PrincipalType"].ToString())
                                    {
                                        case "User":
                                            id = Convert.ToInt32(entity.EntityData["SPUserID"]);
                                            if (id <= 0)
                                            {
                                                var u = web.EnsureUser(entity.Key);
                                                id = u.ID;
                                            }
                                            break;
                                        case "SharePointGroup":
                                            id = Convert.ToInt32(entity.EntityData["SPGroupID"]);
                                            break;
                                    }
                                    res.Add(new SPFieldUserValue(web, id, entity.Key));
                                }
                            }
                        }
                    }
                });
            return res;
        }
    }
}
