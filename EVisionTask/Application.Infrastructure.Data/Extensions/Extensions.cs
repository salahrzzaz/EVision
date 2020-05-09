using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace Application.Infrastructure.Data.Extensions
{
    public static class Extensions
    {
        #region DateTime

        public static bool IsBetween(this DateTime model, DateTime start, DateTime end)
        {
            return model >= start && model <= end;
        }

        public static bool Overlap(this DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1.IsBetween(start2, end2)
                   || end1.IsBetween(start2, end2)
                   || start2.IsBetween(start1, end1) && end2.IsBetween(start1, end1);
        }

        public static string ToTimeAgoString(this DateTime date, Type resourceType, CultureInfo cultureInfo = null)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;
            var resourceManager = new ResourceManager(resourceType);
            cultureInfo = cultureInfo ?? Thread.CurrentThread.CurrentCulture;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1
                    ? resourceManager.GetString("OneSecondAgo", cultureInfo)
                    : string.Format(resourceManager.GetString("SecondsAgo", cultureInfo), ts.Seconds);

            if (delta < 2 * minute)
                return resourceManager.GetString("AMinuteAgo", cultureInfo);

            if (delta < 1 * hour)
                return string.Format(resourceManager.GetString("MinutesAgo", cultureInfo), ts.Minutes);

            if (delta < 2 * hour)
                return resourceManager.GetString("AnHourAgo", cultureInfo);

            if (delta < 24 * hour)
                return string.Format(resourceManager.GetString("HoursAgo", cultureInfo), ts.Hours);

            if (delta < 48 * hour)
                return resourceManager.GetString("Yesterday", cultureInfo);

            if (delta < 30 * day)
                return string.Format(resourceManager.GetString("DaysAgo", cultureInfo), ts.Days);

            if (delta < 12 * month)
            {
                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1
                    ? resourceManager.GetString("OneMonthAgo", cultureInfo)
                    : string.Format(resourceManager.GetString("MonthsAgo", cultureInfo), months);
            }

            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1
                ? resourceManager.GetString("OneYearAgo", cultureInfo)
                : string.Format(resourceManager.GetString("YearsAgo", cultureInfo), years);
        }

        #endregion

        #region Double

        public static bool IsBetween(this double model, double start, double end)
        {
            return model >= start && model <= end;
        }

        public static bool Overlap(this double start1, double end1, double start2, double end2)
        {
            return start1.IsBetween(start2, end2)
                   || end1.IsBetween(start2, end2)
                   || start2.IsBetween(start1, end1) && end2.IsBetween(start1, end1);
        }

        #endregion

        #region Int

        public static bool IsBetween(this int model, int start, int end)
        {
            return model >= start && model <= end;
        }

        public static bool Overlap(this int start1, int end1, int start2, int end2)
        {
            return start1.IsBetween(start2, end2)
                   || end1.IsBetween(start2, end2)
                   || start2.IsBetween(start1, end1) && end2.IsBetween(start1, end1);
        }

        #endregion

        #region Enum

        public static T GetLowestValue<T>(this T values) where T : Enum
        {
            var lowest = Enum.GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(x => x)
                .FirstOrDefault(x => values.HasFlag(x));

            return lowest;
        }

        public static T GetHighestValue<T>(this T values) where T : Enum
        {
            var highest = Enum.GetValues(typeof(T))
                .Cast<T>()
                .OrderByDescending(x => x)
                .FirstOrDefault(x => values.HasFlag(x));

            return highest;
        }

        #endregion

        #region String

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    input = input.Trim();
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        #endregion

        #region Random Values

        private static readonly Lazy<RandomSecureVersion> RandomSecure =
            new Lazy<RandomSecureVersion>(() => new RandomSecureVersion());

        public static IEnumerable<T> ShuffleSecure<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            for (var counter = 0; counter < sourceArray.Length; counter++)
            {
                var randomIndex = RandomSecure.Value.Next(counter, sourceArray.Length);
                yield return sourceArray[randomIndex];

                sourceArray[randomIndex] = sourceArray[counter];
            }
        }

        public static string ShuffleTextSecure(this string source)
        {
            var shuffledChars = source.ShuffleSecure().ToArray();
            return new string(shuffledChars);
        }

        #endregion
    }
}
