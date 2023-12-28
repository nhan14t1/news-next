using NEWS.Entities.Constants;

namespace NEWS.Entities.Extensions
{
    public static class DateExtensions
    {
        public static long ToTimeStamp(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Utc)
            {
                return (long)(date - AppConst.BASE_DATE_UTC).TotalMilliseconds;
            }

            return (long)(date - AppConst.BASE_DATE).TotalMilliseconds;
        }

        public static DateTime ToDate(this int timeStamp)
        {
            return AppConst.BASE_DATE.AddMilliseconds(timeStamp);
        }

        public static DateTime ToDate(this long timeStamp)
        {
            return AppConst.BASE_DATE.AddMilliseconds(timeStamp);
        }

        public static DateTime ToClientDate(this DateTime date, float timezone)
        {
            return date.AddHours(timezone);
        }
    }
}
