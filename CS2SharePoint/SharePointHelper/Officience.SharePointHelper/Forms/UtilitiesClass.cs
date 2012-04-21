using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Officience.SharePointHelper
{
    #region Server Config
    public class ServerConfig
    {
        public List<Server> ListServers { get; set; }
    }
    [Serializable]
    public class Server
    {
        [XmlAttribute]
        public string Url { get; set; }
        [XmlAttribute]
        public bool Default { get; set; }
        public Server() { }
        public Server(string url)
        {
            Url = url;
            Default = false;
        }
        public Server(string url, bool def)
        {
            Url = url;
            Default = def;
        }
    }
    #endregion Server Config
}