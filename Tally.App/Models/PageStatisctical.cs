using System;
using Tally.App.ViewModel;

namespace Tally.App.Models
{
    /// <summary>
    /// 首页统计
    /// </summary>
    public class PageStatisctical : NotifyPropertyChanged
    {
        public int Year => DateTime.Now.Year;
        public int Month => DateTime.Now.Month;
        private decimal? _inCome = 0;
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal? InCome
        {
            get => _inCome;
            set => SetProperty(ref _inCome, value);
        }
        private decimal? _spend = 0;
        /// <summary>
        /// 总支出
        /// </summary>
        public decimal? Spend
        {
            get => _spend;
            set => SetProperty(ref _spend, value);
        }
    }
}
