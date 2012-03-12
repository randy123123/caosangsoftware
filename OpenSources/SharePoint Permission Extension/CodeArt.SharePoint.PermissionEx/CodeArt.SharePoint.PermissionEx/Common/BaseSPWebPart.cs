//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-1-19
//
//修订记录: 
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
 
 

[assembly: TagPrefix("CodeArt.SharePoint.PermissionEx", "mcs")]
namespace CodeArt.SharePoint.PermissionEx
{   
      
    /// <summary>
    /// webpart基类
    /// </summary>
    public abstract class BaseSPWebPart : System.Web.UI.WebControls.WebParts.WebPart, IPostBackEventHandler
    {

        protected const string WEB_NAME_QUERYSTRING_KEY = "__WebName";
        protected const string LIST_NAME_QUERYSTRING_KEY = "__ListName";

        protected string GetQueryString(string key)
        {
            return Page.Request.QueryString[key] != null ? Page.Request.QueryString[key] : "";
        }
               

        //private string _ResourcePath;
        //public string ResourcePath
        //{
        //    get
        //    {
        //        if (_ResourcePath == null)
        //            _ResourcePath = base.ResolveUrl(Constant.MCS_RESOURCES_PATH);

        //        return _ResourcePath;
        //    }
        //}

        #region 绝对定位属性
       
        private bool _AllowCustomPosition = false;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_AllowCustomPosition")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        //[Resources("ToolbarSelectorLabel", "Layout", "ToolbarSelectorLabel")]
        public bool AllowCustomPosition
        {
            get { return _AllowCustomPosition; }
            set { _AllowCustomPosition = value; }
        }

        private Unit _CustomPositionWidth;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_CustomPositionWidth")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        public Unit CustomPositionWidth
        {
            get { return _CustomPositionWidth; }
            set { _CustomPositionWidth = value; }
        }

        private Unit _CustomPositionHeight;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_CustomPositionHeight")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        public Unit CustomPositionHeight
        {
            get { return _CustomPositionHeight; }
            set { _CustomPositionHeight = value; }
        }

        private Unit _PositionLeft ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_PositionLeft")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        public Unit PositionLeft
        {
            get { return _PositionLeft; }
            set { _PositionLeft = value; }
        }

        private Unit _PositionTop ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_PositionTop")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        public Unit PositionTop
        {
            get { return _PositionTop; }
            set { _PositionTop = value; }
        }

        private bool _PositionDependRight = false;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_PositionDependRight")]
        [ResCategory("BaseSPWebPart_Category_Position")]
        public bool PositionDependRight
        {
            get { return _PositionDependRight; }
            set { _PositionDependRight = value; }
        }

        #endregion

 

        #region 数据源属性

        protected SPWeb Web
        {
            get
            {
                return this.GetCurrentSPWeb();
            }
        }

        private string _WebName;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_WebName")]
        public string WebName
        {
            get { return _WebName; }
            set { _WebName = value; }
        }

        private string _SiteUrl;
        public string SiteUrl
        {
            get { return _SiteUrl; }
            set { _SiteUrl = value; }
        }

        #endregion

        #region 错误处理
        
        private bool _EnableShowErrorMessage = true;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("BaseSPWebPart_EnableShowErrorMessage")]
        public bool EnableShowErrorMessage
        {
            get { return _EnableShowErrorMessage; }
            set { _EnableShowErrorMessage = value; }
        }

        protected void RenderError(Exception ex, HtmlTextWriter w)
        {
            if (_EnableShowErrorMessage)
            {
                w.Write(ex.Message);
                w.Write("<br/>");
                w.Write(ex.StackTrace);
                w.Write("<br/>");

                if (ex.InnerException != null)
                    RenderError(ex.InnerException, w);
            }
            else
            {
                w.Write("发生错误");
            }
        }

