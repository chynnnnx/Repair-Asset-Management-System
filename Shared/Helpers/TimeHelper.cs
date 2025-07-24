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
      
    }
}
