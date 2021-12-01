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

        /// <summary>
        /// 日期转xxx前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatDate(this DateTime dt)
        {
            var byTime = new long[] { 24 * 60 * 60 * 365, 24 * 60 * 60, 60 * 60, 60, 1 };
            var unit = new string[] { "年", "天", "小时", "分钟", "秒" };

            var ct = (DateTime.Now - dt).TotalSeconds;
            if (ct < 0)
            {
                return "";
            }

            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < byTime.Length; i++)
            {
                if (ct < byTime[i])
                {
                    continue;
                }
                var temp = Math.Floor(ct / byTime[i]);
                ct %= byTime[i];
                if (temp > 0)
                {
                    sb.Append(temp + unit[i]);
                }


                /*一下控制最多输出几个时间单位：
                    一个时间单位如：N分钟前
                    两个时间单位如：M分钟N秒前
                    三个时间单位如：M年N分钟X秒前
                以此类推
                */
                if (sb.Length >= 1)
                {
                    break;
                }
            }
            return sb.ToString() + "前";
        }
    }
}
