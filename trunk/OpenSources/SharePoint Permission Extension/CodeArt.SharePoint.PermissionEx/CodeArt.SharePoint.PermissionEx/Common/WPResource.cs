//----------------------------------------------------------------
//Code Art.
//
//�ļ�����:
//
//�� �� ��: jianyi0115@163.com
//��������: 2007-12-25
//
//�޶���¼: 
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    /// ��Դ�����࣬����App_GlobalResources �µ�mcswp.resx��Դ�ļ�
    /// </summary>
    public static class WPResource
    {
        const string resource = "codeartPermissionEx";
        //�˴�Ϊ��Դ�ļ�������Щ��Դ�ļ�����App_GlobalResources,��wss.resx��wss.zh-CN.resx�Ĺ�������
        //wssΪϵͳĬ�ϵ���Դ�ļ����������myresource.resx,myresource.zh_CN.resx������

        /// <summary>
        /// ��ȡ��Դ�ַ�����ֵ
        /// </summary>
        /// <param name="key">��Դ�ַ���key</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            try
            {
                string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

                if (value == null || value == "")
                    return key;
                else
                    return value;
            }
            catch (Exception ex)
            {
                throw new SPException("access resource file [" + resource + "] error,please confirm files in App_GlobalResources.", ex);
            }
        }

        /// <summary>
        /// ��ȡ��Դ�ַ�����ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">key������ʱ��Ĭ��ֵ</param>
        /// <returns></returns>
        public static string GetString(string key,string defaultValue)
        {
            try
            {
                string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

                if (value == null || value == "")
                    return String.IsNullOrEmpty( defaultValue ) ? key : defaultValue  ;
                else
                    return value;
            }
            catch (Exception ex)
            {
                throw new SPException("access resource file [" + resource + "] error,please confirm files in App_GlobalResources.", ex);
            }
        }

        public static string GetString(string key , params string[] args )
        { 
            string value = HttpContext.GetGlobalResourceObject(resource, key) as string;

            if (value == null || value == "")
                return key;
            else
                return String.Format( value , args );
        }
    }

    /// <summary>
    /// ���Է��࣬����Դ�л�ȡ
    /// </summary>
    public class ResCategoryAttribute : CategoryAttribute
    {
        public ResCategoryAttribute(string key)
            : base(key)
        { }

         private string _DefaultValue;
        public ResCategoryAttribute(string key, string defaultValue)
            : base(key)
        {
            _DefaultValue = defaultValue;
        }

        protected override string GetLocalizedString(string value)
        {
            return WPResource.GetString(value,_DefaultValue);
        }
    }

    /// <summary>
    /// ������ʾ��������Դ�л�ȡ
    /// </summary>
    public class ResWebDisplayNameAttribute : WebDisplayNameAttribute
    {
        public ResWebDisplayNameAttribute(string key)
            : base(key)
        { }

        private string _DefaultValue;
        public ResWebDisplayNameAttribute(string key,string defaultValue)
            : base(key)
        {
            _DefaultValue = defaultValue;
        }


        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName,_DefaultValue);
            }
        }
    }


    public class ResConnectionProviderAttribute : ConnectionProviderAttribute
    {
        public ResConnectionProviderAttribute(string name) : base(name)
        {
        }

        public ResConnectionProviderAttribute(string name,string id) : base(name,id)            
        {
        }

        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName);
            }
        }
    }

    public class ResConnectionConsumerAttribute : ConnectionConsumerAttribute
    {
        public ResConnectionConsumerAttribute(string name) : base(name)
        {
        }

        public ResConnectionConsumerAttribute(string name, string id)
            : base(name, id)            
        {
        }

        public override string DisplayName
        {
            get
            {
                return WPResource.GetString(base.DisplayName);
            }
        }
    }
}
