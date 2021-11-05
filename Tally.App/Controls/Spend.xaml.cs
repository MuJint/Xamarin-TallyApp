using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spend : ContentView
    {
        public Spend()
        {
            InitializeComponent();
        }

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
    }
}