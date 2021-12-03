using Microcharts;
using Passingwind.UserDialogs;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Tally.Framework.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SevenDayAnalysis : ContentPage
    {
        readonly SSViewModel sSViewModel = null;
        readonly string InComeColor = $"#24C389";
        readonly string SpendColor = $"#3A3A62";
        public SevenDayAnalysis(SSViewModel sSView)
        {
            InitializeComponent();
            sSViewModel = sSView;
            BindingContext = this;
            Init();
        }

        #region Property
        public List<ExpenseRecord> ExpenseRecords { get; set; }
        #endregion

        #region Method
        private void Init()
        {
            var (chart, result) = GetChart();
            if(result.GroupBy(g=>g.DateTime.Day).Count()<=1)
            {
                UserDialogs.Instance.Toast("至少需要两天的数据");
                return;
            }
            //chart
            chartView.Chart = chart;
            //CollectionView
            SetCollectionView(result);
        }

        /// <summary>
        /// 获取曲线图数据
        /// </summary>
        /// <param name="enumSpend"></param>
        private (LineChart, List<SpendLog>) GetChart(EnumSpend enumSpend = EnumSpend.Spend)
        {
            var result = sSViewModel.LoadSevenDayList();
            var color = new List<string>() { "#2c3e50", "#77d065", "#b455b6", "#3498db" };
            var groupResult = result.GroupBy(g => g.DateTime.Day);
            var entries = new List<ChartEntry>();
            foreach (var data in groupResult)
            {
                var total = (float)data.Where(w => w.IsSpend == enumSpend).Sum(d => d.Rmb);
                //不能为O，Microcharts会报空异常
                total = total <= 0 ? 0.1f : total;
                entries.Add(new ChartEntry(total)
                {
                    Label = $"{data.Key}",
                    ValueLabel = $"{total}",
                    Color = SKColor.Parse(color[new Random().Next(0, 3)]),
                });
            }
            return (new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Spline,
                AnimationDuration = new TimeSpan(0, 0, 3),
                IsAnimated = true,
                LabelOrientation = Orientation.Horizontal,
                LabelTextSize = 18,
                ValueLabelOrientation = Orientation.Horizontal,
                EnableYFadeOutGradient = true,
            }, result);
        }

        /// <summary>
        /// SetCollectionView
        /// </summary>
        /// <param name="spendLogs"></param>
        /// <param name="enumSpend"></param>
        private void SetCollectionView(List<SpendLog> spendLogs, EnumSpend enumSpend = EnumSpend.Spend)
        {
            listSource.ItemsSource = spendLogs?.OrderByDescending(o => o.DateTime)
                ?.Where(w => w.IsSpend == enumSpend)
                ?.Select(s => new ExpenseRecord()
                {
                    Icon = s.Icon.ToString(),
                    IconTitle = s.Icon.GetDescription(),
                    Description = s.Descrpition,
                    Rmb = s.Rmb.Value,
                    IsSpend = s.IsSpend == EnumSpend.Income ? "+" : "-",
                    TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor,
                    Time = $"{s.DateTime:MM-dd HH:mm}"
                });
        }
        #endregion

        #region Event
        /// <summary>
        /// btn点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Text)
            {
                case "收入":
                    InComeBtn.BackgroundColor = Color.FromHex("#3388df");
                    InComeBtn.TextColor = Color.White;

                    SpendBtn.BackgroundColor = Color.White;
                    SpendBtn.TextColor = Color.FromHex("#181819");
                    var (chartIn, resultIn) = GetChart(EnumSpend.Income);
                    chartView.Chart = chartIn;
                    SetCollectionView(resultIn, EnumSpend.Income);
                    break;
                case "支出":
                    SpendBtn.BackgroundColor = Color.FromHex("#3388df");
                    SpendBtn.TextColor = Color.White;

                    InComeBtn.BackgroundColor = Color.White;
                    InComeBtn.TextColor = Color.FromHex("#181819");
                    var (chart, result) = GetChart();
                    chartView.Chart = chart;
                    SetCollectionView(result);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}