using Tally.App.ViewModel;

namespace Tally.App.Models
{
    public class EventType : NotifyPropertyChanged
    {
        public string name { get; set; }
        public string image { get; set; }
        public bool selected { get; set; }

        private string _backgroundColor;
        public string backgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        private string _textColor;
        public string textColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }
    }
}
