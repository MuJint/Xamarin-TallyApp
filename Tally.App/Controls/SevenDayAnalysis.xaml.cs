using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SevenDayAnalysis : ContentPage
    {
        readonly SSViewModel sSViewModel = null;
        public SevenDayAnalysis(SSViewModel sSView)
        {
            InitializeComponent();
            sSViewModel = sSView;
            BindingContext = this;
            Init();
        }


        #region Method

        public class ChartDataModel
        {
            public string Name { get; set; }


            public double Value { get; set; }



            public ChartDataModel(string name, double value)
            {
                Name = name;
                Value = value;
            }
        }
        public ObservableCollection<ChartDataModel> LineData1 { get; set; }

        public ObservableCollection<ChartDataModel> LineData2 { get; set; }
        private void Init()
        {
            var result = sSViewModel.LoadSevenDayList();
            var groupResult = result.GroupBy(g => g.DateTime.Day);
            var entries = new List<ChartEntry>();
            var entries2 = new List<ChartEntry>();
            var u = 14;
            foreach (var data in groupResult)
            {
                var total = (float)data.Where(w => w.IsSpend == EnumSpend.Spend).Sum(d => d.Rmb);
                entries.Add(new ChartEntry(total)
                {
                    Label =$"{u}", //$"{new DateTime(DateTime.Now.Year, DateTime.Now.Month, data.Key).DateToMonthAndDay()}",
                    ValueLabel = $"{total}",
                    Color = SKColor.Parse("#2c3e50")
                    //77d065 b455b6 3498db
                });
                u++;
            }
            u = 14;
            for (int i = 0; i < 7; i++)
            {
                entries2.Add(new ChartEntry(20+i)
                {
                    Label = $"{u}",
                    ValueLabel = $"{20}",
                    Color = SKColor.Parse("#2c3e50")
                    //77d065 b455b6 3498db
                });
                u++;
            }
            var chart = new LineChart()
            {
                Entries = entries
            };
            //chart1.Chart = chart;

            LineData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Sun", 21),
                new ChartDataModel("Mon", 24),
                new ChartDataModel("Tue", 36),
                new ChartDataModel("Wed", 38),
                new ChartDataModel("Thu", 54),
                new ChartDataModel("Fri", 57),
                new ChartDataModel("Sat", 70)
            };

            LineData2 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Sun", 28),
                new ChartDataModel("Mon", 44),
                new ChartDataModel("Tue", 48),
                new ChartDataModel("Wed", 50),
                new ChartDataModel("Thu", 66),
                new ChartDataModel("Fri", 78),
                new ChartDataModel("Sat", 84)
            };
        }
        #endregion
    }
}