//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-1-14
//
//修订记录: 
//
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    /// 列表字段编辑权限设置存储类
    /// </summary>
    [Serializable]
    public class ListFieldPermissionSetting : List<FieldPermission> // Dictionary<string,FieldEditSetting>
    {
        public const string Config_List = "__CodeArt_ListPermissionExtension";

        public FieldPermission GetByFieldName(string fieldName)
        {
            foreach (FieldPermission fSetting in this)
            {
                if (String.Compare(fSetting.FieldName, fieldName, true) == 0)
                    return fSetting;
            }

            return null;
        }

        public void Save(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(Config_List);
            cmg.SetConfigData( list.ID.ToString().ToUpper() + "_Fields" , this);
        }

        public static ListFieldPermissionSetting GetListSetting(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(Config_List);

            ListFieldPermissionSetting setting = cmg.GetConfigData<ListFieldPermissionSetting>(list.ID.ToString().ToUpper() + "_Fields");

            return setting;
        }
    }


}
