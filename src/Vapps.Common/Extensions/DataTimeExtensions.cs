using System;
using Vapps.Helpers;

namespace Vapps
{
    public static class DataTimeExtensions
    {
        /// <summary>
        /// 本地时间转UTC时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeHelper"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static DateTime? LocalTimeConverUtcTime(this DateTime? dateTime, IDateTimeHelper dateTimeHelper, int add = 0)
        {
            return (dateTime == null) ? null : (DateTime?)dateTimeHelper.ConvertToUtcTime(dateTime.Value, dateTimeHelper.DefaultTimeZone).AddDays(add);
        }

        /// <summary>
        /// 本地时间转UTC时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeHelper"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static DateTime LocalTimeConverUtcTime(this DateTime dateTime, IDateTimeHelper dateTimeHelper, int add = 0)
        {
            return dateTimeHelper.ConvertToUtcTime(dateTime, dateTimeHelper.DefaultTimeZone).AddDays(add);
        }

        /// <summary>
        /// UTC时间转本地时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeHelper"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static DateTime? UtcTimeConverLocalTime(this DateTime? dateTime, IDateTimeHelper dateTimeHelper, int add = 0)
        {
            return (dateTime == null) ? null : (DateTime?)dateTimeHelper.ConvertToUserTime(dateTime.Value, DateTimeKind.Utc).AddDays(add);
        }

        /// <summary>
        /// UTC时间转本地时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeHelper"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public static DateTime UtcTimeConverLocalTime(this DateTime dateTime, IDateTimeHelper dateTimeHelper, int add = 0)
        {
            return dateTimeHelper.ConvertToUserTime(dateTime, DateTimeKind.Utc).AddDays(add);
        }

        public static string ToString(this DateTime? datetime, string format = "")
        {
            if (!datetime.HasValue)
                return string.Empty;

            if (!string.IsNullOrEmpty(format))
                return datetime.Value.ToString(format);
            else
                return datetime.Value.ToString();
        }

        /// <summary>
        /// 时间转换 短日期
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToShortDateString(this DateTime? datetime)
        {
            if (!datetime.HasValue)
                return string.Empty;

            return datetime.Value.ToString("d");
        }

        /// <summary>
        /// 时间转换 短时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToShortTimeString(this DateTime? datetime)
        {
            if (!datetime.HasValue)
                return string.Empty;

            return datetime.Value.ToString("t");
        }


        /// <summary>
        /// DateTime时间格式转换为Unix(UTC)时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ConvertDateTimeStampLong(this System.DateTime time, bool milliseconds = true)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);

            if (milliseconds)
                return Convert.ToInt64((time - startTime).TotalMilliseconds);
            else
                return Convert.ToInt64((time - startTime).TotalSeconds);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ConvertDateTimeStampLong(this System.DateTime? time, bool milliseconds = true)
        {
            if (time.HasValue)
            {
                System.DateTime startTime = new System.DateTime(1970, 1, 1);

                if (milliseconds)
                    return Convert.ToInt64((time.Value - startTime).TotalMilliseconds);
                else
                    return Convert.ToInt64((time.Value - startTime).TotalSeconds);
            }
            else
                return 0;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix(UTC)时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <param name="milliseconds">使用毫秒</param>
        /// <returns>Unix时间戳格式</returns>
        public static string ConvertDateTimeStamp(this System.DateTime time, bool milliseconds = true)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);

            if (milliseconds)
                return Convert.ToInt64((time - startTime).TotalMilliseconds).ToString();
            else
                return Convert.ToInt64((time - startTime).TotalSeconds).ToString();
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static string ConvertDateTimeStamp(this System.DateTime? time, bool milliseconds = true)
        {
            if (time.HasValue)
            {
                System.DateTime startTime = new System.DateTime(1970, 1, 1);

                if (milliseconds)
                    return Convert.ToInt64((time.Value - startTime).TotalMilliseconds).ToString();
                else
                    return Convert.ToInt64((time.Value - startTime).TotalSeconds).ToString();
            }
            else
                return null;
        }

        /// <summary>
        /// 获取当前日期的23:59:59
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime GetEndTimeOfDate(this System.DateTime time)
        {
            DateTime? day = new DateTime(time.Year, time.Month, time.Day);
            double dayMis = (60 * 60 * 24);//一天的毫秒-1

            var newday = day.Value.AddSeconds(dayMis - 1);

            return newday;
        }

        /// <summary>
        /// 获取当前日期的23:59:59
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? GetEndTimeOfDate(this System.DateTime? time)
        {
            if (time.HasValue)
            {
                DateTime? day = new DateTime(time.Value.Year, time.Value.Month, time.Value.Day);
                double dayMis = (60 * 60 * 24);//一天的毫秒-1

                var newday = day.Value.AddSeconds(dayMis - 1);

                return newday;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取当前日期的0:0:0
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime GetStartTimeOfDate(this System.DateTime time)
        {
            DateTime day = new DateTime(time.Year, time.Month, time.Day);

            return day;
        }

        /// <summary>
        /// 获取当前日期的0:0:0
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? GetStartTimeOfDate(this System.DateTime? time)
        {
            if (time.HasValue)
            {
                DateTime? day = new DateTime(time.Value.Year, time.Value.Month, time.Value.Day);

                var newday = day.Value;

                return newday;
            }
            else
                return null;
        }



        /// <summary>
        /// 转化PST时间为GMT（也就是UTC时间）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime PSTConvertToGMT(this DateTime dateTime)
        {
            TimeZoneInfo timeZoneSource = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            TimeZoneInfo timeZoneDestination = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            return TimeZoneInfo.ConvertTime(dateTime, timeZoneSource, timeZoneDestination);
        }



        public static bool EqualDay(this DateTime dateTime, DateTime compare)
        {
            if (dateTime.Year == compare.Year && dateTime.DayOfYear == compare.DayOfYear)
                return true;

            return false;
        }

        /// <summary>
        /// 判断两个时间是否在同一周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool IsInSameWeek(this DateTime dateTime, DateTime compare)
        {
            TimeSpan ts = dateTime - compare;
            double dbl = ts.TotalDays;
            int intDow = Convert.ToInt32(dateTime.DayOfWeek);
            if (intDow == 0)
                intDow = 7;

            if (dbl >= 7 || dbl >= intDow)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 返回时间特定格式
        /// </summary>
        /// <returns></returns>
        public static string DateTimeString(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 返回日期特定格式
        /// </summary>
        /// <returns></returns>
        public static string DateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }


        /// <summary>
        /// 返回时间特定格式
        /// </summary>
        /// <returns></returns>
        public static string DateTimeString(this DateTime? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
        }

        /// <summary>
        /// 返回日期特定格式
        /// </summary>
        /// <returns></returns>
        public static string DateString(this DateTime? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-dd") ?? string.Empty;
        }

        /// <summary>
        /// 返回日期几
        /// </summary>
        /// <returns></returns>
        public static string DayOfWeek(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek.ToString("D"))
            {
                case "1":
                    return "星期一 ";
                case "2":
                    return "星期二 ";
                case "3":
                    return "星期三 ";
                case "4":
                    return "星期四 ";
                case "5":
                    return "星期五 ";
                case "6":
                    return "星期六 ";
                case "0":
                default:
                    return "星期日 ";
            }
        }
    }
}