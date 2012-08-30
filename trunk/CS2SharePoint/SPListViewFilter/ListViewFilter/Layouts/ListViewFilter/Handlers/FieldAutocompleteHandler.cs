using System;
using System.Linq;
using System.Text;
using System.Web;
using ListViewFilter.Extensions;
using Microsoft.SharePoint;

namespace ListViewFilter.Layouts.ListViewFilter.Handlers
{
    ///<summary>
    ///</summary>
    public class FieldAutocompleteHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            var query = context.Request.QueryString["term"];
            var listId = new Guid(context.Request.QueryString["List"]);
            var fieldName = context.Request.QueryString["FieldName"];
            var fieldId = string.IsNullOrEmpty(fieldName)
                              ? new Guid(context.Request.QueryString["Field"])
                              : Guid.Empty;
            var web = SPContext.Current.Web;
            var list = web.Lists.GetList(listId, true, false);
            var field = string.IsNullOrEmpty(fieldName)
                              ? list.Fields[fieldId]
                              : list.Fields.GetFieldByInternalName(fieldName);
            var allVals = field.DistinctValues();
            var vals = string.IsNullOrEmpty(query)
                           ? allVals
                           : allVals.Where(v => v.ToLower().Contains(query.ToLower()));
            var array = vals.Select(x => string.Format(@"{{""id"":""0"",""value"":""{0}"",""label"":""{0}""}}", x))
                .Take(20).ToArray();
            context.Response.Write("[");
            context.Response.Write(string.Join(",", array));
            context.Response.Write("]");
            context.Response.End();
        }
    }
}
