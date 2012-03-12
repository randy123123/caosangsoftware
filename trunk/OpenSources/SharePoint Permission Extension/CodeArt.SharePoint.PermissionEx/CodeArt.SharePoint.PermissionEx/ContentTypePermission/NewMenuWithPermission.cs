//----------------------------------------------------------------
//Code Art.
//
//�ļ�����:
//
//�� �� ��: jianyi0115@163.com
//��������: 2008-3-21
//
//�޶���¼: 
//
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    /// ���б��½���ť���Ȩ�޿��ơ������û������½���Щ��������
    /// �ô˿ؼ��滻Ĭ�ϵ�NewMenu��
    /// </summary>
    public class NewMenuWithPermission : NewMenu
    {        

        private ListContentTypesCreateSetting _setting;
        bool _settingExist = true ;

        bool UserHaveRight(string cName)
        {
            if (!_settingExist)
                return true;

            if (_setting == null)
            {
                _setting = ListContentTypesCreateSetting.GeSetting(base.List);
                _settingExist = _setting != null;
            }

            if (_setting == null)
                return true ;

            return _setting.CheckRight(SPContext.Current.Web.CurrentUser, cName);

            //return true;
        }


        public override MenuItemTemplate AddMenuItem(string id, string displayName, string imageUrl, string description, string navigateUrl, string onClickScript)
        {          
            MenuItemTemplate m = base.AddMenuItem(id, displayName, imageUrl, description, navigateUrl, onClickScript);

            m.Visible = this.UserHaveRight(displayName);

            return m;
        }

         
    }
}
