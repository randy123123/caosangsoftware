using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CSSoft
{
    public partial class CS2Web : IDisposable
    {
        #region IDisposable
        ~CS2Web() 
        {
            Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable

        #region SPWeb
        public static SPSite CurrentSite { get { return SPContext.Current.Site; } }
        public static SPWeb CurrentWeb { get { return SPContext.Current.Web; } }
        public static SPUser CurrentUser { get { return SPContext.Current.Web.CurrentUser; } }        
        #endregion SPWeb

        public static SPList GetList(string listName)
        {
            return CurrentWeb.Lists[listName];
        }
    }
}
