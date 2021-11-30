using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;
using Tally.App.ViewModel;

namespace Tally.App.Models
{
    /// <summary>
    /// 历史记录
    /// </summary>
    public class History : NotifyPropertyChanged
    {
        private Chart _barChartSource;
        private Chart _donutChartSource;
        public Chart BarChartSource
        {
            get => _barChartSource;
            set => SetProperty(ref _barChartSource, value);
        }
        public Chart DonutChartSource
        {
            get => _donutChartSource;
            set => SetProperty(ref _donutChartSource, value);
        }
    }
}
