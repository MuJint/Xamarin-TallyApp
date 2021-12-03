using System;
using Tally.App.Helpers;
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
            var versionInfo = EssentialsExtensions.GetAppInfo();
            versionLabel.Text = $"{versionInfo.Version}({versionInfo.Build})";
        }

        #region Event

        /// <summary>
        /// btn点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            switch (btn.TabIndex)
            {
                case 2:
                    await Browser.OpenAsync("http://www.qiubb.com/user-agreement", new BrowserLaunchOptions
                    {
                        LaunchMode = BrowserLaunchMode.SystemPreferred,
                        TitleMode = BrowserTitleMode.Show,
                        PreferredToolbarColor = Color.AliceBlue,
                        PreferredControlColor = Color.Violet
                    });
                    break;
                case 3:
                    await Browser.OpenAsync("http://www.qiubb.com/privacy-policy", new BrowserLaunchOptions
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

        /// <summary>
        /// Github点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var frame = sender as Frame;
            if (frame != null)
            {
                switch (frame.TabIndex)
                {
                    case 1:
                        await EssentialsExtensions.SendEmail("yuxin.bb@qq.com", "", null);
                        break;
                    case 2:
                        await Browser.OpenAsync("https://github.com/MuJint/Xamarin-TallyApp", new BrowserLaunchOptions
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
        }
        #endregion
    }
}