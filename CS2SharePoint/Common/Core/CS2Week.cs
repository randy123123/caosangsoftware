using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Globalization;
using System.Diagnostics;

namespace CSSoft
{
    [Serializable]
    public partial class CS2Week
    {
        #region Properties
        public DateTime FirstDateOfWeek { get; set; }
        public DateTime LastDateOfWeek { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        #endregion
        #region Init
        public CS2Week() { }
        public CS2Week(string lastDateOfWeek) 
        {
            DateTime date = CS2Convert.ToDateTime(lastDateOfWeek).Value;
            UpdateData(date);
        }
        public CS2Week(DateTime lastDateOfWeek) 
        {
            UpdateData(lastDateOfWeek);
        }
        #endregion
        #region Methods
        public void UpdateData(DateTime lastDateOfWeek)
        {
            LastDateOfWeek = lastDateOfWeek;
            FirstDateOfWeek = lastDateOfWeek.AddDays(-(int)DayOfWeek.Saturday);
            WeekNumber = CS2Week.GetWeekNumber(LastDateOfWeek);
            Year = LastDateOfWeek.Year;
        }
        #endregion
        #region Static Methods
        public static DateTime CalculateLastDateOfWeek(int week, int year)
        {
            DateTime result = new DateTime(year, 1, 1);
            result = CalculateLastDateOfWeek(result);
            int weeknumber = GetWeekNumber(result);
            if (weeknumber <= 1)
                week -= 1;
            result = result.AddDays(7 * week);
            return result;
        }
        //public static DateTime FirstDateOfWeek2(int year, int weekNum, CalendarWeekRule rule)
        //{
        //    DateTime jan1 = new DateTime(year, 1, 1);
        //    int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
        //    DateTime firstMonday = jan1.AddDays(daysOffset);
        //    var cal = CultureInfo.CurrentCulture.Calendar;
        //    int firstWeek = cal.GetWeekOfYear(jan1, rule, DayOfWeek.Monday);
        //    if (firstWeek <= 1)
        //        weekNum -= 1;
        //    DateTime result = firstMonday.AddDays(weekNum * 7);
        //    return result;
        //}

        public static DateTime CalculateLastDateOfWeek(DateTime date)
        {
            return date.AddDays(1 - (int)date.DayOfWeek + (int)DayOfWeek.Saturday);
        }
        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        #endregion
    }
}
