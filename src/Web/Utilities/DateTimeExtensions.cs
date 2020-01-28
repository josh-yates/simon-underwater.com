using System;

namespace Web.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }
    }
}