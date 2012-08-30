using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Microsoft.SharePoint;

namespace ListViewFilter.Extensions
{
    internal static class SPFieldExtensions
    {
        internal static IEnumerable<string> DistinctValues(this SPField field)
        {
            if (field is SPFieldModStat)
            {
                var tempVals = field.DistinctValues(0);
                var statuses = new Dictionary<int, string>
                                   {
                                       {0, SPResource.GetString("ModerationStatusApproved", new object[0])},
                                       {1, SPResource.GetString("ModerationStatusRejected", new object[0])},
                                       {2, SPResource.GetString("ModerationStatusPending", new object[0])},
                                       {3, SPResource.GetString("ModerationStatusDraft", new object[0])},
                                       {4, SPResource.GetString("ModerationStatusScheduled", new object[0])}
                                   };
                return tempVals.Select(t => statuses[t]);
            }
            switch (field.Type)
            {
                case SPFieldType.Lookup:
                    return field.IsMulti()
                               ? field.DistinctMultiLookupValues().Select(v => v.Value)
                               : field.DistinctLookupValues().Select(v => v.Value);
                case SPFieldType.User:
                    return field.IsMulti()
                               ? field.DistinctMultiUserValues().Select(v => v.Value)
                               : field.DistinctUserValues().Select(v => v.Value);
                case SPFieldType.DateTime:
                    return field
                        .DistinctDateTimeValues(DateTime.MinValue)
                        .Where(x => x != DateTime.MinValue)
                        .Select(x => x.ToShortDateString());
                case SPFieldType.Boolean:
                    return field.DistinctValues(false).Select(b => b.ToString());
                default:
                    return field.DistinctValues(string.Empty);
            }
        }

        internal static IEnumerable<TValue> DistinctValues<TValue>(this SPField field)
        {
            return field.DistinctValues(default(TValue));
        }

