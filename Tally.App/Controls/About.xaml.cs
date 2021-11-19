using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }

        #region Event

        /// <summary>
        /// btn点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            switch (btn.TabIndex)
            {
                case 2:
                    Browser.OpenAsync("https://admin.qidian.qq.com/template/blue/website/agreement.html", new BrowserLaunchOptions
                    {
                        LaunchMode = BrowserLaunchMode.SystemPreferred,
                        TitleMode = BrowserTitleMode.Show,
                        PreferredToolbarColor = Color.AliceBlue,
                        PreferredControlColor = Color.Violet
                    });
                    break;
                case 3:
                    Browser.OpenAsync("https://admin.qidian.qq.com/template/blue/website/agreement.html", new BrowserLaunchOptions
                    {
                        LaunchMode = BrowserLaunchMode.SystemPreferred,
                        TitleMode = BrowserTitleMode.Show,
                        PreferredToolbarColor = Color.AliceBlue,
                        PreferredControlColor = Color.Violet
                    });
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}