        private IList<Exception> _Exceptions = new  List<Exception>();
        /// <summary>
        /// 记录一个错误
        /// </summary>
        /// <param name="ex"></param>
        protected void RegisterError(Exception ex)
        {
            _Exceptions.Add(ex);
        }
        /// <summary>
        /// 呈现所有错误信息，只呈现一次
        /// </summary>
        /// <param name="w"></param>
        protected void RenderErrors(HtmlTextWriter w)
        {
            if (!_EnableShowErrorMessage)
                return;

            if (_Exceptions.Count == 0) return;

            foreach (Exception ex in _Exceptions)
            {
                w.Write(ex.Message);
                w.Write("<br/>");
                w.Write(ex.StackTrace);
                w.Write("<br/>");
            }

            _Exceptions.Clear();
        }

        #endregion

        #region 数据源获取方法


        private SPSite _CurSite;
        /// <summary>
        /// 获取当前站点
        /// </summary>
        /// <returns></returns>
        protected virtual SPSite GetCurrentSPSite()
        {

            if (_CurSite == null)
            {
                if (String.IsNullOrEmpty(SiteUrl))
                {
                    _CurSite = SPContext.Current.Site;
                }
                else
                {
                    _CurSite = new SPSite(SiteUrl);
                }
            }

            return _CurSite;
        }

       

        private SPWeb _CurSPWeb;
        /// <summary>
        /// 获取当前web
        /// </summary>
        /// <returns></returns>
        protected virtual SPWeb GetCurrentSPWeb()
        {
            if (_CurSPWeb == null)
            {

                if (!String.IsNullOrEmpty(_WebName))
                {
                    _CurSPWeb = GetCurrentSPSite().AllWebs[_WebName];
                }
                else if (GetQueryString(WEB_NAME_QUERYSTRING_KEY) != "")
                {
                    _CurSPWeb = GetCurrentSPSite().AllWebs[GetQueryString(WEB_NAME_QUERYSTRING_KEY)];
                }
                else
                {
                    if (String.IsNullOrEmpty(_SiteUrl))
                        _CurSPWeb = SPContext.Current.Web;
                    else
                        _CurSPWeb = GetCurrentSPSite().RootWeb;
                }
            }

            return _CurSPWeb;
        }

        #endregion

        //public override EditorPartCollection CreateEditorParts()
        //{
        //    ArrayList editorArray = new ArrayList();

        //    SkinEditorPart edPart = new SkinEditorPart();
        //    edPart.ID = this.ID + "_skinEditorPart";
        //    editorArray.Add(edPart);

        //    EditorPartCollection initEditorParts = base.CreateEditorParts();

        //    EditorPartCollection editorParts = new EditorPartCollection(initEditorParts, editorArray);

        //    return editorParts;
        //}

        protected readonly IList<String> MCSCommonJs = new List<String>();

        protected void RegisterCommonJs(string jsFileName)
        {
            //MCSCommonJs.Add( jsFileName );
            string jsPath = base.ResolveUrl(Constant.RESOURCES_PATH) + jsFileName ;

            Page.ClientScript.RegisterClientScriptInclude(typeof(BaseSPWebPart), jsFileName.ToLower() , jsPath);    
        }


      protected void RegisterCommonCss( string fileName )
      {
          string id = fileName.ToLower().Replace(".","_");

          foreach( Control ctl in  this.Page.Header.Controls )
          {
              if( ctl.ID == id )
                  return ;
          }

           HtmlLink link1 = new HtmlLink();
           link1.ID = id;

           link1.Attributes["type"] = "text/css";
           link1.Attributes["rel"] = "stylesheet";
           link1.Attributes["href"] = base.ResolveUrl(Constant.RESOURCES_PATH) + fileName; 

           this.Page.Header.Controls.Add(link1);
       }

        

