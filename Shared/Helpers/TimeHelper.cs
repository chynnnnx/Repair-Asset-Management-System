using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public static class TimeHelper
    {
        public static readonly TimeZoneInfo PhilippineTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");

    
        public static DateTime UtcToPH(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, PhilippineTimeZone);
        }

       
        public static DateTime LocalToPH(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, PhilippineTimeZone);
            }

            return TimeZoneInfo.ConvertTime(dateTime, PhilippineTimeZone);
        }


        public static DateTime PHToUtc(DateTime dateTime)
        {
           
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, PhilippineTimeZone);
            }

           
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, PhilippineTimeZone);
        }

        public static string FormatPH(DateTime dateTime, string format = "yyyy-MM-dd hh:mm:ss tt")
        {
            return UtcToPH(dateTime).ToString(format);
        }

        public static string FormatDuration(DateTime start, DateTime? end = null)
        {
            var phStart = UtcToPH(start);
            DateTime phEnd;
            if (end.HasValue)
            {
                phEnd = UtcToPH(end.Value);
            }
            else
            {
                phEnd = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, PhilippineTimeZone);
            }
            var diff = phEnd - phStart;
            if (diff.TotalMinutes < 1)
                return "Just now";

            var hours = (int)diff.TotalHours;
            var minutes = diff.Minutes;

            var parts = new List<string>();
            if (hours > 0) parts.Add($"{hours} hour{(hours > 1 ? "s" : "")}");
            if (minutes > 0) parts.Add($"{minutes} minute{(minutes > 1 ? "s" : "")}");

            return string.Join(", ", parts);
        }
    }
}