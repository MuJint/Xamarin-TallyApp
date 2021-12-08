using Passingwind.UserDialogs;
using System;
using Tally.Framework.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bug : ContentPage
    {
        readonly IErrorLogServices _errorLogServices = DependencyService.Get<IErrorLogServices>();
        public Bug()
        {
            InitializeComponent();
            LoadErrorLogs();
        }
        #region Method

        private void LoadErrorLogs()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 50), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var result = _errorLogServices.Query(t => t.Id > 0);
                    errorLogCollection.ItemsSource = result;
                });
                return false;
            });
        }
        #endregion

        #region Event Handler

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            UserDialogs.Instance.Toast("已成功复制该条错误信息");
        }
        #endregion
    }
}