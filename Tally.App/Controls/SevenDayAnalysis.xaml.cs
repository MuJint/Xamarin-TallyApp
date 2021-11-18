using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SevenDayAnalysis : ContentView
    {
        public SevenDayAnalysis()
        {
            InitializeComponent();
            //Init();
        }

        private void Init()
        {
            var entries = new[]
             {
                 new ChartEntry(212)
                 {
                     Label = "UWP",
                     ValueLabel = "212",
                     Color = SKColor.Parse("#2c3e50")
                 },
                 new ChartEntry(248)
                 {
                     Label = "Android",
                     ValueLabel = "248",
                     Color = SKColor.Parse("#77d065")
                 },
                 new ChartEntry(128)
                 {
                     Label = "iOS",
                     ValueLabel = "128",
                     Color = SKColor.Parse("#b455b6")
                 },
                 new ChartEntry(514)
                 {
                     Label = "Shared",
                     ValueLabel = "514",
                     Color = SKColor.Parse("#3498db")
                 }
            };
            var chart = new LineChart()
            {
                Entries = entries
            };
            //chart1.Chart = chart;
        }
    }
}