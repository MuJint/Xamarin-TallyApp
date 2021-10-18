using System;

namespace Tally.App.Models
{
    /// <summary>
    /// 首页统计
    /// </summary>
    public class PageStatisctical
    {
        public int Year => DateTime.Now.Year;
        public int Month => DateTime.Now.Month;
        /// <summary>
        /// 总收入
        /// </summary>
        public double? InCome { get; set; } = 0;
        /// <summary>
        /// 总支出
        /// </summary>
        public double? Spend { get; set; } = 0;
    }
}
