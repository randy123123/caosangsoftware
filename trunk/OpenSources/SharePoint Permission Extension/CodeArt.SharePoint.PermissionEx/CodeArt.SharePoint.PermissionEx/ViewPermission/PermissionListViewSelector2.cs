//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: andreeyang@163.com
//创建日期: 2009-6
//
//修订记录: 
//修改人: andreeyang@163.com
//修改日期: 2009-6-25
//
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Reflection;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;

namespace CodeArt.SharePoint.PermissionEx
{
    public class PermissionListViewSelector2 : Microsoft.SharePoint.WebControls.ListViewSelector
    {
        private void dl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drop = (DropDownList)sender;
            var value = drop.SelectedValue;
            SPView view = SPContext.Current.ViewContext.View;

            
            if (value.Equals("修改此视图"))
            {
                if (view.PersonalView)
                {
                    string s = System.Web.HttpContext.Current.Request.Url.ToString();
                    int i = s.IndexOf('{');
                    int j = s.LastIndexOf('}');
                    s = s.Substring(i, j - i + 1); 
                    Guid viewId = new Guid(s);
                    view = base.RenderContext.ViewContext.View.ParentList.GetView(viewId);
                    System.Web.HttpContext.Current.Response.Redirect(String.Format(SPContext.Current.Site.Url.ToString() + "/_layouts/ViewEdit.aspx?List={0}&View={1}&Source={2}", SPEncode.UrlEncode(view.ParentList.ID.ToString("B").ToUpper()), SPEncode.UrlEncode(view.ID.ToString("B")).ToUpper(), SPEncode.UrlEncode(SPEncode.UrlEncode(SPContext.Current.Web.Url + "/" + view.Url) + "?PageView=Personal&ShowWebPart={" + view.ID + "}")));
                }
                else
                    System.Web.HttpContext.Current.Response.Redirect(String.Format(SPContext.Current.Site.Url.ToString() + "/_layouts/ViewEdit.aspx?List={0}&View={1}&Source={2}", SPEncode.UrlEncode(view.ParentList.ID.ToString("B").ToUpper()), SPEncode.UrlEncode(view.ID.ToString("B")).ToUpper(), SPEncode.UrlEncode(SPEncode.UrlEncode(SPContext.Current.Web.Url + "/" + view.Url))));
                return;
            }
            if (value.Equals("创建新视图"))
            {
                System.Web.HttpContext.Current.Response.Redirect(SPContext.Current.Site.Url.ToString() + String.Format("/_layouts/ViewType.aspx?List={0}&Source={2}", SPEncode.UrlEncode(view.ParentList.ID.ToString("B")).ToUpper(), SPEncode.UrlEncode(view.ID.ToString("B")), SPEncode.UrlEncode(SPContext.Current.Web.Url +view.Url)));
                return;
            }

            SPView clickedview =base.RenderContext.ViewContext.View.ParentList.Views[value];
            string url = "";
            if (clickedview.PersonalView)
                url = SPContext.Current.Site.Url.ToString() + clickedview.ServerRelativeUrl.ToString() + "?PageView=Personal&ShowWebPart={"+clickedview.ID.ToString().ToUpper()+"}";
            else
                url = SPContext.Current.Site.Url.ToString() + "/" + clickedview.ServerRelativeUrl.ToString();


            //SPUtility.Redirect(url, SPRedirectFlags.Trusted, Context);
            System.Web.HttpContext.Current.Response.Redirect(url);
            
        }
/*        private SPView getCurrentView()
        {
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            int index = url.LastIndexOf('/');
            url = url.Substring(index + 1, url.Length - index - 1);
            string currentViewName = url.Substring(0, url.Length - 5).Replace("%20", " ");
            foreach (SPView view in base.RenderContext.ViewContext.View.ParentList.Views) 
            { 
            if (view.Url.ToString().Contains(currentViewName))
                return view;
            }
            return null;; 

        }
  */
        protected override void CreateChildControls()
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser ;
            SPList parentList = base.RenderContext.ViewContext.View.ParentList;

            DropDownList dl=new DropDownList();
            
            dl.AutoPostBack = true;
            dl.SelectedIndexChanged += new EventHandler(this.dl_SelectedIndexChanged);
            
            ConfigManager cmg = ConfigManager.GetConfigManager(ListViewPermissionSetting.Config_List);
            ListViewPermissionSetting setting=null;
            if (cmg != null)
            setting = cmg.GetConfigData<ListViewPermissionSetting>(parentList.ID.ToString()+"view");

            SPView currentView = SPContext.Current.ViewContext.View;
            if (cmg != null && setting != null)
            { 
                if ((setting.GetByViewName(currentView.Title)!=null)&&(!(setting.GetByViewName(currentView.Title).CanDisplay(currentUser))))
                {
                            SPUtility.TransferToErrorPage("您没有权限访问此视图.");
                }

            }
            foreach (SPView view in parentList.Views)
            {
                if (setting == null)
                {
                    dl.Items.Add(new ListItem(view.Title));
                    continue;
                }
                if ( setting.Count == 0 )
                {
                    dl.Items.Add(new ListItem(view.Title));
                    continue;
                }

                if (currentUser.IsSiteAdmin||(setting.GetByViewName(view.Title)==null)||(setting.GetByViewName(view.Title).CanDisplay(currentUser)))
                dl.Items.Add(new ListItem(view.Title));
            
            }
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            int index = url.LastIndexOf('/'); 
            url = url.Substring(index + 1, url.Length - index - 1); 
            string currentViewName = url.Substring(0, url.Length - 5).Replace("%20"," ");
            //私人视图
            if (url.Contains("{"))
            {
                int i = url.IndexOf('{');
                int j = url.LastIndexOf('}');
                string s = url.Substring(i, j - i + 1);
                Guid viewId = new Guid(s);

                //Guid viewId = new Guid(url.Substring(url.LastIndexOf('{'), url.Length - url.LastIndexOf('}')));
                currentViewName = parentList.GetView(viewId).Title;
            }
            else 
            {
                currentViewName = SPContext.Current.ViewContext.View.Title;

            }
            if (setting.GetByViewName(currentViewName) == null || currentUser.IsSiteAdmin)
            {
                dl.Items.Add(new ListItem("修改此视图"));
                dl.Items.Add(new ListItem("创建新视图"));
                this.Controls.Add(dl);
                dl.SelectedValue = currentViewName;
                return;

            }

            //if (setting.GetByViewName(currentViewName).CanEdit(currentUser) )
            //    dl.Items.Add(new ListItem("修改此视图"));

            //if (setting.GetByViewName(currentViewName).CanDisplay(currentUser) )
            //    dl.Items.Add(new ListItem("创建新视图"));

            
            this.Controls.Add(dl);
            dl.SelectedValue = currentViewName;
            
            

        }
 
        
    }
}
