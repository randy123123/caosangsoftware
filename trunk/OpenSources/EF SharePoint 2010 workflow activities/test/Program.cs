﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace test
{
    class Program
    {
        public static DateTime AddBusinessDays(DateTime dt, int nDays)
        {
            int weeks = nDays / 5;
            nDays %= 5;
            while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                dt = dt.AddDays(1);

            while (nDays-- > 0)
            {
                dt = dt.AddDays(1);
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(2);
                }
            }
            return dt.AddDays(weeks * 7);
        }

        public static int GetBusinessDays(DateTime ctStart, DateTime ctEnd)
        {
            TimeSpan ctp = ctEnd - ctStart;
            int iDays = ctp.Days + 1;
            int iWeeks = iDays / 7;
            int iBusDays = iWeeks * 5;
            int iRem = iDays % 7;
            while (iRem > 0)
            {
                // no sunday, no saturday
                int iStartDay = (Int32)Enum.Parse(typeof(DayOfWeek), ctStart.DayOfWeek.ToString());
                if (iStartDay != 1 && iStartDay != 7)
                {
                    iBusDays++;
                }
                TimeSpan time1 = new TimeSpan(1, 0, 0, 0);
                ctStart += time1;

                iRem--;
            }
            return iBusDays;
        }

        public static int CorrectFirstDayTime(DateTime ctStart, DateTime ctMaxTime, DateTime ctMinTime)
        {
            Int32 daysec = 0;

            if (ctMaxTime < ctStart) // start time is after max time
            {
                return 0; // zero seconds for the first day
            }
            int iStartDay = (Int32)Enum.Parse(typeof(DayOfWeek), ctStart.DayOfWeek.ToString());
            if (iStartDay == 1 && iStartDay == 7)
            {
                return 0;
            }
            if (ctStart < ctMinTime) // start time is befor min time
            {
                ctStart = ctMinTime; // set start time to min time
            }
            TimeSpan ctSpan = ctMaxTime - ctStart;
            daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
            return daysec;
        }

        public static int CorrectLastDayTime(DateTime ctEnd, DateTime ctMaxTime, DateTime ctMinTime)
        {
            Int32 daysec = 0;

            if (ctMinTime > ctEnd) // start time is after max time
            {
                return 0; // zero seconds for the first day
            }
            int iEndDay = (Int32)Enum.Parse(typeof(DayOfWeek), ctEnd.DayOfWeek.ToString());
            if (iEndDay == 1 && iEndDay == 7)
            {
                return 0;
            }
            if (ctEnd > ctMaxTime) // start time is befor min time
            {
                ctEnd = ctMaxTime; // set start time to min time
            }
            TimeSpan ctSpan = ctEnd - ctMinTime;
            daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
            return daysec;
        }

        //thanks for http://www.codeproject.com/tips/57540/Calculate-Business-Hours.aspx
        public static double CalculateBusinessHours_GD(DateTime dtStart, DateTime dtEnd)
        {
            // initialze our return value
            double OverAllMinutes = 0.0;

            // start time must be less than end time
            if (dtStart > dtEnd)
            {
                return OverAllMinutes;
            }
            DateTime ctTempEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 0, 0, 0);
            DateTime ctTempStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 0, 0, 0);

            // check if startdate and enddate are the same day
            bool bSameDay = (ctTempStart == ctTempEnd);

            // calculate the business days between the dates
            int iBusinessDays = GetBusinessDays(ctTempStart, ctTempEnd);

            // now add the time values to our temp times
            TimeSpan CTimeSpan = new TimeSpan(0, dtStart.Hour, dtStart.Minute, 0);
            ctTempStart += CTimeSpan;
            CTimeSpan = new TimeSpan(0, dtEnd.Hour, dtEnd.Minute, 0);
            ctTempEnd += CTimeSpan;

            int iEndingHour = 0;
            int iEndingMinute = 0;
            int iStartingHour = 0;
            int iStartingMinute = 0;

            // set our workingday time range and correct the first day
            iEndingHour = (int)Math.Floor(Convert.ToDouble(EndingHour));
            iEndingMinute = (int)Math.Round((Convert.ToDouble(EndingHour) - iEndingHour) * 60);
            DateTime ctMaxTime = new DateTime(ctTempStart.Year, ctTempStart.Month, ctTempStart.Day, iEndingHour, iEndingMinute, 0);
            iStartingHour = (int)Math.Floor(Convert.ToDouble(StartingHour));
            iStartingMinute = (int)Math.Round((Convert.ToDouble(StartingHour) - iStartingHour) * 60);
            DateTime ctMinTime = new DateTime(ctTempStart.Year, ctTempStart.Month, ctTempStart.Day, iStartingHour, iStartingMinute, 0);
            int FirstDaySec = CorrectFirstDayTime(ctTempStart, ctMaxTime, ctMinTime);

            // set our workingday time range and correct the last day
            DateTime ctMaxTime1 = new DateTime(ctTempEnd.Year, ctTempEnd.Month, ctTempEnd.Day, iEndingHour, iEndingMinute, 0);
            DateTime ctMinTime1 = new DateTime(ctTempEnd.Year, ctTempEnd.Month, ctTempEnd.Day, iStartingHour, iStartingMinute, 0);
            int LastDaySec = CorrectLastDayTime(ctTempEnd, ctMaxTime1, ctMinTime1);
            int OverAllSec = 0;

            // now sum-up all values
            if (bSameDay)
            {
                if (iBusinessDays != 0)
                {
                    TimeSpan cts = ctMaxTime - ctMinTime;
                    Int32 dwBusinessDaySeconds = (cts.Days * 24 * 60 * 60) + (cts.Hours * 60 * 60) + (cts.Minutes * 60) + cts.Seconds;
                    OverAllSec = FirstDaySec + LastDaySec - dwBusinessDaySeconds;
                }
            }
            else
            {
                if (iBusinessDays > 1)
                {
                    OverAllSec = ((iBusinessDays - 2) * 9 * 60 * 60) + FirstDaySec + LastDaySec;
                }
                else // iBusinessDays == 1
                {
                    OverAllSec = FirstDaySec + LastDaySec;
                }
            }
            OverAllMinutes = OverAllSec / 60;

            return OverAllMinutes / 60;
        }

        public const float StartingHour = 9;
        public const float EndingHour = 17.5f;

        public static void Main(string[] args)
        {
            DateTime dtStart = DateTime.Now.AddMinutes(-1);
            DateTime dtEnd = DateTime.Now;

            double dblHours = 0;
            dblHours = CalculateBusinessHours_GD(dtStart, dtEnd);

            Console.WriteLine(string.Format("dtStart={0}, dtEnd={1}, dblHours={2}", dtStart, dtEnd, dblHours));

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
