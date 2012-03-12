using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.SharePoint.PermissionEx
{
    static class JsEncoder
    {
        static public string Encode(string s)
        {
            if (String.IsNullOrEmpty(s)) return "";

            StringBuilder sb = new StringBuilder(s);

            sb.Replace("\"", "\\\"");
            sb.Replace("'", "\\\'");
            sb.Replace("\r", "\\r");
            sb.Replace("\n", "\\n");

            return sb.ToString();
        }

    }
}
