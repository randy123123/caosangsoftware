using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using System.Xml.Serialization;

namespace CSSoft
{
    public partial class CS2Serialize<T>
    {
        public static string Serialize(T t)
        {
            return Serialize(t, null, null);
        }
        public static string Serialize(T t, string strPrefix, string strNamespace)
        {
            string strReturn = "";
            StringWriter sw = new StringWriter();
            XmlSerializerNamespaces namespaceManager = new XmlSerializerNamespaces();
            XmlSerializer ser;

            if (!string.IsNullOrEmpty(strNamespace))
            {
                namespaceManager.Add(strPrefix, strNamespace);
                ser = new XmlSerializer(typeof(T), strNamespace);
                ser.Serialize(sw, t, namespaceManager);
            }
            else
            {
                ser = new XmlSerializer(typeof(T));
                ser.Serialize(sw, t);
            }
            strReturn = sw.ToString();
            return strReturn;
        }
        public static T DeSerialize(string s)
        {
            return DeSerialize(s, null);
        }
        public static T DeSerialize(string s, string strNamespace)
        {
            T t = default(T);
            StringReader sr = new StringReader(s);
            XmlSerializer ser;
            if (!string.IsNullOrEmpty(strNamespace))
            {
                ser = new XmlSerializer(typeof(T), strNamespace);
            }
            else
            {
                ser = new XmlSerializer(typeof(T));
            }
            t = (T)ser.Deserialize(sr);
            return t;
        }
    }
}
