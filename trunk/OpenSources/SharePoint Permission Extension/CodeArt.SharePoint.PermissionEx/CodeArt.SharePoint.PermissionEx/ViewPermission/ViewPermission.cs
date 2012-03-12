//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: andreeyang@163.com
//创建日期: 2008-1-19
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
using CodeArt.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{

    [Serializable]
    public class ViewPermission
    {
        public string ViewName;

        public Guid ViewID;

        public string SpecialAccounts;

        //public string SpecialAccountsDisplay;
        public string SpecialGroups;

        //public string SpecialGroupsDisplay;

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

            return SPContext.Current.Web.IsCurrentUserInGroups(SpecialGroups.Split(','));

            //if (_GroupCollections == null)
            //{
            //    _GroupCollections = new StringCollection();
            //    _GroupCollections.AddRange(SpecialGroups.Split(','));
            //}

            //foreach (SPGroup g in currentUser.Groups)
            //{
            //    if (_GroupCollections.Contains(g.Name))
            //        return true;
            //}

            return false;
        }
       
        //public bool CanEdit(SPUser currentUser)
        //{
        //    if (currentUser.IsSiteAdmin) return true;


        //    bool inAccounts = this.IsInSpecialAccounts(currentUser.LoginName);

        //    if (inAccounts)
        //        return true;

        //    return this.IsInSpecialGroups(currentUser);
        //}

        public bool CanDisplay(SPUser currentUser)
        {
            if (String.IsNullOrEmpty(this.SpecialAccounts) && String.IsNullOrEmpty(this.SpecialGroups))
                return true;

            if (currentUser == null) // 匿名用户
            {
                return false;
            }        

            if (currentUser.IsSiteAdmin) return true;
           

            //if (String.Compare(currentUser.LoginName, creatUser.LoginName, true) == 0)
            //  return true;

            bool inAccounts = this.IsInSpecialAccounts(currentUser.LoginName);

            if (inAccounts)
                return true;

            return this.IsInSpecialGroups(currentUser);
        }
    }
}