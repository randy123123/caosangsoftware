//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: andreeyang@163.com
//创建日期: 2009-6
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeArt.SharePoint.PermissionEx;
using Microsoft.SharePoint;
using CodeArt.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{


    /// <summary>
    /// 列表字段编辑权限设置存储类
    /// </summary>
    [Serializable]
    public class ListViewPermissionSetting : List<ViewPermission> 
    {
        public const string Config_List = "__CodeArt_ListPermissionExtension";


        //public static ListViewPermissionSetting GetViewPermissionSetting(SPList list)
        //{
        //    ConfigManager cmg = ConfigManager.GetConfigManager(ListViewPermissionSetting.Config_List);

        //    ListViewPermissionSetting setting = cmg.GetConfigData<ListViewPermissionSetting>( list.ID.ToString().ToUpper() + "_Views");

        //    return setting;
        //}

        public ViewPermission GetByViewName(string ViewName)
        {
            foreach (ViewPermission fSetting in this)
            {
                if (String.Compare(fSetting.ViewName, ViewName, true) == 0)
                    return fSetting;
            }

            return null;
        }

        public ViewPermission GetByViewID( Guid id )
        {
            foreach (ViewPermission fSetting in this)
            {
                if ( fSetting.ViewID == id )
                    return fSetting;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">listid+"view" </param>
        public void Save(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(ListViewPermissionSetting.Config_List);
            cmg.SetConfigData(list.ID.ToString().ToUpper() + "_Views", this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">listid+"view"</param>
        /// <returns></returns>
        public static ListViewPermissionSetting GetListSetting(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(ListViewPermissionSetting.Config_List);

            ListViewPermissionSetting setting = cmg.GetConfigData<ListViewPermissionSetting>(list.ID.ToString().ToUpper() + "_Views");

            return setting;
        }

        public Guid GetCanDisplayView(SPUser user)
        {
            foreach (ViewPermission vp in this)
            {
                if (vp.CanDisplay(user))
                    return vp.ViewID;
            }
            return Guid.Empty;
        }
    }

}
