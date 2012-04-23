using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using Microsoft.SharePoint.Administration;
using System.IO;

namespace CSSoft
{
    public class CS2ConfigList
    {
        public CS2ConfigList() { Settings = new ConfigListSettings(); }
        public ConfigListSettings Settings { get; set; }
        private SPList _listConfig;
        public SPList Listconfig
        {
            get
            {
                if (_listConfig == null)
                try
                {
                    _listConfig = Settings.UseCurrentWeb ? CS2Web.GetList(Settings.ListName) : CS2Web.GetRootList(Settings.ListName);
                }
                catch { }

                if (_listConfig == null)
                {
                    if (Settings.UseCurrentWeb)
                    {
                        CS2Web.CurrentWeb.Lists.Add(Settings.ListName, "List for config , never delete this list.", SPListTemplateType.GenericList);
                        _listConfig = CS2Web.GetList(Settings.ListName);
                    }
                    else
                    {
                        CS2Web.CurrentSite.RootWeb.Lists.Add(Settings.ListName, "List for config , never delete this list.", SPListTemplateType.GenericList);
                        _listConfig = CS2Web.GetRootList(Settings.ListName);
                    } 
                    
                    //Update title text
                    SPField field = _listConfig.Fields[new Guid("fa564e0f-0c70-4ab9-b863-0177e6ddd247")];
                    field.Title = "Key";
                    field.Update();
                    _listConfig.Update();

                    //Add custome fields
                    _listConfig.Fields.Add("Group", SPFieldType.Text, true);
                    _listConfig.Fields.Add("Value", SPFieldType.Text, true);
                    
                    //Update DefaultView
                    SPView defView = _listConfig.DefaultView;
                    defView.ViewFields.Add("Value");
                    defView.Query = "<GroupBy><FieldRef Name='Group'/></GroupBy>";
                    defView.Update();
                }
                return _listConfig;
            }
        }
        
        public string GetConfig(string group, string key)
        {
            SPListItem itemConfig = GetItemByTitle(Listconfig, group, key);
            return new ConfigValue(itemConfig).Value;
        }

        public void SetConfig(string group, string key, string value)
        { 
            SPListItem itemConfig = GetItemByTitle(Listconfig, group, key);
            itemConfig["Group"] = group;
            itemConfig["Value"] = value;
            itemConfig.SystemUpdate();        
        }

        private SPListItem GetItemByTitle(SPList list, string group, string key)
        {
            SPQuery q = new SPQuery { Query = String.Format("<Where><And><Eq><FieldRef Name='Group'/><Value Type='Text'>{0}</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>{1}</Value></Eq></And></Where>", group, key), RowLimit = 1 };
            SPListItemCollection items = list.GetItems(q);
            if(items.Count == 0)
            {
                SPListItem item = list.AddItem();
                item["Title"] = key;
                item["Group"] = group;
                item["Value"] = "";
                item.SystemUpdate();
                return item;
            }
            else return items[0];
        }
    }
    public class ConfigListSettings
    {
        private const string DEFAULT_LIST_NAME = "__CSSoft_ConfigList";
        private string _listName;
        public string ListName
        {
            get { if (String.IsNullOrEmpty(_listName)) _listName = DEFAULT_LIST_NAME; return _listName; }
            set { _listName = value; }
        }
        private bool _useCurrentWeb = false;
        public bool UseCurrentWeb { get { return _useCurrentWeb; } set { _useCurrentWeb = value; } }
    }    
    public class ConfigValue
    {
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public ConfigValue() { }
        public ConfigValue(SPListItem item)
        {
            Key = item.Title;
            Group = CS2Convert.ToString(item["Group"]);
            Value = CS2Convert.ToString(item["Value"]);
        }
    }
}
