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
        readonly ISpendLogServices _instance = DependencyService.Get<ISpendLogServices>();
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
                    SetFrame(framSpend);
                }
                else if (sender is Frame framInCome && framInCome.StyleId.Equals("InCome"))
                {
                    SetFrame(framInCome, false);
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
        private async void Button_Clicked(object sender, EventArgs e)
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
                    if (GlobalConfigExtensions.IsPermission is false)
                    {
                        var isPermission = await EssentialsExtensions.ApplyPermission();
                        if (isPermission is false)
                            return; //中断操作
                    }
                    //花费为0不提交
                    if (sSViewModel.CostInfo.Cost.ObjToDecimal() <= 0)
                        return;
                    var enumList = Enum.GetValues(typeof(EnumIcon)).OfType<EnumIcon>().ToList();
                    var model = new SpendLog()
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        Descrpition = Remarks?.Text ?? "",
                        Icon = enumList.FirstOrDefault(f => f.ToString().ToLower() == sSViewModel.CostInfo.Icon),
                        IsSpend = _enumSpend,
                        Pay = EnumPay.Alipay,
                        Rmb = sSViewModel.CostInfo.Cost.ObjToDecimal()
                    };
                    _instance.Insert(model);
                    //重置备注
                    Remarks.Text = "";
                    //重置frame
                    SetFrame(frameSpend);
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

        /// <summary>
        /// 重置选中frame
        /// </summary>
        /// <param name="isSpend"></param>
        private void SetSpendImgs(bool isSpend = true)
        {
            if (isSpend)
            {
                sSViewModel.LoadSpendImgs();
                //
                sSViewModel.CostInfo.Cost = "0";
                sSViewModel.CostInfo.Icon = "food";
                sSViewModel.CostInfo.Title = "餐饮";
            }
            else
            {
                sSViewModel.LoadInComeImgs();
                //
                sSViewModel.CostInfo.Cost = "0";
                sSViewModel.CostInfo.Icon = "wage";
                sSViewModel.CostInfo.Title = "工资";
            }
        }

        //设置frame
        private void SetFrame(Frame frame, bool isSpend = true)
        {
            if (isSpend)
            {
                labelSpend.TextColor = Color.White;
                frame.BackgroundColor = Color.FromHex("#3388df");
                //淡入淡出
                frame.Opacity = 0;
                frame.FadeTo(1, 1000, Easing.BounceOut);

                labelInCome.TextColor = Color.FromHex("#3388df");
                frameInCome.BackgroundColor = Color.White;
                _enumSpend = EnumSpend.Spend;
            }
            else if (isSpend is false)
            {
                labelInCome.TextColor = Color.White;
                frame.BackgroundColor = Color.FromHex("#3388df");
                //淡入淡出
                frame.Opacity = 0;
                frame.FadeTo(1, 1000, Easing.BounceOut);

                labelSpend.TextColor = Color.FromHex("#3388df");
                frameSpend.BackgroundColor = Color.White;
                _enumSpend = EnumSpend.Income;
            }
            SetSpendImgs(isSpend);
        }
        #endregion
    }
}