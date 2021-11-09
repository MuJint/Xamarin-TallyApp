using Tally.App.ViewModel;

namespace Tally.App.Models
{
    public class CostInfo : NotifyPropertyChanged
    {
        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        private double _cost;
        public double Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }
        private string _title;
        public new string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
