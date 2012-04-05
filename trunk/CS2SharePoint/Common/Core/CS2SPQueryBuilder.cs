using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace CSSoft
{
    public partial class CS2SPQueryBuilder
    {
        public const string QUERY_DATE_FORMAT = "yyyy-MM-dd";

        /// <summary>
        /// Gets the view fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetViewFields(params string[] fields)
        {
            if (fields.Count() == 0) return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (string fieldName in fields)
                result.AppendFormat("<FieldRef Name='{0}'/>", fieldName);
            return result.ToString();
        }
        /// <summary>
        /// Appends the CAML.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="condition">The condition.</param>
        /// <remarks></remarks>
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
        /// <summary>
        /// Appends the order by.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="fields">The fields.</param>
        /// <remarks></remarks>
        public static void AppendOrderBy(StringBuilder result, params string[] fields)
        {
            if (fields.Length == 0) return;
            string orderBy = String.Format("<OrderBy>{0}</OrderBy>", GetViewFields(fields));
            result.Append(orderBy);
        }
        /// <summary>
        /// Appends the where.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <remarks></remarks>
        public static void AppendWhere(StringBuilder result)
        {
            if (result.Length == 0)
                AppendCAML(result, "", "Neq", "ID", "Counter", "0");
            result.Insert(0, "<Where>");
            result.Append("</Where>");
        }
        /// <summary>
        /// Appends the CAML.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="operatorQuery">The operator query.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        public static void AppendCAML(StringBuilder result, string condition, string operatorQuery, string fieldName, string fieldType, string value)
        {
            AppendCAML(result, condition, operatorQuery, fieldName, fieldType, value, false);
        }
        /// <summary>
        /// Appends the CAML.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="operatorQuery">The operator query.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="queryByLookupId">if set to true [query by lookup id].</param>
        /// <remarks></remarks>
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
        /// <summary>
        /// Appends the CAML.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="operatorQuery">The operator query.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="userDateFormat">The user date format.</param>
        /// <remarks></remarks>
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
        /// <summary>
        /// Value2s the CAML.
        /// </summary>
        /// <param name="operatorQuery">The operator query.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="userDateFormat">The user date format.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Value2CAML(string operatorQuery, string fieldName, string fieldType, string value, string userDateFormat)
        {
            return Value2CAML(operatorQuery, fieldName, fieldType, value, userDateFormat, false);
        }
        /// <summary>
        /// Value2s the CAML.
        /// </summary>
        /// <param name="operatorQuery">The operator query.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="userDateFormat">The user date format.</param>
        /// <param name="queryByLookupId">if set to true [query by lookup id].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Value2CAML(string operatorQuery, string fieldName, string fieldType, string value, string userDateFormat, bool queryByLookupId)
        {
            if ("DateTime".Equals(fieldType))
            {
                DateTime dateValue = DateTime.ParseExact(value, userDateFormat, null);
                value = dateValue.ToString(QUERY_DATE_FORMAT);
            }
            string valueQuery = "";
            if (!String.IsNullOrEmpty(fieldType))
                valueQuery = String.Format("<Value Type=\"{0}\">{1}</Value>", fieldType, value);
            if (queryByLookupId)
                return String.Format("<{0}><FieldRef Name=\"{1}\" LookupId=\"TRUE\"/>{2}</{0}>", operatorQuery, fieldName, valueQuery);
            else
                return String.Format("<{0}><FieldRef Name=\"{1}\" />{2}</{0}>", operatorQuery, fieldName, valueQuery);
        }
        /// <summary>
        /// Simples the query to make CAML Querry with lookup field
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldID">The ID value.</param>
        /// <returns>The SPQuery</returns>
        public static SPQuery SimpleQueryLookupId(string fieldName, int fieldID)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/> <Value Type='Lookup'>{1}</Value> </Eq></Where>", fieldName, fieldID);
            return query;
        }

        public static bool HasItems(SPListItemCollection itemCollection)
        {
            try
            {
                return itemCollection != null && itemCollection.Count > 0;
            }
            catch { return false; }
        }
    }
}