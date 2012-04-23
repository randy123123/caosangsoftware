using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace CSSoft
{
    public partial class CS2Convert
    {
        public static DateTime? ToDateTime(object obj)
        {
            try
            {
                DateTime? result = null;
                if (obj != null)
                    result = Convert.ToDateTime(obj);
                return result;
            }
            catch { return null; }
        }
        public static bool IsEmail(string emailInput)
        {
            return CS2Regex.IsEmail(emailInput);
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
            return CS2Regex.IsTime(timeInput);
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
        public static DateTime? ToDateTime(string strDateTime, string DATE_FORMAT)
        {
            try
            {
                if (String.IsNullOrEmpty(strDateTime))
                    return null;
                else
                    return DateTime.ParseExact(strDateTime, DATE_FORMAT, null);
            }
            catch { return null; }
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
        public static Decimal ToDecimal(object decimalNumber)
        {
            return ToDecimal(ToString(decimalNumber));
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
        public static int ToInt(object number)
        {
            if (number != null && IsInt(number.ToString()))
                return int.Parse(number.ToString());
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
        public static string DateSubfix(int day)
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

        public const string ICON_WORD = "/_layouts/images/ICDOCX.GIF";
        public const string ICON_EXCEL = "/_layouts/images/ICXLSX.PNG";
        public const string ICON_IMAGE = "/_layouts/images/ICBMP.GIF";
        public const string ICON_PDF = "/_layouts/images/ITDECIS.PNG";
        public const string ICON_OTHER = "/_layouts/images/ITDECIS.PNG";
        public static string GetIconUrl(string docIcon)
        {
            docIcon = docIcon.ToLower();
            if (ValueIs(docIcon, "doc", "docx")) { return ICON_WORD; }
            else if (ValueIs(docIcon, "xls", "xlsx", "csv")) { return ICON_EXCEL; }
            else if (ValueIs(docIcon, "bmp", "gif", "jpg", "png", "psd", "tif")) { return ICON_IMAGE; }
            else if (ValueIs(docIcon, "pdf")) { return ICON_PDF; }
            else return ICON_OTHER;
        }

        public static SPUser ToSPUser(object obj)
        {
            SPFieldUserValue user = ToSPFieldUserValue(obj);
            if (user == null) return null;
            try
            {
                return user.User;
            }
            catch { return null; }
        }

        public static SPFieldUserValue ToSPFieldUserValue(object obj)
        {
            if(obj == null) return null;
            try
            {
                SPFieldUserValue lookupValue = new SPFieldUserValue(CS2Web.CurrentWeb, ToString(obj));
                return lookupValue;
            }
            catch { return null; }
        }

        public static SPFieldUserValueCollection ToSPFieldUserValueCollection(object obj)
        {
            if(obj == null) return null;
            try
            {
                SPFieldUserValueCollection lookupValue = new SPFieldUserValueCollection(CS2Web.CurrentWeb, ToString(obj));
                return lookupValue;
            }
            catch { return null; }
        }
        private static readonly string[] XMLSpecialChars = new string[] { "&", "<", ">", "\"", "'" };
        private static readonly string[] XMLSpecialCharsReplace = new string[] { "&amp;", "&lt;", "&gt;", "&quot;", "&apos;" };
        public static string ReplaceXMLSpecialChars(string value)
        {
            for (int i = 0; i < XMLSpecialChars.Count(); i++ )
            {
                value = value.Replace(XMLSpecialChars[i], XMLSpecialCharsReplace[i]);
            }
            return value;
        }
    }
}