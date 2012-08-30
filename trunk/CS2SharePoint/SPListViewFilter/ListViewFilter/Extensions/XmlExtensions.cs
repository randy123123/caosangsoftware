using System.Xml;
using System.Xml.Linq;

namespace ListViewFilter.Extensions
{
    internal static class XmlExtensions
    {
        public static string AttributeValue(this XmlNode node, string attrName)
        {
            return ((node.Attributes[attrName] == null) ? string.Empty : node.Attributes[attrName].Value);
        }

        public static int AttributeValueInteger(this XmlNode node, string attrName)
        {
            int res;
            var attributeValue = node.AttributeValue(attrName);
            int.TryParse(attributeValue, out res);
            return res;
        }

        public static string AttributeValue(this XElement node, string attrName)
        {
            return ((node.Attribute(attrName) == null) ? string.Empty : node.Attribute(attrName).Value);
        }

        public static int AttributeValueInteger(this XElement node, string attrName)
        {
            int res;
            var attributeValue = node.AttributeValue(attrName);
            int.TryParse(attributeValue, out res);
            return res;
        }
    }
}
