using System;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Tally.Framework.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spend : ContentView
    {
        readonly SSViewModel sSViewModel = null;
        //
        readonly ISpendLogServices _instance = Dependcy.Provider.GetService<ISpendLogServices>();
        //私有状态 当前选中是收入还是支出
        private EnumSpend _enumSpend = EnumSpend.Spend;
        public Spend(SSViewModel sSView = null)
        {
            InitializeComponent();
            sSViewModel = sSView;
        }

        #region Method

        /// <summary>
        /// 校验末位
        /// </summary>
        private void CheckBottom()
        {
            sSViewModel.CostInfo.Cost = sSViewModel.CostInfo.Cost == "0" ? "" : sSViewModel.CostInfo.Cost;
        }
        #endregion

        #region Event
        /// <summary>
        /// fram切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (sender is Frame framSpend && framSpend.StyleId.Equals("Spend"))
                {
                    labelSpend.TextColor = Color.White;
                    framSpend.BackgroundColor = Color.FromHex("#3388df");
                    //淡入淡出
                    framSpend.Opacity = 0;
                    framSpend.FadeTo(1, 1000, Easing.BounceOut);

                    labelInCome.TextColor = Color.FromHex("#3388df");
                    frameInCome.BackgroundColor = Color.White;
                    _enumSpend = EnumSpend.Spend;
                }
                else if (sender is Frame framInCome && framInCome.StyleId.Equals("InCome"))
                {
                    labelInCome.TextColor = Color.White;
                    framInCome.BackgroundColor = Color.FromHex("#3388df");
                    //淡入淡出
                    framInCome.Opacity = 0;
                    framInCome.FadeTo(1, 1000, Easing.BounceOut);

                    labelSpend.TextColor = Color.FromHex("#3388df");
                    frameSpend.BackgroundColor = Color.White;
                    _enumSpend = EnumSpend.Income;
                }
            });
        }

        /// <summary>
        /// 选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectSpendType = e.CurrentSelection.FirstOrDefault() as SpendImg;
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                sSViewModel.SpendImgs.ToList().ForEach(s =>
                {
                    s.Opacity = 0.1;
                    s.Color = "#A1A2A9";
                });
                int index = sSViewModel.SpendImgs.ToList().
                    FindIndex(f => f == selectSpendType);
                if (index > -1)
                {
                    sSViewModel.SpendImgs[index].Opacity = 1;
                    sSViewModel.SpendImgs[index].Color = "#181819";
                    //
                    sSViewModel.CostInfo.Icon = selectSpendType.Icon;
                    sSViewModel.CostInfo.Title = selectSpendType.Title;
                }
            });
        }

        /// <summary>
        /// 数字点击以及分割号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            switch (button.Text)
            {
                case "退格":
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost.Substring(0, sSViewModel.CostInfo.Cost.Length - 1)}";
                    //校验一位时，重置为0
                    if (string.IsNullOrEmpty(sSViewModel.CostInfo.Cost))
                        sSViewModel.CostInfo.Cost = "0";
                    break;
                case "清零":
                    sSViewModel.CostInfo.Cost = "0";
                    break;
                case "OK":
                    var enumList = Enum.GetValues(typeof(EnumIcon)).OfType<EnumIcon>().ToList();
                    var model = new SpendLog()
                    {
                        DateTime = DateTime.Now,
                        Descrpition = "测试添加哦",
                        Icon = enumList.FirstOrDefault(f => f.ToString().ToLower() == sSViewModel.CostInfo.Icon),
                        Id = Guid.NewGuid(),
                        IsSpend = _enumSpend,
                        Pay = EnumPay.Alipay,
                        Rmb = sSViewModel.CostInfo.Cost.ObjToDecimal()
                    };
                    _instance.Insert(model);
                    //回到首页
                    sSViewModel.Restore();
                    break;
            }
            //长度超长
            if (sSViewModel.CostInfo.Cost.Length > 10)
                return;
            switch (button.Text)
            {
                case "1":
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}1";
                    break;
                case "2":
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}2"; 
                    break;
                case "3":
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}3"; 
                    break;
                case "4": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}4"; 
                    break;
                case "5": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}5"; 
                    break;
                case "6": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}6"; 
                    break;
                case "7": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}7"; 
                    break;
                case "8": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}8"; 
                    break;
                case "9": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}9"; 
                    break;
                case "0": 
                    CheckBottom();
                    sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}0"; 
                    break;
                case ".":
                    if (string.IsNullOrEmpty(sSViewModel.CostInfo.Cost))
                        sSViewModel.CostInfo.Cost = $"0.";
                    else
                        sSViewModel.CostInfo.Cost = $"{sSViewModel.CostInfo.Cost}.";
                    break;
            }
        }

        /// <summary>
        /// 关闭当前页签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseSpend_Clicked(object sender, EventArgs e)
        {
            sSViewModel.Restore();
        }
        #endregion
    }
}