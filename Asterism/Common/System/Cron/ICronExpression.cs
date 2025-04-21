using System;

namespace Asterism.System.Cron
{
    public interface ICronExpression
    {
        /// <summary>
        /// 指定した日時がCron式に一致するかどうかを返す
        /// </summary>
        bool IsMatch(DateTime time);
        /// <summary>
        /// 指定されたフォーマットと一致するかどうかを返す
        /// </summary>
        bool IsMatchFormat(string format);
        /// <summary>
        /// 指定した時刻から次に一致する時刻を返す
        /// </summary>
        DateTime GetNextOccurrence(DateTime from);
    }
}
