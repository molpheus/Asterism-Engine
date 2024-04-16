using System;

using Asterism.Common.Extension;

namespace Asterism.System.Reminder
{
    [Serializable]
    public sealed class RemindData(DateTime time, string message) : IComparable<RemindData>, INullable<RemindData>
    {
        public DateTime Time = time;
        public string Message = message;

        public RemindData(): this(DateTime.MinValue, string.Empty) { }

        int IComparable<RemindData>.CompareTo(RemindData other) => other.Time.CompareTo(Time);

        public override bool Equals(object obj)
        {
            if (obj is not RemindData data)
                return false;

            return data.Time == Time && data.Message == Message;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
