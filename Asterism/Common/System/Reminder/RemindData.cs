using System;

namespace Asterism.System.Reminder
{
    [Serializable]
    public sealed class RemindData(DateTime time, string message) : IComparable<RemindData>
    {
        public DateTime Time = time;
        public string Message = message;

        public RemindData(): this(DateTime.MinValue, string.Empty) { }

        public int CompareTo(RemindData other)
        {
            return other.Time.CompareTo(Time);
        }
    }
}
