using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.WebControls;
using System.Collections.Specialized;
using System.Data;
using Microsoft.Office.Core;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Xml;
using Microsoft.SharePoint.Administration;


namespace SharePointCustomRibbon.Layouts.SharePointCustomRibbon
{
    public partial class ShowItems : LayoutsPageBase
    {
        // SPContext currentSPContext;
        // SPWeb currentSPWeb;

        public string list;
        public string view;
        public string url;
        public Guid listID;

        protected override void OnLoad(EventArgs e)
        {
            //office2010Buttun.Click += new EventHandler(LinkButton_Click);
            //GenerateButton.Click += new EventHandler(GenerateButton_click);

            //currentSPContext = SPContext.Current;
            //currentSPWeb = SPContext.Current.Web;
            //SPSite currentSPSite = SPContext.Current.Site;

            //SPWebApplication oWebApplicationCurrent = SPContext.Current.Site.WebApplication;
            //SPSiteCollection collSites = oWebApplicationCurrent.Sites;
            //SPWeb webCollection = SPContext.Current.Site.AllWebs[currentSPSite.Url];
            //SPWeb oWebSite = SPControl.GetContextWeb(Context);

            //SPSite siteCollection = new SPSite(SPContext.Current.ListItemServerRelativeUrl.ToString());
            //SPWeb myWeb = siteCollection.OpenWeb();

            if (Request.QueryString["list"] != null && Request.QueryString["view"] != null && Request.QueryString["url"] != null)
            {

                //string correctFieldName = XmlConvert.DecodeName(fields).ToString();
                list = Request.QueryString["list"];
                view = Request.QueryString["view"];
                url = Request.QueryString["url"];

                office2003Buttun.NavigateUrl = "/_layouts/SharePointCustomRibbon/Generate.aspx?list=" + list + "&view=" + view + "&url=" + url + "&office=2003";
                office2010Buttun.NavigateUrl = "/_layouts/SharePointCustomRibbon/Generate.aspx?list=" + list + "&view=" + view + "&url=" + url + "&office=2010";

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script type='text/javascript'>SP.UI.Notify.addNotification('Generating Excel!');</script>");
                //Context.Response.Write("<script type='text/javascript'>SP.UI.Notify.addNotification('Generating Excel!');</script>");

            }
            else
            {

            }
        }
    }
}

