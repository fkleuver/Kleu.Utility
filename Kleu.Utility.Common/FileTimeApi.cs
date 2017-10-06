using System;
using System.Runtime.InteropServices;

namespace Kleu.Utility.Common
{
    public static class FileTimeApi
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        // ReSharper disable once MemberCanBePrivate.Global
        public static bool IsAvailable { get; }

        static FileTimeApi()
        {
            try
            {
                GetSystemTimePreciseAsFileTime(out _);
                IsAvailable = true;
            }
            catch (EntryPointNotFoundException)
            {
                // Not running Windows 8 or higher.             
                IsAvailable = false;
            }
        }

        /// <summary>
        /// Gets the number of ticks (100-nanosecond intervals) that have elapsed since 01/01/1601 00:00:00 (UTC).
        /// </summary>
        /// <returns></returns>
        public static long GetSystemTimePreciseAsFileTime()
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException("High resolution clock isn't available.");
            }
            GetSystemTimePreciseAsFileTime(out var filetime);
            return filetime;
        }

        /// <summary>
        /// Gets the current date and time in 100-nanosecond precision (UTC).
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSystemTimePrecise()
        {
            var filetime = GetSystemTimePreciseAsFileTime();
            return DateTime.FromFileTimeUtc(filetime);
        }
    }
}
