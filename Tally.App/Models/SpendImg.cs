using Tally.App.ViewModel;

namespace Tally.App.Models
{
    public class SpendImg : NotifyPropertyChanged
    {
        public string Icon { get; set; }
        public new string Title { get; set; }
        private double _opacity = 0.1;
        public double Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }
        private string _color = "#A1A2A9";
        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }
        
    }
}
