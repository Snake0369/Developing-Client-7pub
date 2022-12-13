using System;

namespace Trading.Bot.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfHour(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind);
        }

        public static DateTime StartOfTimeframe(this DateTime dateTime, TimeSpan timeframe)
        {
            if (dateTime < dateTime.StartOfBaseSession())
            {
                return dateTime.StartOfDay();    
            }
            var count = new TimeSpan(24, 0, 0).TotalMinutes / timeframe.TotalMinutes;
            return Enumerable.Range(0, (int)count)
                .Select(i => dateTime.StartOfDay().Add(i * timeframe))
                .ToList()
                .Last(x => x < dateTime);
        }

        public static DateTime EndOfTimeframe(this DateTime dateTime, TimeSpan timeframe)
        {
            if (dateTime < dateTime.StartOfBaseSession())
            {
                return dateTime.StartOfBaseSession();
            }
            var count = new TimeSpan(24, 0, 0).TotalMinutes / timeframe.TotalMinutes;
            return Enumerable.Range(0, (int)count)
                .Select(i => dateTime.StartOfDay().Add(i * timeframe))
                .ToList()
                .First(x => x > dateTime);
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Kind);
        }

        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Kind);
        }

        public static DateTime StartOfBaseSession(this DateTime dateTime)
        {
            if (dateTime < new DateTime(2010, 5, 17))
            {
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 30, 0);
            }
            if (dateTime > new DateTime(2021, 3, 1) && dateTime < new DateTime(2022, 2, 24))
            {
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 7, 0, 0);
            }
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 0, 0);
        }

        public static DateTime EndOfBaseSession(this DateTime dateTime, TimeSpan timeframe)
        {
            var count = new TimeSpan(24, 0, 0).TotalMinutes / timeframe.TotalMinutes;
            return Enumerable.Range(0, (int)count + 1)
                .Select(i => dateTime.StartOfDay().Add(i * timeframe))
                .ToList()
                .First(x => x >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 45, 0));
        }

        public static DateTime? StartOfEveningSession(this DateTime dateTime)
        {
            if (dateTime > new DateTime(2022, 2, 24, 0, 0, 0) && dateTime < new DateTime(2022, 7, 12, 0, 0, 0))
            {
                return null;
            }
            var dateTimeUtc = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 19, 0, 0);
            return dateTimeUtc;
        }

        public static DateTime? EndOfEveningSession(this DateTime dateTime, TimeSpan timeframe)
        {
            if (dateTime > new DateTime(2022, 2, 24, 0, 0, 0) && dateTime < new DateTime(2022, 7, 12, 0, 0, 0))
            {
                return null;
            }
            var count = new TimeSpan(24, 0, 0).TotalMinutes / timeframe.TotalMinutes;
            return Enumerable.Range(0, (int)count + 1)
                .Select(i => dateTime.StartOfDay().Add(i * timeframe))
                .ToList()
                .First(x => x >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 50, 0));
        }
    }
}
