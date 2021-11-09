using System;
using System.Linq;
using Tally.App.Models;
using Tally.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spend : ContentView
    {
        readonly SSViewModel sSViewModel = null;
        public Spend(SSViewModel sSView = null)
        {
            InitializeComponent();
            sSViewModel = sSView;
        }

        #region Method

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
        #endregion
    }
}