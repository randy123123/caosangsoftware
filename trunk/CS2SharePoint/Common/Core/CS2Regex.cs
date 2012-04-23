using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace CSSoft
{
    public partial class CS2Regex
    {
        public static bool IsEmail(string emailInput)
        {
            try
            {
                Regex rgx = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
                return rgx.IsMatch(emailInput);
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
       public static IEnumerable<string> Substring(string inputText, string startWithText, string endWithText)
       {
           FixRegexSpecialCharacters(ref startWithText);
           FixRegexSpecialCharacters(ref endWithText);
           string regularExpressionPattern = String.Format(@"{0}(.*?){1}", startWithText, endWithText);
           Regex re = new Regex(regularExpressionPattern);
           foreach (Match m in re.Matches(inputText))
               yield return m.Groups[1].Value; //because Groups[0] value include split text
       }

       public static void FixRegexSpecialCharacters(ref string value)
       {
           if (CS2Convert.ValueIs(value, RegexSpecialCharacters)) value = String.Format(@"\{0}", value);
       }
       public static string[] RegexSpecialCharacters = new string[] { "[", "(", "{", "}", ")", "]" };
    }
}