//----------------------------------------------------------------
//Code Art.
//
//�ļ�����:
//
//�� �� ��: jianyi0115@163.com
//��������: 2007-11-29
//
//�޶���¼: 
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
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.Utilities;
 
namespace CodeArt.SharePoint.PermissionEx 
{
    /// <summary>
    /// ��SPList������WebPart�Ļ���
    /// </summary>
    public class BaseSPListWebPart : BaseSPWebPart
    {
        private string _ListName;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_ListName")]
        //[ManagedLinkAttribute] //Microsoft.SharePoint.Publishing.WebControls.AssetUrlSelector
        public virtual string ListName
        {
            get
            {
                return _ListName;
            }
            set
            {
                _ListName = value;
            }
        }

        private SPView _CurrentView = null;
        /// <summary>
        /// �б�ĵ�ǰ��ͼ
        /// </summary>
        protected SPView CurrentView
        {
            get
            {
                if (_CurrentView != null)
                    return _CurrentView;

                if( this.ViewID != Guid.Empty )
                {
                     SPList list = this.GetCurrentSPList();

                     if (list != null)
                     {
                         try
                         {
                             _CurrentView = list.Views[this.ViewID];
                         }
                         catch (  ArgumentException ex )
                         {
                             this.RegisterError( ex ) ;
                             return null ;
                         }

                         if( _CurrentView.AggregationsStatus == null )
                             _CurrentView = GetRealView(_CurrentView);
                     }
                }              
                else //û������viewʱ��ȡ������view��Ĭ��view
                {
                    if (SPContext.Current.ViewContext != null && SPContext.Current.ViewContext.View != null)
                    {
                        _CurrentView = SPContext.Current.ViewContext.View;
                    }
                    else
                    {
                        SPList list = this.GetCurrentSPList();

                        if (list != null)
                        {
                            //_CurrentView = list.DefaultView ;
                            _CurrentView = GetRealView(list.DefaultView);
                        }
                    }
                }

                return _CurrentView;
            }
        }
        /// <summary>
        /// ���ڣ֣�����ҳ���޷�ȡ����ʵ������
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        SPView GetRealView(SPView view)
        {

            //return this.GetCurrentSPWeb().GetViewFromUrl(view.Url);

            SPSite site = new SPSite(SPContext.Current.Site.ID);
            SPWeb web = site.OpenWeb(this.GetCurrentSPWeb().ID);
            SPView v = web.GetViewFromUrl(view.Url);
            web.Dispose();
            site.Dispose();
            return v;
        }

        private Guid _ViewID = Guid.Empty;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        public Guid ViewID
        {
            get
            {
                //if (Guid.Empty == _ViewID)
                //    _ViewID = Guid.NewGuid();

                return _ViewID;
            }
            set { _ViewID = value; }
        }

        //private string _ViewName;
        //[Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable]
        //[ResWebDisplayName( "BaseSPWebPart_ViewName","��ͼ" )]
        //public string ViewName
        //{
        //    get { return _ViewName; }
        //    set { _ViewName = value; }
        //}

        /// <summary>
        /// �������б�
        /// </summary>
        public virtual SPList List
        {
            get
            {
                return GetCurrentSPList();
            }
        }

        private SPList _CurSPList; 
        /// <summary>
        /// ��ȡ��ǰlist
        /// </summary>
        /// <returns></returns>
        private  SPList GetCurrentSPList()
        {

            if (_CurSPList == null)
            {
                if (!String.IsNullOrEmpty(_ListName))
                {
                    try
                    {
                        _CurSPList = GetCurrentSPWeb().Lists[_ListName];
                    }
                    catch (ArgumentException ex)
                    {
                        base.RegisterError(ex);
                        return null;
                    }

                }
                else if (Page != null && Page.Request.QueryString["List"] != null)
                {
                    _CurSPList = base.Web.Lists[new Guid(Page.Request.QueryString["List"])];
                }
                else
                {
                    _CurSPList = SPContext.Current.List;
                }
            }


            return _CurSPList;
        }

         

        protected bool IsHiddenFolder(SPFolder f)
        {
            return f.Properties.Count < 20;
        }

        protected void RedirectToListSettingPage()
        {
            if (Page.Request.QueryString["List"] != null)
            {
                string webUrl = base.Web.ServerRelativeUrl;
                if (!webUrl.EndsWith("/"))
                    webUrl += "/";

                string sourceUrl = webUrl + "_layouts/listedit.aspx?List=" + this.List.ID.ToString("B").ToUpper();
                Page.Response.Redirect(sourceUrl);
            }
        }

        protected TableRow AddRow(Table table, params string[] texts)
        {
            TableRow row = new TableRow();
            table.Rows.Add(row);

            foreach (string c in texts)
            {
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                cell.Text = c;
            }
            return row;
        }

        public string GetResource(string key)
        {
            return Util.GetResource(key);
        }

    }
}
