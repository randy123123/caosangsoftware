//----------------------------------------------------------------
//CodeArt
//
//文件描述:
//
//创 建 人: andreeyang@163.com
//创建日期: 2009-6
//
//修订记录: andreeyang@163.com
//修改日期 2009-7-9
//

//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    /// 添加此webpart到视图页面控制视图权限，用于自动控制失败的情况
    /// </summary>
    public class ViewPermissionControlPart : System.Web.UI.WebControls.WebParts.WebPart
    {       
        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
        }

        protected override void OnInit(EventArgs e)
        {
            this.Hidden = true;

            base.OnInit(e);        
            //this.Title = "";
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            if (currentUser.IsSiteAdmin)
                return;
            //base.ChromeType = PartChromeType.None;
                        
            SPWeb currenWeb = SPContext.Current.Web;
            //string url = System.Web.HttpContext.Current.Request.Url.ToString();
            //string listName = url.Replace(url.Substring(url.LastIndexOf("/"), url.Length - url.LastIndexOf("/")), "");
            //listName = listName.Replace(listName.Substring(0, listName.LastIndexOf("/") + 1), "");
            //int index = url.LastIndexOf('/');
            //url = url.Substring(index + 1, url.Length - index - 1);
            //string viewName = url.Substring(0, url.Length - 5).Replace("%20", " ").Substring(0,url.LastIndexOf("."));
            //if (viewName.Equals("AllItems"))
            //    viewName="所有项目";

            SPView currentView = SPContext.Current.ViewContext.View;
            if (currentView == null||currentView.ID == Guid.Empty)
                return;

            SPList curList = SPContext.Current.List;
    
            ListViewPermissionSetting listSetting = ListViewPermissionSetting.GetListSetting(curList);
            if (listSetting == null)
            {
                return;
            }

            var vp = listSetting.GetByViewID(currentView.ID);
            if (vp == null)
                return;

            if (vp.CanDisplay(currentUser))
                return;

            if (currentView.DefaultView) //如果访问默认视图而没有权限时，自动转向一个有权限的试图
            {
                Guid id = listSetting.GetCanDisplayView(currentUser);
                if (id == Guid.Empty)
                {
                    SPUtility.TransferToErrorPage(Util.GetResource("Msg_NoViewRight"));
                    return;
                }
                else
                {
                    SPView view = curList.Views[id];
                    SPUtility.Redirect(currenWeb.Url + "/" + view.Url, SPRedirectFlags.Default, this.Context);
                    return;
                }
            }

            SPUtility.TransferToErrorPage(Util.GetResource("Msg_NoViewRight"));        
        }
    }
}



















