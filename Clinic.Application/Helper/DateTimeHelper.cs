using Clinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Helper
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo EgyptTimeZone = GetEgyptTimeZone();

        /// <summary>
        /// Cross-platform: يشتغل على Windows و Linux
        /// </summary>
        private static TimeZoneInfo GetEgyptTimeZone()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            else
                return TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");
        }

        /// <summary>
        /// Converts any DateTime to UTC safely.
        /// Handles Unspecified as Egypt Local Time.
        /// </summary>

        //public static DateTime ToUtc(DateTime dateTime)
        //{
        //    return dateTime.Kind switch
        //    {
        //        DateTimeKind.Utc => dateTime,

        //        // ✅ بدل ما نعتمد على توقيت السيرفر، نعامله كـ Unspecified
        //        DateTimeKind.Local => TimeZoneInfo.ConvertTimeToUtc(
        //            DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified),
        //            EgyptTimeZone),

        //        DateTimeKind.Unspecified => TimeZoneInfo.ConvertTimeToUtc(dateTime, EgyptTimeZone),

        //        _ => dateTime
        //    };
        //}

        public static DateTime ToUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            // أي حاجة جاية من السيستم عندك اعتبرها Egypt
            var unspecified = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(unspecified, EgyptTimeZone);
        }

        /// <summary>
        /// Converts UTC to Egypt Local Time.
        /// </summary>
        public static DateTime ToEgyptTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, EgyptTimeZone);
        }

        /// <summary>
        /// Gets start and end of day in UTC based on Egypt local date.
        /// </summary>
        public static (DateTime startUtc, DateTime endUtc) GetDayRangeUtc(DateTime date)
        {
            // ✅ Fix: date.Date مش [date.Date]
            var localDate = date.Kind == DateTimeKind.Utc
                ? ToEgyptTime(date).Date
                : date.Date;

            var startLocal = DateTime.SpecifyKind(localDate, DateTimeKind.Unspecified);
            var endLocal = DateTime.SpecifyKind(localDate.AddDays(1), DateTimeKind.Unspecified);

            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, EgyptTimeZone);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, EgyptTimeZone);

            return (startUtc, endUtc);
        }

        /// <summary>
        /// Parses string dates and converts to UTC treating input as Egypt time.
        /// </summary>
        public static Dictionary<Guid, DateTime> ParseDictionaryDatesToUtc(
            Dictionary<Guid, string> input)
        {
            if (input == null)
                return new Dictionary<Guid, DateTime>();

            return input.ToDictionary(
                x => x.Key,
                x =>
                {
                    if (!DateTime.TryParse(x.Value, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsed))
                        throw new FormatException($"Invalid date format: {x.Value}");

                    // ✅ دايمًا عامله كـ Unspecified عشان TryParse ممكن يرجع Local
                    var unspecified = DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
                    return TimeZoneInfo.ConvertTimeToUtc(unspecified, EgyptTimeZone);
                }
            );
        }

        /// <summary>
        /// Normalizes DateTime dictionary to UTC.
        /// </summary>
        public static Dictionary<Guid, DateTime> NormalizeDictionaryToUtc(
            Dictionary<Guid, DateTime> input)
        {
            if (input == null)
                return new Dictionary<Guid, DateTime>();

            return input.ToDictionary(
                x => x.Key,
                x => ToUtc(x.Value)
            );
        }

        /// <summary>
        /// Parses string dates keeping them as Egypt local (Unspecified).
        /// </summary>
        public static Dictionary<Guid, DateTime> ParseDictionaryDatesToEgypt(
            Dictionary<Guid, string> input)
        {
            if (input == null)
                return new Dictionary<Guid, DateTime>();

            return input.ToDictionary(
                x => x.Key,
                x =>
                {
                    if (!DateTime.TryParse(x.Value, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsed))
                        throw new FormatException($"Invalid date format: {x.Value}");

                    return DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
                }
            );
        }

        public static Day ConvertToCustomDay(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Saturday => Day.Saturday,
                DayOfWeek.Sunday => Day.Sunday,
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                DayOfWeek.Friday => Day.Friday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek))
            };
        }
    }
}
