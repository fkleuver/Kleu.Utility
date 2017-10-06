using System;

namespace Kleu.Utility.Common
{

    public static class DateTimeExtensions
    {
        internal static readonly DateTime WindowsFileTimeEpoch = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static readonly DateTime GregorianCalendarEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Uses the TimeZoneInfo for conversion, which will throw on historically invalid DateTimes.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static DateTime RobustToUTC(this DateTime dateTime)
        {
            // See: https://stackoverflow.com/questions/1704780/what-is-the-difference-between-datetime-touniversaltime-and-timezoneinfo-convert
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        public static TimeSpan MinusWindowsFileTimeEpoch(this DateTime dateTime)
        {
            return RobustToUTC(dateTime).Subtract(WindowsFileTimeEpoch);
        }

        public static TimeSpan MinusUnixEpoch(this DateTime dateTime)
        {
            return RobustToUTC(dateTime).Subtract(UnixEpoch);
        }

        public static TimeSpan MinusGregorianCalendarEpoch(this DateTime dateTime)
        {
            return RobustToUTC(dateTime).Subtract(GregorianCalendarEpoch);
        }
    }
}
