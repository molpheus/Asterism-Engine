using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.System
{
    public sealed class Cron
    {
        public string Minute { get; }
        public string Hour { get; }
        public string Day { get; }
        public string Month { get; }
        public string Week { get; }

        public Cron(string minute, string hour, string day, string month, string week)
        {
            Minute = minute;
            Hour = hour;
            Day = day;
            Month = month;
            Week = week;
        }

        public bool IsMatch(DateTime dateTime)
        {
            if (!Minute.Contains("*") && !Minute.Contains(dateTime.Minute.ToString()))
                return false;

            if (!Hour.Contains("*") && !Hour.Contains(dateTime.Hour.ToString()))
                return false;

            if (!Day.Contains("*") && !Day.Contains(dateTime.Day.ToString()))
                return false;

            if (!Month.Contains("*") && !Month.Contains(dateTime.Month.ToString()))
                return false;

            if (!Week.Contains("*") && !Week.Contains(dateTime.DayOfWeek.ToString()))
                return false;

            return true;
        }
    }
}
