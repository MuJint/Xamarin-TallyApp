using Microcharts;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using Passingwind.UserDialogs;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Tally.Framework.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        readonly ISpendLogServices _instance = DependencyService.Get<ISpendLogServices>();
        readonly List<string> color = new List<string>() { "#2c3e50", "#77d065", "#b455b6", "#3498db" };
        readonly HistoryViewModel viewModel = null;
        private List<SpendLog> queryList = new List<SpendLog>();
        public History()
        {
            InitializeComponent();
            BindingContext = viewModel = new HistoryViewModel();
            GlobalConfigExtensions.RestoreHistory = StructSpend;
        }

        #region Identity
        private class ExportDto
        {
            [ExcelColumnName("guid")]
            public string Guid { get; set; }
            [ExcelFormat("yyyy-MM-dd HH:mm:ss")]
            [ExcelColumnName("时间")]
            public DateTime DateTime { get; set; }
            [ExcelColumnName("金额")]
            public decimal? Rmb { get; set; }
            [ExcelColumnName("支出/收入")]
            public string IsSpend { get; set; }
            [ExcelColumnName("icon")]
            public string Icon { get; set; }
            [ExcelColumnName("备注")]
            public string Descrpition { get; set; }
        }
        #endregion

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
                UserDialogs.Instance.Toast("结束时间大于当前时间");
                return;
            }
            StructSpend(EnumSpend.Spend);
        }

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Export_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (queryList?.Count <= 0)
                {
                    UserDialogs.Instance.Toast("请先查询");
                    return;
                }

                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"tally.xlsx");
                //var rowsData = MiniExcel.Query<SpendLog>(path);
                //删除文件
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                var data = queryList
                    .Select(s => new ExportDto()
                    {
                        DateTime = s.DateTime,
                        Descrpition = s.Descrpition,
                        Guid = s.Id.ToString(),
                        Icon = s.Icon.ToString(),
                        IsSpend = s.IsSpend == EnumSpend.Spend ? "支出" : "收入",
                        Rmb = s.Rmb
                    });
                //运行时请求权限
                GlobalConfigExtensions.ApplyPermissions();
                //导出excel
                await MiniExcel.SaveAsAsync(path, data, excelType: ExcelType.XLSX);
                //唤起其它应用
                GlobalConfigExtensions.OpenFile(path);
            }
            catch (Exception c)
            {
                //
            }
        }

        /// <summary>
        /// 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SwitchDetail_Clicked(object sender, EventArgs e)
        {
            if (queryList?.Count <= 0)
            {
                UserDialogs.Instance.Toast("请先查询");
                return;
            }
            await Navigation.PushAsync(new HistoryDetail(queryList));
        }

        /// <summary>
        /// 支出收入按钮切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Switch_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Text)
            {
                case "支出":
                    StructSpend(EnumSpend.Spend);
                    break;
                case "收入":
                    StructSpend(EnumSpend.Income);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// 构造页面数据
        /// </summary>
        /// <param name="enumSpend"></param>
        private void StructSpend(EnumSpend enumSpend)
        {
            //先行更改按钮颜色
            ControlBtnColor(enumSpend);
            //查询
            queryList = _instance
                .Query(t => t.Id > 0 && t.DateTime >= StartDate.Date && t.DateTime <= EndDate.Date.GetEndOfDay())
                .Where(w => w.IsSpend == enumSpend)
                .OrderBy(t => t.DateTime).ToList();
            if (queryList?.Count <= 0)
            {
                UserDialogs.Instance.Toast("没有记录");
                viewModel.History = null;
                return;
            }
            var barChartSource = new List<ChartEntry>();
            var donutChartSource = new List<ChartEntry>();
            foreach (var entry in queryList.Skip(0).Take(15))
            {
                var total = (float)entry.Rmb.Value;
                barChartSource.Add(new ChartEntry(total)
                {
                    Label = $"{entry.DateTime:MM-dd}",
                    ValueLabel = $"{total}",
                    Color = SKColor.Parse(color[new Random().Next(0, 3)]),
                });
            }

            var donutSource = queryList
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

        /// <summary>
        /// 设置按钮背景以及字体颜色
        /// </summary>
        /// <param name="enumSpend"></param>
        private void ControlBtnColor(EnumSpend enumSpend)
        {
            switch (enumSpend)
            {
                case EnumSpend.Spend:
                    btnSpned.BackgroundColor = Color.FromHex("#24C389");
                    btnSpned.TextColor = Color.White;

                    btnInCome.BackgroundColor = Color.White;
                    btnInCome.TextColor = Color.FromHex("#24C389");
                    break;
                case EnumSpend.Income:
                    btnInCome.BackgroundColor = Color.FromHex("#24C389");
                    btnInCome.TextColor = Color.White;

                    btnSpned.BackgroundColor = Color.White;
                    btnSpned.TextColor = Color.FromHex("#24C389");
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}