        #region Drag_Js
        private const string Drag_Js = @"
<style type='text/css'>
.webpart-docked
{
    filter:alpha(opacity=100);
}
.webpart-actived
{
    filter:alpha(opacity=20);
    border-right: black 2px dotted; border-top: black 2px dotted; border-left: black 2px dotted; border-bottom: black 2px dotted;
}
</style>
<script language='javascript' type='text/javascript'>    

//拖拽过程中相关变量
var WebPart_draging = false; //是否处于拖拽中
var WebPart_offsetX = 0;     //X方向左右偏移量
var WebPart_offsetY = 0;     //Y方向上下偏移量
var  WebPart_initPosition = ''; 
//准备拖拽
function WebPart_BeforeDrag(id)
{ 
    if (event.button != 1)
    {
        return;
    }
    
    var targetObj = document.getElementById(id);
    
    WebPart_offsetX = document.body.scrollLeft + event.clientX-targetObj.style.pixelLeft;
    WebPart_offsetY = document.body.scrollTop + event.clientY-targetObj.style.pixelTop;
    targetObj.className = 'webpart-actived';
    WebPart_draging = true;
    WebPart_initPosition = targetObj.style.pixelLeft+','+targetObj.style.pixelTop ;
}
//拖拽中
function WebPart_OnDrag(id)
{
    if(!WebPart_draging)
    {
        return;
    }
    //更新位置
    event.returnValue = false;
    
    var targetObj = document.getElementById(id);
    
    targetObj.style.pixelLeft = document.body.scrollLeft + event.clientX-WebPart_offsetX;
    targetObj.style.pixelTop = document.body.scrollTop + event.clientY-WebPart_offsetY;
}
//结束拖拽
function WebPart_EndDrag(id,wpName)
{
    if (event.button != 1)
    {
        return;
    }
    
    WebPart_draging = false;
    
    var targetObj = document.getElementById(id);    
    targetObj.className = 'webpart-docked';    

    var args = targetObj.style.pixelLeft+','+targetObj.style.pixelTop ;

    if(args == WebPart_initPosition) return ;  

    __doPostBack( wpName ,args);
}
    
</script>";

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.RegisterCommonJs("MCSCore.js");
            //RegisterCommonCss( "WebPart" , this.ResolveUrl(Constant.MCS_RESOURCE_PATH + "WebPart.css"));

            //string jsPath = base.ResolveUrl(Constant.MCS_RESOURCE_PATH);

            //foreach (string file in MCSCommonJs)
            //{
            //    Page.ClientScript.RegisterClientScriptInclude(typeof(BaseSPWebPart), file,  jsPath + file );
            //}

            //Page.Response.Write(this.WebPartManager.DisplayMode);

            if (this._AllowCustomPosition ) 
            {
                if (this.WebPartManager == null || this.WebPartManager.DisplayMode == WebPartManager.BrowseDisplayMode) //显示模式
                {
                    this.ChromeType = PartChromeType.None;
                    this.Height = new Unit(0);
                    this.Width = Unit.Empty;

                    this.Attributes.Remove("onmousedown");
                    this.Attributes.Remove("onmousemove");
                    this.Attributes.Remove("onmouseup");
                }
                else
                {
                    this.Height = Unit.Empty;
                    //调用此句，使自动输出__doPostBack函数
                    string serverEventJs = this.Page.ClientScript.GetPostBackEventReference(this,"");

                    this.Style.Add(HtmlTextWriterStyle.Cursor, "move");

                    //this.Controls.AddAt(0, new LiteralControl("&nbsp;&nbsp;<<拖 动>>&nbsp;&nbsp;"));

                    Page.ClientScript.RegisterClientScriptBlock(typeof(BaseSPWebPart), "webpart_drag", Drag_Js);

                    this.Attributes.Add("onmousedown", "WebPart_BeforeDrag('" + this.ClientID + "')");
                    this.Attributes.Add("onmousemove", "WebPart_OnDrag('" + this.ClientID + "')");
                    this.Attributes.Add("onmouseup", "WebPart_EndDrag('" + this.ClientID + "','" + this.UniqueID + "')");
                
                //onmousedown="WebPart_BeforeDrag()" onmousemove="WebPart_OnDrag()" onmouseup="WebPart_EndDrag('ddd')"
                }

            }

