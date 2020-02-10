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

        public static DateTime Trim(this DateTime dt, DateTimeComponent component)
        {
            switch (component)
            {
                case DateTimeComponent.Second:
                    return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
                default:
                    throw new NotSupportedException();
            }
        }

        public enum DateTimeComponent
        {
            MilliSecond = 0,
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }
    }
}