using System;
using System.Collections.Generic;
using System.Text;

namespace Tally.App.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 时间转换为周几
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateToWeekOnDay(this DateTime dateTime)
        {
            var result = string.Empty;
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    result = "周五";
                    break;
                case DayOfWeek.Monday:
                    result = "周一";
                    break;
                case DayOfWeek.Saturday:
                    result = "周六";
                    break;
                case DayOfWeek.Sunday:
                    result = "周日";
                    break;
                case DayOfWeek.Thursday:
                    result = "周四";
                    break;
                case DayOfWeek.Tuesday:
                    result = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    result = "周三";
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 时间转换为 XX月xx日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateToMonthAndDay(this DateTime dateTime)
        {
            return $"{dateTime:MM月dd日}";
        }

        /// <summary>
        /// 获取月初凌晨零分
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetStartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// 获取月末23:59:59
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetEndOfMonth(this DateTime dateTime)
        {
            var end = new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
            return new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
        }
    }
}
