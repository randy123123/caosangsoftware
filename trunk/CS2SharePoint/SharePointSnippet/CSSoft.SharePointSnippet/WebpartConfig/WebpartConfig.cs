using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CSSoft.SharePointSnippet.WebpartConfig
{
    [ToolboxItemAttribute(false)]
    public class WebpartConfig : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/CSSoft.SharePointSnippet/WebpartConfig/WebpartConfig.UserControl.ascx";
        private const string _ascxConfigPath = @"~/_CONTROLTEMPLATES/CSSoft.SharePointSnippet/WebpartConfig/WebpartConfig.ConfigForm.ascx";

        protected  TextBox txtInfo;
        protected override void CreateChildControls()
        {
            Control controlConfig = Page.LoadControl(_ascxConfigPath);
            controlConfig.ID = "ConfigForm";
            controlConfig.Visible = false;
            Controls.Add(controlConfig);
            Control control = Page.LoadControl(_ascxPath);
            control.ID = "UserControl";
            Controls.Add(control);
        }
        public override WebPartVerbCollection Verbs
        {
            get
            {
                WebPartVerb menuWebpartConfig = new WebPartVerb("menuWebpartConfig", new WebPartEventHandler(menuWebpartConfig_Click));
                menuWebpartConfig.Text = "Webpart Config";
                WebPartVerbCollection wbVerbCollection = new WebPartVerbCollection(base.Verbs, new WebPartVerb[] { menuWebpartConfig });
                return wbVerbCollection;
            }
        }
        protected void menuWebpartConfig_Click(object sender, WebPartEventArgs args)
        {
            this.FindControl("ConfigForm").Visible = true;
            this.FindControl("UserControl").Visible = false;
        }  
        //public override WebPartVerbCollection Verbs
        //{
        //    get
        //    {
        //        // Client side verb
        //        WebPartVerb clientSideVerb = new WebPartVerb("clientID", "javascript:alert('Hello World from Java Script Verb!');");
        //        clientSideVerb.Text = "Client Side Verb";
        //        // Server side verb
        //        WebPartVerb serverSideVerb = new WebPartVerb("serverID", new WebPartEventHandler(ServerVerbEventHandler));
        //        serverSideVerb.Text = "Server Side Verb";
        //        // Verb for both client side and server side
        //        WebPartVerb bothSideVerb = new WebPartVerb("bothID", new WebPartEventHandler(ServerVerbEventHandler), "javascript:alert('Hello World from Java Script Verb!');");
        //        bothSideVerb.Text = "Both Side Verb";          
        //        WebPartVerbCollection wbVerbCollection = new WebPartVerbCollection(base.Verbs, new WebPartVerb[] { clientSideVerb, serverSideVerb, bothSideVerb  });
        //        return wbVerbCollection;
        //    }
        //}
        //protected void ServerVerbEventHandler(object sender, WebPartEventArgs args)
        //{
        //    txtInfo.Text="Hello world from Server Side Verb";                           
        //}       
    }
}
