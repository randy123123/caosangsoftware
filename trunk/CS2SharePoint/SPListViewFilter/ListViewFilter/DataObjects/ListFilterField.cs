using System;
using System.Xml.Linq;
using ListViewFilter.Extensions;

namespace ListViewFilter.DataObjects
{
    ///<summary>
    ///</summary>
    public class ListFilterField
    {
        ///<summary>
        /// Field internal name
        ///</summary>
        public string InternalName { get; set; }
        ///<summary>
        /// Caption represents text showing in user interface
        ///</summary>
        public string Caption { get; set; }
        ///<summary>
        /// Filter type
        ///</summary>
        public FilterType Type { get; set; }
        ///<summary>
        /// Index number
        ///</summary>
        public int Position { get; set; }


        ///<summary>
        /// Getting ListFilterField instance from XElement
        ///</summary>
        ///<param name="xml"></param>
        public ListFilterField(XElement xml)
        {
            InternalName = xml.AttributeValue("InternalName");
            Caption = xml.AttributeValue("Caption");
            Type = (FilterType)Enum.Parse(typeof(FilterType), xml.AttributeValue("Type"));
            Position = xml.AttributeValueInteger("Position");
        }

        ///<summary>
        /// Getting blank ListFilterField instance
        ///</summary>
        public ListFilterField()
        {
        }
    }
}
