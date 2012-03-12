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
    #region Utilites Class
    public partial class Common
    {
        public static DateTime? ToDateTime(object obj)
        {
            DateTime? result = null;
            if (obj != null)
                result = (DateTime)obj;
            return result;
        }
        public static bool IsDateTime(string dateInput, string dateFormat)
        {
            try
            {
                DateTime.ParseExact(dateInput, dateFormat, null);
                return true;
            }
            catch { return false; }
        }
        public static bool IsTime(string timeInput)
        {
            try
            {
                Regex rgx = new Regex("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
                return rgx.IsMatch(timeInput);
            }
            catch { return false; }
        }
        public static bool CompareTimeGreaterThan(string time1, string time2, string timeFormat)
        {
            try
            {
                DateTime dt1 = ToDateTime(time1, timeFormat).Value;
                DateTime dt2 = ToDateTime(time2, timeFormat).Value;
                return dt1 < dt2;
            }
            catch { return false; }
        }
        public static bool ValueIs(string value, params string[] values)
        {
            return values.Contains(value);
        }
        public static bool ValueInList(string value, params string[] listValues)
        {
            foreach (string chkValue in listValues)
                if (value == chkValue) return true;
            return false;
        }
        public static DateTime? ToDateTime(string strDateTime, string DATE_FORMAT)
        {
            if (String.IsNullOrEmpty(strDateTime))
                return null;
            else
                return DateTime.ParseExact(strDateTime, DATE_FORMAT, null);
        }
        public static bool IsDecimal(string decimalNumber)
        {
            if (decimalNumber.Contains(","))
                decimalNumber = decimalNumber.Replace(',', '.');
            decimal d; return Decimal.TryParse(decimalNumber, out d);
        }
        public static bool IsInt(string number)
        {
            int i; return int.TryParse(number, out i);
        }
        public static bool IsCorrectDecimalFormat(string decimalNumber)
        {
            if (decimalNumber.Contains(","))
                decimalNumber = decimalNumber.Replace(',', '.');
            string tailer = "";
            if (decimalNumber.Contains("."))
                tailer = decimalNumber.Substring(decimalNumber.LastIndexOf('.'));
            return tailer.Length <= 3;
        }


        public static int CompareString(string s1, string s2, bool asc)
        {
            if (String.IsNullOrEmpty(s1)) s1 = "";
            if (String.IsNullOrEmpty(s2)) s2 = "";
            if (asc) return s1.CompareTo(s2);
            else return s2.CompareTo(s1);
        }
        public static int CompareDateTime(DateTime? d1, DateTime? d2, bool asc)
        {
            if (d1 == null) d1 = new DateTime();
            if (d2 == null) d2 = new DateTime();
            if (asc) return d1.Value.CompareTo(d2.Value);
            else return d2.Value.CompareTo(d1.Value);
        }

        public static Decimal ToDecimal(string decimalNumber)
        {
            if (decimalNumber.Contains(","))
                decimalNumber = decimalNumber.Replace(',', '.');
            if (IsDecimal(decimalNumber))
                return Decimal.Parse(decimalNumber);
            else return 0;
        }
        public static int ToInt(string number)
        {
            if (IsInt(number))
                return int.Parse(number);
            else return 0;
        }
        public static string ToStringWithComma(string[] arr)
        {
            StringBuilder sb = new StringBuilder();
            if (arr.Count() > 0)
            {
                foreach (string s in arr)
                {
                    if (sb.Length > 0) sb.AppendFormat(", {0}", s);
                    else sb.Append(s);
                }
            }
            return sb.ToString();
        }
        public static string ToStringWithDateTime(DateTime? obj, string dateFormat)
        {
            return obj == null ? "" : obj.Value.ToString(dateFormat);
        }
        public static string AttachmentsDateTimeToString(DateTime dateTime) //[Return] at 23:00 on June 21th
        {
            return String.Format("at {0} on {1} {2}", dateTime.ToString("hh:mm"), dateTime.ToString("MMMM"), DateSubfix(dateTime.Day));
        }
        private static string DateSubfix(int day)
        {
            string subfix = "th";
            if (day == 1 || day == 21 || day == 31) subfix = "st";
            if (day == 2 || day == 22) subfix = "nd";
            if (day == 3 || day == 23) subfix = "rd";
            return day.ToString() + subfix;
        }
        public static string ToString(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }
        public static SPFieldLookupValue ToLookupValue(object obj)
        {
            return obj == null ? null : new SPFieldLookupValue(ToString(obj));
        }
        public static SPFieldLookupValueCollection ToLookupValueCollection(object obj)
        {
            return obj == null ? null : new SPFieldLookupValueCollection(ToString(obj));
        }
        public static string ReplaceStringSetValue(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Replace("<", "&lt;").Replace(">", "&gt;");
        }
        public static string ReplaceStringGetValue(string value)
        {
            return value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
        }
        public static bool ToBoolean(object obj)
        {
            return obj == null ? false : bool.Parse(obj.ToString());
        }

        public static void AppendCAML(StringBuilder result, StringBuilder limit, string condition)
        {
            bool addMultiCondition = result.Length > 0;
            result.Append(limit);
            if (addMultiCondition)
            {
                result.Insert(0, String.Format("<{0}>", condition));
                result.AppendFormat("</{0}>", condition);
            }
        }
        public static void AppendCAML(StringBuilder result, string condition, string operatorQuery, string fieldName, string fieldType, string value)
        {
            AppendCAML(result, condition, operatorQuery, fieldName, fieldType, value, false);
        }
        public static void AppendCAML(StringBuilder result, string condition, string operatorQuery, string fieldName, string fieldType, string value, bool queryByLookupId)
        {
            bool addMultiCondition = result.Length > 0;
            result.Append(Value2CAML(operatorQuery, fieldName, fieldType, value, "", queryByLookupId));
            if (addMultiCondition)
            {
                result.Insert(0, String.Format("<{0}>", condition));
                result.AppendFormat("</{0}>", condition);
            }
        }

        public static void AppendCAML(StringBuilder result, string condition, string operatorQuery, string fieldName, string fieldType, string value, string userDateFormat)
        {
            bool addMultiCondition = result.Length > 0;
            result.Append(Value2CAML(operatorQuery, fieldName, fieldType, value, userDateFormat));
            if (addMultiCondition)
            {
                result.Insert(0, String.Format("<{0}>", condition));
                result.AppendFormat("</{0}>", condition);
            }
        }
        public static string Value2CAML(string operatorQuery, string fieldName, string fieldType, string value, string userDateFormat)
        {
            return Value2CAML(operatorQuery, fieldName, fieldType, value, userDateFormat, false);
        }
        public static string Value2CAML(string operatorQuery, string fieldName, string fieldType, string value, string userDateFormat, bool queryByLookupId)
        {
            if ("DateTime".Equals(fieldType))
            {
                DateTime dateValue = DateTime.ParseExact(value, userDateFormat, null);
                value = dateValue.ToString("yyyy-MM-dd");
            }
            string valueQuery = "";
            if (!String.IsNullOrEmpty(fieldType))
                valueQuery = String.Format("<Value Type=\"{0}\">{1}</Value>", fieldType, value);
            if (queryByLookupId)
                return String.Format("<{0}><FieldRef Name=\"{1}\" LookupId=\"TRUE\"/>{2}</{0}>", operatorQuery, fieldName, valueQuery);
            else
                return String.Format("<{0}><FieldRef Name=\"{1}\" />{2}</{0}>", operatorQuery, fieldName, valueQuery);
        }
        //public static void AppendOrderBy(StringBuilder result, params string[] fields)
        //{
        //    if (fields.Length == 0) return;
        //    string orderBy = String.Format("<OrderBy>{0}</OrderBy>", GetViewFields(fields));
        //    result.Append(orderBy);
        //}
        public static void AppendWhere(StringBuilder result)
        {
            if (result.Length == 0)
                AppendCAML(result, "", "Neq", "ID", "Counter", "0");
            result.Insert(0, "<Where>");
            result.Append("</Where>");
        }

    }

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

    #region Serialize and DeSerialize
    public class GenericSerialize<T>
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
    #endregion Serialize and DeSerialize
    #endregion Utilites Class
}