        internal static Dictionary<int, string> DistinctLookupValues(this SPField field)
        {
            var res = new Dictionary<int, string>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    var listId = field.ParentList.ID;
                    var colName = field.AttributeValue("ColName");
                    var sourceListId = field.SourceListId();
                    using (var cmd = new SqlCommand { CommandType = CommandType.Text })
                    {
                        cmd.CommandText = FieldDistinctLookupValues.Replace("%SqlColName%", colName);
                        cmd.Parameters.Add(new SqlParameter("@ListId", listId));
                        cmd.Parameters.Add(new SqlParameter("@SourceListId", sourceListId));
                        using (var con = new SqlConnection(connectionString))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                if(!res.ContainsKey((int)reader[0]))
                                {
                                    res.Add((int)reader[0], (string)reader[1]);
                                }
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            return res;
        }

        internal static Dictionary<int, string> DistinctMultiLookupValues(this SPField field)
        {
            var res = new Dictionary<int, string>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    var listId = field.ParentList.ID;
                    using (var cmd = new SqlCommand { CommandType = CommandType.Text })
                    {
                        cmd.CommandText = FieldDistinctMultiLookupValues;
                        cmd.Parameters.Add(new SqlParameter("@ListId", listId));
                        cmd.Parameters.Add(new SqlParameter("@FieldId", field.Id));
                        using (var con = new SqlConnection(connectionString))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                res.Add((int)reader[0], (string)reader[1]);
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            return res;
        }

        internal static Dictionary<int, string> DistinctMultiUserValues(this SPField field)
        {
            var res = new Dictionary<int, string>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    using (var cmd = new SqlCommand { CommandType = CommandType.Text })
                    {
                        cmd.CommandText = FieldDistinctMultiUserValues;
                        cmd.Parameters.Add(new SqlParameter("@FieldId", field.Id));
                        using (var con = new SqlConnection(connectionString))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                res.Add((int)reader[0], (string)reader[1]);
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            return res;
        }

        internal static Dictionary<int, string> DistinctUserValues(this SPField field)
        {
            var res = new Dictionary<int, string>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    var colName = field.AttributeValue("ColName");
                    var ordinal = field.AttributeValueInteger("RowOrdinal");
                    var listId = field.ParentList.ID;
                    using (var cmd = new SqlCommand { CommandType = CommandType.Text })
                    {
                        cmd.CommandText = FieldDistinctUserValues.Replace("%SqlColName%", colName);
                        cmd.Parameters.Add(new SqlParameter("@ListId", listId));
                        cmd.Parameters.Add(new SqlParameter("@RowOrdinal", ordinal));
                        using (var con = new SqlConnection(connectionString))
                        {

                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                res.Add((int)reader[0], (string)reader[1]);
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            return res;
        }

        internal static IEnumerable<DateTime> DistinctDateTimeValues(this SPField field, DateTime def)
        {
            var res = new List<DateTime>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    var colName = string.Format("DATEADD(dd, 0, DATEDIFF(dd, 0, {0}))",
                        field.AttributeValue("ColName"));
                    var ordinal = field.AttributeValueInteger("RowOrdinal");
                    var listId = field.ParentList.ID;
                    using (var cmd = new SqlCommand { CommandType = CommandType.Text })
                    {
                        cmd.CommandText = FieldDistinctValues.Replace("%SqlColName%", colName);
                        cmd.Parameters.Add(new SqlParameter("@ListId", listId));
                        cmd.Parameters.Add(new SqlParameter("@RowOrdinal", ordinal));
                        using (var con = new SqlConnection(connectionString))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                res.Add(reader[0] is DateTime
                                    ? (DateTime)reader[0]
                                    : def);
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            return res;
        }

        internal static IEnumerable<TValue> DistinctValues<TValue>(this SPField field, TValue def)
        {
            var res = new List<TValue>();
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    var siteId = field.ParentList.ParentWeb.Site.ID;
                    string connectionString;
                    using (var site = new SPSite(siteId))
                    {
                        connectionString = site.ContentDatabase.DatabaseConnectionString;
                    }
                    var colName = field.AttributeValue("ColName");
                    var ordinal = field.AttributeValueInteger("RowOrdinal");
                    var listId = field.ParentList.ID;
                    using (var cmd = new SqlCommand
                    {
                        CommandType = CommandType.Text
                    })
                    {

                        cmd.CommandText = FieldDistinctValues.Replace("%SqlColName%", colName);
                        cmd.Parameters.Add(new SqlParameter("@ListId", listId));
                        cmd.Parameters.Add(new SqlParameter("@RowOrdinal", ordinal));
                        using (var con = new SqlConnection(connectionString))
                        {
                            cmd.Connection = con;
                            con.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader != null && reader.Read())
                            {
                                res.Add(reader[0] is TValue
                                    ? (TValue)reader[0]
                                    : def);
                            }
                        }
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                });
            if (field.Type == SPFieldType.MultiChoice && typeof(TValue) == typeof(string))
            {
                var vals = res.SelectMany(x => x.ToString().Split(new[] { ";#" }, StringSplitOptions.RemoveEmptyEntries))
                    .Distinct().Select(x => (TValue)Convert.ChangeType(x, typeof(TValue))).OrderBy(x => x);
                res = vals.ToList();
            }
            return res;
        }

        internal static string AttributeValue(this SPField field, string attrName)
        {
            var xml = new XmlDocument();
            xml.LoadXml(field.SchemaXml);
            return xml.DocumentElement.AttributeValue(attrName);
        }

        internal static int AttributeValueInteger(this SPField field, string attrName)
        {
            var xml = new XmlDocument();
            xml.LoadXml(field.SchemaXml);
            return xml.DocumentElement.AttributeValueInteger(attrName);
        }

        public static bool IsMulti(this SPField field)
        {
            var attrVal = field.AttributeValue("Mult");
            return (string.Compare(attrVal, "TRUE") == 0);
        }

        internal static Guid SourceListId(this SPField field)
        {
            var attrVal = field.AttributeValue("List");
            return new Guid(attrVal);
        }

        private const string FieldDistinctValues =
            @"select distinct
                %SqlColName%
            from
                dbo.AllUserData
            where
                tp_ListId = @ListId
                and tp_IsCurrent = 1
                and tp_RowOrdinal = @RowOrdinal
                and tp_DeleteTransactionId = 0x
            order by
                1 asc";

        private const string FieldDistinctUserValues =
            @"select distinct
	            dj.%SqlColName% [Id], 
	            isnull(u.tp_Title, g.Title) [Title]
            from
	            dbo.AllUserData dj (nolock)
            left outer join
	            dbo.Groups g (nolock)
	            on 
		            dj.tp_SiteId = g.SiteId 
		            and
		            dj.%SqlColName% = g.ID 
            left outer join
	            dbo.UserInfo u (nolock)
	            on 
		            dj.tp_SiteId = u.tp_SiteID
		            and
		            dj.%SqlColName% = u.tp_ID
            where
	            dj.tp_IsCurrentVersion = 1
	            and dj.tp_ListId = @ListId
	            and dj.tp_DeleteTransactionId = 0x
	            and dj.tp_CalculatedVersion = 0
                and tp_RowOrdinal = @RowOrdinal";

        private const string FieldDistinctMultiUserValues =
            @"select distinct
	            dj.tp_Id [Id], 
	            isnull(u.tp_Title, g.Title) [Title]
            from
	            dbo.AllUserDataJunctions dj (nolock)
            left outer join
	            dbo.Groups g (nolock)
	            on 
		            dj.tp_SiteId = g.SiteId 
		            and
		            dj.tp_Id = g.ID 
            left outer join
	            dbo.UserInfo u (nolock)
	            on 
		            dj.tp_SiteId = u.tp_SiteID
		            and
		            dj.tp_Id = u.tp_ID
		            and
		            u.tp_IsActive = 1
		            and
		            u.tp_Deleted = 0
            where
	            dj.tp_FieldId = @FieldId
	            and
	            dj.tp_DeleteTransactionId = 0x
	            and
	            dj.tp_IsCurrentVersion = 1
	            and
	            dj.tp_CalculatedVersion = 0";

        private const string FieldDistinctMultiLookupValues =
            @"select distinct
	            dj.tp_Id [Id], 
	            d.nvarchar1 [Title]
            from
	            dbo.AllUserDataJunctions dj (nolock)
	            inner join
		            dbo.AllUserData d (nolock)
	            on
		            dj.tp_Id = d.tp_ID
            where
	            dj.tp_IsCurrentVersion = 1
	            and
	            dj.tp_FieldId = @FieldId
	            and
	            d.tp_ListId = @ListId
	            and
	            d.tp_DeleteTransactionId = 0x";

        private const string FieldDistinctLookupValues =
            @"select distinct
	            ds.%SqlColName% [Id], 
	            dd.nvarchar1 [Title]
            from
	            dbo.AllUserData ds (nolock)
	            inner join
		            dbo.AllUserData dd (nolock)
	            on
		            ds.%SqlColName% = dd.tp_ID
            where
	            ds.tp_IsCurrentVersion = 1
	            and
	            ds.tp_ListId = @ListId
	            and
	            dd.tp_ListId = @SourceListId
	            and
	            dd.tp_DeleteTransactionId = 0x";
    }
}
