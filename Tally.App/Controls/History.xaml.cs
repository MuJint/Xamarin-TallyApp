using Microcharts;
using Passingwind.UserDialogs;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tally.App.Helpers;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        //
        readonly ISpendLogServices _instance = DependencyService.Get<ISpendLogServices>();
        readonly List<string> color = new List<string>() { "#2c3e50", "#77d065", "#b455b6", "#3498db" };
        readonly HistoryViewModel viewModel = null;
        public History()
        {
            InitializeComponent();
            BindingContext = viewModel = new HistoryViewModel();
        }



        #region Event Handler
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (EndDate.Date > DateTime.Now)
            {
                UserDialogs.Instance.Alert("结束时间大于当前时间");
                return;
            }
            var data = _instance.Query(t => t.Id != null).OrderByDescending(t => t.DateTime).ToList();
            var barChartSource = new List<ChartEntry>();
            var donutChartSource = new List<ChartEntry>();
            foreach (var entry in data.Skip(0).Take(15))
            {
                var total = (float)entry.Rmb.Value;
                barChartSource.Add(new ChartEntry(total)
                {
                    Label = $"{entry.DateTime:MM-dd}",
                    ValueLabel = $"{total}",
                    Color = SKColor.Parse(color[new Random().Next(0, 3)]),
                });
            }

            var donutSource = data.Where(w => w.IsSpend == EnumSpend.Spend)
                .GroupBy(g => g.Icon)
                .ToList();
            foreach (var donut in donutSource)
            {
                var total = (float)donut.Sum(s => s.Rmb);
                donutChartSource.Add(new ChartEntry(total)
                {
                    Label = $"{donut.Key}",
                    ValueLabel = $"{total}",
                    Color = SKColor.Parse(color[new Random().Next(0, 3)]),
                });
            }

            //
            viewModel.History = new Models.History()
            {
                BarChartSource = new BarChart()
                {
                    Entries = barChartSource,
                    AnimationDuration = new TimeSpan(0, 0, 3),
                    IsAnimated = true,
                    LabelOrientation = Orientation.Default,
                    ValueLabelOrientation = Orientation.Default,
                    LabelTextSize = 20,
                },
                DonutChartSource = new DonutChart()
                {
                    Entries = donutChartSource,
                    AnimationDuration = new TimeSpan(0, 0, 3),
                    IsAnimated = true,
                }
            };
        }
        #endregion


    }
}