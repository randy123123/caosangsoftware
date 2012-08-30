using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ListViewFilter.ApplicationObjects
{
    internal class CAMLPredicateData
    {
        public CAMLOperator Operator;
        public string FeildInternalName;
        public CAMLFieldType FieldType;
        public bool IsLookupId;
        public string NodeValue;

        public override string ToString()
        {
            var opName = Enum.GetName(typeof(CAMLOperator), Operator);
            var fieldTypeName = Enum.GetName(typeof(CAMLFieldType), FieldType);
            var xml = 
                Operator == CAMLOperator.IsNull
                ? new XElement(opName, new XElement("FieldRef", new XAttribute("Name", FeildInternalName)))
                : new XElement(opName,
                         new XElement("FieldRef",
                                      new XAttribute("Name", FeildInternalName),
                                      new XAttribute("LookupId", IsLookupId ? "TRUE" : "FALSE")),
                         new XElement("Value",
                                      new XAttribute("Type", fieldTypeName), NodeValue));
            return xml.ToString();
        }
    }

    internal class CAMLGenerator
    {
        public static IEnumerable<string> BuilderFieldQuery(IEnumerable<CAMLPredicateData> datas, List<string> extraList)
        {
            var res = datas.Select(data => data.ToString()).ToList();
            return res.Union(extraList);
        }

        public static string JoinFilters(IList<string> parts, string connector)
        {
            var retVal = new List<string>();
            if (parts.Count() == 0) return string.Empty;
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using (var xmlWriter = XmlWriter.Create(sb, settings))
            {
                var itemCount = 0;
                for (var i = 0; i < parts.Count; i++)
                {
                    if (itemCount == 0 && parts.Count > 1 && (i + 1) != parts.Count)
                    {
                        xmlWriter.WriteStartElement(connector);
                    }
                    xmlWriter.WriteRaw(parts[i]);
                    itemCount++;
                    if (itemCount == 2 || (i + 1) == parts.Count)
                    {
                        itemCount = 0;
                        if (((i + 1) % 2) == 0)
                            xmlWriter.WriteEndElement();

                        xmlWriter.Flush();
                        retVal.Add(sb.ToString());
                        sb.Remove(0, sb.Length);
                    }
                }
            }
            return retVal.Count == 1
                ? retVal.FirstOrDefault()
                : JoinFilters(retVal, connector);
        }
    }
}