            if (this._AllowCustomPosition && this._PositionDependRight)//左右位置转换
            {
                string js = @"
var divObj = document.getElementById( '{0}' );     
if( divObj.style.width != '' )    
    divObj.style.pixelLeft = screen.width - divObj.style.pixelLeft - parseInt(divObj.style.width.replace( 'px' , '' )) ;
else
    divObj.style.pixelLeft = screen.width - divObj.style.pixelLeft ;
";
                Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "_changePosition" , String.Format(js, this.ClientID), true);

            }
            
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);

            //统一的错误处理
            RenderErrors(writer);
        }
         
        protected override void Render(HtmlTextWriter writer)
        {          
            if (this._AllowCustomPosition)//绝对定位
            {
                writer.Write("<!-- custom position -->");

                base.Style.Add(HtmlTextWriterStyle.Position, "absolute");

                if (this.PositionLeft.IsEmpty)
                    base.Style.Add(HtmlTextWriterStyle.Left, "0px" );
                else              
                    base.Style.Add(HtmlTextWriterStyle.Left, this.PositionLeft.ToString());

                base.Style.Add(HtmlTextWriterStyle.Left, this.PositionLeft.ToString());

                base.Style.Add(HtmlTextWriterStyle.Top, this.PositionTop.ToString());
                //this.CssClass = "mcs-drag";

                if (!this.CustomPositionHeight.IsEmpty)
                    base.Style.Add(HtmlTextWriterStyle.Height, this.CustomPositionHeight.ToString());

                if (!this.CustomPositionWidth.IsEmpty)
                    base.Style.Add(HtmlTextWriterStyle.Width, this.CustomPositionWidth.ToString());

                //设计模式时
                if ( this.WebPartManager != null && this.WebPartManager.DisplayMode != WebPartManager.BrowseDisplayMode)
                {                 
                    writer.Write("<div align='center'>浮动显示</div>");
                }              
                
            }
            else
            {
                //base.Render(writer);
            }

            ////皮肤
            //SkinElement el = GetSkin();

            //if (el != null)
            //{
            //    string html = ParserSkinTags(this.TemplateID + "_header", el.Header );
            //    writer.Write(html);
            //}
             
            base.Render(writer);

            //if (el != null)
            //{
            //    string html = ParserSkinTags(this.TemplateID + "_footer", el.Footer );
            //    writer.Write(html);
            //}

        }

     
        
       


        protected override void OnUnload(EventArgs e)
        {
            //if (_CurSPWeb != null)
            //    _CurSPWeb.Dispose();

            //if (_CurSite != null)
            //    _CurSite.Dispose();

            base.OnUnload(e);
        }

        protected string GeyShowToolPanelScript()
        {
            return "MSOTlPn_ShowToolPane2Wrapper('Edit','129','"+this.ID+"')";
        }

        protected void RegisterShowToolPanelControl(string msg1, string msg2, string msg3)
        {
            LiteralControl lt = new LiteralControl();
            lt.Text = "<P><DIV class=\"UserGeneric\">"+msg1+"<A HREF=\"javascript:" + GeyShowToolPanelScript() + "\">"+msg2+"</A>"+msg3+"</DIV></P>";
            this.Controls.Add(lt);
        }

        protected void AddHtml(string html)
        {
            LiteralControl lt = new LiteralControl();
            lt.EnableViewState = false;
            lt.Text = html;
            this.Controls.Add(lt);
        }

        protected void AddHtml(string html , Control ctl )
        {
            LiteralControl lt = new LiteralControl();
            lt.EnableViewState = false;
            lt.Text = html;
            ctl.Controls.Add(lt);
        }

        #region IPostBackEventHandler Members

        //拖动后触发
        public void RaisePostBackEvent(string eventArgument)
        {
            if (String.IsNullOrEmpty(eventArgument))
                return;

            string[] arr = eventArgument.Split(',');

            this.PositionLeft = Unit.Parse(arr[0]);
            this.PositionTop = Unit.Parse(arr[1]);

            PositionDependRight = false; //拖动后禁用左右位置转换

            if (this.WebPartManager != null)
            {
                this.SetPersonalizationDirty(); //必须设置，才能使改变的属性持久保存
            }

            //IPersonalizable p = this.WebPartManager;
           // p.Save(new PersonalizationDictionary());
        }

        #endregion


        //private string getCurrentLCID()
        //{
        //    return SPControl.GetContextWeb(this.Context).Locale.LCID.ToString();
        //}// returns LCID 
    }
}
