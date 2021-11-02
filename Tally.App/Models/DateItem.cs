using System;
using Tally.App.ViewModel;

namespace Tally.App.Models
{
    public class DateItem : BaseViewModel
    {
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Day { get; set; } = DateTime.Now.Day.ToString();
        public string DayWeek { get; set; }
        public bool Selected { get; set; }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        private string _textColor;
        public string TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }
    }
}
