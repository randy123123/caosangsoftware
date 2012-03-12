using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI ;

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    /// �ͻ���js����
    /// </summary>
    class  Script
    {
        private Control _Ctl;
        public Script( Control ctl )
        {
            _Ctl = ctl;
        }

        /// <summary>
        /// �ڿͻ�����ʾһ����Ϣ 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Alert( string msg) 
        {
            Script.Alert(_Ctl, msg);
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public virtual void Close()
        {
            Script.Close(_Ctl);
        }

        /// <summary>
        /// �ض���
        /// </summary>
        /// <param name="url"></param>
        public virtual void Redirect(string url)
        {
            Script.Redirect(_Ctl, url);
        }

        /// <summary>
        /// ����
        /// </summary>
        public virtual void Back()
        {
            Script.Back(_Ctl);
        }

        /// <summary>
        /// ǰ��
        /// </summary>
        public virtual void Forward()
        {
            Script.Forward(_Ctl);
        }

        /// <summary>
        /// ˢ��̽�����ڵĸ�����
        /// </summary>
        public void ReloadOpener()
        {
            Script.ReloadOpener(_Ctl);
        }

        /// <summary>
        /// ˢ�¸�ҳ����
        /// </summary>
        /// <param name="frame"></param>
        public void ReloadFrame(string frame)
        {
            Script.ReloadFrame(_Ctl, frame);
        }

        /// <summary>
        /// ע������js����
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        public void RegisterJsVar(string varName, string value)
        {
            Script.RegisterJsVar( _Ctl , varName, value);
        }

        /// <summary>
        /// ����jsֵ
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        public void SetJsVar( string varName, string value)
        {
            Script.SetJsVar(_Ctl, varName, value);            
        }

        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="pageUrl"></param>
        public void Open(string pageUrl)
        {
            Script.Open(_Ctl, pageUrl);
        }

        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="newWin">�Ƿ����´��ڻ���ԭ�д��ڴ�</param>
        public void Open(string pageUrl,
            int left, int top, int width, int height, bool newWin)
        {
            Script.Open(_Ctl, pageUrl , left , top , width , height , newWin );
        }



        //static method

        static private string WrapJs( string js )
        {
            return "\n<script language=\"javascript\">\n<!--\n" + js + "\n-->\n</script>";
        }

        static void ValidPage( Control c )
        {
            if (c.Page == null)
                throw new Exception( "Page����Ϊ�գ�" );
        }
 
        /// <summary>
        /// �ڿͻ�����ʾһ����Ϣ
        /// </summary>
        /// <param name="c"></param>
        /// <param name="msg"></param>
        static public void Alert( Control c , string msg )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript( c.GetType(), "__Alert", 
                WrapJs( string.Format( "alert(\"{0}\");" , JsEncoder.Encode( msg ) ) ) ) ;
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        /// <param name="p"></param>
        /// <param name="msg"></param>
        static public void Close(Control c)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript( c.GetType(), "__Close", 
                WrapJs( "window.close();" ));
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="c"></param>
        static public void Back(Control c)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Back",
                WrapJs("history.back();"));
        }

        /// <summary>
        /// ǰ��
        /// </summary>
        /// <param name="c"></param>
        static public void Forward(Control c)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Forward",
                WrapJs("history.go(1);"));
        }

        /// <summary>
        /// �ض���
        /// </summary>
        /// <param name="p"></param>
        /// <param name="msg"></param>
        static public void Redirect(Control c, string url)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Redirect",
                WrapJs(string.Format("window.location=\"{0}\";" , JsEncoder.Encode(url) )));
        }

        /// <summary>
        /// ˢ�µ������ڵĸ�����
        /// </summary>
        /// <param name="c"></param>
        static public void ReloadOpener( Control c )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__ReloadOpener",
                WrapJs("window.opener.location.reload();"));
        }

        /// <summary>
        /// ˢ�¸�ҳ����
        /// </summary>
        /// <param name="c"></param>
        /// <param name="frame"></param>
        static public void ReloadFrame(Control c, string frame )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__ReloadOpener",
                WrapJs( String.Format( "window.parent.frames['{0}'].location.reload();" , frame )  ));
        }

        /// <summary>
        /// ע��һ�νű�
        /// </summary>
        /// <param name="c"></param>
        /// <param name="js"></param>
        static public void RegisterJs(Control c , string js )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__js",
                WrapJs("window.location.reload();"));
        }

        /// <summary>
        /// ע������js����
        /// </summary>
        /// <param name="c"></param>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        static public void RegisterJsVar( Control c , string varName, string value)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), varName ,
                WrapJs(String.Format("var {0}=\"{1}\"; ", varName, JsEncoder.Encode(value) )));
        }

        /// <summary>
        /// ����jsֵ
        /// </summary>
        /// <param name="c"></param>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        static public  void SetJsVar(Control c, string varName, string value)
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Set_" + varName  ,
                WrapJs(String.Format("{0}=\"{1}\"; ", varName  , JsEncoder.Encode(value) )));
        }


        static public void Open(Control c, string pageUrl )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Open_" + pageUrl,
                WrapJs(String.Format("PopUpWindow(\"{0}\"); " , JsEncoder.Encode(pageUrl) )));
        }

        static public void Open(Control c, string pageUrl , 
            int left, int top, int width, int height , bool newWin )
        {
            ValidPage(c);

            c.Page.ClientScript.RegisterStartupScript(c.GetType(), "__Open_" + pageUrl,
                WrapJs(String.Format("PopUpWindow(\"{0}\",{1},{2},{3},{4},{5}); ",
                JsEncoder.Encode(pageUrl) , left , top , width , height , newWin.ToString().ToLower() )));
        }

    }
}
