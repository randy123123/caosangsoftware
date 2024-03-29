﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Text.RegularExpressions;

namespace Dev4Side.SP2010.FilteredLookup
{

  #region Util class
  internal sealed class Util {

    #region ListIsNullOrEmpty method
    /// <summary>
    /// Indicates whether the specified generic list object is null or empty
    /// </summary>
    /// <param name="list">A generic list</param>
    /// <returns>A value indicating whether the specified list object is null or empty</returns>
    internal static bool ListIsNullOrEmpty(List<ListItem> list) {
      return (list != null && list.Count > 0) ? false : true;
    }
    #endregion

    #region GetAvailableValues method
    internal static List<ListItem> GetAvailableValues(SPItem CurrentItem, FilteredLookupField f, HttpContext ctx) {
      List<ListItem> _v = null;
      SPListItemCollection items = null;
      Guid fId = new Guid(f.LookupField);

      SPSite s = SPControl.GetContextSite(ctx);
      SPWeb lookupWeb = s.OpenWeb(f.LookupWebId);
      SPList lookupList = lookupWeb.Lists[new Guid(f.LookupList)];
          try
          {
              if (f.QueryFilter != null)
              {
                  SPQuery q = f.QueryFilter;
                  string queryFilter = q.Query;
                  if (!String.IsNullOrEmpty(queryFilter) && queryFilter.Contains('['))
                  {
                      IEnumerable<string> fiels = GetSubStrings(queryFilter, "[", "]");
                      foreach (string fieldName in fiels)
                      {
                          try
                          {
                              if (fieldName.Contains(","))
                              {
                                  string[] fieldNameSplit = fieldName.Split(',');
                                  if (CurrentItem[fieldNameSplit[0]] == null)
                                      queryFilter = queryFilter.Replace(String.Format("[{0}]", fieldName), fieldNameSplit[1]);
                                  else
                                  {
                                      if(CurrentItem[fieldNameSplit[0]] is SPFieldLookupValue)
                                        queryFilter = queryFilter.Replace(String.Format("[{0}]", fieldName), ((SPFieldLookupValue)CurrentItem[fieldNameSplit[0]]).LookupValue);
                                      else
                                        queryFilter = queryFilter.Replace(String.Format("[{0}]", fieldName), (string)CurrentItem[fieldNameSplit[0]]);
                                  }
                              }
                              else if (CurrentItem.Fields.ContainsField(fieldName))
                                  queryFilter = queryFilter.Replace(String.Format("[{0}]", fieldName), (string)CurrentItem[fieldName]);
                          }
                          catch { }
                      }
                      q.Query = queryFilter;
                  }
                  items = lookupList.GetItems(q);
              }
          }
          catch { }
          if (items == null) { items = lookupList.Items; }
             
      if ((items != null && items.Count > 0)) {
        _v = items
          .Cast<SPListItem>()
          .Where(e => e[fId] != null)
          .Select(e => new ListItem((
            e.Fields[fId].GetFieldValueAsText(e[fId])), e.ID.ToString()))
          .ToList<ListItem>();
      }

      return _v;
    }
    #endregion

    internal static IEnumerable<string> GetSubStrings(string input, string start, string end)
    {
        Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
        MatchCollection matches = r.Matches(input);
        foreach (Match match in matches)
            yield return match.Groups[1].Value;
    }
  } 
  #endregion

  #region Extensions class
  internal static class Extensions {
    // TO DO
    /// <summary>
    /// Indicates whether a field in a list is associated with a SPFolder content type
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    internal static bool AssociatedWithFolder(this SPField field) {
      // THIS IS WORK IN PROGRESS
      if (field != null) {
        SPList list = field.ParentList;
      }

      return false;
    }
  } 
  #endregion
}
