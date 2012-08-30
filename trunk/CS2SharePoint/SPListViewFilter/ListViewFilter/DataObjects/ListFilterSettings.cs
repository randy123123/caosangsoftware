using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ListViewFilter.WebParts.SPListViewFilter;

namespace ListViewFilter.DataObjects
{
    /// <summary>
    /// Settings of ListFilter WebPart
    /// </summary>
    public class ListFilterSettings
    {
        private ListFilterSettings(XDocument element)
        {
            var fields = element.Element("Filter").Elements("Field");
            Fields = fields.Select(x => new ListFilterField(x)).ToList();
        }

        private ListFilterSettings()
        {
            Fields = new List<ListFilterField>();
        }

        ///<summary>
        /// Fields
        ///</summary>
        public IEnumerable<ListFilterField> Fields { get; private set; }

        ///<summary>
        /// Get settings of ListFilter WebPart instance
        ///</summary>
        ///<param name="listFilter">ListFilder WebPart</param>
        ///<returns>Intance of ListFilterSettings</returns>
        public static ListFilterSettings GetCurrent(SPListViewFilter listFilter)
        {
            if (string.IsNullOrEmpty(listFilter.FilterDefinitionString))
                return Empty;
            var xml = XDocument.Parse(listFilter.FilterDefinitionString);
            return new ListFilterSettings(xml);
        }

        ///<summary>
        /// Blank ListFilterSettings intance
        ///</summary>
        public static ListFilterSettings Empty
        {
            get
            {
                return new ListFilterSettings();
            }
        }
    }
}
