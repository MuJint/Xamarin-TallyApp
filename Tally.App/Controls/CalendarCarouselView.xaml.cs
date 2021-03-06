using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Tally.App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarCarouselView : ContentView
    {
        public ObservableCollection<DateItem> ItemsSource { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty
           .Create(
               propertyName: "ItemsSource",
               returnType: typeof(ObservableCollection<DateItem>),
               declaringType: typeof(CalendarCarouselView),
               defaultValue: null,
               defaultBindingMode: BindingMode.OneWay,
               propertyChanged: ItemsSourcePropertyChanged);

        public CalendarCarouselView()
        {
            InitializeComponent();
        }

        private static async void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var items = newValue as ObservableCollection<DateItem>;
            var control = (CalendarCarouselView)bindable;

            var index = items?.ToList()?.FindIndex(p => p.Selected) ?? -1;

            await Task.Delay(100);

            if (index > -1)
                control.listDates.ScrollTo(index, -1, ScrollToPosition.MakeVisible, true);
        }
    }
}