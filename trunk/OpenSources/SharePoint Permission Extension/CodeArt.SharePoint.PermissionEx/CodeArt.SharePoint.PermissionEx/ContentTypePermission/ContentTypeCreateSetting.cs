//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-3-21
//
//修订记录: 
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

using Microsoft.SharePoint;
 

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    /// 内容类型创建权限存储类
    /// </summary>
    [Serializable]
    public class ContentTypeCreateSetting
    {
        //public string ContentTypeId;

        public string ContentTypeName;

        public string SpecialAccounts;

        public string SpecialGroups;

        private StringCollection _GroupCollections;

        public bool IsInSpecialAccounts(string account)
        {
            if (String.IsNullOrEmpty(SpecialAccounts))
                return false;

            string checkList = "," + this.SpecialAccounts.ToLower() + ",";

            return checkList.IndexOf("," + account.ToLower() + ",") != -1;
        }

        public bool IsInSpecialGroups(SPUser currentUser)
        {
            if (String.IsNullOrEmpty(SpecialGroups))
                return false;

            if (_GroupCollections == null)
            {
                _GroupCollections = new StringCollection();
                _GroupCollections.AddRange(SpecialGroups.Split(','));
            }

            foreach (SPGroup g in currentUser.Groups)
            {
                if (_GroupCollections.Contains(g.Name))
                    return true;
            }

            return false;
        }

        public bool CanCreate(SPUser currentUser)
        {
            if (currentUser != null && currentUser.IsSiteAdmin) return true;

            if (String.IsNullOrEmpty(this.SpecialAccounts) && String.IsNullOrEmpty(this.SpecialGroups))
                return true;

            if (currentUser == null)
            {
                return false;
            }

            // if (this.AllUserCanEdit) return true ;

            //if (this.CreatorCanEdit && String.Compare(currentUser.LoginName, creatUser.LoginName, true) == 0)
            //    return true;

            bool inAccounts = this.IsInSpecialAccounts(currentUser.LoginName);

            if (inAccounts)
                return true;

            return this.IsInSpecialGroups(currentUser);
        }
    }

    /// <summary>
    /// 列表内容类型创建权限存储类
    /// </summary>
    [Serializable]
    public class ListContentTypesCreateSetting : List<ContentTypeCreateSetting> // Dictionary<string,FieldEditSetting>
    {
        public const string Config_List = "__CodeArt_ListPermissionExtension";

        public void Save(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(Config_List);
            cmg.SetConfigData(list.ID.ToString().ToUpper() + "_ContentTypes", this);
        }

        public string GetContentTypeCreateGroups(string cName)
        {
            foreach (ContentTypeCreateSetting fSetting in this)
            {
                if (String.Compare(fSetting.ContentTypeName, cName, true) == 0)
                    return fSetting.SpecialGroups;
            }

            return "";
        }

        /// <summary>
        /// 获取列表中某个类型的权限配置类
        /// </summary>
        /// <param name="cName"></param>
        /// <returns></returns>
        public ContentTypeCreateSetting GetContentTypeCreateSetting(string cName)
        {
            foreach (ContentTypeCreateSetting fSetting in this)
            {
                if (String.Compare(fSetting.ContentTypeName, cName, true) == 0)
                    return fSetting;
            }

            return null;
        }

        /// <summary>
        /// 判断用户对某个内容类型是否有创建权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="contentTypeName"></param>
        /// <returns></returns>
        public bool CheckRight(SPUser user, string contentTypeName)
        {
            ContentTypeCreateSetting set = this.GetContentTypeCreateSetting(contentTypeName);

            if (set == null)
                return true;

            return set.CanCreate(user);
        }

        public static ListContentTypesCreateSetting GeSetting(SPList list)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(Config_List);

            ListContentTypesCreateSetting setting = cmg.GetConfigData<ListContentTypesCreateSetting>(list.ID.ToString().ToUpper() + "_ContentTypes");

            return setting;
        }
    }

}
