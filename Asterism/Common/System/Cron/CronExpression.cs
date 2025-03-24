using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.System.Cron
{
    public class CronExpression : ICronExpression
    {
        public CronField Minute { get; }
        public CronField Hour { get; }
        public CronField Day { get; }
        public CronField Month { get; }
        public CronField Weekday { get; }

        public CronExpression(string expression)
        {
            var parts = expression.Split(' ');
            if (parts.Length != 5)
                throw new ArgumentException("Cron式は5フィールド必要です");

            Minute = new CronField(parts[0], 0, 59);
            Hour = new CronField(parts[1], 0, 23);
            Day = new CronField(parts[2], 1, 31);
            Month = new CronField(parts[3], 1, 12);
            Weekday = new CronField(parts[4], 0, 6); // 0 = 日曜日
        }

        public CronExpression(string minute = "*", string hour = "*", string day = "*", string month = "*", string week = "*")
        {
            Minute = new CronField(minute, 0, 59);
            Hour = new CronField(hour, 0, 23);
            Day = new CronField(day, 1, 31);
            Month = new CronField(month, 1, 12);
            Weekday = new CronField(week, 0, 6); // 0 = 日曜日
        }

        public bool IsMatch(DateTime time)
        {
            return Minute.IsMatch(time.Minute)
                && Hour.IsMatch(time.Hour)
                && Day.IsMatch(time.Day)
                && Month.IsMatch(time.Month)
                && Weekday.IsMatch((int)time.DayOfWeek);
        }

        public bool IsMatchFormat(string format)
        {
            return $"{Minute.Field} {Hour.Field} {Day.Field} {Month.Field} {Weekday.Field}" == format;
        }

        public DateTime GetNextOccurrence(DateTime from)
        {
            var time = from.AddMinutes(1).AddSeconds(-from.Second).AddMilliseconds(-from.Millisecond);

            while (true)
            {
                if (IsMatch(time))
                    return time;

                time = time.AddMinutes(1);
            }
        }

        
    }
}
