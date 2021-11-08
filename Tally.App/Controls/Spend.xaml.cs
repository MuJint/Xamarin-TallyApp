using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        SSViewModel sSView = null;
        public Spend()
        {
            InitializeComponent();
            BindingContext = sSView = new SSViewModel(Navigation);
            sSView.SpendImgs = new ObservableCollection<SpendImg>(Load());
        }

        #region Func
        private List<SpendImg> Load()
        {
            var list = new List<SpendImg>()
            {
                new SpendImg()
            {
                Icon = "shop.png",
                Title = "购物"
            },
                new SpendImg()
            {
                Icon = "restaurant.png",
                Title = "餐饮"
            },
                new SpendImg()
            {
                Icon = "bus.png",
                Title = "交通"
            },new SpendImg()
            {
                Icon = "transfer.png",
                Title = "转账"
            }
            };
            return list;
        }
        #endregion

        #region Event

        #endregion

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

        }
    }
}