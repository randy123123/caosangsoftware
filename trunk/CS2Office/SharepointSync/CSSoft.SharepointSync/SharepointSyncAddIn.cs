using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace CSSoft.SharepointSync
{
    public partial class SharepointSyncAddIn
    {
        private void SharepointSyncAddIn_Startup(object sender, System.EventArgs e)
        {
            //this.Application.
        }

        private void SharepointSyncAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(SharepointSyncAddIn_Startup);
            this.Shutdown += new System.EventHandler(SharepointSyncAddIn_Shutdown);
        }
        
        #endregion
    }
}
