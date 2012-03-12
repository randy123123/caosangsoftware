//----------------------------------------------------------------
//Code Art.
//
//�ļ�����:
//
//�� �� ��: jianyi0115@163.com
//��������: 2009-7-25
//
//�޶���¼: 
//�޸���:  
//�޸�����:  
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
    public class PermissionListViewSelector : ViewSelectorMenu // Microsoft.SharePoint.WebControls.ListViewSelector
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //if (!this.Context.User.Identity.IsAuthenticated)//for anonymous access
            //{
            //    return;
            //}

            var currentUser = base.Web.CurrentUser;

            if (currentUser != null && currentUser.IsSiteAdmin)
            {
                return;
            }

            SPList curList = SPContext.Current.List;

            ListViewPermissionSetting listSetting = ListViewPermissionSetting.GetListSetting(curList);
            if (listSetting == null)
            {
                return;
            }

            SPView currentView = SPContext.Current.ViewContext.View;
            //SPUser currentUser = SPContext.Current.Web.CurrentUser;

            ViewPermission viewSetting = listSetting.GetByViewID(currentView.ID);
         
            if (( viewSetting!= null) && (!(viewSetting.CanDisplay(currentUser))))
            {
                //����û�û��Ȩ�޷��ʵ�ǰ��ͼ����ô��Ҫ��������Ȩ�޷��ʵ���ͼ

                if (currentView.DefaultView) //�������Ĭ����ͼ��û��Ȩ��ʱ���Զ�ת��һ����Ȩ�޵���ͼ
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
                        SPUtility.Redirect( base.Web.Url +"/"+ view.Url, SPRedirectFlags.Default, this.Context);
                        return;
                    }
                }

                SPUtility.TransferToErrorPage(Util.GetResource("Msg_NoViewRight"));
            }
            
            //NND, ��2010��07UIģʽ���������ò������á� 
            foreach (Control item in base.MenuTemplateControl.Controls)
            {
                if ((item is MenuItemTemplate))
                {
                    MenuItemTemplate menuItem = (MenuItemTemplate)item;

                    if (menuItem.PermissionsString != "ViewListItems") //�޸���ͼ�ʹ�����ͼ�˵�
                    {
                        menuItem.Visible = false;
                        continue;
                    }

                    try
                    {
                        SPView tempView = curList.Views[menuItem.Text];

                        viewSetting = listSetting.GetByViewName(menuItem.Text);

                        if (viewSetting == null)
                            continue;

                        item.Visible = viewSetting.CanDisplay(currentUser);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }
 
       
        //protected override void CreateChildControls()
        //{
        //    SPUser currentUser = SPContext.Current.Web.CurrentUser ;
        //    SPList parentList = base.RenderContext.ViewContext.View.ParentList;

        //    DropDownList dl=new DropDownList();
            
        //    dl.AutoPostBack = true;
        //    dl.SelectedIndexChanged += new EventHandler(this.dl_SelectedIndexChanged);
            
        //    ConfigManager cmg = ConfigManager.GetConfigManager(ListViewPermissionSetting.Config_List);
        //    ListViewPermissionSetting setting=null;
        //    if (cmg != null)
        //    setting = cmg.GetConfigData<ListViewPermissionSetting>(parentList.ID.ToString()+"view");

        //    SPView currentView = SPContext.Current.ViewContext.View;
        //    if (cmg != null && setting != null)
        //    { 
        //        if ((setting.GetByViewName(currentView.Title)!=null)&&(!(setting.GetByViewName(currentView.Title).CanDisplay(currentUser))))
        //        {
        //                    SPUtility.TransferToErrorPage("��û��Ȩ�޷��ʴ���ͼ.");
        //        }

        //    }
        //    foreach (SPView view in parentList.Views)
        //    {
        //        if (setting == null)
        //        {
        //            dl.Items.Add(new ListItem(view.Title));
        //            continue;
        //        }
        //        if ( setting.Count == 0 )
        //        {
        //            dl.Items.Add(new ListItem(view.Title));
        //            continue;
        //        }

        //        if (currentUser.IsSiteAdmin||(setting.GetByViewName(view.Title)==null)||(setting.GetByViewName(view.Title).CanDisplay(currentUser)))
        //        dl.Items.Add(new ListItem(view.Title));
            
        //    }
        //    string url = System.Web.HttpContext.Current.Request.Url.ToString();
        //    int index = url.LastIndexOf('/'); 
        //    url = url.Substring(index + 1, url.Length - index - 1); 
        //    string currentViewName = url.Substring(0, url.Length - 5).Replace("%20"," ");
        //    //˽����ͼ
        //    if (url.Contains("{"))
        //    {
        //        int i = url.IndexOf('{');
        //        int j = url.LastIndexOf('}');
        //        string s = url.Substring(i, j - i + 1);
        //        Guid viewId = new Guid(s);

        //        //Guid viewId = new Guid(url.Substring(url.LastIndexOf('{'), url.Length - url.LastIndexOf('}')));
        //        currentViewName = parentList.GetView(viewId).Title;
        //    }
        //    else 
        //    {
        //        currentViewName = SPContext.Current.ViewContext.View.Title;

        //    }
        //    if (setting.GetByViewName(currentViewName) == null || currentUser.IsSiteAdmin)
        //    {
        //        dl.Items.Add(new ListItem("�޸Ĵ���ͼ"));
        //        dl.Items.Add(new ListItem("��������ͼ"));
        //        this.Controls.Add(dl);
        //        dl.SelectedValue = currentViewName;
        //        return;

        //    }

        //    if (setting.GetByViewName(currentViewName).CanEdit(currentUser) )
        //        dl.Items.Add(new ListItem("�޸Ĵ���ͼ"));

        //    if (setting.GetByViewName(currentViewName).CanDisplay(currentUser) )
        //        dl.Items.Add(new ListItem("��������ͼ"));

            
        //    this.Controls.Add(dl);
        //    dl.SelectedValue = currentViewName;
            
            

        //}
 
        
    }
}
