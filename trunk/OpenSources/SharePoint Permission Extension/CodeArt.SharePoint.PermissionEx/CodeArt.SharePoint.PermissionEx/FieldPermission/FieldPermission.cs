//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-1-14
//
//修订记录: 
//修改人: andreeyang@163.com
//修改日期:2009-6-23
//修改内容:显示权限
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{
    [Serializable]
    public class FieldPermission
    {
        public string FieldName;

        public bool CreatorCanEdit;

        public bool AllUserCanEdit;

        public bool CreatorCanDisplay;

        public bool AllUserCanDisplay;

        public string SpecialAccounts;

        public string SpecialAccountsDisplay;

        public string SpecialGroups;

        public string SpecialGroupsDisplay;

        private string[] _GroupCollectionsDisplay;
        private string[] _GroupCollectionsEdit;

        public bool IsInSpecialAccounts(string account)
        {
            if (String.IsNullOrEmpty(SpecialAccounts))
                return false;

            string checkList = "," + this.SpecialAccounts.ToLower() + ",";

            return checkList.IndexOf("," + account.ToLower() + ",") != -1;
        }
        public bool IsInSpecialDisplayAccounts(string account)
        {
            if (String.IsNullOrEmpty(SpecialAccountsDisplay))
                return false;

            string checkList = "," + this.SpecialAccountsDisplay.ToLower() + ",";

            return checkList.IndexOf("," + account.ToLower() + ",") != -1;
        }

        public bool IsInSpecialGroups(SPUser currentUser)
        {
            if (String.IsNullOrEmpty(SpecialGroups))
                return false;

            return SPContext.Current.Web.IsCurrentUserInGroups(SpecialGroups.Split(','));

            //if (_GroupCollectionsEdit == null)
            //{
            //    _GroupCollectionsEdit = SpecialGroups.Split(',');
            //}

            //foreach (SPGroup g in currentUser.Groups)
            //{
            //    if (_GroupCollectionsEdit.Contains(g.Name))
            //        return true;
            //}

            ////check by group for DomainGroup           
            //foreach (string g in _GroupCollectionsEdit)
            //{
            //    var group = SPContext.Current.Web.Groups[g];
            //    if (group.ContainsCurrentUser)
            //        return true;
            //}

            return false;
        }
        public bool IsInSpecialDisplayGroups(SPUser currentUser)
        {
            if (String.IsNullOrEmpty(SpecialGroupsDisplay))
                return false;

            return SPContext.Current.Web.IsCurrentUserInGroups(SpecialGroupsDisplay.Split(','));

            //if (_GroupCollectionsDisplay == null)
            //{
            //    _GroupCollectionsDisplay = (SpecialGroupsDisplay.Split(','));
            //}

            //foreach (SPGroup g in currentUser.Groups)
            //{
            //    if (_GroupCollectionsDisplay.Contains(g.Name))
            //        return true;
            //}

            ////check by group for DomainGroup           
            //foreach (string g in _GroupCollectionsDisplay)
            //{
            //    var group = SPContext.Current.Web.Groups[g];
            //    if (group.ContainsCurrentUser)
            //        return true;
            //}

            return false;
        }

        public bool CanEdit(SPUser currentUser, SPUser creatUser)
        {
            if (this.AllUserCanEdit) return true;

            if (currentUser != null && currentUser.IsSiteAdmin) return true;

            if (currentUser == null)
            {
                return false;
            }

            if (creatUser!= null && this.CreatorCanEdit && String.Compare(currentUser.LoginName, creatUser.LoginName, true) == 0)
                return true;

            bool inAccounts = this.IsInSpecialAccounts(currentUser.LoginName);

            if (inAccounts)
                return true;

            return this.IsInSpecialGroups(currentUser);
        }

        public bool CanDisplay(SPUser currentUser, SPUser creatUser)
        {
            if (currentUser!=null && currentUser.IsSiteAdmin) return true;

            if (this.AllUserCanDisplay) return true;

            if (currentUser == null)
            {
                return false;
            }

            if (creatUser != null && this.CreatorCanDisplay && String.Compare(currentUser.LoginName, creatUser.LoginName, true) == 0)
              return true;

            bool inAccounts = this.IsInSpecialDisplayAccounts(currentUser.LoginName);

            if (inAccounts)
                return true;

            return this.IsInSpecialDisplayGroups(currentUser);
        }
    }
}
