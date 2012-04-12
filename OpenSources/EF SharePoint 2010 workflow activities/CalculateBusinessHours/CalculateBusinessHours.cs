using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace EFSPWFActivities
{
    public class CalculateBusinessHours : Activity
    {
        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context",
            typeof(WorkflowContext), typeof(CalculateBusinessHours));

        [Description("Context")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WorkflowContext __Context
        {
            get
            {
                return ((WorkflowContext)(base.GetValue(__ContextProperty)));
            }
            set
            {
                base.SetValue(__ContextProperty, value);
            }
        }

        public static DependencyProperty Date1ValueProperty =
            DependencyProperty.Register("Date1Value",
            typeof(DateTime),
            typeof(CalculateBusinessHours));

        [Description("Date1")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime Date1Value
        {
            get
            {
                return ((DateTime)(base.GetValue(Date1ValueProperty)));
            }
            set
            {
                base.SetValue(Date1ValueProperty, value);
            }
        }

        public static DependencyProperty Date2ValueProperty =
            DependencyProperty.Register("Date2Value",
            typeof(DateTime),
            typeof(CalculateBusinessHours));

        [Description("Date2")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime Date2Value
        {
            get
            {
                return ((DateTime)(base.GetValue(Date2ValueProperty)));
            }
            set
            {
                base.SetValue(Date2ValueProperty, value);
            }
        }

        public static DependencyProperty StartingHourProperty =
            DependencyProperty.Register("StartingHour",
            typeof(string),
            typeof(CalculateBusinessHours));

        [Description("Starting Hour")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string StartingHour
        {
            get
            {
                return ((string)(base.GetValue(StartingHourProperty)));
            }
            set
            {
                base.SetValue(StartingHourProperty, value);
            }
        }

        public static DependencyProperty EndingHourProperty =
            DependencyProperty.Register("EndingHour",
            typeof(string),
            typeof(CalculateBusinessHours));

        [Description("Ending Hour")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string EndingHour
        {
            get
            {
                return ((string)(base.GetValue(EndingHourProperty)));
            }
            set
            {
                base.SetValue(EndingHourProperty, value);
            }
        }

        public static DependencyProperty TimeVariableProperty =
            DependencyProperty.Register("TimeVariable",
            typeof(Double),
            typeof(CalculateBusinessHours));

        [Description("TimeVariable")]
        [Category("EF Workflow Activities")]
        [Browsable(true)]
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Visible)]
        public Double TimeVariable
        {
            get
            {
                return (Double)base.GetValue(TimeVariableProperty);
            }
            set
            {
                base.SetValue(TimeVariableProperty, value);
            }
        }

        public void WriteDebugInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
#if DEBUG
            System.Reflection.Assembly objAssembly = null;
            objAssembly = this.GetType().Assembly;
            FileInfo objFileInfo = new FileInfo(objAssembly.Location);
            string strVersionInfo = string.Empty;
            strVersionInfo = string.Format(@"debug - {0} - {1} - ", objFileInfo.CreationTime, objAssembly.GetName().Version);
            WriteInfoToHistoryLog(web, workflow, strVersionInfo + description);
#endif
        }

        public static void WriteInfoToHistoryLog(SPWeb web, Guid workflow, string description)
        {
            TimeSpan ts = new TimeSpan();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWorkflow.CreateHistoryEvent(web, workflow, 0, web.CurrentUser, ts, "CalculateBusinessHours", description, string.Empty);
            });
        }

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

        private static int GetBusinessDays(DateTime ctStart, DateTime ctEnd)
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

        private static int CorrectFirstDayTime(DateTime ctStart, DateTime ctMaxTime, DateTime ctMinTime)
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

        private static int CorrectLastDayTime(DateTime ctEnd, DateTime ctMaxTime, DateTime ctMinTime)
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

        // thanks for http://www.codeproject.com/tips/57540/Calculate-Business-Hours.aspx
        // fixed a minor bug around "iBusinessDays"
        public double CalculateBusinessHours_GD(DateTime dtStart, DateTime dtEnd)
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

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"Date1Value(start)={0}, Date2Value(end)={1}, StartingHour={2}, EndingHour={3}", Date1Value, Date2Value, StartingHour, EndingHour));
                //DateTime dtEndTime = __Context.Web.RegionalSettings.TimeZone.UTCToLocalTime(Date2Value);
                DateTime dtEndTime = Date2Value;
                TimeVariable = CalculateBusinessHours_GD(Date1Value, dtEndTime);
                WriteDebugInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"TimeVariable={0}, dtEndTime={1}", TimeVariable, dtEndTime));
            }
            catch (Exception ex)
            {
                WriteInfoToHistoryLog(__Context.Web, __Context.WorkflowInstanceId, string.Format(@"ex.Message = {0}, ex.StackTrace = {1}", ex.Message, ex.StackTrace));
                return ActivityExecutionStatus.Faulting;
            }

            return ActivityExecutionStatus.Closed;
        }
    }
}
