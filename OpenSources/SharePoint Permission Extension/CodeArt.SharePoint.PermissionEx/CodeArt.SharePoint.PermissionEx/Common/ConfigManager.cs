//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2007-12-29
//
//修订记录: 
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
//using CodeArt.SharePoint.PermissionEx.CAMLQuery;

namespace CodeArt.SharePoint.PermissionEx
{
    /// <summary>
    ///  
    /// </summary>
    public abstract class ConfigManager
    {        

        public static ConfigManager GetConfigManager(string key)
        {
            return new ContextDocLibConfigManager( key );
        }

        public static ConfigManager GetConfigManager(string key,SPWeb web)
        {
            return new DocLibConfigManager(key,web);
        }

        public virtual T GetConfigData<T>(string id) where T : class , new() 
        {
            object obj = this.GetConfigData(typeof(T), id);

            if( obj == null )
                return null;
            return
                (T)obj;
        }

        public virtual object GetConfigData(Type t, string id)
        {
            return null ;
        }

        public virtual void SetConfigData(string id , object obj)
        {
            
        }

        public virtual void ClearConfigData(string id)
        {
        }
    }
    
    class ContextDocLibConfigManager : ConfigManager
    {
        private string _key;

        public ContextDocLibConfigManager(string key)
        {
            _key = key;
        }      

        SPList EnsureList(SPWeb web)
        {
            SPList list = null;

            try
            {
                list = web.Lists[_key];
            } 
            catch { }

            if (list == null)
            {
                web.AllowUnsafeUpdates = true;
                Guid listId = web.Lists.Add(_key, "List for config , never delete this list.", SPListTemplateType.DocumentLibrary);
                list = web.Lists[listId];
            }

            return list;
        }

        public override void SetConfigData(string id ,object obj)
        {
            SPList list = null;

            string xml = SerializeUtil.Seralize(obj);
            byte[] content = Encoding.UTF8.GetBytes(xml);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                    {
                        list = this.EnsureList(elevatedWeb);
                        elevatedWeb.AllowUnsafeUpdates = true;
                        SPFile file = list.RootFolder.Files.Add( id.ToString() + ".xml", content, true);
                    }
                }
            });

        }

        private SPListItem GetItem(SPList list, string id)
        {            
            SPQuery q = new SPQuery();
            q.Query = "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='Text'>" + id + ".xml</Value></Eq></Where>";
            q.RowLimit = 1;

            SPListItemCollection items = list.GetItems(q);

            if (items.Count == 0)
                return null;
            else
                return items[0];
        }

        public override void ClearConfigData(string id)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                  {
                      using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                      {
                          try
                          {
                              SPList list = this.EnsureList(elevatedWeb);
                              elevatedWeb.AllowUnsafeUpdates = true;

                              SPListItem item = this.GetItem(list, id);
                              if( item != null )
                                  item.Delete();
                          }
                          catch { throw; }
                      }
                  }
              });

        }

        public override object GetConfigData(Type t, string id)
        {
            object obj = null;

            SPSecurity.RunWithElevatedPrivileges(delegate()
              {
                  using (SPSite elevatedsiteColl = new SPSite(SPContext.Current.Site.ID))
                  {
                      using (SPWeb elevatedWeb = elevatedsiteColl.OpenWeb(SPContext.Current.Web.ID))
                      {
                          SPList list = this.EnsureList(elevatedWeb);

                          try
                          {
                              SPListItem item = this.GetItem(list, id);

                              if (item != null)
                              {
                                  SPFile file = item.File;

                                  XmlDocument doc = new XmlDocument();
                                  doc.Load(item.File.OpenBinaryStream());

                                  obj = SerializeUtil.Deserialize(t, doc.OuterXml);
                              }
                          }
                          catch { throw; }
                      }
                  }
              });

            return obj;
        }
    }

    class DocLibConfigManager : ConfigManager
    {
        private string _key;
        private SPWeb _web;

        public DocLibConfigManager(string key, SPWeb web)
        {
            _key = key;
            _web = web;
        }

        SPList EnsureList()
        {
            SPList list = null;

            try
            {
                list = _web.Lists[_key];
            }
            catch { }

            if (list == null)
            {
                _web.AllowUnsafeUpdates = true;
                Guid listId = _web.Lists.Add(_key, "List for config , never delete this list.", SPListTemplateType.DocumentLibrary);
                list = _web.Lists[listId];
            }

            return list;
        }

        public override void SetConfigData(string id, object obj)
        {
            SPList list = null;

            string xml = SerializeUtil.Seralize(obj);
            byte[] content = Encoding.UTF8.GetBytes(xml);

            list = this.EnsureList();

            _web.AllowUnsafeUpdates = true;
            SPFile file = list.RootFolder.Files.Add(id.ToString() + ".xml", content, true);                 

        }

        private SPListItem GetItem(SPList list, string id)
        {
            SPQuery q = new SPQuery();
            //if (id.EndsWith("view")
            q.Query = "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='Text'>" + id.ToString() + ".xml</Value></Eq></Where>";
            q.RowLimit = 1;

            SPListItemCollection items = list.GetItems(q);

            if (items.Count == 0)
                return null;
            else
                return items[0];
        }

        public override void ClearConfigData(string id)
        {            
              SPList list = this.EnsureList();
              _web.AllowUnsafeUpdates = true;

              SPListItem item = this.GetItem(list, id);
              if (item != null)
                  item.Delete();                          
        }

        public override object GetConfigData(Type t, string id)
        {
            object obj = null;
            
              SPList list = this.EnsureList();

              try
              {
                  SPListItem item = this.GetItem(list, id);

                  if (item != null)
                  {
                      SPFile file = item.File;

                      XmlDocument doc = new XmlDocument();
                      doc.Load(item.File.OpenBinaryStream());

                      obj = SerializeUtil.Deserialize(t, doc.OuterXml);
                  }
              }
              catch { throw; }                    

            return obj;
        }
    }
}
